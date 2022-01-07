using Newtonsoft.Json;
using System;
using System.Net;

namespace SC_BtcTurk.Models
{
    public class ReturnModel<T> where T : class
    {
        public System.Net.HttpStatusCode HttpStatusCode { get; set; }
        public bool Success { get; set; }
        /// <summary>
        /// BtcTurk code değeri? Genelde 0 dönüyor. Kullanmıyorum.
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// BtcTurk'ten message gelmiş mi?
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// BtcTurk'ten gelen data
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Yukarıdaki 5'i BtcTurk'ün json dönüş değerine hitap ediyor.
        /// Aşağısı benden...
        /// </summary>
        public string CustomMessage { get; set; }
        public bool HasCustomMessage { get { return !string.IsNullOrWhiteSpace(CustomMessage); } }
        public bool NotSuccessOrCustomMessage { get { return !Success || HasCustomMessage; } }
        public string GetApiOrCustomMessage
        {
            get
            {
                string warn = null;
                if (!string.IsNullOrWhiteSpace(Message)) warn = $"Code: {Code}, Message: {Message}";
                if (HasCustomMessage) warn += (warn != null ? " | " : null) + $"Msg: {CustomMessage}";
                return warn;
            }
        }
        /// <summary> Request ve Response hakkındaki ham veriyi içerelim. Debug yaparken hayat kurtarır. </summary>
        public TradeAbstractions.Helpers.RequestResponseStats ReqResp { get; set; }
    }
}
