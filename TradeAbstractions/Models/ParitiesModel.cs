using System;
using System.Collections.Generic;
using enumExchange = TradeAbstractions.Enums.EnumExchange;

namespace TradeAbstractions.Models
{
    /// <summary>
    /// USD ve USDT olarak pariteleri kullandığımız modeldir.
    /// </summary>
    public class ParitiesModel
    {
        public ParitiesModel()
        {
            UsdTry = new();
            Exchanges = new();

            // Dictionary'i kullanıma hazırlayalım.
            foreach (enumExchange exchangeEnum in Enum.GetValues<enumExchange>())
            {
                Exchanges.Add(exchangeEnum, new ExchangeParityModel());
            }
        }
        /// <summary> Bankadan veya borsa dışındaki kullanılacak kaynaktan alınan USDTRY paritesi. </summary>
        public AskBidModel UsdTry { get; set; }
        /// <summary>
        /// Borsa bazında USDTTRY paritelerinin bilgilerini barındırır.
        /// </summary>
        public Dictionary<enumExchange, ExchangeParityModel> Exchanges { get; set; }
        public void SetExchangeValues(enumExchange enmExchange, decimal ask, decimal bid)
        {
            var exchange = Exchanges[enmExchange];
            exchange.Ask = ask;
            exchange.Bid = bid;
            exchange.BuyRatio = UsdTry.Bid / ask;
            exchange.SellRatio = bid / UsdTry.Ask;
            exchange.BuyDiffRatio = exchange.BuyRatio - 1;
            exchange.SellDiffRatio = exchange.SellRatio - 1;
        }
    }
}
