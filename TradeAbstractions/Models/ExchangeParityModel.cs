namespace TradeAbstractions.Models
{
    public class ExchangeParityModel : AskBidModel
    {
        /// <summary> USDT satıp USD alıyorsak kullanırız. Örn: 1.01 </summary>
        public decimal SellRatio { get; set; }
        /// <summary> Örn: 0.01 </summary>
        public decimal SellDiffRatio { get; set; }
        /// <summary> USD satıp USDT alıyorsak kullanırız. Örn: 0.99 </summary>
        public decimal BuyRatio { get; set; }
        /// <summary> Örn: -0.01 </summary>
        public decimal BuyDiffRatio { get; set; }
    }
}
