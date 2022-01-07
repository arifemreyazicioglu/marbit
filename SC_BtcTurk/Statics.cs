using absInt = TradeAbstractions.Interfaces;
namespace SC_BtcTurk
{
    /// <summary>
    /// FromEngine create edilirken SetAllProperties() ile buradaki static veriler set edilir.
    /// Bu şekilde proje boyunca ayakta kalan class instance'ları kullanılabilir.
    /// </summary>
    internal class Statics
    {
        internal static absInt.ITradeHelper TradeHelper { get; set; }
        internal static ScHttp ScHttp { get; set; }
        internal void SetAllProperties(absInt.ITradeHelper tradeHelper)
        {
            TradeHelper = tradeHelper;
            ScHttp = new ScHttp();
        }
    }
}
