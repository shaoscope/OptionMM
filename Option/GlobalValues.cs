using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class GlobalValues
    {
        //当前日期衡量类型
        public static TimeMeasurementTypeEnum TimeMeasurementType = TimeMeasurementTypeEnum.交易日;

        //一年的日历天数
        public static readonly int GeneralDaysPerYear = 365;

        //一年的交易天数
        public static readonly int TradingDaysPerYear = 252;

        //无风险利率
        public static readonly double InterestRate = 0.04;
        
        //股指波动率
        public static readonly double Volatility = 0.30;

        //到期天数
        public static readonly int[] DaysToMaturity = { 21, 41, 61, 81, 141 };

    }

    public enum OptionTypeEnum : int
    {
        call = 1,
        put = 2
    }

    public enum TimeMeasurementTypeEnum : int
    {
        交易日 = 1,
        日历日 = 2

    }
}
