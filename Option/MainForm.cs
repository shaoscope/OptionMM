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
            //查持仓
            Thread.Sleep(500);
            TDManager.TD.ReqQryInvestorPosition();
            while (!TDManager.TD.bCanReq)
            {
                Thread.Sleep(50);
            }
            PositionList = TDManager.TD.positionList;
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

            //仓位对冲
            //this.positionHedgeTimer = new System.Threading.Timer(this.positionHedgeCallBack, null, 5 * 60 * 1000, 5 * 60 * 1000);

            this.positionHedgeTimer = new System.Threading.Timer(this.positionHedgeCallBack, null, 1 * 60 * 1000, Timeout.Infinite);

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

        private bool areAllRunning = false;

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
    }
}
