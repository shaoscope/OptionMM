using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace CTP
{
    public delegate void OrderHandle(ThostFtdcOrderField pOrder);
    public delegate void CancelActionHandle(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo);

    public delegate void PositionHandle(ThostFtdcInvestorPositionField position);
    
    public class OrderInstert
    {
        public ThostFtdcInputOrderField _req = null;
        public OrderInstertState _state = OrderInstertState.Insterted;
        public Type _type = null;

        public OrderInstert(ThostFtdcInputOrderField req, Type type)
        {
            _req = req;
            _type = type;
        }
    }

    public enum OrderInstertState : byte
    {
        Insterted = 0,
        Canceling = 1,
        Canceled = 2,
        //PartTraded = 3,
        //Traded = 4
    }

    public class OrderReturn
    {
        public ThostFtdcOrderField _pOrder = null;
        public Type _type = null;

        public OrderReturn(ThostFtdcOrderField pOrder, Type type)
        {
            _pOrder = pOrder;
            _type = type;
        }
    }

    public class TraderAPI
    {
        public event OrderHandle OnTrading;
        public void Trading(ThostFtdcOrderField pOrder)
        {
            if (OnTrading != null)
            {
                OnTrading(pOrder);
            }
        }

        public event OrderHandle OnCanceled;
        public void Canceled(ThostFtdcOrderField pOrder)
        {
            if (OnCanceled != null)
            {
                OnCanceled(pOrder);
            }
        }

        public event OrderHandle OnTraded;
        public void Traded(ThostFtdcOrderField pOrder)
        {
            if (OnTraded != null)
            {
                OnTraded(pOrder);
            }
        }

        public event PositionHandle onReqPosition;

        public void ReqPosition(ThostFtdcInvestorPositionField position)
        {
            if(onReqPosition != null)
            {
                onReqPosition(position);
            }
        }

        public bool bCanReq = false; //投资者结算确认后的查询控制
        public bool g_logined;
        public CTPTraderAdapter api = null;
        string FRONT_ADDR = "tcp://asp-sim2-front1.financial-trading-platform.com:26205";  // 前置地址
        string BROKER_ID = "2030";      // 经纪公司代码
        string INVESTOR_ID = "888888";  // 投资者代码
        string PASSWORD = "888888";     // 用户密码, 888888账户的密码被人改了，没法用了
        string INSTRUMENT_ID = "m1305";
        //EnumDirectionType DIRECTION = EnumDirectionType.Sell;
        int iRequestID = 0;
        //public int TodayCancel = 0;
        //public int TodayInsertOrder = 0;

        // 会话参数
        public int FRONT_ID;	//前置编号
        public int SESSION_ID;	//会话编号
        public int ORDER_REF;	//报单引用（当前）

        public List<OrderInstert> orderInstert = new List<OrderInstert>();
        public List<OrderReturn> orderList = new List<OrderReturn>();
        public List<OrderReturn> orderTraded = new List<OrderReturn>();
        public List<OrderReturn> orderCanceled = new List<OrderReturn>();
        public List<ThostFtdcTradeField> tradeList = new List<ThostFtdcTradeField>();
        public ThostFtdcTradingAccountField accountInfo = null;
        public List<ThostFtdcInvestorPositionField> positionList = new List<ThostFtdcInvestorPositionField>();

        public TraderAPI(string addr, string brokerID, string InvesterID, string password)
        {
            FRONT_ADDR = addr;
            BROKER_ID = brokerID;
            INVESTOR_ID = InvesterID;
            PASSWORD = password;
            api = new CTPTraderAdapter();
            AddEvent();
        }

        private void AddEvent()
        {
            api.OnFrontConnected += new FrontConnected(OnFrontConnected);
            api.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);
            api.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            api.OnRspError += new RspError(OnRspError);
            api.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            api.OnRspOrderAction += new RspOrderAction(OnRspOrderAction);
            api.OnRspQryOrder += new RspQryOrder(OnRspQryOrder);
            api.OnRspQryTrade += new RspQryTrade(OnRspQryTrade);
            api.OnRspOrderInsert += new RspOrderInsert(OnRspOrderInsert);
            api.OnRspQryInstrument += new RspQryInstrument(OnRspQryInstrument);
            api.OnRspQryInvestorPosition += new RspQryInvestorPosition(OnRspQryInvestorPosition);
            api.OnRspQryTradingAccount += new RspQryTradingAccount(OnRspQryTradingAccount);
            api.OnRspSettlementInfoConfirm += new RspSettlementInfoConfirm(OnRspSettlementInfoConfirm);
            api.OnRtnOrder += new RtnOrder(OnRtnOrder);
            api.OnRtnTrade += new RtnTrade(OnRtnTrade);
        }

        public void Connect()
        {
            api.SubscribePublicTopic(EnumTeResumeType.THOST_TERT_QUICK);
            api.SubscribePrivateTopic(EnumTeResumeType.THOST_TERT_QUICK);
            try
            {
                api.RegisterFront(FRONT_ADDR);
                api.Init();
                //ReqQryOrder();
                //Thread.Sleep(1000);
                //ReqQryTrade();
                //api.Join(); // 阻塞直到关闭或者CTRL+C
                //Release();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //GUIRefresh.UpdateMessageBox(e.Message);
            }
            finally
            {
                //Release();
            }
        }

        public void Release()
        {
            if (api != null)
            {
                g_logined = false;
                api.Release();
                api = null;
            }
        }

        void OnFrontConnected()
        {
            ReqUserLogin();
        }

        public void ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = BROKER_ID;
            req.UserID = INVESTOR_ID;
            req.Password = PASSWORD;
            int iResult = api.ReqUserLogin(req, ++iRequestID);
        }

        void OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                // 保存会话参数
                FRONT_ID = pRspUserLogin.FrontID;
                SESSION_ID = pRspUserLogin.SessionID;
                int iNextOrderRef = 0;
                if (!string.IsNullOrEmpty(pRspUserLogin.MaxOrderRef))
                    iNextOrderRef = Convert.ToInt32(pRspUserLogin.MaxOrderRef);
                //iNextOrderRef++;
                ORDER_REF = iNextOrderRef;
                ///投资者结算结果确认
                ReqSettlementInfoConfirm();
            }
        }

        void ReqSettlementInfoConfirm()
        {
            Thread.Sleep(1000);
            ThostFtdcSettlementInfoConfirmField req = new ThostFtdcSettlementInfoConfirmField();
            req.BrokerID = BROKER_ID;
            req.InvestorID = INVESTOR_ID;
            int iResult = api.ReqSettlementInfoConfirm(req, ++iRequestID);
        }

        void OnRspSettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                ///请求查询合约
                //ReqQryInstrument();
                
                g_logined = true;
            }
            if (bIsLast)
                bCanReq = true;
        }

        void ReqQryInstrument()
        {
            Thread.Sleep(1000);
            ThostFtdcQryInstrumentField req = new ThostFtdcQryInstrumentField();
            req.InstrumentID = INSTRUMENT_ID;
            int iResult = api.ReqQryInstrument(req, ++iRequestID);
        }

        void OnRspQryInstrument(ThostFtdcInstrumentField pInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pInstrument != null && bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                //请求查询资金
                //ReqQryTradingAccount();
            }
            if(bIsLast)
                bCanReq = true;
        }

        public void ReqQryTradingAccount()
        {
            //Thread.Sleep(1000);
            while (!bCanReq)
            {
                Thread.Sleep(50);
            }
            bCanReq = false;
            ThostFtdcQryTradingAccountField req = new ThostFtdcQryTradingAccountField();
            req.BrokerID = BROKER_ID;
            req.InvestorID = INVESTOR_ID;
            int iResult = api.ReqQryTradingAccount(req, ++iRequestID);
        }

        void OnRspQryTradingAccount(ThostFtdcTradingAccountField pTradingAccount, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                //请求查询投资者持仓
                //ReqQryInvestorPosition();
                accountInfo = pTradingAccount;
            }
            if (bIsLast)
            {
                bCanReq = true;
            }
        }

        public void ReqQryInvestorPosition()
        {
            Thread.Sleep(1000);
            while (!bCanReq)
            {
                Thread.Sleep(50);
            }
            bCanReq = false;
            ThostFtdcQryInvestorPositionField req = new ThostFtdcQryInvestorPositionField();
            req.BrokerID = BROKER_ID;
            req.InvestorID = INVESTOR_ID;
            //req.InstrumentID = INSTRUMENT_ID;
            positionList.Clear();
            int iResult = api.ReqQryInvestorPosition(req, ++iRequestID);
        }

        void OnRspQryInvestorPosition(ThostFtdcInvestorPositionField pInvestorPosition, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            positionList.Add(pInvestorPosition);
            if (bIsLast)
            { 
                bCanReq = true;
            }
        }

        void ReqQryOrder()
        {
            //Thread.Sleep(1000);
            while (!bCanReq)
            {
                Thread.Sleep(50);
            }
            bCanReq = false;
            ThostFtdcQryOrderField req = new ThostFtdcQryOrderField();
            req.BrokerID = BROKER_ID;
            ///投资者代码
            req.InvestorID = INVESTOR_ID;
            //Thread.Sleep(1000);
            int iResult = api.ReqQryOrder(req, ++iRequestID);
        }

        void OnRspQryOrder(ThostFtdcOrderField pOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //返回登录前的报单
            if (!IsErrorRspInfo(pRspInfo))
            {
                StackTrace trace = new StackTrace();
                Type StragetyType = trace.GetFrame(0).GetMethod().ReflectedType;
                ThostFtdcInputOrderField req = new ThostFtdcInputOrderField();
                req.BrokerID = pOrder.BrokerID;
                ///投资者代码
                req.InvestorID = pOrder.InvestorID;
                ///合约代码
                req.InstrumentID = pOrder.InstrumentID;
                ///报单引用
                req.OrderRef = pOrder.OrderRef;
                ///用户代码
                //	TThostFtdcUserIDType	UserID;
                ///报单价格条件: 限价
                req.OrderPriceType = pOrder.OrderPriceType;
                ///买卖方向: 
                req.Direction = pOrder.Direction;
                ///组合开平标志: 开仓
                req.CombOffsetFlag_0 = pOrder.CombOffsetFlag_0;
                ///组合投机套保标志
                req.CombHedgeFlag_0 = pOrder.CombHedgeFlag_0;
                ///价格
                req.LimitPrice = pOrder.LimitPrice;
                ///数量: 1
                req.VolumeTotalOriginal = pOrder.VolumeTotalOriginal;
                ///有效期类型: 当日有效
                req.TimeCondition = pOrder.TimeCondition;
                ///GTD日期
                //	TThostFtdcDateType	GTDDate;
                ///成交量类型: 任何数量
                req.VolumeCondition = pOrder.VolumeCondition;
                ///最小成交量: 1
                req.MinVolume = pOrder.MinVolume;
                ///触发条件: 立即
                req.ContingentCondition = pOrder.ContingentCondition;
                ///止损价
                //	TThostFtdcPriceType	StopPrice;
                ///强平原因: 非强平
                req.ForceCloseReason = pOrder.ForceCloseReason;
                ///自动挂起标志: 否
                req.IsAutoSuspend = pOrder.IsAutoSuspend;
                ///业务单元
                //	TThostFtdcBusinessUnitType	BusinessUnit;
                ///请求编号
                //	TThostFtdcRequestIDType	RequestID;
                ///用户强评标志: 否
                req.UserForceClose = pOrder.UserForceClose;
                OrderInstert order = new OrderInstert(req, StragetyType);
                orderInstert.Add(order);
                if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
                {
                    OrderReturn orderRtn = new OrderReturn(pOrder, StragetyType);
                    orderCanceled.Add(orderRtn);
                }
                else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
                {
                    OrderReturn orderRtn = new OrderReturn(pOrder, StragetyType);
                    orderTraded.Add(orderRtn);
                }
                else
                {
                    OrderReturn orderRtn = new OrderReturn(pOrder, StragetyType);
                    orderList.Add(orderRtn);
                }
            }
            if (bIsLast)
            {
                bCanReq = true;
                //ReqQryTrade();
            }
        }

        void ReqQryTrade()
        {
            //Thread.Sleep(1000);
            while (!bCanReq)
            {
                Thread.Sleep(50);
            }
            bCanReq = false;
            ThostFtdcQryTradeField req = new ThostFtdcQryTradeField();
            req.BrokerID = BROKER_ID;
            ///投资者代码
            req.InvestorID = INVESTOR_ID;
            //Thread.Sleep(1000);
            int iResult = api.ReqQryTrade(req, ++iRequestID);
        }

        void OnRspQryTrade(ThostFtdcTradeField pTrade, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!IsErrorRspInfo(pRspInfo) && pTrade != null)
            {
                tradeList.Add(pTrade);
            }
            if (bIsLast)
            {
                bCanReq = true;
            }
        }

        private string ReqOrderInsert(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            ORDER_REF++;
            ThostFtdcInputOrderField req = new ThostFtdcInputOrderField();
            ///经纪公司代码
            req.BrokerID = BROKER_ID;
            ///投资者代码
            req.InvestorID = INVESTOR_ID;
            ///合约代码
            req.InstrumentID = instrumentID;
            ///报单引用
            req.OrderRef = ORDER_REF.ToString();
            ///用户代码
            //	TThostFtdcUserIDType	UserID;
            ///报单价格条件: 限价
            req.OrderPriceType = CTP.EnumOrderPriceTypeType.LimitPrice;
            ///买卖方向: 
            req.Direction = DIRECTION;
            ///组合开平标志: 开仓
            req.CombOffsetFlag_0 = Offset;

            ///组合投机套保标志
            req.CombHedgeFlag_0 = CTP.EnumHedgeFlagType.Speculation;
            ///价格
            req.LimitPrice = price;
            ///数量: 1
            req.VolumeTotalOriginal = lots;
            ///有效期类型: 当日有效
            req.TimeCondition = CTP.EnumTimeConditionType.GFD;
            ///GTD日期
            //	TThostFtdcDateType	GTDDate;
            ///成交量类型: 任何数量
            req.VolumeCondition = CTP.EnumVolumeConditionType.AV;
            ///最小成交量: 1
            req.MinVolume = 1;
            ///触发条件: 立即
            req.ContingentCondition = CTP.EnumContingentConditionType.Immediately;
            ///止损价
            //	TThostFtdcPriceType	StopPrice;
            ///强平原因: 非强平
            req.ForceCloseReason = CTP.EnumForceCloseReasonType.NotForceClose;
            ///自动挂起标志: 否
            req.IsAutoSuspend = 0;
            ///业务单元
            //	TThostFtdcBusinessUnitType	BusinessUnit;
            ///请求编号
            //	TThostFtdcRequestIDType	RequestID;
            ///用户强评标志: 否
            req.UserForceClose = 0;
            int iResult = api.ReqOrderInsert(req, ++iRequestID);
            StackTrace trace = new StackTrace();
            Type StragetyType = trace.GetFrame(2).GetMethod().ReflectedType;
            OrderInstert order = new OrderInstert(req, StragetyType);
            orderInstert.Add(order);
            return req.OrderRef;
        }

        void OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //交易系统已接收报单
            IsErrorRspInfo(pRspInfo);
        }

        public int ReqOrderAction(string InstrumentID, string OrderRef)
        {
            ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
            ///经纪公司代码
            req.BrokerID = this.BROKER_ID;
            ///投资者代码
            req.InvestorID = this.INVESTOR_ID;
            ///报单操作引用
            //	TThostFtdcOrderActionRefType	OrderActionRef;
            ///报单引用
            req.OrderRef = OrderRef;
            ///请求编号
            //	TThostFtdcRequestIDType	RequestID;
            ///前置编号
            req.FrontID = FRONT_ID;
            ///会话编号
            req.SessionID = SESSION_ID;
            ///交易所代码
            //	TThostFtdcExchangeIDType	ExchangeID;
            ///报单编号
            //	TThostFtdcOrderSysIDType	OrderSysID;
            ///操作标志
            req.ActionFlag = CTP.EnumActionFlagType.Delete;
            ///价格
            //	TThostFtdcPriceType	LimitPrice;
            ///数量变化
            //	TThostFtdcVolumeType	VolumeChange;
            ///用户代码
            //	TThostFtdcUserIDType	UserID;
            ///合约代码
            req.InstrumentID = InstrumentID;
            int iResult = api.ReqOrderAction(req, ++iRequestID);
            return iResult;
        }

        public int ReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
            ///操作标志
            req.ActionFlag = CTP.EnumActionFlagType.Delete;
            ///经纪公司代码
            req.BrokerID = pInputOrderAction.BrokerID;
            ///交易所代码
            //	TThostFtdcExchangeIDType	ExchangeID;
            req.ExchangeID = pInputOrderAction.ExchangeID;
            ///前置编号
            req.FrontID = pInputOrderAction.FrontID;
            ///合约代码
            req.InstrumentID = pInputOrderAction.InstrumentID;
            ///投资者代码
            req.InvestorID = pInputOrderAction.InvestorID;
            ///报单操作引用
            //req.OrderActionRef = pInputOrderAction.OrderActionRef;
            ///报单引用
            req.OrderRef = pInputOrderAction.OrderRef;
            ///报单编号
            //	TThostFtdcOrderSysIDType	OrderSysID;
            req.OrderSysID = pInputOrderAction.OrderSysID;
            ///请求编号
            //	TThostFtdcRequestIDType	RequestID;
            //req.RequestID = pInputOrderAction.RequestID;
            ///会话编号
            req.SessionID = pInputOrderAction.SessionID;
            ///用户代码
            //	TThostFtdcUserIDType	UserID;
            req.UserID = pInputOrderAction.UserID;

            ///价格
            //	TThostFtdcPriceType	LimitPrice;
            ///数量变化
            //	TThostFtdcVolumeType	VolumeChange;            

            int iResult = api.ReqOrderAction(req, ++iRequestID);
            return iResult;
            //Console.WriteLine("--->>> 报单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 报单录入请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        public int ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
            ///操作标志
            req.ActionFlag = CTP.EnumActionFlagType.Delete;
            ///经纪公司代码
            req.BrokerID = pOrder.BrokerID;
            ///交易所代码
            //	TThostFtdcExchangeIDType	ExchangeID;
            req.ExchangeID = pOrder.ExchangeID;
            ///前置编号
            req.FrontID = pOrder.FrontID;
            ///投资者代码
            req.InvestorID = pOrder.InvestorID;
            ///合约代码
            req.InstrumentID = pOrder.InstrumentID;
            ///报单引用
            req.OrderRef = pOrder.OrderRef;
            ///报单编号
            //	TThostFtdcOrderSysIDType	OrderSysID;
            req.OrderSysID = pOrder.OrderSysID;
            //请求编号
            //	TThostFtdcRequestIDType	RequestID;
            //req.RequestID = pOrder.RequestID;
            ///会话编号
            req.SessionID = pOrder.SessionID;
            ///用户代码
            //	TThostFtdcUserIDType	UserID;
            req.UserID = pOrder.UserID;


            ///价格
            //	TThostFtdcPriceType	LimitPrice;
            ///数量变化
            //	TThostFtdcVolumeType	VolumeChange;

            int iResult = api.ReqOrderAction(req, ++iRequestID);
            //Console.WriteLine("--->>> 报单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 撤单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            return iResult;
        }

        public event CancelActionHandle OnCancelAction;
        public void CancelAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo)
        {
            if (OnCancelAction != null)
            {
                OnCancelAction(pInputOrderAction, pRspInfo);
            }
        }

        /// <summary>
        /// 撤单失败
        /// </summary>
        /// <param name="pInputOrderAction"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        public void OnRspOrderAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (IsErrorRspInfo(pRspInfo))
            {
            }
            if (bIsLast && IsErrorRspInfo(pRspInfo))
            {
                CancelAction(pInputOrderAction, pRspInfo);
                ////撤单失败
                //if (pRspInfo.ErrorID == 26 || pRspInfo.ErrorID == 25 || pRspInfo.ErrorID == 24)
                //{
                    
                //}
            }
        }

        ///报单通知
        void OnRtnOrder(ThostFtdcOrderField pOrder)
        {
            if (IsMyOrder(pOrder))
            {
                CurrentOrder = pOrder;
                if (IsTradingOrder(pOrder))
                {
                    Trading(pOrder);
                }
                else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
                {
                    Canceled(pOrder);
                }
                else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
                {
                    Traded(pOrder);
                }
                else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
                {

                }
            }
        }

        ThostFtdcOrderField CurrentOrder = null;

        /// <summary>
        /// 成交回报
        /// </summary>
        /// <param name="pTrade"></param>
        void OnRtnTrade(ThostFtdcTradeField pTrade)
        {
            if (IsMyTrade(CurrentOrder, pTrade))
            {
                tradeList.Add(pTrade);
            }
        }

        void OnFrontDisconnected(int nReason)
        {
            g_logined = false;
        }

        void OnHeartBeatWarning(int nTimeLapse)
        {

        }

        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            IsErrorRspInfo(pRspInfo);
        }

        public bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
            {

            }
            return bResult;
        }

        public bool IsMyOrder(ThostFtdcOrderField pOrder)
        {
            //return ((pOrder.FrontID == FRONT_ID) && (pOrder.SessionID == SESSION_ID));
            return true;
        }

        public bool IsMyTrade(ThostFtdcOrderField pOrder, ThostFtdcTradeField pTrade)
        {
            return (pTrade != null && pOrder != null &&
                    (pTrade.OrderRef == pOrder.OrderRef) &&
                    (pTrade.BrokerID == pOrder.BrokerID) &&
                    (pTrade.BrokerOrderSeq == pOrder.BrokerOrderSeq) &&
                    (pTrade.ExchangeID == pOrder.ExchangeID) &&
                    (pTrade.TraderID == pOrder.TraderID) &&
                    (pTrade.OrderLocalID == pOrder.OrderLocalID) &&
                    (pTrade.OrderSysID == pOrder.OrderSysID));
        }

        public bool IsTradingOrder(ThostFtdcOrderField pOrder)
        {
            //return ((pOrder.OrderStatus != EnumOrderStatusType.PartTradedNotQueueing) &&
            //        (pOrder.OrderStatus != EnumOrderStatusType.Canceled) &&
            //        (pOrder.OrderStatus != EnumOrderStatusType.AllTraded));
            return pOrder.OrderStatus != EnumOrderStatusType.Canceled;
        }

        public bool IsErrorOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus == EnumOrderStatusType.Unknown) ||
                pOrder.OrderSysID == "");
        }

        /// <summary>
        /// 买入开仓
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="lots"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public string Buy(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Buy, EnumOffsetFlagType.Open, instrumentID, lots, price);
            return orderref;
        }

        /// <summary>
        /// 卖出平仓
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="lots"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public string Sell(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Sell, EnumOffsetFlagType.CloseToday, instrumentID, lots, price);
            return orderref;
        }

        /// <summary>
        /// 买入平仓
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="lots"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public string BuyToCover(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Buy, EnumOffsetFlagType.CloseToday, instrumentID, lots, price);
            return orderref;
        }

        /// <summary>
        /// 卖出开仓
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="lots"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public string SellShort(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Sell, EnumOffsetFlagType.Open, instrumentID, lots, price);
            return orderref;
        }

        /// <summary>
        /// 取消报单
        /// </summary>
        /// <param name="order"></param>
        /// <returns>-1--无效报单，0--下撤单指令成功，1--下撤单指令失败</returns>
        public int CancelOrder(ThostFtdcOrderField order)  
        {
            if (order == null)
            {
                return 0;
            }
            else
            {
                return ReqOrderAction(order);
            }
        }

        public int CancelOrder(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            return ReqOrderAction(pInputOrderAction);
        }
    }
}
