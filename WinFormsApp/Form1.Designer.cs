
namespace WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gboxUserBalance = new System.Windows.Forms.GroupBox();
            this.tboxUserBalance = new System.Windows.Forms.TextBox();
            this.gboxUsdRatios = new System.Windows.Forms.GroupBox();
            this.tboxUsdRatios = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tboxParities = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.gboxUserBalance.SuspendLayout();
            this.gboxUsdRatios.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gboxUserBalance
            // 
            this.gboxUserBalance.Controls.Add(this.tboxUserBalance);
            this.gboxUserBalance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gboxUserBalance.Location = new System.Drawing.Point(14, 16);
            this.gboxUserBalance.Margin = new System.Windows.Forms.Padding(2);
            this.gboxUserBalance.Name = "gboxUserBalance";
            this.gboxUserBalance.Padding = new System.Windows.Forms.Padding(2);
            this.gboxUserBalance.Size = new System.Drawing.Size(375, 188);
            this.gboxUserBalance.TabIndex = 16;
            this.gboxUserBalance.TabStop = false;
            this.gboxUserBalance.Text = "Balance";
            // 
            // tboxUserBalance
            // 
            this.tboxUserBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tboxUserBalance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tboxUserBalance.Location = new System.Drawing.Point(2, 26);
            this.tboxUserBalance.Margin = new System.Windows.Forms.Padding(2);
            this.tboxUserBalance.Multiline = true;
            this.tboxUserBalance.Name = "tboxUserBalance";
            this.tboxUserBalance.ReadOnly = true;
            this.tboxUserBalance.Size = new System.Drawing.Size(371, 160);
            this.tboxUserBalance.TabIndex = 0;
            this.tboxUserBalance.TabStop = false;
            // 
            // gboxUsdRatios
            // 
            this.gboxUsdRatios.Controls.Add(this.tboxUsdRatios);
            this.gboxUsdRatios.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gboxUsdRatios.Location = new System.Drawing.Point(774, 16);
            this.gboxUsdRatios.Margin = new System.Windows.Forms.Padding(2);
            this.gboxUsdRatios.Name = "gboxUsdRatios";
            this.gboxUsdRatios.Padding = new System.Windows.Forms.Padding(2);
            this.gboxUsdRatios.Size = new System.Drawing.Size(288, 188);
            this.gboxUsdRatios.TabIndex = 18;
            this.gboxUsdRatios.TabStop = false;
            this.gboxUsdRatios.Text = "USD Ratios";
            // 
            // tboxUsdRatios
            // 
            this.tboxUsdRatios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tboxUsdRatios.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tboxUsdRatios.Location = new System.Drawing.Point(2, 26);
            this.tboxUsdRatios.Margin = new System.Windows.Forms.Padding(2);
            this.tboxUsdRatios.Multiline = true;
            this.tboxUsdRatios.Name = "tboxUsdRatios";
            this.tboxUsdRatios.ReadOnly = true;
            this.tboxUsdRatios.Size = new System.Drawing.Size(284, 160);
            this.tboxUsdRatios.TabIndex = 0;
            this.tboxUsdRatios.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Location = new System.Drawing.Point(14, 1008);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(2159, 181);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(4, 28);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(2151, 149);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tboxParities);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.Location = new System.Drawing.Point(394, 16);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(375, 188);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "USD | USDT";
            // 
            // tboxParities
            // 
            this.tboxParities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tboxParities.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tboxParities.Location = new System.Drawing.Point(2, 26);
            this.tboxParities.Margin = new System.Windows.Forms.Padding(2);
            this.tboxParities.Multiline = true;
            this.tboxParities.Name = "tboxParities";
            this.tboxParities.ReadOnly = true;
            this.tboxParities.Size = new System.Drawing.Size(371, 160);
            this.tboxParities.TabIndex = 0;
            this.tboxParities.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 268);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(2159, 732);
            this.dataGridView1.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 239);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 25);
            this.label1.TabIndex = 21;
            this.label1.Text = "Selected:";
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(109, 239);
            this.lblSelected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(19, 25);
            this.lblSelected.TabIndex = 22;
            this.lblSelected.Text = "-";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2186, 1198);
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gboxUsdRatios);
            this.Controls.Add(this.gboxUserBalance);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "MARBIT | Mevcut Klasör Adı";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gboxUserBalance.ResumeLayout(false);
            this.gboxUserBalance.PerformLayout();
            this.gboxUsdRatios.ResumeLayout(false);
            this.gboxUsdRatios.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox gboxUserBalance;
        private System.Windows.Forms.TextBox tboxUserBalance;
        private System.Windows.Forms.GroupBox gboxUsdRatios;
        private System.Windows.Forms.TextBox tboxUsdRatios;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tboxParities;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSelected;
    }
}

