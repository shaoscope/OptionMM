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
    internal partial class ParityPanel : UserControl
    {
        /// <summary>
        /// 构造一个新实例
        /// </summary>
        public ParityPanel()
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
        /// <param name="parity"></param>
        public void AddParity(Parity parity)
        {
            parity.SetPanel(this);
            parity.CreateCells(this.dataTable);
            DataGridViewCellCollection cells = parity.Cells;
            cells[0].Value = parity.CallInstrumentID;
            cells[1].Value = parity.PositionVolume;
            cells[2].Value = parity.ParityInterval;
            cells[3].Value = parity.AveragePrice;
            this.dataTable.Rows.Add(parity);
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
