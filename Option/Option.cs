using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionMM
{
    class Option : DataGridViewRow
    {

        public OptionValue optionValue; //动态理论值

        private OptionProperties optionProperties;   //属性

        public MMQuotation mmQuotation;

        public OptionProperties OptionProperties
        {
            get
            {
                if (underlyingPrice != 0)
                {
                    return new OptionProperties(optionType, underlyingPrice, strikePrice, GlobalValues.InterestRate, GlobalValues.Volatility, GlobalValues.DaysToMaturity);
                }
                else return null;
            }
        }


        public string instrumentID;

        public double price;    //当前价格

        public OptionTypeEnum optionType;

        public double strikePrice;

        public double underlyingPrice = 0;  //标的物当前价格

        public double impridVolatility; //隐含波动率

        public double optionPositionThreshold;  //开仓阈值

        public double minOptionOpenLots;    //最小开仓数

        public double maxOptionOpenLots;    //最大开仓数

        public string underlyingInstrumentID;   //标的物

        public Option()
        {
            optionValue = new OptionValue();
            optionProperties = new OptionProperties();
            mmQuotation = new MMQuotation();
        }

        //所属面板控件
        private OptionPanel optionPanel;

        /// <summary>
        /// 设置对冲明细应该显示的面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(OptionPanel optionPanel)
        {
            this.optionPanel = optionPanel;
        }

    }
}
