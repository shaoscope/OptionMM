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
            this.optionPanel = new OptionMM.OptionPanel();
            this.menuBarPanel = new System.Windows.Forms.Panel();
            this.hedgeIFVolumeLabel = new System.Windows.Forms.Label();
            this.riskManagementButton = new System.Windows.Forms.Button();
            this.startAllButton = new System.Windows.Forms.Button();
            this.forQuoteInfoLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.optionPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.menuBarPanel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1161, 665);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // optionPanel
            // 
            this.optionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionPanel.Location = new System.Drawing.Point(0, 30);
            this.optionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.optionPanel.Name = "optionPanel";
            this.optionPanel.Size = new System.Drawing.Size(1161, 635);
            this.optionPanel.TabIndex = 0;
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
            this.menuBarPanel.Size = new System.Drawing.Size(1161, 29);
            this.menuBarPanel.TabIndex = 1;
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
            this.riskManagementButton.Location = new System.Drawing.Point(1086, 0);
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
            this.forQuoteInfoLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 665);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.menuBarPanel.ResumeLayout(false);
            this.menuBarPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public OptionPanel optionPanel;
        private System.Windows.Forms.Panel menuBarPanel;
        private System.Windows.Forms.Button startAllButton;
        private System.Windows.Forms.Button riskManagementButton;
        private System.Windows.Forms.Label hedgeIFVolumeLabel;
        public System.Windows.Forms.Label forQuoteInfoLabel;


    }
}

