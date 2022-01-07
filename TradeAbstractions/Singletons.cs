using System;
using System.Collections.Generic;

namespace TradeAbstractions
{
    /// <summary>
    /// Multithreaded Singleton class'ımız.
    /// Kendi instance'ı volatile olduğu için içindeki tüm değerlere farklı thread'lerden ulaşılabilir.
    /// </summary>
    public sealed class Singletons
    {
        /// <summary>
        /// Performanslı sorgulama için bu yapıyı kullanacağız. Path'ler veri tekrarı yapmış oluyor.
        /// Ancak amacımız veriye en az maliyetle ulaşmak olduğundan sorun değil.
        /// Hem program başlangıcında ve arada bir CreatePossiblePaths servisi kullanıldığından create'deki zaman kaybı da çok problem değil.
        /// Key örnek1: BTCUSDTBTCTRY Pair1 + Pair2 şeklindedir.
        /// </summary>
        public Dictionary<string, List<Models.PossiblePathModel>> PossiblePathsDict = new();
        public Models.ParitiesModel Parities = new();
        public List<Models.DataGridViewFlatModel> DataGridViewFlat = new();

        #region Multithreaded Singleton tanımlaması
        // https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff650316(v=pandp.10)?redirectedfrom=MSDN
        private static volatile Singletons instance;
        private static object syncRoot = new Object();
        private Singletons() { }
        public static Singletons Me
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new Singletons();
                    }
                }
                return instance;
            }
        }
        #endregion
    }
}
