using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTP;
using System.ComponentModel;

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
        private System.Threading.Timer QuotePanelRefreshTimer;

        /// <summary>
        /// 合约是否在报价
        /// </summary>
        [DefaultValue(false)]
        public bool IsQuoting { get; private set; }

        /// <summary>
        /// 有成交时该事件被触发
        /// </summary>
        public event EventHandler OnTrade;

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        /// <param name="Panel">所属行情面板</param>
        /// <param name="activeContract">合约</param>
        public Quote(ActiveContract call, ActiveContract put, ActiveContract underlying)
        {
            this.Tag = this;
            this.call = call;
            this.call.MarketUpdated += call_MarketUpdated;
            this.call.ForQuoteArrived += call_ForQuoteArrived;
            this.put = put;
            this.put.MarketUpdated += put_MarketUpdated;
            this.put.ForQuoteArrived += put_ForQuoteArrived;
            this.underlying = underlying;
            this.underlying.MarketUpdated += underlying_MarketUpdated;
            this.OnTrade += Quote_OnTrade;
            this.QuotePanelRefreshTimer = new System.Threading.Timer(this.QuotePanelRefreshCallback, null, 1000, 1000);
            this.call_MarketUpdated(this, EventArgs.Empty);
            this.put_MarketUpdated(this, EventArgs.Empty);
        }

        /// <summary>
        /// 报价有成交时方法被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Quote_OnTrade(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 启动报价
        /// </summary>
        public void Start()
        {
            this.IsQuoting = true;
            MainForm.Instance.TraderManager.PlaceQuote(this.call.Contract.InstrumentID, EnumOffsetFlagType.Open, 10, this.call.MarketData.UpperLimitPrice, EnumOffsetFlagType.Open, 10, this.call.MarketData.LowerLimitPrice);
        }

        /// <summary>
        /// 暂停报价
        /// </summary>
        public void Stop()
        {
            this.IsQuoting = false;
        }

        /// <summary>
        /// Call询价单到达时方法被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void put_ForQuoteArrived(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Put询价单到达时方法被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void call_ForQuoteArrived(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 报价面板更新回调
        /// </summary>
        /// <param name="state"></param>
        private void QuotePanelRefreshCallback(object state)
        {
            try
            {
                lock (new object())
                {
                    if (Panel.InvokeRequired)
                    {
                        Panel.BeginInvoke(new MethodInvoker(this.RefreshQuotePanel));
                    }
                    else
                    {
                        this.RefreshQuotePanel();
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
        private void RefreshQuotePanel()
        {
            if(call.MarketData == null || call.PreMarketData == null || put.MarketData == null || put.PreMarketData == null)
            {
                return;
            }
            setPriceCell(this.Cells[3], call.MarketData.BidPrice1, call.PreMarketData.LastPrice, call.MarketData.PreClosePrice);
            setPriceCell(this.Cells[5], call.MarketData.AskPrice1, call.PreMarketData.LastPrice, call.MarketData.PreClosePrice);
            this.Cells[9].Value = call.ImpliedVolatility;
            this.Cells[11].Value = put.ImpliedVolatility;
            setPriceCell(this.Cells[15], put.MarketData.BidPrice1, put.PreMarketData.LastPrice, put.MarketData.PreClosePrice);
            setPriceCell(this.Cells[17], put.MarketData.AskPrice1, put.PreMarketData.LastPrice, put.MarketData.PreClosePrice);
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
            if (call.MarketData != null && underlying.MarketData != null)
            {
                //计算期权理论价格
                OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(this.call.OptionType,
                    this.underlying.MarketData.LastPrice, this.call.StrikePrice, GlobalValues.InterestRate, GlobalValues.Volatility,
                    StaticFunction.GetDaysToMaturity(this.call.Contract.InstrumentID));
                this.call.OptionValue = OptionPricingModel.EuropeanBS(optionPricingModelParams);
                //计算隐含波动率
                this.call.ImpliedVolatility = StaticFunction.CalculateImpliedVolatility(this.underlying.MarketData.LastPrice, 
                    this.call.StrikePrice, StaticFunction.GetDaysToMaturity(this.call.Contract.InstrumentID), 
                    GlobalValues.InterestRate, this.call.MarketData.LastPrice, this.call.OptionType);

            }
        }

        /// <summary>
        /// Put行情更新时该方法被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void put_MarketUpdated(object sender, EventArgs e)
        {
            //计算期权相关值
            if (put.MarketData != null && underlying.MarketData != null)
            {
                //计算期权理论价格
                OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(this.put.OptionType,
                    this.underlying.MarketData.LastPrice, this.put.StrikePrice, GlobalValues.InterestRate, GlobalValues.Volatility,
                    StaticFunction.GetDaysToMaturity(this.put.Contract.InstrumentID));
                this.put.OptionValue = OptionPricingModel.EuropeanBS(optionPricingModelParams);
                //计算隐含波动率
                this.put.ImpliedVolatility = StaticFunction.CalculateImpliedVolatility(this.underlying.MarketData.LastPrice, 
                    this.put.StrikePrice, StaticFunction.GetDaysToMaturity(this.put.Contract.InstrumentID), 
                    GlobalValues.InterestRate, this.put.MarketData.LastPrice, this.put.OptionType);

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
            {
                cell.Style.SelectionForeColor = cell.Style.ForeColor = Color.Red;
            }
            else if (price == preSettle)
            {
                cell.Style.SelectionForeColor = cell.Style.ForeColor = Color.DarkOrange;
            }
            else
            {
                cell.Style.SelectionForeColor = cell.Style.ForeColor = Color.Green;
            }
            if (price > prePrice)
            {
                cell.Style.SelectionBackColor = cell.Style.BackColor = Color.LightPink;
            }
            else if (price == prePrice)
            {
                cell.Style.SelectionBackColor = cell.Style.BackColor = Color.Empty;
            }
            else
            {
                cell.Style.SelectionBackColor = cell.Style.BackColor = Color.LightGreen;
            }
            cell.Value = price > 99999 ? double.NaN : price;
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
