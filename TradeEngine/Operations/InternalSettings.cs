using System.Threading.Tasks;
using helpMe = TradeEngine.Models.HelpMe;
using System;
using enumExchange = TradeAbstractions.Enums.EnumExchange;
using absSettings = TradeAbstractions.Helpers.Settings;
using absMdls = TradeAbstractions.Models;
using System.Collections.Generic;
using absHlpr = TradeAbstractions.Helpers;

namespace TradeEngine.Operations
{
    public class InternalSettings
    {
        internal static async Task<bool> SetUserBalance()
        {
            foreach (var item in Enum.GetValues<enumExchange>())
            {
                var exchange = Statics.TradeHelpers[item];

                // Enumeration tanımı olsa da create edilmemiş olabilir. Öyle bir durumda hata almamak için es geçelim.
                if (exchange == null) continue;

                if (absSettings.EngineSettings.IsProd)
                {
                    var userBalance = await exchange.FromEngine.GetUserBalance();
                    if (!userBalance.Success)
                    {
                        helpMe.TradeEvents.PrintMessageEvent(userBalance.Message, anyError: true);
                        return false;
                    }

                    exchange.UserBalance = userBalance.Data;
                }
                // Test ise balance verisini manuel olarak set edelim.
                else
                {
                    absMdls.TradeHelper.FromEngine.UserBalanceModel userBalanceModel = new() { BalanceList = new() };

                    userBalanceModel.BalanceList.Add(
                        new absMdls.BalanceItemModel
                        {
                            Asset = "TRY",
                            Balance = 1000000,
                            Free = 1000000
                        }
                    );

                    userBalanceModel.BalanceList.Add(
                        new absMdls.BalanceItemModel
                        {
                            Asset = "USDT",
                            Balance = 100000,
                            Free = 100000
                        }
                    );

                    

                    exchange.UserBalance = userBalanceModel;
                }
            }

            helpMe.TradeEvents.BalanceUpdatedEvent();

            return true;
        }
    }
}
