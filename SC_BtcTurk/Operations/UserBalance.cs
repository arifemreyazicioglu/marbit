using System.Collections.Generic;
using System.Threading.Tasks;
using absHlpr = TradeAbstractions.Helpers;
using absMdls = TradeAbstractions.Models;
using TradeAbstractions.Models.TradeHelper.FromEngine;
namespace SC_BtcTurk.Operations
{
    internal class UserBalance
    {
        internal async Task<absHlpr.ReturnTask<UserBalanceModel>> CreateList()
        {
            var retTask = new absHlpr.ReturnTask<UserBalanceModel>
            {
                Data = new UserBalanceModel(),
                Success = false
            };

            Models.ReturnModel<List<Models.UserBalance>> resp = await GetUserBalance();
            retTask.Data.ReqResp = resp.ReqResp;

            // Hata varsa mesajı yazalım ve işlemi sonlandıralım
            if (resp.NotSuccessOrCustomMessage)
            {
                retTask.Message = resp.GetApiOrCustomMessage;
                return retTask;
            }

            if (resp.Data.Count == 0)
            {
                retTask.Message = "CreateList(), UserBalance servisinden veri gelmedi!";
                return retTask;
            }

            List<absMdls.BalanceItemModel> balanceList = new();
            retTask.Data.BalanceList = balanceList;

            // Verileri dolduralım.
            int limit = resp.Data.Count;
            Models.UserBalance item;
            for (int i = 0; i < limit; i++)
            {
                item = resp.Data[i];

                balanceList.Add(new absMdls.BalanceItemModel
                {
                    Asset = item.Asset,
                    Balance = item.Balance,
                    Free = item.Free,
                    Locked = item.Locked
                });
            }

            retTask.Success = true;
            return retTask;
        }
        async Task<Models.ReturnModel<List<Models.UserBalance>>> GetUserBalance()
        {
            return await Statics.ScHttp.SendRequest<List<Models.UserBalance>>(
              "api/v1/users/balances",
              ScHttp.Methods.Get,
              "GetUserBalance",
              needAuthentication: true
              );
        }
    }
}
