using System.Collections.Generic;
using System.Threading.Tasks;
using absHelper = TradeAbstractions.Helpers;
using absMdl = TradeAbstractions.Models;
using absHelpers = TradeAbstractions.Helpers;
using helpMe = TradeEngine.Models.HelpMe;
using absSettings = TradeAbstractions.Helpers.Settings;
using enumExchange = TradeAbstractions.Enums.EnumExchange;
using System;
using singletons = TradeAbstractions.Singletons;

namespace TradeEngine.Operations
{
    public class CreatePossiblePaths
    {
        /// <summary>
        /// Program başlangıcında bir kere çalışması yeterli. Arada bir çağırmakta da fayda var.
        /// Sık olmasa da belki borsa tarafında bazı pariteler eklenebilir/çıkarılabilir.
        /// </summary>
        /// <remarks>
        /// Şimdilik bu fonksiyon için rate limits hesabı yaptırmayacağız.
        /// BtcTurk dökümanlarında bu bilgi yok, destek hattı da ExchangeInfo kullanmayın dedi.
        /// Gerçi dökümanda ExchangeInfo olduğunu bile bilmeyen bir çalışan yanıt verdi :)
        /// </remarks>
        internal static async Task<absHelper.ReturnTask<absMdl.Engine.PossiblePathsModel>> Init(bool firstInit)
        {
            var retTask = new absHelper.ReturnTask<absMdl.Engine.PossiblePathsModel>
            {
                Data = new absMdl.Engine.PossiblePathsModel { ReqRespList = new List<absHelper.RequestResponseStats>() },
                Success = false
            };

            var engineSettings = new TradeEngine.Settings();
            // Tüm ayarlar json dosyalarından tekrar okunur
            engineSettings.SetAllSettings();

            Dictionary<string, List<absMdl.PossiblePathModel>> tempPossiblePathsDict = new();

            foreach (enumExchange exchangeEnum in Enum.GetValues<enumExchange>())
            {
                var exchange = Statics.TradeHelpers[exchangeEnum];

                // Enumeration tanımı olsa da create edilmemiş olabilir. Öyle bir durumda hata almamak için es geçelim.
                if (exchange == null) continue;

                var commissions = exchange.FromEngine.SetCommissions();
                if (commissions.Data.ReqResp != null) retTask.Data.ReqRespList.Add(commissions.Data.ReqResp);

                if (!commissions.Success)
                {
                    helpMe.TradeEvents.PrintMessageEvent(commissions.Message, true);
                    return retTask;
                }

                var rawPairs = await exchange.FromEngine.GetRawPairs();
                retTask.Data.ReqRespList.Add(rawPairs.Data.ReqResp);

                if (!rawPairs.Success)
                {
                    helpMe.TradeEvents.PrintMessageEvent(rawPairs.Message, true);
                    return retTask;
                }

                if (rawPairs.Data.PairList.Count == 0)
                {
                    helpMe.TradeEvents.PrintMessageEvent($"{exchangeEnum} için PossiblePaths listesi doldurulamadı. Veri yok!", true);
                    return retTask;
                }

                var pairList = rawPairs.Data.PairList;

                // Önce servisten gelen pairlist'teki pairlerden bizim istemediklerimizi çıkaralım. İstediklerimiz Engine.json içinde belirtilenlerdir.
                var exchangeCurrencies = absSettings.EngineSettings.Shared[exchangeEnum].Currencies;
                for (int i = pairList.Count - 1; i >= 0; i--)
                {
                    bool find = false;
                    var pair = pairList[i];
                    for (int j = 0; j < exchangeCurrencies.Count; j++)
                    {
                        string curr = exchangeCurrencies[j];
                        if (
                            (pair.Numerator == curr && (pair.Denominator == "TRY" || pair.Denominator == "USDT")) ||
                            (pair.Denominator == curr && (pair.Numerator == "TRY" || pair.Numerator == "USDT"))
                            )
                        {
                            find = true;
                            break;
                        }
                    }
                    // ayar dosyamızdaki currency listesinde yoksa çıkaralım
                    if (!find) pairList.RemoveAt(i);
                }

                for (int b = 0; b < absSettings.InternalSettings.BaseList.Count; b++)
                {
                    string baseCurrency = absSettings.InternalSettings.BaseList[b];

                    for (int i1 = 0; i1 < pairList.Count; i1++)
                    {
                        absMdl.PairRawModel pair1 = pairList[i1];

                        // data1 baseCurrency'i içerene kadar devam
                        if (pair1.Numerator != baseCurrency && pair1.Denominator != baseCurrency) continue;

                        // Standart olsun diye node create'inin içine yazmadım.
                        absMdl.PairModel newPair1 = new absMdl.PairModel
                        {
                            Base = baseCurrency,
                            CommissionRatio = pair1.CommissionRatio,
                            Denominator = pair1.Denominator,
                            DenominatorScale = pair1.DenominatorScale,
                            IsDenominator = pair1.Denominator == baseCurrency,
                            MinDenominatorValue = pair1.MinDenominatorValue,
                            MinNumeratorValue = pair1.MinNumeratorValue,
                            Numerator = pair1.Numerator,
                            NumeratorScale = pair1.NumeratorScale,
                            Pair = pair1.Pair
                        };
                        newPair1.NonBase = newPair1.IsDenominator ? newPair1.Numerator : newPair1.Denominator;

                        string pair2Base = newPair1.NonBase;
                        for (int i2 = 0; i2 < pairList.Count; i2++)
                        {
                            absMdl.PairRawModel pair2 = pairList[i2];

                            // data2 Base2'yi içerene kadar devam
                            if (pair2.Denominator != pair2Base && pair2.Numerator != pair2Base) continue;

                            // Ancak, bulunan paritede Base1 de olmamalı
                            if (pair2.Denominator == newPair1.Base || pair2.Numerator == newPair1.Base) continue;

                            absMdl.PairModel newPair2 = new absMdl.PairModel
                            {
                                Base = pair2Base,
                                CommissionRatio = pair2.CommissionRatio,
                                Denominator = pair2.Denominator,
                                DenominatorScale = pair2.DenominatorScale,
                                IsDenominator = pair2.Denominator == pair2Base,
                                MinDenominatorValue = pair2.MinDenominatorValue,
                                MinNumeratorValue = pair2.MinNumeratorValue,
                                Numerator = pair2.Numerator,
                                NumeratorScale = pair2.NumeratorScale,
                                Pair = pair2.Pair
                            };
                            newPair2.NonBase = newPair2.IsDenominator ? newPair2.Numerator : newPair2.Denominator;

                            absMdl.PossiblePathModel pathNode = new absMdl.PossiblePathModel
                            {
                                Exchange = exchangeEnum,
                                PairPaths = new List<absMdl.PairModel>(),
                                RatioFactor = (1 - newPair1.CommissionRatio) * (1 - newPair2.CommissionRatio)
                            };

                            pathNode.PairPaths.Add(newPair1);
                            pathNode.PairPaths.Add(newPair2);

                            // Sadece BTC eklenmiş olacak veya diğer Currencies cinsinden json dosyamızda belirttiklerimizden biri..
                            AddToPossiblePathsDictTemp(tempPossiblePathsDict, newPair1.NonBase, pathNode);

                            break;
                        }
                    }
                }
            }

            // 50ns, Başka yerden erişilmek istenirse bir anlığına kilitleyelim ki yeni değerlerin atanmış olduğuna emin olalım.
            lock (singletons.Me.PossiblePathsDict)
            {
                singletons.Me.PossiblePathsDict = tempPossiblePathsDict;
            }

            retTask.Success = true;

            return retTask;
        }
        static void AddToPossiblePathsDictTemp(Dictionary<string, List<absMdl.PossiblePathModel>> tempPossiblePathsDict, string pathString, absMdl.PossiblePathModel pathNode)
        {
            if (!tempPossiblePathsDict.ContainsKey(pathString)) tempPossiblePathsDict.Add(pathString, new List<absMdl.PossiblePathModel>());
            tempPossiblePathsDict[pathString].Add(pathNode);
        }
    }
}
