using absSettings = TradeAbstractions.Helpers.Settings;
using helpMe = TradeEngine.Models.HelpMe;
using absMdl = TradeAbstractions.Models;

namespace TradeEngine.Operations
{
    internal class MyAccount
    {
        /// <summary>
        /// Emir miktarlarını hesaplatalım. Serbest bakiyemiz ve minimum miktarlar da hesaba katılır.
        /// </summary>
        /// <param name="amount"> İlk pair'in base cinsine çevirilmiş miktarını geriye döneceğiz. </param>
        /// <param name="minAmount"> İlk pair'in base cinsine çevirilmiş miktarını geriye döneceğiz. </param>
        /// <returns></returns>
        internal static bool CalculateOrderPrices(absMdl.Engine.Opportunity opportunity, ref decimal amount, ref decimal minAmount)
        {
            absMdl.PairModel firstPair = opportunity.Possibility.PossiblePath.PairPaths[0];
            absMdl.OrderPriceModel firstPrice = opportunity.IntersectionPrices[0];

            
            bool baseFound = false;
            
            // Settings'teki base listemizde varsa trade edeceğiz.
            if (!baseFound) return false;

            if (firstPair.MinNumeratorValue == 0 && firstPair.MinDenominatorValue == 0)
            {
                helpMe.TradeEvents.PrintMessageEvent("CalculateOrderPrices, firstPair.MinNumeratorValue == 0 && firstPair.MinDenominatorValue == 0", anyError: true);
                return false;
            }

            decimal tempAmount = 0, tempMinAmount;

            if (firstPair.IsDenominator)
            {
                if (firstPair.MinDenominatorValue != 0) tempMinAmount = firstPair.MinDenominatorValue;
                else tempMinAmount = firstPrice.Price * firstPair.MinNumeratorValue;

            }
            else
            {
                if (firstPair.MinNumeratorValue != 0) tempMinAmount = firstPair.MinNumeratorValue;
                else tempMinAmount = firstPair.MinDenominatorValue / firstPrice.Price;
            }

            // Minimum limit borsanın belirttiği değerin 2 katı olsun isteriz.
            // 3 ayrı pair için ayrı minamount isteyen borsaların handikapını bu şekilde bypass edelim.
            tempMinAmount = tempMinAmount * 2;

            // User Balance'dan amaount bilgisini set edelim.
            // SCTODO
            //int listCount = absSettings.InternalSettings.UserBalance.BalanceList.Count;
            //for (int i = 0; i < listCount; i++)
            //{
            //    absMdl.BalanceItemModel item = absSettings.InternalSettings.UserBalance.BalanceList[i];
            //    if (item.Asset == firstPair.Base)
            //    {
            //        tempAmount = item.Free;
            //        break;
            //    }
            //}

            // Free bakiyemiz minimum değerden az olamaz. Yeni fırsat arayalım.
            if (tempAmount < tempMinAmount) return false;

            amount = tempAmount;
            minAmount = tempMinAmount;

            return true;
        }
    }
}
