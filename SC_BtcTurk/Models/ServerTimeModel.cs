using Newtonsoft.Json;
using System;

namespace SC_BtcTurk.Models
{
    public class ServerTimeModel
    {
        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }
        [JsonProperty("serverTime2")]
        public DateTime ServerTime2 { get; set; }
    }
}
