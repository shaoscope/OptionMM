using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class Future : Position
    {
        /// <summary>
        /// 期货合约编码
        /// </summary>
        private string instrumentID;

        /// <summary>
        /// 获取或者设置期货合约编码
        /// </summary>
        public string InstrumentID
        {
            get { return this.instrumentID; }
            set { this.instrumentID = value; }
        }

        /// <summary>
        /// 合约最后行情
        /// </summary>
        private ThostFtdcDepthMarketDataField lastMarket;

        /// <summary>
        /// 获取或者设置合约最后行情
        /// </summary>
        public ThostFtdcDepthMarketDataField LastMarket
        {
            get { return this.lastMarket; }
            set { this.lastMarket = value; }
        }

 
    }
}
