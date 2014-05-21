using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class Option : CTPEvents
    {
        //期权合约代码
        private string instrumentID;

        public string InstrumentID
        {
            get { return this.instrumentID; }
            set { this.instrumentID = value; }
        }

        //期权的价格
        private double theoreticalPrice;

        /// <summary>
        /// 获取或者设置期权的价格
        /// </summary>
        public double TheoreticalPrice
        {
            get { return this.theoreticalPrice; }
            set { this.theoreticalPrice = value; }
        }

        //Delta
        private double delta;

        /// <summary>
        /// 获取或者设置Delta
        /// </summary>
        public double Delta
        {
            get { return this.delta; }
            set { this.delta = value; }
        }

        //Gamma
        private double gamma;

        /// <summary>
        /// 获取或者设置Gamma
        /// </summary>
        public double Gamma
        {
            get { return this.gamma; }
            set { this.gamma = value; }
        }

        //Vega
        private double vega;

        /// <summary>
        /// 获取或者设置期权的Vega
        /// </summary>
        public double Vega
        {
            get { return this.vega; }
            set { this.vega = value; }
        }

        //Theta
        private double theta;

        /// <summary>
        /// 获取或者设置期权的Theta
        /// </summary>
        public double Theta
        {
            get { return this.theta; }
            set { this.theta = value; }
        }

        //Rho
        private double rho;

        /// <summary>
        /// 获取或者设置期权的Rho
        /// </summary>
        public double Rho
        {
            get { return this.rho; }
            set { this.rho = value; }
        }

        //距离期权到期日
        private double maturity;

        /// <summary>
        /// 获取或者设置距离期权的到期日
        /// </summary>
        public double Maturity
        {
            get { return this.maturity; }
            set { this.maturity = value; }
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

        /// <summary>
        /// 隐含波动率
        /// </summary>
        private double impridVolatility;

        /// <summary>
        /// 获取或者设置隐含波动率
        /// </summary>
        public double ImpridVolatility
        {
            get { return this.impridVolatility; }
            set { this.impridVolatility = value; }
        }

        /// <summary>
        /// 期权合约最后行情
        /// </summary>
        private ThostFtdcDepthMarketDataField lastMarket;


        public ThostFtdcDepthMarketDataField LastMarket
        {
            get { return this.lastMarket; }
        }

    }
}
