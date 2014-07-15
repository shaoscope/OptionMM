using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    class ActiveContract
    {
        /// <summary>
        /// 行情更新时该事件被触发
        /// </summary>
        public event EventHandler MarketUpdated;

        /// <summary>
        /// 合约信息
        /// </summary>
        public ThostFtdcInstrumentField Contract { get; private set; }

        /// <summary>
        /// 上一刻行情
        /// </summary>
        public ThostFtdcDepthMarketDataField PreMarketData { get; private set; }

        /// <summary>
        /// 是否自选锁定的标志
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// 最新的行情数据
        /// </summary>
        public ThostFtdcDepthMarketDataField MarketData { get; private set; }

        public ActiveContract(ThostFtdcInstrumentField contract)
        {
            this.Contract = contract;
        }

        /// <summary>
        /// 更新行情信息
        /// </summary>
        public void UpdateMarket(ThostFtdcDepthMarketDataField MarketData)
        {
            this.PreMarketData = this.MarketData;
            this.MarketData = MarketData;
            if (this.MarketUpdated != null)
            {
                this.MarketUpdated(this, EventArgs.Empty);
            }
        }


    }//class
}
