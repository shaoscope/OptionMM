using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CTP
{
   // public delegate void DepthMarketDataHandle(ThostFtdcDepthMarketDataField md);
    public delegate void TickHandle(ThostFtdcDepthMarketDataField md);
    public delegate void ForQuoteHandle(ThostFtdcForQuoteRspField ForQuoteRsp);
    public class MDAPI
    {   
        public bool bLogin = false;
        public CTPMDAdapter MD = null;
        int iRequestID = 0;

        public MDAPI()
        {
            MD = new CTPMDAdapter();
        }

        public int SubscribeMarketData(string[] ppInstrumentID)
        {
            int iRet;
            iRet = MD.SubscribeMarketData(ppInstrumentID);
            SubscribeForQuoteRsp(ppInstrumentID);
            return iRet;
        }

        public int UnSubscribeMarketData(string[] ppInstrumentID)
        {
            int iRet;
            iRet = MD.UnSubscribeMarketData(ppInstrumentID);
            UnSubscribeForQuoteRsp(ppInstrumentID);
            return iRet;
        }

        private int SubscribeForQuoteRsp(string[] ppInstrumentID)
        {
            int iRet;
            iRet = MD.SubscribeForQuoteRsp(ppInstrumentID);
            return iRet;
        }

        private int UnSubscribeForQuoteRsp(string[] ppInstrumentID)
        {
            int iRet;
            iRet = MD.UnSubscribeForQuoteRsp(ppInstrumentID);
            return iRet;
        }

        void OnRspUserLogout(ThostFtdcUserLogoutField pUserLogout, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            bLogin = false;
        }

        void OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                bLogin = true;
            }
        }

        void OnRspUnSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
        }

        void OnRspSubMarketData(ThostFtdcSpecificInstrumentField pSpecificInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
        }

        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
            }
        }

        void OnHeartBeatWarning(int nTimeLapse)
        {

        }

        void OnFrontDisconnected(int nReason)
        {

        }

        private bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
            {

            }
            return bResult;
        }

        public event TickHandle OnTick;
        public void PushTick(ThostFtdcDepthMarketDataField md)
        {
            if (OnTick != null)
            {
                OnTick(md);
            }
        }

        void OnRtnDepthMarketData(ThostFtdcDepthMarketDataField pDepthMarketData)
        {
            if (pDepthMarketData != null)
            {
                PushTick(pDepthMarketData);
            }
        }

        public event ForQuoteHandle OnForQuote;
        public void PushForQuote(ThostFtdcForQuoteRspField pForQuoteRsp)
        {
            if (OnForQuote != null)
            {
                OnForQuote(pForQuoteRsp);
            }
        }
        void OnRtnForQuoteRsp(ThostFtdcForQuoteRspField pForQuoteRsp)
        {
            if (pForQuoteRsp != null)
            {
                PushForQuote(pForQuoteRsp);
            }
        }

        public double GetStrikePrice(string InstrumentID)
        {
            double dRet = 0;
            string[] strTemp = InstrumentID.Split('-');
            try
            {
                dRet = double.Parse(strTemp[strTemp.Length - 1]);
            }
            catch
            {
            }
            return dRet;
        }

    }
}
