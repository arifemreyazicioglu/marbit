using System;
using System.Data.SQLite;
using absAllSettings = TradeAbstractions.Models.AllSettings;
using absSettings = TradeAbstractions.Helpers.Settings;
using helpMe = TradeEngine.Models.HelpMe;
using absHelper = TradeAbstractions.Helpers;
using Newtonsoft.Json;
using System.Data;

namespace TradeEngine.Operations
{
    internal static class Dbs
    {
        /// <summary>
        /// Geliştirme sırasında bulk logları buraya atalım. Tek tek kolonları ayarlamadan json
        /// olarak dataObj'den veriyi geçirebiliriz.
        /// </summary>
        internal static bool AddDevelopmentLog(string name, string msg = null, object dataObj = null, bool printLog = false)
        {
            string commandText = "INSERT INTO TriangleArbitrage (Name";

            commandText += msg == null ? "" : ", Msg";
            commandText += dataObj == null ? "" : ", Datas";

            commandText += ") VALUES (@Name";

            commandText += msg == null ? "" : ", @Msg";
            commandText += dataObj == null ? "" : ", @Datas";

            commandText += ")";

            using (var cmd = new SQLiteCommand(commandText, helpMe.Dbs.DevLogConn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                if (msg != null)
                    cmd.Parameters.AddWithValue("@Msg", msg);
                if (dataObj != null)
                    cmd.Parameters.AddWithValue("@Datas", JsonConvert.SerializeObject(dataObj));

                try
                {
                    int count = cmd.ExecuteNonQuery();

                    if (printLog)
                        helpMe.TradeEvents.PrintMessageEvent("DevelopmentLog.db'ye kayıt girildi.");
                }
                catch (Exception ex)
                {
                    helpMe.TradeEvents.PrintMessageEvent("AddDevelopmentLog(), Exception: " + ex.ToString(), true);

                    return false;
                }
            }

            return true;
        }
    }
}
