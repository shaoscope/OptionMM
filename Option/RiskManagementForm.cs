using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionMM
{
    /// <summary>
    /// 风险管理窗口
    /// </summary>
    public partial class RiskManagementForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RiskManagementForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 风险管理窗口加载时的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiskManagementForm_Load(object sender, EventArgs e)
        {
            //List<string> instrumentIDList = new List<string>();
            //instrumentIDList.Add("IO1407");
            //instrumentIDList.Add("IO1408");
            //instrumentIDList.Add("IO1409");
            //instrumentIDList.Add("IO1412");
            //instrumentIDList.Add("IO1503");
            ////instrumentIDList.Add("IOALL");
            //foreach(string instrumentID in instrumentIDList)
            //{
            //    Greeks greeks = new Greeks();
            //    greeks.InstrumentID = instrumentID;
            //    foreach (DataGridViewRow dataRow in MainForm.Instance.optionPanel.dataTable.Rows)
            //    {
            //        Strategy strategy = (Strategy)dataRow.Tag;
            //        if(strategy.Option.InstrumentID.StartsWith(instrumentID))
            //        {
            //            greeks.Delta += strategy.Option.OptionValue.Delta * strategy.Option.longPosition.Position;
            //            greeks.Delta += -strategy.Option.OptionValue.Delta * strategy.Option.shortPosition.Position;
            //            greeks.Gamma += strategy.Option.OptionValue.Gamma * strategy.Option.longPosition.Position;
            //            greeks.Gamma += -strategy.Option.OptionValue.Gamma * strategy.Option.shortPosition.Position;
            //            greeks.Vega += strategy.Option.OptionValue.Vega * strategy.Option.longPosition.Position;
            //            greeks.Vega += -strategy.Option.OptionValue.Vega * strategy.Option.shortPosition.Position;
            //            greeks.Theta += strategy.Option.OptionValue.Theta * strategy.Option.longPosition.Position;
            //            greeks.Theta += -strategy.Option.OptionValue.Theta * strategy.Option.shortPosition.Position;
            //            greeks.Rho += strategy.Option.OptionValue.Rho * strategy.Option.longPosition.Position;
            //            greeks.Rho += -strategy.Option.OptionValue.Rho * strategy.Option.shortPosition.Position;
            //        }
            //    }
            //    this.greeksPanel.AddGreeks(greeks);
            //}
        }
    }
}
