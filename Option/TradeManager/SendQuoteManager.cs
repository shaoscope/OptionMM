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
    public class SendQuoteManager
    {
        
        Dictionary<string, string> TradingOrderQuoteMap = new Dictionary<string, string>();
        Dictionary<string, string> FinishedOrderQuoteMap = new Dictionary<string, string>();
        object TradingOrderQuoteMapLock = new object();
        public Dictionary<string, QuoteField> TradingQuoteMap = new Dictionary<string, QuoteField>();
        object TradingQuoteMapLock = new object();
        public Dictionary<string, QuoteField> WaitInputQuoteMap = new Dictionary<string, QuoteField>();
        object WaitInputQuoteMapLock = new object();
        public Dictionary<string, QuoteField> FinishedQuoteMap = new Dictionary<string, QuoteField>();
        public Dictionary<string, QuoteField> ReInputQuoteMap = new Dictionary<string, QuoteField>();
        object ReInputQuoteMapLock = new object();
        Dictionary<string, string> QuoteSignalMap = new Dictionary<string, string>();
        string BROKER_ID;
        string INVESTOR_ID;
        int QUOTE_REF = 0;

        public int GetReInputQuoteCount()
        {
            return ReInputQuoteMap.Count;
        }

        public int GetWaitInputQuoteCount()
        {
            return WaitInputQuoteMap.Count;
        }

        public void Init(string brokerid, string investorid)
        {
            BROKER_ID = brokerid;
            INVESTOR_ID = investorid;
        }

        object ReqQuoteInsertWhenReSendingLock = new object();
        public void ReqQuoteInsertWhenReSending(ThostFtdcInputQuoteField req)
        {
            lock (ReqQuoteInsertWhenReSendingLock)
            {
                DateTime logTime = DateTime.Now;
                QuoteField newQuote = new QuoteField(req, logTime);
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
        }

        public void ReqQuoteInsertWhenNotReSending(ThostFtdcInputQuoteField req)
        {
            lock (TradingQuoteMapLock)
            {
                DateTime logTime = DateTime.Now;
                QuoteField newQuote = new QuoteField(req, logTime);
                string SignalQuoteRef = req.QuoteRef;
                newQuote.QuoteRef = SignalQuoteRef;
                TradingQuoteMap.Add(SignalQuoteRef, newQuote);
            }
        }

        public ThostFtdcInputQuoteActionField ReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            lock (TradingQuoteMapLock)
            {
                DateTime logtime = DateTime.Now;
                string SignalQuoteRef = pInputQuoteAction.QuoteRef;
                lock (TradingQuoteMapLock)
                {
                    if (QuoteSignalMap.ContainsKey(SignalQuoteRef))
                    {
                        SignalQuoteRef = QuoteSignalMap[SignalQuoteRef];
                        pInputQuoteAction.QuoteRef = SignalQuoteRef;
                    }
                    if (TradingQuoteMap.ContainsKey(SignalQuoteRef))
                    {
                        TradingQuoteMap[SignalQuoteRef].Cancel(pInputQuoteAction, logtime);
                    }
                }
                return pInputQuoteAction;
            }
        }

        public void ReReqQuoteAction(ThostFtdcInputQuoteActionField pInputQuoteAction)
        {
            lock (TradingQuoteMapLock)
            {
                DateTime logtime = DateTime.Now;
                string SignalQuoteRef = pInputQuoteAction.QuoteRef;
                if (TradingQuoteMap.ContainsKey(SignalQuoteRef))
                {
                    TradingQuoteMap[SignalQuoteRef].Cancel(pInputQuoteAction, logtime);
                }
            }
        }

        public void UpdateTradingQuoteMap(ThostFtdcOrderField pOrder)
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

        public bool ContainsTrading(string SignalOrderRef)
        {
            return TradingQuoteMap.ContainsKey(SignalOrderRef);
        }

        public int OrderCanceled(ThostFtdcOrderField pOrder)
        {
            //双边报价单
            lock (TradingQuoteMapLock)
            {
                int iRet = 0;
                string strQuoteRef = TradingOrderQuoteMap[pOrder.OrderRef];
                FinishedOrderQuoteMap.Add(pOrder.OrderRef, strQuoteRef);
                TradingOrderQuoteMap.Remove(pOrder.OrderRef);
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
                        iRet = 1;   //?
                    }
                    else
                    {
                        iRet = 2;
                    }
                }
                else
                {
                    iRet = 3;
                }
                return iRet;
            }
        }

        public void OrderFinished(ThostFtdcOrderField pOrder)
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

        public void AddTradingOrderQuote(ThostFtdcQuoteField pQuote, List<ThostFtdcOrderField> QuoteOrderList)
        {
            lock (TradingQuoteMapLock)
            {
                if (TradingQuoteMap.ContainsKey(pQuote.QuoteRef))
                {
                    TradingQuoteMap[pQuote.QuoteRef].Quote = pQuote;
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
        }

        public void QuoteTouched(ThostFtdcQuoteField pQuote)
        {
            //流量超限，需要重报
            lock (ReInputQuoteMapLock)
            {
                TradingQuoteMap[pQuote.QuoteRef].Quote = pQuote;
                ReInputQuoteMap.Add(pQuote.QuoteRef, TradingQuoteMap[pQuote.QuoteRef]);
            }
        }

        public void QuoteCanceled(ThostFtdcQuoteField pQuote)
        {
            if (TradingQuoteMap.ContainsKey(pQuote.QuoteRef))
            {
                lock (TradingQuoteMapLock)
                {
                    //完成撤单
                    TradingQuoteMap[pQuote.QuoteRef].Quote = pQuote;
                    lock (TradingQuoteMapLock)
                    {
                        FinishedQuoteMap.Add(pQuote.QuoteRef, TradingQuoteMap[pQuote.QuoteRef]);
                        TradingQuoteMap.Remove(pQuote.QuoteRef);
                    }
                }
            }
        }

        public List<ThostFtdcInputQuoteActionField> ReCancelQuote()
        {
            //优先撤单
            lock (TradingQuoteMapLock)
            {
                List<ThostFtdcInputQuoteActionField> listRet = new List<ThostFtdcInputQuoteActionField>();
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
                            }
                        }
                        else if (WaitInputQuoteMap.ContainsKey(signal))
                        {
                            lock (WaitInputQuoteMapLock)
                            {
                                WaitInputQuoteMap.Remove(signal);
                                CanceledQuoteSignal.Add(signal);
                                DateTime logtime = DateTime.Now;
                            }
                        }
                        if (TradingQuoteMap[signal].CancelTime.Count > 0 && DateTime.Now > TradingQuoteMap[signal].CancelTime[TradingQuoteMap[signal].CancelTime.Count - 1].AddMilliseconds(500))
                        {
                            //重新撤单
                            listRet.Add(TradingQuoteMap[signal].CancelQuote);
                        }
                    }
                }
                foreach (string signal in CanceledQuoteSignal)
                {
                    FinishedQuoteMap.Add(signal, TradingQuoteMap[signal]);
                    TradingQuoteMap.Remove(signal);
                }
                return listRet;
            }
        }

        public List<ThostFtdcInputQuoteField> ReInputQuote()
        {
            List<ThostFtdcInputQuoteField> listRet = new List<ThostFtdcInputQuoteField>();
            List<QuoteField> SendingList = new List<QuoteField>();
            if (ReInputQuoteMap.Count > 0)
            {
                lock (ReInputQuoteMapLock)
                {
                    foreach (QuoteField pQuote in ReInputQuoteMap.Values)
                    {
                        SendingList.Add(pQuote);
                    }
                    ReInputQuoteMap.Clear();
                }
                foreach (QuoteField pQuote in SendingList)
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
                                QuoteField newQuote = new QuoteField(req, logtime);
                                for (int i = 0; i < TradingQuoteMap[OldQuoteRef].OrigialQuoteRef.Count; i++)
                                {
                                    newQuote.OrigialQuoteRef.Add(TradingQuoteMap[OldQuoteRef].OrigialQuoteRef[i]);
                                }
                                newQuote.OrigialQuoteRef.Add(pQuote.QuoteRef);
                                newQuote.QuoteRef = newQuoteRef;

                                TradingQuoteMap.Add(newQuoteRef, newQuote);
                                listRet.Add(req);

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
            return listRet;
        }

        public List<ThostFtdcInputQuoteField> WaitInputQuote()
        {
            List<ThostFtdcInputQuoteField> listRet = new List<ThostFtdcInputQuoteField>();
            List<QuoteField> SendingList = new List<QuoteField>();
            if (WaitInputQuoteMap.Count > 0)
            {
                lock (WaitInputQuoteMapLock)
                {
                    foreach (QuoteField pQuote in WaitInputQuoteMap.Values)
                        {
                            SendingList.Add(pQuote);
                        }
                        WaitInputQuoteMap.Clear();
                }
                foreach (QuoteField pQuote in SendingList)
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
                                QuoteField newQuote = new QuoteField(req, logtime);
                                newQuote.OrigialQuoteRef.Add(pQuote.QuoteRef);
                                newQuote.QuoteRef = newQuoteRef;

                                TradingQuoteMap.Add(newQuoteRef, newQuote);
                                listRet.Add(req);

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
            return listRet;
        }

        private bool IsQuoteFinish(QuoteField qoute)
        {
            bool bRet = false;
            if (!IsTradingOrder(qoute.AskOrderField) && !IsTradingOrder(qoute.BidOrderField))
                bRet = true;
            return bRet;
        }

        private bool IsTradingOrder(ThostFtdcOrderField pOrder)
        {
            return ((pOrder.OrderStatus != EnumOrderStatusType.PartTradedNotQueueing) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.Canceled) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.AllTraded) &&
                    (pOrder.OrderStatus != EnumOrderStatusType.NoTradeNotQueueing));
            //return pOrder.OrderStatus != EnumOrderStatusType.Canceled;
        }

        object InputQuoteFieldLock = new object();
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
        public ThostFtdcInputQuoteField GetNewInputQuoteField(string instrumentID, EnumOffsetFlagType AskOffset, int Asklots, double Askprice,
            EnumOffsetFlagType BidOffset, int Bidlots, double Bidprice)
        {
            lock (InputQuoteFieldLock)
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
        }

        object InputQuoteActionFieldLock = new object();
        public ThostFtdcInputQuoteActionField GetNewInputQuoteActionField(ThostFtdcQuoteField pQuote)
        {
            lock (InputQuoteActionFieldLock)
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
        }

        public ThostFtdcInputQuoteActionField GetNewInputQuoteActionField(string QuoteRef, string InstrumentID, int FRONT_ID, int SESSION_ID)
        {
            lock (InputQuoteActionFieldLock)
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
        }
    }
}
