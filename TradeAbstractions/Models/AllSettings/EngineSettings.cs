using System.Collections.Generic;

namespace TradeAbstractions.Models.AllSettings
{
    public class EngineSettings
    {
        /// <summary>
        /// true: balance verileri serverdan alınır ve sistem Trade edebilir.
        /// false: 1,000,000USDT ve 10,000,000TRY test parasına göre fırsatları yakalar ancak trade etmez. 
        /// </summary>
        public bool IsProd { get; set; }
        /// <summary>
        /// Trade işlemlerine girmeden önce son izni almak için kullanacağımız yüzdelik değer.
        /// Örn, %2 için: 0.02
        /// </summary>
        public decimal LimitPercentage { get; set; }
        public Engine.EmailSettings EmailSettings { get; set; }
        public Dictionary<Enums.EnumExchange, SharedSettings> Shared { get; set; }
    }
}
