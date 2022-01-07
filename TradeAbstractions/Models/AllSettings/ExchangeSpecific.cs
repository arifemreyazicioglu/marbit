using System.Collections.Generic;
using TradeAbstractions.Models.AllSettings.ExchangeSpecificModels;
using specificModels = TradeAbstractions.Models.AllSettings.ExchangeSpecificModels;

namespace TradeAbstractions.Models.AllSettings
{
    public class ExchangeSpecific
    {
        public specificModels.BtcTurk BtcTurk { get; set; }

        public specificModels.Binance Binance { get; set; }

    }
}
