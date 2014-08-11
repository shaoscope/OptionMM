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
        public static double CalculateImpliedVolatilityBisection(double underlyingPrice, double strikePrice, int daysToMaturity, double interestRate, double marketPrice, OptionTypeEnum optionType)
        {
            double lower = 0;
            double upper = 10;
            double impliedVolatility = 0;
            double impliedPrice = 0;
            while (Math.Abs(upper - lower) > 0.000000001)
            {
                impliedVolatility = (lower + upper) / 2;
                OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(optionType,
                    underlyingPrice, strikePrice, interestRate, impliedVolatility, daysToMaturity);
                OptionValue optionValue = OptionPricingModel.EuropeanBS(optionPricingModelParams);
                if(impliedPrice == optionValue.Price)
                {
                    break;
                }
                impliedPrice = optionValue.Price;
                if (optionValue.Price >= marketPrice)
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
        /// 计算隐含波动率（牛顿方法）
        /// </summary>
        /// <param name="underlyingPrice"></param>
        /// <param name="strikePrice"></param>
        /// <param name="daysToMaturity"></param>
        /// <param name="interestRate"></param>
        /// <param name="marketPrice"></param>
        /// <param name="optionType"></param>
        /// <returns></returns>
        public static double CalculateImpliedVolatilityNewton(double underlyingPrice, double strikePrice,
            int daysToMaturity, double interestRate, double marketPrice, OptionTypeEnum optionType)
        {
            double maturity = 0;
            if (GlobalValues.TimeMeasurementType == TimeMeasurementTypeEnum.交易日)
            {
                maturity = (double)daysToMaturity / GlobalValues.TradingDaysPerYear;
            }
            else if (GlobalValues.TimeMeasurementType == TimeMeasurementTypeEnum.日历日)
            {
                maturity = (double)daysToMaturity / GlobalValues.GeneralDaysPerYear;
            }
            double sigmahat = Math.Sqrt(2 * Math.Abs((Math.Log(underlyingPrice / strikePrice) + interestRate * maturity) / maturity));
            double tol = 0.00000001;
            double sigma = sigmahat;
            double sigmadiff = 1;
            int index = 1;
            int indexMax = 100;
            while (sigmadiff >= tol && index < indexMax)
            {
                double d1 = (Math.Log(underlyingPrice / strikePrice) + (interestRate + 0.5 * sigma * sigma) * (maturity)) / (sigma * Math.Sqrt(maturity));
                double d2 = d1 - sigma * Math.Sqrt(maturity);
                double optionImpliedPrice = 0;
                double optionImpliedVega = 0;
                if (optionType == OptionTypeEnum.call)
                {
                    optionImpliedPrice = underlyingPrice * Math.Exp(-interestRate * maturity) * normdist(d1) -
                    normdist(d2) * strikePrice * Math.Exp(-interestRate * maturity);
                    optionImpliedVega = (Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * Math.Exp(-interestRate * maturity) * underlyingPrice * Math.Sqrt(maturity);
                }
                else if (optionType == OptionTypeEnum.put)
                {
                    optionImpliedPrice = -underlyingPrice * Math.Exp(-interestRate * maturity) * normdist(-d1) +
                        normdist(-d2) * strikePrice * Math.Exp(-interestRate * maturity);
                    optionImpliedVega = (Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * Math.Exp(-interestRate * maturity) * underlyingPrice * Math.Sqrt(maturity);
                }
                double increment = (optionImpliedPrice - marketPrice) / optionImpliedVega;
                sigma = sigma - increment;
                index++;
                sigmadiff = Math.Abs(increment);
            }
            return sigma;
        }

        /// <summary>
        /// 获取期权距到期交易天数
        /// </summary>
        /// <returns></returns>
        public static int GetDaysToMaturity(string instrumentID)
        {

            int daysToMaturity = 0;
            if (instrumentID.Contains("1408"))
            {
                daysToMaturity = GlobalValues.DaysToMaturity[2];
            }
            else if(instrumentID.Contains("1409"))
            {
                daysToMaturity = GlobalValues.DaysToMaturity[3];
            }
            else if(instrumentID.Contains("1412"))
            {
                daysToMaturity = GlobalValues.DaysToMaturity[4];
            }
            return daysToMaturity;
        }

        /// <summary>
        /// 获取价格的格式说明符
        /// </summary>
        /// <param name="PriceTick">最小变动价位</param>
        /// <returns>格式说明符</returns>
        public static string GetPriceFormat(double PriceTick)
        {
            int c = 0;
            while (PriceTick < 1)
            {
                PriceTick *= 10;
                c++;
            }
            return "F" + c.ToString();
        }

        /// <summary>
        /// 标准正态分布
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double normdist(double x)
        {
            double b1 = 0.319381530;
            double b2 = -0.356563782;
            double b3 = 1.781477937;
            double b4 = -1.821255978;
            double b5 = 1.330274429;
            double p = 0.2316419;
            double c = 0.39894228;

            if (x >= 0.0)
            {
                double t = 1.0 / (1.0 + p * x);
                return (1.0 - c * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * b5 + b4) + b3) + b2) + b1));
            }
            else
            {
                double t = 1.0 / (1.0 - p * x);
                return (c * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * b5 + b4) + b3) + b2) + b1));
            }
        }
    }
}
