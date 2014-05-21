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

        private List<Strategy> optionList = new List<Strategy>();
        public MainForm()
        {
            InitializeComponent();
        }

        private Dictionary<string, string[]> configValues = new Dictionary<string, string[]>();
        
        //同步对象
        private readonly object gSyncRoot = new object();

        private void MainForm_Load(object sender, EventArgs e)
        {
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
                this.optionPanel.AddOption(strategy);
                optionList.Add(strategy);
            }
            
            //从optionPanel中初始化策略实例
            for (int i = 0; i < optionPanel.dataTable.Rows.Count; i++)
            {
                Strategy op = (Strategy)optionPanel.dataTable.Rows[i];
                ContractManager.CreatContract(op);
            }
            //生成策略实例(期权）
            for (int i = 0; i < optionPanel.dataTable.Rows.Count; i++)
            {
                CTPEvents[] contracts = new CTPEvents[1] { ContractManager.GetContract(i + 1 + UnderlyingCount) };
                myTS1 ts = new myTS1(contracts[0].stragety.instrumentID);
                ts.SetContracts(contracts);
                tsList.Add((TSBase)ts);
            }
            //生成策略实例(期货）
            for (int i = 0; i < UnderlyingCount; i++)
            {
                CTPEvents[] contracts = new CTPEvents[1] { ContractManager.GetContract(i + 1) };
                myTS2 ts = new myTS2(contracts[0].instrument.InstrumentID);
                ts.SetContracts(contracts);
                tsList.Add((TSBase)ts);
                ts.Run();
            }
        }

        int UnderlyingCount = 0;
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
                                //ContractManager.CreatContract(InstrumentManager.GetInstrument(xmlRder.Value));
                            }
                        }
                        configValues.Add(strID, Property);
                    }
                }
                //每个标的物只生成一个CONTRACT
                Dictionary<string, Instrument> instrumentTemp = InstrumentManager.GetAllInstrument();
                UnderlyingCount = instrumentTemp.Count;
                foreach (Instrument inst in instrumentTemp.Values)
                {
                    ContractManager.CreatContract(inst);
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
