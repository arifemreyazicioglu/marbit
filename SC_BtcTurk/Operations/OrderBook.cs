using System.Threading.Tasks;
using absHlpr = TradeAbstractions.Helpers;
using absMdl = TradeAbstractions.Models;

namespace SC_BtcTurk.Operations
{
    public class OrderBook
    {
        /// <summary>
        /// Bir adet pair için tahtadan veri çekelim.
        /// </summary>
        /// <param name="pair"> Örn: USDTTRY </param>
        /// <param name="limit"> Tahtadan kaç veri alınsın. Max: 1000 </param>
        /// <returns></returns>
        public async Task<absHlpr.ReturnTask<absMdl.OrderBookItemModel>> GetOrderBookItem(string pair, int limit = 10)
        {
            absHlpr.ReturnTask<absMdl.OrderBookItemModel> retTask = new()
            {
                Data = new absMdl.OrderBookItemModel { Pair = pair },
                Success = false
            };

            Models.ReturnModel<Models.OrderBook> resp = await GetOrderBook(pair, limit: limit);
            retTask.Data.ReqResp = resp.ReqResp;

            // Hata varsa mesajı yazalım ve işlemi sonlandıralım
            if (resp.NotSuccessOrCustomMessage)
            {
                retTask.Message = resp.GetApiOrCustomMessage;
                return retTask;
            }

            absMdl.OrderBookItemModel orderBookItem = retTask.Data;
            orderBookItem.Time = absHlpr.TimeHelper.TimestampToUtcDateTime(resp.Data.TimeStamp);

            int askCount = resp.Data.Asks.GetLength(0);
            int bidCount = resp.Data.Bids.GetLength(0);

            if (askCount > 0)
            {
                orderBookItem.Ask = new decimal[askCount];
                orderBookItem.AskAmount = new decimal[askCount];

                for (int i = 0; i < askCount; i++)
                {
                    orderBookItem.Ask[i] = resp.Data.Asks[i, 0];
                    orderBookItem.AskAmount[i] = resp.Data.Asks[i, 1];
                }
            }

            if (bidCount > 0)
            {
                orderBookItem.Bid = new decimal[bidCount];
                orderBookItem.BidAmount = new decimal[bidCount];

                for (int i = 0; i < bidCount; i++)
                {
                    orderBookItem.Bid[i] = resp.Data.Bids[i, 0];
                    orderBookItem.BidAmount[i] = resp.Data.Bids[i, 1];
                }
            }

            retTask.Success = true;
            return retTask;
        }
        /// <summary> OrderBook servisinden verileri çekelim. </summary>
        async Task<Models.ReturnModel<Models.OrderBook>> GetOrderBook(string pair, int limit = 1)
        {
            return await Statics.ScHttp.SendRequest<Models.OrderBook>(
             $"api/v2/orderbook?pairSymbol={pair}&limit={limit}",
             ScHttp.Methods.Get,
             "GetOrderBook"
             );
        }
    }
}
