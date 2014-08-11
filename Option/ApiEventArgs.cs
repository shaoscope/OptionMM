using CTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OptionMM
{
    /// <summary>
    /// 报单成交事件的委托
    /// </summary>
    /// <param name="sender">事件发送者</param>
    /// <param name="e">事件的数据</param>
    //public delegate void RtnOrderEventHandler(object sender, RtnOrderEventArgs e);

    /// <summary>
    /// 时间委托泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EventHandler<T>(object sender, T e);

    /// <summary>
    /// 报单成交时间的数据
    /// </summary>
    public class RtnOrderEventArgs : EventArgs
    {
        //报单信息
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ThostFtdcOrderField orderField;
        //关联数据
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object tag;

        /// <summary>
        /// 获取报单信息
        /// </summary>
        public ThostFtdcOrderField OrderFiled
        {
            get { return this.orderField; }
            protected set { this.orderField = value; }
        }

        /// <summary>
        /// 获取关联数据
        /// </summary>
        public object Tag
        {
            get { return this.tag; }
            protected set { this.tag = value; }
        }

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        protected RtnOrderEventArgs()
        {
        }

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        /// <param name="MarketData">报单信息</param>
        public RtnOrderEventArgs(ThostFtdcOrderField orderField)
        {
            this.orderField = orderField;
        }

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        /// <param name="MarketData">报单信息</param>
        /// <param name="Tag">关联数据</param>
        public RtnOrderEventArgs(ThostFtdcOrderField orderField, object Tag)
        {
            this.orderField = orderField;
            this.tag = Tag;
        }
    }//class

    /// <summary>
    /// 报单成交时间的数据
    /// </summary>
    public class RtnTradeEventArgs : EventArgs
    {
        //报单信息
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ThostFtdcTradeField tradeField;
        //关联数据
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object tag;

        /// <summary>
        /// 获取报单信息
        /// </summary>
        public ThostFtdcTradeField TradeFiled
        {
            get { return this.tradeField; }
            protected set { this.tradeField = value; }
        }

        /// <summary>
        /// 获取关联数据
        /// </summary>
        public object Tag
        {
            get { return this.tag; }
            protected set { this.tag = value; }
        }

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        protected RtnTradeEventArgs()
        {
        }

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        /// <param name="MarketData">报单信息</param>
        public RtnTradeEventArgs(ThostFtdcTradeField tradeField)
        {
            this.tradeField = tradeField;
        }

        /// <summary>
        /// 构造一个新实例
        /// </summary>
        /// <param name="MarketData">报单信息</param>
        /// <param name="Tag">关联数据</param>
        public RtnTradeEventArgs(ThostFtdcTradeField tradeField, object Tag)
        {
            this.tradeField = tradeField;
            this.tag = Tag;
        }
    }//class

}
