using System.Collections.Generic;

namespace TradeAbstractions.Models.AllSettings
{
    public class SharedSettings
    {
        public string ServerUrl { get; set; }
        /// <summary>
        /// Biraz ters mantık. Dakikada 300 adet order verebiliyorsak saniyede 5 adet olur.
        /// Bu da 1 order için teoride 200ms süre tanınıyor demektir. Engine içinde bekleme süresi hesabında kullanılır.
        /// BtcTurk bu hesaplamalara uymadığı ve Engine bundan dolayı patlamasın diye 750ms verildi.
        /// </summary>
        public int MsPerOrder { get; set; }
        public List<string> Currencies { get; set; }
    }
}
