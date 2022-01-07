namespace TradeAbstractions.Models
{
    public class BalanceItemModel
    {
        /// <example> TRY </example>
        public string Asset { get; set; }
        /// <example> 2144.1535911058659401 </example>
        public decimal Balance { get; set; }
        /// <example> 0 </example>
        public decimal Locked { get; set; }
        /// <example> 2144.1535911058659401 </example>
        public decimal Free { get; set; }
    }
}