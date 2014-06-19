using System.Windows.Forms;
namespace OptionMM
{
    partial class VerticalPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.cOptionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCoveredInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCoveredAveragePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cParityInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.cVolume,
            this.cCoveredInterval,
            this.cCoveredAveragePrice,
            this.cParityInterval});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataTable.EnableHeadersVisualStyles = false;
            this.dataTable.Location = new System.Drawing.Point(0, 0);
            this.dataTable.Margin = new System.Windows.Forms.Padding(0);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.RowHeadersVisible = false;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataTable.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dataTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataTable.RowTemplate.Height = 40;
            this.dataTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataTable.Size = new System.Drawing.Size(751, 100);
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
            this.cOptionID.HeaderText = "高执行价合约";
            this.cOptionID.Name = "cOptionID";
            this.cOptionID.ReadOnly = true;
            this.cOptionID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cOptionID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cOptionID.Width = 120;
            // 
            // cVolume
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.NullValue = null;
            this.cVolume.DefaultCellStyle = dataGridViewCellStyle3;
            this.cVolume.HeaderText = "高执行价合约方向";
            this.cVolume.Name = "cVolume";
            this.cVolume.ReadOnly = true;
            this.cVolume.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cVolume.Width = 140;
            // 
            // cCoveredInterval
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.Format = "F2";
            dataGridViewCellStyle4.NullValue = null;
            this.cCoveredInterval.DefaultCellStyle = dataGridViewCellStyle4;
            this.cCoveredInterval.HeaderText = "低执行价合约";
            this.cCoveredInterval.Name = "cCoveredInterval";
            this.cCoveredInterval.ReadOnly = true;
            this.cCoveredInterval.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCoveredInterval.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCoveredInterval.Width = 120;
            // 
            // cCoveredAveragePrice
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.NullValue = null;
            this.cCoveredAveragePrice.DefaultCellStyle = dataGridViewCellStyle5;
            this.cCoveredAveragePrice.HeaderText = "低执行价合约方向";
            this.cCoveredAveragePrice.Name = "cCoveredAveragePrice";
            this.cCoveredAveragePrice.ReadOnly = true;
            this.cCoveredAveragePrice.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cCoveredAveragePrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCoveredAveragePrice.Width = 140;
            // 
            // cParityInterval
            // 
            dataGridViewCellStyle6.Format = "F2";
            this.cParityInterval.DefaultCellStyle = dataGridViewCellStyle6;
            this.cParityInterval.HeaderText = "套利区间";
            this.cParityInterval.Name = "cParityInterval";
            this.cParityInterval.ReadOnly = true;
            this.cParityInterval.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // VerticalPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataTable);
            this.Name = "VerticalPanel";
            this.Size = new System.Drawing.Size(751, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DataGridView dataTable;
        private DataGridViewTextBoxColumn cOptionID;
        private DataGridViewTextBoxColumn cVolume;
        private DataGridViewTextBoxColumn cCoveredInterval;
        private DataGridViewTextBoxColumn cCoveredAveragePrice;
        private DataGridViewTextBoxColumn cParityInterval;

    }
}
