using System.Collections.Generic;
using TradeAbstractions.Helpers;

namespace TradeAbstractions.Models.TradeHelper.FromEngine
{
    public class OrderModel
    {
        public OrderRequestModel OrderRequest { get; set; }
        public OrderResponseModel OrderResponse { get; set; }
        public class OrderRequestModel
        {
            public string PairSymbol { get; set; }
            /// <summary> Market order için ihtiyaç yok ama yine de kayıtlarımızda bulunması açısından girelim </summary>
            public decimal Price { get; set; }
            public bool IsBuyOrder { get; set; }
            /// <summary>
            ///  Borsaya göre ya bu kullanılır ya DenominatorTotal. Ancak ikisini de set edelim.
            ///  Henüz request'te bulunduğumuz bedelin içinden komisyon düşülmediği için gross denilmiştir.
            ///  1000USDT'lik satış yap dediğimizde 998UDT'lik currency satabilir ve 2USDT komisyona gidebilir.
            ///  Yine de market emirlerinin response'larındaki değerlerin Net olanlarını set etmeliyiz.
            ///  Response'a göre elimizde olan para net olan paradır çünkü.
            /// </summary>
            public decimal NumeratorAmountGross { get; set; }
            /// <summary>
            ///  Borsaya göre ya bu kullanılır ya NumeratorAmount. Ancak ikisini de set edelim.
            ///  1000TL'lik alım yap dediğimizde 998TL'lik currency alabilir ve 2TL komisyona gidebilir.
            ///  Yine de market emirlerinin response'larındaki değerlerin Net olanlarını set etmeliyiz.
            ///  Response'a göre elimizde olan para net olan paradır çünkü.
            /// </summary>
            public decimal DenominatorTotalGross { get; set; }
            /// <summary>
            /// MetaTrader'daki magic number mantığı. Açtığımız emire bize özel bir id tanımlamış oluruz.
            /// </summary>
            public string OrderClientId { get; set; }
            /// <summary>
            /// Komisyon ile kontrol sağlanması gereken durumlarda kullanılacak.
            /// BtcTurk'ün User Transaction servisinden doğru veri gelme kontrolünü yaparız.
            /// 0.0018 vs şeklinde bir değerdir.
            /// </summary>
            public decimal CommissionRatio { get; set; }
            /// <summary> Girilen fiyat noktadan sonra kaç ondalık değer kabul ediyor </summary>
            public int PriceScale { get; set; }
            /// <summary> Sol kısım noktadan sonra kaç ondalık değer kabul ediyor </summary>
            public int NumeratorScale { get; set; }
            /// <summary> Sağ kısım noktadan sonra kaç ondalık değer kabul ediyor </summary>
            public int DenominatorScale { get; set; }
            /// <summary>
            /// Bu emir için kaç defa servera post attık? Mesela BtcTurk, WebSocket'ten bize sinyali veriyor ama emir verdiğimizde hata
            /// verebiliyor. Bu durumda yeniden emir vermek zorunda kalabiliyoruz. Toplam kaç defa emir vermişiz bunu sayalım.
            /// </summary>
            public int OrderCount { get; set; }
            /// <summary> Tüm request ve responseları barındırsın </summary>
            public List<RequestResponseStats> ReqRespList { get; set; }
        }
        public class OrderResponseModel
        {
            public long Id { get; set; }
            public string PairSymbol { get; set; }
            /// <summary>
            /// Gerçekleşen ortalama Fiyat hesaplanıp set edilir.
            /// BtcTurk için ikinci bir servise gitmek gerekiyor. Order servis reponse'unda gelmiyor.
            /// </summary>
            public decimal Price { get; set; }
            /// <summary> Commission içerir </summary>
            public decimal NumeratorAmountGross { get; set; }
            /// <summary> Commission içerir </summary>
            public decimal DenominatorTotalGross { get; set; }
            /// <summary>
            ///  Borsaya göre ya bu kullanılır ya DenominatorTotal. API Response'a göre olanlar set edilir.
            /// </summary>
            public decimal NumeratorAmountNet { get; set; }
            /// <summary>
            ///  Borsaya göre ya bu kullanılır ya NumeratorAmount. API Response'a göre olanlar set edilir.
            /// </summary>
            public decimal DenominatorTotalNet { get; set; }
            /// <summary>
            /// Komisyon miktarı
            /// BtcTurk = Math.Abs(Fee + Tax)
            /// Binance için direk bu değer var.
            /// </summary>
            public decimal Commission { get; set; }
            /// <summary>
            /// Commission değerinden hesaplanabilir.
            /// 0.0018 vs şeklinde bir değerdir.
            /// </summary>
            public decimal CommissionRatio { get; set; }
            /// <summary>
            /// MetaTrader'daki magic number mantığı. Açtığımız emire bize özel bir id tanımlamış oluruz.
            /// </summary>
            public string OrderClientId { get; set; }
            /// <summary>
            /// Mesela BtcTurk, WebSocket'ten bize sinyali veriyor ama emir verdiğimizde hata verebiliyor.
            /// Bu durumda yeniden emir vermek zorunda kalabiliyoruz. O zaman bu property'i true yapalım.
            /// </summary>
            public bool NeedReOrder { get; set; }
            public int RetryAfterMs { get; set; }
        }
    }
}
