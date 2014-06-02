using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace OptionMM
{
    class Position
    {
        /// <summary>
        /// 仓位多头信息
        /// </summary>
        public ThostFtdcInvestorPositionField longPosition = new ThostFtdcInvestorPositionField();
        /// <summary>
        /// 仓位空头信息
        /// </summary>
        public ThostFtdcInvestorPositionField shortPosition = new ThostFtdcInvestorPositionField();

        /// <summary>
        /// 上一次返回回来的成交单
        /// </summary>
        public ThostFtdcOrderField previousOrder = null;

        public ThostFtdcOrderField ShortOptionOrder = null;
        public ThostFtdcOrderField LongOptionOrder = null;
        public ThostFtdcOrderField CloseLongOptionOrder = null;
        public ThostFtdcOrderField CloseShortOptionOrder = null;
        public string PlaceShortOptionOrderRef = null;
        public string PlaceLongOptionOrderRef = null;
        public string CloseLongOptionOrderRef = null;
        public string CloseShortOptionOrderRef = null;
        private object LongInputLotsLock = new object();
        private object LongPositionLock = new object();
        private object ShortInputLotsLock = new object();
        private object ShortPositionLock = new object();

        public Position()
        {

        }

        /// <summary>
        /// 清空保单信息
        /// </summary>
        public void clearOrderInfo()
        {
            ShortOptionOrder = null;
            LongOptionOrder = null;
            CloseLongOptionOrder = null;
            CloseShortOptionOrder = null;
            PlaceShortOptionOrderRef = null;
            PlaceLongOptionOrderRef = null;
            CloseLongOptionOrderRef = null;
            CloseShortOptionOrderRef = null;
        }
    }
}
