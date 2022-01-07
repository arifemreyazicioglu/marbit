using System;
using System.Threading.Tasks;
using absHelper = TradeAbstractions.Helpers;
using absMdl = TradeAbstractions.Models;
using helpMe = TradeEngine.Models.HelpMe;
using System.Collections.Generic;
using mdlOrder = TradeAbstractions.Models.TradeHelper.FromEngine.OrderModel;

namespace TradeEngine.Operations
{
    internal class HandleOrders
    {
        /// <summary>
        /// Ana üçgen arbitrajın yapıldığı fonksiyondur.
        /// </summary>
        /// <returns> Data null olmaz en azından request bilgisi barındırır </returns>
        internal static async Task<absHelper.ReturnTask<absMdl.Engine.TriangleOrders>> MakeOrders(absMdl.Engine.Opportunity opportunity)
        {
            var retTask = new absHelper.ReturnTask<absMdl.Engine.TriangleOrders> { Success = false };

            absMdl.Engine.TriangleOrders triangleOrders = new absMdl.Engine.TriangleOrders
            {
                Base = opportunity.Possibility.PossiblePath.PairPaths[0].Base,
                MarketOrderRequests = new List<mdlOrder.OrderRequestModel>(),
                MarketOrderResponses = new List<mdlOrder.OrderResponseModel>()
            };
            retTask.Data = triangleOrders;

            absMdl.PairModel firstPair = opportunity.Possibility.PossiblePath.PairPaths[0];
            absMdl.PairModel secondPair = opportunity.Possibility.PossiblePath.PairPaths[1];
            absMdl.PairModel thirdPair = opportunity.Possibility.PossiblePath.PairPaths[2];
            absMdl.OrderPriceModel firstPrice = opportunity.FinalPrices[0];

            // First Order >>>
            mdlOrder.OrderRequestModel firstOrderReq = new mdlOrder.OrderRequestModel
            {
                PairSymbol = firstPair.Pair,
                Price = firstPrice.Price,
                CommissionRatio = firstPair.CommissionRatio,
                OrderClientId = Guid.NewGuid() + "_1",
                PriceScale = firstPair.DenominatorScale,
                NumeratorScale = firstPair.NumeratorScale,
                DenominatorScale = firstPair.DenominatorScale
            };
            if (firstPair.IsDenominator)
            {
                firstOrderReq.IsBuyOrder = true;
                firstOrderReq.DenominatorTotalGross = firstPrice.DenominatorTotal;
            }
            else
            {
                firstOrderReq.NumeratorAmountGross = firstPrice.NumeratorAmount;
            }
            triangleOrders.MarketOrderRequests.Add(firstOrderReq);

            //var firstOrderResp = await Statics.TradeHelper.FromEngine.CreateOrder(firstOrderReq);
            //if (!firstOrderResp.Success)
            //{
            //    helpMe.TradeEvents.PrintMessageEvent("1. Order: " + firstOrderResp.Message, true);
            //    return retTask;
            //}
            //triangleOrders.MarketOrderResponses.Add(firstOrderResp.Data);
            //// <<< First Order



            //// Second Order >>>
            //mdlOrder.OrderRequestModel secondOrderReq = SetOrderRequest(firstPair, opportunity.FinalPrices[1].Price, firstOrderResp.Data, secondPair, "2");
            //triangleOrders.MarketOrderRequests.Add(secondOrderReq);

            //var secondOrderResp = await Statics.TradeHelper.FromEngine.CreateOrder(secondOrderReq);
            //if (!secondOrderResp.Success)
            //{
            //    helpMe.TradeEvents.PrintMessageEvent("2. Order: " + secondOrderResp.Message, true);
            //    return retTask;
            //}
            //triangleOrders.MarketOrderResponses.Add(secondOrderResp.Data);
            //// <<< Second Order



            //// Third Order >>>
            //mdlOrder.OrderRequestModel thirdOrderReq = SetOrderRequest(secondPair, opportunity.FinalPrices[2].Price, secondOrderResp.Data, thirdPair, "3");
            //triangleOrders.MarketOrderRequests.Add(thirdOrderReq);

            //var thirdOrderResp = await Statics.TradeHelper.FromEngine.CreateOrder(thirdOrderReq);
            //if (!thirdOrderResp.Success)
            //{
            //    helpMe.TradeEvents.PrintMessageEvent("3. Order: " + thirdOrderResp.Message, true);
            //    return retTask;
            //}
            //triangleOrders.MarketOrderResponses.Add(thirdOrderResp.Data);
            // <<< Third Order


            CalculationsAfterTriangleOrders(opportunity, triangleOrders);

            retTask.Success = true;

            return retTask;
        }
        internal static void CalculationsAfterTriangleOrders(absMdl.Engine.Opportunity opportunity, absMdl.Engine.TriangleOrders triangleOrders)
        {
            // Emirlerden sonra NetProfit ve Ratio hesaplamaları yapalım >>>
            List<absMdl.PairModel> pairs = opportunity.Possibility.PossiblePath.PairPaths;

            decimal rawPrice1, rawPrice2, rawPrice3;
            rawPrice1 = Calculations.GetRawPrice(pairs[0].IsDenominator, triangleOrders.MarketOrderResponses[0].Price);
            rawPrice2 = Calculations.GetRawPrice(pairs[1].IsDenominator, triangleOrders.MarketOrderResponses[1].Price);
            rawPrice3 = Calculations.GetRawPrice(pairs[2].IsDenominator, triangleOrders.MarketOrderResponses[2].Price);

            absMdl.RatioModel ratio = new absMdl.RatioModel();
            ratio.RatioRaw = rawPrice1 * rawPrice2 * rawPrice3;
            ratio.Ratio = ratio.RatioRaw * opportunity.Possibility.PossiblePath.RatioFactor;

            triangleOrders.FinalRatio = ratio;

            if (pairs[0].IsDenominator)
            {
                if (pairs[2].IsDenominator)
                {
                    triangleOrders.InputTotalGross = triangleOrders.MarketOrderResponses[0].DenominatorTotalGross;
                    triangleOrders.OutputTotalNet = triangleOrders.MarketOrderResponses[2].NumeratorAmountNet;
                }
                else
                {
                    triangleOrders.InputTotalGross = triangleOrders.MarketOrderResponses[0].DenominatorTotalGross;
                    triangleOrders.OutputTotalNet = triangleOrders.MarketOrderResponses[2].DenominatorTotalNet;
                }
            }
            else
            {
                if (pairs[2].IsDenominator)
                {
                    triangleOrders.InputTotalGross = triangleOrders.MarketOrderResponses[0].NumeratorAmountGross;
                    triangleOrders.OutputTotalNet = triangleOrders.MarketOrderResponses[2].NumeratorAmountNet;
                }
                else
                {
                    triangleOrders.InputTotalGross = triangleOrders.MarketOrderResponses[0].NumeratorAmountGross;
                    triangleOrders.OutputTotalNet = triangleOrders.MarketOrderResponses[2].DenominatorTotalNet;
                }
            }
            triangleOrders.NetProfit = triangleOrders.OutputTotalNet - triangleOrders.InputTotalGross;
            triangleOrders.ProfitableTrade = triangleOrders.NetProfit > 0;
            triangleOrders.BaseCurrencyRatioFromPrice = triangleOrders.OutputTotalNet / triangleOrders.InputTotalGross;
            triangleOrders.CrumbRatio = triangleOrders.FinalRatio.Ratio - triangleOrders.BaseCurrencyRatioFromPrice;
            // <<< Emirlerden sonra NetProfit ve Ratio hesaplamaları yapalım
        }
        /// <summary> Sadece 2. ve 3. order için kullanılır. </summary>
        internal static mdlOrder.OrderRequestModel SetOrderRequest(absMdl.PairModel prevPair, decimal price, mdlOrder.OrderResponseModel prevOrderResp, absMdl.PairModel pair, string orderClientId)
        {
            mdlOrder.OrderRequestModel newReq = new()
            {
                PairSymbol = pair.Pair,
                Price = price,
                CommissionRatio = pair.CommissionRatio,
                OrderClientId = Guid.NewGuid() + "_" + orderClientId,
                PriceScale = pair.DenominatorScale,
                NumeratorScale = pair.NumeratorScale,
                DenominatorScale = pair.DenominatorScale
            };

            // Kural: Üçgen arbitrajda öncekinin NonBase'i sonrakinin Base'idir.
            // Scale'lere dikkat!
            if (prevPair.IsDenominator) // PrevPair: "BTCUSDT" PrevPairBase: "USDT"
            {
                if (pair.IsDenominator) // Pair: "ETHBTC" PairBase: "BTC"
                {
                    newReq.IsBuyOrder = true;
                    // Request'teki gross yazan değerlerden sonunda komisyon düşeceğinden gross olarak nitelendirildi.
                    // Yeni request çekileceğinde önceki reques'in response'unda gelen değerlerin Net olanları set edilir ki
                    //  elimizde olan para ile emir verebilelim.
                    newReq.DenominatorTotalGross = prevOrderResp.NumeratorAmountNet;
                }
                else // Pair: "BTCTRY" PairBase: "BTC"
                {
                    newReq.NumeratorAmountGross = prevOrderResp.NumeratorAmountNet;
                }
            }
            else // PrevPair: "BTCUSDT" PrevPairBase: "BTC"
            {
                if (pair.IsDenominator) // Pair: "ETHUSDT" PairBase: "USDT"
                {
                    newReq.IsBuyOrder = true;
                    newReq.DenominatorTotalGross = prevOrderResp.DenominatorTotalNet;
                }
                else // Pair: "USDTTRY" PairBase: "USDT"
                {
                    newReq.NumeratorAmountGross = prevOrderResp.DenominatorTotalNet;
                }
            }

            return newReq;
        }
    }
}
