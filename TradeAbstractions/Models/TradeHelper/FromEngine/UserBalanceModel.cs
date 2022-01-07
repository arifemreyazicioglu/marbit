using System.Collections.Generic;
using TradeAbstractions.Helpers;

namespace TradeAbstractions.Models.TradeHelper.FromEngine
{
    public class UserBalanceModel
    {
        public List<BalanceItemModel> BalanceList { get; set; }
        public RequestResponseStats ReqResp { get; set; }
    }
}
