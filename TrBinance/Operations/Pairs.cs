using System.Collections.Generic;
using System.Threading.Tasks;
using absHlpr = TradeAbstractions.Helpers;
using absSettings = TradeAbstractions.Helpers.Settings;
using specificModels = TradeAbstractions.Models.AllSettings.ExchangeSpecificModels;
using absMdl = TradeAbstractions.Models;
using TradeAbstractions.Models.TradeHelper.FromEngine;
namespace TrBinance.Operations
{
    internal class Pairs
    {
        /// <summary>
        /// Create possible path oluşturulurken kullanılacak.
        /// </summary>
        /// <returns>
        /// İstisna olarak: Task.Data == null veya Task.Data.PairList.Count == 0 olabilir
        /// </returns>
        internal async Task<absHlpr.ReturnTask<PairsModel>> FillRawPairs()
        {
            absHlpr.ReturnTask<PairsModel> retTask = new()
            {
                Data = new PairsModel() { PairList = new List<absMdl.PairRawModel>() },
                Success = false
            };

            //Models.ReturnModel<Models.ExchangeInfoModel> resp = await GetExchangeInfo();
            //retTask.Data.ReqResp = resp.ReqResp;

            //// Hata varsa mesajı yazalım ve işlemi sonlandıralım
            //if (resp.NotSuccessOrCustomMessage)
            //{
            //    retTask.Message = resp.GetApiOrCustomMessage;
            //    return retTask;
            //}

            //specificModels.BtcTurk settings = absSettings.ExchangeSpecific.BtcTurk;

            //// Pairs listesini doldurmaya başlayalım
            //int limit = resp.Data.Symbols.Count;
            //int filterCount, orderMethodsCount;
            //Models.ExchangeInfoModel.SymbolModel item;
            //decimal minDenominatorValue;
            //bool hasMarketOrder;
            //for (int i = 0; i < limit; i++)
            //{
            //    item = resp.Data.Symbols[i];
            //    if (item.Status != "TRADING") continue;

            //    minDenominatorValue = 0;
            //    hasMarketOrder = false;

            //    decimal commissionRatio;
            //    if (item.CommissionFromNumerator)
            //        commissionRatio = item.Numerator == settings.Commissions.HighCommissionCurrency ? settings.Commissions.CryptoTryTaker : settings.Commissions.CryptoCryptoTaker;
            //    else
            //        commissionRatio = item.Denominator == settings.Commissions.HighCommissionCurrency ? settings.Commissions.CryptoTryTaker : settings.Commissions.CryptoCryptoTaker;

            //    filterCount = item.Filters.Count;
            //    for (int j = 0; j < filterCount; j++)
            //    {
            //        if (item.Filters[j].FilterType == "PRICE_FILTER")
            //        {
            //            minDenominatorValue = item.Filters[j].MinExchangeValue;
            //            break;
            //        }
            //    }

            //    orderMethodsCount = item.OrderMethods.Count;
            //    for (int k = 0; k < orderMethodsCount; k++)
            //    {
            //        if (item.OrderMethods[k] == "MARKET")
            //        {
            //            hasMarketOrder = true;
            //            break;
            //        }
            //    }

            //    // Market order desteklemiyorsa üçgen arbitraj yapamayız
            //    if (!hasMarketOrder) continue;

            //    retTask.Data.PairList.Add(new absMdl.PairRawModel
            //    {
            //        Pair = item.Name,
            //        Numerator = item.Numerator,
            //        Denominator = item.Denominator,
            //        CommissionRatio = commissionRatio,
            //        DenominatorScale = item.HasFraction ? item.DenominatorScale : 0,
            //        NumeratorScale = item.NumeratorScale,
            //        MinDenominatorValue = minDenominatorValue
            //    });
            //}

            retTask.Success = true;
            return retTask;
        }
        /// <summary> ExchangeInfo servisinden verileri çekip Pairs listesini oluşturalım </summary>
        /// <returns> Geriye null dönmez. </returns>
        //async Task<Models.ReturnModel<Models.ExchangeInfoModel>> GetExchangeInfo()
        //{
        //    return await Statics.ScHttp.SendRequest<Models.ExchangeInfoModel>(
        //        "api/v2/server/exchangeinfo",
        //        ScHttp.Methods.Get,
        //        "GetExchangeInfo"
        //        );
        //}
    }
}
