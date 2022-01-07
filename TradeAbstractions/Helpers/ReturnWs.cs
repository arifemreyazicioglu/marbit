using System;

namespace TradeAbstractions.Helpers
{
    public class ReturnWs<T> where T : class
    {
        public ReturnWs(string respStr, double receiveMs, DateTime receiveTime, double processMs)
        {
            ReqResp = new ResponseStatsWs(respStr, receiveMs, receiveTime, processMs);
        }
        public T Data { get; set; }
        public ResponseStatsWs ReqResp { get; private set; }
    }
}
