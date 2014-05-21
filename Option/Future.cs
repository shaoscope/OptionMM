using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class Future : CTPEvents
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
    }
}
