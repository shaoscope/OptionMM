using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    /// <summary>
    /// 行情管理器
    /// </summary>
    class MarketManager
    {
        /// <summary>
        /// 活跃合约字典
        /// </summary>
        public Dictionary<string, ActiveContract> ActiveContractDictionary { get; private set; }

        /// <summary>
        /// 行情接口
        /// </summary>
        public CTPMDAdapter marketer { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MarketManager(CTPMDAdapter marketer)
        {
            ActiveContractDictionary = new Dictionary<string, ActiveContract>();
            this.marketer = marketer;
        }

        /// <summary>
        /// 加入活跃合约
        /// </summary>
        public void AddActiveContract(ActiveContract activeContract)
        {
            ActiveContractDictionary.Add(activeContract.Contract.InstrumentID, activeContract);
            marketer.SubscribeMarketData(new string[] { activeContract.Contract.InstrumentID });

        }
    }
}
