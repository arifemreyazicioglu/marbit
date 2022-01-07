using System.Collections.Generic;
using TradeAbstractions.Helpers;

namespace TradeAbstractions.Models.TradeHelper.FromEngine
{
    public class OrderBookModel
    {
        public List<OrderBookItemModel> OrderBookList { get; set; }
        public bool HasTimePerPair { get; set; }
    }
}
