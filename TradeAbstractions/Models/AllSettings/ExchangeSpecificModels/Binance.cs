using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAbstractions.Models.AllSettings.ExchangeSpecificModels
{
    public class Binance
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public CommissionsModel Commissions { get; set; }
        public class CommissionsModel
        {
            public string HighCommissionCurrency { get; set; }
            public decimal CryptoCryptoMaker { get; set; }
            public decimal CryptoCryptoTaker { get; set; }
            public decimal CryptoTryMaker { get; set; }
            public decimal CryptoTryTaker { get; set; }
        }
    }
}
