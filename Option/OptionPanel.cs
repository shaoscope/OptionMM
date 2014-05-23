﻿using System;
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
            cells[0].Value = strategy.Option.InstrumentID;               //合约名 cOptionID
            cells[1].Value = strategy.Option.ImpridVolatility;                     //隐含波动率 cImpridVolatility
            cells[2].Value = strategy.Option.Delta;      //Delta cDelta
            cells[3].Value = strategy.Option.TheoreticalPrice;      //理论价格 cTheroricalPrice
            cells[4].Value = strategy.Option.LastMarket != null ? strategy.Option.LastMarket.LastPrice : 0;     //实际价格 cRealPrice
            cells[5].Value = strategy.optionPositionThreshold;        //开仓阈值 cOptionPositionThreshold
            cells[6].Value = strategy.minOptionOpenLots;     //最少开仓数 cMiniumOptionOpenPosition
            //cells[7].Value = hedgeRecord.AdjustVolume;        //期权多头仓位数 cOptionLongPositionNum
            //cells[8].Value = hedgeRecord.StrikePrice;     //期权空头仓位数 cOptionShortPositionNum
            //cells[9].Value = hedgeRecord.UnderlyingPrice;     //股指多头仓位数 cIndexLongPositionNum
            //cells[10].Value = hedgeRecord.OptionProfit;        //股指空头仓位数 cIndexShortPositionNum
            //cells[11].Value = hedgeRecord.CloseProfit;        //持仓盈亏 cPositionProfit
            //cells[12].Value = hedgeRecord.PositionProfit;     //期权限仓数 cOptionMaximumPositionNum
            //cells[13].Value = hedgeRecord.Commission;             //股指限仓数 cIndexMaximumPositionNum
            cells[16].Value = "停止";
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
