using System;
using System.Threading.Tasks;
using absHelper = TradeAbstractions.Helpers;
using absMdl = TradeAbstractions.Models;
using helpMe = TradeEngine.Models.HelpMe;
using absSettings = TradeAbstractions.Helpers.Settings;
using singletons = TradeAbstractions.Singletons;
using enumExchange = TradeAbstractions.Enums.EnumExchange;
using System.Collections.Generic;

namespace TradeEngine.Operations
{
    internal class CalculateRatios
    {
        /// <summary>
        /// Trade.Helper.HasTickerIncludesOpportunity = false ise kullanılacak.
        /// False ise hacim barındırmaz. Onun için hangi fırsatın en iyi olduğunu bilemeyiz.
        /// Bundan dolayı Ticker içindeki en iyi oranı tercih ederiz. (Örn: BtcTurk)
        /// Sonradan miktar için OrderBook gibi bir servise başvururuz (ona da 3 sorgu çekmek gerekiyor).
        /// Binance gibi bir borsada iki veri aynı anda ticker serviste sunulduğundan en iyi fırsatı hesaplayabiliriz.
        /// </summary>
        /// <returns>
        /// Data null olamaz!
        /// </returns>
        internal static async Task<absHelper.ReturnSimple> GetRatios()
        {
            var retTask = new absHelper.ReturnSimple { Success = false };

            var usdTry = await Operations.BankCurrency.GetUsdTry();
            if (usdTry == 0)
            {
                helpMe.TradeEvents.PrintMessageEvent("USD/TRY değeri 0 okundu! İşlemlere devam edilemez.", true);
                return retTask;
            }

            // Ask ve bid verileri elimizde olmadığından ikisini de okuduğumuz değerler ile set edelim.
            singletons.Me.Parities.UsdTry.Ask = usdTry;
            singletons.Me.Parities.UsdTry.Bid = usdTry;

            // Borsa'daki ticker veriler okunduktan sonra yine çağırılır. Ticker'lar okunurken hata alınırsa en azından banka paritesi gösterilmiş olsun.
            helpMe.TradeEvents.ParitiesUpdatedEvent();

            foreach (enumExchange exchangeEnum in Enum.GetValues<enumExchange>())
            {
                var exchange = Statics.TradeHelpers[exchangeEnum];

                // Enumeration tanımı olsa da create edilmemiş olabilir. Öyle bir durumda hata almamak için es geçelim.
                if (exchange == null) continue;

                var tickerTask = await Statics.TradeHelpers[exchangeEnum].FromEngine.GetTickerList();
                //retTask.Data.ReqResp = tickerTask.Data.ReqResp;

                if (!tickerTask.Success)
                {
                    helpMe.TradeEvents.PrintMessageEvent(tickerTask.Message, true);
                    return retTask;
                }

                absMdl.TradeHelper.FromEngine.TickerModel tickerModel = tickerTask.Data;

                singletons.Me.Parities.SetExchangeValues(exchangeEnum, tickerModel.AskLookup["USDTTRY"], tickerModel.BidLookup["USDTTRY"]);

                helpMe.TradeEvents.ParitiesUpdatedEvent();

                decimal ask1, ask2, bid1, bid2;

                // Olası borsa hatalarına karşı böyle bir try catch kullanıp hatayı loglayalım.
                try
                {
                    // Trade.Init() kısmında set edilen singletons.Me.PossiblePathsDict'i kullanırız.
                    foreach (KeyValuePair<string, List<absMdl.PossiblePathModel>> pathHashSet in singletons.Me.PossiblePathsDict)
                    {
                        foreach (absMdl.PossiblePathModel path in pathHashSet.Value)
                        {
                            // Ticker verisini aldığımız borsaya ait olan veriler dışındakileri es geçelim.
                            if (path.Exchange != exchangeEnum) continue;

                            try
                            {
                                ask1 = tickerModel.AskLookup[path.PairPaths[0].Pair];
                                ask2 = tickerModel.AskLookup[path.PairPaths[1].Pair];

                                bid1 = tickerModel.BidLookup[path.PairPaths[0].Pair];
                                bid2 = tickerModel.BidLookup[path.PairPaths[1].Pair];
                            }
                            catch
                            {
                                //helpMe.TradeEvents.PrintMessageEvent("GetBestRatioPossibility(), Warning(ask, bid bulunamadı): " + ex.ToString(), true);
                                // Dbs.AddDevelopmentLog("TickerList", "GetBestRatioPossibility(), Warning(ask, bid bulunamadı): " + ex.ToString(), tickerList);

                                retTask.Success = true;
                                return retTask;
                            }

                            if (ask1 == 0 || ask2 == 0 || bid1 == 0 || bid2 == 0)
                            {
                                helpMe.TradeEvents.PrintMessageEvent($"GetRatios() for {exchangeEnum}, Info(ask1={ask1}, ask2={ask2}, bid1={bid1}, bid2={bid2})");

                                Dbs.AddDevelopmentLog("TickerList", $"GetRatios() for {exchangeEnum}, Info(ask1={ask1}, ask2={ask2}, bid1={bid1}, bid2={bid2})", tickerTask);

                                retTask.Success = true;
                                return retTask;
                            }

                            path.PairPaths[0].Ask = ask1;
                            path.PairPaths[0].Bid = bid1;
                            path.PairPaths[1].Ask = ask2;
                            path.PairPaths[1].Bid = bid2;

                            // ilk paritenin base cinsinden serbest bakiyemizi alalım.
                            decimal firstBasedFreeBalance = 0;
                            for (int i = 0; i < exchange.UserBalance.BalanceList.Count; i++)
                            {
                                var balance = exchange.UserBalance.BalanceList[i];
                                if (balance.Asset == path.PairPaths[0].Base)
                                {
                                    firstBasedFreeBalance = balance.Free;
                                    break;
                                }
                            }

                            decimal commissionMultiplier1 = (1 - path.PairPaths[0].CommissionRatio);
                            path.PairPaths[0].DenominatorTotalGross = firstBasedFreeBalance;
                            path.PairPaths[0].DenominatorTotalNet = path.PairPaths[0].DenominatorTotalGross * commissionMultiplier1;
                            path.PairPaths[0].NumeratorAmountGross = path.PairPaths[0].DenominatorTotalGross / ask1;
                            path.PairPaths[0].NumeratorAmountNet = path.PairPaths[0].NumeratorAmountGross * commissionMultiplier1;

                            decimal commissionMultiplier2 = (1 - path.PairPaths[1].CommissionRatio);
                            path.PairPaths[1].NumeratorAmountGross = path.PairPaths[0].NumeratorAmountNet;
                            path.PairPaths[1].DenominatorTotalGross = path.PairPaths[1].NumeratorAmountGross * bid2;
                            path.PairPaths[1].DenominatorTotalNet = path.PairPaths[1].DenominatorTotalGross * commissionMultiplier2;
                            path.PairPaths[1].NumeratorAmountNet = path.PairPaths[1].NumeratorAmountGross * commissionMultiplier2;

                            // USDT > Crypto > TRY
                            if (path.PairPaths[0].Base == "USDT")
                            {
                                path.PairPaths[0].UsdOrUsdtTotal = path.PairPaths[0].DenominatorTotalNet;
                                path.PairPaths[1].UsdOrUsdtTotal = path.PairPaths[1].DenominatorTotalNet / singletons.Me.Parities.UsdTry.Ask;
                            }
                            // TRY > Crypto > USDT
                            else
                            {
                                path.PairPaths[0].UsdOrUsdtTotal = path.PairPaths[0].DenominatorTotalNet / singletons.Me.Parities.UsdTry.Ask;
                                path.PairPaths[1].UsdOrUsdtTotal = path.PairPaths[1].DenominatorTotalNet;
                            }

                            path.Ratio = path.PairPaths[1].UsdOrUsdtTotal / path.PairPaths[0].UsdOrUsdtTotal;
                            path.DiffRatio = path.Ratio - 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    helpMe.TradeEvents.PrintMessageEvent("GetRatios(), Exception: " + ex.ToString(), true);
                    Dbs.AddDevelopmentLog("TickerList", "GetRatios(), Exception: " + ex.ToString(), tickerTask);

                    return retTask;
                }
            }

            PossiblePathsDictToFlatList();
            helpMe.TradeEvents.RatiosCalculatedEvent();

            retTask.Success = true;

            return retTask;
        }
        /// <summary>
        /// DataGridView içerisinde göstermek için flat bir listeye çevirelim.
        /// </summary>
        static void PossiblePathsDictToFlatList()
        {
            singletons.Me.DataGridViewFlat.Clear();
            var flatList = singletons.Me.DataGridViewFlat;

            foreach (KeyValuePair<string, List<absMdl.PossiblePathModel>> pathHashSet in singletons.Me.PossiblePathsDict)
            {
                for (int i = 0; i < pathHashSet.Value.Count; i++)
                {
                    flatList.Add(new absMdl.DataGridViewFlatModel());
                }
            }

            int index = -1;
            foreach (KeyValuePair<string, List<absMdl.PossiblePathModel>> pathHashSet in singletons.Me.PossiblePathsDict)
            {
                for (int i = 0; i < pathHashSet.Value.Count; i++)
                {
                    index++;

                    absMdl.PossiblePathModel path = pathHashSet.Value[i];

                    var flat = flatList[index];

                    flat.Currency = pathHashSet.Key;
                    flat.Exchange = path.Exchange;

                    flat.Pair1 = path.PairPaths[0].Pair;
                    flat.Price1 = path.PairPaths[0].Ask;
                    flat.Amount1 = path.PairPaths[0].NumeratorAmountNet;
                    flat.Total1 = path.PairPaths[0].DenominatorTotalNet;

                    flat.Pair2 = path.PairPaths[1].Pair;
                    flat.Price2 = path.PairPaths[1].Bid;
                    flat.Amount2 = path.PairPaths[1].NumeratorAmountNet;
                    flat.Total2 = path.PairPaths[1].DenominatorTotalNet;

                    flat.ParityRatio = path.DiffRatio;
                    flat.UsdOrUsdtResult = path.PairPaths[1].UsdOrUsdtTotal;
                    flat.Profit = path.PairPaths[1].UsdOrUsdtTotal - path.PairPaths[0].UsdOrUsdtTotal;
                }
            }

            // scTEST
            //string a1 = Newtonsoft.Json.JsonConvert.SerializeObject(singletons.Me.PossiblePathsDict);
            //string a2 = Newtonsoft.Json.JsonConvert.SerializeObject(singletons.Me.DataGridViewFlat);
        }
    }
}
