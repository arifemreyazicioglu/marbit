using System;
using System.Collections.Generic;
using TradeAbstractions.Models.TradeHelper.FromEngine;

namespace TradeAbstractions.Models.Engine
{
    public class Opportunity
    {
        public Engine.Possibility Possibility { get; set; }
        public OrderBookModel OrderBook { get; set; }
        /// <summary> Mevcut durumda tahtadaki fiyat ve miktardan 1,2 ve 3. pairler hesaplanır. </summary>
        public List<OrderPriceModel> ActualPrices { get; set; }
        /// <summary> 3 parite baz alınarak en küçük toplam emire göre 1,2 ve 3. pairler şekillenir. </summary>
        public List<OrderPriceModel> IntersectionPrices { get; set; }
        /// <summary>
        /// Trade edeceğimiz price değeridir. Yani sermayemiz de artık hesaba katılmıştır. 
        /// Market emri için hesaplanmış değerleri içerir. Artık emir vermeye hazırız.
        /// 1,2 ve 3. pairleri barındırır.
        /// </summary>
        public List<OrderPriceModel> FinalPrices { get; set; }
        public bool HasOpportunity { get; set; }
        /// <summary> İşlem bittikten sonra backend tarafında UTC olarak set ederiz. </summary>
        public DateTime OpportunityEventTime { get; set; }
        /// <summary> Engine çalıştığından beri yakaladığımız fırsatları saydırırız. </summary>
        public int OpportunityCount { get; set; }
        /// <summary>
        /// Hangi seçenekler fırsat vaadetti?
        /// </summary>
        public List<ModeModel> OrderBookModes { get; set; }
        /// <summary> Bu fırsat hangi mod'a göre yakalandı. </summary>
        public ModeModel OrderBookModeSelected { get; set; }
         /// <summary> Possibility ile karşılaştırma yapacağımız 0 0 0 indexlerine ait oran. </summary>
        public RatioModel ZeroIndexRatio { get; set; }
        /// <summary>
        /// Index'i 0 olanları sayar.
        /// İşleme girmeden önce karar aşamasında kullanılır.
        /// </summary>
        /// <example>
        /// 0 0 1 için 2'dir. 0 0 0 için 3'tür. 2 1 4 için 0'dır.
        /// </example>
        public int ZeroIndexCount { get; set; }
        /// <summary>
        /// Seçili index değerlerini toplar.
        /// </summary>
        /// <example>
        /// 0 0 1 için 1'dir. 1 1 2 için 4'tür. 2 1 4 için 7'dir.
        /// </example>
        public int SumIndexes { get; set; }
        /// <summary>
        /// Üçgen arbitraj yapacak mıyız kararını bu değişken gösteriyor.
        /// </summary>
        public bool HasTradeDecision { get; set; }
    }
}
