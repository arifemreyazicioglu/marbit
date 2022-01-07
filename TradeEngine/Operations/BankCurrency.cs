using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace TradeEngine.Operations
{
    internal class BankCurrency
    {
        static readonly HttpClient _http = new HttpClient();
        internal static async Task<decimal> GetUsdTry()
        {
            var url = "https://portal-widgets-v3.foreks.com/symbol-summary?code=USD/TRL";

            HttpResponseMessage response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return 0;
            string data = await response.Content.ReadAsStringAsync();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(data);

            if (htmlDoc.DocumentNode != null)
            {
                foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//div[@class='" + "symbolHead" + "']"))
                {

                    int sayac = 1;
                    var values = node.InnerText.Split('\n');

                    foreach (var value in values)
                    {

                        switch (sayac)
                        {
                            case 4:
                                string decimalDonusumu = value.Replace(",", ".");
                                decimal deger = decimal.Parse(decimalDonusumu);
                                return deger < 0 ? 0 : deger;
                            case 6:

                                break;

                            case 8:
                                string dailychange = value.Replace(" ", "").ToString();
                                break;
                            case 10:

                                break;

                            default:

                                break;
                        }
                        sayac++;
                    }
                }
            }

            return 0;
        }
    }
}
