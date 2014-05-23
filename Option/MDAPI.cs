﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CTP
{
   // public delegate void DepthMarketDataHandle(ThostFtdcDepthMarketDataField md);
    public delegate void TickHandle(ThostFtdcDepthMarketDataField md);
    public class MDAPI
    {   
        public bool bLogin = false;
        public CTPMDAdapter api = null;
        string FRONT_ADDR = "tcp://asp-sim2-md1.financial-trading-platform.com:26213";  // 前置地址
        string BrokerID = "2030";   // 经纪公司代码
        string UserID = "888888";   // 投资者代码
        string Password = "888888"; // 用户密码
        // 大连,上海代码为小写
        // 郑州,中金所代码为大写
        // 郑州品种年份为一位数
        //string[] ppInstrumentID = { "ag1301", "cu1301", "ru1201", "TA301", "SR301", "y1305", "IF1212" };	// 行情订阅列表
        int iRequestID = 0;
        //Strategy stg = null;

        public MDAPI(string mdaddr, string brokerID, string InvesterID, string password)
        {
            FRONT_ADDR = mdaddr;
            BrokerID = brokerID;
            UserID = InvesterID;
            Password = password;
            //ppInstrumentID = InstrumentID;
            //stg = new Strategy(mdaddr, tdaddr, brokerID, InvesterID, password, ppInstrumentID);
            api = new CTPMDAdapter();
            AddEvent();
        }

        private void AddEvent()
        {
            api.OnFrontConnected += new FrontConnected(OnFrontConnected);
            api.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);
            api.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            api.OnRspError += new RspError(OnRspError);
            api.OnRspSubMarketData += new RspSubMarketData(OnRspSubMarketData);
            api.OnRspUnSubMarketData += new RspUnSubMarketData(OnRspUnSubMarketData);
            api.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            api.OnRspUserLogout += new RspUserLogout(OnRspUserLogout);
            api.OnRtnDepthMarketData += new RtnDepthMarketData(OnRtnDepthMarketData);
        }

        public void Connect()
        {
            try
            {
                api.RegisterFront(FRONT_ADDR);
                api.Init();
                //api.Join(); // 阻塞直到关闭或者CTRL+C
                //this.Release();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //GUIRefresh.UpdateMessageBox(e.Message);
            }
            finally
            {
                //api.Release();
            }
        }

        public void Release()
        {
            if (api != null)
            {
                api.Release();
                api = null;
            }
        }

        private int ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = BrokerID;
            req.UserID = UserID;
            req.Password = Password;
            int iResult = api.ReqUserLogin(req, ++iRequestID);

            //Console.WriteLine("--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 发送行情用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
            return iResult;
        }

        public int SubscribeMarketData(string[] ppInstrumentID)
        {
            int iRet;
            iRet = api.SubscribeMarketData(ppInstrumentID);
            return iRet;
        }

        public int UnSubscribeMarketData(string[] ppInstrumentID)
        {
            int iRet;
            iRet = api.UnSubscribeMarketData(ppInstrumentID);
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
                //Console.WriteLine("行情登陆成功");
                //GUIRefresh.UpdateListBox2("行情登陆成功");
                ///获取当前交易日
                //Console.WriteLine("--->>> 获取当前交易日 = " + api.GetTradingDay());
                //GUIRefresh.UpdateListBox2("--->>> 获取行情当前交易日 = " + api.GetTradingDay());
                // 请求订阅行情
                //SubscribeMarketData();
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
            //Console.WriteLine("--->>> nTimerLapse = " + nTimeLapse);
            //GUIRefresh.UpdateListBox2("--->>> nTimerLapse = " + nTimeLapse);
        }

        void OnFrontDisconnected(int nReason)
        {
            //Console.WriteLine("--->>> Reason = {0}", nReason);
            //GUIRefresh.UpdateListBox2(string.Format("--->>> Reason = {0}", nReason));
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
                //Console.WriteLine("--->>> ErrorID={0}, ErrorMsg={1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg);
                //GUIRefresh.UpdateListBox2(string.Format("--->>> ErrorID={0}, ErrorMsg={1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg));
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
                //DepthMarketData(pDepthMarketData);
                PushTick(pDepthMarketData);
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

        /*public event DepthMarketDataHandle OnDepthMarketData;
        public void DepthMarketData(ThostFtdcDepthMarketDataField md)
        {
            if (OnDepthMarketData != null)
                OnDepthMarketData(md);
        }*/

    }
}
