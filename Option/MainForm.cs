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
        public static List<TSBase> tsList = new List<TSBase>();

        private List<Option> optionList = new List<Option>();
        public MainForm()
        {
            InitializeComponent();
        }

        private Dictionary<string, string[]> configValues = new Dictionary<string, string[]>();

        private System.Threading.Timer flushTimer;

        //同步对象
        private readonly object gSyncRoot = new object();

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitFromXML("Option.xml");
            foreach (string instrumentID in configValues.Keys)
            {
                Option option = new Option();
                option.instrumentID = instrumentID;
                string[] strTemp = instrumentID.Split('-');
                option.strikePrice = double.Parse(strTemp[2]);
                option.optionType = strTemp[1] == "C" ? OptionTypeEnum.call : OptionTypeEnum.put;
                option.impridVolatility = Double.Parse(configValues[instrumentID][0]);
                option.optionPositionThreshold = Double.Parse(configValues[instrumentID][1]);
                option.minOptionOpenLots = Double.Parse(configValues[instrumentID][2]);
                //option.maxOptionOpenLots = Double.Parse(configValues[instrumentID][3]);   //开仓点数
                option.maxOptionOpenLots = Double.Parse(configValues[instrumentID][4]);
                option.underlyingInstrumentID = configValues[instrumentID][5];
                this.optionPanel.AddOption(option);
                optionList.Add(option);
            }
            
            //从optionPanel中初始化策略实例
            for (int i = 0; i < optionPanel.dataTable.Rows.Count; i++)
            {
                Option op = (Option)optionPanel.dataTable.Rows[i];
                ContractManager.CreatContract(op);
            }
            //生成策略实例
            for (int i = 0; i < optionPanel.dataTable.Rows.Count; i++)
            {
                Contract[] contracts = new Contract[2]{ ContractManager.GetContract(i + 1 + optionPanel.dataTable.Rows.Count), ContractManager.GetContract(i + 1)};
                myTS1 ts = new myTS1(contracts[0].option.instrumentID);
                ts.SetContracts(contracts);
                tsList.Add((TSBase)ts);
            }
            this.flushTimer = new System.Threading.Timer(this.flushTimerCallback, null, 1000, 1000);
        }

        /// <summary>
        /// 刷新定时器回调
        /// </summary>
        private void flushTimerCallback(object state)
        {
            try
            {
                lock (this.gSyncRoot)
                {
                    if (!this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(this.RefreshMainForm));
                    }
                }
            }
            catch
            { }
        }

        //刷新主面板
        public void RefreshMainForm()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                optionPanel.dataTable.Rows[i].Cells["cDelta"].Value = optionList[i].optionValue.Delta;
                optionPanel.dataTable.Rows[i].Cells["cTheroricalPrice"].Value = optionList[i].optionValue.Price;
                optionPanel.dataTable.Rows[i].Cells["cRealPrice"].Value = optionList[i].price;
                optionPanel.dataTable.Rows[i].Cells["cAskPrice"].Value = optionList[i].mmQuotation.AskPrice;
                optionPanel.dataTable.Rows[i].Cells["cBidPrice"].Value = optionList[i].mmQuotation.BidPrice;
            }
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
                                InstrumentManager.CreatInstrument(xmlRder.Value);
                                ContractManager.CreatContract(InstrumentManager.GetInstrument(xmlRder.Value));
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

        private void optionPanel_DoubleClick(object sender, EventArgs e)
        {
            //启动停止策略
            //例：tsList[0].bRun = false;
        }
    }
}
