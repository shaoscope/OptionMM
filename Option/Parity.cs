using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OptionMM
{
    class Parity : DataGridViewRow
    {
        //private Future future = new Future("IF1407");

        /// <summary>
        /// 多头期权
        /// </summary>
        private Option longOption = new Option();

        /// <summary>
        /// 获取多头期权
        /// </summary>
        public Option LongOption
        {
            get { return this.longOption; }
        }

        /// <summary>
        /// 空头期权
        /// </summary>
        private Option shortOption = new Option();

        /// <summary>
        /// 获取空头期权
        /// </summary>
        public Option ShortOption
        {
            get { return this.shortOption; }
        }

        /// <summary>
        /// 看涨合约开仓方向
        /// </summary>
        private EnumPosiDirectionType callDirection;

        /// <summary>
        /// 获取或者设置看涨合约开仓方向
        /// </summary>
        public EnumPosiDirectionType CallDirection
        {
            set { this.callDirection = value; }
            get { return this.callDirection; }
        }

        /// <summary>
        /// 看跌合约开仓方向
        /// </summary>
        private EnumPosiDirectionType putDirection;

        /// <summary>
        /// 获取或者设置看跌合约开仓方向
        /// </summary>
        public EnumPosiDirectionType PutDirection
        {
            set { this.putDirection = value; }
            get { return this.putDirection; }
        }

        /// <summary>
        /// Parity Interval
        /// </summary>
        private double parityInterval;

        /// <summary>
        /// 获取后者设置Parity Interval
        /// </summary>
        public double ParityInterval
        {
            get { return this.parityInterval; }
            set { this.parityInterval = value; }
        }

        /// <summary>
        /// 开仓区间
        /// </summary>
        private double openThreshold;

        /// <summary>
        /// 获取后者设置开仓区间
        /// </summary>
        public double OpenThreshold
        {
            get { return this.openThreshold; }
            set 
            {
                this.openThreshold = value; 
            }
        }

        /// <summary>
        /// 平仓区间
        /// </summary>
        private double closeThreshold;

        /// <summary>
        /// 获取或者设置平仓区间
        /// </summary>
        public double CloseThreshold
        {
            get { return this.closeThreshold; }
            set { this.closeThreshold = value; }
        }

        /// <summary>
        /// 最大开仓数
        /// </summary>
        private double maxOpenSets;

        /// <summary>
        /// 获取后者设置最大开仓数
        /// </summary>
        public double MaxOpenSets
        {
            get { return this.maxOpenSets; }
            set { this.maxOpenSets = value; }
        }

        /// <summary>
        /// 已开仓数
        /// </summary>
        private double openedSets;

        /// <summary>
        /// 获取或者设置已开仓数
        /// </summary>
        public double OpenedSets
        {
            get { return this.openedSets; }
            set { this.openedSets = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Parity()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="callInstrumentID"></param>
        /// <param name="positionVolume"></param>
        /// <param name="parityInterval"></param>
        /// <param name="averagePrice"></param>
        public Parity(string callInstrumentID, EnumPosiDirectionType callDirection, string putInstrumentID, EnumPosiDirectionType putDirection, 
            double parityInterval, double openThreshold, double closeThreshold, double maxOpenSets, double openedSets)
        {
            this.Tag = this;
            string[] callStrTemp = callInstrumentID.Split('-');
            this.longOption.InstrumentID = callInstrumentID;
            this.longOption.StrikePrice = double.Parse(callStrTemp[2]);
            this.longOption.OptionType = callStrTemp[1] == "C" ? OptionTypeEnum.call : OptionTypeEnum.put;
            this.callDirection = callDirection;
            //this.future.InstrumentID = callStrTemp[0];
            string[] putStrTemp = putInstrumentID.Split('-');
            this.shortOption.InstrumentID = putInstrumentID;
            this.shortOption.StrikePrice = double.Parse(putStrTemp[2]);
            this.shortOption.OptionType = putStrTemp[1] == "C" ? OptionTypeEnum.call : OptionTypeEnum.put;
            this.putDirection = putDirection;
            this.parityInterval = parityInterval;
            this.openThreshold = openThreshold;
            this.closeThreshold = closeThreshold;
            this.maxOpenSets = maxOpenSets;
            this.openedSets = openedSets;
        }

        /// <summary>
        /// 配置策略
        /// </summary>
        public void Configuration()
        {
            //MDManager.MD.SubscribeMarketData(new string[] { this.longOption.InstrumentID, this.shortOption.InstrumentID });
            //MDManager.MD.OnTick += MD_OnTick;
            //TDManager.TD.OnCanceled += TD_OnCanceled;
            //TDManager.TD.OnTraded += TD_OnTraded;
            //TDManager.TD.OnTrading += TD_OnTrading;
            //TDManager.TD.OnCancelAction += TD_OnCancelAction;
            //TDManager.TD.OnOrderRefReplace += TD_OnOrderRefReplace;
            //刷新面板定时器
            this.panelRefreshTimer = new System.Threading.Timer(this.panelRefreshCallback, null, 1000, 1000);
        }

        private void TD_OnOrderRefReplace(string orderrefold, string orderrefnew)
        {
            

        }

        private void TD_OnCancelAction(ThostFtdcInputOrderActionField pInputOrderAction, ThostFtdcRspInfoField pRspInfo)
        {
           

        }

        private void TD_OnTrading(ThostFtdcOrderField pOrder)
        {

        }

        private void TD_OnTraded(ThostFtdcOrderField pOrder)
        {

        }

        private void TD_OnCanceled(ThostFtdcOrderField pOrder)
        {

        }

        private void MD_OnTick(ThostFtdcDepthMarketDataField md)
        {
            if (this.longOption.InstrumentID == md.InstrumentID || this.shortOption.InstrumentID == md.InstrumentID)
            {
                if(this.longOption.InstrumentID == md.InstrumentID)
                {
                    this.longOption.LastMarket = md;
                }
                else if(this.shortOption.InstrumentID == md.InstrumentID)
                {
                    this.shortOption.LastMarket = md;
                }
                if(this.longOption.LastMarket != null && this.shortOption.LastMarket != null && MainForm.Future.LastMarket != null)
                {
                    if(this.longOption.OptionType == OptionTypeEnum.call)
                    {
                        //反向套利
                        if (this.shortOption.LastMarket.BidPrice1 < 999999 && this.longOption.LastMarket.AskPrice1 < 999999 && MainForm.Future.LastMarket.BidPrice1 < 999999)
                        {
                            this.parityInterval = (this.shortOption.LastMarket.BidPrice1 - this.longOption.LastMarket.AskPrice1) + (MainForm.Future.LastMarket.BidPrice1 - this.longOption.StrikePrice);
                            //开仓逻辑
                        }
                        else
                        {
                            this.parityInterval = 0;
                        }
                    }
                    else if(this.longOption.OptionType == OptionTypeEnum.put)
                    {
                        //正向套利
                        if (this.shortOption.LastMarket.BidPrice1 < 999999 && this.longOption.LastMarket.AskPrice1 < 999999 && MainForm.Future.LastMarket.AskPrice1 < 999999)
                        {
                            this.parityInterval = (this.shortOption.LastMarket.BidPrice1 - this.longOption.LastMarket.AskPrice1) - (MainForm.Future.LastMarket.AskPrice1 - this.longOption.StrikePrice);
                            //开仓逻辑
                        }
                        else
                        {
                            this.parityInterval = 0;
                        }
                    }
                }
                
            }
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
                    if (parityPanel.InvokeRequired)
                    {
                        parityPanel.BeginInvoke(new MethodInvoker(this.RefreshDataRow));
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

        private System.Threading.Timer panelRefreshTimer;

        /// <summary>
        /// 所属面板控件
        /// </summary>
        private ParityPanel parityPanel;

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(ParityPanel parityPanel)
        {
            this.parityPanel = parityPanel;
        }

        /// <summary>
        /// 刷新策略行
        /// </summary>
        public void RefreshDataRow()
        {
            this.Cells["cParityInterval"].Value = this.parityInterval;
            this.Cells["cOpenedSets"].Value = this.openedSets;
        }


    }//class
}//namespace
