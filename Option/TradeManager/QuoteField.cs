using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using CTP;

namespace OptionMM
{
    public class QuoteField
    {
        public ThostFtdcInputQuoteField InputQuote;
        public DateTime InputTime;
        public ThostFtdcInputQuoteActionField CancelQuote;
        public List<DateTime> CancelTime = new List<DateTime>();
        //最新标志
        public string QuoteRef;
        //重发前标志
        public List<string> OrigialQuoteRef = new List<string>();   //OrigialSignal
        public ThostFtdcQuoteField Quote;
        public ThostFtdcOrderField AskOrderField;
        public ThostFtdcOrderField BidOrderField;

        public QuoteField(ThostFtdcInputQuoteField pInput, DateTime pTime)
        {
            InputQuote = pInput;
            InputTime = pTime;
        }

        public void Cancel(ThostFtdcInputQuoteActionField pInputAction, DateTime pTime)
        {
            CancelQuote = pInputAction;
            CancelTime.Add(pTime);
        }
    }
}
