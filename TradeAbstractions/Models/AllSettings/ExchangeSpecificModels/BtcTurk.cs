namespace TradeAbstractions.Models.AllSettings.ExchangeSpecificModels
{
    public class BtcTurk
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
