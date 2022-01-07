using absSettings = TradeAbstractions.Helpers.Settings;
using absInt = TradeAbstractions.Interfaces;
using absMdl = TradeAbstractions.Models;
using System.Data.SQLite;
using System;
using enumExchange = TradeAbstractions.Enums.EnumExchange;
namespace TradeEngine.Models
{
    public static class HelpMe
    {
        /// <summary> SetHelper fonksiyonu ile set ederiz ve proje boyunca kullanırız. </summary>
        internal static absInt.ITradeEvents TradeEvents { get; private set; }
        internal static absMdl.Utils.Dbs Dbs { get; private set; }
        public static EmailModel Email { get; private set; }
        /// <summary> Yukarıdaki property'ler kullanılmadan önce SetHelper çağırılmalı. </summary>
        /// <param name="msg"> Geriye döneceğimiz uyarı için parametre. </param>
        internal static bool SetHelper(absInt.ITradeEvents tradeEvents)
        {
            TradeEvents = tradeEvents;

            // Sadece bir kere create edelim..
            if (Statics.TradeHelpers == null)
            {
                Statics.TradeHelpers = new System.Collections.Generic.Dictionary<enumExchange, TradeHelper>();
                absInt.IToEngine toEngine = new ToEngine();

                #region Btcturk
                var tradeHelperBtcTurk = new TradeHelper();
                absInt.IFromEngine fromEngineBtcTurk = new SC_BtcTurk.FromEngine(tradeHelperBtcTurk);
                tradeHelperBtcTurk.SetFromTo(fromEngineBtcTurk, toEngine);

                Statics.TradeHelpers.Add(enumExchange.BtcTurk, tradeHelperBtcTurk);
                #endregion

                #region TrBinance
                //// SCTODO, Engine için tek yapılacak şey bu alttaki satırların commentleri kaldırılacak.
                ////kalan işler TrBinance library'si içinde yapılacak.
                //var tradeHelperTrBinance = new TradeHelper();
                //absInt.IFromEngine fromEngineTrBinance = new TrBinance.FromEngine(tradeHelperTrBinance);
                //tradeHelperTrBinance.SetFromTo(fromEngineTrBinance, toEngine);

                //Statics.TradeHelpers.Add(enumExchange.TrBinance, tradeHelperTrBinance);
                Statics.TradeHelpers.Add(enumExchange.TrBinance, null);
                #endregion
            }

            if (!SetDbsAndConnectionsOpen())
                return false;

            // Email kısmını set edelim
            Email = new TradeEngine.Models.EmailModel();

            return true;
        }
        /// <summary>
        /// SetHelper fonksiyonu ile set ederiz ve proje boyunca kullanırız.
        /// Db'leri static olarak set edelim ve connectionları sürekli açık bırakalım
        /// Amacımız veritabanı ile iletişimde gecikme yaşanmaması
        /// Web veya API tabanlı bir projemiz olsa bu şekilde kullanamazdık!
        /// </summary>
        static bool SetDbsAndConnectionsOpen()
        {
            if (Dbs == null) Dbs = new TradeAbstractions.Models.Utils.Dbs();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var cbDevelopmentLog = new SQLiteConnectionStringBuilder { DataSource = baseDirectory + "Dbs\\DevelopmentLog.db" };
            Dbs.DevLogConn = new SQLiteConnection(cbDevelopmentLog.ConnectionString);

            try
            {
                Dbs.DevLogConn.Open();
            }
            catch (Exception ex)
            {
                Dbs.DevLogConn.Close();

                TradeEvents.PrintMessageEvent("SetDbsAndConnectionsOpen(), Exception: " + ex.ToString(), true);

                return false;
            }

            return true;
        }
    }
}
