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
    internal partial class OptionPanel : UserControl
    {
        /// <summary>
        /// 构造一个新实例
        /// </summary>
        public OptionPanel()
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
        /// <param name="strategy"></param>
        public void AddStrategy(Strategy strategy)
        {
            strategy.SetPanel(this);
            strategy.CreateCells(this.dataTable);
            DataGridViewCellCollection cells = strategy.Cells;
            cells[0].Value = strategy.Option.InstrumentID;
            cells[1].Value = strategy.Option.MMQuotation.BidQuote;
            cells[2].Value = strategy.Option.LastMarket != null ? strategy.Option.LastMarket.BidPrice1 : 0;
            cells[3].Value = strategy.Option.LastMarket != null ? strategy.Option.LastMarket.LastPrice : 0;
            cells[4].Value = strategy.Option.LastMarket != null ? strategy.Option.LastMarket.AskPrice1 : 0;
            cells[5].Value = strategy.Option.MMQuotation.AskQuote;
            cells[6].Value = strategy.Option.longPosition != null ? strategy.Option.longPosition.Position : 0;
            cells[7].Value = strategy.Option.shortPosition != null ? strategy.Option.shortPosition.Position : 0;
            cells[8].Value = strategy.Option.ImpliedVolatility;
            cells[9].Value = strategy.Option.OptionValue.Delta;
            cells[10].Value = "停止";
            this.dataTable.Rows.Add(strategy);
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
