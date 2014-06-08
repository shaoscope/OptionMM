using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionMM
{
    /// <summary>
    /// 显示期权回测报告面板
    /// </summary>
    internal partial class CoveredPanel : UserControl
    {
        /// <summary>
        /// 构造一个新实例
        /// </summary>
        public CoveredPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 窗体载入时
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// 清楚所有记录
        /// </summary>
        public void ClearRecord()
        {
            this.dataTable.Rows.Clear();
        }

        //释放用到的资源
        private void Release()
        {
            foreach (DataGridViewRow row in this.dataTable.Rows)
            {
                row.Dispose();
            }
        }

        /// <summary>
        /// 加入一个策略
        /// </summary>
        /// <param name="covered"></param>
        public void AddCovered(Covered covered)
        {
            covered.SetPanel(this);
            covered.CreateCells(this.dataTable);
            DataGridViewCellCollection cells = covered.Cells;
            cells[0].Value = covered.InstrumentID;
            cells[1].Value = covered.PositionVolume;
            cells[2].Value = covered.CoveredInterval;
            cells[3].Value = covered.AveragePrice;
            this.dataTable.Rows.Add(covered);
        }

        /// <summary>
        /// 双击策略行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataTable_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dataTable.SelectedRows.Count == 1)
            {
                Covered covered = (Covered)this.dataTable.SelectedRows[0].Tag;
            }
        }

        /// <summary>
        /// 选中策略行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataTable_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                this.dataTable.Rows[e.RowIndex].Selected = true;
            }
        }
    }
}
