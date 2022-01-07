namespace TradeAbstractions.Models.Engine
{
    public class ShortCircuit
    {
        public bool Active { get; set; }
        /// <summary>
        /// opportunity.ZeroIndexRatio.Ratio > opportunity.Possibility.Ratio
        /// kontrolünde kullanılacaktır.
        /// </summary>
        /// <value>
        /// 1.000001m'dan az olamaz
        /// </value>
        public decimal OpportunityRatio { get; set; }
        /// <summary>
        /// opportunity.ZeroIndexRatio.Ratio > opportunity.Possibility.Ratio kontrolünde
        /// kontrolünde kullanılacaktır.
        /// </summary>
        /// <value>
        /// 1.00001m'dan az olamaz
        /// </value>
        public decimal ZeroIndexRatio { get; set; }
    }
}
