using System.Collections.Generic;
using System.Threading.Tasks;
using TradeAbstractions.Helpers;
using absMdl = TradeAbstractions.Models;
using absInt = TradeAbstractions.Interfaces;
using TradeAbstractions.Models.TradeHelper.FromEngine;
using absSettings = TradeAbstractions.Helpers.Settings;
using TradeAbstractions.Models;

namespace TrBinance
{
    public class FromEngine : absInt.IFromEngine
    {
        Operations.Pairs PairsInstance { get; set; } = new Operations.Pairs();
        Operations.Ticker TickerInstance { get; set; } = new Operations.Ticker();
        /// <summary>
        /// Sık talepte bulunulacağı için her fonksiyon çağrısında instance oluşturmak yerine
        /// FromEngineModel create olurken bir kere oluştururuz.
        /// </summary>
        public FromEngine(absInt.ITradeHelper tradeHelper)
        {
            Statics statics = new();
            statics.SetAllProperties(tradeHelper);
        }
        public async Task<ReturnTask<PairsModel>> GetRawPairs()
        {
            return await PairsInstance.FillRawPairs().ConfigureAwait(false);
        }
        public async Task<ReturnTask<TickerModel>> GetTickerList()
        {
            return await TickerInstance.CreateList().ConfigureAwait(false);
        }

        public ReturnTask<CommissionsModel> SetCommissions()
        {
            throw new System.NotImplementedException();
        }

        public Task<ReturnTask<OrderBookModel>> GetOrderBookListForThreePairs(PossiblePathModel possiblePath)
        {
            throw new System.NotImplementedException();
        }

        public Task<ReturnTask<OrderModel.OrderResponseModel>> CreateOrder(OrderModel.OrderRequestModel orderRequest)
        {
            throw new System.NotImplementedException();
        }

        public Task<ReturnTask<UserBalanceModel>> GetUserBalance()
        {
            throw new System.NotImplementedException();
        }
    }
}
