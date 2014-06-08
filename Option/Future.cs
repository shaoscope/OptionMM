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
                if (position.InstrumentID == instrumentID && position.PosiDirection == EnumPosiDirectionType.Long)
                {
                    this.longPosition = position;
                }
                else if (position.InstrumentID == instrumentID && position.PosiDirection == EnumPosiDirectionType.Short)
                {
                    this.shortPosition = position;
                }
            }
            MDManager.MD.SubscribeMarketData(new string[] { InstrumentID });
            MDManager.MD.OnTick += MD_OnTick;
            TDManager.TD.OnCanceled += TD_OnCanceled;
            TDManager.TD.OnTrading += TD_OnTrading;
        }

        private void TD_OnTrading(ThostFtdcOrderField pOrder)
        {
            if (pOrder.InstrumentID == this.InstrumentID)
            {
                if (this.previousOrder == null)
                {
                    if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                    {
                        this.longPosition.Position += pOrder.VolumeTraded;
                    }
                    else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                    {
                        this.shortPosition.Position -= pOrder.VolumeTraded;
                    }
                    else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                    {
                        this.shortPosition.Position += pOrder.VolumeTraded;
                    }
                    else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                    {
                        this.longPosition.Position -= pOrder.VolumeTraded;
                    }
                    this.previousOrder = pOrder;
                }
                else
                {
                    if (pOrder.OrderRef == this.previousOrder.OrderRef)
                    {
                        if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.longPosition.Position += pOrder.VolumeTraded - this.previousOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.shortPosition.Position -= pOrder.VolumeTraded - this.previousOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.shortPosition.Position += pOrder.VolumeTraded - this.previousOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.longPosition.Position -= pOrder.VolumeTraded - this.previousOrder.VolumeTraded;
                        }
                        this.previousOrder = pOrder;
                    }
                    else
                    {
                        if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.longPosition.Position += pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.shortPosition.Position -= pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.shortPosition.Position += pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.longPosition.Position -= pOrder.VolumeTraded;
                        }
                        this.previousOrder = pOrder;
                    }
                }
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

        private void TD_OnCanceled(ThostFtdcOrderField pOrder)
        {
            throw new NotImplementedException();
        }

        private void MD_OnTick(ThostFtdcDepthMarketDataField md)
        {
            if (this.InstrumentID == md.InstrumentID)
            {
                this.LastMarket = md;
            }
        }

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
            int hedgingVolume = (int)(hedgeVolume > 0 ? Math.Floor(hedgeVolume) : Math.Ceiling(hedgeVolume));
            int adjustVolume = hedgingVolume - (this.longPosition.Position - this.shortPosition.Position);
            if (adjustVolume >= 3)
            {
                TDManager.TD.Buy(instrumentID, hedgingVolume, this.lastMarket.AskPrice1);
            }
            else if (adjustVolume <= -3)
            {
                TDManager.TD.SellShort(instrumentID, Math.Abs(hedgingVolume), this.lastMarket.BidPrice1);
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
