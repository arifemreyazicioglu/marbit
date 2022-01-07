using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using absMdl = TradeAbstractions.Models;
using absInt = TradeAbstractions.Interfaces;
using absHlpr = TradeAbstractions.Helpers;
namespace TradeEngine
{
    public class Statics
    {
        /// <summary> TradeEngine.Models.HelpMe.SetHelper(absIntUtils.ITradeInfos tradeInfos) içinden set edilir </summary>
        public static Dictionary<TradeAbstractions.Enums.EnumExchange, TradeHelper> TradeHelpers { get; set; }
        /// <summary>
        /// Tüm borsalar aynı ayar grubunu kullansın. Aynı anda sadece tek borsadaki trade gerçekleştirilir.
        /// Paralel olarak verileri alıp değerlendiririz ancak paralal trade ettirmeyiz.
        /// </summary>
        public static absMdl.Engine.EngineProps EngineProps { get; set; } = new();
    }
}
