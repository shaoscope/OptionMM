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
                strategy.Option.ImpridVolatility = Double.Parse(configValues[instrumentID][0]);
                strategy.optionPositionThreshold = Double.Parse(configValues[instrumentID][1]);
                strategy.minOptionOpenLots = Double.Parse(configValues[instrumentID][2]);
                //option.maxOptionOpenLots = Double.Parse(configValues[instrumentID][3]);   //开仓点数
                strategy.maxOptionOpenLots = Double.Parse(configValues[instrumentID][4]);
                strategy.Future.InstrumentID = configValues[instrumentID][5];
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
                //插入策略
                this.optionPanel.AddStrategy(strategy);
            }

            this.positionHedgeTimer = new System.Threading.Timer(this.positionHedgeCallBack, null, 3 * 60 * 1000, 3 * 60 * 1000);

        }

        /// <summary>
        /// 仓位对冲回调
        /// </summary>
        /// <param name="state"></param>
        private void positionHedgeCallBack(object state)
        {            
            PositionHedge positionHedge = new PositionHedge(PositionList, this.optionPanel.dataTable);
            //positionHedge.DoHedge();
        }


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
                        string[] Property = new string[6];
                        for (int i = 0; i < xmlRder.AttributeCount; i++)
                        {
                            xmlRder.MoveToAttribute(i);
                            if (xmlRder.Name == "InstrumentID")
                            {
                                strID = xmlRder.Value;
                            }
                            else if (xmlRder.Name == "隐含波动率")
                            {
                                Property[0] = xmlRder.Value;
                            }
                            else if (xmlRder.Name == "开仓阈值")
                            {
                                Property[1] = xmlRder.Value;
                            }
                            else if (xmlRder.Name == "最少挂单数")
                            {
                                Property[2] = xmlRder.Value;
                            }
                            else if (xmlRder.Name == "平仓点数")
                            {
                                Property[3] = xmlRder.Value;
                            }
                            else if (xmlRder.Name == "最大开仓数")
                            {
                                Property[4] = xmlRder.Value;
                            }
                            else if (xmlRder.Name == "标的物")
                            {
                                Property[5] = xmlRder.Value;
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
    }
}
