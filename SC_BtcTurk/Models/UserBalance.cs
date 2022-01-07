using Newtonsoft.Json;

namespace SC_BtcTurk.Models
{
    public class UserBalance
    {
        /// <example> TRY </example>
        [JsonProperty("asset")]
        public string Asset { get; set; }
        /// <example> Türk Lirası </example>
        [JsonProperty("assetname")]
        public string AssetName { get; set; }
        /// <example> 2144.1535911058659401 </example>
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
        /// <example> 0 </example>
        [JsonProperty("locked")]
        public decimal Locked { get; set; }
        /// <example> 2144.1535911058659401 </example>
        [JsonProperty("free")]
        public decimal Free { get; set; }
        /// <example> 0 </example>
        [JsonProperty("orderFund")]
        public decimal OrderFund { get; set; }
        /// <example> 0 </example>
        [JsonProperty("requestFund")]
        public decimal RequestFund { get; set; }
        /// <example> 2 </example>
        [JsonProperty("precision")]
        public int Precision { get; set; }
    }
}
