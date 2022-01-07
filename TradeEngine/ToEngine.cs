using System;
using System.Threading.Tasks;
using absInt = TradeAbstractions.Interfaces;
using mdlTradeHelper = TradeAbstractions.Models.TradeHelper;
using absHlpr = TradeAbstractions.Helpers;
using helpMe = TradeEngine.Models.HelpMe;
using mdlEngine = TradeAbstractions.Models.Engine;
using System.Collections.Generic;
using TradeAbstractions.Helpers;
using TradeAbstractions.Models.TradeHelper.FromEngine;

namespace TradeEngine
{
    public class ToEngine : absInt.IToEngine
    {
        public void BalanceUpdatedEvent()
        {
            throw new NotImplementedException();
        }

        public void OrderDeleteEvent()
        {
            throw new NotImplementedException();
        }

        public void OrderInsertEvent()
        {
            // Şimdilik kullanmaya gerek yok. Burada dursun belki ileride lazım olur.
        }

        public void OrderUpdateEvent()
        {
            throw new NotImplementedException();
        }

        public void UserLoginResultEvent()
        {
        }

        public void PrintMessageEvent(string msg, bool anyError = false)
        {
            helpMe.TradeEvents.PrintMessageEvent(msg, anyError);
        }
    }
}