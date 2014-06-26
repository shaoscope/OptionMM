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
    public delegate void OrderRefReplaceHandle(string orderrefold, string orderrefnew);

    public class Order
    {
        public ThostFtdcInputOrderField InputOrder;
        public DateTime InputTime;
        public ThostFtdcInputOrderActionField CancelOrder;
        public DateTime CancelTime;
        //最新标志
        public OrderSignal Signal;
        //重发前标志
        public List<OrderSignal> OrigialSignal;
        public ThostFtdcOrderField OrderField;

        public Order(ThostFtdcInputOrderField pInput, DateTime pTime)
        {
            InputOrder = pInput;
            InputTime = pTime;
        }

        public void Cancel(ThostFtdcInputOrderActionField pInputAction, DateTime pTime)
        {
            CancelOrder = pInputAction;
            CancelTime = pTime;
        }
    }

    public struct OrderSignal
    {
        public string OrderRef;
        public int FrontID;
        public int SessionID;
        //public string ExchangeID;
        //public string OrderSysID;
    }

    public class TraderAPI
    {
        //private System.Threading.Timer OrderTimer;
        private Thread OrderReSender = null;

        public event OrderRefReplaceHandle OnOrderRefReplace;
        public void OrderRefReplace(string orderrefold, string orderrefnew)
        {
            if (OnOrderRefReplace != null)
            {
                OnOrderRefReplace(orderrefold, orderrefnew);
            }
        }

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
            if (onReqPosition != null)
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
        int MaxOrderExcuteCount = 50;
        
        //public int TodayCancel = 0;
        //public int TodayInsertOrder = 0;

        // 会话参数
        public int FRONT_ID;	//前置编号
        public int SESSION_ID;	//会话编号
        public int ORDER_REF;	//报单引用（当前）

        /// <summary>
        /// 系统启动前今天的报单情况
        /// </summary>
        //public Dictionary<OrderSignal, Order> TodayTradingOrderMap = new Dictionary<OrderSignal, Order>();
        //public Dictionary<OrderSignal, Order> TodayFinishedOrderMap = new Dictionary<OrderSignal, Order>();

        /// <summary>
        /// 系统启动后今天的报单情况
        /// </summary>
        public List<Order> ReInputOrderMap = new List<Order>();
        public List<Order> WaitInputOrderMap = new List<Order>();
        object ReInputOrderMapLock = new object();
        object WaitInputOrderMapLock = new object();

        //public Dictionary<OrderSignal, ThostFtdcInputOrderActionField> ReCancelOrderMap = new Dictionary<OrderSignal, ThostFtdcInputOrderActionField>();
        //public Dictionary<OrderSignal, ThostFtdcInputOrderActionField> WaitCancelOrderMap = new Dictionary<OrderSignal, ThostFtdcInputOrderActionField>();
        //object ReCancelOrderMapLock = new object();
        //object WaitCancelOrderMapLock = new object();

        public Dictionary<OrderSignal, Order> TradingOrderMap = new Dictionary<OrderSignal, Order>();
        object TradingOrderMapLock = new object();
        public Dictionary<OrderSignal, Order> FinishedOrderMap = new Dictionary<OrderSignal, Order>();

        //public Dictionary<DateTime, OrderSignal> OrderTimeMap = new Dictionary<DateTime, OrderSignal>();
        //object OrderTimeLock = new object();

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
            //api.OnErrRtnOrderInsert += new ErrRtnOrderInsert(OnErrRtnOrderInsert);
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
                //OrderReSender.Start();
                //this.OrderTimer = new System.Threading.Timer(this.OrderManagerCallBack, null, 1000, 1000);
                OrderReSender = new Thread(new ThreadStart(OrderReSendCallBack));
                OrderReSender.Start();
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

        List<OrderSignal> InputList = new List<OrderSignal>();
        List<OrderSignal> ActionList = new List<OrderSignal>();
        object InputLock = new object();
        object ActionLock = new object();
        
        //bool OrderManagerRunning = false;
        bool OrderReSendRun = false;
        bool OrderReSendRunning = false;

        bool IsReSending = false;
        /// <summary>
        /// 订单重发器
        /// </summary>
        private void OrderReSendCallBack()
        {
            OrderReSendRun = true;
            OrderReSendRunning = true;
            while(OrderReSendRun)
            {
                //优先撤单
                lock (TradingOrderMapLock)
                {
                    foreach (Order pOrder in TradingOrderMap.Values)
                    {
                        if (pOrder.CancelOrder != null)
                        {
                            //重新撤单
                            ReReqOrderAction(pOrder.CancelOrder);
                        }
                    }
                }

                if (ReInputOrderMap.Count > 0)
                {
                    List<Order> SendingList = new List<Order>();
                    lock (ReInputOrderMapLock)
                    {
                        if (ReInputOrderMap.Count > 0)
                        {
                            foreach (Order pOrder in ReInputOrderMap)
                            {
                                SendingList.Add(pOrder);
                            }
                            ReInputOrderMap.Clear();
                        }
                    }
                    foreach (Order pOrder in SendingList)
                    {
                        lock (TradingOrderMapLock)
                        {
                            if (TradingOrderMap.ContainsKey(pOrder.Signal))
                            {
                                if (TradingOrderMap[pOrder.Signal].CancelOrder != null)
                                {
                                    ThostFtdcInputOrderField req = new ThostFtdcInputOrderField();
                                    ///经纪公司代码
                                    req.BrokerID = BROKER_ID;
                                    ///投资者代码
                                    req.InvestorID = INVESTOR_ID;
                                    ///合约代码
                                    req.InstrumentID = pOrder.InputOrder.InstrumentID;

                                    ///用户代码
                                    //	TThostFtdcUserIDType	UserID;
                                    ///报单价格条件: 限价
                                    req.OrderPriceType = CTP.EnumOrderPriceTypeType.LimitPrice;
                                    ///买卖方向: 
                                    req.Direction = pOrder.InputOrder.Direction;
                                    ///组合开平标志: 开仓
                                    req.CombOffsetFlag_0 = pOrder.InputOrder.CombOffsetFlag_0;

                                    ///组合投机套保标志
                                    req.CombHedgeFlag_0 = pOrder.InputOrder.CombHedgeFlag_0;
                                    ///价格
                                    req.LimitPrice = pOrder.InputOrder.LimitPrice;
                                    ///数量: 1
                                    req.VolumeTotalOriginal = pOrder.InputOrder.VolumeTotalOriginal;
                                    ///有效期类型: 当日有效
                                    req.TimeCondition = pOrder.InputOrder.TimeCondition;
                                    ///GTD日期
                                    //	TThostFtdcDateType	GTDDate;
                                    ///成交量类型: 任何数量
                                    req.VolumeCondition = pOrder.InputOrder.VolumeCondition;
                                    ///最小成交量: 1
                                    req.MinVolume = pOrder.InputOrder.MinVolume;
                                    ///触发条件: 立即
                                    req.ContingentCondition = pOrder.InputOrder.ContingentCondition;
                                    ///止损价
                                    //	TThostFtdcPriceType	StopPrice;
                                    ///强平原因: 非强平
                                    req.ForceCloseReason = pOrder.InputOrder.ForceCloseReason;
                                    ///自动挂起标志: 否
                                    req.IsAutoSuspend = pOrder.InputOrder.IsAutoSuspend;
                                    ///业务单元
                                    //	TThostFtdcBusinessUnitType	BusinessUnit;
                                    ///请求编号
                                    //	TThostFtdcRequestIDType	RequestID;
                                    ///用户强评标志: 否
                                    req.UserForceClose = pOrder.InputOrder.UserForceClose;

                                    ORDER_REF++;
                                    OrderSignal OldSignal = new OrderSignal();
                                    OldSignal.FrontID = FRONT_ID;
                                    OldSignal.OrderRef = req.OrderRef;
                                    OldSignal.SessionID = SESSION_ID;

                                    req.OrderRef = ORDER_REF.ToString();
                                    OrderSignal newSignal = new OrderSignal();
                                    newSignal.FrontID = FRONT_ID;
                                    newSignal.OrderRef = req.OrderRef;
                                    newSignal.SessionID = SESSION_ID;
                                    DateTime logtime = DateTime.Now;
                                    Order newOrder = new Order(req, logtime);
                                    newOrder.OrigialSignal.Add(pOrder.Signal);
                                    TradingOrderMap.Add(newSignal, newOrder);
                                    ReReqOrderInsert(newOrder);
                                    //重发OrderRef更新回调
                                    OrderRefReplace(OldSignal.OrderRef, newOrder.InputOrder.OrderRef);

                                    FinishedOrderMap.Add(OldSignal, pOrder);
                                    TradingOrderMap.Remove(OldSignal);
                                }
                            }
                            else
                            {
                                //报错，不存在这种需要重发，但是没在发送队列中的情况。
                            }
                        }
                    }
                }
                else if (WaitInputOrderMap.Count > 0)
                {
                    List<Order> SendingList = new List<Order>();
                    lock (WaitInputOrderMapLock)
                    {
                        if (WaitInputOrderMap.Count > 0)
                        {
                            foreach (Order pOrder in WaitInputOrderMap)
                            {
                                SendingList.Add(pOrder);
                            }
                            WaitInputOrderMap.Clear();
                        }
                    }
                    foreach (Order pOrder in SendingList)
                    {
                        lock (TradingOrderMapLock)
                        {
                            ORDER_REF++;

                            pOrder.InputOrder.OrderRef = ORDER_REF.ToString();
                            pOrder.Signal.OrderRef = ORDER_REF.ToString();
                            OrderSignal Signal = new OrderSignal();
                            Signal.FrontID = FRONT_ID;
                            Signal.OrderRef = ORDER_REF.ToString();
                            Signal.SessionID = SESSION_ID;

                            TradingOrderMap.Add(Signal, pOrder);
                            ReReqOrderInsert(pOrder);
                        }
                    }
                }
                else// if (ReInputOrderMap.Count <= 0 && WaitInputOrderMap.Count <= 0)
                {
                    IsReSending = false;
                }
                Thread.Sleep(500);
            }
            OrderReSendRunning = false;
        }

        public void Release()
        {
            if (api != null)
            {
                g_logined = false;
                OrderReSendRun = false;
                Thread.Sleep(50);
                if(OrderReSendRunning)
                {
                    OrderReSender.Abort();
                }
                
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
            if (bIsLast)
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

        public void ReqQryOrder()
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
                OrderSignal Signal = new OrderSignal();
                Signal.OrderRef = req.OrderRef;
                Signal.FrontID = pOrder.FrontID;
                Signal.SessionID = pOrder.SessionID;
                //Signal.ExchangeID = pOrder.ExchangeID;
                //Signal.OrderSysID = pOrder.OrderSysID;
                //if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
                //{
                //    TodayFinishedOrderMap.Add(Signal, pOrder);
                //}
                //else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
                //{
                //    TodayFinishedOrderMap.Add(Signal, pOrder);
                //}
                //else
                //{
                //    TodayTradingOrderMap.Add(Signal, pOrder);
                //}
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
            }
            if (bIsLast)
            {
                bCanReq = true;
            }
        }

        /// <summary>
        /// 重发
        /// </summary>
        /// <param name="req"></param>
        private void ReReqOrderInsert(Order pOrder)//ThostFtdcInputOrderField req)
        {
            DateTime logtime = DateTime.Now;
            api.ReqOrderInsert(pOrder.InputOrder, ++iRequestID);

            //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "ReReqOrderInsert" + "," + pOrder.InputOrder.OrderRef + "," + pOrder.InputOrder.InstrumentID + "," + pOrder.InputOrder.Direction.ToString()
            //        + "," + pOrder.InputOrder.CombOffsetFlag_0.ToString());
        }

        private string ReqOrderInsert(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            int iResult = 0;
            
            ThostFtdcInputOrderField req = new ThostFtdcInputOrderField();
            ///经纪公司代码
            req.BrokerID = BROKER_ID;
            ///投资者代码
            req.InvestorID = INVESTOR_ID;
            ///合约代码
            req.InstrumentID = instrumentID;
            
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
            ORDER_REF++;
            req.OrderRef = ORDER_REF.ToString();
            
            DateTime logtime = DateTime.Now;

            Order newOrder = new Order(req, logtime);
            if (IsReSending)
            {
                //有重发的命令，将命令加入等待队列
                lock (WaitInputOrderMapLock)
                {
                    WaitInputOrderMap.Add(newOrder);
                }
            }
            else
            {
                lock (TradingOrderMapLock)
                {
                    ///报单引用
                    OrderSignal Signal = new OrderSignal();
                    Signal.FrontID = FRONT_ID;
                    Signal.OrderRef = req.OrderRef;
                    Signal.SessionID = SESSION_ID;

                    TradingOrderMap.Add(Signal, newOrder);
                }
                iResult = api.ReqOrderInsert(req, ++iRequestID);
                //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "ReqOrderInsert" + "," + ORDER_REF.ToString() + "," + req.InstrumentID + "," + req.Direction.ToString()
                //    + "," + req.CombOffsetFlag_0.ToString());
            }

            return req.OrderRef;
        }

        Dictionary<OrderSignal, Order> InputOrderErrorMap = new Dictionary<OrderSignal, Order>();
        void OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //交易系统已接收报单
            IsErrorRspInfo(pRspInfo);
            {
                OrderSignal Signal = new OrderSignal();
                Signal.FrontID = FRONT_ID;
                Signal.OrderRef = pInputOrder.OrderRef;
                Signal.SessionID = SESSION_ID;
                lock (TradingOrderMapLock)
                {
                    if (TradingOrderMap.ContainsKey(Signal))
                    {
                        InputOrderErrorMap.Add(Signal, TradingOrderMap[Signal]);
                        TradingOrderMap.Remove(Signal);
                    }
                }
            }
        }

        public void ReqOrderActionAll()
        {
            lock (TradingOrderMapLock)
            {
                foreach (OrderSignal signal in TradingOrderMap.Keys)
                {
                    ReqOrderAction(TradingOrderMap[signal].OrderField);
                }
            }
        }

        public int ReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            int iResult = 0;
            //ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
            OrderSignal Signal = new OrderSignal();
            Signal.FrontID = pInputOrderAction.FrontID;
            Signal.OrderRef = pInputOrderAction.OrderRef;
            Signal.SessionID = pInputOrderAction.SessionID;
            //Signal.ExchangeID = pInputOrderAction.ExchangeID;
            //Signal.OrderSysID = pInputOrderAction.OrderSysID;
            iResult = api.ReqOrderAction(pInputOrderAction, ++iRequestID);

            ///价格
            //	TThostFtdcPriceType	LimitPrice;
            ///数量变化
            //	TThostFtdcVolumeType	VolumeChange;            
            return iResult;
            //Console.WriteLine("--->>> 报单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 报单录入请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        public int ReReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            int iResult = 0;
            iResult = api.ReqOrderAction(pInputOrderAction, ++iRequestID);

            DateTime logtime = DateTime.Now;
            //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "ReqOrderAction" + "," + pInputOrderAction.OrderRef);
     
            return iResult;
            //Console.WriteLine("--->>> 报单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 报单录入请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        public int ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            int iResult = 0;
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

            
            OrderSignal Signal = new OrderSignal();
            Signal.FrontID = pOrder.FrontID;
            Signal.OrderRef = pOrder.OrderRef;
            Signal.SessionID = pOrder.SessionID;
            DateTime logtime = DateTime.Now;
            lock (TradingOrderMapLock)
            {
                TradingOrderMap[Signal].Cancel(req, logtime);
            }
            iResult = api.ReqOrderAction(req, ++iRequestID);
            //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "ReqOrderAction" + "," + req.OrderRef);
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
            DateTime logtime = DateTime.Now;
            //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "OnRspOrderAction" + "," + pInputOrderAction.OrderRef + "," + pRspInfo.ErrorMsg);
            if (IsErrorRspInfo(pRspInfo))
            {
            }
            if (bIsLast && IsErrorRspInfo(pRspInfo))
            {
                CancelAction(pInputOrderAction, pRspInfo);
            }
        }

        ///报单通知
        void OnRtnOrder(ThostFtdcOrderField pOrder)
        {
            DateTime logtime = DateTime.Now;
            //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "OnRtnOrder" + "," + pOrder.OrderRef + "," + pOrder.OrderStatus + "," + pOrder.StatusMsg);
            CurrentOrder = pOrder;
            if (IsTradingOrder(pOrder))
            {
                if (IsMyOrder(pOrder))
                {
                    if (pOrder.ExchangeID != null && pOrder.ExchangeID != "" && pOrder.OrderSysID != null && pOrder.OrderSysID != "")
                    {
                        if (pOrder.OrderStatus != EnumOrderStatusType.Unknown)
                        {
                            OrderSignal Signal = new OrderSignal();
                            Signal.FrontID = pOrder.FrontID;
                            Signal.OrderRef = pOrder.OrderRef;
                            Signal.SessionID = pOrder.SessionID;
                            lock (TradingOrderMapLock)
                            {
                                if (TradingOrderMap.ContainsKey(Signal))
                                {
                                    TradingOrderMap[Signal].OrderField = pOrder;
                                }
                            }
                        }
                    }
                }

                Trading(pOrder);
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
            {
                if (IsMyOrder(pOrder))
                {
                    OrderSignal Signal = new OrderSignal();
                    Signal.FrontID = pOrder.FrontID;
                    Signal.OrderRef = pOrder.OrderRef;
                    Signal.SessionID = pOrder.SessionID;
                    //TradingOrderMap.Remove(Signal);
                    lock (TradingOrderMapLock)
                    {
                        if (TradingOrderMap.ContainsKey(Signal))
                        {
                            if (TradingOrderMap[Signal].CancelOrder != null)
                            {
                                TradingOrderMap[Signal].OrderField = pOrder;
                                FinishedOrderMap.Add(Signal, TradingOrderMap[Signal]);

                                TradingOrderMap.Remove(Signal);
                            }
                            else
                            {
                                if (TradingOrderMap[Signal].OrderField == null || TradingOrderMap[Signal].OrderField.OrderStatus == EnumOrderStatusType.Unknown)
                                {
                                    //柜台自动撤单，需要重发
                                    lock (ReInputOrderMapLock)
                                    {
                                        TradingOrderMap[Signal].OrderField = pOrder;
                                        IsReSending = true;
                                        ReInputOrderMap.Add(TradingOrderMap[Signal]);
                                    }
                                }
                                else
                                {
                                    //系统外撤单，不处理。
                                }
                            }
                        }
                    }
                }

                Canceled(pOrder);
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
            {
                if (IsMyOrder(pOrder))
                {
                    OrderSignal Signal = new OrderSignal();
                    Signal.FrontID = pOrder.FrontID;
                    Signal.OrderRef = pOrder.OrderRef;
                    Signal.SessionID = pOrder.SessionID;
                    lock (TradingOrderMapLock)
                    {
                        if (TradingOrderMap.ContainsKey(Signal))
                        {
                            FinishedOrderMap.Add(Signal, TradingOrderMap[Signal]);
                            TradingOrderMap.Remove(Signal);
                        }
                    }
                }

                Traded(pOrder);
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
            {
                OrderSignal Signal = new OrderSignal();
                Signal.FrontID = pOrder.FrontID;
                Signal.OrderRef = pOrder.OrderRef;
                Signal.SessionID = pOrder.SessionID;
                lock (TradingOrderMapLock)
                {
                    if (TradingOrderMap.ContainsKey(Signal))
                    {
                        FinishedOrderMap.Add(Signal, TradingOrderMap[Signal]);
                        TradingOrderMap.Remove(Signal);
                    }
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.NoTradeNotQueueing)
            {
                OrderSignal Signal = new OrderSignal();
                Signal.FrontID = pOrder.FrontID;
                Signal.OrderRef = pOrder.OrderRef;
                Signal.SessionID = pOrder.SessionID;
                lock (TradingOrderMapLock)
                {
                    if (TradingOrderMap.ContainsKey(Signal))
                    {
                        FinishedOrderMap.Add(Signal, TradingOrderMap[Signal]);
                        TradingOrderMap.Remove(Signal);
                    }
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
                //tradeList.Add(pTrade);
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
            DateTime logtime = DateTime.Now;
            //GUI.GUIRefresh.UpdateListbox1(logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + "OnRspError" + "," + pRspInfo.ErrorMsg);
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
            return true;
            //return ((pOrder.FrontID == FRONT_ID) && (pOrder.SessionID == SESSION_ID));
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
            return ((pOrder.OrderStatus != EnumOrderStatusType.PartTradedNotQueueing) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.Canceled) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.AllTraded) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.NoTradeNotQueueing));
            //return pOrder.OrderStatus != EnumOrderStatusType.Canceled;
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
        //public int CancelOrder(ThostFtdcInputOrderField pInputOrderAction)
        //{
        //    return this.ReqOrderAction(pInputOrderAction);
        //}
    }
}
