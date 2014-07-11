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
            cells[0].Value = parity.LongOption.InstrumentID;
            cells[1].Value = parity.CallDirection;
            cells[2].Value = parity.ShortOption.InstrumentID;
            cells[3].Value = parity.PutDirection;
            cells[4].Value = parity.ParityInterval;
            cells[5].Value = parity.OpenThreshold;
            cells[6].Value = parity.CloseThreshold;
            cells[7].Value = parity.MaxOpenSets;
            cells[8].Value = parity.OpenedSets;
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
                Parity parity = (Parity)this.dataTable.SelectedRows[0].Tag;

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

        /// <summary>
        /// 结束单元格编辑时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row;
            DataGridViewCell cell;
            double dTemp;
            switch (e.ColumnIndex)
            {
                case 5:
                    row = this.dataTable.Rows[e.RowIndex];
                    cell = row.Cells[5];
                    if (double.TryParse(cell.Value as string, out dTemp))
                    {
                        ((Parity)row).OpenThreshold = dTemp;
                    }
                    else
                    {
                        cell.Value = ((Parity)row).OpenThreshold;
                    }
                    return;
                case 6:
                    row = this.dataTable.Rows[e.RowIndex];
                    cell = row.Cells[6];
                    if (double.TryParse(cell.Value as string, out dTemp))
                    {
                        ((Parity)row).CloseThreshold = dTemp;
                    }
                    else
                    {
                        cell.Value = ((Parity)row).OpenThreshold;
                    }
                    return;
                case 7:
                    row = this.dataTable.Rows[e.RowIndex];
                    cell = row.Cells[7];
                    if (double.TryParse(cell.Value as string, out dTemp))
                    {
                        ((Parity)row).MaxOpenSets = dTemp;
                    }
                    else
                    {
                        cell.Value = ((Parity)row).OpenThreshold;
                    }
                    return;
                case 8:
                    row = this.dataTable.Rows[e.RowIndex];
                    cell = row.Cells[8];
                    if (double.TryParse(cell.Value as string, out dTemp))
                    {
                        ((Parity)row).OpenedSets = dTemp;
                    }
                    else
                    {
                        cell.Value = ((Parity)row).OpenThreshold;
                    }
                    return;
            }
        }
    }
}
