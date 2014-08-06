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
            AddEvent();
        }

        /// <summary>
        /// 这些必须加载
        /// </summary>
        private void AddEvent()
        {
            trader.OnRspError += new RspError(OnRspError);
            trader.OnRspOrderAction += new RspOrderAction(OnRspOrderAction);
            trader.OnErrRtnOrderInsert += new ErrRtnOrderInsert(OnErrRtnOrderInsert);
            trader.OnRspOrderInsert += new RspOrderInsert(OnRspOrderInsert);
            trader.OnRtnOrder += new RtnOrder(OnRtnOrder);
            trader.OnRtnTrade += new RtnTrade(OnRtnTrade);
            trader.OnErrRtnOrderAction += new ErrRtnOrderAction(OnErrRtnOrderAction);

            trader.OnRspForQuoteInsert += new RspForQuoteInsert(OnRspForQuoteInsert);     //询价录入请求响应
            trader.OnRspQryForQuote += new RspQryForQuote(OnRspQryForQuote);           //请求查询询价响应
            trader.OnErrRtnForQuoteInsert += new ErrRtnForQuoteInsert(OnErrRtnForQuoteInsert); //询价录入错误回报
            trader.OnRspQryQuote += new RspQryQuote(OnRspQryQuote);               //请求查询报价响应
            trader.OnRspQuoteInsert += new RspQuoteInsert(OnRspQuoteInsert);           //报价录入请求响应
            trader.OnRspQuoteAction += new RspQuoteAction(OnRspQuoteAction);           //报价操作请求响应
            trader.OnRtnQuote += new RtnQuote(OnRtnQuote);                       //报价通知
            trader.OnErrRtnQuoteInsert += new ErrRtnQuoteInsert(OnErrRtnQuoteInsert);     //报价录入错误回报
            trader.OnErrRtnQuoteAction += new ErrRtnQuoteAction(OnErrRtnQuoteAction);   //报价操作错误回报

            trader.OnRspQryExecOrder += new RspQryExecOrder(OnRspQryExecOrder);                    //请求查询执行宣告响应
            trader.OnRtnExecOrder += new RtnExecOrder(OnRtnExecOrder);                             //执行宣告通知
            trader.OnErrRtnExecOrderInsert += new ErrRtnExecOrderInsert(OnErrRtnExecOrderInsert);  //执行宣告录入错误回报
            trader.OnErrRtnExecOrderAction += new ErrRtnExecOrderAction(OnErrRtnExecOrderAction);  //执行宣告操作错误回报
            trader.OnRspExecOrderInsert += new RspExecOrderInsert(OnRspExecOrderInsert);           //执行宣告录入请求响应
            trader.OnRspExecOrderAction += new RspExecOrderAction(OnRspExecOrderAction);           //执行宣告操作请求响应
        }

        int iRequestID = 0;
        object ReqLock = new object();
        int iForQuoteRef = 0;
        string BROKER_ID = null;
        string INVESTOR_ID = null;
        int QUOTE_REF = 0;
        int ORDER_REF = 0;
        ThostFtdcOrderField CurrentOrder = null;

        // 会话参数
        public int StgOrderRef; //策略报单，TD实例自己维护；

        public List<ThostFtdcInvestorPositionField> positionList = new List<ThostFtdcInvestorPositionField>();

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


        private ThostFtdcInputOrderActionField GetNewInputOrderActionField(ThostFtdcOrderField pOrder)
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
            req.OrderSysID = pOrder.OrderSysID;
            //请求编号
            //req.RequestID = pOrder.RequestID;
            ///会话编号
            req.SessionID = pOrder.SessionID;
            ///用户代码
            req.UserID = pOrder.UserID;
            ///价格
            //	TThostFtdcPriceType	LimitPrice;
            ///数量变化
            //	TThostFtdcVolumeType	VolumeChange;

            return req;
        }

        private ThostFtdcInputOrderField GetNewInputOrderField(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
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

            return req;
        }

        private ThostFtdcInputQuoteField GetNewInputQuoteField(string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
        {
            ThostFtdcInputQuoteField req = new ThostFtdcInputQuoteField();
            req.BrokerID = BROKER_ID;
            req.InvestorID = INVESTOR_ID;
            req.InstrumentID = instrumentID;
            req.AskHedgeFlag = EnumHedgeFlagType.Speculation;
            req.AskOffsetFlag = AskOffset;
            req.AskPrice = Askprice;
            req.AskVolume = Asklots;
            req.BidHedgeFlag = EnumHedgeFlagType.Speculation;
            req.BidOffsetFlag = BidOffset;
            req.BidPrice = Bidprice;
            req.BidVolume = Bidlots;
            QUOTE_REF++;
            req.QuoteRef = QUOTE_REF.ToString();

            return req;
        }

        private ThostFtdcInputQuoteActionField GetNewInputQuoteActionField(ThostFtdcQuoteField pQuote)
        {
            ThostFtdcInputQuoteActionField req = new ThostFtdcInputQuoteActionField();
            req.ActionFlag = EnumActionFlagType.Delete;
            req.BrokerID = BROKER_ID;
            req.FrontID = pQuote.FrontID;
            req.ExchangeID = pQuote.ExchangeID;
            req.InstrumentID = pQuote.InstrumentID;
            req.InvestorID = pQuote.InvestorID;
            req.QuoteRef = pQuote.QuoteRef;
            req.SessionID = pQuote.SessionID;
            req.UserID = pQuote.UserID;

            return req;
        }

        private ThostFtdcInputQuoteActionField GetNewInputQuoteActionField(string QuoteRef, string InstrumentID, int FRONT_ID, int SESSION_ID)
        {
            ThostFtdcInputQuoteActionField req = new ThostFtdcInputQuoteActionField();
            req.ActionFlag = EnumActionFlagType.Delete;
            req.BrokerID = BROKER_ID;
            req.FrontID = FRONT_ID;
            //req.ExchangeID = pQuote.ExchangeID;
            req.InstrumentID = InstrumentID;
            req.InvestorID = INVESTOR_ID;
            req.QuoteRef = QuoteRef;
            req.SessionID = SESSION_ID;
            //req.UserID = pQuote.UserID;

            return req;
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
        /// 错误回调
        /// </summary>
        #region
        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DateTime logtime = DateTime.Now;
            IsErrorRspInfo(pRspInfo);
        }
        #endregion

        /// <summary>
        /// 报单
        /// </summary>
        #region
        private string ReqOrderInsert(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            int iResult = 0;
            ThostFtdcInputOrderField req = GetNewInputOrderField(DIRECTION, Offset, instrumentID, lots, price);
            iResult = trader.ReqOrderInsert(req, ++iRequestID);
            return req.OrderRef;
        }

        public int ReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            lock (ReqLock)
            {
                int iResult = 0;
                iResult = trader.ReqOrderAction(pInputOrderAction, ++iRequestID);
                return iResult;
            }
        }

        public int ReqOrderAction(ThostFtdcOrderField pOrder)
        {
            lock (ReqLock)
            {
                int iResult = 0;
                ThostFtdcInputOrderActionField req = GetNewInputOrderActionField(pOrder);
                iResult = trader.ReqOrderAction(req, ++iRequestID);
                return iResult;
            }
        }

        void OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //交易系统已接收报单
            IsErrorRspInfo(pRspInfo);
            {

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

                        }
                    }
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.Canceled)
            {
                if (IsMyOrder(pOrder))
                {

                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.AllTraded)
            {
                if (IsMyOrder(pOrder))
                {

                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.PartTradedNotQueueing)
            {

            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.NoTradeNotQueueing)
            {

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

        void OnErrRtnOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo)
        {

        }
        #endregion

        /// <summary>
        /// 报价
        /// </summary>
        #region
        private Dictionary<string, string> OrderQuoteIndexMap = new Dictionary<string, string>();
        private Dictionary<string, ThostFtdcQuoteField> QuoteIndexMap = new Dictionary<string, ThostFtdcQuoteField>();
        /// <summary>
        /// 报价录入请求
        /// </summary>
        public string PlaceQuote(string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
        {
            int iRet = 0;
            ThostFtdcInputQuoteField req = GetNewInputQuoteField(instrumentID, AskOffset, Asklots, Askprice, BidOffset, Bidlots, Bidprice);
            iRet = trader.ReqQuoteInsert(req, ++iRequestID);
            return req.QuoteRef;
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        public int ReqQuoteAction(ThostFtdcQuoteField pQuote)
        {
            lock (ReqLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = GetNewInputQuoteActionField(pQuote);
                iRet = trader.ReqQuoteAction(req, ++iRequestID);
                return iRet;
            }
        }

        public int ReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            lock (ReqLock)
            {
                int iRet = 0;
                iRet = trader.ReqQuoteAction(pInputQuoteAction, ++iRequestID);
                return iRet;
            }
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        public int ReqQuoteAction(string QuoteRef, string InstrumentID)
        {
            lock (ReqLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = GetNewInputQuoteActionField(QuoteRef, InstrumentID, trader.FrontID, trader.SessionID);
                iRet = trader.ReqQuoteAction(req, ++iRequestID);

                return iRet;
            }
        }

        //请求查询报价响应
        void OnRspQryQuote(ThostFtdcQuoteField pQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        //报价录入请求响应
        void OnRspQuoteInsert(ThostFtdcInputQuoteField pInputQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        //报价操作请求响应
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

        //报价录入错误回报
        void OnErrRtnQuoteInsert(ThostFtdcInputQuoteField pInputQuote, ThostFtdcRspInfoField pRspInfo)
        {

        }

        //报价操作错误回报
        void OnErrRtnQuoteAction(ThostFtdcQuoteActionField pQuoteAction, ThostFtdcRspInfoField pRspInfo)
        {
            DateTime logtime = DateTime.Now;
            if (pQuoteAction.OrderActionStatus == EnumOrderActionStatusType.Rejected)
            {
                //撤单错误，需要重撤
            }
        }

        //报价通知
        void OnRtnQuote(ThostFtdcQuoteField pQuote)
        {
            DateTime logtime = DateTime.Now;
            if (IsMyQuote(pQuote))
            {
                if (pQuote.QuoteStatus == EnumOrderStatusType.Unknown)
                {
                    if (pQuote.ExchangeID != null && pQuote.ExchangeID != "" && pQuote.QuoteSysID != null && pQuote.QuoteSysID != "")
                    {

                    }
                }
                else if (pQuote.QuoteStatus == EnumOrderStatusType.NoTradeQueueing)
                {
                    QuoteIndexMap.Add(pQuote.QuoteRef, pQuote);
                    if (pQuote.AskOrderSysID != null || pQuote.AskOrderSysID != "")
                    {
                        OrderQuoteIndexMap.Add(pQuote.AskOrderSysID, pQuote.QuoteRef);
                    }
                    if (pQuote.BidOrderSysID != null || pQuote.BidOrderSysID != "")
                    {
                        OrderQuoteIndexMap.Add(pQuote.BidOrderSysID, pQuote.QuoteRef);
                    }
                }
                else if (pQuote.QuoteStatus == EnumOrderStatusType.Touched)
                {

                }
                else if (pQuote.QuoteStatus == EnumOrderStatusType.Canceled)
                {
                    //全部成交自动撤单
                    if (OrderQuoteIndexMap.ContainsKey(pQuote.AskOrderSysID))
                    {
                        OrderQuoteIndexMap.Remove(pQuote.AskOrderSysID);
                    }
                    if (OrderQuoteIndexMap.ContainsKey(pQuote.BidOrderSysID))
                    {
                        OrderQuoteIndexMap.Remove(pQuote.BidOrderSysID);
                    }
                    QuoteIndexMap.Remove(pQuote.QuoteRef);
                }
            }
        }
        #endregion

        /// <summary>
        /// 询价
        /// </summary>
        #region
        /// <summary>
        /// 询价录入请求
        /// </summary>
        public int ReqForQuoteInsert(string instrumentID)
        {
            lock (ReqLock)
            {
                int iRet = 0;
                ThostFtdcInputForQuoteField req = new ThostFtdcInputForQuoteField();
                req.BrokerID = trader.BrokerID;
                req.InvestorID = trader.InvestorID;
                req.InstrumentID = instrumentID;
                iForQuoteRef++;
                req.ForQuoteRef = iForQuoteRef.ToString();
                //req.ForQuoteRef = "1";    //?
                iRet = trader.ReqForQuoteInsert(req, ++iRequestID);
                return iRet;
            }
        }

        //询价录入请求响应
        void OnRspForQuoteInsert(ThostFtdcInputForQuoteField pInputForQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        //请求查询询价响应
        void OnRspQryForQuote(ThostFtdcForQuoteField pForQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        //询价录入错误回报
        void OnErrRtnForQuoteInsert(ThostFtdcInputForQuoteField pInputForQuote, ThostFtdcRspInfoField pRspInfo)
        {

        }
        #endregion


        /// <summary>
        /// 行权
        /// </summary>
        #region
        ///执行宣告操作请求响应
        void OnRspQryExecOrder(ThostFtdcExecOrderField pExecOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        ///执行宣告通知
        void OnRtnExecOrder(ThostFtdcExecOrderField pExecOrder)
        {

        }

        ///执行宣告录入错误回报
        void OnErrRtnExecOrderInsert(ThostFtdcInputExecOrderField pInputExecOrder, ThostFtdcRspInfoField pRspInfo)
        {

        }

        ///执行宣告操作错误回报
        void OnErrRtnExecOrderAction(ThostFtdcExecOrderActionField pExecOrderAction, ThostFtdcRspInfoField pRspInfo)
        {

        }

        ///执行宣告录入请求响应
        void OnRspExecOrderInsert(ThostFtdcInputExecOrderField pInputExecOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        ///执行宣告操作请求响应
        void OnRspExecOrderAction(ThostFtdcInputExecOrderActionField pInputExecOrderAction, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }
        #endregion
    }
}
