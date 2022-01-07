namespace TradeAbstractions.Models
{
    public class PairModel : PairRawModel
    {
        /// <summary> Örn: USDT </summary>
        /// <remarks> Sonradan hesaplanır. </remarks>
        public string Base { get; set; }
        /// <summary> Örn: TRY </summary>
        /// <remarks> Sonradan hesaplanır. </remarks>
        public string NonBase { get; set; }
        /// <summary> true: Base paydada, false: Base payda</summary>
        /// <remarks> Sonradan hesaplanır. </remarks>
        public bool IsDenominator { get; set; }

        // Buradan sonrası CreatePossiblePaths sonrasında hesaplanır ve kullanılır. >>>

        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        /// <summary> Commission içerir </summary>
        public decimal NumeratorAmountGross { get; set; }
        /// <summary> Commission içerir </summary>
        public decimal DenominatorTotalGross { get; set; }
        public decimal NumeratorAmountNet { get; set; }
        public decimal DenominatorTotalNet { get; set; }
        /// <summary>
        /// Bunu parite oranını hesaplamak için kullanacağız. Komisyonlar çıkarılmış olarak Ask ve Bid'ler de dikkate alınarak net değer hesaplanır.
        /// Aslında Base: USDT iken yukarıdaki DenominatorTotalNet değeri ile aynı değerdir. Ancak Base: TRY iken biz hesaplayacağız.
        /// </summary>
        public decimal UsdOrUsdtTotal { get; set; }
    }
}
