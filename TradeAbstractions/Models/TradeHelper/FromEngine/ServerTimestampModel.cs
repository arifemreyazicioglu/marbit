using System;
using TradeAbstractions.Helpers;

namespace TradeAbstractions.Models.TradeHelper.FromEngine
{
    public class ServerTimestampModel
    {
        /// <summary> Servis'e gitmeden önce set edilir. </summary>
        public DateTime OurServerStartTime { get; set; }
        /// <summary> OurServerStartTime ile OurServerEndTime arasında bir değere sahip olması beklenir. </summary>
        public DateTime ApiServerTime { get; set; }
        /// <summary> OurServerStartTime'a ElapsedMs eklenerek bulunur. </summary>
        public DateTime OurServerEndTime { get; set; }
        /// <summary>
        /// StopWatch kullanılır. Hassas ölçüm sağlanır.
        /// Sadece request öncesi ve response sonrası ele alınır.
        /// Serilalization ve deserialization işlemlerini kapsamaz. Net ölçüm yapılır.
        /// </summary>
        public double ElapsedMs { get; set; }
        public RequestResponseStats ReqResp { get; set; }
    }
}
