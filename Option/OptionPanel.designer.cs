using System.Windows.Forms;
namespace OptionMM
{
    partial class OptionPanel
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
            this.Release();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cOptionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cImpridVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDelta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTheroricalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRealPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionPositionThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMiniumOptionOpenPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionLongPositionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionShortPositionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIndexLongPositionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIndexShortPositionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPositionProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionMaximumPositionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIndexMaximumPositionNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBidPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAskPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRunningStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dataTable
            // 
            this.dataTable.AllowUserToAddRows = false;
            this.dataTable.AllowUserToDeleteRows = false;
            this.dataTable.AllowUserToResizeRows = false;
            this.dataTable.BackgroundColor = System.Drawing.SystemColors.WindowText;
            this.dataTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataTable.ColumnHeadersHeight = 21;
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cOptionID,
            this.cImpridVolatility,
            this.cDelta,
            this.cTheroricalPrice,
            this.cRealPrice,
            this.cOptionPositionThreshold,
            this.cMiniumOptionOpenPosition,
            this.cOptionLongPositionNum,
            this.cOptionShortPositionNum,
            this.cIndexLongPositionNum,
            this.cIndexShortPositionNum,
            this.cPositionProfit,
            this.cOptionMaximumPositionNum,
            this.cIndexMaximumPositionNum,
            this.cBidPrice,
            this.cAskPrice,
            this.cRunningStatus});
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle19;
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.EnableHeadersVisualStyles = false;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.Margin = new System.Windows.Forms.Padding(0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.RowHeadersVisible = false;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dataTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(790, 100);
            this.dataTable.TabIndex = 0;
            this.dataTable.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTable_CellMouseDoubleClick);
            this.dataTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTable_CellMouseDown);
            // 
            // cOptionID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.Format = "g";
            dataGridViewCellStyle2.NullValue = null;
            this.cOptionID.DefaultCellStyle = dataGridViewCellStyle2;
            this.cOptionID.HeaderText = "期权代码";
            this.cOptionID.Name = "cOptionID";
            this.cOptionID.ReadOnly = true;
            this.cOptionID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOptionID.Width = 120;
            // 
            // cImpridVolatility
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.NullValue = null;
            this.cImpridVolatility.DefaultCellStyle = dataGridViewCellStyle3;
            this.cImpridVolatility.HeaderText = "隐含波动率";
            this.cImpridVolatility.Name = "cImpridVolatility";
            this.cImpridVolatility.ReadOnly = true;
            this.cImpridVolatility.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cImpridVolatility.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cDelta
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.Format = "F6";
            dataGridViewCellStyle4.NullValue = null;
            this.cDelta.DefaultCellStyle = dataGridViewCellStyle4;
            this.cDelta.HeaderText = "Delta";
            this.cDelta.Name = "cDelta";
            this.cDelta.ReadOnly = true;
            this.cDelta.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cDelta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cDelta.Width = 80;
            // 
            // cTheroricalPrice
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.Format = "F1";
            dataGridViewCellStyle5.NullValue = null;
            this.cTheroricalPrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.cTheroricalPrice.HeaderText = "理论价格";
            this.cTheroricalPrice.Name = "cTheroricalPrice";
            this.cTheroricalPrice.ReadOnly = true;
            this.cTheroricalPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cTheroricalPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cRealPrice
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "F1";
            this.cRealPrice.DefaultCellStyle = dataGridViewCellStyle6;
            this.cRealPrice.HeaderText = "实际价格";
            this.cRealPrice.Name = "cRealPrice";
            // 
            // cOptionPositionThreshold
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.Format = "N1";
            dataGridViewCellStyle7.NullValue = null;
            this.cOptionPositionThreshold.DefaultCellStyle = dataGridViewCellStyle7;
            this.cOptionPositionThreshold.HeaderText = "开仓阈值";
            this.cOptionPositionThreshold.Name = "cOptionPositionThreshold";
            this.cOptionPositionThreshold.ReadOnly = true;
            this.cOptionPositionThreshold.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionPositionThreshold.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cMiniumOptionOpenPosition
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.cMiniumOptionOpenPosition.DefaultCellStyle = dataGridViewCellStyle8;
            this.cMiniumOptionOpenPosition.HeaderText = "最少开仓数";
            this.cMiniumOptionOpenPosition.Name = "cMiniumOptionOpenPosition";
            this.cMiniumOptionOpenPosition.ReadOnly = true;
            this.cMiniumOptionOpenPosition.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cMiniumOptionOpenPosition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cOptionLongPositionNum
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.Format = "N0";
            dataGridViewCellStyle9.NullValue = null;
            this.cOptionLongPositionNum.DefaultCellStyle = dataGridViewCellStyle9;
            this.cOptionLongPositionNum.HeaderText = "期权多头仓位数";
            this.cOptionLongPositionNum.Name = "cOptionLongPositionNum";
            this.cOptionLongPositionNum.ReadOnly = true;
            this.cOptionLongPositionNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionLongPositionNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOptionLongPositionNum.Width = 150;
            // 
            // cOptionShortPositionNum
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = null;
            this.cOptionShortPositionNum.DefaultCellStyle = dataGridViewCellStyle10;
            this.cOptionShortPositionNum.HeaderText = "期权空头仓位数";
            this.cOptionShortPositionNum.Name = "cOptionShortPositionNum";
            this.cOptionShortPositionNum.ReadOnly = true;
            this.cOptionShortPositionNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionShortPositionNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOptionShortPositionNum.Width = 150;
            // 
            // cIndexLongPositionNum
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.Format = "N0";
            dataGridViewCellStyle11.NullValue = null;
            this.cIndexLongPositionNum.DefaultCellStyle = dataGridViewCellStyle11;
            this.cIndexLongPositionNum.HeaderText = "股指多头仓位数";
            this.cIndexLongPositionNum.Name = "cIndexLongPositionNum";
            this.cIndexLongPositionNum.ReadOnly = true;
            this.cIndexLongPositionNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIndexLongPositionNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIndexLongPositionNum.Width = 150;
            // 
            // cIndexShortPositionNum
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.Format = "N0";
            dataGridViewCellStyle12.NullValue = null;
            this.cIndexShortPositionNum.DefaultCellStyle = dataGridViewCellStyle12;
            this.cIndexShortPositionNum.HeaderText = "股指空头仓位数";
            this.cIndexShortPositionNum.Name = "cIndexShortPositionNum";
            this.cIndexShortPositionNum.ReadOnly = true;
            this.cIndexShortPositionNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIndexShortPositionNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cIndexShortPositionNum.Width = 150;
            // 
            // cPositionProfit
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.Format = "F6";
            dataGridViewCellStyle13.NullValue = null;
            this.cPositionProfit.DefaultCellStyle = dataGridViewCellStyle13;
            this.cPositionProfit.HeaderText = "持仓盈亏";
            this.cPositionProfit.Name = "cPositionProfit";
            this.cPositionProfit.ReadOnly = true;
            this.cPositionProfit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPositionProfit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cOptionMaximumPositionNum
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.Format = "F6";
            dataGridViewCellStyle14.NullValue = null;
            this.cOptionMaximumPositionNum.DefaultCellStyle = dataGridViewCellStyle14;
            this.cOptionMaximumPositionNum.HeaderText = "期权限仓数";
            this.cOptionMaximumPositionNum.Name = "cOptionMaximumPositionNum";
            this.cOptionMaximumPositionNum.ReadOnly = true;
            this.cOptionMaximumPositionNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionMaximumPositionNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIndexMaximumPositionNum
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.Format = "F6";
            this.cIndexMaximumPositionNum.DefaultCellStyle = dataGridViewCellStyle15;
            this.cIndexMaximumPositionNum.HeaderText = "股指限仓数";
            this.cIndexMaximumPositionNum.Name = "cIndexMaximumPositionNum";
            this.cIndexMaximumPositionNum.ReadOnly = true;
            this.cIndexMaximumPositionNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIndexMaximumPositionNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cBidPrice
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Format = "f1";
            this.cBidPrice.DefaultCellStyle = dataGridViewCellStyle16;
            this.cBidPrice.HeaderText = "买入报价";
            this.cBidPrice.Name = "cBidPrice";
            // 
            // cAskPrice
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.Format = "f1";
            this.cAskPrice.DefaultCellStyle = dataGridViewCellStyle17;
            this.cAskPrice.HeaderText = "卖出报价";
            this.cAskPrice.Name = "cAskPrice";
            // 
            // cRunningStatus
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cRunningStatus.DefaultCellStyle = dataGridViewCellStyle18;
            this.cRunningStatus.HeaderText = "运行状态";
            this.cRunningStatus.Name = "cRunningStatus";
            this.cRunningStatus.ReadOnly = true;
            this.cRunningStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRunningStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OptionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTable);
            this.Name = "OptionPanel";
            this.Size = new System.Drawing.Size(790, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DataGridView dataTable;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DataGridViewTextBoxColumn cOptionID;
        private DataGridViewTextBoxColumn cImpridVolatility;
        private DataGridViewTextBoxColumn cDelta;
        private DataGridViewTextBoxColumn cTheroricalPrice;
        private DataGridViewTextBoxColumn cRealPrice;
        private DataGridViewTextBoxColumn cOptionPositionThreshold;
        private DataGridViewTextBoxColumn cMiniumOptionOpenPosition;
        private DataGridViewTextBoxColumn cOptionLongPositionNum;
        private DataGridViewTextBoxColumn cOptionShortPositionNum;
        private DataGridViewTextBoxColumn cIndexLongPositionNum;
        private DataGridViewTextBoxColumn cIndexShortPositionNum;
        private DataGridViewTextBoxColumn cPositionProfit;
        private DataGridViewTextBoxColumn cOptionMaximumPositionNum;
        private DataGridViewTextBoxColumn cIndexMaximumPositionNum;
        private DataGridViewTextBoxColumn cBidPrice;
        private DataGridViewTextBoxColumn cAskPrice;
        private DataGridViewTextBoxColumn cRunningStatus;

    }
}
