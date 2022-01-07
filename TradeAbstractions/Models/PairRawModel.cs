namespace TradeAbstractions.Models
{
    public class PairRawModel
    {
        /// <summary> Örn: USDTTRY </summary>
        public string Pair { get; set; }
        /// <summary> Örn: USDT </summary>
        public string Numerator { get; set; }
        /// <summary> Örn: TRY </summary>
        public string Denominator { get; set; }
        /// <summary> Örn: 0.001 (binde bir)</summary>
        public decimal CommissionRatio { get; set; }
        /// <summary> Sol kısım noktadan sonra kaç ondalık değer kabul ediyor </summary>
        public int NumeratorScale { get; set; }
        /// <summary> Sağ kısım noktadan sonra kaç ondalık değer kabul ediyor </summary>
        public int DenominatorScale { get; set; }
        /// <summary>
        /// Pair'in sol kısmı minimum ne kadar olmalı?
        /// 0 ise borsa tarafından belirtilmemiş demektir
        /// </summary>
        /// <example>
        /// BtcTurk'te bu veri bulunmuyor.
        /// </example>
        public decimal MinNumeratorValue { get; set; }
        /// <summary>
        /// Pair'in sağ kısmı minimum ne kadar olmalı?
        /// 0 ise borsa tarafından belirtilmemiş demektir
        /// </summary>
        /// <example>
        /// BtcTurk sadece bu veriyi veriyor.
        /// </example>
        public decimal MinDenominatorValue { get; set; }
    }
}
