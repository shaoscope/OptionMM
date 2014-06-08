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

namespace OptionMM
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            instance = this;
        }


        //唯一实例
        private static MainForm instance;

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        public static MainForm Instance
        {
            get { return MainForm.instance; }
        }

        private Dictionary<string, string[]> configValues = new Dictionary<string, string[]>();

        /// <summary>
        /// 所有仓位信息
        /// </summary>
        public static List<ThostFtdcInvestorPositionField> PositionList = new List<ThostFtdcInvestorPositionField>();
        
        //同步对象
        private readonly object gSyncRoot = new object();

        //仓位对冲定时器
        private System.Threading.Timer positionHedgeTimer;

        private System.Threading.Timer recordVolatilityTimer; 

        private void MainForm_Load(object sender, EventArgs e)
        {
            //加载面板
            InitFromXML("Option.xml");
            foreach (string instrumentID in configValues.Keys)
            {
                Strategy strategy = new Strategy();
                strategy.Option.InstrumentID = instrumentID;
                string[] strTemp = instrumentID.Split('-');
                strategy.Option.StrikePrice = double.Parse(strTemp[2]);
                strategy.Option.OptionType = strTemp[1] == "C" ? OptionTypeEnum.call : OptionTypeEnum.put;
                strategy.Future.InstrumentID = configValues[instrumentID][0];
                strategy.IsMarketMakingContract = configValues[instrumentID][1] == "1" ? true : false;
                //加入仓位信息
                foreach (ThostFtdcInvestorPositionField position in MainForm.PositionList)
                {
                    if(position.InstrumentID == instrumentID && position.PosiDirection == EnumPosiDirectionType.Long)
                    {
                        strategy.Option.longPosition = position;
                    }
                    else if (position.InstrumentID == instrumentID && position.PosiDirection == EnumPosiDirectionType.Short)
                    {
                        strategy.Option.shortPosition = position;
                    }
                }
                strategy.Configuration();
                this.optionPanel.AddStrategy(strategy);
            }
            this.positionHedgeTimer = new System.Threading.Timer(this.positionHedgeCallBack, null, 10 * 1000, 1 * 10 * 1000);
            this.recordVolatilityTimer = new System.Threading.Timer(this.recordVolatilityCallBack, null, 20 * 1000, 10 * 60 * 1000);
        }

        /// <summary>
        /// 记录波动率回调
        /// </summary>
        /// <param name="state"></param>
        private void recordVolatilityCallBack(object state)
        {
            StreamWriter fileWriter = new StreamWriter("C://Users//user//Desktop//VolatilityRecord//" + 
                System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Hour + "-" + 
                System.DateTime.Now.Minute + ".csv");
            foreach (DataGridViewRow dataRow in MainForm.instance.optionPanel.dataTable.Rows)
            {
                Strategy strategy = (Strategy)dataRow.Tag;
                fileWriter.WriteLine(strategy.ToString());
            }
            fileWriter.Flush();
            fileWriter.Close();
        }

        /// <summary>
        /// 仓位对冲回调
        /// </summary>
        /// <param name="state"></param>
        private void positionHedgeCallBack(object state)
        {            
            PositionHedge positionHedge = new PositionHedge(PositionList, this.optionPanel.dataTable);
            this.BeginInvoke(new Action<PositionHedge>(this.RefreshHedgeVolumeLabel), positionHedge);
            //positionHedge.DoHedge();
        }

        /// <summary>
        /// 更新面板对冲手数
        /// </summary>
        /// <param name="positionHedge"></param>
        private void RefreshHedgeVolumeLabel(PositionHedge positionHedge)
        {
            this.hedgeIFVolumeLabel.Text = "对冲IF1406(手): " + (int)positionHedge.FutureHedgeVolume["IF1406"];
        }

        /// <summary>
        /// 读取XML并初始化面板
        /// </summary>
        /// <param name="strFile"></param>
        private void InitFromXML(string strFile)
        {
            
            XmlTextReader xmlRder = null;
            string strFullPathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "") + @"\" + strFile;

            try
            {
                xmlRder = new XmlTextReader(strFullPathFile);
                xmlRder.WhitespaceHandling = WhitespaceHandling.None;

                if (xmlRder == null)
                {
                    return;
                }
                while (xmlRder.Read())
                {
                    xmlRder.MoveToContent();

                    if ((xmlRder.Name == "Option") && (xmlRder.HasAttributes))
                    {
                        string strID = null;
                        string[] Property = new string[2];
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
                            else if(xmlRder.Name == "做市合约")
                            {
                                Property[1] = xmlRder.Value;
                            }
                        }
                        configValues.Add(strID, Property);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (xmlRder != null)
                    xmlRder.Close();
            }
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
            if (!areAllRunning)
            {
                foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
                {
                    Strategy strategy = (Strategy)dataRow.Tag;
                    strategy.Start();
                    //Thread.Sleep(300);
                }
                areAllRunning = true;
                this.startAllButton.Text = "全部停止";
            }
            else
            {
                foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
                {
                    Strategy strategy = (Strategy)dataRow.Tag;
                    strategy.Stop();
                    //Thread.Sleep(300);
                }
                areAllRunning = false;
                this.startAllButton.Text = "全部启动";
            }
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
            this.arbitrageRichTextBox.Clear();
            scanCoveredTask = new Task(ScanCovered);
            scanCoveredTask.Start();
            scanCoveredTask.ContinueWith(ScanArbitrageFinished);
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
            //下限套利
            foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            {
                Strategy strategy = (Strategy)dataRow.Tag;
                if (strategy.Option.OptionType == OptionTypeEnum.call && strategy.Future.LastMarket.LastPrice > strategy.Option.StrikePrice &&
                    strategy.Option.LastMarket.LastPrice < strategy.Future.LastMarket.LastPrice - strategy.Option.StrikePrice)
                {
                    //Long Call + Short IF
                    string appendingText = "\n" + strategy.Option.InstrumentID + "--Long";
                    this.BeginInvoke(new Action<string>(this.AppendTextToArbitrageRichTextBox), appendingText);
                }
                else if (strategy.Option.OptionType == OptionTypeEnum.put && strategy.Option.StrikePrice > strategy.Future.LastMarket.LastPrice &&
                    strategy.Option.LastMarket.LastPrice < strategy.Option.StrikePrice - strategy.Future.LastMarket.LastPrice)
                {
                    //Long Put + Long IF
                    string appendingText = "\n" + strategy.Option.InstrumentID + "--Short";
                    this.BeginInvoke(new Action<string>(this.AppendTextToArbitrageRichTextBox), appendingText);
                }
            }
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
            Dictionary<string, Strategy> strategyDictionary = new Dictionary<string,Strategy>();
            foreach (DataGridViewRow dataRow in this.optionPanel.dataTable.Rows)
            {
                Strategy strategy = (Strategy)dataRow.Tag;
                strategyDictionary.Add(strategy.Option.InstrumentID, strategy);
            }
            foreach(Strategy strategyCall in strategyDictionary.Values)
            {
                if(strategyCall.Option.OptionType == OptionTypeEnum.call)
                {
                    string parityPutInstrumentID = strategyCall.Option.InstrumentID.Replace('C', 'P');
                    Strategy strategyPut = strategyDictionary[parityPutInstrumentID];
                    if(strategyCall.Option.LastMarket.LastPrice + strategyCall.Option.StrikePrice > strategyPut.Option.LastMarket.LastPrice + strategyPut.Future.LastMarket.LastPrice)
                    {
                        //Long Put + Short Call
                        string appendingText = "\n" + strategyPut.Option.InstrumentID + "--Long + " + strategyCall.Option.InstrumentID + "--Short";
                        this.BeginInvoke(new Action<string>(this.AppendTextToArbitrageRichTextBox), appendingText);
                    }
                    else if (strategyCall.Option.LastMarket.LastPrice + strategyCall.Option.StrikePrice < strategyPut.Option.LastMarket.LastPrice + strategyPut.Future.LastMarket.LastPrice)
                    {
                        //Long Call + Short Put
                        string appendingText = "\n" + strategyCall.Option.InstrumentID + "--Long + " + strategyPut.Option.InstrumentID + "--Short";
                        this.BeginInvoke(new Action<string>(this.AppendTextToArbitrageRichTextBox), appendingText);
                    }
                }
            }
        }

        private void AppendTextToArbitrageRichTextBox(string appendingText)
        {
            this.arbitrageRichTextBox.AppendText(appendingText);
        }

        /// <summary>
        /// 寻找平价套利机会
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parityButton_Click(object sender, EventArgs e)
        {
            this.arbitrageRichTextBox.Clear();
            scanParityTask = new Task(ScanParity);
            scanParityTask.Start();
            scanParityTask.ContinueWith(ScanArbitrageFinished);
        }


    }//class
}//namespace
