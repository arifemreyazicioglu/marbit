using System;
using System.Collections.Generic;
namespace TradeAbstractions.Models.Engine
{
    public class Possibility
    {
        public PossiblePathModel PossiblePath { get; set; }
        /// <summary> Komisyonlar çıkarılmadan önceki oran </summary>
        public decimal RatioRaw { get; set; }
        /// <summary> Komisyonlu oran </summary>
        public decimal Ratio { get; set; }
        /// <summary> 
        /// Ask1 Fiyatı (Pair1 in solundaki 1 adet currency'nin, sağ kısmındaki currency cinsinden bedeli)
        /// Tahtada ilk sıradaki satış fiyatıdır.
        /// Denominator1 true ise ask alınır.
        /// Yani Base1 payda ise yüksek değer alınır.
        /// </summary>
        public decimal Ask1 { get; set; }
        /// <summary>  Pair2 için yukarıda yazılanlar aynen geçerlidir. </summary>
        public decimal Ask2 { get; set; }
        /// <summary>  Pair3 için yukarıda yazılanlar aynen geçerlidir. </summary>
        public decimal Ask3 { get; set; }
        /// <summary> 
        /// Bid1 Fiyatı (Pair1 in solundaki 1 adet currency'nin, sağ kısmındaki currency cinsinden bedeli)
        /// Tahtada ilk sıradaki alış fiyatıdır.
        /// Denominator1 false ise bid alınır.
        /// Yani Base1 pay ise düşük değer alınır.
        /// </summary>
        public decimal Bid1 { get; set; }
        /// <summary>  Pair2 için yukarıda yazılanlar aynen geçerlidir. </summary>
        public decimal Bid2 { get; set; }
        /// <summary>  Pair3 için yukarıda yazılanlar aynen geçerlidir. </summary>
        public decimal Bid3 { get; set; }
        /// <summary>
        /// Ticker servisinden time verisi gelirse Time1,2,3 set edilecek.
        /// </summary>
        public bool HasTimePerPair { get; set; }
        /// <summary> Pair1 için UTC cinsinden </summary>
        public DateTime Time1 { get; set; }
        /// <summary> Pair2 için UTC cinsinden </summary>
        public DateTime Time2 { get; set; }
        /// <summary> Pair3 için UTC cinsinden </summary>
        public DateTime Time3 { get; set; }
        /// <summary>
        /// WebSocket OrderBook'tan beslendiğinde tüm ihtimaller geriye liste olarak dönüldüğünden bu property dikkate alınmaz. Set edilmez.
        /// </summary>
        public bool HasPossibility { get; set; }
        public Helpers.RequestResponseStats ReqResp { get; set; }
    }
}
