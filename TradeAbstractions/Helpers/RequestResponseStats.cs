using System;

namespace TradeAbstractions.Helpers
{
    public class RequestResponseStats
    {
        public System.Net.HttpStatusCode HttpStatusCode { get; set; }
        /// <summary> Talepte bulunduğumuz tam url verisidir. </summary>
        public string Url { get; set; }
        /// <summary> Raw request verisini içerir. GET için null'dur. </summary>
        public string Req { get; set; }
        /// <summary> Raw response verisini içerir </summary>
        public string Resp { get; set; }
        /// <summary> Servis'e gitmeden önce set edilir. </summary>
        public DateTime StartTime { get; private set; }
        /// <summary> StartTime'a ElapsedMs eklenerek bulunur. </summary>
        public DateTime EndTime { get; private set; }
        /// <summary>
        /// StopWatch kullanılır. Hassas ölçüm sağlanır.
        /// Sadece request öncesi ve response sonrası ele alınır.
        /// Serilalization ve deserialization işlemlerini kapsamaz. Net ölçüm yapılır.
        /// </summary>
        public double ElapsedMs { get; private set; }
        /// <summary>
        /// StopWatch kullanılır. Hassas ölçüm sağlanır.
        /// Serilalization ve deserialization işlemlerini de kapsar.
        /// Hatta ElapsedMs hesaplamasındaki DateTime.UtcNow atamalarını da kapsar.
        /// </summary>
        public double TotalElapsedMs { get; private set; }
        /// <summary> try catch bloğundan bir hata fırlatıldı mı? </summary>
        public string ExceptionMsg { get; set; }
        /// <summary> Too many requests 429 kodu geldiğinde kaç saniye bekleteceğimizi gireriz. </summary>
        public int RetryAfter { get; set; }
        public void SetTimes(DateTime startTime, double elapsedMs, double totalElapsedMs)
        {
            StartTime = startTime;
            EndTime = startTime.AddMilliseconds(elapsedMs);
            ElapsedMs = elapsedMs;
            TotalElapsedMs = totalElapsedMs;
        }
    }
}
