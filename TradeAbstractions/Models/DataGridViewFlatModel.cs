namespace TradeAbstractions.Models
{
    public class DataGridViewFlatModel
    {
        /// <summary>
        /// Örn: BTC
        /// Pair1 ve Pair2 için aynı.
        /// </summary>
        public string Currency { get; set; }
        /// <summary> 
        /// Bu path hangi borsa için oluşturuldu? 
        /// Pair1 ve Pair2 için aynı.
        /// </summary>
        public Enums.EnumExchange Exchange { get; set; }
        /// <summary> Örn: BTCUSDT </summary>
        public string Pair1 { get; set; }
        /// <summary>
        /// Ask veya Bid değerini içerir.
        /// </summary>
        public decimal Price1 { get; set; }
        /// <summary>
        /// Net amount miktarı. Komisyondan arındırılmış halini görürüz.
        /// </summary>
        public decimal Amount1 { get; set; }
        public decimal Total1 { get; set; }
        /// <summary> Örn: BTCTRY </summary>
        public string Pair2 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Total2 { get; set; }

        /// <summary>
        /// Kâr yüzdemiz nedir? Örn: 0.01 ise %1'dir.
        /// Pair1 ve Pair2 için aynı.
        /// </summary>
        public decimal ParityRatio { get; set; }
        /// <summary>
        /// Trade tamamlandığında yöne göre sonuçtaki USD veya USDT varlığı ne oluyor?
        /// </summary>
        public decimal UsdOrUsdtResult { get; set; }
        /// <summary>
        /// Bunu parite oranını hesaplamak için kullanacağız. Komisyonlar çıkarılmış olarak Ask ve Bid'ler de dikkate alınarak net değer hesaplanır.
        /// Aslında Base: USDT iken yukarıdaki DenominatorTotalNet değeri ile aynı değerdir. Ancak Base: TRY iken biz hesaplayacağız.
        /// </summary>
        public decimal Profit { get; set; }
    }
}
