using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    /// <summary>
    /// 计算模型输入参数类
    /// </summary>
    class OptionProperties
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OptionProperties()
        {

        }

        public OptionProperties(OptionTypeEnum optionType, double underlyingPrice, double strikePrice, double interestRate, double volatility, double daysToMaturity)
        {
            this.optionType = optionType;
            this.underlyingPrice = underlyingPrice;
            this.strikePrice = strikePrice;
            this.interestRate = interestRate;
            this.volatility = volatility;
            if (GlobalValues.TimeMeasurementType == TimeMeasurementTypeEnum.交易日)
            {
                this.maturity = daysToMaturity / GlobalValues.TradingDaysPerYear;
            }
            else if (GlobalValues.TimeMeasurementType == TimeMeasurementTypeEnum.日历日)
            {
                this.maturity = daysToMaturity / GlobalValues.GeneralDaysPerYear;
            }
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
        /// 距到期时间
        /// </summary>
        private double maturity;

        /// <summary>
        /// 获取或者设置距到期时间
        /// </summary>
        public double Maturity
        {
            get { return this.maturity; }
            set { this.maturity = value; }
        }
    }//class
}
