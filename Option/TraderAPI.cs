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
        //public bool _bCancelOrderAction = false;
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
        //List<OrderInstert> orderTraded = new List<OrderInstert>();
        //List<OrderInstert> orderCanceled = new List<OrderInstert>();

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
            //api.SubscribePublicTopic(EnumTeResumeType.THOST_TERT_RESTART);//THOST_TERT_QUICK);					// 注册公有流
            //api.SubscribePrivateTopic(EnumTeResumeType.THOST_TERT_RESTART);//THOST_TERT_QUICK);					// 注册私有流
            try
            {
                api.RegisterFront(FRONT_ADDR);
                api.Init();
                ReqQryOrder();
                Thread.Sleep(1000);
                ReqQryTrade();
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
            //__DEBUGPF__();
            ReqUserLogin();
        }

        public void ReqUserLogin()
        {
            ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
            req.BrokerID = BROKER_ID;
            req.UserID = INVESTOR_ID;
            req.Password = PASSWORD;
            int iResult = api.ReqUserLogin(req, ++iRequestID);
            //Console.WriteLine("--->>> 发送用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 发送交易用户登录请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                //GUIRefresh.UpdateListBox2("交易登陆成功");
                // 保存会话参数
                FRONT_ID = pRspUserLogin.FrontID;
                SESSION_ID = pRspUserLogin.SessionID;
                int iNextOrderRef = 0;
                if (!string.IsNullOrEmpty(pRspUserLogin.MaxOrderRef))
                    iNextOrderRef = Convert.ToInt32(pRspUserLogin.MaxOrderRef);
                //iNextOrderRef++;
                ORDER_REF = iNextOrderRef;
                ///获取当前交易日
                //Console.WriteLine("--->>> 获取当前交易日 = " + api.GetTradingDay());
                //GUIRefresh.UpdateListBox2("--->>> 获取交易当前交易日 = " + api.GetTradingDay());
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
            //Console.WriteLine("--->>> 投资者结算结果确认: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 投资者结算结果确认: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRspSettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
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
            //Console.WriteLine("--->>> 请求查询合约: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 请求查询合约: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRspQryInstrument(ThostFtdcInstrumentField pInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
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
            //Console.WriteLine("--->>> 请求查询资金账户: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 请求查询资金账户: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRspQryTradingAccount(ThostFtdcTradingAccountField pTradingAccount, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();

            if (bIsLast && !IsErrorRspInfo(pRspInfo))
            {
                //请求查询投资者持仓
                //ReqQryInvestorPosition();
                accountInfo = pTradingAccount;
            }
            if (bIsLast)
                bCanReq = true;
        }

        public void ReqQryInvestorPosition()
        {
            //Thread.Sleep(1000);
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
            //Console.WriteLine("--->>> 请求查询投资者持仓: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 请求查询投资者持仓: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRspQryInvestorPosition(ThostFtdcInvestorPositionField pInvestorPosition, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
            //if (!IsErrorRspInfo(pRspInfo))
            //{
            //    positionDictionary.Add(pInvestorPosition.InstrumentID, pInvestorPosition);
            //}
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
            //Console.WriteLine("--->>> 查询报单请求: " + ((iResult == 0) ? "成功" : "失败"));
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
            //Console.WriteLine("--->>> 查询报单请求: " + ((iResult == 0) ? "成功" : "失败"));
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
            //Console.WriteLine("--->>> 报单录入请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 报单录入请求: " + ((iResult == 0) ? "成功" : "失败"));
            StackTrace trace = new StackTrace();
            Type StragetyType = trace.GetFrame(2).GetMethod().ReflectedType;
            OrderInstert order = new OrderInstert(req, StragetyType);
            orderInstert.Add(order);
            return req.OrderRef;
        }

        void OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
            //交易系统已接收报单
            IsErrorRspInfo(pRspInfo);
        }

        public void ReqOrderAction(string InstrumentID, string OrderRef)
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
            //Console.WriteLine("--->>> 报单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 报单录入请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        public void ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
            ///经纪公司代码
            req.BrokerID = pOrder.BrokerID;
            ///投资者代码
            req.InvestorID = pOrder.InvestorID;
            ///报单操作引用
            //	TThostFtdcOrderActionRefType	OrderActionRef;
            ///报单引用
            req.OrderRef = pOrder.OrderRef;
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
            req.InstrumentID = pOrder.InstrumentID;

            int iResult = api.ReqOrderAction(req, ++iRequestID);
            //Console.WriteLine("--->>> 报单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
            //GUIRefresh.UpdateListBox2("--->>> 撤单操作请求: " + ((iResult == 0) ? "成功" : "失败"));
        }

        void OnRspOrderAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
            if (IsErrorRspInfo(pRspInfo))
            {
            }
            if (bIsLast && IsErrorRspInfo(pRspInfo))
            {
                //撤单失败
                if (pRspInfo.ErrorID == 26 || pRspInfo.ErrorID == 25 || pRspInfo.ErrorID == 24)
                {
                    foreach (OrderInstert order in orderInstert)
                    {
                        if (order._req.OrderRef == pInputOrderAction.OrderRef)
                            order._state = OrderInstertState.Insterted;
                    }
                    
                    //GUIRefresh.UpdateListBox1(pRspInfo.ErrorMsg);
                }
            }
        }

        ///报单通知
        void OnRtnOrder(ThostFtdcOrderField pOrder)
        {
            //__DEBUGPF__();
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
            /*foreach (OrderInstert order in orderInstert)
            {
                if (order._req.OrderRef == pOrder.OrderRef)
                {
                    if (IsTradingOrder(pOrder))
                    {
                        Trading(pOrder);
                        //tradeList.Add(pOrder);
                        /*bool bAdded = true;
                        foreach (OrderReturn orderRtn in orderList)
                        {
                            orderRtn._pOrder = pOrder;
                            bAdded = false;
                            break;
                        }
                        if (bAdded)
                        {
                            OrderReturn orderRtn = new OrderReturn(pOrder, order._type);
                            orderList.Add(orderRtn);
                        }
                            
                        //更新界面（未成交，成交记录）
                    }
                    else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
                    {
                        Canceled(pOrder);
                        if (order._state == OrderInstertState.Canceling) //CTP系统自动撤单
                        {
                            //order._bCancelOrderAction = false;
                            //orderCanceled.Add(order);
                            orderInstert.Remove(order);
                        }
                        else
                        {
                            order._state = OrderInstertState.Canceled;
                            foreach (OrderReturn orderRtn in orderList)
                            {
                                if (orderRtn._pOrder.OrderRef == pOrder.OrderRef)
                                {
                                    orderRtn._pOrder = pOrder;
                                    orderCanceled.Add(orderRtn);
                                    orderList.Remove(orderRtn);
                                        
                                    //撤单回调
                                    //OnCancel(pOrder);
                                    //更新界面（未成交）
                                    break;
                                }
                            }
                        }
                    }
                    else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
                    {
                        Traded(pOrder);
                        /*foreach (OrderReturn orderRtn in orderList)
                        {
                            if (orderRtn._pOrder.OrderRef == pOrder.OrderRef)
                            {
                                orderRtn._pOrder = pOrder;
                                orderTraded.Add(orderRtn);
                                orderList.Remove(orderRtn);
                                //成交回调
                                //OnTrade(pOrder);
                                //更新界面（未成交）
                                break;
                            }
                        }
                    }
                    else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
                    {
                        bool bAdded = true;
                        foreach (OrderReturn orderRtn in orderList)
                        {
                            orderRtn._pOrder = pOrder;
                            bAdded = false;
                            break;
                        }
                        if (bAdded)
                        {
                            OrderReturn orderRtn = new OrderReturn(pOrder, order._type);
                            orderList.Add(orderRtn);
                        }
                        //更新界面（未成交，成交记录）
                    }
                    break;
                }
            }
        }*/
        /*else
        {
            //重启，重新接受当天数据，维护各个ORDERLIST
            foreach (OrderInstert order in orderInstert)
            {
                if (order._req.OrderRef == pOrder.OrderRef)
                {
                    if (IsTradingOrder(pOrder))
                    {
                        //orderStateChange.Add(pOrder);
                        bool bAdded = true;
                        foreach (OrderReturn orderRtn in orderList)
                        {
                            orderRtn._pOrder = pOrder;
                            bAdded = false;
                            break;
                        }
                        if (bAdded)
                        {
                            OrderReturn orderRtn = new OrderReturn(pOrder, order._type);
                            orderList.Add(orderRtn);
                        }
                    }
                    else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
                    {
                        if (order._state == OrderInstertState.Canceling) //CTP系统自动撤单
                        {
                            orderInstert.Remove(order);
                        }
                        else
                        {
                            order._state = OrderInstertState.Canceled;
                            foreach (OrderReturn orderRtn in orderList)
                            {
                                if (orderRtn._pOrder.OrderRef == pOrder.OrderRef)
                                {
                                    orderRtn._pOrder = pOrder;
                                    orderCanceled.Add(orderRtn);
                                    orderList.Remove(orderRtn);
                                    break;
                                }
                            }
                        }
                    }
                    else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
                    {
                        foreach (OrderReturn orderRtn in orderList)
                        {
                            if (orderRtn._pOrder.OrderRef == pOrder.OrderRef)
                            {
                                orderRtn._pOrder = pOrder;
                                orderTraded.Add(orderRtn);
                                orderList.Remove(orderRtn);
                                break;
                            }
                        }
                    }
                    else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
                    {
                        bool bAdded = true;
                        foreach (OrderReturn orderRtn in orderList)
                        {
                            orderRtn._pOrder = pOrder;
                            bAdded = false;
                            break;
                        }
                        if (bAdded)
                        {
                            OrderReturn orderRtn = new OrderReturn(pOrder, order._type);
                            orderList.Add(orderRtn);
                        }
                    }
                    break;
                }
            }
        }
    }*/

        ThostFtdcOrderField CurrentOrder = null;
        ///成交通知
        void OnRtnTrade(ThostFtdcTradeField pTrade)
        {
            //__DEBUGPF__();
            if (IsMyTrade(CurrentOrder, pTrade))
            {
                tradeList.Add(pTrade);
            }
        }

        void OnFrontDisconnected(int nReason)
        {
            //__DEBUGPF__();
            //Console.WriteLine("--->>> Reason = {0}", nReason);
            g_logined = false;
            //GUIRefresh.UpdateListBox2("--->>> Reason = " + nReason);
        }

        void OnHeartBeatWarning(int nTimeLapse)
        {
            //__DEBUGPF__();
            //Console.WriteLine("--->>> nTimerLapse = " + nTimeLapse);
            //GUIRefresh.UpdateListBox2("--->>> nTimerLapse = " + nTimeLapse);
        }

        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //__DEBUGPF__();
            IsErrorRspInfo(pRspInfo);
        }

        public bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo == null) || (pRspInfo.ErrorID != 0));
            if (bResult)
            {
                //Console.WriteLine("--->>> ErrorID={0}, ErrorMsg={1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg);
                //GUIRefresh.UpdateListBox2(string.Format("--->>> ErrorID={0}, ErrorMsg={1}", pRspInfo.ErrorID, pRspInfo.ErrorMsg));
            }
            return bResult;
        }

        public bool IsMyOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.FrontID == FRONT_ID) &&
                (pOrder.SessionID == SESSION_ID));
                    //(pOrder.SessionID == SESSION_ID) &&
                    //(pOrder.OrderRef == ORDER_REF.ToString()));
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
                    (pOrder.OrderStatus != EnumOrderStatusType.AllTraded));
        }

        public bool IsErrorOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus == EnumOrderStatusType.Unknown) ||
                pOrder.OrderSysID == "");
        }

        void __DEBUGPF__()
        {
            StackTrace ss = new StackTrace(false);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            string str = "--->>> " + mb.DeclaringType.Name + "." + mb.Name + "()";
            Debug.WriteLine(str);
        }

        public string Buy(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Buy, EnumOffsetFlagType.Open, instrumentID, lots, price);
            return orderref;
        }

        public string Sell(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Sell, EnumOffsetFlagType.CloseToday, instrumentID, lots, price);
            return orderref;
        }

        public string BuyToCover(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Buy, EnumOffsetFlagType.CloseToday, instrumentID, lots, price);
            return orderref;
        }

        public string SellShort(string instrumentID, int lots, double price)
        {
            string orderref = ReqOrderInsert(EnumDirectionType.Sell, EnumOffsetFlagType.Open, instrumentID, lots, price);
            return orderref;
        }

        public void CancelOrder(string OrderRef)
        {
            foreach (OrderInstert order in orderInstert)
            {
                if (order._req.OrderRef == OrderRef && !(order._state == OrderInstertState.Canceling))
                {
                    ReqOrderAction(order._req.InstrumentID, OrderRef);
                    order._state = OrderInstertState.Canceling;
                }
            }
        }

        public void CancelOrder(ThostFtdcOrderField order)  
        {
            ReqOrderAction(order);
        }
    }
}
