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
        string FRONT_ADDR = "tcp://asp-sim2-md1.financial-trading-platform.com:26213";  // 前置地址
        string BrokerID = "2030";   // 经纪公司代码
        string UserID = "888888";   // 投资者代码
        string Password = "888888"; // 用户密码
        // 大连,上海代码为小写
        // 郑州,中金所代码为大写
        // 郑州品种年份为一位数
        //string[] ppInstrumentID = { "ag1301", "cu1301", "ru1201", "TA301", "SR301", "y1305", "IF1212" };	// 行情订阅列表
        int iRequestID = 0;

        public MDAPI(string mdaddr, string brokerID, string InvesterID, string password)
        {
            FRONT_ADDR = mdaddr;
            BrokerID = brokerID;
            UserID = InvesterID;
            Password = password;
            MD = new CTPMDAdapter();
            //AddEvent();
        }

        private void AddEvent()
        {
            MD.OnFrontConnected += new FrontConnected(OnFrontConnected);
            MD.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);
            MD.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            MD.OnRspError += new RspError(OnRspError);
            MD.OnRspSubMarketData += new RspSubMarketData(OnRspSubMarketData);
            MD.OnRspUnSubMarketData += new RspUnSubMarketData(OnRspUnSubMarketData);
            MD.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            MD.OnRspUserLogout += new RspUserLogout(OnRspUserLogout);
            MD.OnRtnDepthMarketData += new RtnDepthMarketData(OnRtnDepthMarketData);
            MD.OnRtnForQuoteRsp += new RtnForQuoteRsp(OnRtnForQuoteRsp);
        }

        public void Connect()
        {
            try
            {
                MD.RegisterFront(FRONT_ADDR);
                MD.Init();
                //api.Join(); // 阻塞直到关闭或者CTRL+C
                //this.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //api.Release();
            }
        }

        public void Release()
        {
            if (MD != null)
            {
                MD.Release();
                MD = null;
            }
        }

        public int ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = BrokerID;
            req.UserID = UserID;
            req.Password = Password;
            int iResult = MD.ReqUserLogin(req, ++iRequestID);
            return iResult;
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

        void OnFrontConnected()
        {
            ReqUserLogin();
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
