using TradeAbstractions.Enums;
using absMdl = TradeAbstractions.Models;
namespace TradeAbstractions.Models
{
    public class ModeModel
    {
        public EnumOrderBookMode OrderBookMode { get; set; }
        /// <summary>
        /// ask ve bid'lerden hangileri seçildi ise onlar set edilir.
        /// Örn: Pair1 için tahtadaki ilk sıradaki olsun, AskBidIndexes[0] = 0
        /// Örn: Pair2 için tahtadaki 3. sıradaki olsun, AskBidIndexes[1] = 2
        /// Örn: Pair3 için tahtadaki 5. sıradaki olsun, AskBidIndexes[2] = 4
        /// </summary>
        public int[] AskBidIndexes { get; set; }
        public absMdl.RatioModel ModRatio { get; set; }
    }
}
