using System.Collections.Generic;
using System.Threading.Tasks;
using TradeAbstractions.Models.TradeHelper.FromEngine;
using absMdl = TradeAbstractions.Models;
namespace TradeAbstractions.Interfaces
{
    /// <summary>
    /// Borsa tarafında create edilecek. Engine tarafından tetiklenecek fonksiyonlar kullanılacak.
    /// </summary>
    public interface IFromEngine
    {
        /// <summary>
        /// GetRawPairs içerisinde bu fonksiyonda set edilen komisyonlar kullanılıyor.
        /// Yani, GetRawPairs öncesi muhakkak çalıştırılmalı.
        /// Komisyonlar her borsanın ExchangeSpecific içindeki kendi ayar dosyasına set edilir.
        /// </summary>
        Helpers.ReturnTask<CommissionsModel> SetCommissions();
        Task<Helpers.ReturnTask<PairsModel>> GetRawPairs();
        Task<Helpers.ReturnTask<TickerModel>> GetTickerList();
        Task<Helpers.ReturnTask<OrderBookModel>> GetOrderBookListForThreePairs(absMdl.PossiblePathModel possiblePath);
        /// <summary> Sadece Market Order gönderilir. </summary>
        Task<Helpers.ReturnTask<OrderModel.OrderResponseModel>> CreateOrder(OrderModel.OrderRequestModel orderRequest);
        Task<Helpers.ReturnTask<UserBalanceModel>> GetUserBalance();
    }
}
