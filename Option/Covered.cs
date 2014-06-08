using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OptionMM
{
    class Covered : DataGridViewRow
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
        /// Position Volume
        /// </summary>
        private double positionVolume;

        /// <summary>
        /// 获取或者设置Position Volume
        /// </summary>
        public double PositionVolume
        {
            get { return this.positionVolume; }
            set { this.positionVolume = value; }
        }

        /// <summary>
        /// Covered Interval
        /// </summary>
        private double coveredInterval;

        /// <summary>
        /// 获取后者设置Covered Interval
        /// </summary>
        public double CoveredInterval
        {
            get { return this.coveredInterval; }
            set { this.coveredInterval = value; }
        }

        /// <summary>
        /// Average Price
        /// </summary>
        private double averagePrice;

        /// <summary>
        /// 获取或者设置Average Price
        /// </summary>
        public double AveragePrice
        {
            get { return this.averagePrice; }
            set { this.averagePrice = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Covered()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <param name="positionVolume"></param>
        /// <param name="coveredInterval"></param>
        /// <param name="averagePrice"></param>
        public Covered(string instrumentID, double positionVolume, double coveredInterval, double averagePrice)
        {
            this.instrumentID = instrumentID;
            this.positionVolume = positionVolume;
            this.coveredInterval = coveredInterval;
            this.averagePrice = averagePrice;
        }

        /// <summary>
        /// 所属面板控件
        /// </summary>
        private CoveredPanel coveredPanel;

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(CoveredPanel coveredPanel)
        {
            this.coveredPanel = coveredPanel;
        }

        /// <summary>
        /// 刷新策略行
        /// </summary>
        public void RefreshDataRow()
        {
            this.Cells["cPositionVolume"].Value = this.positionVolume;
            this.Cells["cCoveredInterval"].Value = this.coveredInterval;
            this.Cells["cAveragePrice"].Value = this.averagePrice;
        }

    }//class
}//namespace
