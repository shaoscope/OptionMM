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
    class SendOrderManager
    {
        Dictionary<string, OrderField> ReInputOrderMap = new Dictionary<string, OrderField>();
        Dictionary<string, OrderField> WaitInputOrderMap = new Dictionary<string, OrderField>();
        object ReInputOrderMapLock = new object();
        object WaitInputOrderMapLock = new object();
        Dictionary<string, OrderField> TradingOrderMap = new Dictionary<string, OrderField>();
        object TradingOrderMapLock = new object();
        Dictionary<string, OrderField> FinishedOrderMap = new Dictionary<string, OrderField>();
        //记录原始，最新单号
        Dictionary<string, string> OrderSignalMap = new Dictionary<string, string>();
        Dictionary<string, OrderField> InputOrderErrorMap = new Dictionary<string, OrderField>();
        int ORDER_REF = 0;
        string BROKER_ID;
        string INVESTOR_ID;

        public int GetReInputOrderCount()
        {
            return ReInputOrderMap.Count;
        }

        public int GetWaitInputOrderCount()
        {
            return WaitInputOrderMap.Count;
        }

        public void Init(string brokerid, string investorid, int orderref)
        {
            BROKER_ID = brokerid;
            INVESTOR_ID = investorid;
            ORDER_REF = orderref;
        }

        object ReqOrderInsertWhenReSendingLock = new object();
        public void ReqOrderInsertWhenReSending(ThostFtdcInputOrderField req)
        {
            lock (ReqOrderInsertWhenReSendingLock)
            {
                DateTime logtime = DateTime.Now;
                OrderField newOrder = new OrderField(req, logtime);
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
        }

        public void ReqOrderInsertWhenNotReSending(ThostFtdcInputOrderField req)
        {
            lock (TradingOrderMapLock)
            {
                DateTime logtime = DateTime.Now;
                OrderField newOrder = new OrderField(req, logtime);
                string newOrderRef = req.OrderRef;
                newOrder.OrderRef = newOrderRef;
                TradingOrderMap.Add(newOrderRef, newOrder);
            }
        }

        public ThostFtdcInputOrderActionField ReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            lock (TradingOrderMapLock)
            {
                DateTime logtime = DateTime.Now;
                string SignalOrderRef = pInputOrderAction.OrderRef;
                if (OrderSignalMap.ContainsKey(SignalOrderRef))
                {
                    SignalOrderRef = OrderSignalMap[SignalOrderRef];
                    pInputOrderAction.OrderRef = SignalOrderRef;
                }
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    TradingOrderMap[SignalOrderRef].Cancel(pInputOrderAction, logtime);
                }
                return pInputOrderAction;
            }
        }

        public void ReReqOrderAction(ThostFtdcInputOrderActionField pInputOrderAction)
        {
            lock (TradingOrderMapLock)
            {
                DateTime logtime = DateTime.Now;
                string SignalOrderRef = pInputOrderAction.OrderRef;
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    TradingOrderMap[SignalOrderRef].Cancel(pInputOrderAction, logtime);
                }
            }
        }

        public bool UpdateTradingOrderMap(ThostFtdcOrderField pOrder)
        {
            lock (TradingOrderMapLock)
            {
                string SignalOrderRef = pOrder.OrderRef;
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    //更新报单状态
                    TradingOrderMap[SignalOrderRef].Order = pOrder;
                    return true;
                }
                else
                    return false;
            }
        }

        public bool ContainsTrading(string SignalOrderRef)
        {
            return TradingOrderMap.ContainsKey(SignalOrderRef);
        }

        public int OrderCanceled(ThostFtdcOrderField pOrder)
        {
            lock (TradingOrderMapLock)
            {
                int iRet = 0;
                DateTime logtime = DateTime.Now;
                string SignalOrderRef = pOrder.OrderRef;
                if (TradingOrderMap[SignalOrderRef].CancelOrder != null)
                {
                    TradingOrderMap[SignalOrderRef].Order = pOrder;
                    FinishedOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);

                    TradingOrderMap.Remove(SignalOrderRef);
                    //return iRet;
                }
                else
                {
                    if (TradingOrderMap[SignalOrderRef].Order == null || TradingOrderMap[SignalOrderRef].Order.OrderStatus == EnumOrderStatusType.Unknown)
                    {
                        //柜台自动撤单，需要重发
                        lock (ReInputOrderMapLock)
                        {
                            TradingOrderMap[SignalOrderRef].Order = pOrder;
                            
                            ReInputOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                            iRet = 1;
                        }
                    }
                    else
                    {
                        //系统外撤单，不处理。
                        iRet = 2;
                    }
                }
                return iRet;
            }
        }

        public void OrderFinished(ThostFtdcOrderField pOrder)
        {
            lock (TradingOrderMapLock)
            {
                string SignalOrderRef = pOrder.OrderRef;
                if (TradingOrderMap.ContainsKey(SignalOrderRef))
                {
                    FinishedOrderMap.Add(SignalOrderRef, TradingOrderMap[SignalOrderRef]);
                    TradingOrderMap.Remove(SignalOrderRef);
                }
            }
        }


        public List<ThostFtdcInputOrderActionField> ReCancelOrder()
        {
            //优先撤单
            lock (TradingOrderMapLock)
            {
                List<ThostFtdcInputOrderActionField> listRet = new List<ThostFtdcInputOrderActionField>();
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
                            }
                        }
                        else if (WaitInputOrderMap.ContainsKey(signal))
                        {
                            lock (WaitInputOrderMapLock)
                            {
                                WaitInputOrderMap.Remove(signal);
                                CanceledOrderSignal.Add(signal);
                                DateTime logtime = DateTime.Now;
                            }
                        }
                        if (TradingOrderMap[signal].CancelTime.Count > 0 && DateTime.Now > TradingOrderMap[signal].CancelTime[TradingOrderMap[signal].CancelTime.Count - 1].AddMilliseconds(500))
                        {
                            //重新撤单
                            listRet.Add(TradingOrderMap[signal].CancelOrder);
                        }
                    }
                }
                foreach (string signal in CanceledOrderSignal)
                {
                    FinishedOrderMap.Add(signal, TradingOrderMap[signal]);
                    TradingOrderMap.Remove(signal);
                }
                return listRet;
            }
        }

        public List<ThostFtdcInputOrderField> ReInputOrder()
        {
            List<ThostFtdcInputOrderField> listRet = new List<ThostFtdcInputOrderField>();
            if (ReInputOrderMap.Count > 0)
            {
                List<OrderField> SendingList = new List<OrderField>();
                lock (ReInputOrderMapLock)
                {
                    if (ReInputOrderMap.Count > 0)
                    {
                        foreach (OrderField pOrder in ReInputOrderMap.Values)
                        {
                            SendingList.Add(pOrder);
                        }
                        ReInputOrderMap.Clear();
                    }
                }
                foreach (OrderField pOrder in SendingList)
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
                                OrderField newOrder = new OrderField(req, logtime);
                                for (int i = 0; i < TradingOrderMap[OldOrderRef].OrigialOrderRef.Count; i++)
                                {
                                    newOrder.OrigialOrderRef.Add(TradingOrderMap[OldOrderRef].OrigialOrderRef[i]);
                                }
                                newOrder.OrigialOrderRef.Add(pOrder.OrderRef);
                                newOrder.OrderRef = newOrderRef;

                                TradingOrderMap.Add(newOrderRef, newOrder);
                                listRet.Add(req);

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
            return listRet;
        }

        public List<ThostFtdcInputOrderField> WaitInputOrder()
        {
            List<ThostFtdcInputOrderField> listRet = new List<ThostFtdcInputOrderField>();
            if (WaitInputOrderMap.Count > 0)
            {
                List<OrderField> SendingList = new List<OrderField>();
                lock (WaitInputOrderMapLock)
                {
                    if (WaitInputOrderMap.Count > 0)
                    {
                        foreach (OrderField pOrder in WaitInputOrderMap.Values)
                        {
                            SendingList.Add(pOrder);
                        }
                        WaitInputOrderMap.Clear();
                    }
                }
                foreach (OrderField pOrder in SendingList)
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
                                OrderField newOrder = new OrderField(req, logtime);
                                newOrder.OrigialOrderRef.Add(pOrder.OrderRef);
                                newOrder.OrderRef = newOrderRef;

                                TradingOrderMap.Add(newOrderRef, newOrder);
                                listRet.Add(req);

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
            return listRet;
        }


        public string GetTradingOrderOrigialOrderRef(string orderRef)
        {
            string strOrderRefRet = orderRef;
            if (TradingOrderMap.ContainsKey(orderRef))
            {
                if (TradingOrderMap[orderRef].OrigialOrderRef.Count > 0)
                {
                    strOrderRefRet = TradingOrderMap[orderRef].OrigialOrderRef[0];
                }
            }
            return strOrderRefRet;
        }

        public string GetFinishedOrderOrderOrigialOrderRef(string orderRef)
        {
            string strOrderRefRet = orderRef;
            if (FinishedOrderMap.ContainsKey(orderRef))
            {
                if (FinishedOrderMap[orderRef].OrigialOrderRef.Count > 0)
                {
                    strOrderRefRet = FinishedOrderMap[orderRef].OrigialOrderRef[0];
                }
            }
            return strOrderRefRet;
        }

        public void AddInputOrderError(ThostFtdcInputOrderField pInputOrder)
        {
            lock (TradingOrderMapLock)
            {
                string SignalOrderRef = pInputOrder.OrderRef;
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


        object InputOrderActionFieldLock = new object();
        public ThostFtdcInputOrderActionField GetNewInputOrderActionField(ThostFtdcOrderField pOrder)
        {
            lock (InputOrderActionFieldLock)
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

                return req;
            }
        }

        object InputOrderFieldLock = new object();
        /// <summary>
        /// 生成新报单
        /// </summary>
        /// <param name="DIRECTION"></param>
        /// <param name="Offset"></param>
        /// <param name="instrumentID"></param>
        /// <param name="lots"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public ThostFtdcInputOrderField GetNewInputOrderField(EnumDirectionType DIRECTION, EnumOffsetFlagType Offset, string instrumentID, int lots, double price)
        {
            lock (InputOrderFieldLock)
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
        }
    }
}
