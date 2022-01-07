using System.Collections.Generic;

namespace TradeAbstractions.Models.TradeHelper.FromEngine
{
    public class TickerModel
    {
        /// <summary> Ask Price değerlerini tutacağımız lookup </summary>
        /// <example> Key:BTCTRY Value:309649 </example>
        public Dictionary<string, decimal> AskLookup { get; set; }
        /// <summary> Bid Price değerlerini tutacağımız lookup </summary>
        /// <example> Key:BTCTRY Value:309473 </example>
        public Dictionary<string, decimal> BidLookup { get; set; }
        public TradeAbstractions.Helpers.RequestResponseStats ReqResp { get; set; }
    }
}
