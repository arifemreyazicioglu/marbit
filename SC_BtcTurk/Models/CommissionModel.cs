using Newtonsoft.Json;

namespace SC_BtcTurk.Models
{
    public class CommissionModel
    {
        [JsonProperty("maker")]
        public decimal Maker { get; set; }
        [JsonProperty("pairSymbol")]
        public string PairSymbol { get; set; }
        [JsonProperty("pairSymbolNormalized")]
        public string PairSymbolNormalized { get; set; }
        [JsonProperty("taker")]
        public decimal Taker { get; set; }
    }
}
