namespace Option
{
    partial class HedgeRecordPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.dataTable = new EQuant.Client.DoubleBufferedDataGridView();
            this.cAdjustPositionDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDelta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHedgeVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAdjustVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStrikePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUnderlyingPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOptionProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCloseProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPositionProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCommission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSlippage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTotalProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAccumulatedProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.cAdjustPositionDateTime,
            this.cOptionType,
            this.cOptionPrice,
            this.cDelta,
            this.cVolatility,
            this.cHedgeVolume,
            this.cAdjustVolume,
            this.cStrikePrice,
            this.cUnderlyingPrice,
            this.cOptionProfit,
            this.cCloseProfit,
            this.cPositionProfit,
            this.cCommission,
            this.cSlippage,
            this.cTotalProfit,
            this.cAccumulatedProfit});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle18;
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.EnableHeadersVisualStyles = false;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.Margin = new System.Windows.Forms.Padding(0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.RowHeadersVisible = false;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.RowsDefaultCellStyle = dataGridViewCellStyle19;
            this.dataTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(790, 100);
            this.dataTable.TabIndex = 0;
            // 
            // cAdjustPositionDateTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.Format = "g";
            dataGridViewCellStyle2.NullValue = null;
            this.cAdjustPositionDateTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.cAdjustPositionDateTime.HeaderText = "调仓时刻";
            this.cAdjustPositionDateTime.Name = "cAdjustPositionDateTime";
            this.cAdjustPositionDateTime.ReadOnly = true;
            this.cAdjustPositionDateTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAdjustPositionDateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cAdjustPositionDateTime.Width = 120;
            // 
            // cOptionType
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.NullValue = null;
            this.cOptionType.DefaultCellStyle = dataGridViewCellStyle3;
            this.cOptionType.HeaderText = "期权类型";
            this.cOptionType.Name = "cOptionType";
            this.cOptionType.ReadOnly = true;
            this.cOptionType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOptionType.Width = 80;
            // 
            // cOptionPrice
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.Format = "F1";
            dataGridViewCellStyle4.NullValue = null;
            this.cOptionPrice.DefaultCellStyle = dataGridViewCellStyle4;
            this.cOptionPrice.HeaderText = "期权价格";
            this.cOptionPrice.Name = "cOptionPrice";
            this.cOptionPrice.ReadOnly = true;
            this.cOptionPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cDelta
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.Format = "F6";
            dataGridViewCellStyle5.NullValue = null;
            this.cDelta.DefaultCellStyle = dataGridViewCellStyle5;
            this.cDelta.HeaderText = "Delta";
            this.cDelta.Name = "cDelta";
            this.cDelta.ReadOnly = true;
            this.cDelta.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cDelta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cVolatility
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.Format = "F6";
            dataGridViewCellStyle6.NullValue = null;
            this.cVolatility.DefaultCellStyle = dataGridViewCellStyle6;
            this.cVolatility.HeaderText = "波动率";
            this.cVolatility.Name = "cVolatility";
            this.cVolatility.ReadOnly = true;
            this.cVolatility.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cVolatility.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cHedgeVolume
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.Format = "F6";
            dataGridViewCellStyle7.NullValue = null;
            this.cHedgeVolume.DefaultCellStyle = dataGridViewCellStyle7;
            this.cHedgeVolume.HeaderText = "对冲数量";
            this.cHedgeVolume.Name = "cHedgeVolume";
            this.cHedgeVolume.ReadOnly = true;
            this.cHedgeVolume.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cHedgeVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAdjustVolume
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.Format = "F6";
            this.cAdjustVolume.DefaultCellStyle = dataGridViewCellStyle8;
            this.cAdjustVolume.HeaderText = "调整手数";
            this.cAdjustVolume.Name = "cAdjustVolume";
            this.cAdjustVolume.ReadOnly = true;
            this.cAdjustVolume.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAdjustVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cStrikePrice
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.Format = "F6";
            this.cStrikePrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.cStrikePrice.HeaderText = "交割价格";
            this.cStrikePrice.Name = "cStrikePrice";
            this.cStrikePrice.ReadOnly = true;
            this.cStrikePrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cStrikePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cUnderlyingPrice
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.Format = "F6";
            this.cUnderlyingPrice.DefaultCellStyle = dataGridViewCellStyle10;
            this.cUnderlyingPrice.HeaderText = "标的价格";
            this.cUnderlyingPrice.Name = "cUnderlyingPrice";
            this.cUnderlyingPrice.ReadOnly = true;
            this.cUnderlyingPrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cUnderlyingPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cOptionProfit
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.Format = "F6";
            dataGridViewCellStyle11.NullValue = null;
            this.cOptionProfit.DefaultCellStyle = dataGridViewCellStyle11;
            this.cOptionProfit.HeaderText = "期权盈亏";
            this.cOptionProfit.Name = "cOptionProfit";
            this.cOptionProfit.ReadOnly = true;
            this.cOptionProfit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionProfit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cCloseProfit
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.Format = "F6";
            dataGridViewCellStyle12.NullValue = null;
            this.cCloseProfit.DefaultCellStyle = dataGridViewCellStyle12;
            this.cCloseProfit.HeaderText = "平仓盈亏";
            this.cCloseProfit.Name = "cCloseProfit";
            this.cCloseProfit.ReadOnly = true;
            this.cCloseProfit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCloseProfit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // cCommission
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.Format = "F6";
            this.cCommission.DefaultCellStyle = dataGridViewCellStyle14;
            this.cCommission.HeaderText = "手续费";
            this.cCommission.Name = "cCommission";
            this.cCommission.ReadOnly = true;
            this.cCommission.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCommission.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSlippage
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle15.Format = "F6";
            dataGridViewCellStyle15.NullValue = null;
            this.cSlippage.DefaultCellStyle = dataGridViewCellStyle15;
            this.cSlippage.HeaderText = "滑点";
            this.cSlippage.Name = "cSlippage";
            this.cSlippage.ReadOnly = true;
            this.cSlippage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cSlippage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cTotalProfit
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle16.Format = "F6";
            dataGridViewCellStyle16.NullValue = null;
            this.cTotalProfit.DefaultCellStyle = dataGridViewCellStyle16;
            this.cTotalProfit.HeaderText = "总盈亏";
            this.cTotalProfit.Name = "cTotalProfit";
            this.cTotalProfit.ReadOnly = true;
            this.cTotalProfit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cTotalProfit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAccumulatedProfit
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle17.Format = "F6";
            dataGridViewCellStyle17.NullValue = null;
            this.cAccumulatedProfit.DefaultCellStyle = dataGridViewCellStyle17;
            this.cAccumulatedProfit.HeaderText = "累计盈亏";
            this.cAccumulatedProfit.Name = "cAccumulatedProfit";
            this.cAccumulatedProfit.ReadOnly = true;
            this.cAccumulatedProfit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAccumulatedProfit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HedgeRecordPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTable);
            this.Name = "HedgeRecordPanel";
            this.Size = new System.Drawing.Size(790, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DoubleBufferedDataGridView dataTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAdjustPositionDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOptionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOptionPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDelta;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVolatility;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHedgeVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAdjustVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStrikePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUnderlyingPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOptionProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCloseProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPositionProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCommission;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSlippage;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTotalProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAccumulatedProfit;

    }
}
