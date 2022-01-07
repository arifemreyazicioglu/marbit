using Newtonsoft.Json;

namespace SC_BtcTurk.Models
{
    public class OrderBook
    {
        [JsonProperty("timeStamp")]
        public long TimeStamp { get; set; }
        [JsonProperty("bids")]
        public decimal[,] Bids { get; set; }
        [JsonProperty("asks")]
        public decimal[,] Asks { get; set; }
    }
}
