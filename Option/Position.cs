using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    class Position
    {
        public int LongInputLots = 0;
        public int LongPositionVolume = 0;
        public int ShortInputLots = 0;
        public int ShortPositionVolume = 0;
        public ThostFtdcOrderField ShortOptionOrder = null;
        public ThostFtdcOrderField LongOptionOrder = null;
        public ThostFtdcOrderField CloseLongOptionOrder = null;
        public ThostFtdcOrderField CloseShortOptionOrder = null;
        public string PlaceShortOptionOrderRef = null;
        public string PlaceLongOptionOrderRef = null;
        public string CloseLongOptionOrderRef = null;
        public string CloseShortOptionOrderRef = null;
        private object LongInputLotsLock = new object();
        private object LongPositionLock = new object();
        private object ShortInputLotsLock = new object();
        private object ShortPositionLock = new object();

        public Position()
        {
            //MDManager.MD.OnDepthMarketData += new DepthMarketDataHandle(OnDepthMarketData);
            //MDManager.MD.OnTick += new TickHandle()
            //TDManager.TD.OnCanceled += new CanceledHandle(OnOrderCanceled);
            //TDManager.TD.OnTraded += new TradedHandle(OnOrderTraded);
            //TDManager.TD.OnTrading += new TradingHandle(OnOrderTrading);
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
                LongPositionVolume = LongPositionVolume + Lots;
            }
        }

        public void AddShortPosition(int Lots)
        {
            lock (ShortPositionLock)
            {
                ShortPositionVolume = ShortPositionVolume + Lots;
            }
        }

    }
}
