using TradeAbstractions.Models.TradeHelper.FromEngine;
using absMdl = TradeAbstractions.Models;
namespace TradeAbstractions.Interfaces
{
    public interface ITradeEvents
    {
        /// <summary>
        /// Engine'i kullanan uygulamaya mesaj göndermek istediğimizde kullanırız.
        /// </summary>
        /// <param name="msg"> Set ettiğimiz mesaj. </param>
        /// <param name="anyError"> Bir hata var mı? </param>
        void PrintMessageEvent(string msg, bool anyError = false);
        /// <summary> User Balance bilgisi için servisten her veri alışımızda çağırılsın. </summary>
        void BalanceUpdatedEvent();
        /// <summary> Dolar ve USDT verileri okunduktan sonra UI'da gösterelim. </summary>
        void ParitiesUpdatedEvent();
        /// <summary>
        /// Oran hesaplamalarının yapıldığı ana algoritmanın işi bittiğinde çağıralım.
        /// Data grid buradan sonra doldurulacak.
        /// </summary>
        void RatiosCalculatedEvent();
    }
}