using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    public delegate void OrderHandle(ThostFtdcOrderField pOrder);
    public delegate void CancelActionHandle(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo);
    public delegate void PositionHandle(ThostFtdcInvestorPositionField position);
    public delegate void OrderRefReplaceHandle(string orderrefold, string orderrefnew);

    public class Order
    {
        public ThostFtdcInputOrderField InputOrder;
        public ThostFtdcInputQuoteField InputQuote;
        public DateTime InputTime;
        public ThostFtdcInputOrderActionField CancelOrder;
        public List<DateTime> CancelTime = new List<DateTime>();
        //最新标志
        public string OrderRef;
        //重发前标志
        public List<string> OrigialOrderRef = new List<string>();   //OrigialSignal
        public ThostFtdcOrderField OrderField;

        public Order(ThostFtdcInputOrderField pInput, DateTime pTime)
        {
            InputOrder = pInput;
            InputTime = pTime;
        }

        public Order(ThostFtdcInputQuoteField pInput, DateTime pTime)
        {
            InputQuote = pInput;
            InputTime = pTime;
        }

        public void Cancel(ThostFtdcInputOrderActionField pInputAction, DateTime pTime)
        {
            CancelOrder = pInputAction;
            CancelTime.Add(pTime);
        }
    }
}
