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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.cInstrumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cInstrumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLastPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBidPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBidVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAskPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAskVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOpenInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUpperLimitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLowerLimitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOpenPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPreSettlementPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHighestPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLowestPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCurrentVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPreClosePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTurnover = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cExchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUpdateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.cInstrumentID,
            this.cInstrumentName,
            this.cLastPrice,
            this.cDeta,
            this.cBidPrice,
            this.cBidVolume,
            this.cAskPrice,
            this.cAskVolume,
            this.cVolume,
            this.cOpenInterest,
            this.cUpperLimitPrice,
            this.cLowerLimitPrice,
            this.cOpenPrice,
            this.cPreSettlementPrice,
            this.cHighestPrice,
            this.cLowestPrice,
            this.cCurrentVolume,
            this.cRate,
            this.cPreClosePrice,
            this.cTurnover,
            this.cExchange,
            this.cUpdateTime});
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.ReadOnly = true;
            this.dataTable.RowHeadersVisible = false;
            this.dataTable.RowTemplate.Height = 23;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(1200, 100);
            this.dataTable.TabIndex = 0;
            // 
            // cInstrumentID
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cInstrumentID.DefaultCellStyle = dataGridViewCellStyle1;
            this.cInstrumentID.HeaderText = "合约代码";
            this.cInstrumentID.Name = "cInstrumentID";
            this.cInstrumentID.ReadOnly = true;
            this.cInstrumentID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cInstrumentID.Width = 90;
            // 
            // cInstrumentName
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cInstrumentName.DefaultCellStyle = dataGridViewCellStyle2;
            this.cInstrumentName.HeaderText = "合约名称";
            this.cInstrumentName.Name = "cInstrumentName";
            this.cInstrumentName.ReadOnly = true;
            this.cInstrumentName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLastPrice
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cLastPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.cLastPrice.HeaderText = "最新价";
            this.cLastPrice.Name = "cLastPrice";
            this.cLastPrice.ReadOnly = true;
            this.cLastPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLastPrice.Width = 75;
            // 
            // cDeta
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cDeta.DefaultCellStyle = dataGridViewCellStyle4;
            this.cDeta.HeaderText = "涨跌";
            this.cDeta.Name = "cDeta";
            this.cDeta.ReadOnly = true;
            this.cDeta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cDeta.Width = 60;
            // 
            // cBidPrice
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cBidPrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.cBidPrice.HeaderText = "买价";
            this.cBidPrice.Name = "cBidPrice";
            this.cBidPrice.ReadOnly = true;
            this.cBidPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cBidPrice.Width = 75;
            // 
            // cBidVolume
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cBidVolume.DefaultCellStyle = dataGridViewCellStyle6;
            this.cBidVolume.HeaderText = "买量";
            this.cBidVolume.Name = "cBidVolume";
            this.cBidVolume.ReadOnly = true;
            this.cBidVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cBidVolume.Width = 50;
            // 
            // cAskPrice
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cAskPrice.DefaultCellStyle = dataGridViewCellStyle7;
            this.cAskPrice.HeaderText = "卖价";
            this.cAskPrice.Name = "cAskPrice";
            this.cAskPrice.ReadOnly = true;
            this.cAskPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cAskPrice.Width = 75;
            // 
            // cAskVolume
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cAskVolume.DefaultCellStyle = dataGridViewCellStyle8;
            this.cAskVolume.HeaderText = "卖量";
            this.cAskVolume.Name = "cAskVolume";
            this.cAskVolume.ReadOnly = true;
            this.cAskVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cAskVolume.Width = 50;
            // 
            // cVolume
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cVolume.DefaultCellStyle = dataGridViewCellStyle9;
            this.cVolume.HeaderText = "成交量";
            this.cVolume.Name = "cVolume";
            this.cVolume.ReadOnly = true;
            this.cVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cVolume.Width = 70;
            // 
            // cOpenInterest
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cOpenInterest.DefaultCellStyle = dataGridViewCellStyle10;
            this.cOpenInterest.HeaderText = "持仓量";
            this.cOpenInterest.Name = "cOpenInterest";
            this.cOpenInterest.ReadOnly = true;
            this.cOpenInterest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOpenInterest.Width = 70;
            // 
            // cUpperLimitPrice
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cUpperLimitPrice.DefaultCellStyle = dataGridViewCellStyle11;
            this.cUpperLimitPrice.HeaderText = "涨停价";
            this.cUpperLimitPrice.Name = "cUpperLimitPrice";
            this.cUpperLimitPrice.ReadOnly = true;
            this.cUpperLimitPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cUpperLimitPrice.Width = 75;
            // 
            // cLowerLimitPrice
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cLowerLimitPrice.DefaultCellStyle = dataGridViewCellStyle12;
            this.cLowerLimitPrice.HeaderText = "跌停价";
            this.cLowerLimitPrice.Name = "cLowerLimitPrice";
            this.cLowerLimitPrice.ReadOnly = true;
            this.cLowerLimitPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLowerLimitPrice.Width = 75;
            // 
            // cOpenPrice
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cOpenPrice.DefaultCellStyle = dataGridViewCellStyle13;
            this.cOpenPrice.HeaderText = "今开盘";
            this.cOpenPrice.Name = "cOpenPrice";
            this.cOpenPrice.ReadOnly = true;
            this.cOpenPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOpenPrice.Width = 75;
            // 
            // cPreSettlementPrice
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPreSettlementPrice.DefaultCellStyle = dataGridViewCellStyle14;
            this.cPreSettlementPrice.HeaderText = "昨结算";
            this.cPreSettlementPrice.Name = "cPreSettlementPrice";
            this.cPreSettlementPrice.ReadOnly = true;
            this.cPreSettlementPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPreSettlementPrice.Width = 75;
            // 
            // cHighestPrice
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cHighestPrice.DefaultCellStyle = dataGridViewCellStyle15;
            this.cHighestPrice.HeaderText = "最高价";
            this.cHighestPrice.Name = "cHighestPrice";
            this.cHighestPrice.ReadOnly = true;
            this.cHighestPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cHighestPrice.Width = 75;
            // 
            // cLowestPrice
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cLowestPrice.DefaultCellStyle = dataGridViewCellStyle16;
            this.cLowestPrice.HeaderText = "最低价";
            this.cLowestPrice.Name = "cLowestPrice";
            this.cLowestPrice.ReadOnly = true;
            this.cLowestPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLowestPrice.Width = 75;
            // 
            // cCurrentVolume
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cCurrentVolume.DefaultCellStyle = dataGridViewCellStyle17;
            this.cCurrentVolume.HeaderText = "现量";
            this.cCurrentVolume.Name = "cCurrentVolume";
            this.cCurrentVolume.ReadOnly = true;
            this.cCurrentVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCurrentVolume.Width = 60;
            // 
            // cRate
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cRate.DefaultCellStyle = dataGridViewCellStyle18;
            this.cRate.HeaderText = "涨跌幅";
            this.cRate.Name = "cRate";
            this.cRate.ReadOnly = true;
            this.cRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cRate.Width = 70;
            // 
            // cPreClosePrice
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cPreClosePrice.DefaultCellStyle = dataGridViewCellStyle19;
            this.cPreClosePrice.HeaderText = "昨收盘";
            this.cPreClosePrice.Name = "cPreClosePrice";
            this.cPreClosePrice.ReadOnly = true;
            this.cPreClosePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPreClosePrice.Width = 75;
            // 
            // cTurnover
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cTurnover.DefaultCellStyle = dataGridViewCellStyle20;
            this.cTurnover.HeaderText = "成交额";
            this.cTurnover.Name = "cTurnover";
            this.cTurnover.ReadOnly = true;
            this.cTurnover.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cTurnover.Width = 105;
            // 
            // cExchange
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cExchange.DefaultCellStyle = dataGridViewCellStyle21;
            this.cExchange.HeaderText = "交易所";
            this.cExchange.Name = "cExchange";
            this.cExchange.ReadOnly = true;
            this.cExchange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cExchange.Width = 65;
            // 
            // cUpdateTime
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cUpdateTime.DefaultCellStyle = dataGridViewCellStyle22;
            this.cUpdateTime.HeaderText = "更新时间";
            this.cUpdateTime.Name = "cUpdateTime";
            this.cUpdateTime.ReadOnly = true;
            this.cUpdateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cUpdateTime.Width = 80;
            // 
            // QuotePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTable);
            this.Name = "QuotePanel";
            this.Size = new System.Drawing.Size(1200, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataTable;
        private DataGridViewTextBoxColumn cInstrumentID;
        private DataGridViewTextBoxColumn cInstrumentName;
        private DataGridViewTextBoxColumn cLastPrice;
        private DataGridViewTextBoxColumn cDeta;
        private DataGridViewTextBoxColumn cBidPrice;
        private DataGridViewTextBoxColumn cBidVolume;
        private DataGridViewTextBoxColumn cAskPrice;
        private DataGridViewTextBoxColumn cAskVolume;
        private DataGridViewTextBoxColumn cVolume;
        private DataGridViewTextBoxColumn cOpenInterest;
        private DataGridViewTextBoxColumn cUpperLimitPrice;
        private DataGridViewTextBoxColumn cLowerLimitPrice;
        private DataGridViewTextBoxColumn cOpenPrice;
        private DataGridViewTextBoxColumn cPreSettlementPrice;
        private DataGridViewTextBoxColumn cHighestPrice;
        private DataGridViewTextBoxColumn cLowestPrice;
        private DataGridViewTextBoxColumn cCurrentVolume;
        private DataGridViewTextBoxColumn cRate;
        private DataGridViewTextBoxColumn cPreClosePrice;
        private DataGridViewTextBoxColumn cTurnover;
        private DataGridViewTextBoxColumn cExchange;
        private DataGridViewTextBoxColumn cUpdateTime;
    }
}
