namespace TradeAbstractions.Models
{
    public class RatioModel
    {
        /// <summary> Komisyonlar çıkarılmadan önceki oran </summary>
        public decimal RatioRaw { get; set; }
        /// <summary> Komisyonlu oran </summary>
        public decimal Ratio { get; set; }
    }
}
