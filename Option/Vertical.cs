using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OptionMM
{
    class Vertical : DataGridViewRow
    {
        /// <summary>
        /// 看涨合约ID
        /// </summary>
        private string callInstrumentID;

        /// <summary>
        /// 获取或者设置合约看涨ID
        /// </summary>
        public string CallInstrumentID
        {
            get { return this.callInstrumentID; }
            set { this.callInstrumentID = value; }
        }

        /// <summary>
        /// 看涨合约开仓方向
        /// </summary>
        private EnumPosiDirectionType callDirection;

        /// <summary>
        /// 获取或者设置看涨合约开仓方向
        /// </summary>
        public EnumPosiDirectionType CallDirection
        {
            set { this.callDirection = value; }
            get { return this.callDirection; }
        }

        /// <summary>
        /// 看涨合约ID
        /// </summary>
        private string putInstrumentID;

        /// <summary>
        /// 获取或者设置合约看涨ID
        /// </summary>
        public string PutInstrumentID
        {
            get { return this.putInstrumentID; }
            set { this.putInstrumentID = value; }
        }

        /// <summary>
        /// 看跌合约开仓方向
        /// </summary>
        private EnumPosiDirectionType putDirection;

        /// <summary>
        /// 获取或者设置看跌合约开仓方向
        /// </summary>
        public EnumPosiDirectionType PutDirection
        {
            set { this.putDirection = value; }
            get { return this.putDirection; }
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
        /// Parity Interval
        /// </summary>
        private double parityInterval;

        /// <summary>
        /// 获取后者设置Parity Interval
        /// </summary>
        public double ParityInterval
        {
            get { return this.parityInterval; }
            set { this.parityInterval = value; }
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
        public Vertical()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="callInstrumentID"></param>
        /// <param name="positionVolume"></param>
        /// <param name="parityInterval"></param>
        /// <param name="averagePrice"></param>
        public Vertical(string callInstrumentID, EnumPosiDirectionType callDirection, string putInstrumentID, EnumPosiDirectionType putDirection, double parityInterval)
        {
            this.callInstrumentID = callInstrumentID;
            this.callDirection = callDirection;
            this.putInstrumentID = putInstrumentID;
            this.putDirection = putDirection;
            this.parityInterval = parityInterval;
        }

        /// <summary>
        /// 所属面板控件
        /// </summary>
        private VerticalPanel verticalPanel;

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(VerticalPanel verticalPanel)
        {
            this.verticalPanel = verticalPanel;
        }

        /// <summary>
        /// 刷新策略行
        /// </summary>
        public void RefreshDataRow()
        {
            this.Cells["cPositionVolume"].Value = this.positionVolume;
            this.Cells["cCoveredInterval"].Value = this.parityInterval;
            this.Cells["cAveragePrice"].Value = this.averagePrice;
        }


    }//class
}//namespace
