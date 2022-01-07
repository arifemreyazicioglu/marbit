using System;
using System.Threading.Tasks;
using absHlpr = TradeAbstractions.Helpers;
using Newtonsoft.Json;
using TradeAbstractions.Models.TradeHelper.FromEngine;

namespace SC_BtcTurk.Operations
{
    internal class ServerTimestamp
    {
        internal async Task<absHlpr.ReturnTask<ServerTimestampModel>> GetServerTimestamp()
        {
            var retTask = new absHlpr.ReturnTask<ServerTimestampModel>
            {
                Data = new ServerTimestampModel(),
                Success = false
            };

            Models.ReturnModel<Models.ServerTimeModel> resp = await ApiServerTime().ConfigureAwait(false);
            retTask.Data.ReqResp = resp.ReqResp;

            // Hata varsa mesajı yazalım ve işlemi sonlandıralım
            if (resp.NotSuccessOrCustomMessage)
            {
                retTask.Message = resp.GetApiOrCustomMessage;
                return retTask;
            }

            retTask.Data.OurServerStartTime = resp.ReqResp.StartTime;
            retTask.Data.ApiServerTime = resp.Data.ServerTime2;
            retTask.Data.OurServerEndTime = resp.ReqResp.EndTime;
            retTask.Data.ElapsedMs = resp.ReqResp.ElapsedMs;

            retTask.Success = true;
            return retTask;
        }
        /// <summary> Ticker servisinden verileri çekip Ticker listesini oluşturalım </summary>
        async Task<Models.ReturnModel<Models.ServerTimeModel>> ApiServerTime()
        {
            // Normalde burada direk return kullanmamız gerekirdi.
            Models.ReturnModel<Models.ServerTimeModel> respTask = await Statics.ScHttp.SendRequest<Models.ServerTimeModel>(
              "api/v2/server/time",
              ScHttp.Methods.Get,
              "ApiServerTime"
              ).ConfigureAwait(false);

            // Bu fonksiyona özel BtcTurk'ün Code,Data vs yapısını manuel oluşturacağız.
            // Başvurduğumuz serviste BtcTurk kendi standardını bozduğu için bu işlem yapılıyor.
            if (respTask.ReqResp.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    respTask.Data = JsonConvert.DeserializeObject<Models.ServerTimeModel>(respTask.ReqResp.Resp);
                }
                catch (Exception ex)
                {
                    respTask.CustomMessage += "ApiServerTime, Exception: " + ex.ToString();
                    return respTask;
                }
                respTask.Success = true;
            }
            else
            {
                respTask.CustomMessage += $"ApiServerTime, Code:{respTask.ReqResp.HttpStatusCode}";
            }

            return respTask;
        }
    }
}
