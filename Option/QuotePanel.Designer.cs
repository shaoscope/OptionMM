using System.Windows.Forms;
namespace OptionMM
{
    partial class QuotePanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataTable = new OptionMM.DoubleBufferedDataGridView();
            this.cCallLongOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallTodayLongOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallBidQuote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallBidPrice1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallTheoricalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallAskPrice1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallAskQuote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallTodayShortOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStrikePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutLongOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutTodayLongOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutBidQuote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutBidPrice1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutTheoricalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutAskPrice1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutAskQuote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutTodayShortOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dataTable
            // 
            this.dataTable.AllowUserToAddRows = false;
            this.dataTable.AllowUserToDeleteRows = false;
            this.dataTable.AllowUserToResizeRows = false;
            this.dataTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cCallLongOpenInterest,
            this.cCallTodayLongOpenInterest,
            this.cCallBidQuote,
            this.cCallBidPrice1,
            this.cCallTheoricalPrice,
            this.cCallAskPrice1,
            this.cCallAskQuote,
            this.cCallTodayShortOpenInterest,
            this.cCallOpenInterest,
            this.cCallVolatility,
            this.cStrikePrice,
            this.cPutVolatility,
            this.cPutLongOpenInterest,
            this.cPutTodayLongOpenInterest,
            this.cPutBidQuote,
            this.cPutBidPrice1,
            this.cPutTheoricalPrice,
            this.cPutAskPrice1,
            this.cPutAskQuote,
            this.cPutTodayShortOpenInterest,
            this.cPutOpenInterest});
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.ReadOnly = true;
            this.dataTable.RowHeadersVisible = false;
            this.dataTable.RowTemplate.Height = 23;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(1003, 100);
            this.dataTable.TabIndex = 0;
            this.dataTable.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTable_CellMouseDoubleClick);
            this.dataTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTable_CellMouseDown);
            // 
            // cCallLongOpenInterest
            // 
            this.cCallLongOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallLongOpenInterest.DefaultCellStyle = dataGridViewCellStyle1;
            this.cCallLongOpenInterest.HeaderText = "总多仓量";
            this.cCallLongOpenInterest.Name = "cCallLongOpenInterest";
            this.cCallLongOpenInterest.ReadOnly = true;
            this.cCallLongOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallLongOpenInterest.Width = 59;
            // 
            // cCallTodayLongOpenInterest
            // 
            this.cCallTodayLongOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallTodayLongOpenInterest.DefaultCellStyle = dataGridViewCellStyle2;
            this.cCallTodayLongOpenInterest.HeaderText = "今多仓量";
            this.cCallTodayLongOpenInterest.Name = "cCallTodayLongOpenInterest";
            this.cCallTodayLongOpenInterest.ReadOnly = true;
            this.cCallTodayLongOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallTodayLongOpenInterest.Width = 59;
            // 
            // cCallBidQuote
            // 
            this.cCallBidQuote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallBidQuote.DefaultCellStyle = dataGridViewCellStyle3;
            this.cCallBidQuote.HeaderText = "Bid in";
            this.cCallBidQuote.Name = "cCallBidQuote";
            this.cCallBidQuote.ReadOnly = true;
            this.cCallBidQuote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallBidQuote.Width = 47;
            // 
            // cCallBidPrice1
            // 
            this.cCallBidPrice1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallBidPrice1.DefaultCellStyle = dataGridViewCellStyle4;
            this.cCallBidPrice1.HeaderText = "Bid";
            this.cCallBidPrice1.Name = "cCallBidPrice1";
            this.cCallBidPrice1.ReadOnly = true;
            this.cCallBidPrice1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallBidPrice1.Width = 29;
            // 
            // cCallTheoricalPrice
            // 
            this.cCallTheoricalPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallTheoricalPrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.cCallTheoricalPrice.HeaderText = "Theor.";
            this.cCallTheoricalPrice.Name = "cCallTheoricalPrice";
            this.cCallTheoricalPrice.ReadOnly = true;
            this.cCallTheoricalPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallTheoricalPrice.Width = 47;
            // 
            // cCallAskPrice1
            // 
            this.cCallAskPrice1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallAskPrice1.DefaultCellStyle = dataGridViewCellStyle6;
            this.cCallAskPrice1.HeaderText = "Ask";
            this.cCallAskPrice1.Name = "cCallAskPrice1";
            this.cCallAskPrice1.ReadOnly = true;
            this.cCallAskPrice1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallAskPrice1.Width = 29;
            // 
            // cCallAskQuote
            // 
            this.cCallAskQuote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallAskQuote.DefaultCellStyle = dataGridViewCellStyle7;
            this.cCallAskQuote.HeaderText = "Ask in";
            this.cCallAskQuote.Name = "cCallAskQuote";
            this.cCallAskQuote.ReadOnly = true;
            this.cCallAskQuote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallAskQuote.Width = 47;
            // 
            // cCallTodayShortOpenInterest
            // 
            this.cCallTodayShortOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallTodayShortOpenInterest.DefaultCellStyle = dataGridViewCellStyle8;
            this.cCallTodayShortOpenInterest.HeaderText = "今空仓量";
            this.cCallTodayShortOpenInterest.Name = "cCallTodayShortOpenInterest";
            this.cCallTodayShortOpenInterest.ReadOnly = true;
            this.cCallTodayShortOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallTodayShortOpenInterest.Width = 59;
            // 
            // cCallOpenInterest
            // 
            this.cCallOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallOpenInterest.DefaultCellStyle = dataGridViewCellStyle9;
            this.cCallOpenInterest.HeaderText = "总空仓量";
            this.cCallOpenInterest.Name = "cCallOpenInterest";
            this.cCallOpenInterest.ReadOnly = true;
            this.cCallOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallOpenInterest.Width = 59;
            // 
            // cCallVolatility
            // 
            this.cCallVolatility.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCallVolatility.DefaultCellStyle = dataGridViewCellStyle10;
            this.cCallVolatility.HeaderText = "Act.Vol";
            this.cCallVolatility.Name = "cCallVolatility";
            this.cCallVolatility.ReadOnly = true;
            this.cCallVolatility.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallVolatility.Width = 53;
            // 
            // cStrikePrice
            // 
            this.cStrikePrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cStrikePrice.DefaultCellStyle = dataGridViewCellStyle11;
            this.cStrikePrice.HeaderText = "St";
            this.cStrikePrice.Name = "cStrikePrice";
            this.cStrikePrice.ReadOnly = true;
            this.cStrikePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.cStrikePrice.Width = 42;
            // 
            // cPutVolatility
            // 
            this.cPutVolatility.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutVolatility.DefaultCellStyle = dataGridViewCellStyle12;
            this.cPutVolatility.HeaderText = "Act.Vol";
            this.cPutVolatility.Name = "cPutVolatility";
            this.cPutVolatility.ReadOnly = true;
            this.cPutVolatility.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutVolatility.Width = 53;
            // 
            // cPutLongOpenInterest
            // 
            this.cPutLongOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutLongOpenInterest.DefaultCellStyle = dataGridViewCellStyle13;
            this.cPutLongOpenInterest.HeaderText = "总多仓量";
            this.cPutLongOpenInterest.Name = "cPutLongOpenInterest";
            this.cPutLongOpenInterest.ReadOnly = true;
            this.cPutLongOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutLongOpenInterest.Width = 59;
            // 
            // cPutTodayLongOpenInterest
            // 
            this.cPutTodayLongOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutTodayLongOpenInterest.DefaultCellStyle = dataGridViewCellStyle14;
            this.cPutTodayLongOpenInterest.HeaderText = "今多仓量";
            this.cPutTodayLongOpenInterest.Name = "cPutTodayLongOpenInterest";
            this.cPutTodayLongOpenInterest.ReadOnly = true;
            this.cPutTodayLongOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutTodayLongOpenInterest.Width = 59;
            // 
            // cPutBidQuote
            // 
            this.cPutBidQuote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutBidQuote.DefaultCellStyle = dataGridViewCellStyle15;
            this.cPutBidQuote.HeaderText = "Bid in";
            this.cPutBidQuote.Name = "cPutBidQuote";
            this.cPutBidQuote.ReadOnly = true;
            this.cPutBidQuote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutBidQuote.Width = 47;
            // 
            // cPutBidPrice1
            // 
            this.cPutBidPrice1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutBidPrice1.DefaultCellStyle = dataGridViewCellStyle16;
            this.cPutBidPrice1.HeaderText = "Bid";
            this.cPutBidPrice1.Name = "cPutBidPrice1";
            this.cPutBidPrice1.ReadOnly = true;
            this.cPutBidPrice1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutBidPrice1.Width = 29;
            // 
            // cPutTheoricalPrice
            // 
            this.cPutTheoricalPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutTheoricalPrice.DefaultCellStyle = dataGridViewCellStyle17;
            this.cPutTheoricalPrice.HeaderText = "Theor.";
            this.cPutTheoricalPrice.Name = "cPutTheoricalPrice";
            this.cPutTheoricalPrice.ReadOnly = true;
            this.cPutTheoricalPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutTheoricalPrice.Width = 47;
            // 
            // cPutAskPrice1
            // 
            this.cPutAskPrice1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutAskPrice1.DefaultCellStyle = dataGridViewCellStyle18;
            this.cPutAskPrice1.HeaderText = "Ask";
            this.cPutAskPrice1.Name = "cPutAskPrice1";
            this.cPutAskPrice1.ReadOnly = true;
            this.cPutAskPrice1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutAskPrice1.Width = 29;
            // 
            // cPutAskQuote
            // 
            this.cPutAskQuote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutAskQuote.DefaultCellStyle = dataGridViewCellStyle19;
            this.cPutAskQuote.HeaderText = "Ask in";
            this.cPutAskQuote.Name = "cPutAskQuote";
            this.cPutAskQuote.ReadOnly = true;
            this.cPutAskQuote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutAskQuote.Width = 47;
            // 
            // cPutTodayShortOpenInterest
            // 
            this.cPutTodayShortOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutTodayShortOpenInterest.DefaultCellStyle = dataGridViewCellStyle20;
            this.cPutTodayShortOpenInterest.HeaderText = "今空仓量";
            this.cPutTodayShortOpenInterest.Name = "cPutTodayShortOpenInterest";
            this.cPutTodayShortOpenInterest.ReadOnly = true;
            this.cPutTodayShortOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutTodayShortOpenInterest.Width = 59;
            // 
            // cPutOpenInterest
            // 
            this.cPutOpenInterest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPutOpenInterest.DefaultCellStyle = dataGridViewCellStyle21;
            this.cPutOpenInterest.HeaderText = "总空仓量";
            this.cPutOpenInterest.Name = "cPutOpenInterest";
            this.cPutOpenInterest.ReadOnly = true;
            this.cPutOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutOpenInterest.Width = 59;
            // 
            // QuotePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTable);
            this.Name = "QuotePanel";
            this.Size = new System.Drawing.Size(1003, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedDataGridView dataTable;
        private DataGridViewTextBoxColumn cCallLongOpenInterest;
        private DataGridViewTextBoxColumn cCallTodayLongOpenInterest;
        private DataGridViewTextBoxColumn cCallBidQuote;
        private DataGridViewTextBoxColumn cCallBidPrice1;
        private DataGridViewTextBoxColumn cCallTheoricalPrice;
        private DataGridViewTextBoxColumn cCallAskPrice1;
        private DataGridViewTextBoxColumn cCallAskQuote;
        private DataGridViewTextBoxColumn cCallTodayShortOpenInterest;
        private DataGridViewTextBoxColumn cCallOpenInterest;
        private DataGridViewTextBoxColumn cCallVolatility;
        private DataGridViewTextBoxColumn cStrikePrice;
        private DataGridViewTextBoxColumn cPutVolatility;
        private DataGridViewTextBoxColumn cPutLongOpenInterest;
        private DataGridViewTextBoxColumn cPutTodayLongOpenInterest;
        private DataGridViewTextBoxColumn cPutBidQuote;
        private DataGridViewTextBoxColumn cPutBidPrice1;
        private DataGridViewTextBoxColumn cPutTheoricalPrice;
        private DataGridViewTextBoxColumn cPutAskPrice1;
        private DataGridViewTextBoxColumn cPutAskQuote;
        private DataGridViewTextBoxColumn cPutTodayShortOpenInterest;
        private DataGridViewTextBoxColumn cPutOpenInterest;
    }
}
