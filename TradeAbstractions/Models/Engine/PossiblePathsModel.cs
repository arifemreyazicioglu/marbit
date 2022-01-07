using System.Collections.Generic;
using TradeAbstractions.Helpers;
namespace TradeAbstractions.Models.Engine
{
    public class PossiblePathsModel
    {
        public List<PossiblePathModel> PathList { get; set; }
        public List<RequestResponseStats> ReqRespList { get; set; }
    }
}
