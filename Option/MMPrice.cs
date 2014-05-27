using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class MMPrice
    {
        public static double GetAskPriceThisMonth(double BidPrice)
        {
            double dRet = 0;
            if (BidPrice < 10)
            {
                dRet = BidPrice + 0.5;
            }
            else if(BidPrice < 20)
            {
                dRet = BidPrice + 1;
            }
            else if (BidPrice < 50)
            {
                dRet = BidPrice + 2.5;
            }
            else if (BidPrice < 100)
            {
                dRet = BidPrice + 5;
            }
            else if (BidPrice < 250)
            {
                dRet = BidPrice + 8;
            }
            else
            {
                dRet = BidPrice + 15;
            }
            return dRet;
        }

        public static double GetBidPriceThisMonth(double AskPrice)
        {
            double dRet = 0;
            if (AskPrice < 0.6)
            {
                dRet = 0.1;
                AskPrice = 0.6;
            }
            else if (AskPrice < 10.5)
            {
                dRet = AskPrice - 0.5;
            }
            else if (AskPrice < 21)
            {
                dRet = AskPrice - 1;
                if (dRet < 10)
                    dRet = 10;
            }
            else if (AskPrice < 52.5)
            {
                dRet = AskPrice - 2.5;
                if (dRet < 20)
                    dRet = 20;
            }
            else if (AskPrice < 105)
            {
                dRet = AskPrice - 5;
                if (dRet < 50)
                    dRet = 50;
            }
            else if (AskPrice < 258)
            {
                dRet = AskPrice - 8;
                if (dRet < 100)
                    dRet = 100;
            }
            else
            {
                dRet = AskPrice - 15;
                if (dRet < 250)
                    dRet = 250;
            }
            return dRet;
        }

        public static double GetAskPriceNextMonth(double BidPrice)
        {
            double dRet = 0;
            if (BidPrice < 10)
            {
                dRet = BidPrice + 1;
            }
            else if (BidPrice < 20)
            {
                dRet = BidPrice + 2;
            }
            else if (BidPrice < 50)
            {
                dRet = BidPrice + 4;
            }
            else if (BidPrice < 100)
            {
                dRet = BidPrice + 8;
            }
            else if (BidPrice < 250)
            {
                dRet = BidPrice + 15;
            }
            else
            {
                dRet = BidPrice + 25;
            }
            return dRet;
        }

        public static double GetBidPriceNextMonth(double AskPrice)
        {
            double dRet = 0;
            if (AskPrice < 1.1)
            {
                dRet = 0.1;
                AskPrice = 1.1;
            }
            else if (AskPrice < 11)
            {
                dRet = AskPrice - 1;
            }
            else if (AskPrice < 22)
            {
                dRet = AskPrice - 2;
                if (dRet < 10)
                    dRet = 10;
            }
            else if (AskPrice < 54)
            {
                dRet = AskPrice - 4;
                if (dRet < 20)
                    dRet = 20;
            }
            else if (AskPrice < 108)
            {
                dRet = AskPrice - 8;
                if (dRet < 50)
                    dRet = 50;
            }
            else if (AskPrice < 265)
            {
                dRet = AskPrice - 15;
                if (dRet < 100)
                    dRet = 100;
            }
            else
            {
                dRet = AskPrice - 25;
                if (dRet < 250)
                    dRet = 250;
            }
            return dRet;
        }
    }
}
