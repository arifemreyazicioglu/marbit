using System;
using System.Windows.Forms;
using absHelpers = TradeAbstractions.Helpers;
using Newtonsoft.Json;
using absSettings = TradeAbstractions.Helpers.Settings;
using absMdl = TradeAbstractions.Models;
using System.Linq;
using TradeAbstractions.Models.TradeHelper.FromEngine;
using System.Threading.Tasks;
using TradeAbstractions.Models.Engine;
using enumExchange = TradeAbstractions.Enums.EnumExchange;
using singletons = TradeAbstractions.Singletons;
using System.Data;
using System.Collections.Generic;

namespace WinFormsApp
{
    public partial class Form1 : Form, TradeAbstractions.Interfaces.ITradeEvents
    {
        private void stopButton_Click(object sender, EventArgs e)
        {
            if (TradeEngine.Statics.EngineProps.EngineStarted)
            {
                TradeEngine.Trade.StopEngine();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (TradeEngine.Statics.EngineProps.EngineStarted)
            //{
            //    MessageBox.Show("Öncelikle TradeEngine durdurulmalı!");
            //    e.Cancel = true;
            //}
            //else Application.Exit(); // Engine durduğuna göre çıkış yaptırabiliriz.
        }
        public void BalanceUpdatedEvent()
        {
            string output = null;
            foreach (var item in Enum.GetValues<enumExchange>())
            {
                var exchange = TradeEngine.Statics.TradeHelpers[item];

                // Enumeration tanımı olsa da create edilmemiş olabilir. Öyle bir durumda hata almamak için es geçelim.
                if (exchange == null) continue;

                for (int j = 0; j < exchange.UserBalance.BalanceList.Count; j++)
                {
                    var balance = exchange.UserBalance.BalanceList[j];
                    for (int i = 0; i < absSettings.InternalSettings.BaseList.Count; i++)
                    {
                        var settBase = absSettings.InternalSettings.BaseList[i];
                        if (settBase == balance.Asset)
                        {
                            if (output != null) output += Environment.NewLine;
                            output += $"{item} | {settBase}: {balance.Free.ToString("#,##0.##")}";
                        }
                    }
                }
            }
            // Test modunda ise bakiyeyi borsadan çekmeyelim.
            if(!absSettings.EngineSettings.IsProd) gboxUserBalance.InvokeIfRequired(p => p.Text = "Balance - TEST (IsProd: false)");
            tboxUserBalance.InvokeIfRequired(p => p.Text = output);
        }
        public void ParitiesUpdatedEvent()
        {
            string output = null;
            string output2 = null;

            output += $"Banka USD | Ask: {singletons.Me.Parities.UsdTry.Ask.ToString("0.########")} # Bid: {singletons.Me.Parities.UsdTry.Bid.ToString("0.########")}";

            foreach (var item in Enum.GetValues<enumExchange>())
            {
                var exchange = TradeEngine.Statics.TradeHelpers[item];

                // Enumeration tanımı olsa da create edilmemiş olabilir.
                if (exchange == null) continue;

                var parity = singletons.Me.Parities.Exchanges[item];

                output += Environment.NewLine;
                output += $"{item} USDT | Ask: {parity.Ask.ToString("0.########")} # Bid: {parity.Bid.ToString("0.########")}";

                if (output2 != null) output2 += Environment.NewLine;
                output2 += $"{item}" + Environment.NewLine;
                output2 += "   USDT / USD: " + (parity.SellDiffRatio > 0 ? " " : "") + $"{parity.SellDiffRatio.ToString("0.########")}" + Environment.NewLine;
                output2 += "   USD / USDT: " + (parity.BuyDiffRatio > 0 ? " " : "") + $"{parity.BuyDiffRatio.ToString("0.########")}";
            }
            output2 += Environment.NewLine + "Limit Ratio: " + absSettings.EngineSettings.LimitPercentage.ToString();

            tboxParities.InvokeIfRequired(p => p.Text = output);
            tboxUsdRatios.InvokeIfRequired(p => p.Text = output2);
        }
        public void RatiosCalculatedEvent()
        {
            if (dataGridView1.DataSource == null)
            {
                dataGridView1.CellClick += dataGridView1_CellClick;
                dataGridView1.ReadOnly = true;
            }

            // Zarar yazacak kayıtları göstermeyelim.
            singletons.Me.DataGridViewFlat.RemoveAll(x => x.ParityRatio < 0);
            singletons.Me.DataGridViewFlat = singletons.Me.DataGridViewFlat.OrderByDescending(o => o.Profit).ToList();

            dataGridView1.DataSource = singletons.Me.DataGridViewFlat;
            dataGridView1.Refresh();

            // Sıralama özelliğini devre dışı bırakalım
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Header'a tıklanmıştır birşey yapılmasın.
            if (e.RowIndex == -1) return;

            dataGridView1.Rows[e.RowIndex].Selected = true;
            var row = dataGridView1.Rows[e.RowIndex];
            lblSelected.Text = $"{row.Cells[1].Value} # {row.Cells[2].Value} {row.Cells[6].Value} # Parity Ratio: " + Convert.ToDecimal(row.Cells[10].Value).ToString("0.######");
        }
        public void PrintMessageEvent(string msg, bool anyError = false)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                // Zaman damgasını hep kullanalım.
                msg = absHelpers.TimeHelper.PrintZuluTime() + " # " + msg;
                listBox1.InvokeIfRequired(p => p.Items.Insert(0, msg));
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "MARBIT | " + System.IO.Path.GetFileName(System.IO.Directory.GetCurrentDirectory());
            _ = TradeEngine.Trade.StartTradeOnlyApi(this);

            //scTEST
            //singletons.Me.DataGridViewFlat = JsonConvert.DeserializeObject<List<absMdl.DataGridViewFlatModel>>(TestDatas.DataGridViewFlat);
            //RatiosCalculatedEvent();
        }
    }
    public static class InvokePurpose
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            //c.Invoke(new Action(() => action(c)));
            // Yukarıdakinin yerine delegate kullanımının daha kullanışlı olduğunu görmüştüm bir yerde..
            if (c.InvokeRequired) c.Invoke((MethodInvoker)delegate { action(c); });
            else action(c);
        }
    }
}
