using Newtonsoft.Json;

namespace SC_BtcTurk.Models
{
    public class Ticker
    {
        [JsonProperty("pair")]
        public string Pair { get; set; }
        [JsonProperty("pairNormalized")]
        public string PairNormalized { get; set; }
        [JsonProperty("timeStamp")]
        public long Timestamp { get; set; }
        [JsonProperty("last")]
        public decimal Last { get; set; }
        [JsonProperty("high")]
        public decimal High { get; set; }
        [JsonProperty("low")]
        public decimal Low { get; set; }
        [JsonProperty("bid")]
        public decimal Bid { get; set; }
        [JsonProperty("ask")]
        public decimal Ask { get; set; }
        [JsonProperty("open")]
        public decimal Open { get; set; }
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
        [JsonProperty("average")]
        public decimal Average { get; set; }
        [JsonProperty("daily")]
        public decimal Daily { get; set; }
        [JsonProperty("dailyPercent")]
        public decimal DailyPercent { get; set; }
        [JsonProperty("denominatorSymbol")]
        public string DenominatorSymbol { get; set; }
        [JsonProperty("numeratorSymbol")]
        public string NumeratorSymbol { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
