using System;
using System.Collections.Generic;
using TradeAbstractions.Models.TradeHelper.FromEngine;
using absMdl = TradeAbstractions.Models;
using absHelpers = TradeAbstractions.Helpers;
namespace TradeAbstractions.Models.Engine
{
    public class TriangleOrders
    {
        /// <summary>
        /// Hangi Base ile işleme başladık?
        /// Detaylar Opportunity içinde var ama kolaylık olsun.
        /// </summary>
        public string Base { get; set; }
        /// <summary> </summary>
        public List<OrderModel.OrderRequestModel> MarketOrderRequests { get; set; }
        public List<OrderModel.OrderResponseModel> MarketOrderResponses { get; set; }
        /// <summary> İşlem bittikten sonra backend tarafında UTC olarak set ederiz. </summary>
        public DateTime TriangleOrdersEventTime { get; set; }
        /// <summary> Engine çalıştığından beri yaptığımız üçgen arbitraj sayısıdır. </summary>
        public int TriangleArbitrageCount { get; set; }
        /// <summary>
        /// Triangle Arbitrage tamamlandıktan sonra hesaplanır. Acaba nasıl bir oranda işlem tamamlandı?
        /// FinalRatio.Ratio yolda bıraktığı kırıntıları bırakmıyormuş gibi hesaplama yapar.
        /// Aslında kırıntılar bizim hesabımızda kalan diğer currency cinsinden küsüratlardır. Yani yine bizimdir.
        /// </summary>
        public absMdl.RatioModel FinalRatio { get; set; }
        /// <summary>
        /// Triangle Arbitrage tamamlandıktan sonra base currency'nin değerinin işleme sadece giriş ve çıkış miktarları ile hesaplanır.
        /// Yani içinden komisyon çıkarılmış net bir orandır.
        /// FinalRatio.Ratio ile aynı değeri vermesi beklenir. Ancak farklı olarak yolda kaybedilen kırıntılar hesaplamada yoktur.
        /// Ayrıca bu oran üçgen arbitrajın yolda bıraktığı kırıntılardan sonra base currency cinsinden son orandır.
        /// </summary>
        public decimal BaseCurrencyRatioFromPrice { get; set; }
        /// <summary>
        /// FinalRatio.Ratio - RealRatioFromPrice kadardır.
        /// Üçgen arbitrajın bittikten sonra başlangıçtan beri kırıntı olarak giden oran.
        /// </summary>
        /// <example>
        /// Base: USDT, USDTTRY ETHTRY ETHUSDT
        /// Yukarıdaki arbitrajda bir miktar para TRY ve ETH'da kalır. Buna kırıntı denir.
        /// Hesaplanan değerlerin son ondalık hanesi aşağı yuvarlanarak işlem yapılır. Yuvarlanan kısımdan küçük bir miktar
        /// o currency içinde bakiye olarak kalır ancak çıkış bakiyesine ulaşamaz.
        /// </example>
        public decimal CrumbRatio { get; set; }
        /// <summary> Base1 curreny ile işleme toplam ne kadarlık bir bakiye ile girildi. </summary>
        public decimal InputTotalGross { get; set; }
        /// <summary> Base1 curreny ile işlemden toplam ne kadarlık bir bakiye ile çıkıldı. </summary>
        public decimal OutputTotalNet { get; set; }
        /// <summary> Base1 curreny ile işlem sonundaki kâr miktarı. Eksi veya artı sonuç görebiliriz. </summary>
        public decimal NetProfit { get; set; }
        /// <summary>
        /// Yapılan işlem kârlı bir şekilde mi gerçekleşti?
        /// NetProfit set edildikten sonra burası set edilir.
        /// </summary>
        public bool ProfitableTrade { get; set; }
        public List<absHelpers.ResponseStatsWs> RespWsList { get; set; }
    }
}
