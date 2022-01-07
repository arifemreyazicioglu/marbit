using TradeAbstractions.Models.AllSettings;
namespace TradeAbstractions.Helpers
{
    public static class Settings
    {
        /// <summary>
        /// Tüm sistem için kullanılacak genel ayarları barındırır.
        /// </summary>
        public static EngineSettings EngineSettings { get; set; }
        public static ExchangeSpecific ExchangeSpecific { get; set; }
        /// <summary>
        /// Bir json dosyasından okuma yapılmaz.
        /// Engine içinden set edilip kullanılacak tüm ayarları barındırır.
        /// </summary>
        public static InternalSettings InternalSettings { get; set; }
    }
}
