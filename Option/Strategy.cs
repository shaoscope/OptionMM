using CTP;
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

        /// <summary>
        /// 上一次下单时间
        /// </summary>
        private DateTime lastUpdateDateTime = new DateTime();

        /// <summary>
        /// 是否是做市合约
        /// </summary>
        private bool isMarketMakingContract = false;

        private System.Threading.Timer panelRefreshTimer;

        /// <summary>
        /// 获取或者设置本合约是否是做市合约
        /// </summary>
        public bool IsMarketMakingContract
        {
            get { return this.isMarketMakingContract; }
            set { this.isMarketMakingContract = value; }
        }

        public Strategy()
        {
            this.Tag = this;
            option = new Option();
        }

        /// <summary>
        /// 配置策略
        /// </summary>
        public void Configuration()
        {
            MDManager.MD.SubscribeMarketData(new string[] { this.option.InstrumentID });
            MDManager.MD.OnTick += MD_OnTick;
            MDManager.MD.OnForQuote += MD_OnForQuote;
            TDManager.TD.OnCanceled += TD_OnCanceled;
            TDManager.TD.OnTraded += TD_OnTraded;
            TDManager.TD.OnTrading += TD_OnTrading;
            TDManager.TD.OnCancelAction += TD_OnCancelAction;
            //刷新面板定时器
            this.panelRefreshTimer = new System.Threading.Timer(this.panelRefreshCallback, null, 1000, 1000);
        }

        private void TD_OnCancelAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID == 26 || pRspInfo.ErrorID == 25 || pRspInfo.ErrorID == 24)
            {
                //不处理
            }
            else
            {
                int cancelOrderResult = -1;
                do
                {
                    cancelOrderResult = TDManager.TD.CancelOrder(pInputOrderAction);
                }
                while (cancelOrderResult != 0);
            }
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

        /// <summary>
        /// 撤单再下单的时间间隔(秒)
        /// </summary>
        private int ReplaceOrderDuration = 11;

        /// <summary>
        /// 策略是否运行标记位
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// 是否有询价单标记位
        /// </summary>
        private bool hasForQuote = false;

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
            if (this.isMarketMakingContract)
            {
                Thread.Sleep(300);
                isRunning = true;
            }
        }

        /// <summary>
        /// 有报单成交时的处理
        /// </summary>
        /// <param name="pOrder"></param>
        void TD_OnTrading(ThostFtdcOrderField pOrder)
        {            
            if(pOrder.InstrumentID == this.option.InstrumentID)
            {
                if(this.option.previousOrder == null)
                {
                    if(pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                    {
                        this.option.longPosition.Position += pOrder.VolumeTraded;
                    }
                    else if(pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                    {
                        this.option.shortPosition.Position -= pOrder.VolumeTraded;
                    }
                    else if(pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                    {
                        this.option.shortPosition.Position += pOrder.VolumeTraded;
                    }
                    else if(pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                    {
                        this.option.longPosition.Position -= pOrder.VolumeTraded;
                    }
                    this.option.previousOrder = pOrder;
                }
                else
                {
                    if(pOrder.OrderRef == this.option.previousOrder.OrderRef)
                    {
                        if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.option.longPosition.Position += pOrder.VolumeTraded - this.option.previousOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.option.shortPosition.Position -= pOrder.VolumeTraded - this.option.previousOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.option.shortPosition.Position += pOrder.VolumeTraded - this.option.previousOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.option.longPosition.Position -= pOrder.VolumeTraded - this.option.previousOrder.VolumeTraded;
                        }
                        this.option.previousOrder = pOrder;
                    }
                    else
                    {
                        if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.option.longPosition.Position += pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.option.shortPosition.Position -= pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Open)
                        {
                            this.option.shortPosition.Position += pOrder.VolumeTraded;
                        }
                        else if (pOrder.Direction == EnumDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumOffsetFlagType.Close)
                        {
                            this.option.longPosition.Position -= pOrder.VolumeTraded;
                        }
                        this.option.previousOrder = pOrder;
                    }
                }
                if (this.option.PlaceLongOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.LongOptionOrder = pOrder;                    
                }
                else if (this.option.PlaceShortOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.ShortOptionOrder = pOrder;
                }
                else if (this.option.CloseLongOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.CloseLongOptionOrder = pOrder;
                }
                else if (this.option.CloseShortOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.CloseShortOptionOrder = pOrder;
                }
            }
        }

        void TD_OnTraded(ThostFtdcOrderField pOrder)
        {
            
        }

        /// <summary>
        /// 撤单成功的处理
        /// </summary>
        /// <param name="pOrder"></param>
        void TD_OnCanceled(ThostFtdcOrderField pOrder)
        {
            if(pOrder.InstrumentID == this.option.InstrumentID)
            {
                if (this.option.PlaceLongOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.PlaceLongOptionOrderRef = null;
                }
                else if (this.option.PlaceShortOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.PlaceShortOptionOrderRef = null;
                }
                else if (this.option.CloseLongOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.CloseLongOptionOrderRef = null;
                }
                else if (this.option.CloseShortOptionOrderRef == pOrder.OrderRef)
                {
                    this.option.CloseShortOptionOrderRef = null;
                }
            }
        }

        /// <summary>
        /// 询价行情到来时的响应
        /// </summary>
        /// <param name="pForQuoteRsp"></param>
        private void MD_OnForQuote(ThostFtdcForQuoteRspField pForQuoteRsp)
        {
            if (this.option.InstrumentID == pForQuoteRsp.InstrumentID)
            {
                hasForQuote = true;
                this.optionPanel.BeginInvoke(new Action<ThostFtdcForQuoteRspField>(this.UpdateForQupteLabel), pForQuoteRsp);
            }
        }

        /// <summary>
        /// 更新询价单标签
        /// </summary>
        /// <param name="pForQuoteRsp"></param>
        private void UpdateForQupteLabel(ThostFtdcForQuoteRspField pForQuoteRsp)
        {
            MainForm.Instance.forQuoteInfoLabel.Text = "询价单：" + pForQuoteRsp.InstrumentID + "-" + pForQuoteRsp.ForQuoteTime;
        }

        /// <summary>
        /// 有行情到来时的处理逻辑
        /// </summary>
        /// <param name="md"></param>
        private void MD_OnTick(ThostFtdcDepthMarketDataField md)
        {
            if (this.option.InstrumentID == md.InstrumentID)
            {
                this.option.LastMarket = md;
                //计算期权理论价格
                OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(this.option.OptionType,
                    MainForm.Future.LastMarket.LastPrice, this.option.StrikePrice, GlobalValues.InterestRate, GlobalValues.Volatility,
                    StaticFunction.GetDaysToMaturity(this.option.InstrumentID));
                this.option.OptionValue = OptionPricingModel.EuropeanBS(optionPricingModelParams);
                ////计算隐含波动率
                if (MainForm.Future.LastMarket != null)
                {
                    this.option.ImpliedVolatility = StaticFunction.CalculateImpliedVolatility(MainForm.Future.LastMarket.LastPrice, this.option.StrikePrice,
                        StaticFunction.GetDaysToMaturity(this.option.InstrumentID), GlobalValues.InterestRate, md.LastPrice, this.option.OptionType);

                    string[] updateDateTimeString = md.UpdateTime.Split(':');
                    DateTime updateDateTime = new DateTime();
                    updateDateTime = updateDateTime.AddHours(double.Parse(updateDateTimeString[0]));
                    updateDateTime = updateDateTime.AddMinutes(double.Parse(updateDateTimeString[1]));
                    updateDateTime = updateDateTime.AddSeconds(double.Parse(updateDateTimeString[2]));
                    if ((updateDateTime - lastUpdateDateTime).TotalSeconds > this.ReplaceOrderDuration)
                    {
                        //撤单
                        this.CancelOrder();
                        //计算报价
                        double[] quote = this.CalculateQuote();
                        if (isRunning)
                        {
                            this.lastUpdateDateTime = updateDateTime;
                            this.PlaceOrder(quote);
                        }
                        else if (hasForQuote)
                        {
                            this.lastUpdateDateTime = updateDateTime;
                            hasForQuote = false;
                            this.PlaceOrder(quote);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 计算报价
        /// </summary>
        /// <returns></returns>
        private double[] CalculateQuote()
        {
            double[] quote = new double[] { 0, 0 };
            double bidQuote = 0;
            double askQuote = 0;
            #region 计算报单价格--最大区间计算法
            if (this.option.InstrumentID.Contains("1406"))
            {
                if (this.option.LastMarket.LastPrice < 10)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 0.3;
                    askQuote = this.option.LastMarket.LastPrice + 0.2;
                }
                else if (this.option.LastMarket.LastPrice < 20)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 0.5;
                    askQuote = this.option.LastMarket.LastPrice + 0.5;
                }
                else if (this.option.LastMarket.LastPrice < 50)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 1.3;
                    askQuote = this.option.LastMarket.LastPrice + 1.2;
                }
                else if (this.option.LastMarket.LastPrice < 100)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 2.5;
                    askQuote = this.option.LastMarket.LastPrice + 2.5;
                }
                else if (this.option.LastMarket.LastPrice < 250)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 4;
                    askQuote = this.option.LastMarket.LastPrice + 4;
                }
                else
                {
                    bidQuote = this.option.LastMarket.LastPrice - 7.5;
                    askQuote = this.option.LastMarket.LastPrice + 7.5;
                }
            }
            else
            {
                if (this.option.LastMarket.LastPrice < 10)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 0.5;
                    askQuote = this.option.LastMarket.LastPrice + 0.5;
                }
                else if (this.option.LastMarket.LastPrice < 20)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 1;
                    askQuote = this.option.LastMarket.LastPrice + 1;
                }
                else if (this.option.LastMarket.LastPrice < 50)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 2;
                    askQuote = this.option.LastMarket.LastPrice + 2;
                }
                else if (this.option.LastMarket.LastPrice < 100)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 4;
                    askQuote = this.option.LastMarket.LastPrice + 4;
                }
                else if (this.option.LastMarket.LastPrice < 250)
                {
                    bidQuote = this.option.LastMarket.LastPrice - 7.5;
                    askQuote = this.option.LastMarket.LastPrice + 7.5;
                }
                else
                {
                    bidQuote = this.option.LastMarket.LastPrice - 12.5;
                    askQuote = this.option.LastMarket.LastPrice + 12.5;
                }
            }
            #endregion

            #region 计算报单价格--隐含波动率计算法
            //if (this.option.InstrumentID.Contains("1406"))
            //{
            //    if (this.option.OptionType == OptionTypeEnum.call)
            //    {
            //        double volatility = get1406CallImpliedVolatility(this.option.StrikePrice);
            //        double bidVolatility = volatility - 0.01;
            //        double askVolatility = volatility + 0.01;
            //        OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(this.option.OptionType,
            //            this.future.LastMarket.LastPrice, this.option.StrikePrice, GlobalValues.InterestRate, bidVolatility,
            //            StaticFunction.GetDaysToMaturity(this.option.InstrumentID));
            //        bidQuote = OptionPricingModel.EuropeanBS(optionPricingModelParams).Price;
            //        optionPricingModelParams = new OptionPricingModelParams(this.option.OptionType,
            //            this.future.LastMarket.LastPrice, this.option.StrikePrice, GlobalValues.InterestRate, askVolatility,
            //            StaticFunction.GetDaysToMaturity(this.option.InstrumentID));
            //        askQuote = OptionPricingModel.EuropeanBS(optionPricingModelParams).Price;
            //    }
            //    else if (this.option.OptionType == OptionTypeEnum.put)
            //    {
            //        double volatility = get1406PutImpliedVolatility(this.option.StrikePrice);
            //        double bidVolatility = volatility - 0.01;
            //        double askVolatility = volatility + 0.01;
            //        OptionPricingModelParams optionPricingModelParams = new OptionPricingModelParams(this.option.OptionType,
            //            this.future.LastMarket.LastPrice, this.option.StrikePrice, GlobalValues.InterestRate, bidVolatility,
            //            StaticFunction.GetDaysToMaturity(this.option.InstrumentID));
            //        bidQuote = OptionPricingModel.EuropeanBS(optionPricingModelParams).Price;
            //        optionPricingModelParams = new OptionPricingModelParams(this.option.OptionType,
            //            this.future.LastMarket.LastPrice, this.option.StrikePrice, GlobalValues.InterestRate, askVolatility,
            //            StaticFunction.GetDaysToMaturity(this.option.InstrumentID));
            //        askQuote = OptionPricingModel.EuropeanBS(optionPricingModelParams).Price;
            //    }
            //}
            //else
            //{
            //    if (this.option.LastMarket.LastPrice < 10)
            //    {
            //        bidQuote = this.option.LastMarket.LastPrice - 0.5;
            //        askQuote = this.option.LastMarket.LastPrice + 0.5;
            //    }
            //    else if (this.option.LastMarket.LastPrice < 20)
            //    {
            //        bidQuote = this.option.LastMarket.LastPrice - 1;
            //        askQuote = this.option.LastMarket.LastPrice + 1;
            //    }
            //    else if (this.option.LastMarket.LastPrice < 50)
            //    {
            //        bidQuote = this.option.LastMarket.LastPrice - 2;
            //        askQuote = this.option.LastMarket.LastPrice + 2;
            //    }
            //    else if (this.option.LastMarket.LastPrice < 100)
            //    {
            //        bidQuote = this.option.LastMarket.LastPrice - 4;
            //        askQuote = this.option.LastMarket.LastPrice + 4;
            //    }
            //    else if (this.option.LastMarket.LastPrice < 250)
            //    {
            //        bidQuote = this.option.LastMarket.LastPrice - 7.5;
            //        askQuote = this.option.LastMarket.LastPrice + 7.5;
            //    }
            //    else
            //    {
            //        bidQuote = this.option.LastMarket.LastPrice - 12.5;
            //        askQuote = this.option.LastMarket.LastPrice + 12.5;
            //    }
            //}
            #endregion

            this.option.MMQuotation.BidQuote = bidQuote;
            this.option.MMQuotation.AskQuote = askQuote;
            quote[0] = bidQuote;
            quote[1] = askQuote;
            return quote;
        }


        private double get1406CallImpliedVolatility(double strikePrice)
        {
            return 0.000001007 * strikePrice * strikePrice - 0.004444 * strikePrice + 5.178;
        }

        private double get1406PutImpliedVolatility(double strikePrice)
        {
            return 0.0000005747 * strikePrice * strikePrice - 0.002561 * strikePrice + 3.11;
        }

        private double get1407CallImpliedVolatility(double strikePrice)
        {
            return 0.0013 * strikePrice * strikePrice - 0.0084 * strikePrice + 0.2909;
        }

        private double get1407PutImpliedVolatility(double strikePrice)
        {
            return 0.001 * strikePrice * strikePrice - 0.0069 * strikePrice + 0.2912;
        }
        private double get1408CallImpliedVolatility(double strikePrice)
        {
            return -0.001 * strikePrice * strikePrice - 0.0048 * strikePrice + 0.2831;
        }

        private double get1408PutImpliedVolatility(double strikePrice)
        {
            return -0.0015 * strikePrice * strikePrice - 0.0115 * strikePrice + 0.2517;
        }

        /// <summary>
        /// 撤未成交单逻辑
        /// </summary>
        private void CancelOrder()
        {
            if (this.option.PlaceLongOptionOrderRef != null)
            {
                int cancelOrderResult = -1;
                do
                {
                    cancelOrderResult = TDManager.TD.CancelOrder(this.option.LongOptionOrder);
                }
                while (cancelOrderResult != 0);
            }
            if (this.option.PlaceShortOptionOrderRef != null)
            {
                int cancelOrderResult = -1;
                do
                {
                    cancelOrderResult = TDManager.TD.CancelOrder(this.option.ShortOptionOrder);
                }
                while (cancelOrderResult != 0);
            }
            if (this.option.CloseLongOptionOrderRef != null)
            {
                int cancelOrderResult = -1;
                do
                {
                    cancelOrderResult = TDManager.TD.CancelOrder(this.option.CloseLongOptionOrder);
                }
                while (cancelOrderResult != 0);
            }
            if (this.option.CloseShortOptionOrderRef != null)
            {
                int cancelOrderResult = -1;
                do
                {
                    cancelOrderResult = TDManager.TD.CancelOrder(this.option.CloseShortOptionOrder);
                }
                while (cancelOrderResult != 0);
            }
        }

        /// <summary>
        /// 下单逻辑
        /// </summary>
        private void PlaceOrder(double []quote)
        {
            //清空报单信息
            this.option.clearOrderInfo();
            int placeOrderVolume = 10;
            double bidQuote = quote[0];
            double askQuote = quote[1];
            #region 下单--平仓优先
            //if (this.option.shortPosition.Position >= 10)
            //{
            //    this.option.CloseShortOptionOrderRef = TDManager.TD.BuyToCover(this.option.InstrumentID, placeOrderVolume, bidQuote);
            //    if (this.option.longPosition.Position >= 10)
            //    {
            //        this.option.CloseLongOptionOrderRef = TDManager.TD.Sell(this.option.InstrumentID, placeOrderVolume, askQuote);
            //    }
            //    else
            //    {
            //        this.option.PlaceShortOptionOrderRef = TDManager.TD.SellShort(this.option.InstrumentID, placeOrderVolume, askQuote);
            //    }
            //}
            //else if (this.option.longPosition.Position >= 10)
            //{
            //    this.option.CloseLongOptionOrderRef = TDManager.TD.Sell(this.option.InstrumentID, placeOrderVolume, askQuote);
            //    if (this.option.shortPosition.Position >= 10)
            //    {
            //        this.option.CloseShortOptionOrderRef = TDManager.TD.BuyToCover(this.option.InstrumentID, placeOrderVolume, bidQuote);
            //    }
            //    else
            //    {
            //        this.option.PlaceLongOptionOrderRef = TDManager.TD.Buy(this.option.InstrumentID, placeOrderVolume, bidQuote);
            //    }
            //}
            //else
            //{
            //    this.option.PlaceLongOptionOrderRef = TDManager.TD.Buy(this.option.InstrumentID, placeOrderVolume, bidQuote);
            //    this.option.PlaceShortOptionOrderRef = TDManager.TD.SellShort(this.option.InstrumentID, placeOrderVolume, askQuote);
            //}
            #endregion

            #region 直接开仓
            this.option.PlaceLongOptionOrderRef = TDManager.TD.Buy(this.option.InstrumentID, placeOrderVolume, bidQuote);
            this.option.PlaceShortOptionOrderRef = TDManager.TD.SellShort(this.option.InstrumentID, placeOrderVolume, askQuote);
            #endregion
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

        /// <summary>
        /// 刷新策略行
        /// </summary>
        public void RefreshDataRow()
        {
            this.Cells["cBidQuote"].Value = this.option.MMQuotation.BidQuote;
            this.Cells["cBidPrice"].Value = this.option.LastMarket == null ? 0 : this.option.LastMarket.BidPrice1;
            this.Cells["cMarketPrice"].Value = this.option.LastMarket == null ? 0 : this.option.LastMarket.LastPrice;
            this.Cells["cAskPrice"].Value = this.option.LastMarket == null ? 0 : this.option.LastMarket.AskPrice1;
            this.Cells["cAskQuote"].Value = this.option.MMQuotation.AskQuote;
            this.Cells["cLongVolume"].Value = this.option.longPosition.Position;
            this.Cells["cShortVolume"].Value = this.option.shortPosition.Position;
            this.Cells["cImpliedVolatility"].Value = this.option.ImpliedVolatility;
            this.Cells["cDelta"].Value = this.option.OptionValue.Delta;
            string runningStatus = "";
            if(this.isMarketMakingContract)
            {
                if(this.isRunning)
                {
                    runningStatus = "正在运行";
                }
                else
                {
                    runningStatus = "停止";
                }
            }
            else
            {
                runningStatus = "非做市合约";
            }
            this.Cells["cRunningStatus"].Value = runningStatus;
        }

        /// <summary>
        /// 停止策略
        /// </summary>
        public void Stop()
        {
            if(isMarketMakingContract && isRunning)
            {
                this.isRunning = false;
                this.panelRefreshCallback(null);
                Thread.Sleep(300);
            }
            //MDManager.MD.OnTick -= MD_OnTick;
            //TDManager.TD.OnCanceled -= TD_OnCanceled;
            //TDManager.TD.OnTraded -= TD_OnTraded;
            //TDManager.TD.OnTrading -= TD_OnTrading;
        }

        /// <summary>
        /// 重写ToString函数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(this.option.LastMarket == null)
            {
                return "";
            }
            else
            {
                return this.option.InstrumentID + "," + this.option.LastMarket.LastPrice + "," + this.option.ImpliedVolatility + "," + this.option.OptionValue.Delta;
            }
        }
    }//class
}
