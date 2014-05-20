using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class OptionPricingModel
    {
        public static OptionValue EuropeanBS(OptionProperties optionProperties)
        {
            double sigmaTimesSqrtMaturity = optionProperties.Volatility * Math.Sqrt(optionProperties.Maturity);
            double d1 = (Math.Log(optionProperties.UnderlyingPrice / optionProperties.StrikePrice) + (optionProperties.Volatility * optionProperties.Volatility / 2) *
                optionProperties.Maturity) / sigmaTimesSqrtMaturity;
            if (double.IsNaN(d1))
            {
                d1 = Double.PositiveInfinity;
            }
            double d2 = d1 - sigmaTimesSqrtMaturity;
            double optionPrice = 0;
            double delta = 0;
            double gamma = 0;
            double vega = 0;
            double theta = 0;
            double rho = 0;
            switch (optionProperties.OptionType)
            {
                case OptionTypeEnum.call:
                    optionPrice = optionProperties.UnderlyingPrice * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * normdist(d1) -
                        normdist(d2) * optionProperties.StrikePrice * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity);
                    delta = Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * normdist(d1);
                    gamma = ((Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity)) / (optionProperties.UnderlyingPrice * optionProperties.Volatility * Math.Sqrt(optionProperties.Maturity));
                    vega = (Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * optionProperties.UnderlyingPrice * Math.Sqrt(optionProperties.Maturity);
                    theta = optionProperties.InterestRate * optionPrice - Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * optionProperties.UnderlyingPrice * (Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * optionProperties.Volatility / (2 * Math.Sqrt(optionProperties.Maturity));
                    rho = -optionProperties.Maturity * optionPrice;
                    break;
                case OptionTypeEnum.put:
                    optionPrice = -optionProperties.UnderlyingPrice * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * normdist(-d1) +
                        normdist(-d2) * optionProperties.StrikePrice * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity);
                    delta = Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * (normdist(d1) - 1);
                    gamma = ((Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity)) / (optionProperties.UnderlyingPrice * optionProperties.Volatility * Math.Sqrt(optionProperties.Maturity));
                    vega = (Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * optionProperties.UnderlyingPrice * Math.Sqrt(optionProperties.Maturity);
                    theta = optionProperties.InterestRate * optionPrice -
                Math.Exp(-optionProperties.InterestRate * optionProperties.Maturity) * optionProperties.UnderlyingPrice * (Math.Exp(-d1 * d1 / 2) / Math.Sqrt(2 * Math.PI)) * optionProperties.Volatility / (2 * Math.Sqrt(optionProperties.Maturity));
                    rho = optionProperties.Maturity * optionPrice;
                    break;
                default:
                    break;
            }
            switch (GlobalValues.TimeMeasurementType)
            {
                case TimeMeasurementTypeEnum.交易日:
                    theta /= GlobalValues.TradingDaysPerYear;
                    break;
                case TimeMeasurementTypeEnum.日历日:
                    theta /= GlobalValues.GeneralDaysPerYear;
                    break;
                default:
                    break;
            }
            return new OptionValue(optionPrice, delta, gamma, vega / 100, theta, rho / 100, optionProperties.Maturity);
        }

        /// <summary>
        /// 标准正态分布
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double normdist(double x)
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
