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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.cOptionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBidQuote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBidPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMarketPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAskPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAskQuote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLongVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLongAveragePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cShortVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cShortAveragePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cImpliedVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDelta = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.cBidQuote,
            this.cBidPrice,
            this.cMarketPrice,
            this.cAskPrice,
            this.cAskQuote,
            this.cLongVolume,
            this.cLongAveragePrice,
            this.cShortVolume,
            this.cShortAveragePrice,
            this.cImpliedVolatility,
            this.cDelta,
            this.cRunningStatus});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.EnableHeadersVisualStyles = false;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.Margin = new System.Windows.Forms.Padding(0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.RowHeadersVisible = false;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dataTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(786, 96);
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
            // 
            // cBidQuote
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "f1";
            this.cBidQuote.DefaultCellStyle = dataGridViewCellStyle3;
            this.cBidQuote.HeaderText = "买入报价";
            this.cBidQuote.Name = "cBidQuote";
            this.cBidQuote.ReadOnly = true;
            this.cBidQuote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cBidQuote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cBidQuote.Width = 80;
            // 
            // cBidPrice
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "f1";
            this.cBidPrice.DefaultCellStyle = dataGridViewCellStyle4;
            this.cBidPrice.HeaderText = "买入价格";
            this.cBidPrice.Name = "cBidPrice";
            this.cBidPrice.ReadOnly = true;
            this.cBidPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cBidPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cBidPrice.Width = 80;
            // 
            // cMarketPrice
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.Format = "F1";
            dataGridViewCellStyle5.NullValue = null;
            this.cMarketPrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.cMarketPrice.HeaderText = "最新价";
            this.cMarketPrice.Name = "cMarketPrice";
            this.cMarketPrice.ReadOnly = true;
            this.cMarketPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cMarketPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cMarketPrice.Width = 80;
            // 
            // cAskPrice
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "f1";
            this.cAskPrice.DefaultCellStyle = dataGridViewCellStyle6;
            this.cAskPrice.HeaderText = "卖出价格";
            this.cAskPrice.Name = "cAskPrice";
            this.cAskPrice.ReadOnly = true;
            this.cAskPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAskPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cAskPrice.Width = 80;
            // 
            // cAskQuote
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "F1";
            this.cAskQuote.DefaultCellStyle = dataGridViewCellStyle7;
            this.cAskQuote.HeaderText = "卖出报价";
            this.cAskQuote.Name = "cAskQuote";
            this.cAskQuote.ReadOnly = true;
            this.cAskQuote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAskQuote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cAskQuote.Width = 80;
            // 
            // cLongVolume
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.cLongVolume.DefaultCellStyle = dataGridViewCellStyle8;
            this.cLongVolume.HeaderText = "多头仓位";
            this.cLongVolume.Name = "cLongVolume";
            this.cLongVolume.ReadOnly = true;
            this.cLongVolume.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLongVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLongVolume.Width = 80;
            // 
            // cLongAveragePrice
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Format = "F2";
            this.cLongAveragePrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.cLongAveragePrice.HeaderText = "多头均价";
            this.cLongAveragePrice.Name = "cLongAveragePrice";
            this.cLongAveragePrice.ReadOnly = true;
            this.cLongAveragePrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLongAveragePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cLongAveragePrice.Width = 80;
            // 
            // cShortVolume
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.Format = "N0";
            dataGridViewCellStyle10.NullValue = null;
            this.cShortVolume.DefaultCellStyle = dataGridViewCellStyle10;
            this.cShortVolume.HeaderText = "空头仓位";
            this.cShortVolume.Name = "cShortVolume";
            this.cShortVolume.ReadOnly = true;
            this.cShortVolume.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cShortVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cShortVolume.Width = 80;
            // 
            // cShortAveragePrice
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Format = "F2";
            this.cShortAveragePrice.DefaultCellStyle = dataGridViewCellStyle11;
            this.cShortAveragePrice.HeaderText = "空头均价";
            this.cShortAveragePrice.Name = "cShortAveragePrice";
            this.cShortAveragePrice.ReadOnly = true;
            this.cShortAveragePrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cShortAveragePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cImpliedVolatility
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.Format = "p2";
            dataGridViewCellStyle12.NullValue = null;
            this.cImpliedVolatility.DefaultCellStyle = dataGridViewCellStyle12;
            this.cImpliedVolatility.HeaderText = "隐含波动率";
            this.cImpliedVolatility.Name = "cImpliedVolatility";
            this.cImpliedVolatility.ReadOnly = true;
            this.cImpliedVolatility.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cImpliedVolatility.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cDelta
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle13.Format = "F6";
            dataGridViewCellStyle13.NullValue = null;
            this.cDelta.DefaultCellStyle = dataGridViewCellStyle13;
            this.cDelta.HeaderText = "Delta";
            this.cDelta.Name = "cDelta";
            this.cDelta.ReadOnly = true;
            this.cDelta.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cDelta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cDelta.Width = 80;
            // 
            // cRunningStatus
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cRunningStatus.DefaultCellStyle = dataGridViewCellStyle14;
            this.cRunningStatus.HeaderText = "运行状态";
            this.cRunningStatus.Name = "cRunningStatus";
            this.cRunningStatus.ReadOnly = true;
            this.cRunningStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRunningStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cRunningStatus.Width = 80;
            // 
            // OptionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.dataTable);
            this.Name = "OptionPanel";
            this.Size = new System.Drawing.Size(786, 96);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DataGridView dataTable;
        private DataGridViewTextBoxColumn cOptionID;
        private DataGridViewTextBoxColumn cBidQuote;
        private DataGridViewTextBoxColumn cBidPrice;
        private DataGridViewTextBoxColumn cMarketPrice;
        private DataGridViewTextBoxColumn cAskPrice;
        private DataGridViewTextBoxColumn cAskQuote;
        private DataGridViewTextBoxColumn cLongVolume;
        private DataGridViewTextBoxColumn cLongAveragePrice;
        private DataGridViewTextBoxColumn cShortVolume;
        private DataGridViewTextBoxColumn cShortAveragePrice;
        private DataGridViewTextBoxColumn cImpliedVolatility;
        private DataGridViewTextBoxColumn cDelta;
        private DataGridViewTextBoxColumn cRunningStatus;

    }
}
