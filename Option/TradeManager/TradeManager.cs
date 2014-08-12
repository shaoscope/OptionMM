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
    /// <summary>
    /// 成交管理
    /// </summary>
    public class TradeManager
    {
        /// <summary>
        /// 交易接口
        /// </summary>
        public CTPTraderAdapter trader { get; private set; }

        /// <summary>
        /// 请求服务器线程锁
        /// </summary>
        private object ReqLock = new object();

        /// <summary>
        /// 请求编号
        /// </summary>
        private int RequestID = 0;

        /// <summary>
        /// 询价单引用
        /// </summary>
        private int ForQuoteRef = 0;

        /// <summary>
        /// 做市报价引用
        /// </summary>
        private int QuoteRef = 0;

        /// <summary>
        /// 报价引用
        /// </summary>
        private int OrderRef = 0;

        /// <summary>
        /// 当前报单
        /// </summary>
        private ThostFtdcOrderField CurrentOrder;

        /// <summary>
        /// 报价单被成交时触发
        /// </summary>
        //public event RtnOrderEventHandler OnQuoteTraded;
        public event EventHandler<RtnTradeEventArgs> OnQuoteTraded;

        /// <summary>
        /// 套利单成交时触发
        /// </summary>
        //public event RtnOrderEventHandler OnArbitrageTraded;
        public event EventHandler<RtnTradeEventArgs> OnArbitrageTraded;

        /// <summary>
        /// 买卖报价单OrderSysID和对应做市单引用的映射
        /// </summary>
        private Dictionary<string, string> OrderSysID2QuoteRefMap = new Dictionary<string, string>();

        /// <summary>
        /// 做市报价单引用和报价单的映射
        /// </summary>
        private Dictionary<string, ThostFtdcQuoteField> QuoteRef2QuoteFieldMap = new Dictionary<string, ThostFtdcQuoteField>();

        private List<string> NeedRemoveQoute = new List<string>();

        /// <summary>
        /// 做市单引用和做市策略对象间的映射
        /// </summary>
        private Dictionary<string, Quote> QuoteRef2QuoteStategyMap = new Dictionary<string, Quote>();

        /// <summary>
        /// 报单引用和套利策略对象的映射
        /// </summary>
        private Dictionary<string, Arbitrage> OrderRef2ArbitrageStategyMap = new Dictionary<string, Arbitrage>();

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

        #region 请求部分
        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="sender">单子来源 Quote/Arbitrage</param>
        /// <param name="instrumentID">合约ID</param>
        /// <param name="directionType">买卖方向 Buy/Sell</param>
        /// <param name="offsetFlagType">开平标记 Open/CloseToday</param>
        /// <param name="lots">数量</param>
        /// <param name="price">价格</param>
        /// <returns>报单引用OrderRef</returns>
        public string PlaceOrder(object sender, string instrumentID, EnumDirectionType directionType, EnumOffsetFlagType offsetFlagType, int lots, double price)
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
            req.Direction = directionType;
            ///组合开平标志: 开仓
            req.CombOffsetFlag_0 = offsetFlagType;
            ///组合投机套保标志
            req.CombHedgeFlag_0 = CTP.EnumHedgeFlagType.Speculation;
            ///价格
            req.LimitPrice = price;
            ///数量: 1
            req.VolumeTotalOriginal = lots;
            ///有效期类型: 当日有效
            req.TimeCondition = CTP.EnumTimeConditionType.GFD;
            ///成交量类型: 任何数量
            req.VolumeCondition = CTP.EnumVolumeConditionType.AV;
            ///最小成交量: 1
            req.MinVolume = 1;
            ///触发条件: 立即
            req.ContingentCondition = CTP.EnumContingentConditionType.Immediately;
            ///强平原因: 非强平
            req.ForceCloseReason = CTP.EnumForceCloseReasonType.NotForceClose;
            ///自动挂起标志: 否
            req.IsAutoSuspend = 0;
            ///用户强评标志: 否
            req.UserForceClose = 0;
            OrderRef++;
            req.OrderRef = OrderRef.ToString();
            int iResult = trader.ReqOrderInsert(req, ++RequestID);

            //OrderRef2ArbitrageStategyMap.Add(req.OrderRef, (Arbitrage)sender);

            return req.OrderRef;
        }

        /// <summary>
        /// 报价录入请求
        /// </summary>
        public string PlaceQuote(object sender, string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
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
            QuoteRef++;
            req.QuoteRef = QuoteRef.ToString();
            int iRet = trader.ReqQuoteInsert(req, ++RequestID);

            //QuoteRef2QuoteStategyMap.Add(req.QuoteRef, (Quote)sender);

            return req.QuoteRef;
        }

        /// <summary>
        /// 取消报单
        /// </summary>
        /// <param name="order"></param>
        /// <returns>-1--无效报单，0--下撤单指令成功，1--下撤单指令失败</returns>
        public int CancelOrder(ThostFtdcOrderField pOrder)
        {
            lock (ReqLock)
            {
                ThostFtdcInputOrderActionField req = new ThostFtdcInputOrderActionField();
                ///操作标志
                req.ActionFlag = EnumActionFlagType.Delete;
                ///经纪公司代码
                req.BrokerID = pOrder.BrokerID;
                ///交易所代码
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
                int iResult = trader.ReqOrderAction(req, ++RequestID);

                if (OrderRef2ArbitrageStategyMap.ContainsKey(pOrder.OrderRef))
                {
                    OrderRef2ArbitrageStategyMap.Remove(pOrder.OrderRef);
                }
                return iResult;
            }
        }

        /// <summary>
        /// 撤销报单
        /// </summary>
        /// <param name="pInputOrderAction"></param>
        /// <returns></returns>
        public int CancelOrder(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            lock (ReqLock)
            {
                int iResult = trader.ReqOrderAction(pInputOrderAction, ++RequestID);
                if (OrderRef2ArbitrageStategyMap.ContainsKey(pInputOrderAction.OrderRef))
                {
                    OrderRef2ArbitrageStategyMap.Remove(pInputOrderAction.OrderRef);
                }
                return iResult;
            }
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        /// <param name="pQuote"></param>
        /// <returns></returns>
        public int ReqQuoteAction(ThostFtdcQuoteField pQuote)
        {
            lock (ReqLock)
            {
                ThostFtdcInputQuoteActionField req = new ThostFtdcInputQuoteActionField();
                req.ActionFlag = EnumActionFlagType.Delete;
                req.BrokerID = trader.BrokerID;
                req.FrontID = trader.FrontID;
                req.ExchangeID = pQuote.ExchangeID;
                req.InstrumentID = pQuote.InstrumentID;
                req.InvestorID = pQuote.InvestorID;
                req.QuoteRef = pQuote.QuoteRef;
                req.SessionID = pQuote.SessionID;
                req.UserID = pQuote.UserID;
                int iRet = trader.ReqQuoteAction(req, ++RequestID);
                if (QuoteRef2QuoteStategyMap.ContainsKey(pQuote.QuoteRef))
                {
                    QuoteRef2QuoteStategyMap.Remove(pQuote.QuoteRef);
                }
                return iRet;
            }
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        /// <param name="pInputQuoteAction"></param>
        /// <returns></returns>
        public int ReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            lock (ReqLock)
            {
                int iRet = 0;
                iRet = trader.ReqQuoteAction(pInputQuoteAction, ++RequestID);
                if (QuoteRef2QuoteStategyMap.ContainsKey(pInputQuoteAction.QuoteRef))
                {
                    QuoteRef2QuoteStategyMap.Remove(pInputQuoteAction.QuoteRef);
                }
                return iRet;
            }
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        /// <param name="QuoteRef"></param>
        /// <param name="InstrumentID"></param>
        /// <returns></returns>
        public int ReqQuoteAction(string QuoteRef, string InstrumentID)
        {
            lock (ReqLock)
            {
                int iRet = 0;
                ThostFtdcInputQuoteActionField req = new ThostFtdcInputQuoteActionField();
                req.ActionFlag = EnumActionFlagType.Delete;
                req.BrokerID = trader.BrokerID;
                req.FrontID = trader.FrontID;
                req.InstrumentID = InstrumentID;
                req.InvestorID = trader.InvestorID;
                req.QuoteRef = QuoteRef;
                req.SessionID = trader.SessionID;
                iRet = trader.ReqQuoteAction(req, ++RequestID);
                if (QuoteRef2QuoteStategyMap.ContainsKey(QuoteRef))
                {
                    QuoteRef2QuoteStategyMap.Remove(QuoteRef);
                }
                return iRet;
            }
        }
        #endregion

        #region 回报部分

        /// <summary>
        /// 报单回报
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
                if (IsMyOrder(pOrder))
                {
                }
            }
            else if (pOrder.OrderStatus == EnumOrderStatusType.NoTradeNotQueueing)
            {
                if (IsMyOrder(pOrder))
                {
                }
            }
        }

        /// <summary>
        /// 成交回报
        /// </summary>
        /// <param name="pTrade"></param>
        public void OnRtnTrade(ThostFtdcTradeField pTrade)
        {
            //if (IsMyTrade(CurrentOrder, pTrade))
            //{
            //    if (OrderQuoteIndexMap.ContainsKey(pTrade.OrderSysID))
            //    {
            //        pTrade.OrderSysID = OrderQuoteIndexMap[pTrade.OrderSysID];
            //        this.OnQuoteTraded(this, new RtnTradeEventArgs(pTrade));
            //    }
            //    else
            //    {
            //        this.OnArbitrageTraded(this, new RtnTradeEventArgs(pTrade));
            //    }
            //}
            if (OrderSysID2QuoteRefMap.ContainsKey(pTrade.OrderSysID))
            {
                pTrade.OrderSysID = OrderSysID2QuoteRefMap[pTrade.OrderSysID];
                this.OnQuoteTraded(this, new RtnTradeEventArgs(pTrade));
            }
            else
            {
                this.OnArbitrageTraded(this, new RtnTradeEventArgs(pTrade));
            }
        }

        /// <summary>
        /// 错误回调
        /// </summary>
        void OnRspError(ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            DateTime logtime = DateTime.Now;
            IsErrorRspInfo(pRspInfo);
        }

        /// <summary>
        /// 报单录入请求响应
        /// </summary>
        /// <param name="pInputOrder"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        private void OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //交易系统已接收报单
            IsErrorRspInfo(pRspInfo);
            {

            }
        }

        /// <summary>
        /// 报单操作请求响应
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

        /// <summary>
        /// 报单录入错误回报
        /// </summary>
        /// <param name="pInputOrder"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo)
        {

        }

        /// <summary>
        /// 报价录入错误回报
        /// </summary>
        /// <param name="pInputQuote"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnQuoteInsert(ThostFtdcInputQuoteField pInputQuote, ThostFtdcRspInfoField pRspInfo)
        {

        }

        /// <summary>
        /// 请求查询报价响应
        /// </summary>
        /// <param name="pQuote"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspQryQuote(ThostFtdcQuoteField pQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        /// <summary>
        /// 报价录入请求响应
        /// </summary>
        /// <param name="pInputQuote"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspQuoteInsert(ThostFtdcInputQuoteField pInputQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        /// <summary>
        /// 报价操作请求响应
        /// </summary>
        /// <param name="pInputQuoteAction"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
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

        /// <summary>
        /// 报价操作错误回报
        /// </summary>
        /// <param name="pQuoteAction"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnQuoteAction(ThostFtdcQuoteActionField pQuoteAction, ThostFtdcRspInfoField pRspInfo)
        {
            DateTime logtime = DateTime.Now;
            if (pQuoteAction.OrderActionStatus == EnumOrderActionStatusType.Rejected)
            {
                //撤单错误，需要重撤
            }
        }

        /// <summary>
        /// 报价通知
        /// </summary>
        /// <param name="pQuote"></param>
        void OnRtnQuote(ThostFtdcQuoteField pQuote)
        {
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
                    QuoteRef2QuoteFieldMap.Add(pQuote.QuoteRef, pQuote);
                    if (pQuote.AskOrderSysID != null || pQuote.AskOrderSysID != "")
                    {
                        OrderSysID2QuoteRefMap.Add(pQuote.AskOrderSysID, pQuote.QuoteRef);
                    }
                    if (pQuote.BidOrderSysID != null || pQuote.BidOrderSysID != "")
                    {
                        OrderSysID2QuoteRefMap.Add(pQuote.BidOrderSysID, pQuote.QuoteRef);
                    }
                }
                else if (pQuote.QuoteStatus == EnumOrderStatusType.Touched)
                {

                }
                else if (pQuote.QuoteStatus == EnumOrderStatusType.Canceled)
                {
                    //全部成交自动撤单
                    if (OrderSysID2QuoteRefMap.ContainsKey(pQuote.AskOrderSysID))
                    {
                        OrderSysID2QuoteRefMap.Remove(pQuote.AskOrderSysID);
                    }
                    if (OrderSysID2QuoteRefMap.ContainsKey(pQuote.BidOrderSysID))
                    {
                        OrderSysID2QuoteRefMap.Remove(pQuote.BidOrderSysID);
                    }
                    QuoteRef2QuoteFieldMap.Remove(pQuote.QuoteRef);
                }
            }
        }
        #endregion

        #region 条件判断部分
        /// <summary>
        /// 判断是否是错误信息
        /// </summary>
        /// <param name="pRspInfo"></param>
        /// <returns></returns>
        private bool IsErrorRspInfo(ThostFtdcRspInfoField pRspInfo)
        {
            // 如果ErrorID != 0, 说明收到了错误的响应
            bool bResult = ((pRspInfo != null) && (pRspInfo.ErrorID != 0));
            if (bResult)
            {

            }
            return bResult;
        }

        /// <summary>
        /// 判断是否是我的报单
        /// </summary>
        /// <param name="pOrder"></param>
        /// <returns></returns>
        private bool IsMyOrder(ThostFtdcOrderField pOrder)
        {
            //return true;
            return ((pOrder.FrontID == trader.FrontID) && (pOrder.SessionID == trader.SessionID));
        }

        /// <summary>
        /// 判断是否是我的做市单
        /// </summary>
        /// <param name="pQuote"></param>
        /// <returns></returns>
        private bool IsMyQuote(ThostFtdcQuoteField pQuote)
        {
            return true;
            //return ((pQuote.FrontID == trader.FrontID) && (pQuote.SessionID == trader.SessionID));
        }

        /// <summary>
        /// 判断成交单是否是我的报单
        /// </summary>
        /// <param name="pOrder"></param>
        /// <param name="pTrade"></param>
        /// <returns></returns>
        private bool IsMyTrade(ThostFtdcOrderField pOrder, ThostFtdcTradeField pTrade)
        {
            return (pTrade != null && pOrder != null && (pTrade.OrderRef == pOrder.OrderRef) &&
                    (pTrade.BrokerID == pOrder.BrokerID) && (pTrade.BrokerOrderSeq == pOrder.BrokerOrderSeq) &&
                    (pTrade.ExchangeID == pOrder.ExchangeID) && (pTrade.TraderID == pOrder.TraderID) &&
                    (pTrade.OrderLocalID == pOrder.OrderLocalID) && (pTrade.OrderSysID == pOrder.OrderSysID));
        }

        /// <summary>
        /// 判断报单是否正在被成交
        /// </summary>
        /// <param name="pOrder"></param>
        /// <returns></returns>
        private bool IsTradingOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus != EnumOrderStatusType.PartTradedNotQueueing) && (pOrder.OrderStatus != EnumOrderStatusType.Canceled) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.AllTraded) && (pOrder.OrderStatus != EnumOrderStatusType.NoTradeNotQueueing));
            //return pOrder.OrderStatus != EnumOrderStatusType.Canceled;
        }

        /// <summary>
        /// 判断是否是错误报单
        /// </summary>
        /// <param name="pOrder"></param>
        /// <returns></returns>
        private bool IsErrorOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus == EnumOrderStatusType.Unknown) || pOrder.OrderSysID == "");
        }
        #endregion

        #region 询价
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
                ForQuoteRef++;
                req.ForQuoteRef = ForQuoteRef.ToString();
                iRet = trader.ReqForQuoteInsert(req, ++RequestID);
                return iRet;
            }
        }

        /// <summary>
        /// 询价录入请求响应
        /// </summary>
        /// <param name="pInputForQuote"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspForQuoteInsert(ThostFtdcInputForQuoteField pInputForQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        /// <summary>
        /// 请求查询询价响应
        /// </summary>
        /// <param name="pForQuote"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspQryForQuote(ThostFtdcForQuoteField pForQuote, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        /// <summary>
        /// 询价录入错误回报
        /// </summary>
        /// <param name="pInputForQuote"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnForQuoteInsert(ThostFtdcInputForQuoteField pInputForQuote, ThostFtdcRspInfoField pRspInfo)
        {

        }
        #endregion

        #region 行权方法
        /// <summary>
        /// 执行宣告操作请求响应
        /// </summary>
        /// <param name="pExecOrder"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspQryExecOrder(ThostFtdcExecOrderField pExecOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        /// <summary>
        /// 执行宣告通知
        /// </summary>
        /// <param name="pExecOrder"></param>
        void OnRtnExecOrder(ThostFtdcExecOrderField pExecOrder)
        {

        }

        /// <summary>
        /// 执行宣告录入错误回报
        /// </summary>
        /// <param name="pInputExecOrder"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnExecOrderInsert(ThostFtdcInputExecOrderField pInputExecOrder, ThostFtdcRspInfoField pRspInfo)
        {

        }

        /// <summary>
        /// 执行宣告操作错误回报
        /// </summary>
        /// <param name="pExecOrderAction"></param>
        /// <param name="pRspInfo"></param>
        void OnErrRtnExecOrderAction(ThostFtdcExecOrderActionField pExecOrderAction, ThostFtdcRspInfoField pRspInfo)
        {

        }

        /// <summary>
        /// 执行宣告录入请求响应
        /// </summary>
        /// <param name="pInputExecOrder"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspExecOrderInsert(ThostFtdcInputExecOrderField pInputExecOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }

        /// <summary>
        /// 执行宣告操作请求响应
        /// </summary>
        /// <param name="pInputExecOrderAction"></param>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspExecOrderAction(ThostFtdcInputExecOrderActionField pInputExecOrderAction, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {

        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (trader != null)
            {
                trader.Release();
                trader = null;
            }
        }

    }//class
}
