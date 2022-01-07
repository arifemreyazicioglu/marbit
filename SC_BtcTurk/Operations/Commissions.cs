using System.Threading.Tasks;
using absSettings = TradeAbstractions.Helpers.Settings;
using System;
using TradeAbstractions.Models.TradeHelper.FromEngine;

namespace SC_BtcTurk.Operations
{
    internal class Commissions
    {
        internal TradeAbstractions.Helpers.ReturnTask<CommissionsModel> SetCommissions()
        {
            var retTask = new TradeAbstractions.Helpers.ReturnTask<CommissionsModel>
            {
                Data = new CommissionsModel(),
                Success = false
            };

            // Burayı çağırmadan önce zaten tüm ayarlar tekrar json dosyasından okunmuştu.
            var settings = absSettings.ExchangeSpecific.BtcTurk;

            string output = $"CryptoCryptoTaker: {settings.Commissions.CryptoCryptoTaker}" + Environment.NewLine;
            output += $"CryptoTryTaker: {settings.Commissions.CryptoTryTaker}" + Environment.NewLine;
            output += $"HighCommCurr: {settings.Commissions.HighCommissionCurrency}";

            retTask.Data.CommissionsForPrint = output;

            retTask.Success = true;
            return retTask;
        }
    }
}
