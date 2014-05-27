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
    internal partial class GreeksPanel : UserControl
    {
        /// <summary>
        /// 构造一个新实例
        /// </summary>
        public GreeksPanel()
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
        /// <param name="greeks"></param>
        public void AddGreeks(Greeks greeks)
        {
            greeks.SetPanel(this);
            greeks.CreateCells(this.dataTable);
            DataGridViewCellCollection cells = greeks.Cells;
            cells[0].Value = greeks.InstrumentID;               //合约名
            cells[1].Value = greeks.Delta;                     //Delta
            cells[2].Value = greeks.Gamma;      //Gamma
            cells[3].Value = greeks.Vega;      //Vega
            cells[4].Value = greeks.Theta;     //Theta
            cells[5].Value = greeks.Rho;        //Rho
            this.dataTable.Rows.Add(greeks);
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
                Strategy strategy = (Strategy)this.dataTable.SelectedRows[0].Tag;
                if(strategy.IsRunning)
                {
                    strategy.Stop();
                }
                else
                {
                    strategy.Start();
                }
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
