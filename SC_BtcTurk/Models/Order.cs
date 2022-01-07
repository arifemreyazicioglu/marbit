using Newtonsoft.Json;

namespace SC_BtcTurk.Models
{
    public class Order
    {
        public class RequestModel
        {
            public decimal Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal StopPrice { get; set; }
            public string NewOrderClientId { get; set; }
            public Enums.OrderMethod OrderMethod { get; set; }
            public Enums.OrderType OrderType { get; set; }
            public string PairSymbol { get; set; }
        }
        public class ResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }
            [JsonProperty("datetime")]
            public long Datetime { get; set; }
            [JsonProperty("type")]
            public Enums.OrderType Type { get; set; }
            [JsonProperty("method")]
            public Enums.OrderMethod Method { get; set; }
            [JsonProperty("price")]
            public decimal Price { get; set; }
            [JsonProperty("amount")]
            public decimal Amount { get; set; }
            [JsonProperty("quantity")]
            public decimal Quantity { get; set; }
            [JsonProperty("pairSymbol")]
            public string PairSymbol { get; set; }
            [JsonProperty("pairSymbolNormalized")]
            public string PairSymbolNormalized { get; set; }
            [JsonProperty("newOrderClientId")]
            public string NewOrderClientId { get; set; }
        }
    }
}
