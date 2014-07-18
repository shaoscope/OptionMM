using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using CTP;

namespace OptionMM
{
    public class TradeManager
    {
        public event OrderRefReplaceHandle OnOrderRefReplace;
        public event CancelActionHandle OnCancelAction;
        public event OrderHandle OnTrading;
        public event OrderHandle OnCanceled;
        public event OrderHandle OnTraded;
        public event PositionHandle onReqPosition;

        #region

        /// <summary>
        /// 报单回调
        /// </summary>
        /// <param name="orderrefold"></param>
        /// <param name="orderrefnew"></param>
        public void OrderRefReplace(string orderrefold, string orderrefnew)
        {
            if (OnOrderRefReplace != null)
            {
                OnOrderRefReplace(orderrefold, orderrefnew);
            }
        }

        public void CancelAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo)
        {
            if (OnCancelAction != null)
            {
                if (TradingOrderMap.ContainsKey(pInputOrderAction.OrderRef))
                {
                    if (TradingOrderMap[pInputOrderAction.OrderRef].OrigialOrderRef.Count > 0)
                    {
                        pInputOrderAction.OrderRef = TradingOrderMap[pInputOrderAction.OrderRef].OrigialOrderRef[0];
                    }
                }
                OnCancelAction(pInputOrderAction, pRspInfo);
            }
        }

        public void Trading(ThostFtdcOrderField pOrder)
        {
            if (OnTrading != null)
            {
                if (TradingOrderMap.ContainsKey(pOrder.OrderRef))
                {
                    if (TradingOrderMap[pOrder.OrderRef].OrigialOrderRef.Count > 0)
                    {
                        pOrder.OrderRef = TradingOrderMap[pOrder.OrderRef].OrigialOrderRef[0];
                    }
                }
                if (FinishedOrderMap.ContainsKey(pOrder.OrderRef))
                {
                    if (FinishedOrderMap[pOrder.OrderRef].OrigialOrderRef.Count > 0)
                    {
                        pOrder.OrderRef = FinishedOrderMap[pOrder.OrderRef].OrigialOrderRef[0];
                    }
                }
                OnTrading(pOrder);
            }
        }

        public void Canceled(ThostFtdcOrderField pOrder)
        {
            if (OnCanceled != null)
            {
                if (FinishedOrderMap.ContainsKey(pOrder.OrderRef))
                {
                    if (FinishedOrderMap[pOrder.OrderRef].OrigialOrderRef.Count > 0)
                    {
                        pOrder.OrderRef = FinishedOrderMap[pOrder.OrderRef].OrigialOrderRef[0];
                    }
                }
                DateTime logtime = DateTime.Now;
                OnCanceled(pOrder);
            }
        }

        public void Traded(ThostFtdcOrderField pOrder)
        {
            if (OnTraded != null)
            {
                if (FinishedOrderMap.ContainsKey(pOrder.OrderRef))
                {
                    if (FinishedOrderMap[pOrder.OrderRef].OrigialOrderRef.Count > 0)
                    {
                        pOrder.OrderRef = FinishedOrderMap[pOrder.OrderRef].OrigialOrderRef[0];
                    }
                }
                OnTraded(pOrder);
            }
        }

        public void ReqPosition(ThostFtdcInvestorPositionField position)
        {
            if (onReqPosition != null)
            {
                onReqPosition(position);
            }
        }

        #endregion

        /// <summary>
        /// 交易接口
        /// </summary>
        public CTPTraderAdapter trader { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trader"></param>
        public TradeManager(CTPTraderAdapter trader)
        {
            this.trader = trader;
            AddEvent();
        }

        public bool bCanReq = false; //投资者结算确认后的查询控制
        public bool g_logined;

        int iRequestID = 0;
        int MaxOrderExcuteCount = 50;

        public int ORDER_REF;	//报单引用（当前）
        public int StgOrderRef; //策略报单，TD实例自己维护；

        private Thread OrderReSender = null;    //重发线程

        public Dictionary<string, Order> ReInputOrderMap = new Dictionary<string, Order>();
        public Dictionary<string, Order> WaitInputOrderMap = new Dictionary<string, Order>();
        object ReInputOrderMapLock = new object();
        object WaitInputOrderMapLock = new object();
        public Dictionary<string, Order> TradingOrderMap = new Dictionary<string, Order>();
        object TradingOrderMapLock = new object();
        public Dictionary<string, Order> FinishedOrderMap = new Dictionary<string, Order>();
        //记录原始，最新单号
        public Dictionary<string, string> OrderSignalMap = new Dictionary<string, string>();
        public List<ThostFtdcInvestorPositionField> positionList = new List<ThostFtdcInvestorPositionField>();
        List<string> InputList = new List<string>();
        List<string> ActionList = new List<string>();
        object InputLock = new object();
        object ActionLock = new object();
        bool OrderReSendRun = false;
        bool OrderReSendRunning = false;
        bool IsReSending = false;
        object CancelOrderLock = new object();
        object CancelQuoteLock = new object();
        List<ThostFtdcOrderField> QuoteOrderList = new List<ThostFtdcOrderField>();
        Dictionary<string, string> TradingOrderQuoteMap = new Dictionary<string, string>();
        Dictionary<string, string> FinishedOrderQuoteMap = new Dictionary<string, string>();
        object TradingOrderQuoteMapLock = new object();

        Dictionary<string, BilateralQuotation> TradingQuoteMap = new Dictionary<string, BilateralQuotation>();
        object TradingQuoteMapLock = new object();
        Dictionary<string, BilateralQuotation> WaitInputQuoteMap = new Dictionary<string, BilateralQuotation>();
        object WaitInputQuoteMapLock = new object();
        Dictionary<string, BilateralQuotation> FinishedQuoteMap = new Dictionary<string, BilateralQuotation>();
        Dictionary<string, BilateralQuotation> ReInputQuoteMap = new Dictionary<string, BilateralQuotation>();
        object ReInputQuoteMapLock = new object();
        Dictionary<string, Order> InputOrderErrorMap = new Dictionary<string, Order>();
        object APITradeLock = new object();

        ThostFtdcOrderField CurrentOrder = null;
        int iQuoteRef = 0;
        Dictionary<string, string> QuoteSignalMap = new Dictionary<string, string>();

        /// <summary>
        /// 连接登陆
        /// </summary>
        #region
        private void AddEvent()
        {
            trader.OnRspForQuoteInsert += new RspForQuoteInsert(OnRspForQuoteInsert);     //询价录入请求响应
            trader.OnRspQuoteInsert += new RspQuoteInsert(OnRspQuoteInsert);           //报价录入请求响应
            trader.OnRspQuoteAction += new RspQuoteAction(OnRspQuoteAction);           //报价操作请求响应
            trader.OnRspQryForQuote += new RspQryForQuote(OnRspQryForQuote);           //请求查询询价响应
            trader.OnRspQryQuote += new RspQryQuote(OnRspQryQuote);               //请求查询报价响应
            trader.OnErrRtnForQuoteInsert += new ErrRtnForQuoteInsert(OnErrRtnForQuoteInsert); //询价录入错误回报
            trader.OnRtnQuote += new RtnQuote(OnRtnQuote);                       //报价通知
            trader.OnErrRtnQuoteInsert += new ErrRtnQuoteInsert(OnErrRtnQuoteInsert);     //报价录入错误回报
            trader.OnErrRtnQuoteAction += new ErrRtnQuoteAction(OnErrRtnQuoteAction);   //报价操作错误回报
            trader.OnRtnForQuoteRsp += new RtnForQuoteRsp(OnRtnForQuoteRsp);         //询价通知
        }
        #endregion

        /// <summary>
        /// 订单重发器
        /// </summary>
        private void OrderReSendCallBack()
        {
            OrderReSendRun = true;
            OrderReSendRunning = true;
            while (OrderReSendRun)
            {
                lock (CancelOrderLock)
                {
                    //优先撤单
                    lock (TradingOrderMapLock)
                    {
                        List<string> CanceledOrderSignal = new List<string>();
                        foreach (string signal in TradingOrderMap.Keys)
                        {
                            //撤单的不再重发
                            if (TradingOrderMap[signal].CancelOrder != null)
                            {
                                if (ReInputOrderMap.ContainsKey(signal))
                                {
                                    lock (ReInputOrderMapLock)
                                    {
                                        ReInputOrderMap.Remove(signal);
                                        CanceledOrderSignal.Add(signal);
                                        DateTime logtime = DateTime.Now;
                                        //Logger.AddToLoggerFile("RemoveReInput.txt", logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + signal.OrderRef + "," + ReInputOrderMap[signal].OrderField.InstrumentID);
                                    }
                                }
                                else if (WaitInputOrderMap.ContainsKey(signal))
                                {
                                    lock (WaitInputOrderMapLock)
                                    {
                                        WaitInputOrderMap.Remove(signal);
                                        CanceledOrderSignal.Add(signal);
                                        DateTime logtime = DateTime.Now;
                                        //Logger.AddToLoggerFile("WaitInput.txt", logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + signal.OrderRef + "," + WaitInputOrderMap[signal].OrderField.InstrumentID);
                                    }
                                }
                                if (TradingOrderMap[signal].CancelTime.Count > 0 && DateTime.Now > TradingOrderMap[signal].CancelTime[TradingOrderMap[signal].CancelTime.Count - 1].AddMilliseconds(500))
                                {
                                    //重新撤单
                                    ReReqOrderAction(TradingOrderMap[signal].CancelOrder);
                                }
                            }
                        }
                        foreach (string signal in CanceledOrderSignal)
                        {
                            FinishedOrderMap.Add(signal, TradingOrderMap[signal]);
                            TradingOrderMap.Remove(signal);
                        }
                    }
                }
                lock (CancelQuoteLock)
                {
                    //优先撤单
                    lock (TradingQuoteMapLock)
                    {
                        List<string> CanceledQuoteSignal = new List<string>();
                        foreach (string signal in TradingQuoteMap.Keys)
                        {
                            //撤单的不再重发
                            if (TradingQuoteMap[signal].CancelQuote != null)
                            {
                                if (ReInputQuoteMap.ContainsKey(signal))
                                {
                                    lock (ReInputQuoteMapLock)
                                    {
                                        ReInputQuoteMap.Remove(signal);
                                        CanceledQuoteSignal.Add(signal);
                                        DateTime logtime = DateTime.Now;
                                        //Logger.AddToLoggerFile("RemoveReInput.txt", logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + signal.OrderRef + "," + ReInputOrderMap[signal].OrderField.InstrumentID);
                                    }
                                }
                                else if (WaitInputQuoteMap.ContainsKey(signal))
                                {
                                    lock (WaitInputQuoteMapLock)
                                    {
                                        WaitInputQuoteMap.Remove(signal);
                                        CanceledQuoteSignal.Add(signal);
                                        DateTime logtime = DateTime.Now;
                                        //Logger.AddToLoggerFile("WaitInput.txt", logtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + signal.OrderRef + "," + WaitInputOrderMap[signal].OrderField.InstrumentID);
                                    }
                                }
                                if (TradingQuoteMap[signal].CancelTime.Count > 0 && DateTime.Now > TradingQuoteMap[signal].CancelTime[TradingQuoteMap[signal].CancelTime.Count - 1].AddMilliseconds(500))
                                {
                                    //重新撤单
                                    ReReqQuoteAction(TradingQuoteMap[signal].CancelQuote);
                                }
                            }
                        }
                        foreach (string signal in CanceledQuoteSignal)
                        {
                            FinishedQuoteMap.Add(signal, TradingQuoteMap[signal]);
                            TradingQuoteMap.Remove(signal);
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
                            foreach (Order pOrder in ReInputOrderMap.Values)
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
                            if (TradingOrderMap.ContainsKey(pOrder.OrderRef))
                            {
                                //没有撤单重发
                                if (TradingOrderMap[pOrder.OrderRef].CancelOrder == null)
                                {
                                    string OldOrderRef = pOrder.InputOrder.OrderRef;
                                    ThostFtdcInputOrderField req = GetNewInputOrderField(pOrder.InputOrder.Direction, pOrder.InputOrder.CombOffsetFlag_0,
                                        pOrder.InputOrder.InstrumentID, pOrder.InputOrder.VolumeTotalOriginal, pOrder.InputOrder.LimitPrice);
                                    string newOrderRef = req.OrderRef;

                                    DateTime logtime = DateTime.Now;
                                    Order newOrder = new Order(req, logtime);
                                    for (int i = 0; i < TradingOrderMap[OldOrderRef].OrigialOrderRef.Count; i++)
                                    {
                                        newOrder.OrigialOrderRef.Add(TradingOrderMap[OldOrderRef].OrigialOrderRef[i]);
                                    }
                                    newOrder.OrigialOrderRef.Add(pOrder.OrderRef);
                                    newOrder.OrderRef = newOrderRef;

                                    TradingOrderMap.Add(newOrderRef, newOrder);
                                    ReReqOrderInsert(newOrder);

                                    string oriSignal = OldOrderRef;
                                    if (TradingOrderMap[OldOrderRef].OrigialOrderRef.Count > 0)
                                    {
                                        oriSignal = TradingOrderMap[OldOrderRef].OrigialOrderRef[0];
                                    }

                                    if (OrderSignalMap.ContainsKey(oriSignal))
                                    {
                                        OrderSignalMap[oriSignal] = newOrderRef;
                                    }
                                    else
                                    {
                                        OrderSignalMap.Add(oriSignal, newOrderRef);
                                    }
                                    FinishedOrderMap.Add(OldOrderRef, pOrder);
                                    TradingOrderMap.Remove(OldOrderRef);
                                }
                                else
                                {
                                    int i = 0;
                                }
                            }
                            else
                            {
                                int i = 0;
                                //报错，不存在这种需要重发，但是没在发送队列中的情况。
                            }
                        }
                    }
                }
                if (ReInputQuoteMap.Count > 0)
                {
                    List<BilateralQuotation> SendingList = new List<BilateralQuotation>();
                    lock (ReInputQuoteMapLock)
                    {
                        if (ReInputQuoteMap.Count > 0)
                        {
                            foreach (BilateralQuotation pQuote in ReInputQuoteMap.Values)
                            {
                                SendingList.Add(pQuote);
                            }
                            ReInputQuoteMap.Clear();
                        }
                    }
                    foreach (BilateralQuotation pQuote in SendingList)
                    {
                        lock (TradingQuoteMapLock)
                        {
                            if (TradingQuoteMap.ContainsKey(pQuote.QuoteRef))
                            {
                                //没有撤单重发
                                if (TradingQuoteMap[pQuote.QuoteRef].CancelQuote == null)
                                {
                                    string OldQuoteRef = pQuote.InputQuote.QuoteRef;
                                    ThostFtdcInputQuoteField req = GetNewInputQuoteField(pQuote.InputQuote.InstrumentID,
                                        pQuote.InputQuote.AskOffsetFlag, pQuote.InputQuote.AskVolume, pQuote.InputQuote.AskPrice,
                                        pQuote.InputQuote.BidOffsetFlag, pQuote.InputQuote.BidVolume, pQuote.InputQuote.BidPrice);
                                    string newQuoteRef = req.QuoteRef;

                                    DateTime logtime = DateTime.Now;
                                    BilateralQuotation newQuote = new BilateralQuotation(req, logtime);
                                    for (int i = 0; i < TradingQuoteMap[OldQuoteRef].OrigialQuoteRef.Count; i++)
                                    {
                                        newQuote.OrigialQuoteRef.Add(TradingQuoteMap[OldQuoteRef].OrigialQuoteRef[i]);
                                    }
                                    newQuote.OrigialQuoteRef.Add(pQuote.QuoteRef);
                                    newQuote.QuoteRef = newQuoteRef;

                                    TradingQuoteMap.Add(newQuoteRef, newQuote);
                                    ReReqQuoteInsert(newQuote);

                                    string oriSignal = OldQuoteRef;
                                    if (TradingQuoteMap[OldQuoteRef].OrigialQuoteRef.Count > 0)
                                    {
                                        oriSignal = TradingQuoteMap[OldQuoteRef].OrigialQuoteRef[0];
                                    }

                                    if (QuoteSignalMap.ContainsKey(oriSignal))
                                    {
                                        QuoteSignalMap[oriSignal] = newQuoteRef;
                                    }
                                    else
                                    {
                                        QuoteSignalMap.Add(oriSignal, newQuoteRef);
                                    }
                                    FinishedQuoteMap.Add(OldQuoteRef, pQuote);
                                    TradingQuoteMap.Remove(OldQuoteRef);
                                }
                                else
                                {
                                    int i = 0;
                                }
                            }
                            else
                            {
                                int i = 0;
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
                            foreach (Order pOrder in WaitInputOrderMap.Values)
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
                            if (TradingOrderMap.ContainsKey(pOrder.OrderRef))
                            {
                                //没有撤单重发
                                if (TradingOrderMap[pOrder.OrderRef].CancelOrder == null)
                                {
                                    string OldOrderRef = pOrder.InputOrder.OrderRef;

                                    ThostFtdcInputOrderField req = GetNewInputOrderField(pOrder.InputOrder.Direction, pOrder.InputOrder.CombOffsetFlag_0,
                                        pOrder.InputOrder.InstrumentID, pOrder.InputOrder.VolumeTotalOriginal, pOrder.InputOrder.LimitPrice);
                                    string newOrderRef = req.OrderRef;

                                    DateTime logtime = DateTime.Now;
                                    Order newOrder = new Order(req, logtime);
                                    newOrder.OrigialOrderRef.Add(pOrder.OrderRef);
                                    newOrder.OrderRef = newOrderRef;

                                    TradingOrderMap.Add(newOrderRef, newOrder);
                                    ReReqOrderInsert(newOrder);

                                    OrderSignalMap.Add(OldOrderRef, newOrderRef);
                                    FinishedOrderMap.Add(OldOrderRef, pOrder);
                                    TradingOrderMap.Remove(OldOrderRef);
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                //报错，不存在这种需要重发，但是没在发送队列中的情况。
                            }
                        }
                    }
                }
                else if (WaitInputQuoteMap.Count > 0)
                {
                    List<BilateralQuotation> SendingList = new List<BilateralQuotation>();
                    lock (WaitInputQuoteMapLock)
                    {
                        if (WaitInputQuoteMap.Count > 0)
                        {
                            foreach (BilateralQuotation pQuote in WaitInputQuoteMap.Values)
                            {
                                SendingList.Add(pQuote);
                            }
                            WaitInputQuoteMap.Clear();
                        }
                    }
                    foreach (BilateralQuotation pQuote in SendingList)
                    {
                        lock (TradingQuoteMapLock)
                        {
                            if (TradingQuoteMap.ContainsKey(pQuote.QuoteRef))
                            {
                                //没有撤单重发
                                if (TradingQuoteMap[pQuote.QuoteRef].CancelQuote == null)
                                {
                                    string OldQuoteRef = pQuote.InputQuote.QuoteRef;

                                    ThostFtdcInputQuoteField req = GetNewInputQuoteField(pQuote.InputQuote.InstrumentID,
                                        pQuote.InputQuote.AskOffsetFlag, pQuote.InputQuote.AskVolume, pQuote.InputQuote.AskPrice,
                                        pQuote.InputQuote.BidOffsetFlag, pQuote.InputQuote.BidVolume, pQuote.InputQuote.BidPrice);
                                    string newQuoteRef = req.QuoteRef;

                                    DateTime logtime = DateTime.Now;
                                    BilateralQuotation newQuote = new BilateralQuotation(req, logtime);
                                    newQuote.OrigialQuoteRef.Add(pQuote.QuoteRef);
                                    newQuote.QuoteRef = newQuoteRef;

                                    TradingQuoteMap.Add(newQuoteRef, newQuote);
                                    ReReqQuoteInsert(newQuote);

                                    QuoteSignalMap.Add(OldQuoteRef, newQuoteRef);
                                    FinishedQuoteMap.Add(OldQuoteRef, pQuote);
                                    TradingQuoteMap.Remove(OldQuoteRef);
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                //报错，不存在这种需要重发，但是没在发送队列中的情况。
                            }
                        }
                    }
                }
                else// if (ReInputOrderMap.Count <= 0 && WaitInputOrderMap.Count <= 0)
                    IsReSending = false;

                Thread.Sleep(100);
            }
            OrderReSendRunning = false;
        }

        /// <summary>
        /// 报单重发
        /// </summary>
        /// <param name="req"></param>
        private void ReReqOrderInsert(Order pOrder)
        {
            DateTime logtime = DateTime.Now;
            lock (APITradeLock)
            {
                trader.ReqOrderInsert(pOrder.InputOrder, ++iRequestID);
            }
        }

        /// <summary>
        /// Private
        /// </summary>
        /// <param name="pRspInfo"></param>
        /// <returns></returns>
        #region
        private bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
            {

            }
            return bResult;
        }

        private bool IsMyOrder(ThostFtdcOrderField pOrder)
        {
            //return true;
            return ((pOrder.FrontID == trader.FrontID) && (pOrder.SessionID == trader.SessionID));
        }

        private bool IsMyQuote(ThostFtdcQuoteField pQuote)
        {
            //return true;
            return ((pQuote.FrontID == trader.FrontID) && (pQuote.SessionID == trader.SessionID));
        }

        private bool IsMyTrade(ThostFtdcOrderField pOrder, ThostFtdcTradeField pTrade)
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

        private bool IsTradingOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus != EnumOrderStatusType.PartTradedNotQueueing) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.Canceled) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.AllTraded) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.NoTradeNotQueueing));
            //return pOrder.OrderStatus != EnumOrderStatusType.Canceled;
        }

        private bool IsErrorOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus == EnumOrderStatusType.Unknown) ||
                pOrder.OrderSysID == "");
        }

        /// <summary>
        /// 生成新报单
        /// </summary>
        /// <param name="DIRECTION"></param>
        /// <param name="Offset"></param>
        /// <param name="instrumentID"></param>
        /// <param name="lots"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private ThostFtdcInputOrderField GetNewInputOrderField(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            lock (this)
            {
                ThostFtdcInputOrderField req = new ThostFtdcInputOrderField();
                ///经纪公司代码
                req.BrokerID = trader.BrokerID;
                ///投资者代码
                req.InvestorID = trader.InvestorID;
                ///合约代码
                req.InstrumentID = instrumentID;
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

                return req;
            }
        }

        private string ReqOrderInsert(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            int iResult = 0;
            ThostFtdcInputOrderField req = GetNewInputOrderField(DIRECTION, Offset, instrumentID, lots, price);

            DateTime logtime = DateTime.Now;

            Order newOrder = new Order(req, logtime);
            if (IsReSending)
            {
                string newOrderRef = req.OrderRef;
                newOrder.OrderRef = newOrderRef;

                //有重发的命令，将命令加入等待队列
                lock (TradingOrderMapLock)
                {
                    TradingOrderMap.Add(newOrderRef, newOrder);
                }
                lock (WaitInputOrderMapLock)
                {
                    WaitInputOrderMap.Add(newOrderRef, newOrder);
                }
            }
            else
            {
                lock (TradingOrderMapLock)
                {
                    ///报单引用
                    string newOrderRef = req.OrderRef;
                    newOrder.OrderRef = newOrderRef;

                    TradingOrderMap.Add(newOrderRef, newOrder);
                }
                lock (APITradeLock)
                {
                    iResult = trader.ReqOrderInsert(req, ++iRequestID);
                }
            }

            return req.OrderRef;
        }

        private int ReReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            int iResult = 0;
            string SignalOrderRef = pInputOrderAction.OrderRef;
            DateTime logtime = DateTime.Now;
            lock (TradingOrderMapLock)
            {
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    TradingOrderMap[SignalOrderRef].Cancel(pInputOrderAction, logtime);
                }
            }
            lock (APITradeLock)
            {
                iResult = trader.ReqOrderAction(pInputOrderAction, ++iRequestID);
            }
            return iResult;
        }

        private int ReReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            int iResult = 0;
            string SignalQuoteRef = pInputQuoteAction.QuoteRef;
            DateTime logtime = DateTime.Now;
            lock (TradingQuoteMapLock)
            {
                if (TradingQuoteMap.ContainsKey(SignalQuoteRef))
                {
                    TradingQuoteMap[SignalQuoteRef].Cancel(pInputQuoteAction, logtime);
                }
            }
            lock (APITradeLock)
            {
                iResult = trader.ReqQuoteAction(pInputQuoteAction, ++iRequestID);
            }
            return iResult;
        }

        private bool IsQuoteFinish(BilateralQuotation qoute)
        {
            bool bRet = false;
            if (!IsTradingOrder(qoute.AskOrderField) && !IsTradingOrder(qoute.BidOrderField))
                bRet = true;
            return bRet;
        }


        /// <summary>
        /// 生成新报价
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="AskOffset"></param>
        /// <param name="Asklots"></param>
        /// <param name="Askprice"></param>
        /// <param name="BidOffset"></param>
        /// <param name="Bidlots"></param>
        /// <param name="Bidprice"></param>
        /// <returns></returns>
        private ThostFtdcInputQuoteField GetNewInputQuoteField(string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
        {
            lock (this)
            {
                ThostFtdcInputQuoteField req = new ThostFtdcInputQuoteField();
                req.BrokerID = trader.BrokerID;
                req.InvestorID = trader.InvestorID;
                req.InstrumentID = instrumentID;
                req.AskHedgeFlag = EnumHedgeFlagType.Speculation;
                req.AskOffsetFlag = AskOffset;
                req.AskPrice = Askprice;
                req.AskVolume = Asklots;
                req.BidHedgeFlag = EnumHedgeFlagType.Speculation;
                req.BidOffsetFlag = BidOffset;
                req.BidPrice = Bidprice;
                req.BidVolume = Bidlots;
                iQuoteRef++;
                req.QuoteRef = iQuoteRef.ToString();

                return req;
            }
        }

        /// <summary>
        /// 重发
        /// </summary>
        /// <param name="req"></param>
        private void ReReqQuoteInsert(BilateralQuotation pQuote)//ThostFtdcInputOrderField req)
        {
            DateTime logtime = DateTime.Now;
            lock (APITradeLock)
            {
                trader.ReqQuoteInsert(pQuote.InputQuote, ++iRequestID);
            }
        }
        #endregion



        /// <summary>
        /// Public
        /// </summary>
        #region
        public void Release()
        {
            if (trader != null)
            {
                g_logined = false;
                OrderReSendRun = false;
                Thread.Sleep(50);
                if (OrderReSendRunning)
                {
                    OrderReSender.Abort();
                }

                trader.Release();
                trader = null;
            }
        }

        public void ReqOrderActionAll()
        {
            lock (TradingOrderMapLock)
            {
                foreach (string signal in TradingOrderMap.Keys)
                {
                    ReqOrderAction(TradingOrderMap[signal].OrderField);
                }
            }
        }

        public int ReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            lock (CancelOrderLock)
            {
                int iResult = 0;
                string SignalOrderRef = pInputOrderAction.OrderRef;
                DateTime logtime = DateTime.Now;
                lock (TradingOrderMapLock)
                {
                    if (OrderSignalMap.ContainsKey(SignalOrderRef))
                    {
                        SignalOrderRef = OrderSignalMap[SignalOrderRef];
                        pInputOrderAction.OrderRef = SignalOrderRef;
                    }
                    if (TradingOrderMap.ContainsKey(SignalOrderRef))
                    {
                        TradingOrderMap[SignalOrderRef].Cancel(pInputOrderAction, logtime);
                    }
                }
                lock (APITradeLock)
                {
                    iResult = trader.ReqOrderAction(pInputOrderAction, ++iRequestID);
                }        
                return iResult;
            }
        }

        public int ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            lock (CancelOrderLock)
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
                req.UserID = pOrder.UserID;

                ///价格
                //	TThostFtdcPriceType	LimitPrice;
                ///数量变化
                //	TThostFtdcVolumeType	VolumeChange;


                string SignalOrderRef = pOrder.OrderRef;
                DateTime logtime = DateTime.Now;
                lock (TradingOrderMapLock)
                {
                    if (OrderSignalMap.ContainsKey(SignalOrderRef))
                    {
                        SignalOrderRef = OrderSignalMap[SignalOrderRef];
                        req.OrderRef = SignalOrderRef;
                    }
                    if (TradingOrderMap.ContainsKey(SignalOrderRef))
                    {
                        TradingOrderMap[SignalOrderRef].Cancel(req, logtime);
                    }
                }
                lock (APITradeLock)
                {
                    iResult = trader.ReqOrderAction(req, ++iRequestID);
                }
                return iResult;
            }
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

        /// <summary>
        /// 询价录入请求
        /// </summary>
        public int PlaceForQuote(string instrumentID)
        {
            int iRet = 0;
            ThostFtdcInputForQuoteField req = new ThostFtdcInputForQuoteField();
            req.BrokerID = trader.BrokerID;
            req.InvestorID = trader.InvestorID;
            req.InstrumentID = instrumentID;
            iRet = trader.ReqForQuoteInsert(req, ++iRequestID);

            return iRet;
        }

        /// <summary>
        /// 报单录入
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="AskOffset"></param>
        /// <param name="Asklots"></param>
        /// <param name="Askprice"></param>
        /// <param name="BidOffset"></param>
        /// <param name="Bidlots"></param>
        /// <param name="Bidprice"></param>
        /// <returns></returns>
        public string PlaceQuote(string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
        {
            int iRet = 0;
            ThostFtdcInputQuoteField req = GetNewInputQuoteField(instrumentID, AskOffset, Asklots, Askprice, BidOffset, Bidlots, Bidprice);
            DateTime logTime = DateTime.Now;
            BilateralQuotation newQuote = new BilateralQuotation(req, logTime);
            if (IsReSending)
            {
                string SignalQuoteRef = req.QuoteRef;
                newQuote.QuoteRef = SignalQuoteRef;
                lock (TradingQuoteMapLock)
                {
                    TradingQuoteMap.Add(SignalQuoteRef, newQuote);
                }
                lock (WaitInputQuoteMapLock)
                {
                    WaitInputQuoteMap.Add(SignalQuoteRef, newQuote);
                }
            }
            else
            {
                lock (TradingQuoteMapLock)
                {
                    ///报单引用
                    string SignalQuoteRef = req.QuoteRef;
                    newQuote.QuoteRef = SignalQuoteRef;
                    TradingQuoteMap.Add(SignalQuoteRef, newQuote);
                }
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteInsert(req, ++iRequestID);
                }
            }
            return req.QuoteRef;
        }

        /// <summary>
        /// 报价删除
        /// </summary>
        public int DeleteQuote(ThostFtdcQuoteField pQuote)
        {
            lock (CancelQuoteLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = new ThostFtdcInputQuoteActionField();
                req.ActionFlag = EnumActionFlagType.Delete;
                req.BrokerID = trader.BrokerID;
                req.FrontID = pQuote.FrontID;
                req.ExchangeID = pQuote.ExchangeID;
                req.InstrumentID = pQuote.InstrumentID;
                req.InvestorID = pQuote.InvestorID;
                req.QuoteRef = pQuote.QuoteRef;
                req.SessionID = pQuote.SessionID;
                req.UserID = pQuote.UserID;
                string SignalQuoteRef = pQuote.QuoteRef;
                DateTime logtime = DateTime.Now;
                lock (TradingQuoteMapLock)
                {
                    if (QuoteSignalMap.ContainsKey(SignalQuoteRef))
                    {
                        SignalQuoteRef = QuoteSignalMap[SignalQuoteRef];
                    }
                    if (TradingQuoteMap.ContainsKey(SignalQuoteRef))
                    {
                        TradingQuoteMap[SignalQuoteRef].Cancel(req, logtime);
                    }
                }
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteAction(req, ++iRequestID);
                }
                return iRet;
            }
        }

        /// <summary>
        /// 报单删除
        /// </summary>
        /// <param name="pInputQuoteAction"></param>
        /// <returns></returns>
        public int DeleteQuote(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            int iRet = 0;
            string SignalQuoteRef = pInputQuoteAction.QuoteRef;
            DateTime logtime = DateTime.Now;
            lock (TradingQuoteMapLock)
            {
                if (TradingQuoteMap.ContainsKey(SignalQuoteRef))
                {
                    TradingQuoteMap[SignalQuoteRef].Cancel(pInputQuoteAction, logtime);
                }
            }
            lock (APITradeLock)
            {
                iRet = trader.ReqQuoteAction(pInputQuoteAction, ++iRequestID);
            }
            return iRet;
        }

        /// <summary>
        /// 报价删除
        /// </summary>
        public int DeleteQuote(string QuoteRef, string InstrumentID)
        {
            lock (CancelQuoteLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = new ThostFtdcInputQuoteActionField();
                req.ActionFlag = EnumActionFlagType.Delete;
                req.BrokerID = trader.BrokerID;
                req.InvestorID = trader.InvestorID;
                req.FrontID = trader.FrontID;
                req.InstrumentID = InstrumentID;
                req.QuoteRef = QuoteRef;
                req.SessionID = trader.SessionID;
                string SignalQuoteRef = QuoteRef;
                DateTime logtime = DateTime.Now;
                lock (TradingQuoteMapLock)
                {
                    if (QuoteSignalMap.ContainsKey(SignalQuoteRef))
                    {
                        SignalQuoteRef = QuoteSignalMap[SignalQuoteRef];
                    }
                    if (TradingQuoteMap.ContainsKey(SignalQuoteRef))
                    {
                        TradingQuoteMap[SignalQuoteRef].Cancel(req, logtime);
                    }
                }
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteAction(req, ++iRequestID);
                }
                return iRet;
            }
        }
        #endregion

        /// <summary>
        /// 报单、报价、行权、询价回调
        /// </summary>
        #region
        void OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //交易系统已接收报单
            IsErrorRspInfo(pRspInfo);
            {
                string SignalOrderRef = pInputOrder.OrderRef;
                lock (TradingOrderMapLock)
                {
                    if (TradingOrderMap.ContainsKey(SignalOrderRef))
                    {
                        InputOrderErrorMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                        TradingOrderMap.Remove(SignalOrderRef);
                    }
                    // 需要重发
                    //lock (ReInputOrderMapLock)
                    //{
                    //    TradingOrderMap[Signal].OrderField = pOrder;
                    //    IsReSending = true;
                    //    ReInputOrderMap.Add(Signal, TradingOrderMap[Signal]);
                    //}
                }
            }
        }
        /// <summary>
        /// 撤单失败
        /// </summary>
        /// <param name="pInputOrderAction"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspOrderAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DateTime logtime = DateTime.Now;
            if (IsErrorRspInfo(pRspInfo))
            {
            }
            if (bIsLast && IsErrorRspInfo(pRspInfo))
            {
                CancelAction(pInputOrderAction, pRspInfo);
            }
        }
        /// <summary>
        /// 报单操作错误回报
        /// </summary>
        /// <param name="pOrderAction"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnOrderAction(ThostFtdcOrderActionField pOrderAction, ThostFtdcRspInfoField pRspInfo)
        {
            DateTime logtime = DateTime.Now;
            if (pOrderAction.OrderActionStatus == EnumOrderActionStatusType.Rejected)
            {
                //撤单错误，需要重撤
            }
        }
        void OnErrRtnQuoteAction(ThostFtdcQuoteActionField pQuoteAction, ThostFtdcRspInfoField pRspInfo)
        {
            DateTime logtime = DateTime.Now;
            if (pQuoteAction.OrderActionStatus == EnumOrderActionStatusType.Rejected)
            {
                //撤单错误，需要重撤
            }
        }
        void OnRtnQuote(ThostFtdcQuoteField pQuote)
        {
            DateTime logtime = DateTime.Now;
            if (IsMyQuote(pQuote))
            {
                if (pQuote.ExchangeID != null && pQuote.ExchangeID != "" && pQuote.QuoteSysID != null && pQuote.QuoteSysID != "")
                {
                    if (pQuote.QuoteStatus == EnumOrderStatusType.Unknown)
                    {
                        if (QuoteOrderList.Count == 2)
                        {
                            lock (TradingQuoteMapLock)
                            {
                                if (TradingQuoteMap.ContainsKey(pQuote.QuoteRef))
                                {
                                    TradingQuoteMap[pQuote.QuoteRef].QuoteField = pQuote;
                                    foreach (ThostFtdcOrderField pOrder in QuoteOrderList)
                                    {
                                        if (pOrder.Direction == EnumDirectionType.Buy)
                                        {
                                            TradingQuoteMap[pQuote.QuoteRef].BidOrderField = pOrder;
                                            TradingOrderQuoteMap.Add(pOrder.OrderRef, pQuote.QuoteRef);
                                        }
                                        else if (pOrder.Direction == EnumDirectionType.Sell)
                                        {
                                            TradingQuoteMap[pQuote.QuoteRef].AskOrderField = pOrder;
                                            TradingOrderQuoteMap.Add(pOrder.OrderRef, pQuote.QuoteRef);
                                        }
                                    }
                                }
                            }
                            QuoteOrderList.Clear();
                        }
                        else
                        {
                            //CTP错误
                        }
                    }
                    else if (pQuote.QuoteStatus == EnumOrderStatusType.Touched)
                    {
                        //流量超限，需要重报
                        if (QuoteOrderList.Count == 2)
                        {
                            lock (ReInputQuoteMapLock)
                            {
                                TradingQuoteMap[pQuote.QuoteRef].QuoteField = pQuote;
                                IsReSending = true;
                                foreach (ThostFtdcOrderField pOrder in QuoteOrderList)
                                {
                                    if (pOrder.Direction == EnumDirectionType.Buy)
                                    {
                                        TradingQuoteMap[pQuote.QuoteRef].BidOrderField = pOrder;
                                    }
                                    else if (pOrder.Direction == EnumDirectionType.Sell)
                                    {
                                        TradingQuoteMap[pQuote.QuoteRef].AskOrderField = pOrder;
                                    }
                                }
                                ReInputQuoteMap.Add(pQuote.QuoteRef, TradingQuoteMap[pQuote.QuoteRef]);
                            }
                            QuoteOrderList.Clear();
                        }
                    }
                    else if (pQuote.QuoteStatus == EnumOrderStatusType.Canceled)
                    {
                        if (TradingQuoteMap.ContainsKey(pQuote.QuoteRef))
                        {
                            lock (TradingOrderMapLock)
                            {
                                //完成撤单
                                TradingQuoteMap[pQuote.QuoteRef].QuoteField = pQuote;
                                lock (TradingQuoteMapLock)
                                {
                                    FinishedQuoteMap.Add(pQuote.QuoteRef, TradingQuoteMap[pQuote.QuoteRef]);
                                    TradingQuoteMap.Remove(pQuote.QuoteRef);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 报单通知
        /// </summary>
        /// <param name="pOrder"></param>
        public void OnRtnOrder(ThostFtdcOrderField pOrder)
        {
            DateTime logtime = DateTime.Now;
            if (IsTradingOrder(pOrder))
            {
                if (IsMyOrder(pOrder))
                {
                    if (pOrder.ExchangeID != null && pOrder.ExchangeID != "" && pOrder.OrderSysID != null && pOrder.OrderSysID != "")
                    {
                        if (pOrder.OrderStatus != EnumOrderStatusType.Unknown)
                        {
                            string SignalOrderRef = pOrder.OrderRef;
                            if (TradingOrderMap.ContainsKey(SignalOrderRef))
                            {
                                lock (TradingOrderMapLock)
                                {
                                    //更新报单状态
                                    TradingOrderMap[SignalOrderRef].OrderField = pOrder;
                                }
                            }
                            else
                            {
                                lock (TradingQuoteMapLock)
                                {
                                    //更新报价状态
                                    string strQuoteRef = TradingOrderQuoteMap[pOrder.OrderRef];
                                    if (TradingQuoteMap.ContainsKey(strQuoteRef))
                                    {
                                        if (TradingQuoteMap[strQuoteRef].CancelQuote != null)
                                        {
                                            //撤报价
                                            if (TradingQuoteMap[strQuoteRef].AskOrderField.OrderRef == pOrder.OrderRef)
                                            {
                                                TradingQuoteMap[strQuoteRef].AskOrderField = pOrder;
                                            }
                                            else if (TradingQuoteMap[strQuoteRef].BidOrderField.OrderRef == pOrder.OrderRef)
                                            {
                                                TradingQuoteMap[strQuoteRef].BidOrderField = pOrder;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //双边报价单，此处需要处理
                            QuoteOrderList.Add(pOrder);
                        }
                    }
                    Trading(pOrder);
                }
                //Trading(pOrder);
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
            {
                if (IsMyOrder(pOrder))
                {
                    string SignalOrderRef = pOrder.OrderRef;
                    //TradingOrderMap.Remove(Signal);
                    bool bNeedCallBack = false;
                    if (TradingOrderMap.ContainsKey(SignalOrderRef))
                    {
                        lock (TradingOrderMapLock)
                        {
                            if (TradingOrderMap[SignalOrderRef].CancelOrder != null)
                            {
                                TradingOrderMap[SignalOrderRef].OrderField = pOrder;
                                FinishedOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);

                                TradingOrderMap.Remove(SignalOrderRef);
                                bNeedCallBack = true;
                            }
                            else
                            {
                                if (TradingOrderMap[SignalOrderRef].OrderField == null || TradingOrderMap[SignalOrderRef].OrderField.OrderStatus == EnumOrderStatusType.Unknown)
                                {
                                    //柜台自动撤单，需要重发
                                    lock (ReInputOrderMapLock)
                                    {
                                        TradingOrderMap[SignalOrderRef].OrderField = pOrder;
                                        IsReSending = true;
                                        ReInputOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                                    }
                                }
                                else
                                {
                                    //系统外撤单，不处理。
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TradingOrderQuoteMap.ContainsKey(pOrder.OrderRef))
                        {
                            lock (TradingQuoteMapLock)
                            {
                                string strQuoteRef = TradingOrderQuoteMap[pOrder.OrderRef];
                                if (TradingQuoteMap.ContainsKey(strQuoteRef))
                                {
                                    if (TradingQuoteMap[strQuoteRef].CancelQuote != null)
                                    {
                                        //撤报价
                                        if (TradingQuoteMap[strQuoteRef].AskOrderField.OrderRef == pOrder.OrderRef)
                                        {
                                            TradingQuoteMap[strQuoteRef].AskOrderField = pOrder;
                                        }
                                        else if (TradingQuoteMap[strQuoteRef].BidOrderField.OrderRef == pOrder.OrderRef)
                                        {
                                            TradingQuoteMap[strQuoteRef].BidOrderField = pOrder;
                                        }
                                        bNeedCallBack = true;   //?
                                    }
                                    else
                                    {
                                        if (TradingQuoteMap[strQuoteRef].QuoteField == null || TradingQuoteMap[strQuoteRef].QuoteField.QuoteStatus == EnumOrderStatusType.Unknown)
                                        {
                                            //自动撤单
                                            QuoteOrderList.Add(pOrder);
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    if (bNeedCallBack)
                    {
                        Canceled(pOrder);
                    }
                }

                //Canceled(pOrder);
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
            {
                if (IsMyOrder(pOrder))
                {
                    string SignalOrderRef = pOrder.OrderRef;
                    if (TradingOrderMap.ContainsKey(SignalOrderRef))
                    {
                        lock (TradingOrderMapLock)
                        {
                            FinishedOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                            TradingOrderMap.Remove(SignalOrderRef);
                        }
                    }
                    else
                    {
                        if (TradingOrderQuoteMap.ContainsKey(pOrder.OrderRef))
                        {
                            string strQuoteRef = TradingOrderQuoteMap[pOrder.OrderRef];
                            FinishedOrderQuoteMap.Add(pOrder.OrderRef, strQuoteRef);
                            TradingOrderQuoteMap.Remove(pOrder.OrderRef);
                            if (TradingQuoteMap.ContainsKey(strQuoteRef))
                            {
                                if (TradingQuoteMap[strQuoteRef].AskOrderField.OrderRef == pOrder.OrderRef)
                                {
                                    TradingQuoteMap[strQuoteRef].AskOrderField = pOrder;
                                }
                                else if (TradingQuoteMap[strQuoteRef].BidOrderField.OrderRef == pOrder.OrderRef)
                                {
                                    TradingQuoteMap[strQuoteRef].BidOrderField = pOrder;
                                }
                                if (IsQuoteFinish(TradingQuoteMap[strQuoteRef]))
                                {
                                    lock (TradingQuoteMapLock)
                                    {
                                        FinishedQuoteMap.Add(strQuoteRef, TradingQuoteMap[strQuoteRef]);
                                        TradingQuoteMap.Remove(strQuoteRef);
                                    }
                                }
                            }
                        }
                    }
                    Trading(pOrder);
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
            {
                string SignalOrderRef = pOrder.OrderRef;
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    lock (TradingOrderMapLock)
                    {
                        FinishedOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                        TradingOrderMap.Remove(SignalOrderRef);
                    }
                }
                else
                {
                    if (TradingOrderQuoteMap.ContainsKey(pOrder.OrderRef))
                    {
                        string strQuoteRef = TradingOrderQuoteMap[pOrder.OrderRef];
                        FinishedOrderQuoteMap.Add(pOrder.OrderRef, strQuoteRef);
                        TradingOrderQuoteMap.Remove(pOrder.OrderRef);
                        if (TradingQuoteMap.ContainsKey(strQuoteRef))
                        {
                            if (TradingQuoteMap[strQuoteRef].AskOrderField.OrderRef == pOrder.OrderRef)
                            {
                                TradingQuoteMap[strQuoteRef].AskOrderField = pOrder;
                            }
                            else if (TradingQuoteMap[strQuoteRef].BidOrderField.OrderRef == pOrder.OrderRef)
                            {
                                TradingQuoteMap[strQuoteRef].BidOrderField = pOrder;
                            }
                            if (IsQuoteFinish(TradingQuoteMap[strQuoteRef]))
                            {
                                lock (TradingQuoteMapLock)
                                {
                                    FinishedQuoteMap.Add(strQuoteRef, TradingQuoteMap[strQuoteRef]);
                                    TradingQuoteMap.Remove(strQuoteRef);
                                }
                            }
                        }
                    }
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.NoTradeNotQueueing)
            {
                string SignalOrderRef = pOrder.OrderRef;
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    lock (TradingOrderMapLock)
                    {
                        FinishedOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                        TradingOrderMap.Remove(SignalOrderRef);
                    }
                }
                else
                {
                    if (TradingOrderQuoteMap.ContainsKey(pOrder.OrderRef))
                    {
                        string strQuoteRef = TradingOrderQuoteMap[pOrder.OrderRef];
                        FinishedOrderQuoteMap.Add(pOrder.OrderRef, strQuoteRef);
                        TradingOrderQuoteMap.Remove(pOrder.OrderRef);
                        if (TradingQuoteMap.ContainsKey(strQuoteRef))
                        {
                            if (TradingQuoteMap[strQuoteRef].AskOrderField.OrderRef == pOrder.OrderRef)
                            {
                                TradingQuoteMap[strQuoteRef].AskOrderField = pOrder;
                            }
                            else if (TradingQuoteMap[strQuoteRef].BidOrderField.OrderRef == pOrder.OrderRef)
                            {
                                TradingQuoteMap[strQuoteRef].BidOrderField = pOrder;
                            }
                            if (IsQuoteFinish(TradingQuoteMap[strQuoteRef]))
                            {
                                lock (TradingQuoteMapLock)
                                {
                                    FinishedQuoteMap.Add(strQuoteRef, TradingQuoteMap[strQuoteRef]);
                                    TradingQuoteMap.Remove(strQuoteRef);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 成交回报
        /// </summary>
        /// <param name="pTrade"></param>
        public void OnRtnTrade(ThostFtdcTradeField pTrade)
        {
            if (IsMyTrade(CurrentOrder, pTrade))
            {
                //tradeList.Add(pTrade);
            }
        }
        void OnRspQuoteInsert(ThostFtdcInputQuoteField pInputQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }
        void OnRspQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DateTime logtime = DateTime.Now;
            if (IsErrorRspInfo(pRspInfo))
            {
            }
            if (bIsLast && IsErrorRspInfo(pRspInfo))
            {
                //QuoteCancelAction(pInputQuoteAction, pRspInfo);
            }
        }
        void OnRspForQuoteInsert(ThostFtdcInputForQuoteField pInputForQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }
        void OnRspQryForQuote(ThostFtdcForQuoteField pForQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }
        void OnRspQryQuote(ThostFtdcQuoteField pQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }
        void OnErrRtnForQuoteInsert(ThostFtdcInputForQuoteField pInputForQuote, ThostFtdcRspInfoField pRspInfo)
        {

        }
        void OnErrRtnQuoteInsert(ThostFtdcInputQuoteField pInputQuote, ThostFtdcRspInfoField pRspInfo)
        {

        }
        void OnRtnForQuoteRsp(ThostFtdcForQuoteRspField pForQuoteRsp)
        {

        }
        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DateTime logtime = DateTime.Now;
            IsErrorRspInfo(pRspInfo);
        }
        #endregion
    }
}
