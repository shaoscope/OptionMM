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
            //AddEvent();
        }

        /// <summary>
        /// 这些必须加载
        /// </summary>
        private void AddEvent()
        {
            //trader.OnFrontConnected += new FrontConnected(OnFrontConnected);
            //trader.OnFrontDisconnected += new FrontDisconnected(OnFrontDisconnected);
            //trader.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            trader.OnRspError += new RspError(OnRspError);
            //api.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            trader.OnRspOrderAction += new RspOrderAction(OnRspOrderAction);
            //trader.OnRspQryOrder += new RspQryOrder(OnRspQryOrder);
            //trader.OnRspQryTrade += new RspQryTrade(OnRspQryTrade);
            trader.OnErrRtnOrderInsert += new ErrRtnOrderInsert(OnErrRtnOrderInsert);
            trader.OnRspOrderInsert += new RspOrderInsert(OnRspOrderInsert);
            //trader.OnRspQryInstrument += new RspQryInstrument(OnRspQryInstrument);
            //trader.OnRspQryInvestorPosition += new RspQryInvestorPosition(OnRspQryInvestorPosition);
            //trader.OnRspQryTradingAccount += new RspQryTradingAccount(OnRspQryTradingAccount);
            //trader.OnRspSettlementInfoConfirm += new RspSettlementInfoConfirm(OnRspSettlementInfoConfirm);
            //api.OnRtnOrder += new RtnOrder(OnRtnOrder);
            //api.OnRtnTrade += new RtnTrade(OnRtnTrade);
            trader.OnErrRtnOrderAction += new ErrRtnOrderAction(OnErrRtnOrderAction);

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

        int iRequestID = 0;
        int MaxOrderExcuteCount = 50;

        // 会话参数
        public int StgOrderRef; //策略报单，TD实例自己维护；

        private Thread ReSender = null;    //重发线程

        bool ReSendRun = false;
        bool ReSendRunning = false;
        bool IsReSending = false;
        object CancelOrderLock = new object();
        object CancelQuoteLock = new object();

        public List<ThostFtdcInvestorPositionField> positionList = new List<ThostFtdcInvestorPositionField>();

        //报价指令第一次收到的两条OnRtnOrder指令
        List<ThostFtdcOrderField> QuoteOrderList = new List<ThostFtdcOrderField>();

        object APITradeLock = new object();
        ThostFtdcOrderField CurrentOrder = null;

        SendOrderManager orderManager = new SendOrderManager();
        SendQuoteManager quoteManager = new SendQuoteManager();

        /// <summary>
        /// 订单重发器
        /// </summary>
        private void ReSendCallBack()
        {
            ReSendRun = true;
            ReSendRunning = true;
            while (ReSendRun)
            {
                lock (CancelOrderLock)
                {
                    //优先撤单
                    List<ThostFtdcInputOrderActionField> ReCancelList = orderManager.ReCancelOrder();
                    //重新撤单
                    foreach (ThostFtdcInputOrderActionField CancelOrder in ReCancelList)
                    {
                        ReReqOrderAction(CancelOrder);
                    }

                }
                lock (CancelQuoteLock)
                {
                    //优先撤单
                    List<ThostFtdcInputQuoteActionField> ReCancelList = quoteManager.ReCancelQuote();
                    //重新撤单
                    foreach (ThostFtdcInputQuoteActionField CancelQuote in ReCancelList)
                    {
                        ReReqQuoteAction(CancelQuote);
                    }
                }

                if (orderManager.GetReInputOrderCount() > 0)
                {
                    List<ThostFtdcInputOrderField> ReInputOrderList = orderManager.ReInputOrder();
                    foreach (ThostFtdcInputOrderField req in ReInputOrderList)
                    {
                        ReReqOrderInsert(req);
                    }
                }
                else if (quoteManager.GetReInputQuoteCount() > 0)
                {
                    List<ThostFtdcInputQuoteField> ReInputQuoteList = quoteManager.ReInputQuote();
                    foreach (ThostFtdcInputQuoteField req in ReInputQuoteList)
                    {
                        ReReqQuoteInsert(req);
                    }
                }
                else if (orderManager.GetWaitInputOrderCount() > 0)
                {
                    List<ThostFtdcInputOrderField> WaitInputOrderList = orderManager.WaitInputOrder();
                    foreach (ThostFtdcInputOrderField req in WaitInputOrderList)
                    {
                        ReReqOrderInsert(req);
                    }
                }
                else if (quoteManager.GetWaitInputQuoteCount() > 0)
                {
                    List<ThostFtdcInputQuoteField> WaitInputQuoteList = quoteManager.WaitInputQuote();
                    foreach (ThostFtdcInputQuoteField req in WaitInputQuoteList)
                    {
                        ReReqQuoteInsert(req);
                    }
                }
                else
                    IsReSending = false;

                Thread.Sleep(100);
            }
            ReSendRunning = false;
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
        /// 重发
        /// </summary>
        /// <param name="req"></param>
        private void ReReqQuoteInsert(ThostFtdcInputQuoteField req)//ThostFtdcInputOrderField req)
        {
            DateTime logtime = DateTime.Now;
            lock (APITradeLock)
            {
                trader.ReqQuoteInsert(req, ++iRequestID);
            }
        }

        /// <summary>
        /// 报单重发
        /// </summary>
        /// <param name="req"></param>
        private void ReReqOrderInsert(ThostFtdcInputOrderField pInputOrder)//ThostFtdcInputOrderField req)
        {
            DateTime logtime = DateTime.Now;
            lock (APITradeLock)
            {
                trader.ReqOrderInsert(pInputOrder, ++iRequestID);
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
                ReSendRun = false;
                Thread.Sleep(50);
                if (ReSendRunning)
                {
                    ReSender.Abort();
                }

                trader.Release();
                trader = null;
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
                orderManager.AddInputOrderError(pInputOrder);
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
                if (pQuote.QuoteStatus == EnumOrderStatusType.Unknown)
                {
                    if (pQuote.ExchangeID != null && pQuote.ExchangeID != "" && pQuote.QuoteSysID != null && pQuote.QuoteSysID != "")
                    {
                        if (QuoteOrderList.Count == 2)
                        {
                            quoteManager.AddTradingOrderQuote(pQuote, QuoteOrderList);
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
                        quoteManager.QuoteTouched(pQuote);
                        IsReSending = true;
                    }
                    else if (pQuote.QuoteStatus == EnumOrderStatusType.Canceled)
                    {
                        quoteManager.QuoteCanceled(pQuote);
                    }
                }
            }
        }

        ///报单通知
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
                            if (orderManager.ContainsTrading(pOrder.OrderRef))
                            {
                                orderManager.UpdateTradingOrderMap(pOrder);
                            }
                            else
                            {
                                quoteManager.UpdateTradingQuoteMap(pOrder);
                            }
                        }
                        else
                        {
                            if (!orderManager.ContainsTrading(pOrder.OrderRef))
                            {
                                //双边报价单，此处需要处理
                                QuoteOrderList.Add(pOrder);
                            }
                        }
                    }
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
            {
                if (IsMyOrder(pOrder))
                {
                    string SignalOrderRef = pOrder.OrderRef;
                    //TradingOrderMap.Remove(Signal);
                    bool bNeedCallBack = false;
                    if (orderManager.ContainsTrading(SignalOrderRef))
                    {
                        int iCanceled = orderManager.OrderCanceled(pOrder);
                        if (iCanceled == 0)
                            bNeedCallBack = true;
                        else if (iCanceled == 1)
                            IsReSending = true;
                    }
                    else if (quoteManager.ContainsTrading(SignalOrderRef))
                    {
                        //双边报价单
                        quoteManager.OrderCanceled(pOrder);
                    }
                    if (bNeedCallBack)
                    {
                    }
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
            {
                if (IsMyOrder(pOrder))
                {
                    string SignalOrderRef = pOrder.OrderRef;
                    if (orderManager.ContainsTrading(SignalOrderRef))
                    {
                        orderManager.OrderFinished(pOrder);
                    }
                    else
                    {
                        quoteManager.OrderFinished(pOrder);
                    }
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
            {
                string SignalOrderRef = pOrder.OrderRef;
                if (orderManager.ContainsTrading(SignalOrderRef))
                {
                    orderManager.OrderFinished(pOrder);
                }
                else
                {
                    quoteManager.OrderFinished(pOrder);
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.NoTradeNotQueueing)
            {
                string SignalOrderRef = pOrder.OrderRef;
                if (orderManager.ContainsTrading(SignalOrderRef))
                {
                    orderManager.OrderFinished(pOrder);
                }
                else
                {
                    quoteManager.OrderFinished(pOrder);
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

        void OnErrRtnOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo)
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

        /// <summary>
        /// 报单、报价、行权、询价请求
        /// </summary>
        #region
        private string ReqOrderInsert(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            int iResult = 0;
            ThostFtdcInputOrderField req = orderManager.GetNewInputOrderField(DIRECTION, Offset, instrumentID, lots, price);
            DateTime logtime = DateTime.Now;
            OrderField newOrder = new OrderField(req, logtime);
            if (IsReSending)
            {
                orderManager.ReqOrderInsertWhenReSending(req);
            }
            else
            {
                orderManager.ReqOrderInsertWhenNotReSending(req);
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
            orderManager.ReReqOrderAction(pInputOrderAction);
            lock (APITradeLock)
            {
                iResult = trader.ReqOrderAction(pInputOrderAction, ++iRequestID);
            }

            return iResult;
        }
        public int ReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            lock (CancelOrderLock)
            {
                int iResult = 0;
                ThostFtdcInputOrderActionField req = orderManager.ReqOrderAction(pInputOrderAction);
                lock (APITradeLock)
                {
                    iResult = trader.ReqOrderAction(req, ++iRequestID);
                }
                return iResult;
            }
        }

        public int ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            lock (CancelOrderLock)
            {
                int iResult = 0;
                ThostFtdcInputOrderActionField req = orderManager.GetNewInputOrderActionField(pOrder);
                req = orderManager.ReqOrderAction(req);
                lock (APITradeLock)
                {
                    iResult = trader.ReqOrderAction(req, ++iRequestID);
                }
                return iResult;
            }
        }

        /// <summary>
        /// 报价录入请求
        /// </summary>
        public string PlaceQuote(string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
        {
            int iRet = 0;
            ThostFtdcInputQuoteField req = quoteManager.GetNewInputQuoteField(instrumentID, AskOffset, Asklots, Askprice, BidOffset, Bidlots, Bidprice);
            if (IsReSending)
            {
                quoteManager.ReqQuoteInsertWhenReSending(req);
            }
            else
            {
                quoteManager.ReqQuoteInsertWhenNotReSending(req);
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteInsert(req, ++iRequestID);
                }
            }
            return req.QuoteRef;
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        public int ReqQuoteAction(ThostFtdcQuoteField pQuote)
        {
            lock (CancelQuoteLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = quoteManager.GetNewInputQuoteActionField(pQuote);
                quoteManager.ReqQuoteAction(req);
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteAction(req, ++iRequestID);
                }
                return iRet;
            }
        }

        public int ReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            lock (CancelQuoteLock)
            {
                int iRet = 0;
                quoteManager.ReqQuoteAction(pInputQuoteAction);
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteAction(pInputQuoteAction, ++iRequestID);
                }
                return iRet;
            }
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        public int ReqQuoteAction(string QuoteRef, string InstrumentID)
        {
            lock (CancelQuoteLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = quoteManager.GetNewInputQuoteActionField(QuoteRef, InstrumentID, trader.FrontID, trader.SessionID);
                quoteManager.ReqQuoteAction(req);
                lock (APITradeLock)
                {
                    iRet = trader.ReqQuoteAction(req, ++iRequestID);
                }
                return iRet;
            }
        }

        private int ReReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            int iResult = 0;
            quoteManager.ReReqQuoteAction(pInputQuoteAction);
            lock (APITradeLock)
            {
                iResult = trader.ReqQuoteAction(pInputQuoteAction, ++iRequestID);
            }

            return iResult;
        }

        /// <summary>
        /// 询价录入请求
        /// </summary>
        public int ReqForQuoteInsert(string instrumentID)
        {
            int iRet = 0;
            ThostFtdcInputForQuoteField req = new ThostFtdcInputForQuoteField();
            req.BrokerID = trader.BrokerID;
            req.InvestorID = trader.InvestorID;
            req.InstrumentID = instrumentID;
            //req.ForQuoteRef = "1";    //?
            iRet = trader.ReqForQuoteInsert(req, ++iRequestID);

            return iRet;
        }
        #endregion
    }
}
