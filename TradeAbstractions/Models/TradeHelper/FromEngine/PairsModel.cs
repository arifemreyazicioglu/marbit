using System.Collections.Generic;

namespace TradeAbstractions.Models.TradeHelper.FromEngine
{
    public class PairsModel
    {
        public List<PairRawModel> PairList { get; set; }
        public TradeAbstractions.Helpers.RequestResponseStats ReqResp { get; set; }
    }
}
