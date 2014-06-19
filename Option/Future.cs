using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionMM
{
    class Future : Position
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Future(string instrumentID)
        {
            this.instrumentID = instrumentID;
            foreach (ThostFtdcInvestorPositionField position in MainForm.PositionList)
            {
                if ((position.InstrumentID == "IF1406" || position.InstrumentID == "IF1407" || position.InstrumentID == "IF1409" || position.InstrumentID == "IF1412") && position.PosiDirection == EnumPosiDirectionType.Long)
                {
                    this.longPosition.Position += position.Position;
                }
                else if ((position.InstrumentID == "IF1406" || position.InstrumentID == "IF1407" || position.InstrumentID == "IF1409" || position.InstrumentID == "IF1412") && position.PosiDirection == EnumPosiDirectionType.Short)
                {
                    this.shortPosition.Position += position.Position;
                }
            }
            MDManager.MD.SubscribeMarketData(new string[] { InstrumentID });
            MDManager.MD.OnTick += MD_OnTick;
            TDManager.TD.OnCanceled += TD_OnCanceled;
            TDManager.TD.OnTrading += TD_OnTrading;
        }

        /// <summary>
        /// 对报单回报的处理
        /// </summary>
        /// <param name="pOrder"></param>
        private void TD_OnTrading(ThostFtdcOrderField pOrder)
        {
            if (pOrder.InstrumentID == "IF1406" || pOrder.InstrumentID == "IF1407" || pOrder.InstrumentID == "IF1409" || pOrder.InstrumentID == "IF1412")
            {
                if (pOrder.VolumeTraded != 0)
                {
                    if (!this.tradingOrderDictionary.ContainsKey(pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID))
                    {
                        if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            if (this.longPosition.Position + pOrder.VolumeTraded == 0)
                            {
                                this.longPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.longPosition.PositionCost = (this.longPosition.PositionCost * this.longPosition.Position +
                                    pOrder.LimitPrice * pOrder.VolumeTraded) / (this.longPosition.Position + pOrder.VolumeTraded);
                            }
                            this.longPosition.Position += pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 != EnumOffsetFlagType.Open)
                        {
                            if (this.shortPosition.Position + pOrder.VolumeTraded == 0)
                            {
                                this.shortPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.shortPosition.PositionCost = (this.shortPosition.PositionCost * this.shortPosition.Position +
                                    pOrder.LimitPrice * pOrder.VolumeTraded) / (this.shortPosition.Position + pOrder.VolumeTraded);
                            }
                            this.shortPosition.Position -= pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            if (this.shortPosition.Position + pOrder.VolumeTraded == 0)
                            {
                                this.shortPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.shortPosition.PositionCost = (this.shortPosition.PositionCost * this.shortPosition.Position +
                                    pOrder.LimitPrice * pOrder.VolumeTraded) / (this.shortPosition.Position + pOrder.VolumeTraded);
                            }
                            this.shortPosition.Position += pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 != EnumOffsetFlagType.Open)
                        {
                            if (this.longPosition.Position + pOrder.VolumeTraded == 0)
                            {
                                this.longPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.longPosition.PositionCost = (this.longPosition.PositionCost * this.longPosition.Position +
                                    pOrder.LimitPrice * pOrder.VolumeTraded) / (this.longPosition.Position + pOrder.VolumeTraded);
                            }
                            this.longPosition.Position -= pOrder.VolumeTraded;
                        }
                        if (pOrder.VolumeTotal != 0)
                        {
                            this.tradingOrderDictionary.Add(pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID, pOrder);
                        }
                    }
                    else
                    {
                        if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            if (this.longPosition.Position - (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded) == 0)
                            {
                                this.longPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.longPosition.PositionCost = (this.longPosition.PositionCost * this.longPosition.Position -
                                    pOrder.LimitPrice * (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded)) / (this.longPosition.Position -
                                    (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded));
                            }
                            this.longPosition.Position += pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 != EnumOffsetFlagType.Open)
                        {
                            if (this.shortPosition.Position - (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded) == 0)
                            {
                                this.shortPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.shortPosition.PositionCost = (this.shortPosition.PositionCost * this.shortPosition.Position -
                                    pOrder.LimitPrice * (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded)) / (this.shortPosition.Position -
                                    (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded));
                            }
                            this.shortPosition.Position -= pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            if (this.shortPosition.Position - (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded) == 0)
                            {
                                this.shortPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.shortPosition.PositionCost = (this.shortPosition.PositionCost * this.shortPosition.Position -
                                    pOrder.LimitPrice * (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded)) / (this.shortPosition.Position -
                                    (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded));
                            }
                            this.shortPosition.Position += pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 != EnumOffsetFlagType.Open)
                        {
                            if (this.longPosition.Position - (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded) == 0)
                            {
                                this.longPosition.PositionCost = 0;
                            }
                            else
                            {
                                this.longPosition.PositionCost = (this.longPosition.PositionCost * this.longPosition.Position -
                                    pOrder.LimitPrice * (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded)) / (this.longPosition.Position -
                                    (pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded));
                            }
                            this.longPosition.Position -= pOrder.VolumeTraded - this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID].VolumeTraded;
                        }
                        if (pOrder.VolumeTotal == 0)
                        {
                            this.tradingOrderDictionary.Remove(pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID);
                        }
                        else
                        {
                            this.tradingOrderDictionary[pOrder.OrderRef + "-" + pOrder.FrontID + "-" + pOrder.SessionID] = pOrder;
                        }
                    }
                }
                else
                {
                    if (this.PlaceLongOptionOrderRef == pOrder.OrderRef)
                    {
                        this.LongOptionOrder = pOrder;
                    }
                    else if (this.PlaceShortOptionOrderRef == pOrder.OrderRef)
                    {
                        this.ShortOptionOrder = pOrder;
                    }
                    else if (this.CloseLongOptionOrderRef == pOrder.OrderRef)
                    {
                        this.CloseLongOptionOrder = pOrder;
                    }
                    else if (this.CloseShortOptionOrderRef == pOrder.OrderRef)
                    {
                        this.CloseShortOptionOrder = pOrder;
                    }
                }
            }
        }

        private void TD_OnCanceled(ThostFtdcOrderField pOrder)
        {
            
        }

        private void MD_OnTick(ThostFtdcDepthMarketDataField md)
        {
            if (this.InstrumentID == md.InstrumentID)
            {
                this.LastMarket = md;
            }
        }

        /// <summary>
        /// Delta中性对冲期货手数
        /// </summary>
        /// <param name="dataTable"></param>
        public void DeltaHedge(DataGridView dataTable)
        {
            this.hedgeVolume = 0;
            foreach (DataGridViewRow dataRow in dataTable.Rows)
            {
                Strategy strategy = (Strategy)dataRow.Tag;
                hedgeVolume += -strategy.Option.OptionValue.Delta * strategy.Option.longPosition.Position / 3;
                hedgeVolume += strategy.Option.OptionValue.Delta * strategy.Option.shortPosition.Position / 3;
            }
        }

        /// <summary>
        /// 进行对冲下单
        /// </summary>
        public void AutoHedge()
        {
            int adjustVolume = (int)hedgeVolume - (this.longPosition.Position - this.shortPosition.Position);
            return;
            if (adjustVolume >= 8)
            {
                adjustVolume = adjustVolume > 50 ? 50 : adjustVolume;
                if (this.shortPosition.Position > 0)
                {
                    if (this.shortPosition.Position > adjustVolume)
                    {
                        TDManager.TD.BuyToCover(this.instrumentID, adjustVolume, this.lastMarket.AskPrice1);
                    }
                    else
                    {
                        TDManager.TD.BuyToCover(this.instrumentID, this.shortPosition.Position, this.lastMarket.AskPrice1);
                        TDManager.TD.Buy(this.instrumentID, adjustVolume - this.shortPosition.Position, this.lastMarket.AskPrice1);
                    }
                }
                else
                {
                    TDManager.TD.Buy(instrumentID, adjustVolume, this.lastMarket.AskPrice1);
                }
            }
            else if (adjustVolume <= -8)
            {
                adjustVolume = Math.Abs(adjustVolume);
                adjustVolume = adjustVolume > 50 ? 50 : adjustVolume;
                if (this.longPosition.Position > 0)
                {
                    if(this.longPosition.Position > adjustVolume)
                    {
                        TDManager.TD.Sell(this.instrumentID, adjustVolume, this.lastMarket.BidPrice1);
                    }
                    else
                    {
                        TDManager.TD.Sell(this.instrumentID, this.longPosition.Position, this.lastMarket.BidPrice1);
                        TDManager.TD.SellShort(this.instrumentID, adjustVolume - this.longPosition.Position, this.lastMarket.BidPrice1);
                    }
                }
                else
                {
                    TDManager.TD.SellShort(instrumentID, adjustVolume, this.lastMarket.BidPrice1);
                }
            }

        }

        /// <summary>
        /// 期货对冲手数
        /// </summary>
        private double hedgeVolume;

        /// <summary>
        /// 获取期货对冲手数
        /// </summary>
        public double HedgeVolume
        {
            get { return this.hedgeVolume; }
        }


        /// <summary>
        /// 期货合约编码
        /// </summary>
        private string instrumentID;

        /// <summary>
        /// 获取或者设置期货合约编码
        /// </summary>
        public string InstrumentID
        {
            get { return this.instrumentID; }
            set { this.instrumentID = value; }
        }



        private Dictionary<string, ThostFtdcOrderField> tradingOrderDictionary = new Dictionary<string, ThostFtdcOrderField>();

        /// <summary>
        /// 合约最后行情
        /// </summary>
        private ThostFtdcDepthMarketDataField lastMarket;

        /// <summary>
        /// 获取或者设置合约最后行情
        /// </summary>
        public ThostFtdcDepthMarketDataField LastMarket
        {
            get { return this.lastMarket; }
            set { this.lastMarket = value; }
        }

 
    }
}
