using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    public delegate void TickHandle(ThostFtdcDepthMarketDataField md);
    public delegate void OrderHandle(ThostFtdcOrderField order);

    class Contract
    {
        public Option option = null;
        public double LongInputLots = 0;
        public double LongPosiont = 0;
        public double ShortInputLots = 0;
        public double ShortPosiont = 0;
        public Instrument instrument = null;

        public Contract(Option op)
        {
            option = op;
            MDManager.MD.OnDepthMarketData += new DepthMarketDataHandle(OnDepthMarketData);
            TDManager.TD.OnCanceled += new CanceledHandle(OnOrderCanceled);
            TDManager.TD.OnTraded += new TradedHandle(OnOrderTraded);
            TDManager.TD.OnTrading += new TradingHandle(OnOrderTrading);
        }

        public Contract(Instrument instrment)
        {
            instrument = instrment;
            MDManager.MD.OnDepthMarketData += new DepthMarketDataHandle(OnDepthMarketData);
            TDManager.TD.OnCanceled += new CanceledHandle(OnOrderCanceled);
            TDManager.TD.OnTraded += new TradedHandle(OnOrderTraded);
            TDManager.TD.OnTrading += new TradingHandle(OnOrderTrading);
        }

        public void SubMD(string[] inst)
        {
            MDManager.MD.SubscribeMarketData(inst);
        }

        void OnDepthMarketData(ThostFtdcDepthMarketDataField md)
        {
            if ((option != null && md.InstrumentID == option.instrumentID) || (instrument != null && md.InstrumentID == instrument.InstrumentID))
            {
                PushTick(md);
            }
        }
        void OnOrderCanceled(ThostFtdcOrderField order)
        {
            if ((option != null && order.InstrumentID == option.instrumentID) || (instrument != null && order.InstrumentID == instrument.InstrumentID))
            {
                PushTrading(order);
            }
        }
        void OnOrderTraded(ThostFtdcOrderField order)
        {
            if ((option != null && order.InstrumentID == option.instrumentID) || (instrument != null && order.InstrumentID == instrument.InstrumentID))
            {
                PushTrading(order);
            }
        }
        void OnOrderTrading(ThostFtdcOrderField order)
        {
            if ((option != null && order.InstrumentID == option.instrumentID) || (instrument != null && order.InstrumentID == instrument.InstrumentID))
            {
                PushTrading(order);
            }
        }

        public event OrderHandle OnTrading;
        public void PushTrading(ThostFtdcOrderField order)
        {
            if (OnTrading != null)
                OnTrading(order);
        }
        public event OrderHandle OnTraded;
        public void PushTraded(ThostFtdcOrderField order)
        {
            if (OnTraded != null)
                OnTraded(order);
        }
        public event OrderHandle OnCanceled;
        public void PushCanceled(ThostFtdcOrderField order)
        {
            if (OnCanceled != null)
                OnCanceled(order);
        }

        public event TickHandle OnTick;
        public void PushTick(ThostFtdcDepthMarketDataField md)
        {
            if (OnTick != null)
                OnTick(md);
        }

        public string Buy(string instrumentID, int lots, double price)
        {
            return TDManager.TD.Buy(instrumentID, lots, price);
        }

        public string Sell(string instrumentID, int lots, double price)
        {
            return TDManager.TD.Sell(instrumentID, lots, price);
        }

        public string BuyToCover(string instrumentID, int lots, double price)
        {
            return TDManager.TD.BuyToCover(instrumentID, lots, price);
        }

        public string SellShort(string instrumentID, int lots, double price)
        {
            return TDManager.TD.SellShort(instrumentID, lots, price);
        }

        public void CancelOrder(string OrderRef)
        {
            TDManager.TD.CancelOrder(OrderRef);
        }
    }
}
