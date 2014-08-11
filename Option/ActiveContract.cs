using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    class ActiveContract
    {
        /// <summary>
        /// 行情更新时该事件被触发
        /// </summary>
        public event EventHandler MarketUpdated;

        /// <summary>
        /// 询价单到达时被触发
        /// </summary>
        public event EventHandler ForQuoteArrived;

        /// <summary>
        /// 报价单被成交时触发
        /// </summary>
        //public event RtnOrderEventHandler QuoteTraded;
        public event EventHandler<RtnTradeEventArgs> QuoteTraded;

        /// <summary>
        /// 套利单成交时触发
        /// </summary>
        //public event RtnOrderEventHandler ArbitrageTraded;
        public event EventHandler<RtnTradeEventArgs> ArbitrageTraded;

        /// <summary>
        /// 合约信息
        /// </summary>
        public ThostFtdcInstrumentField Contract { get; private set; }

        /// <summary>
        /// 合约多头持仓信息
        /// </summary>
        public ThostFtdcInvestorPositionField LongPosition { get; set; }

        /// <summary>
        /// 合约空头持仓信息
        /// </summary>
        public ThostFtdcInvestorPositionField ShortPosition { get; set; }

        /// <summary>
        /// 上一刻行情
        /// </summary>
        public ThostFtdcDepthMarketDataField PreMarketData { get; private set; }

        /// <summary>
        /// 期权基本值
        /// </summary>
        public OptionValue OptionValue { get; set; }

        /// <summary>
        /// 是否自选锁定的标志
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// 最新的行情数据
        /// </summary>
        public ThostFtdcDepthMarketDataField MarketData { get; private set; }

        public ActiveContract(ThostFtdcInstrumentField contract)
        {
            this.Contract = contract;
            LongPosition = new ThostFtdcInvestorPositionField();
            ShortPosition = new ThostFtdcInvestorPositionField();
            OptionValue = new OptionValue();
        }

        /// <summary>
        /// 更新行情信息
        /// </summary>
        public void UpdateMarket(ThostFtdcDepthMarketDataField MarketData)
        {
            this.PreMarketData = this.MarketData;
            this.MarketData = MarketData;
            if (this.MarketUpdated != null)
            {
                this.MarketUpdated(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 新询价单
        /// </summary>
        public void NewForQuote(ThostFtdcForQuoteRspField forQuoteField)
        {
            if(this.ForQuoteArrived != null)
            {
                this.ForQuoteArrived(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 报价被成交
        /// </summary>
        /// <param name="order"></param>
        public void OnQuoteTraded(ThostFtdcTradeField trade)
        {
            if (this.QuoteTraded != null)
            {
                this.QuoteTraded(this, new RtnTradeEventArgs(trade));
            }
        }

        /// <summary>
        /// 套利被成交
        /// </summary>
        /// <param name="order"></param>
        public void OnArbitrageTraded(ThostFtdcTradeField order)
        {
            if (this.ArbitrageTraded != null)
            {
                this.ArbitrageTraded(this, new RtnTradeEventArgs(order));
            }
        }

        /// <summary>
        /// 期权类型
        /// </summary>
        public OptionTypeEnum OptionType 
        { 
            get
            {
                return this.Contract.InstrumentID.Contains('C') ? OptionTypeEnum.call : OptionTypeEnum.put;
            }
        }

        /// <summary>
        /// 执行价格
        /// </summary>
        public double StrikePrice
        {
            get
            {
                return double.Parse(this.Contract.InstrumentID.Split('-')[2]);
            }
        }

        /// <summary>
        /// 隐含波动率
        /// </summary>
        public double ImpliedVolatility { get; set; }





    }//class
}
