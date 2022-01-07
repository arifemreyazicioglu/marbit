using mdlTradeHelper = TradeAbstractions.Models.TradeHelper;
using absHlpr = TradeAbstractions.Helpers;
using System.Threading.Tasks;

namespace TradeAbstractions.Interfaces
{
    /// <summary>
    /// Engine tarafında create edilecek. Borsa tarafından tetiklenecek fonksiyonlar kullanılacak.
    /// </summary>
    public interface IToEngine
    {
        void PrintMessageEvent(string msg, bool anyError = false);

        void UserLoginResultEvent();
        void BalanceUpdatedEvent();
        /// <summary>
        /// Emrimizin tahtaya yerleştiği bilgisi alınır. Emir henüz eşleşmiş değildir. 
        /// Şimdilik bu event ile hiç bir işimiz yoktur.
        /// </summary>
        void OrderInsertEvent();
        void OrderDeleteEvent();
        void OrderUpdateEvent();
    }
}