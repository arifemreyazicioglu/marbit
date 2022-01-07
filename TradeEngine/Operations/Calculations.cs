using System.Collections.Generic;
using absMdl = TradeAbstractions.Models;
namespace TradeEngine.Operations
{
    internal static class Calculations
    {
        internal static decimal GetRawPrice(bool isDenominator, decimal ask, decimal bid)
        {
            return isDenominator ? (1 / ask) : bid;
        }
        /// <summary>
        /// Price ask da olabilir bid de.
        /// isDenominator ile price'ın doğru gönderilmesi sorumluluğu sana ait :)
        /// isDenominator=true ise price Ask'tir, false ise Bid'dir.
        /// Merak etme MarketOrderReponse'da ona göre set edildi.
        /// </summary>
        internal static decimal GetRawPrice(bool isDenominator, decimal price)
        {
            return isDenominator ? (1 / price) : price;
        }
        /// <param name="pairIndex"> Pair1,2 ve 3 için 0, 1 veya 2 olabilir. </param>
        /// <param name="askBidIndex"> En uygun fiyatı almak için 0, tahtadaki 5. fiyatı almak için 4 girilmelidir. </param>
        /// <returns></returns>
        internal static decimal GetRawPrice(absMdl.PossiblePathModel path, List<absMdl.OrderBookItemModel> orderBookList, int pairIndex, int askBidIndex)
        {
            return GetRawPrice(path.PairPaths[pairIndex].IsDenominator, orderBookList[pairIndex].Ask[askBidIndex], orderBookList[pairIndex].Bid[askBidIndex]);
        }
        internal static absMdl.RatioModel RatioCalculation(absMdl.PossiblePathModel path, List<absMdl.OrderBookItemModel> orderBookList, int[] askBidIndexes)
        {
            absMdl.RatioModel ratio = new absMdl.RatioModel();

            decimal rawPrice1, rawPrice2, rawPrice3;

            // path.TrianglePairs ve OrderBook'ta Pair'lar sıralı kayıt edildiği için aynı sıra ile alınır.
            rawPrice1 = GetRawPrice(path, orderBookList, 0, askBidIndexes[0]);
            rawPrice2 = GetRawPrice(path, orderBookList, 1, askBidIndexes[1]);
            rawPrice3 = GetRawPrice(path, orderBookList, 2, askBidIndexes[2]);

            ratio.RatioRaw = rawPrice1 * rawPrice2 * rawPrice3;
            ratio.Ratio = ratio.RatioRaw * path.RatioFactor;

            return ratio;
        }
        internal static List<absMdl.OrderPriceModel> GetOrderActualPrices(absMdl.Engine.Opportunity opportunity)
        {
            List<absMdl.PairModel> trianglePairs = opportunity.Possibility.PossiblePath.PairPaths;
            List<absMdl.OrderBookItemModel> orderBookList = opportunity.OrderBook.OrderBookList;
            int[] askBidIndexes = opportunity.OrderBookModeSelected.AskBidIndexes;

            var actualPrices = new List<absMdl.OrderPriceModel>();

            for (int pairIndex = 0; pairIndex < 3; pairIndex++)
            {
                absMdl.OrderPriceModel orderPrice = new absMdl.OrderPriceModel();

                if (trianglePairs[pairIndex].IsDenominator)
                {
                    orderPrice.Price = orderBookList[pairIndex].Ask[askBidIndexes[pairIndex]];
                    orderPrice.NumeratorAmount = orderBookList[pairIndex].AskAmount[askBidIndexes[pairIndex]];
                    orderPrice.DenominatorTotal = orderBookList[pairIndex].Ask[askBidIndexes[pairIndex]] * orderBookList[pairIndex].AskAmount[askBidIndexes[pairIndex]];
                }
                else
                {
                    orderPrice.Price = orderBookList[pairIndex].Bid[askBidIndexes[pairIndex]];
                    orderPrice.NumeratorAmount = orderBookList[pairIndex].BidAmount[askBidIndexes[pairIndex]];
                    orderPrice.DenominatorTotal = orderBookList[pairIndex].Bid[askBidIndexes[pairIndex]] * orderBookList[pairIndex].BidAmount[askBidIndexes[pairIndex]];
                }

                actualPrices.Add(orderPrice);
            }

            return actualPrices;
        }
        internal static List<absMdl.OrderPriceModel> CalculateOrderIntersectionPrices(List<absMdl.PairModel> trianglePairs, List<absMdl.OrderPriceModel> actualPrices)
        {
            // ActualPrice'lardan en düşük değere sahip olanı bulalım.
            int smallPrcInd = GetSmallestPriceIndex(trianglePairs, actualPrices);

            absMdl.OrderPriceModel smallestPrice = new absMdl.OrderPriceModel
            {
                Price = actualPrices[smallPrcInd].Price,
                NumeratorAmount = actualPrices[smallPrcInd].NumeratorAmount,
                DenominatorTotal = actualPrices[smallPrcInd].DenominatorTotal
            };

            return ReCalculateFromSelectedOrderPrice(smallestPrice, smallPrcInd, trianglePairs, actualPrices);
        }
        /// <summary>
        /// Buraya gelmeden opportunity'nin ilk pairinin base'i SharedSettings.BaseList içerisinde var mı kontrolü yapılmış olmalı !!!
        /// Tahtada bizim gireceğimiz miktardan fazlası var ise bizim fiyata göre yeniden şekilllenir (instance create edilir)
        /// Tahtada bizim gireceğimiz miktardan daha azı varsa FinalPrices = IntersectionPrices (pointer kullanmaktan vazgeçtim)
        /// </summary>
        /// <param name="amount"> Önceden ilk pair'in base cinsine çevirilmiş olarak gönderilmeli. </param>
        /// <param name="minAmount"> Önceden ilk pair'in base cinsine çevirilmiş olarak gönderilmeli. </param>
        /// <returns> İlk Pair'de bizim base currency yoksa veya bakiyemiz yetersizse null döneriz. </returns>
        internal static List<absMdl.OrderPriceModel> CalculateOrderFinalPrices(List<absMdl.PairModel> trianglePairs, List<absMdl.OrderPriceModel> intersectionPrices, decimal amount, decimal minAmount)
        {
            absMdl.PairModel firstPair = trianglePairs[0];
            absMdl.OrderPriceModel firstIntersectionPrice = intersectionPrices[0];

            bool setNewPrice = false;
            absMdl.OrderPriceModel newFirstPrice = null;

            if (firstPair.IsDenominator)
            {
                // Tahtada bizim gireceğimiz miktardan daha fazlası olmalı ki bizim yeni değeri set edelim.
                if (firstIntersectionPrice.DenominatorTotal > amount)
                {
                    newFirstPrice = new absMdl.OrderPriceModel
                    {
                        Price = firstIntersectionPrice.Price,
                        NumeratorAmount = amount / firstIntersectionPrice.Price,
                        DenominatorTotal = amount
                    };

                    setNewPrice = true;
                }
                // Tahtadaki miktar borsa'nın istediği minimum miktarı karşılamazsa FinalPrices set etmeyiz
                else if (firstIntersectionPrice.DenominatorTotal < minAmount)
                {
                    return null;
                }
            }
            else
            {
                // Tahtada bizim gireceğimiz miktardan daha fazlası olmalı ki bizim yeni değeri set edelim.
                if (firstIntersectionPrice.NumeratorAmount > amount)
                {
                    newFirstPrice = new absMdl.OrderPriceModel
                    {
                        Price = firstIntersectionPrice.Price,
                        NumeratorAmount = amount,
                        DenominatorTotal = firstIntersectionPrice.Price * amount
                    };

                    setNewPrice = true;
                }
                // Tahtadaki miktar borsa'nın istediği minimum miktarı karşılamazsa FinalPrices set etmeyiz
                else if (firstIntersectionPrice.NumeratorAmount < minAmount)
                {
                    return null;
                }
            }

            // Tahtada bizim gireceğimiz miktardan daha azı varsa FinalPrices = IntersectionPrices
            if (!setNewPrice)
            {
                List<absMdl.OrderPriceModel> newPrices = new();
                newFirstPrice = new absMdl.OrderPriceModel
                {
                    Price = firstIntersectionPrice.Price,
                    NumeratorAmount = firstIntersectionPrice.NumeratorAmount,
                    DenominatorTotal = firstIntersectionPrice.DenominatorTotal
                };
                absMdl.OrderPriceModel newSecondPrice = new absMdl.OrderPriceModel
                {
                    Price = intersectionPrices[1].Price,
                    NumeratorAmount = intersectionPrices[1].NumeratorAmount,
                    DenominatorTotal = intersectionPrices[1].DenominatorTotal
                };
                absMdl.OrderPriceModel newThirdPrice = new absMdl.OrderPriceModel
                {
                    Price = intersectionPrices[2].Price,
                    NumeratorAmount = intersectionPrices[2].NumeratorAmount,
                    DenominatorTotal = intersectionPrices[2].DenominatorTotal
                };

                newPrices.Add(newFirstPrice);
                newPrices.Add(newSecondPrice);
                newPrices.Add(newThirdPrice);

                return newPrices;
            }

            return ReCalculateFromSelectedOrderPrice(newFirstPrice, 0, trianglePairs, intersectionPrices);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromPrice"> Bu değer create edildikten sonra gönderilmeli. </param>
        /// <param name="fromIndex"> ActualPrices içindeki index'i. </param>
        /// <param name="opportunity"></param>
        /// <returns></returns>
        static List<absMdl.OrderPriceModel> ReCalculateFromSelectedOrderPrice(absMdl.OrderPriceModel fromPrice, int fromIndex, List<absMdl.PairModel> trianglePairs, List<absMdl.OrderPriceModel> actualPrices)
        {
            List<absMdl.OrderPriceModel> newPrices = new();
            for (int i = 0; i < 3; i++)
            {
                absMdl.OrderPriceModel newPrice;

                // Seçili değere sahip olanda isek, sırası geldiği için sadece listeye ekleyelim
                if (i == fromIndex)
                {
                    newPrices.Add(fromPrice);
                    continue;
                }

                newPrice = new absMdl.OrderPriceModel
                {
                    Price = actualPrices[i].Price
                };

                if (trianglePairs[i].Numerator == trianglePairs[fromIndex].Numerator)
                {
                    newPrice.NumeratorAmount = fromPrice.NumeratorAmount;
                    newPrice.DenominatorTotal = newPrice.Price * newPrice.NumeratorAmount;
                }
                if (trianglePairs[i].Numerator == trianglePairs[fromIndex].Denominator)
                {
                    newPrice.NumeratorAmount = fromPrice.DenominatorTotal;
                    newPrice.DenominatorTotal = newPrice.Price * newPrice.NumeratorAmount;
                }
                if (trianglePairs[i].Denominator == trianglePairs[fromIndex].Numerator)
                {
                    newPrice.DenominatorTotal = fromPrice.NumeratorAmount;
                    newPrice.NumeratorAmount = newPrice.DenominatorTotal / newPrice.Price;
                }
                if (trianglePairs[i].Denominator == trianglePairs[fromIndex].Denominator)
                {
                    newPrice.DenominatorTotal = fromPrice.DenominatorTotal;
                    newPrice.NumeratorAmount = newPrice.DenominatorTotal / newPrice.Price;
                }

                newPrices.Add(newPrice);
            }

            return newPrices;
        }
        /// <summary> ActualPrice'lardan en düşük değere sahip olanı bulmamıza yarar. </summary>
        /// <returns> 0,1 veya 2 </returns>
        static int GetSmallestPriceIndex(List<absMdl.PairModel> trianglePairs, List<absMdl.OrderPriceModel> actualPrices)
        {
            decimal bigIndexPrice, smallIndexPrice;

            // ActualPrices[0]'ı en küçük kabul edelim
            int smallInd = 0, indexDiff;
            for (int i = 1; i < 3; i++)
            {
                indexDiff = i - smallInd;

                // BüyükIndex-KüçükIndex = 1 ise BüyükIndex'in Base'i ile KüçükIndex'in NonBase'i karşılaştırılır.
                if (indexDiff == 1)
                {
                    // Base sağda ise total, solda ise amount kullanılır.
                    bigIndexPrice = trianglePairs[i].IsDenominator ? actualPrices[i].DenominatorTotal : actualPrices[i].NumeratorAmount;
                    // Base sağda ise NonBase soldadır ya da tersi.
                    smallIndexPrice = trianglePairs[smallInd].IsDenominator ? actualPrices[smallInd].NumeratorAmount : actualPrices[smallInd].DenominatorTotal;
                }
                // BüyükIndex-KüçükIndex = 2 ise BüyükIndex'in NonBase'i ile KüçükIndex'in Base'i karşılaştırılır.
                else
                {
                    // Base sağda ise NonBase soldadır ya da tersi.
                    bigIndexPrice = trianglePairs[i].IsDenominator ? actualPrices[i].NumeratorAmount : actualPrices[i].DenominatorTotal;
                    // Base sağda ise total, solda ise amount kullanılır.
                    smallIndexPrice = trianglePairs[smallInd].IsDenominator ? actualPrices[smallInd].DenominatorTotal : actualPrices[smallInd].NumeratorAmount;
                }
                if (bigIndexPrice < smallIndexPrice) smallInd = i;
            }

            return smallInd;
        }
    }
}
