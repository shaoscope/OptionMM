using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OptionMM
{
    class Greeks : DataGridViewRow
    {
        /// <summary>
        /// 合约ID
        /// </summary>
        private string instrumentID;

        /// <summary>
        /// 获取或者设置合约ID
        /// </summary>
        public string InstrumentID
        {
            get { return this.instrumentID; }
            set { this.instrumentID = value; }
        }

        /// <summary>
        /// Delta
        /// </summary>
        private double delta;

        /// <summary>
        /// 获取或者设置Delta
        /// </summary>
        public double Delta
        {
            get { return this.delta; }
            set { this.delta = value; }
        }

        /// <summary>
        /// Gamma
        /// </summary>
        private double gamma;

        /// <summary>
        /// 获取后者设置Gamma
        /// </summary>
        public double Gamma
        {
            get { return this.gamma; }
            set { this.gamma = value; }
        }

        /// <summary>
        /// Vega
        /// </summary>
        private double vega;

        /// <summary>
        /// 获取或者设置Vega
        /// </summary>
        public double Vega
        {
            get { return this.vega; }
            set { this.vega = value; }
        }

        /// <summary>
        /// Theta
        /// </summary>
        private double theta;

        /// <summary>
        /// 获取或者设置Theta
        /// </summary>
        public double Theta
        {
            get { return this.theta; }
            set { this.theta = value; }
        }

        /// <summary>
        /// Rho
        /// </summary>
        private double rho;

        /// <summary>
        /// 获取或者设置Vega
        /// </summary>
        public double Rho
        {
            get { return this.rho; }
            set { this.rho = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Greeks()
        {

        }

        /// <summary>
        /// 所属面板控件
        /// </summary>
        private GreeksPanel greeksPanel;

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(GreeksPanel greeksPanel)
        {
            this.greeksPanel = greeksPanel;
        }

        /// <summary>
        /// 刷新策略行
        /// </summary>
        public void RefreshDataRow()
        {
            this.Cells["cDelta"].Value = this.delta;
            this.Cells["cGamma"].Value = this.gamma;
            this.Cells["cVega"].Value = this.vega;
            this.Cells["cTheta"].Value = this.theta;
            this.Cells["cRho"].Value = this.rho;
        }


    }//class
}//namespace
