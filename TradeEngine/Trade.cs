using System;
using System.Threading.Tasks;
using absSettings = TradeAbstractions.Helpers.Settings;
using helpMe = TradeEngine.Models.HelpMe;
using absHelper = TradeAbstractions.Helpers;
using absEngine = TradeAbstractions.Models.Engine;
using System.Diagnostics;
using absMdl = TradeAbstractions.Models;
using absInt = TradeAbstractions.Interfaces;
namespace TradeEngine
{
    public static class Trade
    {
        /// <summary>
        /// TriangleArbitrageMaxCount = 0 ise ve hata almadığı sürece sonsuza kadar trade eder. Yani geriye değer dönmez.
        /// Geriye dönerse ya hata almıştır ya da kısıtlı sayıda trade etmesi için ayarlanmıştır.
        /// </summary>
        public static async Task<bool> StartTradeOnlyApi(absInt.ITradeEvents tradeEvents)
        {
            bool isSuccess;
            try
            {
                isSuccess = await BeginStart(tradeEvents);
                if (isSuccess) isSuccess = await InfiniteCalculation();
            }
            catch (Exception ex)
            {
                helpMe.TradeEvents.PrintMessageEvent("StartTradeForever, Exeption: " + ex.ToString(), true);
                helpMe.Email.SendEmail("StartTradeForever içinde Exception Oluştu", "Exeption: " + ex.ToString());

                return false;
            }

            if (isSuccess)
            {
                // Engine'i manuel olarak durdurmadıysak..
                if (Statics.EngineProps.EngineStarted) InfiniteTradeResultSuccess();
            }
            else
            {
                helpMe.TradeEvents.PrintMessageEvent("Trade sırasında hata oluştu!", true);
                helpMe.Email.SendEmail("Hata Oluştu", "InfiniteTrade() içinde bir hata oluştu! Log'lar incelenmeli.");
            }

            return isSuccess;
        }

        internal static void InfiniteTradeResultSuccess()
        {
            helpMe.TradeEvents.PrintMessageEvent("Trade sonuçlandı!");
        }

        internal static async Task<bool> BeginStart(absInt.ITradeEvents tradeEvents)
        {
            if (!helpMe.SetHelper(tradeEvents)) return false;

            bool softStart = await SoftRestart(true).ConfigureAwait(false);
            if (!softStart) return false;

            StartEngine();

            // Buraya kadar başarı ile gelirsek true dönelim
            return true;
        }

        /// <summary>
        /// Arada bir çağırarak komisyon oranlarını ve kullanıcı bakiyesini güncellemek faydalı olacaktır.
        /// Komisyon oranları bayağı seyrek bir şekilde değişecektir ancak değişimi yakalamak gerekmektedir.
        /// Kullanıcı bakiyesi ise el ile trade ettiğimizde engine'nin bundan haberi olması için gerekmektedir.
        /// WebSocket ("type":201 BalanceUpdated U) ile otomatik tetikleyecektik ancak tutarlı olarak bu response'u göndermiyor maalesef.
        /// PossiblePaths kısmı da yeni eklenen veya çıkarılan kripto paralar olabileceğinden yenilenmeli. Fonksiyon içinde komisyon oranlarını da barındırır.
        /// </summary>
        static async Task<bool> SoftRestart(bool firstInit)
        {
            Statics.EngineProps.SoftRestartTime = DateTime.UtcNow;

            // API Key ile kullanılabilir sermayemizi çekelim.
            var setUserBalance = await Operations.InternalSettings.SetUserBalance().ConfigureAwait(false);
            if (!setUserBalance) return false;

            // Olası üçgen arbitraj yollarını çıkaralım. Ancak dönüş nesnesini kullanmayacağız. Yerine singletons.Me.PossiblePathsDict kullanılacak.
            var possiblePathsTask = await Operations.CreatePossiblePaths.Init(firstInit).ConfigureAwait(false);
            if (!possiblePathsTask.Success) return false;

            return true;
        }
        public static void StartEngine()
        {
            Statics.EngineProps.StartEngine();
        }
        public static void StopEngine()
        {
            Statics.EngineProps.StopEngine();
        }
        /// <summary> WebSocket işlemlerinde Fire and Forget şeklinde çağırılsın. </summary>
        internal static async Task<bool> TriggerSoftRestartIfNeeded()
        {
            // 30 saniyede bir soft restart yapalım.
            if ((DateTime.UtcNow - Statics.EngineProps.SoftRestartTime).TotalSeconds > 30)
            {
                return await SoftRestart(false).ConfigureAwait(false);
            }

            return true;
        }
        /// <summary>
        /// Trade'in sonsuz döngü şeklinde çalışacak versiyonu
        /// </summary>
        /// <returns></returns>
        static async Task<bool> InfiniteCalculation()
        {
            int waitMs = 1000;
            Stopwatch sw = new Stopwatch();
            while (Statics.EngineProps.EngineStarted)
            {
                // Belirlediğimiz süreden kısa sürmüşse biraz bekletelim.
                if (sw.ElapsedMilliseconds < waitMs)
                    await Task.Delay((int)(waitMs - sw.ElapsedMilliseconds));

                bool triggerSoftRestartIfNeeded = await TriggerSoftRestartIfNeeded();
                if (!triggerSoftRestartIfNeeded) return false;

                sw.Restart();

                // Tüm borsa tickerlarını ve banka döviz değerini alalım ve genel listemizi oluşturmuş olarak geriye dönelim.
                var ratiosTask = await Operations.CalculateRatios.GetRatios();
                if (!ratiosTask.Success)
                {
                    sw.Stop();
                    return false;
                }

                // Burası button click ile birlikte yapılacak
                // Fırsat sonrası trade kararı verildi mi?
                //if (opportunity.HasTradeDecision)
                //{
                //    var makeOrdersTask = await Operations.HandleOrders.MakeOrders(opportunity);

                //    // Data null olamaz. Log kaydı için burada atayalım.
                //    absEngine.TriangleOrders triangleOrders = makeOrdersTask.Data;

                //    bool setTriangleCompletedData;
                //    if (!makeOrdersTask.Success)
                //    {
                //        setTriangleCompletedData = await SetTriangleCompletedData(false, triangleOrders, opportunity, sw);
                //        if (!setTriangleCompletedData) return false;
                //    }

                //    // 3 EMRİ DE BAŞARI İLE VERDİYSEK ARTIK BURADAYIZ.
                //    // Kâr da etmiş olabiliriz zarar da.
                //    triangleOrders.TriangleArbitrageCount = ++Statics.EngineProps.TriangleArbitrageCount;

                //    setTriangleCompletedData = await SetTriangleCompletedData(true, triangleOrders, opportunity, sw);
                //    if (!setTriangleCompletedData) return false;
                //}
            }

            return true;
        }
        internal static async Task<bool> SetTriangleCompletedData(bool ordersSuccess, absEngine.TriangleOrders triangleOrders, absMdl.Engine.Opportunity opportunity, Stopwatch sw)
        {
            sw.Stop();

            triangleOrders.TriangleOrdersEventTime = DateTime.UtcNow;
            absEngine.TriangleCompletedData triangleCompletedData = new()
            {
                Opportunity = opportunity,
                TriangleOrders = triangleOrders,
                TotalElapsedMs = sw.Elapsed.TotalMilliseconds
            };

            if (ordersSuccess)
            {
                Operations.Dbs.AddDevelopmentLog("TriangleArbitrage", "Success", triangleCompletedData);
            }
            else Operations.Dbs.AddDevelopmentLog("TriangleArbitrage", "Error", triangleCompletedData);

            return ordersSuccess;
        }
    }
}