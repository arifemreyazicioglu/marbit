using System.IO;
using System.Text.Json;
using absSettings = TradeAbstractions.Helpers.Settings;
using absAllSettings = TradeAbstractions.Models.AllSettings;

namespace TradeEngine
{
    public class Settings
    {
        /// <summary>
        /// Tüm ayarları json dosyalarından set ettiğimiz fonksiyon.
        /// </summary>
        public void SetAllSettings()
        {
            // Json dosyası yoktur. İçeride kullanacağımız static bir değişkendir.
            absSettings.InternalSettings = new absAllSettings.InternalSettings();

            // Engine ile ilgili ayarlar.
            string engineJson = File.ReadAllText("AllSettings/Engine.json");
            absSettings.EngineSettings = JsonSerializer.Deserialize<absAllSettings.EngineSettings>(engineJson);
            var engineSett = absSettings.EngineSettings;

            // Sadece borsaya özel ayarlar.
            SetExchangeSpecificSettings();
        }
        void SetExchangeSpecificSettings()
        {
            absSettings.ExchangeSpecific = new absAllSettings.ExchangeSpecific();

            string exchangeSpecificJson = File.ReadAllText("AllSettings/BtcTurk.json");
            absSettings.ExchangeSpecific.BtcTurk = JsonSerializer.Deserialize<absAllSettings.ExchangeSpecificModels.BtcTurk>(exchangeSpecificJson);

            string exchangeSpecificJson1 = File.ReadAllText("AllSettings/Binance.json");
            absSettings.ExchangeSpecific.Binance = JsonSerializer.Deserialize<absAllSettings.ExchangeSpecificModels.Binance>(exchangeSpecificJson1);


            // SCTODO Binance için de deserialize yapılacak
        }
    }
}
