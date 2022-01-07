using Newtonsoft.Json;
using System.Collections.Generic;

namespace SC_BtcTurk.Models
{
    /// <summary>
    /// Tüm property'lere JsonProperty atanmadı çünkü performans burada hiç önemli değil.
    /// Uygulamanın başında ve belki saatte bir falan başvurulacak bir servis
    /// </summary>
    public class ExchangeInfoModel
    {
        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }
        [JsonProperty("symbols")]
        public List<SymbolModel> Symbols { get; set; }
        [JsonProperty("currencies")]
        public List<CurrencyModel> Currencies { get; set; }
        [JsonProperty("currencyOperationBlocks")]
        public List<CurrencyOperationBlockModel> CurrencyOperationBlocks { get; set; }

        public class SymbolModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string NameNormalized { get; set; }
            public string Status { get; set; }
            public string Numerator { get; set; }
            public string Denominator { get; set; }
            /// <summary> Numerator (sol currency) noktadan sonraki ondalık değeri </summary>
            public int NumeratorScale { get; set; }
            /// <summary> DenominatorScale (sağ currency) noktadan sonraki ondalık değeri </summary>
            public int DenominatorScale { get; set; }
            /// <summary> false olursa DenominatorScale'i 0 yapacağız abstraction modelinde </summary>
            public bool HasFraction { get; set; }
            public List<FilterModel> Filters { get; set; }
            public List<string> OrderMethods { get; set; }
            public string DisplayFormat { get; set; }
            public bool CommissionFromNumerator { get; set; }
            public int Order { get; set; }
            public bool PriceRounding { get; set; }
            public bool IsNew { get; set; }

            public class FilterModel
            {
                public string FilterType { get; set; }
                public string MinPrice { get; set; }
                public string MaxPrice { get; set; }
                /// <summary>
                /// https://github.com/BTCTrader/broker-api-docs/issues/205
                /// tickSize değeri yalnızca web ve mobil uygulama üzerinden fiyatın yanındaki aşağı ve yukarı oklarda
                /// fiyatın kaçar kaçar artıp azalacağını belirtir. Birim fiyat veya minimum fiyat değişimi anlamına gelmez.
                /// </summary>
                public string TickSize { get; set; }
                /// <summary> Denominator (sağ currency) için minimum emir miktarı </summary>
                public decimal MinExchangeValue { get; set; }
                public string MinAmount { get; set; }
                public string MaxAmount { get; set; }
            }
        }

        public class CurrencyModel
        {
            public int Id { get; set; }
            public string Symbol { get; set; }
            public decimal MinWithdrawal { get; set; }
            public decimal MinDeposit { get; set; }
            public int Precision { get; set; }
            public AddressModel Address { get; set; }
            public string CurrencyType { get; set; }
            public TagModel Tag { get; set; }
            public string Color { get; set; }
            public string Name { get; set; }
            public bool IsAddressRenewable { get; set; }
            public bool GetAutoAddressDisabled { get; set; }
            public bool IsNew { get; set; }

            public class AddressModel
            {
                public int? MinLen { get; set; }
                public int? MaxLen { get; set; }
            }
            public class TagModel
            {
                public bool Enable { get; set; }
                public string Name { get; set; }
                public int? MinLen { get; set; }
                public int? MaxLen { get; set; }
            }
        }

        public class CurrencyOperationBlockModel
        {
            public string CurrencySymbol { get; set; }
            public bool WithdrawalDisabled { get; set; }
            public bool DepositDisabled { get; set; }
        }
    }
}
