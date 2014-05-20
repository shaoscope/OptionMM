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
        public int LongInputLots = 0;
        public int LongPosition = 0;
        public int ShortInputLots = 0;
        public int ShortPosition = 0;
        public Instrument instrument = null;
        public ThostFtdcOrderField CurrentAskOptionOrder = null;
        public ThostFtdcOrderField CurrentBidOptionOrder = null;
        public string AskOptionOrderRef = null;
        public string BidOptionOrderRef = null;
        private object LongInputLotsLock = new object();
        private object LongPositionLock = new object();
        private object ShortInputLotsLock = new object();
        private object ShortPositionLock = new object();

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
            AddLongInputLots(lots);
            return TDManager.TD.Buy(instrumentID, lots, price);
        }

        public string Sell(string instrumentID, int lots, double price)
        {
            AddLongInputLots(-1 * lots);
            return TDManager.TD.Sell(instrumentID, lots, price);
        }

        public string BuyToCover(string instrumentID, int lots, double price)
        {
            AddShortInputLots(lots);
            return TDManager.TD.BuyToCover(instrumentID, lots, price);
        }

        public string SellShort(string instrumentID, int lots, double price)
        {
            AddShortInputLots(-1 * lots);
            return TDManager.TD.SellShort(instrumentID, lots, price);
        }

        public void AddShortInputLots(int Lots)
        {
            lock(ShortInputLotsLock)
            {
                ShortInputLots = ShortInputLots + Lots;
            }
        }

        public void AddLongInputLots(int Lots)
        {
            lock (LongInputLotsLock)
            {
                LongInputLots = LongInputLots + Lots;
            }
        }

        public void AddLongPosition(int Lots)
        {
            lock (LongPositionLock)
            {
                LongPosition = LongPosition + Lots;
            }
        }

        public void AddShortPosition(int Lots)
        {
            lock (ShortPositionLock)
            {
                ShortPosition = ShortPosition + Lots;
            }
        }

        public void CancelOrder(string OrderRef)
        {
            TDManager.TD.CancelOrder(OrderRef);
        }
    }
}
