using System;
using System.Threading;

namespace TradeAbstractions.Models.Engine
{
    public class EngineProps
    {
        // Interlocked ile kullanılır. 0 for false, 1 for true.
        private static int tradeStarted = 0;
        /// <summary>
        /// True olursa Engine yeni fırsatlar aramaz. Mevcut bir trade varsa bitirince durur.
        /// </summary>
        public volatile bool EngineStarted;
        /// <summary>
        /// Engine trade etmeye karar verdiyse burası true olur.
        /// False yapılırsa WebSocket'ten uygun veriler gelse de trade etmez.
        /// </summary>
        public volatile bool TradeStarted;
        public volatile int OpportunityCount;
        public volatile int TriangleArbitrageCount;
        public volatile Engine.TriangleCompletedData TriangleCompletedData;
        public DateTime SoftRestartTime { get; set; } = default;

        public void StopEngine()
        {
            EngineStarted = false;
        }
        public void StartEngine()
        {
            EngineStarted = true;
        }
        /// <summary> WebSocket OrderBook servisi ile birlikte kullanılır. Paralel çalışma için kullanıldı. </summary>
        public bool StartTradeAndDenyNewTradeInterlocked()
        {
            if (0 == Interlocked.Exchange(ref tradeStarted, 1))
            {
                TradeStarted = true;
                return true; // Lock success, trade is newly started
            }
            else return false; //denied the lock, trade already started before
        }
        /// <summary> WebSocket OrderBook servisi ile birlikte kullanılır. Paralel çalışma için kullanıldı. </summary>
        public void AllowNewTradeInterlocked()
        {
            TradeStarted = false;
            Interlocked.Exchange(ref tradeStarted, 0);
        }
    }
}
