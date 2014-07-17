using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTP;

namespace OptionMM
{
    /// <summary>
    /// 一个报价行
    /// </summary>
    class Quote : DataGridViewRow
    {
        /// <summary>
        /// Call
        /// </summary>
        public ActiveContract call;

        /// <summary>
        /// Put
        /// </summary>
        public ActiveContract put;

        /// <summary>
        /// Underlying
        /// </summary>
        public ActiveContract underlying;

        /// <summary>
        /// 所属行情面板
        /// </summary>
        public QuotePanel Panel { get; private set; }

        /// <summary>
        /// 报价界面刷新定时器
        /// </summary>
        private System.Threading.Timer QuoteRefreshTimer;

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        /// <param name="Panel">所属行情面板</param>
        /// <param name="activeContract">合约</param>
        public Quote(ActiveContract call, ActiveContract put, ActiveContract underlying)
        {
            this.call = call;
            this.call.MarketUpdated += call_MarketUpdated;
            //this.call_MarketUpdated(this, EventArgs.Empty);
            this.put = put;
            this.put.MarketUpdated += put_MarketUpdated;
            //this.put_MarketUpdated(this, EventArgs.Empty);
            this.underlying = underlying;
            this.underlying.MarketUpdated += underlying_MarketUpdated;
            //this.underlying_MarketUpdated(this, EventArgs.Empty);
            this.QuoteRefreshTimer = new System.Threading.Timer(this.QuoteRefreshCallback, null, 1000, 1000);
        }

        /// <summary>
        /// 报价面板更新回调
        /// </summary>
        /// <param name="state"></param>
        private void QuoteRefreshCallback(object state)
        {
            try
            {
                lock (new object())
                {
                    if (Panel.InvokeRequired)
                    {
                        Panel.BeginInvoke(new MethodInvoker(this.RefreshQuote));
                    }
                    else
                    {
                        this.RefreshQuote();
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 刷新行
        /// </summary>
        private void RefreshQuote()
        {
            if(call.MarketData == null || call.PreMarketData == null || put.MarketData == null || put.PreMarketData == null)
            {
                return;
            }
            //this.Cells[0].Value = quote.call.LongPosition.Position;
            //this.Cells[1].Value = quote.call.LongPosition.TodayPosition;
            //this.Cells[2].Value = "";
            setPriceCell(this.Cells[3], call.MarketData.BidPrice1, call.PreMarketData.LastPrice, call.MarketData.PreClosePrice);
            //this.Cells[4].Value = "";
            setPriceCell(this.Cells[5], call.MarketData.AskPrice1, call.PreMarketData.LastPrice, call.MarketData.PreClosePrice);
            //this.Cells[6].Value = "";
            //this.Cells[7].Value = quote.call.ShortPosition.TodayPosition;
            //this.Cells[8].Value = quote.call.ShortPosition.Position;
            //this.Cells[9].Value = "";
            //string[] temp = quote.call.Contract.InstrumentID.Split('-');
            //this.Cells[10].Value = temp[0] + " " + temp[2];
            //this.Cells[11].Value = "";
            //this.Cells[12].Value = quote.put.LongPosition.Position;
            //this.Cells[13].Value = quote.put.LongPosition.TodayPosition;
            //this.Cells[14].Value = "";
            setPriceCell(this.Cells[15], put.MarketData.BidPrice1, put.PreMarketData.LastPrice, put.MarketData.PreClosePrice);
            //this.Cells[16].Value = "";
            setPriceCell(this.Cells[17], put.MarketData.AskPrice1, put.PreMarketData.LastPrice, put.MarketData.PreClosePrice);
            //this.Cells[18].Value = "";
            //this.Cells[19].Value = quote.put.ShortPosition.TodayPosition;
            //this.Cells[20].Value = quote.put.ShortPosition.Position; 
        } 

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(QuotePanel panel)
        {
            this.Panel = panel;
        }

        /// <summary>
        /// Call行情更新时该方法被调用
        /// </summary>
        void call_MarketUpdated(object sender, EventArgs e)
        {
            ThostFtdcDepthMarketDataField marketData = call.MarketData;
            if (marketData != null)
            {
                ThostFtdcDepthMarketDataField preMarketData = call.PreMarketData ?? marketData;
                //计算期权相关值

                

            }
        }

        /// <summary>
        /// Put行情更新时该方法被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void put_MarketUpdated(object sender, EventArgs e)
        {
            ThostFtdcDepthMarketDataField marketData = put.MarketData;
            if (marketData != null)
            {
                ThostFtdcDepthMarketDataField preMarketData = put.PreMarketData ?? marketData;
                //计算期权相关值

            }
        }

        /// <summary>
        /// 设置价格单元格
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="price"></param>
        /// <param name="prePrice"></param>
        /// <param name="preSettle"></param>
        static void setPriceCell(DataGridViewCell cell, double price, double prePrice, double preSettle)
        {
            if (price > preSettle)
                cell.Style.SelectionForeColor =
                cell.Style.ForeColor = Color.Red;
            else if (price == preSettle)
                cell.Style.SelectionForeColor =
                cell.Style.ForeColor = Color.DarkOrange;
            else
                cell.Style.SelectionForeColor =
                cell.Style.ForeColor = Color.Green;

            if (price > prePrice)
                cell.Style.SelectionBackColor =
                cell.Style.BackColor = Color.LightPink;
            else if (price == prePrice)
                cell.Style.SelectionBackColor =
                cell.Style.BackColor = Color.Empty;
            else
                cell.Style.SelectionBackColor =
                cell.Style.BackColor = Color.LightGreen;

            cell.Value = price;
        }

        /// <summary>
        /// Underlying行情更新时该方法被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void underlying_MarketUpdated(object sender, EventArgs e)
        {
            ThostFtdcDepthMarketDataField marketData = underlying.MarketData;
            if (marketData != null)
            {
                ThostFtdcDepthMarketDataField preMarketData = underlying.PreMarketData ?? marketData;


            }
        }

        /// <summary>
        /// 释放用到的资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.call.MarketUpdated -= this.call_MarketUpdated;
            this.put.MarketUpdated -= this.put_MarketUpdated;
            this.underlying.MarketUpdated -= this.underlying_MarketUpdated;
        }
    }
}
