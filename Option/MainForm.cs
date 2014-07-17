using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTP;
using System.Threading;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace OptionMM
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            instance = this;
        }


        /// <summary>
        /// 唯一实例
        /// </summary>
        private static MainForm instance;

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        public static MainForm Instance
        {
            get { return MainForm.instance; }
        }

        /// <summary>
        /// 所有仓位信息
        /// </summary>
        public static List<ThostFtdcInvestorPositionField> PositionList = new List<ThostFtdcInvestorPositionField>();
        
        //同步对象
        private readonly object gSyncRoot = new object();

        //仓位对冲定时器
        private System.Threading.Timer positionHedgeTimer;

        private System.Threading.Timer recordVolatilityTimer;

        private System.Threading.Timer writeXmlTimer;

        public static Future Future;

        /// <summary>
        /// 行情接口
        /// </summary>
        public CTPMDAdapter marketer { get; private set; }

        /// <summary>
        /// 交易接口
        /// </summary>
        public CTPTraderAdapter trader { get; private set; }

        /// <summary>
        /// 所有合约列表
        /// </summary>
        public List<ThostFtdcInstrumentField> InstrumentsList { get; private set; }

        /// <summary>
        /// 投资者持仓
        /// </summary>
        public List<ThostFtdcInvestorPositionField> InvestorPositionList { get; private set; }

        /// <summary>
        /// 报价窗口
        /// </summary>
        public QuoteForm QuoteForm { get; private set; }

        /// <summary>
        /// 行情管理器
        /// </summary>
        public MarketManager MarketManeger { get; private set; }

        /// <summary>
        /// 启动报价任务
        /// </summary>
        private Task startUpMMTask;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); 
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.Cancel)
            {
                this.Close();
                return;
            }
            this.trader = loginForm.trader;
            this.marketer = loginForm.marketer;
            //订阅行情
            this.InstrumentsList = loginForm.instrumentsList;
            this.InvestorPositionList = loginForm.investorPositionList;
            MarketManeger = new MarketManager(this.marketer);
            foreach (ThostFtdcInstrumentField instrument in InstrumentsList)
            {
                if(instrument.InstrumentID.StartsWith("IF")||instrument.InstrumentID.StartsWith("IO"))
                {
                    ActiveContract activeContract = new ActiveContract(instrument);
                    foreach(ThostFtdcInvestorPositionField position in this.InvestorPositionList)
                    {
                        if(position.InstrumentID == instrument.InstrumentID && position.PosiDirection == EnumPosiDirectionType.Long)
                        {
                            activeContract.LongPosition = position;
                        }
                        else if(position.InstrumentID == instrument.InstrumentID && position.PosiDirection == EnumPosiDirectionType.Short)
                        {
                            activeContract.ShortPosition = position;
                        }
                    }
                    MarketManeger.AddActiveContract(activeContract);
                }
            }

            this.marketer.OnRtnDepthMarketData += marketer_OnRtnDepthMarketData;

            #region 加载报价面板
            
            //this.quoteFormToolStripMenuItem.PerformClick();
            #endregion



            //Dictionary<string, string[]> MMConfigValues = ReadOptionXML("Option.xml");
            //Future = new Future("IF1407");
            //foreach (string instrumentID in MMConfigValues.Keys)
            //{
            //    Strategy strategy = new Strategy();
            //    strategy.Option.InstrumentID = instrumentID;
            //    string[] strTemp = instrumentID.Split('-');
            //    strategy.Option.StrikePrice = double.Parse(strTemp[2]);
            //    strategy.Option.OptionType = strTemp[1] == "C" ? OptionTypeEnum.call : OptionTypeEnum.put;
            //    strategy.IsMarketMakingContract = MMConfigValues[instrumentID][1] == "1" ? true : false;
            //    //加入仓位信息
            //    foreach (ThostFtdcInvestorPositionField position in MainForm.PositionList)
            //    {
            //        if (position != null)
            //        {
            //            if (position.InstrumentID == instrumentID && position.PosiDirection == EnumPosiDirectionType.Long)
            //            {
            //                strategy.Option.longPosition = position;
            //            }
            //            else if (position.InstrumentID == instrumentID && position.PosiDirection == EnumPosiDirectionType.Short)
            //            {
            //                strategy.Option.shortPosition = position;
            //            }
            //        }
            //    }
            //    strategy.Option.longPosition.PositionCost = double.Parse(MMConfigValues[instrumentID][3]);
            //    strategy.Option.shortPosition.PositionCost = double.Parse(MMConfigValues[instrumentID][5]);
            //    strategy.Configuration();
            //    this.optionPanel.AddStrategy(strategy);
            //}

            #region 加载平价套利
            //Dictionary<string, string[]> ParityConfigValues = ReadParityXML("Parity.xml");
            //foreach (string instrumentID in ParityConfigValues.Keys)
            //{
            //    string[] configValues = ParityConfigValues[instrumentID];
            //    Parity parity = new Parity(instrumentID, (EnumPosiDirectionType)Enum.Parse(typeof(EnumPosiDirectionType), configValues[0]), configValues[1],
            //        (EnumPosiDirectionType)Enum.Parse(typeof(EnumPosiDirectionType), configValues[2]), 0, Double.Parse(configValues[3]), Double.Parse(configValues[4]),
            //        Double.Parse(configValues[5]), Double.Parse(configValues[6]));
            //    parity.Configuration();
            //    this.parityPanel.AddParity(parity);
            //}

            #endregion

            //this.positionHedgeTimer = new System.Threading.Timer(this.positionHedgeCallBack, null, 3 * 1000, 5 * 1000);
            //this.recordVolatilityTimer = new System.Threading.Timer(this.recordVolatilityCallBack, null, 20 * 1000, 10 * 60 * 1000);
            //this.writeXmlTimer = new System.Threading.Timer(this.writerXmlCallBack, null, 10 * 1000, 10 * 1000);
        }

        /// <summary>
        /// 行情到达时的处理
        /// </summary>
        /// <param name="MarketData"></param>
        void marketer_OnRtnDepthMarketData(ThostFtdcDepthMarketDataField MarketData)
        {
            ActiveContract activeContract;
            if (MarketManeger.ActiveContractDictionary.TryGetValue(MarketData.InstrumentID, out activeContract))
            {
                activeContract.UpdateMarket(MarketData);
            }
        }

        /// <summary>
        /// 保存XML文件回调
        /// </summary>
        /// <param name="state"></param>
        private void writerXmlCallBack(object state)
        {
            this.positionHedgeTimer.Dispose();
            this.recordVolatilityTimer.Dispose();
            this.WriterOptionXML("Option.xml");
        }

        /// <summary>
        /// 记录波动率回调
        /// </summary>
        /// <param name="state"></param>
        private void recordVolatilityCallBack(object state)
        {
            //StreamWriter fileWriter = new StreamWriter("C://Users//user//Desktop//VolatilityRecord//" + 
            //    System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Hour + "-" + 
            //    System.DateTime.Now.Minute + ".csv");
            //foreach (DataGridViewRow dataRow in MainForm.instance.optionPanel.dataTable.Rows)
            //{
            //    Strategy strategy = (Strategy)dataRow.Tag;
            //    fileWriter.WriteLine(strategy.ToString());
            //}
            //fileWriter.Flush();
            //fileWriter.Close();
        }

        

        /// <summary>
        /// 仓位对冲回调
        /// </summary>
        /// <param name="state"></param>
        private void positionHedgeCallBack(object state)
        {
            //Future.DeltaHedge(this.optionPanel.dataTable);
            //this.BeginInvoke(new Action<Future>(this.RefreshHedgeVolumeLabel), Future);
            //Future.AutoHedge();
        }

        /// <summary>
        /// 更新面板对冲手数
        /// </summary>
        /// <param name="future"></param>
        private void RefreshHedgeVolumeLabel(Future future)
        {
            //this.hedgeIFVolumeLabel.Text = "对冲IF1406(手): " + (int)future.HedgeVolume + "-(" + 
            //future.longPosition.Position + "-" + future.shortPosition.Position + ")=" + ((int)future.HedgeVolume - 
            //    (future.longPosition.Position - future.shortPosition.Position));
        }

        /// <summary>
        /// 读取报价配置文件
        /// </summary>
        /// <param name="strFile"></param>
        private Dictionary<string, string[]> ReadOptionXML(string strFile)
        {
            Dictionary<string, string[]> MMConfigValues = new Dictionary<string, string[]>();
            XmlTextReader xmlRder = null;
            string strFullPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "") + @"\" + strFile;
            try
            {
                xmlRder = new XmlTextReader(strFullPathFile);
                xmlRder.WhitespaceHandling = WhitespaceHandling.None;

                if (xmlRder != null)
                {
                    while (xmlRder.Read())
                    {
                        xmlRder.MoveToContent();

                        if ((xmlRder.Name == "Option") && (xmlRder.HasAttributes))
                        {
                            string strID = null;
                            string[] Property = new string[6];
                            for (int i = 0; i < xmlRder.AttributeCount; i++)
                            {
                                xmlRder.MoveToAttribute(i);
                                if (xmlRder.Name == "InstrumentID")
                                {
                                    strID = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "标的物")
                                {
                                    Property[0] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "做市合约")
                                {
                                    Property[1] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "多头仓位数")
                                {
                                    Property[2] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "多头持仓均价")
                                {
                                    Property[3] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "空头仓位数")
                                {
                                    Property[4] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "空头持仓均价")
                                {
                                    Property[5] = xmlRder.Value;
                                }
                            }
                            MMConfigValues.Add(strID, Property);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (xmlRder != null)
                {
                    xmlRder.Close();
                }
            }
            return MMConfigValues;
        }

        /// <summary>
        /// 读取平价套利配置文件
        /// </summary>
        /// <param name="strFile"></param>
        private Dictionary<string, string[]> ReadParityXML(string strFile)
        {
            Dictionary<string, string[]> ParityConfigValues = new Dictionary<string, string[]>();
            XmlTextReader xmlRder = null;
            string strFullPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "") + @"\" + strFile;
            try
            {
                xmlRder = new XmlTextReader(strFullPathFile);
                xmlRder.WhitespaceHandling = WhitespaceHandling.None;
                if (xmlRder != null)
                {
                    while (xmlRder.Read())
                    {
                        xmlRder.MoveToContent();

                        if ((xmlRder.Name == "Parity") && (xmlRder.HasAttributes))
                        {
                            string strID = null;
                            string[] Property = new string[7];
                            for (int i = 0; i < xmlRder.AttributeCount; i++)
                            {
                                xmlRder.MoveToAttribute(i);
                                if (xmlRder.Name == "LongInstrumentID")
                                {
                                    strID = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "CallDirection")
                                {
                                    Property[0] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "ShortInstrumentID")
                                {
                                    Property[1] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "PutDirection")
                                {
                                    Property[2] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "OpenThreshold")
                                {
                                    Property[3] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "CloseThreshold")
                                {
                                    Property[4] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "MaxOpenSets")
                                {
                                    Property[5] = xmlRder.Value;
                                }
                                else if (xmlRder.Name == "OpenedSets")
                                {
                                    Property[6] = xmlRder.Value;
                                }
                            }
                            ParityConfigValues.Add(strID, Property);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (xmlRder != null)
                {
                    xmlRder.Close();
                }
            }
            return ParityConfigValues;
        }

        /// <summary>
        /// 策略是否已经全开的标记位
        /// </summary>
        private bool areAllRunning = false;

        /// <summary>
        /// 点击全部启动/全部停止按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAllButton_Click(object sender, EventArgs e)
        {
            //if (!areAllRunning)
            //{
            //    startUpMMTask = new Task(startUpMM);
            //    startUpMMTask.Start();
            //    startUpMMTask.ContinueWith(StartUpMMFinished);
                
            //}
            //else
            //{
            //    foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            //    {
            //        Strategy strategy = (Strategy)dataRow.Tag;
            //        strategy.Stop();
            //    }
            //    areAllRunning = false;
            //    this.startAllButton.Text = "全部启动";
            //}
        }

        /// <summary>
        /// 报价全部启动完成事件
        /// </summary>
        /// <param name="task"></param>
        private void StartUpMMFinished(Task task)
        {
            //areAllRunning = true;
            //this.BeginInvoke(new Action(()=>this.startAllButton.Text = "全部停止"));
        }

        /// <summary>
        /// 启动报价方法
        /// </summary>
        private void startUpMM()
        {
            //foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            //{
            //    Strategy strategy = (Strategy)dataRow.Tag;
            //    strategy.Start();
            //}
        }

        /// <summary>
        /// 点击风险管理按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void riskManagementButton_Click(object sender, EventArgs e)
        {
            RiskManagementForm riskManagementForm = new RiskManagementForm();
            riskManagementForm.ShowDialog();
        }

        /// <summary>
        /// 寻找单期权上下限套利机会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coveredButton_Click(object sender, EventArgs e)
        {
            //this.coveredPanel.dataTable.Rows.Clear();
            //scanCoveredTask = new Task(ScanCovered);
            //scanCoveredTask.Start();
            //scanCoveredTask.ContinueWith(ScanArbitrageFinished);
        }

        /// <summary>
        /// 扫描上限先套利Task
        /// </summary>
        private Task scanCoveredTask;

        /// <summary>
        /// 扫描上下限套利方法
        /// </summary>
        private void ScanCovered()
        {
            ////下限套利
            //foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            //{
            //    Strategy strategy = (Strategy)dataRow.Tag;
            //    if (strategy.Option.OptionType == OptionTypeEnum.call && MainForm.Future.LastMarket.LastPrice > strategy.Option.StrikePrice &&
            //        strategy.Option.LastMarket.AskPrice1 < MainForm.Future.LastMarket.LastPrice - strategy.Option.StrikePrice)
            //    {
            //        //Long Call + Short IF
            //        double coveredInterval = MainForm.Future.LastMarket.LastPrice - strategy.Option.StrikePrice - strategy.Option.LastMarket.AskPrice1;
            //        Covered covered = new Covered(strategy.Option.InstrumentID, 0, coveredInterval, 0);
            //        this.BeginInvoke(new Action<Covered>(this.AddCovered), covered);
            //    }
            //    else if (strategy.Option.OptionType == OptionTypeEnum.put && strategy.Option.StrikePrice > MainForm.Future.LastMarket.LastPrice &&
            //        strategy.Option.LastMarket.AskPrice1 < strategy.Option.StrikePrice - MainForm.Future.LastMarket.LastPrice)
            //    {
            //        //Long Put + Long IF
            //        double coveredInterval = strategy.Option.StrikePrice - MainForm.Future.LastMarket.LastPrice - strategy.Option.LastMarket.AskPrice1;
            //        Covered covered = new Covered(strategy.Option.InstrumentID, 0, coveredInterval, 0);
            //        this.BeginInvoke(new Action<Covered>(this.AddCovered), covered);
            //    }
            //}
        }

        /// <summary>
        /// 扫描套利机会完成
        /// </summary>
        private void ScanArbitrageFinished(Task task)
        {
            try
            {
                //MessageBox.Show("套利机会扫描完成");
            }
            catch
            {
                
            }
            finally
            {

            }
        }

        /// <summary>
        /// 扫描平价套利Task
        /// </summary>
        private Task scanParityTask;

        /// <summary>
        /// 扫描平价套利机会
        /// </summary>
        private void ScanParity()
        {
            //Dictionary<string, Strategy> strategyDictionary = new Dictionary<string,Strategy>();
            //foreach (DataGridViewRow dataRow in this.ParityPanel.dataTable.Rows)
            //{
            //    Strategy strategy = (Strategy)dataRow.Tag;
            //    strategyDictionary.Add(strategy.Parity.InstrumentID, strategy);
            //}
            //foreach(Strategy strategyCall in strategyDictionary.Values)
            //{
            //    if(strategyCall.Parity.ParityType == ParityTypeEnum.call)
            //    {
            //        string parityPutInstrumentID = strategyCall.Parity.InstrumentID.Replace('C', 'P');
            //        Strategy strategyPut = strategyDictionary[parityPutInstrumentID];
            //        if (strategyCall.Parity.LastMarket.BidPrice1 + strategyCall.Parity.StrikePrice > strategyPut.Parity.LastMarket.AskPrice1 + MainForm.Future.LastMarket.LastPrice)
            //        {
            //            //Long Put + Short Call
            //            double parityInterval = strategyCall.Parity.LastMarket.BidPrice1 + strategyCall.Parity.StrikePrice - strategyPut.Parity.LastMarket.AskPrice1 - MainForm.Future.LastMarket.LastPrice;
            //            Parity parity = new Parity(strategyCall.Parity.InstrumentID, EnumPosiDirectionType.Short, strategyPut.Parity.InstrumentID, EnumPosiDirectionType.Long, parityInterval);
            //            this.BeginInvoke(new Action<Parity>(this.AddPrity), parity);
            //        }
            //        else if (strategyCall.Parity.LastMarket.AskPrice1 + strategyCall.Parity.StrikePrice < strategyPut.Parity.LastMarket.BidPrice1 + MainForm.Future.LastMarket.LastPrice)
            //        {
            //            //Long Call + Short Put
            //            double parityInterval = strategyPut.Parity.LastMarket.BidPrice1 + MainForm.Future.LastMarket.LastPrice - strategyCall.Parity.LastMarket.AskPrice1 - strategyCall.Parity.StrikePrice;
            //            Parity parity = new Parity(strategyCall.Parity.InstrumentID, EnumPosiDirectionType.Long, strategyPut.Parity.InstrumentID, EnumPosiDirectionType.Short, parityInterval);
            //            this.BeginInvoke(new Action<Parity>(this.AddPrity), parity);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 添加上下限套利
        /// </summary>
        /// <param name="covered"></param>
        private void AddCovered(Covered covered)
        {

            //this.coveredPanel.AddCovered(covered);
        }

        /// <summary>
        /// 寻找平价套利机会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parityButton_Click(object sender, EventArgs e)
        {
            //this.parityPanel.dataTable.Rows.Clear();
            //scanParityTask = new Task(ScanParity);
            //scanParityTask.Start();
            //scanParityTask.ContinueWith(ScanArbitrageFinished);
        }

        /// <summary>
        /// 将报价状态写入XML
        /// </summary>
        private void WriterOptionXML(string strFile)
        {
            //lock (this)
            //{
            //    string strFullPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "") + @"\" + strFile;
            //    XmlDocument xmlDoc = new XmlDocument();
            //    try
            //    {
            //        xmlDoc.Load(strFullPathFile);
            //        XmlNodeList xnl = xmlDoc.SelectSingleNode("root").ChildNodes;
            //        int itemIndex = 0;
            //        foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            //        {
            //            Strategy strategy = (Strategy)dataRow.Tag;
            //            XmlElement xe = (XmlElement)xnl.Item(itemIndex++);
            //            if (xe.Attributes["InstrumentID"].Value == strategy.Option.InstrumentID)
            //            {
            //                xe.SetAttribute("多头仓位数", strategy.Option.longPosition.Position.ToString());
            //                xe.SetAttribute("多头持仓均价", strategy.Option.longPosition.PositionCost.ToString());
            //                xe.SetAttribute("空头仓位数", strategy.Option.shortPosition.Position.ToString());
            //                xe.SetAttribute("空头持仓均价", strategy.Option.shortPosition.PositionCost.ToString());
            //            }
            //        }
            //        xmlDoc.Save(strFullPathFile);
            //    }
            //    catch
            //    {

            //    }
            //    finally
            //    {

            //    }
            //}
        }

        /// <summary>
        /// 将平价套利策略状态写入XML
        /// </summary>
        /// <param name="strFile"></param>
        private void WriteParityXML(string strFile)
        {
            //lock (this)
            //{
            //    string strFullPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "") + @"\" + strFile;
            //    XmlDocument xmlDoc = new XmlDocument();
            //    try
            //    {
            //        xmlDoc.Load(strFullPathFile);
            //        XmlNodeList xnl = xmlDoc.SelectSingleNode("root").ChildNodes;
            //        int itemIndex = 0;
            //        foreach (DataGridViewRow dataRow in this.parityPanel.dataTable.Rows)
            //        {
            //            Parity parity = (Parity)dataRow.Tag;
            //            XmlElement xe = (XmlElement)xnl.Item(itemIndex++);
            //            if (xe.Attributes["LongInstrumentID"].Value == parity.LongOption.InstrumentID)
            //            {
            //                xe.SetAttribute("OpenThreshold", parity.OpenThreshold.ToString());
            //                xe.SetAttribute("CloseThreshold", parity.CloseThreshold.ToString());
            //                xe.SetAttribute("MaxOpenSets", parity.MaxOpenSets.ToString());
            //                xe.SetAttribute("OpenedSets", parity.OpenedSets.ToString());
            //            }
            //        }
            //        xmlDoc.Save(strFullPathFile);
            //    }
            //    catch
            //    {

            //    }
            //    finally
            //    {

            //    }
            //}
        }


        /// <summary>
        /// 扫描平价套利Task
        /// </summary>
        private Task scanVerticalTask;

        private void verticalButton_Click(object sender, EventArgs e)
        {
            //this.verticalPanel.dataTable.Rows.Clear();
            //scanVerticalTask = new Task(ScanVertical);
            //scanVerticalTask.Start();
            //scanVerticalTask.ContinueWith(ScanArbitrageFinished);
        }

        /// <summary>
        /// 扫描垂直套利机会
        /// </summary>
        private void ScanVertical()
        {
            //List<Strategy> strategyList = new List<Strategy>();
            //foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            //{
            //    Strategy strategy = (Strategy)dataRow.Tag;
            //    if (strategy.Option.InstrumentID.Contains("IF1407"))
            //    {
            //        strategyList.Add(strategy);
            //    }
            //}
            //for (int i = 0; i < strategyList.Count - 1; i ++ )
            //{
            //    Strategy strategyLower = strategyList[i];
            //    for(int j = i+1; j < strategyList.Count; j++)
            //    {
            //        Strategy strategyUpper = strategyList[j];
            //        if (strategyLower.Option.OptionType == strategyUpper.Option.OptionType && strategyUpper.Option.OptionType == OptionTypeEnum.call)
            //        {
            //            if (strategyUpper.Option.StrikePrice - strategyLower.Option.StrikePrice < strategyLower.Option.LastMarket.AskPrice1 - strategyUpper.Option.LastMarket.BidPrice1)
            //            {
            //                double verticalInterval = strategyLower.Option.LastMarket.AskPrice1 - strategyUpper.Option.LastMarket.BidPrice1 -
            //                    strategyUpper.Option.StrikePrice + strategyLower.Option.StrikePrice;
            //                Vertical vertical = new Vertical(strategyUpper.Option.InstrumentID, EnumPosiDirectionType.Long, strategyLower.Option.InstrumentID, EnumPosiDirectionType.Short, verticalInterval);
            //                this.BeginInvoke(new Action<Vertical>(this.AddVertical), vertical);
            //            }
            //        }
            //        else if (strategyLower.Option.OptionType == strategyUpper.Option.OptionType && strategyUpper.Option.OptionType == OptionTypeEnum.put)
            //        {
            //            if (strategyUpper.Option.StrikePrice - strategyLower.Option.StrikePrice < strategyUpper.Option.LastMarket.BidPrice1 - strategyLower.Option.LastMarket.AskPrice1)
            //            {
            //                double verticalInterval = strategyUpper.Option.LastMarket.BidPrice1 - strategyLower.Option.LastMarket.AskPrice1 -
            //                    strategyUpper.Option.StrikePrice + strategyLower.Option.StrikePrice;
            //                Vertical vertical = new Vertical(strategyUpper.Option.InstrumentID, EnumPosiDirectionType.Short, strategyLower.Option.InstrumentID, EnumPosiDirectionType.Long, verticalInterval);
            //                this.BeginInvoke(new Action<Vertical>(this.AddVertical), vertical);
            //            }
            //        }
            //    }
            //}
        }


        private void AddVertical(Vertical vertical)
        {
            //this.verticalPanel.AddVertical(vertical);
        }

        /// <summary>
        /// 显示或者隐藏报价窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quoteFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuoteForm = new QuoteForm();
            foreach (ActiveContract activeContract in MarketManeger.ActiveContractDictionary.Values)
            {
                if (activeContract.Contract.InstrumentID.Contains("C"))
                {
                    string callID = activeContract.Contract.InstrumentID;
                    string putID = callID.Replace("C", "P");
                    ActiveContract call;
                    ActiveContract put;
                    ActiveContract underlying;
                    if (MarketManeger.ActiveContractDictionary.TryGetValue(callID, out call) &&
                        MarketManeger.ActiveContractDictionary.TryGetValue(putID, out put) &&
                        MarketManeger.ActiveContractDictionary.TryGetValue("IF1407", out underlying))
                    {
                        Quote quote = new Quote(call, put, underlying);
                        QuoteForm.quotePanel.AddQuote(quote);
                    }
                }
            }
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                QuoteForm.MdiParent = this;
                QuoteForm.Show();
            }
            else
            {
                QuoteForm.Show(dockPanel);
            }
        }

    }//class
}//namespace
