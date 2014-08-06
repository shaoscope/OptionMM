using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OptionMM
{
    /// <summary>
    /// 显示波动率窗口
    /// </summary>
    public partial class VolatilityForm : DockContent
    {
        /// <summary>
        /// 刷新图表定时器
        /// </summary>
        private System.Threading.Timer RefreshChartTimer;


        public VolatilityForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载完毕时发生
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshChartTimer = new System.Threading.Timer(this.RefreshChartCallBack, null, 1 * 1000, 10 * 1000);

        }

        /// <summary>
        /// 刷新图表回调函数
        /// </summary>
        /// <param name="state"></param>
        private void RefreshChartCallBack(object state)
        {
            List<double[]> volatilityList = new List<double[]>();
            foreach(Quote quote in MainForm.Instance.QuoteForm.quotePanel.dataTable.Rows)
            {
                if(quote.call.Contract.InstrumentID.StartsWith("IO1408"))
                {
                    volatilityList.Add(new double[] { quote.call.StrikePrice, quote.call.ImpliedVolatility });
                }
            }
            this.BeginInvoke(new Action<List<double[]>>(this.RefreshChart), volatilityList);
        }

        /// <summary>
        /// 刷新图表函数
        /// </summary>
        /// <param name="volatilityList"></param>
        private void RefreshChart(List<double[]> volatilityList)
        {
            double []ArrayX = new double[volatilityList.Count];
            double []ArrayY = new double[volatilityList.Count];
            for(int i = 0; i < volatilityList.Count; i++)
            {
                ArrayX[i] = volatilityList[i][0];
                ArrayY[i] = volatilityList[i][1];
            }
            MatrixEquation matrixEquation = new MatrixEquation(ArrayX, ArrayY, 2);
            double []coefficient = matrixEquation.GetResult();
            this.volatilityChart.Series[0].Points.Clear();
            this.volatilityChart.Series[1].Points.Clear();
            for(int i = 0; i < ArrayX.Length; i++)
            {
                this.volatilityChart.Series[0].Points.AddXY(ArrayX[i], ArrayY[i]);
                double fitY = coefficient[0] + coefficient[1] * ArrayX[i] + coefficient[2] * ArrayX[i] * ArrayX[i];
                this.volatilityChart.Series[1].Points.AddXY(ArrayX[i], fitY);
            }
        }



    }//class
}//namespace
