using System.Collections.Generic;
namespace TradeAbstractions.Models
{
    public class PossiblePathModel
    {
        /// <summary> Bu path hangi borsa için oluşturuldu? </summary>
        public Enums.EnumExchange Exchange { get; set; }
        /// <summary> Index sırasına göre Pair1, 2 olarak kullanılır. </summary>
        public List<PairModel> PairPaths { get; set; }
        /// <summary>
        /// İki adet komisyonun çarpımından oluşur. Ratio hesaplaması için kullanılır.
        /// Örn: 0.9992 * 0.9984
        /// Sonunda mesela 1.005'lik bir fırsat(RatioRaw) varsa komisyon düşülmüş hali * RatioFactor ile hesaplanır.
        /// Örn: Ratio = 1.005 * 0.996005117952 = 1.00098514354176'lik bir fırsattır aslında.
        /// </summary>
        public decimal RatioFactor { get; set; }

        // Buradan sonrası CreatePossiblePaths sonrasında hesaplanır ve kullanılır. >>>

        /// <summary> İşlem sonucundaki parite oranımız nedir? Alış veya satış yönüne göre USDT/USD veya  USD/USDT hesaplanır. Örn: 1.01 </summary>
        public decimal Ratio { get; set; }
        /// <summary> Kâr yüzdemiz nedir? Örn: 0.01 ise %1'dir.</summary>
        public decimal DiffRatio { get; set; }
    }
}
