using System;

namespace TradeAbstractions.Helpers
{
    public class ResponseStatsWs
    {
        public ResponseStatsWs(string respStr, double receiveMs, DateTime receiveTime, double processMs)
        {
            ResponseStr = respStr;
            ReceiveMs = receiveMs;
            ReceiveTime = receiveTime;
            ProcessMs = processMs;
        }
        /// <summary>
        /// WebSocket'ten verinin receive edilip string haline dönüştürülmüş hali.
        /// </summary>
        public string ResponseStr { get; private set; }
        /// <summary>
        /// Veri WebSocket'ten beklemeye başladığında sayaç başlar, receive edilip string haline döndükten sonra stop edilir.
        /// Kısaca veriyi almaya başladığımız anda başlamaz. Bekleme süresi de dahildir.
        /// </summary>
        public double ReceiveMs { get; private set; }
        /// <summary>
        /// Veri WebSocket'ten receive edilip string haline döndükten hemen sonra UTC cinsinden set edilir.
        /// </summary>
        public DateTime ReceiveTime { get; private set; }
        /// <summary>
        /// WebSocket sonrası gidilen fonksiyonun içindeki işlemlerin toplam milisaniyesidir.
        /// </summary>
        public double ProcessMs { get; private set; }
    }
}
