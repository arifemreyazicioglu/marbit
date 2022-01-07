using Newtonsoft.Json;

namespace SC_BtcTurk.Models
{
    public class UserTrade
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        /// <summary> Ben ekledim </summary>
        [JsonProperty("orderId")]
        public long OrderId { get; set; }
        /// <summary> Pair'in sağ kısmındaki currency </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("numeratorSymbol")]
        public string NumeratorSymbol { get; set; }
        [JsonProperty("denominatorSymbol")]
        public string DenominatorSymbol { get; set; }
        [JsonProperty("orderType")]
        public Enums.OrderType OrderType { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        /// <summary>
        /// Pair'in sol kısmındaki currency
        /// Response'larda gelen amount yukarı yuvarlanmıştır BtcTurk tarafından. Güvenilmezdir, kullanılmayacak.
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        /// <summary> Fee, tax'i içermediğinden commission için ikisi toplanacak. Pair'in sağ kısmı cinsindendir. </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; set; }
        /// <summary> Fee, tax'i içermediğinden commission için ikisi toplanacak. Pair'in sağ kısmı cinsindendir. </summary>
        [JsonProperty("tax")]
        public decimal Tax { get; set; }
    }
}
