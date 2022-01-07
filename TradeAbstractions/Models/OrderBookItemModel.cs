using System;

namespace TradeAbstractions.Models
{
    public class OrderBookItemModel
    {
        /// <example>USDTTRY</example>
        public string Pair { get; set; }
        /// <summary> 
        /// Ask Fiyatı (Pair'in solundaki 1 adet currency'nin, sağ kısmındaki currency cinsinden bedeli)
        /// 0 index tahtada ilk sıradaki satış fiyatıdır.
        /// Denominator true ise ask alınır.
        /// Yani Base payda ise yüksek değer alınır.
        /// </summary>
        public decimal[] Ask { get; set; }
        /// <summary> 
        /// Bid Fiyatı (Pair'in solundaki 1 adet currency'nin, sağ kısmındaki currency cinsinden bedeli)
        /// 0 index tahtada ilk sıradaki alış fiyatıdır.
        /// Denominator false ise bid alınır.
        /// Yani Base pay ise düşük değer alınır.
        /// </summary>
        public decimal[] Bid { get; set; }
        /// <summary>
        /// Ask miktarı (Pair'in solundaki currency'den kaç adet var)
        /// </summary>
        public decimal[] AskAmount { get; set; }
        /// <summary>
        /// Bid miktarı (Pair'in solundaki currency'den kaç adet var)
        /// </summary>
        public decimal[] BidAmount { get; set; }
        /// <summary>
        /// Varsa UTC cinsinden set edilecek. BtcTurk bu bilgiyi gönderiyor.
        /// </summary>
        public DateTime Time { get; set; }
        public TradeAbstractions.Helpers.RequestResponseStats ReqResp { get; set; }
    }
}
