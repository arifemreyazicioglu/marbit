using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var engineSettings = new TradeEngine.Settings();
            engineSettings.SetAllSettings();

            // DİKKAT!!! Ayarların yüklenmesini sağlayan satır. Kesinlikle silinmemeli.
            //using IHost host = TradeAbstractions.Models.AllSettings.AppHostBuilderModel.CreateHostBuilder().Build();

            // Pairs listesi hazırlanır. CreatePossiblePaths, TradeEngine'den yönetilir.
            // Bu şekilde bir çok borsa entegrasyonu yapabileceğiz.
            //var tradeHelper = new SC_BtcTurk.TradeHelper();
            //var pairList = await tradeHelper.GetPairs();

            //await TradeEngine.Trade.Init();
            //await TradeEngine.Trade.GetOpportunity();

            //var orderBookObj = new SC_BtcTurk.Operations.OrderBook("MKRBTC");
            //var aa = await orderBookObj.CreateList();
        }
    }
}
