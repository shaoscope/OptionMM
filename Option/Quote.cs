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
        /// 构造一个新实例
        /// </summary>
        /// <param name="Panel">所属行情面板</param>
        /// <param name="activeContract">合约</param>
        public Quote(ActiveContract call, ActiveContract put, ActiveContract underlying)
        {
            this.call = call;
            this.call.MarketUpdated += call_MarketUpdated;
            this.call_MarketUpdated(this, EventArgs.Empty);
            this.put = put;
            this.put.MarketUpdated += put_MarketUpdated;
            this.put_MarketUpdated(this, EventArgs.Empty);
            this.underlying = underlying;
            this.underlying.MarketUpdated += underlying_MarketUpdated;
            this.underlying_MarketUpdated(this, EventArgs.Empty);
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


            }
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
