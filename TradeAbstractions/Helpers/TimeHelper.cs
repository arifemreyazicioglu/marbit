using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAbstractions.Helpers
{
    public static class TimeHelper
    {
        /// <param name="timeStamp">Milliseconds cinsinden Unix Time değeri</param>
        /// <returns> UTC datetime alırız </returns>
        public static DateTime TimestampToUtcDateTime(long timeStamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).UtcDateTime;
        }
        /// <summary>
        /// 2021-08-09T03:52:57.123Z cinsinden
        /// </summary>
        /// <returns></returns>
        public static string PrintZuluTime(DateTime dt = default)
        {
            if (dt == default) dt = DateTime.UtcNow;

            return dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'");
        }
        /// <summary>
        /// 2021-08-09T03:52:57.123456 cinsinden
        /// </summary>
        /// <returns></returns>
        public static string PrintMicroseconds(DateTime dt = default)
        {
            if (dt == default) dt = DateTime.UtcNow;

            return dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ffffff");
        }
    }
}
