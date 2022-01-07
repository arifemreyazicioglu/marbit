using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using absSettings = TradeAbstractions.Helpers.Settings;

namespace SC_BtcTurk
{
    public class ScHttp
    {
        /// <summary>
        /// Kesinlikle direk kullanılmamalı.
        /// GetHttp metodu içinde ayarlamalar olduğundan onu çağırarak elde etmeli ve kullanmalıyız.
        /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
        /// HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        /// HttpClient is intended to be instantiated once and re-used throughout the life of an application.
        /// Instantiating an HttpClient class for every request will exhaust the number of sockets available
        /// under heavy loads. This will result in SocketException errors.
        /// </summary>
        /// <remarks>
        /// Properties of HttpClient should not be modified while there are outstanding requests,
        /// because it is not thread-safe.
        /// </remarks>
        static readonly HttpClient _http = new HttpClient();
        /// <param name="endPoint"> Örn: api/v1/order </param>
        /// <param name="httpMethod"> Örn: Methods.Get, Methods.Post vb. </param>
        /// <param name="functionName"> Hangi fonksiyondan çağırmışsak ismi. Örn: CreateOrderService </param>
        /// <param name="needAuthentication"> Authentication gerekiyorsa true yapılacak. </param>
        /// <param name="inputModel"> Post gibi model gönderilecek metotlar kullanılacaksa doldurulacak. </param>
        /// <returns>
        /// Geriye null dönmez.
        /// </returns>
        internal async Task<Models.ReturnModel<T>> SendRequest<T>(string endPoint, Methods httpMethod, string functionName, bool needAuthentication = false, object inputModel = null) where T : class
        {
            Models.ReturnModel<T> retMdl = null;

            HttpClient client;
            HttpResponseMessage response = null;

            string url = absSettings.EngineSettings.Shared[TradeAbstractions.Enums.EnumExchange.BtcTurk].ServerUrl + endPoint;
            string jsonReq = null, responseBody = null, customMsg = null, exceptionMsg = null;
            DateTime startTime = default;

            Stopwatch elapsedMsSw = new Stopwatch();
            Stopwatch totalElapsedMsSw = Stopwatch.StartNew();
            int retryAfterSeconds = 0;
            try
            {
                client = GetHttp(needAuthentication: needAuthentication);

                switch (httpMethod)
                {
                    case Methods.Get:
                        startTime = DateTime.UtcNow;
                        elapsedMsSw.Start();
                        response = await client.GetAsync(url);

                        break;
                    case Methods.Post:
                        jsonReq = JsonConvert.SerializeObject(inputModel);
                        var data = new StringContent(jsonReq, Encoding.UTF8, "application/json");

                        startTime = DateTime.UtcNow;
                        elapsedMsSw.Start();
                        response = await client.PostAsync(url, data);

                        break;
                }
                elapsedMsSw.Stop();

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    string retryAfter = "";
                    TimeSpan? delta = response.Headers.RetryAfter.Delta;
                    if (delta != null)
                    {
                        retryAfterSeconds = (int)((TimeSpan)delta).TotalSeconds;
                        retryAfter = $"RetryAfter: {retryAfterSeconds}secs";
                    }

                    customMsg = $"{Convert.ToInt32(response.StatusCode)} {response.StatusCode} {retryAfter}";
                }
                else if (!response.IsSuccessStatusCode)
                {
                    // Başka http response code'ları
                    customMsg = $"{Convert.ToInt32(response.StatusCode)} {response.StatusCode}";
                }

                responseBody = await response.Content.ReadAsStringAsync();
                retMdl = JsonConvert.DeserializeObject<Models.ReturnModel<T>>(responseBody);
            }
            catch (HttpRequestException ex)
            {
                exceptionMsg += "HttpRequestException: " + ex.ToString();
            }
            catch (Exception ex)
            {
                exceptionMsg += "Exception: " + ex.ToString();
            }
            finally
            {
                if (elapsedMsSw.IsRunning) elapsedMsSw.Stop();
                totalElapsedMsSw.Stop();
            }

            if (retMdl == null) retMdl = new Models.ReturnModel<T>();
            if (exceptionMsg != null)
            {
                if (customMsg != null) customMsg += ", ";
                customMsg += exceptionMsg;
            }
            if (customMsg != null) retMdl.CustomMessage = $"{functionName}, " + customMsg;

            // RequestResponseStats set edelim >>>
            retMdl.ReqResp = new TradeAbstractions.Helpers.RequestResponseStats
            {
                Url = url,
                Req = jsonReq, // GET requestlerde null olur.
                Resp = responseBody,
                ExceptionMsg = exceptionMsg,
                RetryAfter = retryAfterSeconds
            };

            retMdl.ReqResp.SetTimes(startTime, elapsedMsSw.Elapsed.TotalMilliseconds, totalElapsedMsSw.Elapsed.TotalMilliseconds);
            if (response != null) retMdl.ReqResp.HttpStatusCode = response.StatusCode;
            // <<< RequestResponseStats set edelim

            return retMdl;
        }
        HttpClient GetHttp(bool needAuthentication = false)
        {
            _http.DefaultRequestHeaders.Clear();

            if (needAuthentication)
            {
                _http.DefaultRequestHeaders.Add("X-PCK", absSettings.ExchangeSpecific.BtcTurk.PublicKey);
                var stamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _http.DefaultRequestHeaders.Add("X-Stamp", stamp.ToString(CultureInfo.InvariantCulture));
                var signature = GetSignature(stamp);
                _http.DefaultRequestHeaders.Add("X-Signature", signature);
            }

            return _http;
        }
        /// <summary>
        /// Generate a signature by giving timestamp
        /// </summary>
        /// <param name="stamp">long tickes for current time</param>
        /// <returns>string segnature</returns>
        string GetSignature(long stamp)
        {
            string signature = null;

            var data = $"{absSettings.ExchangeSpecific.BtcTurk.PublicKey}{stamp}";
            using (var hmac = new HMACSHA256(Convert.FromBase64String(absSettings.ExchangeSpecific.BtcTurk.PrivateKey)))
            {
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                signature = Convert.ToBase64String(signatureBytes);
            }

            return signature;
        }
        public enum Methods
        {
            Get,
            Post
        }
    }
}
