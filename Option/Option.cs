using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class Option : Position
    {
        //期权合约代码
        private string instrumentID;

        public string InstrumentID
        {
            get { return this.instrumentID; }
            set { this.instrumentID = value; }
        }

        /// <summary>
        /// 期权类型
        /// </summary>
        private OptionTypeEnum optionType;

        /// <summary>
        /// 获取期权类型
        /// </summary>
        public OptionTypeEnum OptionType
        {
            get { return this.optionType; }
            set { this.optionType = value; }
        }

        /// <summary>
        /// 标的物的价格
        /// </summary>
        private double underlyingPrice;

        /// <summary>
        /// 获取标的物的价格
        /// </summary>
        public double UnderlyingPrice
        {
            get { return this.underlyingPrice; }
            set { this.underlyingPrice = value; }
        }

        /// <summary>
        /// 执行价
        /// </summary>
        private double strikePrice;

        /// <summary>
        /// 获取执行价
        /// </summary>
        public double StrikePrice
        {
            get { return this.strikePrice; }
            set { this.strikePrice = value; }
        }

        /// <summary>
        /// 无风险利率
        /// </summary>
        private double interestRate;

        /// <summary>
        /// 获取无风险利率
        /// </summary>
        public double InterestRate
        {
            get { return this.interestRate; }
        }

        /// <summary>
        /// 波动率
        /// </summary>
        private double volatility;

        /// <summary>
        /// 获取波动率
        /// </summary>
        public double Volatility
        {
            get { return this.volatility; }
        }
        
        //隐含波动率
        private double impliedVolatility;

        /// <summary>
        /// 获取或者设置隐含波动率
        /// </summary>
        public double ImpliedVolatility
        {
            get { return this.impliedVolatility; }
            set { this.impliedVolatility = value; }
        }

        /// <summary>
        /// 合约最后行情
        /// </summary>
        private ThostFtdcDepthMarketDataField lastMarket;

        /// <summary>
        /// 获取或者设置合约最后行情
        /// </summary>
        public ThostFtdcDepthMarketDataField LastMarket
        {
            get { return this.lastMarket; }
            set { this.lastMarket = value; }
        }

        /// <summary>
        /// 期权希腊字母值
        /// </summary>
        private OptionValue optionValue;

        /// <summary>
        /// 获取或者设置期权希腊字母值
        /// </summary>
        public OptionValue OptionValue
        {
            get { return this.optionValue; }
            set { this.optionValue = value; }
        }

        /// <summary>
        /// 期权报价信息
        /// </summary>
        private MMQuotation mmQuotation;

        /// <summary>
        /// 获取或者设置期权报价信息
        /// </summary>
        public MMQuotation MMQuotation
        {
            get { return this.mmQuotation; }
            set { this.mmQuotation = value; }
        }

        public Option()
        {
            this.mmQuotation = new MMQuotation();
            this.optionValue = new OptionValue();
        }
        
    }
}
