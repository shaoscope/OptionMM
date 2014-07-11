using System.Windows.Forms;
namespace OptionMM
{
    partial class ParityPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.cCallInstrumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCallDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutInstrumentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPutDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cParityInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOpenThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCloseThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMaxOpenSets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOpenedSets = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.cCallInstrumentID,
            this.cCallDirection,
            this.cPutInstrumentID,
            this.cPutDirection,
            this.cParityInterval,
            this.cOpenThreshold,
            this.cCloseThreshold,
            this.cMaxOpenSets,
            this.cOpenedSets});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.EnableHeadersVisualStyles = false;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.Margin = new System.Windows.Forms.Padding(0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.RowHeadersVisible = false;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dataTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(912, 100);
            this.dataTable.TabIndex = 0;
            this.dataTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTable_CellEndEdit);
            this.dataTable.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTable_CellMouseDoubleClick);
            this.dataTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataTable_CellMouseDown);
            // 
            // cCallInstrumentID
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.Format = "g";
            dataGridViewCellStyle2.NullValue = null;
            this.cCallInstrumentID.DefaultCellStyle = dataGridViewCellStyle2;
            this.cCallInstrumentID.HeaderText = "看涨合约代码";
            this.cCallInstrumentID.Name = "cCallInstrumentID";
            this.cCallInstrumentID.ReadOnly = true;
            this.cCallInstrumentID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCallInstrumentID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallInstrumentID.Width = 120;
            // 
            // cCallDirection
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.NullValue = null;
            this.cCallDirection.DefaultCellStyle = dataGridViewCellStyle3;
            this.cCallDirection.HeaderText = "看涨合约方向";
            this.cCallDirection.Name = "cCallDirection";
            this.cCallDirection.ReadOnly = true;
            this.cCallDirection.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCallDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCallDirection.Width = 120;
            // 
            // cPutInstrumentID
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.Format = "F2";
            dataGridViewCellStyle4.NullValue = null;
            this.cPutInstrumentID.DefaultCellStyle = dataGridViewCellStyle4;
            this.cPutInstrumentID.HeaderText = "看跌合约代码";
            this.cPutInstrumentID.Name = "cPutInstrumentID";
            this.cPutInstrumentID.ReadOnly = true;
            this.cPutInstrumentID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPutInstrumentID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutInstrumentID.Width = 120;
            // 
            // cPutDirection
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.NullValue = null;
            this.cPutDirection.DefaultCellStyle = dataGridViewCellStyle5;
            this.cPutDirection.HeaderText = "看跌合约方向";
            this.cPutDirection.Name = "cPutDirection";
            this.cPutDirection.ReadOnly = true;
            this.cPutDirection.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPutDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cPutDirection.Width = 120;
            // 
            // cParityInterval
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "F2";
            this.cParityInterval.DefaultCellStyle = dataGridViewCellStyle6;
            this.cParityInterval.HeaderText = "套利区间";
            this.cParityInterval.Name = "cParityInterval";
            this.cParityInterval.ReadOnly = true;
            this.cParityInterval.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cParityInterval.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cParityInterval.Width = 80;
            // 
            // cOpenThreshold
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "F2";
            this.cOpenThreshold.DefaultCellStyle = dataGridViewCellStyle7;
            this.cOpenThreshold.HeaderText = "开仓区间";
            this.cOpenThreshold.Name = "cOpenThreshold";
            this.cOpenThreshold.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOpenThreshold.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOpenThreshold.Width = 80;
            // 
            // cCloseThreshold
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Format = "F2";
            this.cCloseThreshold.DefaultCellStyle = dataGridViewCellStyle8;
            this.cCloseThreshold.HeaderText = "平仓区间";
            this.cCloseThreshold.Name = "cCloseThreshold";
            this.cCloseThreshold.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCloseThreshold.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCloseThreshold.Width = 80;
            // 
            // cMaxOpenSets
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Format = "N0";
            this.cMaxOpenSets.DefaultCellStyle = dataGridViewCellStyle9;
            this.cMaxOpenSets.HeaderText = "最大开仓数";
            this.cMaxOpenSets.Name = "cMaxOpenSets";
            this.cMaxOpenSets.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cMaxOpenSets.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cOpenedSets
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Format = "N0";
            this.cOpenedSets.DefaultCellStyle = dataGridViewCellStyle10;
            this.cOpenedSets.HeaderText = "已开仓数";
            this.cOpenedSets.Name = "cOpenedSets";
            this.cOpenedSets.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOpenedSets.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOpenedSets.Width = 80;
            // 
            // ParityPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTable);
            this.Name = "ParityPanel";
            this.Size = new System.Drawing.Size(912, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DataGridView dataTable;
        private DataGridViewTextBoxColumn cCallInstrumentID;
        private DataGridViewTextBoxColumn cCallDirection;
        private DataGridViewTextBoxColumn cPutInstrumentID;
        private DataGridViewTextBoxColumn cPutDirection;
        private DataGridViewTextBoxColumn cParityInterval;
        private DataGridViewTextBoxColumn cOpenThreshold;
        private DataGridViewTextBoxColumn cCloseThreshold;
        private DataGridViewTextBoxColumn cMaxOpenSets;
        private DataGridViewTextBoxColumn cOpenedSets;

    }
}
