using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using absHlpr = TradeAbstractions.Helpers;
using absTimerHelper = TradeAbstractions.Helpers.TimeHelper;
using TradeAbstractions.Models.TradeHelper.FromEngine;
namespace SC_BtcTurk.Operations
{
    internal class Ticker
    {
        internal async Task<absHlpr.ReturnTask<TickerModel>> CreateList()
        {
            absHlpr.ReturnTask<TickerModel> retTask = new()
            {
                Data = new TickerModel()
                {
                    AskLookup = new Dictionary<string, decimal>(),
                    BidLookup = new Dictionary<string, decimal>(),
                },
                Success = false
            };

            Models.ReturnModel<List<Models.Ticker>> resp = await GetTicker();
            retTask.Data.ReqResp = resp.ReqResp;

            // Hata varsa mesajı yazalım ve işlemi sonlandıralım
            if (resp.NotSuccessOrCustomMessage)
            {
                retTask.Message = resp.GetApiOrCustomMessage;
                return retTask;
            }

            if (resp.Data.Count == 0)
            {
                retTask.Message = "CreateList(), Ticker servisinden veri gelmedi!";
                return retTask;
            }

            // Ticker verilerini doldurmaya başlayalım
            int limit = resp.Data.Count;
            Models.Ticker item;
            for (int i = 0; i < limit; i++)
            {
                item = resp.Data[i];

                retTask.Data.AskLookup.Add(item.Pair, item.Ask);
                retTask.Data.BidLookup.Add(item.Pair, item.Bid);
            }

            retTask.Success = true;
            return retTask;
        }
        /// <summary> Ticker servisinden verileri çekip Ticker listesini oluşturalım </summary>
        async Task<Models.ReturnModel<List<Models.Ticker>>> GetTicker()
        {
            return await Statics.ScHttp.SendRequest<List<Models.Ticker>>(
                "api/v2/ticker",
                ScHttp.Methods.Get,
                "GetTicker"
                );
        }
    }
}
