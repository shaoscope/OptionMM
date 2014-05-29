using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class StaticFunction
    {

        /// <summary>
        /// 计算历史波动率
        /// </summary>
        /// <returns></returns>
        public static double CalculateHistoricalVolatility(List<double> barsList)
        {
            List<double> logReturnList = new List<double>();
            List<double> sigmaSquareList = new List<double>();
            sigmaSquareList.Add(0);
            double lambda = 0.94;
            double oneMinusLambda = 1 - lambda;
            if (barsList.Count < 2)
            {
                return 0;
            }
            for (int i = 0; i < barsList.Count - 1; i++)
            {
                if (barsList[i] == 0)
                {
                    logReturnList.Add(0);
                }
                else
                {
                    logReturnList.Add(Math.Log(barsList[i + 1] / barsList[i]));
                }
                sigmaSquareList.Add(lambda * sigmaSquareList[sigmaSquareList.Count - 1] + oneMinusLambda * Math.Pow(logReturnList[logReturnList.Count - 1], 2));
            }
            if (GlobalValues.TimeMeasurementType == TimeMeasurementTypeEnum.交易日)
            {
                return Math.Sqrt(sigmaSquareList[sigmaSquareList.Count - 1]) * Math.Sqrt(GlobalValues.TradingDaysPerYear);
            }
            else if (GlobalValues.TimeMeasurementType == TimeMeasurementTypeEnum.日历日)
            {
                return Math.Sqrt(sigmaSquareList[sigmaSquareList.Count - 1]) * Math.Sqrt(GlobalValues.GeneralDaysPerYear);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 计算隐含波动率
        /// </summary>
        /// <param name="underlyingPrice"></param>
        /// <param name="strikePrice"></param>
        /// <param name="daysToMaturity"></param>
        /// <param name="interestRate"></param>
        /// <param name="marketPrice"></param>
        /// <param name="optionType"></param>
        /// <returns></returns>
        public static double CalculateImpliedVolatility(double underlyingPrice, double strikePrice, int daysToMaturity, double interestRate, double marketPrice, OptionTypeEnum optionType)
        {
            double lower = 0;
            double upper = 100;
            double impliedPrice = 0;
            double impliedVolatility = 0;
            while (Math.Abs(marketPrice - impliedPrice) > 0.05)
            {
                impliedVolatility = (lower + upper) / 2;
                OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(optionType,
                    underlyingPrice, strikePrice, interestRate, impliedVolatility, daysToMaturity);
                OptionValue optionValue = OptionPricingModel.EuropeanBS(optionPricingModelParams);
                impliedPrice = optionValue.Price;
                if(impliedPrice >= marketPrice)
                {
                    upper = impliedVolatility;
                }
                else
                {
                    lower = impliedVolatility;
                }
            }
            return impliedVolatility;
        }

        /// <summary>
        /// 获取期权距到期交易天数
        /// </summary>
        /// <returns></returns>
        public static int GetDaysToMaturity(string instrumentID)
        {

            int daysToMaturity = 0;
            if (instrumentID.Contains("1406"))
            {
                daysToMaturity = GlobalValues.DaysToMaturity[0];
            }
            else if (instrumentID.Contains("1407"))
            {
                daysToMaturity = GlobalValues.DaysToMaturity[1];
            }
            else if (instrumentID.Contains("1408"))
            {
                daysToMaturity = GlobalValues.DaysToMaturity[2];
            }
            return daysToMaturity;
        }
    }
}
