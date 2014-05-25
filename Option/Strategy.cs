﻿using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OptionMM
{
    class Strategy : DataGridViewRow, IStrategyBase
    {
        //期权合约
        private Option option;

        /// <summary>
        /// 获取或者设置期权合约
        /// </summary>
        public Option Option
        {
            get { return this.option; }
            set { this.option = value; }
        }

        //期货合约
        private Future future;

        /// <summary>
        /// 获取或者设置期权对应的期货合约
        /// </summary>
        public Future Future
        {
            get { return this.future; }
            set { this.future = value; }
        }

        public double optionPositionThreshold;  //开仓阈值

        public double minOptionOpenLots;    //最小开仓数

        public double maxOptionOpenLots;    //最大开仓数

        public Strategy()
        {
            this.Tag = this;
            option = new Option();
            future = new Future();
        }

        //所属面板控件
        private OptionPanel optionPanel;

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(OptionPanel optionPanel)
        {
            this.optionPanel = optionPanel;
        }

        //刷新面板定时器
        private System.Threading.Timer panelRefreshTimer;

        //撤买单定时器
        private System.Threading.Timer cancelLongOrderTimer;

        //撤卖单定时器
        private System.Threading.Timer cancelShortOrderTimer;

        //撤平买单定时器
        private System.Threading.Timer cancelCloseLongOrderTimer;

        //撤平卖单定时器
        private System.Threading.Timer cancelCloseShortOrderTimer;


        //买单是否成交标记位
        private bool canPlaceLongOrder = true;

        //卖单是否成交标记位
        private bool canPlaceShortOrder = true;

        //撤单时间(秒)
        private double cancelOrderDuration = 30;

        /// <summary>
        /// 策略是否运行标记位
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// 获取或者设置策略是否在运行的标记位
        /// </summary>
        public bool IsRunning
        {
            set { this.isRunning = value; }
            get { return this.isRunning; }
        }

        /// <summary>
        /// 运行策略
        /// </summary>
        public void Start()
        {
            //启动策略
            isRunning = true;
            MDManager.MD.SubscribeMarketData(new string[] { this.option.InstrumentID, this.future.InstrumentID });
            MDManager.MD.OnTick += MD_OnTick;
            TDManager.TD.OnCanceled += TD_OnCanceled;
            TDManager.TD.OnTraded += TD_OnTraded;
            TDManager.TD.OnTrading += TD_OnTrading;
            
            this.panelRefreshTimer = new System.Threading.Timer(this.panelRefreshCallback, null, 1000, 1000);
        }

        void TD_OnTrading(ThostFtdcOrderField pOrder)
        {
            if(pOrder.InstrumentID == this.option.InstrumentID)
            {
                if (this.option.PlaceLongOptionOrderRef == pOrder.OrderRef)
                {
                    if (pOrder.VolumeTraded == 0 && this.option.LongOptionOrder == null)
                    {
                        this.option.LongOptionOrder = pOrder;
                        this.cancelLongOrderTimer = new System.Threading.Timer(this.cancelLongOrderCallBack, null, (long)cancelOrderDuration * 1000, Timeout.Infinite);
                    }
                    else
                    {
                        if(this.option.LongOptionOrder == null)
                        {
                            this.option.LongPositionVolume = pOrder.VolumeTraded;
                        }
                        else
                        {
                            int tradedVolume = pOrder.VolumeTraded - this.option.LongOptionOrder.VolumeTraded;
                            TDManager.TD.Buy(this.option.InstrumentID, tradedVolume, this.option.MMQuotation.BidPrice);
                            this.option.LongPositionVolume += tradedVolume;
                            this.option.LongOptionOrder = pOrder;
                        }
                    }
                }
                else if (this.option.PlaceShortOptionOrderRef == pOrder.OrderRef)
                {
                    if (pOrder.VolumeTraded == 0 && this.option.ShortOptionOrder == null)
                    {
                        this.option.ShortOptionOrder = pOrder;
                        this.cancelShortOrderTimer = new System.Threading.Timer(this.cancelShortOrderCallBack, null, (long)cancelOrderDuration * 1000, Timeout.Infinite);
                    }
                    else
                    {
                        if (this.option.ShortOptionOrder == null)
                        {
                            this.option.ShortPositionVolume = pOrder.VolumeTraded;
                        }
                        else
                        {
                            int tradedVolume = pOrder.VolumeTraded - this.option.ShortOptionOrder.VolumeTraded;
                            TDManager.TD.SellShort(this.option.InstrumentID, tradedVolume, this.option.MMQuotation.AskPrice);
                            this.option.ShortPositionVolume += tradedVolume;
                            this.option.ShortOptionOrder = pOrder;
                        }
                    }
                }
                else if(this.option.CloseLongOptionOrderRef == pOrder.OrderRef)
                {
                    if(pOrder.VolumeTraded == 0 && this.option.CloseLongOptionOrder == null)
                    {
                        this.option.CloseLongOptionOrder = pOrder;
                        this.cancelCloseLongOrderTimer = new System.Threading.Timer(this.cancelCloseLongOrderCallBack, null, (long)cancelOrderDuration * 1000, Timeout.Infinite);
                    }
                    else
                    {

                    }
                }
                else if(this.option.CloseShortOptionOrderRef == pOrder.OrderRef)
                {
                    if(pOrder.VolumeTraded == 0 && this.option.CloseShortOptionOrder == null)
                    {
                        this.option.CloseShortOptionOrder = pOrder;
                        this.cancelCloseShortOrderTimer = new System.Threading.Timer(this.cancelCloseShortOrderCallBack, null, (long)cancelOrderDuration * 1000, Timeout.Infinite);
                    }
                }
            }
            else if(pOrder.InstrumentID == this.future.InstrumentID)
            {

            }
        }

        void TD_OnTraded(ThostFtdcOrderField pOrder)
        {
            
        }

        void TD_OnCanceled(ThostFtdcOrderField pOrder)
        {
            if(pOrder.InstrumentID == this.option.InstrumentID)
            {
                ////CurrentOptionOrder = order;
                //if (pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open && pOrder.Direction == EnumDirectionType.Buy)
                //{
                //    //买开
                //    this.option.LongInputLots -= pOrder.VolumeTotal;
                //}
                //else if (pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open && pOrder.Direction == EnumDirectionType.Sell)
                //{
                //    //卖开
                //    this.option.ShortInputLots -= pOrder.VolumeTotal;
                //}
                //else if (pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.CloseToday && pOrder.Direction == EnumDirectionType.Buy)
                //{
                //    //买平
                //    this.option.ShortInputLots += pOrder.VolumeTotal;
                //}
                //else if (pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.CloseToday && pOrder.Direction == EnumDirectionType.Sell)
                //{
                //    //买平
                //    this.option.LongInputLots += pOrder.VolumeTotal;
                //}
            }
            else if(pOrder.InstrumentID == this.future.InstrumentID)
            {

            }
        }

        private void MD_OnTick(ThostFtdcDepthMarketDataField md)
        {
            if (this.future.InstrumentID == md.InstrumentID)
            {
                this.future.LastMarket = md;
                //计算期权理论价格
                OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(this.option.OptionType,
                    this.future.LastMarket.LastPrice, this.option.StrikePrice, GlobalValues.InterestRate, GlobalValues.Volatility, GlobalValues.DaysToMaturity);
                OptionValue optionValue = OptionPricingModel.EuropeanBS(optionPricingModelParams);
                this.option.TheoreticalPrice = optionValue.Price;
                this.option.Delta = optionValue.Delta;
                this.option.Gamma = optionValue.Gamma;
                this.option.Vega = optionValue.Vega;
                this.option.Theta = optionValue.Theta;
                this.option.Rho = optionValue.Rho;
            }
            else if (this.option.InstrumentID == md.InstrumentID)
            {
                this.option.LastMarket = md;
                //计算出的做市价格
                double MMBid = 0;
                double MMAsk = 0;
                if (this.option.TheoreticalPrice - this.option.LastMarket.LastPrice > this.optionPositionThreshold)
                {
                    ////计算开仓手数 股指手数定义 期权手数 = 1/隐含波动率 向上取整 * 3, 买入期权，//卖出股指。
                    //报10手期权
                    MMBid = Math.Round(this.option.LastMarket.BidPrice1 - 0.1, 2);
                    MMAsk = MMPrice.GetAskPriceThisMonth(MMBid);
                }
                else if (this.option.LastMarket.LastPrice - this.option.TheoreticalPrice > this.optionPositionThreshold)
                {
                    MMAsk = Math.Round(this.option.LastMarket.AskPrice1 + 0.1, 2);
                    MMBid = MMPrice.GetBidPriceThisMonth(ref MMAsk);
                }
                else if (this.option.TheoreticalPrice > this.option.LastMarket.LastPrice)
                {
                    MMBid = Math.Round(this.option.TheoreticalPrice - this.optionPositionThreshold);
                    MMAsk = MMPrice.GetAskPriceThisMonth(MMBid);
                }
                else if (this.option.TheoreticalPrice > this.option.LastMarket.LastPrice)
                {
                    MMAsk = Math.Round(this.option.TheoreticalPrice + this.optionPositionThreshold);
                    MMBid = MMPrice.GetBidPriceThisMonth(ref MMAsk);
                }
                else
                {
                    
                }
                this.option.MMQuotation.AskPrice = MMAsk;
                this.option.MMQuotation.BidPrice = MMBid;
                //策略逻辑
                if (this.canPlaceLongOrder == true && this.canPlaceShortOrder == true)
                {
                    this.canPlaceLongOrder = false;
                    this.canPlaceShortOrder = false;
                    this.InitPlaceOrder();
                }
            }
        }

        /// <summary>
        /// 做市策略逻辑
        /// </summary>
        private void InitPlaceOrder()
        {
            this.option.MMQuotation.AskLots = 10;
            this.option.MMQuotation.BidLots = 10;
            //挂买单 先平卖单
            foreach(ThostFtdcInvestorPositionField optionPosition in MainForm.PositionList)
            {
                if(this.option.InstrumentID == optionPosition.InstrumentID)
                {
                    //平卖单逻辑 
                    if(optionPosition.PosiDirection == EnumPosiDirectionType.Short)
                    {
                        if (optionPosition.Position != 0)
                        {
                            int shortSellVlume = optionPosition.Position >=
                                this.option.MMQuotation.BidLots ? this.option.MMQuotation.BidLots : optionPosition.Position;
                            this.option.CloseShortOptionOrderRef = TDManager.TD.Sell(this.option.InstrumentID, shortSellVlume, this.option.MMQuotation.AskPrice);
                        }
                        else
                        {
                            //this.option.PlaceLongOptionOrderRef = TDManager.TD.Buy(this.option.InstrumentID, this.option.MMQuotation.BidLots, this.option.MMQuotation.BidPrice);
                        }
                    }
                }
            }
            //挂卖单 先平买单
            foreach (ThostFtdcInvestorPositionField optionPosition in MainForm.PositionList)
            {
                if (this.option.InstrumentID == optionPosition.InstrumentID)
                {
                    //平买单逻辑
                    if (optionPosition.PosiDirection == EnumPosiDirectionType.Long)
                    {
                        if (optionPosition.Position != 0)
                        {
                            int longCallVlume = optionPosition.Position >=
                                this.option.MMQuotation.AskLots ? this.option.MMQuotation.AskLots : optionPosition.Position;
                            this.option.CloseLongOptionOrderRef = TDManager.TD.BuyToCover(this.option.InstrumentID, longCallVlume, this.option.MMQuotation.BidPrice);
                        }
                        else
                        {
                            //this.option.PlaceShortOptionOrderRef = TDManager.TD.SellShort(this.option.InstrumentID, this.option.MMQuotation.AskLots, this.option.MMQuotation.AskPrice);
                        }
                    }
                }
            }


            //this.option.BidOptionOrderRef = TDManager.TD.Buy(this.option.InstrumentID, this.option.MMQuotation.BidLots, this.option.MMQuotation.BidPrice);
            //this.option.AskOptionOrderRef = TDManager.TD.SellShort(this.option.InstrumentID, this.option.MMQuotation.AskLots, this.option.MMQuotation.AskPrice);
        }

        /// <summary>
        /// 刷新定时器回调
        /// </summary>
        private void panelRefreshCallback(object state)
        {
            try
            {
                lock (new object())
                {
                    if (optionPanel.InvokeRequired)
                    {
                        optionPanel.BeginInvoke(new MethodInvoker(this.RefreshDataRow));
                    }
                    else
                    {
                        this.RefreshDataRow();
                    }
                }
            }
            catch
            { 

            }
        }

        //刷新策略行
        public void RefreshDataRow()
        {
            this.Cells["cDelta"].Value = this.option.Delta;
            this.Cells["cTheroricalPrice"].Value = this.option.TheoreticalPrice;
            this.Cells["cRealPrice"].Value = this.option.LastMarket == null ? 0 : this.option.LastMarket.LastPrice;
            this.Cells["cAskPrice"].Value = this.option.LastMarket == null ? 0 : this.option.LastMarket.AskPrice1;
            this.Cells["cBidPrice"].Value = this.option.LastMarket == null ? 0 : this.option.LastMarket.BidPrice1;
            this.Cells["cRunningStatus"].Value = this.isRunning == true ? "正在运行" : "停止";
        }

        /// <summary>
        /// 撤买单回调
        /// </summary>
        /// <param name="state"></param>
        private void cancelLongOrderCallBack(object state)
        {
            if (this.option.PlaceLongOptionOrderRef != null)
            {
                TDManager.TD.CancelOrder(this.option.LongOptionOrder);
                canPlaceLongOrder = true;
                this.option.LongOptionOrder = null;
            }
        }

        /// <summary>
        /// 撤卖单回调
        /// </summary>
        /// <param name="state"></param>
        private void cancelShortOrderCallBack(object state)
        {
            if (this.option.PlaceShortOptionOrderRef != null)
            {
                TDManager.TD.CancelOrder(this.option.ShortOptionOrder);
                canPlaceShortOrder = true;
                this.option.ShortOptionOrder = null;
            }
        }

        private void cancelCloseShortOrderCallBack(object state)
        {
            if(this.option.CloseShortOptionOrderRef != null)
            {
                TDManager.TD.CancelOrder(this.option.CloseShortOptionOrder);
                canPlaceShortOrder = true;
                this.option.CloseShortOptionOrder = null;
            }
        }

        private void cancelCloseLongOrderCallBack(object state)
        {
            if(this.option.CloseLongOptionOrderRef != null)
            {
                TDManager.TD.CancelOrder(this.option.CloseLongOptionOrder);
                canPlaceLongOrder = true;
                this.option.CloseLongOptionOrder = null;
            }
        }

        /// <summary>
        /// 停止策略
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;
            this.RefreshDataRow();
            this.panelRefreshTimer.Dispose();
            //MDManager.MD.UnSubMD(new string[] { this.future.InstrumentID, this.option.InstrumentID });
            MDManager.MD.OnTick -= MD_OnTick;
            TDManager.TD.OnCanceled -= TD_OnCanceled;
            TDManager.TD.OnTraded -= TD_OnTraded;
            TDManager.TD.OnTrading -= TD_OnTrading;
        }
    }
}