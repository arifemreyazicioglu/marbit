using TradeAbstractions.Models.TradeHelper.FromEngine;
namespace TradeAbstractions.Models.Engine
{
    public class TriangleCompletedData
    {
        public Opportunity Opportunity { get; set; }
        public TriangleOrders TriangleOrders { get; set; }
        public ServerTimestampModel ServerTimestamp { get; set; }
        /// <summary> Tüm bir Triangle Arbitrage'ın ne kadar sürdüğü bilgisini tutalım </summary>
        public double TotalElapsedMs { get; set; }
        public UserBalancesModel UserBalances { get; set; }
        public class UserBalancesModel
        {
            public UserBalanceModel BeforeTrade { get; set; }
            public UserBalanceModel AfterTrade { get; set; }
        }
    }
}
