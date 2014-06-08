namespace OptionMM
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuBarPanel = new System.Windows.Forms.Panel();
            this.forQuoteInfoLabel = new System.Windows.Forms.Label();
            this.hedgeIFVolumeLabel = new System.Windows.Forms.Label();
            this.riskManagementButton = new System.Windows.Forms.Button();
            this.startAllButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.parityButton = new System.Windows.Forms.Button();
            this.coveredButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.optionPanel = new OptionMM.OptionPanel();
            this.coveredPanel = new OptionMM.CoveredPanel();
            this.parityPanel = new OptionMM.ParityPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuBarPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.menuBarPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1444, 665);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuBarPanel
            // 
            this.menuBarPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.menuBarPanel.Controls.Add(this.forQuoteInfoLabel);
            this.menuBarPanel.Controls.Add(this.hedgeIFVolumeLabel);
            this.menuBarPanel.Controls.Add(this.riskManagementButton);
            this.menuBarPanel.Controls.Add(this.startAllButton);
            this.menuBarPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuBarPanel.Location = new System.Drawing.Point(0, 0);
            this.menuBarPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.menuBarPanel.Name = "menuBarPanel";
            this.menuBarPanel.Size = new System.Drawing.Size(1444, 29);
            this.menuBarPanel.TabIndex = 1;
            // 
            // forQuoteInfoLabel
            // 
            this.forQuoteInfoLabel.AutoSize = true;
            this.forQuoteInfoLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.forQuoteInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.forQuoteInfoLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.forQuoteInfoLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.forQuoteInfoLabel.Location = new System.Drawing.Point(288, 8);
            this.forQuoteInfoLabel.Name = "forQuoteInfoLabel";
            this.forQuoteInfoLabel.Size = new System.Drawing.Size(74, 18);
            this.forQuoteInfoLabel.TabIndex = 3;
            this.forQuoteInfoLabel.Text = "询价单：";
            // 
            // hedgeIFVolumeLabel
            // 
            this.hedgeIFVolumeLabel.AutoSize = true;
            this.hedgeIFVolumeLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.hedgeIFVolumeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.hedgeIFVolumeLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hedgeIFVolumeLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.hedgeIFVolumeLabel.Location = new System.Drawing.Point(83, 8);
            this.hedgeIFVolumeLabel.Name = "hedgeIFVolumeLabel";
            this.hedgeIFVolumeLabel.Size = new System.Drawing.Size(138, 18);
            this.hedgeIFVolumeLabel.TabIndex = 2;
            this.hedgeIFVolumeLabel.Text = "对冲IF1406(手)：";
            // 
            // riskManagementButton
            // 
            this.riskManagementButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.riskManagementButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.riskManagementButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.riskManagementButton.Location = new System.Drawing.Point(1369, 0);
            this.riskManagementButton.Name = "riskManagementButton";
            this.riskManagementButton.Size = new System.Drawing.Size(75, 29);
            this.riskManagementButton.TabIndex = 1;
            this.riskManagementButton.Text = "风险管理";
            this.riskManagementButton.UseVisualStyleBackColor = true;
            this.riskManagementButton.Click += new System.EventHandler(this.riskManagementButton_Click);
            // 
            // startAllButton
            // 
            this.startAllButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.startAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startAllButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.startAllButton.Location = new System.Drawing.Point(0, 0);
            this.startAllButton.Name = "startAllButton";
            this.startAllButton.Size = new System.Drawing.Size(75, 29);
            this.startAllButton.TabIndex = 0;
            this.startAllButton.Text = "全部启动";
            this.startAllButton.UseVisualStyleBackColor = true;
            this.startAllButton.Click += new System.EventHandler(this.startAllButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 950F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.optionPanel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1444, 635);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Controls.Add(this.parityButton);
            this.panel1.Controls.Add(this.coveredButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(950, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 635);
            this.panel1.TabIndex = 1;
            // 
            // parityButton
            // 
            this.parityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.parityButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.parityButton.Location = new System.Drawing.Point(116, 3);
            this.parityButton.Name = "parityButton";
            this.parityButton.Size = new System.Drawing.Size(107, 29);
            this.parityButton.TabIndex = 3;
            this.parityButton.Text = "平价套利";
            this.parityButton.UseVisualStyleBackColor = true;
            this.parityButton.Click += new System.EventHandler(this.parityButton_Click);
            // 
            // coveredButton
            // 
            this.coveredButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.coveredButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.coveredButton.Location = new System.Drawing.Point(3, 3);
            this.coveredButton.Name = "coveredButton";
            this.coveredButton.Size = new System.Drawing.Size(107, 29);
            this.coveredButton.TabIndex = 1;
            this.coveredButton.Text = "单期权上限套利";
            this.coveredButton.UseVisualStyleBackColor = true;
            this.coveredButton.Click += new System.EventHandler(this.coveredButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.coveredPanel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.parityPanel, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.33334F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(479, 360);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // optionPanel
            // 
            this.optionPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionPanel.Location = new System.Drawing.Point(0, 0);
            this.optionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.optionPanel.Name = "optionPanel";
            this.optionPanel.Size = new System.Drawing.Size(950, 635);
            this.optionPanel.TabIndex = 0;
            // 
            // coveredPanel
            // 
            this.coveredPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coveredPanel.Location = new System.Drawing.Point(3, 3);
            this.coveredPanel.Name = "coveredPanel";
            this.coveredPanel.Size = new System.Drawing.Size(473, 107);
            this.coveredPanel.TabIndex = 4;
            // 
            // parityPanel
            // 
            this.parityPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parityPanel.Location = new System.Drawing.Point(3, 116);
            this.parityPanel.Name = "parityPanel";
            this.parityPanel.Size = new System.Drawing.Size(473, 241);
            this.parityPanel.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 665);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.menuBarPanel.ResumeLayout(false);
            this.menuBarPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel menuBarPanel;
        private System.Windows.Forms.Button startAllButton;
        private System.Windows.Forms.Button riskManagementButton;
        private System.Windows.Forms.Label hedgeIFVolumeLabel;
        public System.Windows.Forms.Label forQuoteInfoLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public OptionPanel optionPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button coveredButton;
        private System.Windows.Forms.Button parityButton;
        public CoveredPanel coveredPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public ParityPanel parityPanel;


    }
}

