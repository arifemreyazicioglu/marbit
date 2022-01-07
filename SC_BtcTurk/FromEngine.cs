using System.Collections.Generic;
using System.Threading.Tasks;
using TradeAbstractions.Helpers;
using absMdl = TradeAbstractions.Models;
using absInt = TradeAbstractions.Interfaces;
using TradeAbstractions.Models.TradeHelper.FromEngine;
using absSettings = TradeAbstractions.Helpers.Settings;
namespace SC_BtcTurk
{
    public class FromEngine : absInt.IFromEngine
    {
        Operations.Pairs PairsInstance { get; set; } = new Operations.Pairs();
        Operations.Ticker TickerInstance { get; set; } = new Operations.Ticker();
        Operations.OrderBook OrderInstance { get; set; } = new Operations.OrderBook();
        Operations.Order Order { get; set; } = new Operations.Order();
        Operations.UserBalance UserBalance { get; set; } = new Operations.UserBalance();
        Operations.ServerTimestamp ServerTimestamp { get; set; } = new Operations.ServerTimestamp();
        Operations.Commissions Commissions { get; set; } = new Operations.Commissions();
        /// <summary>
        /// Sık talepte bulunulacağı için her fonksiyon çağrısında instance oluşturmak yerine
        /// FromEngineModel create olurken bir kere oluştururuz.
        /// </summary>
        public FromEngine(absInt.ITradeHelper tradeHelper)
        {
            Statics statics = new();
            statics.SetAllProperties(tradeHelper);
        }
        public async Task<ReturnTask<OrderModel.OrderResponseModel>> CreateOrder(OrderModel.OrderRequestModel orderRequest)
        {
            return await Order.CreateOrder(orderRequest).ConfigureAwait(false);
        }
        public async Task<ReturnTask<PairsModel>> GetRawPairs()
        {
            return await PairsInstance.FillRawPairs().ConfigureAwait(false);
        }
        public async Task<ReturnTask<TickerModel>> GetTickerList()
        {
            return await TickerInstance.CreateList().ConfigureAwait(false);
        }
        /// <summary>
        /// Tahtadaki verileri çekelim
        /// </summary>
        /// <param name="possiblePath"></param>
        /// <returns></returns>
        public async Task<ReturnTask<OrderBookModel>> GetOrderBookListForThreePairs(absMdl.PossiblePathModel possiblePath)
        {
            var retTask = new ReturnTask<OrderBookModel> { Success = false };

            // Paralel olarak çağırma işlemi başlasın
            var item1Task = OrderInstance.GetOrderBookItem(possiblePath.PairPaths[0].Pair);
            var item2Task = OrderInstance.GetOrderBookItem(possiblePath.PairPaths[1].Pair);
            var item3Task = OrderInstance.GetOrderBookItem(possiblePath.PairPaths[2].Pair);

            // Tüm çağırılanlar beklesin (bekleme seri olsa da işlemler arkada paralel yürüyor)
            // En uzun süren işleme + paralel orkestrasyon için de süreyi eklersek toplam harcanan zaman oluyor
            var item1 = await item1Task;
            var item2 = await item2Task;
            var item3 = await item3Task;

            retTask.Data = new OrderBookModel
            {
                OrderBookList = new List<absMdl.OrderBookItemModel>() { item1.Data, item2.Data, item3.Data },
                HasTimePerPair = true
            };

            // Üç OrderBook sorgusu da hatasız ise geriye hata döndürmeyiz.
            retTask.Success = item1.Success && item2.Success && item3.Success;
            // Varsa hata mesajları geriye döndörelim
            if (!retTask.Success)
            {
                retTask.Message = "";
                if (!item1.Success)
                    retTask.Message += $"{possiblePath.PairPaths[0].Pair}: {item1.Message} ";
                if (!item2.Success)
                    retTask.Message += $"{possiblePath.PairPaths[1].Pair}: {item2.Message} ";
                if (!item3.Success)
                    retTask.Message += $"{possiblePath.PairPaths[2].Pair}: {item3.Message} ";
                if (retTask.Message != "")
                    retTask.Message = "GetOrderBookListForThreePairs " + retTask.Message;
            }

            return retTask;
        }
        public async Task<ReturnTask<UserBalanceModel>> GetUserBalance()
        {
            return await UserBalance.CreateList().ConfigureAwait(false);
        }
        public ReturnTask<CommissionsModel> SetCommissions()
        {
            return Commissions.SetCommissions();
        }
    }
}
