namespace TradeAbstractions.Models
{
    public class OrderPriceModel
    {
        /// <summary>
        /// Bid veya Ask price bedeli. Pair'in sağındaki currency cinsindendir
        /// Order Request için kullanacağımızda, TradeEngineMode2 için bu bedel average price bedelidir!
        ///     Tahtadaki trade edilebilir alt fiyatları da ortalama için hesaplar tabii ki amount miktarları da dahil edilerek yapılır.
        ///     Sonuçta trade edebileceğimiz güvenli minimum bid veya maksimum ask fiyatını bulmuş olur.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary> Amount her zaman Pair'in solundaki currency cinsindendir </summary>
        public decimal NumeratorAmount { get; set; }
        /// <summary>
        /// Total değeri her zaman Pair'in sağındaki currency cinsindendir
        /// Ask * AskAmount veya Bid * BidAmount
        /// </summary>
        public decimal DenominatorTotal { get; set; }
    }
}
