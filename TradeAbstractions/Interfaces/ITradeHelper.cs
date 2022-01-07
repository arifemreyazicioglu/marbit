using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TradeAbstractions.Interfaces
{
    public interface ITradeHelper
    {
        IFromEngine FromEngine { get; set; }
        IToEngine ToEngine { get; set; }
    }
}
