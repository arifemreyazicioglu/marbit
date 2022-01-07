using System.Collections.Concurrent;
using System.Collections.Generic;
using TradeAbstractions.Interfaces;
using TradeAbstractions.Models;
using TradeAbstractions.Models.TradeHelper;
using absInt = TradeAbstractions.Interfaces;
using TradeAbstractions.Models.TradeHelper.FromEngine;
namespace TradeEngine
{
    public class TradeHelper : absInt.ITradeHelper
    {
        public absInt.IFromEngine FromEngine { get; set; }
        public absInt.IToEngine ToEngine { get; set; }
        public void SetFromTo(absInt.IFromEngine fromEngine, absInt.IToEngine toEngine)
        {
            FromEngine = fromEngine;
            ToEngine = toEngine;
        }
        // >>>>>>>>>>>>>>
        // Buradan itibaren sadece Engine'i ilgilediren kısımlar kullanılır.
        // >>>>>>>>>>>>>>
        public volatile UserBalanceModel UserBalance;
    }
}
