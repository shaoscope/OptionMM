﻿using CTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionMM
{
    /// <summary>
    /// 对冲计算类
    /// </summary>
    class PositionHedge
    {
        //对冲期货手数字典
        private Dictionary<string, double> futureHedgeVolume = new Dictionary<string, double>();

        /// <summary>
        /// 获取或者设置期货对冲手数字典
        /// </summary>
        public Dictionary<string, double> FutureHedgeVolume
        {
            get { return this.futureHedgeVolume; }
            set { this.futureHedgeVolume = value; }
        }


        private ThostFtdcOrderField IF1406PreviousOrder = null;
        private ThostFtdcOrderField IF1407PreviousOrder = null;

        //期权持仓列表
        private List<ThostFtdcInvestorPositionField> positionDictionary = new List<ThostFtdcInvestorPositionField>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="positionList"></param>
        /// <param name="dataTable"></param>
        public PositionHedge(List<ThostFtdcInvestorPositionField> positionList, DataGridView dataTable)
        {
            futureHedgeVolume.Add("IF1406", 0);
            futureHedgeVolume.Add("IF1407", 0);
            this.positionDictionary = positionList;
            foreach(DataGridViewRow dataRow in dataTable.Rows)
            {
                Strategy strategy = (Strategy)dataRow.Tag;
                foreach(ThostFtdcInvestorPositionField optionPosition in positionList)
                {
                    if(strategy.Option.InstrumentID == optionPosition.InstrumentID)
                    {
                        if (optionPosition.PosiDirection == EnumPosiDirectionType.Long && strategy.Option.OptionType == OptionTypeEnum.call)
                        {
                            futureHedgeVolume[strategy.Future.InstrumentID] += -strategy.Option.Delta * optionPosition.Position / 3;
                        }
                        else if (optionPosition.PosiDirection == EnumPosiDirectionType.Short && strategy.Option.OptionType == OptionTypeEnum.call)
                        {
                            futureHedgeVolume[strategy.Future.InstrumentID] += strategy.Option.Delta * optionPosition.Position / 3;
                        }
                        else if (optionPosition.PosiDirection == EnumPosiDirectionType.Long && strategy.Option.OptionType == OptionTypeEnum.put)
                        {
                            futureHedgeVolume[strategy.Future.InstrumentID] += -strategy.Option.Delta * optionPosition.Position / 3;
                        }
                        else if (optionPosition.PosiDirection == EnumPosiDirectionType.Short && strategy.Option.OptionType == OptionTypeEnum.put)
                        {
                            futureHedgeVolume[strategy.Future.InstrumentID] += strategy.Option.Delta * optionPosition.Position / 3;
                        }
                    }
                }
            }
            Console.WriteLine(System.DateTime.Now.ToShortTimeString() + ":" + "IF1406" + futureHedgeVolume["IF1406"] + "," + "IF1407" + futureHedgeVolume["IF1407"]);
        }

        /// <summary>
        /// 进行对冲下单
        /// </summary>
        public void DoHedge()
        {
            TDManager.TD.OnTrading += TD_OnTrading;
            TDManager.TD.OnTraded += TD_OnTraded;
            TDManager.TD.OnCanceled += TD_OnCanceled;
            foreach(string instrumentID in futureHedgeVolume.Keys)
            {
                int hedgeVolume = (int)(futureHedgeVolume[instrumentID] > 0 ? Math.Floor(futureHedgeVolume[instrumentID]) : Math.Ceiling(futureHedgeVolume[instrumentID]));
                if(hedgeVolume > 0)
                {
                    //市价单
                    //TDManager.TD.Buy(instrumentID, hedgeVolume, "市场价");
                }
                else if(hedgeVolume < 0)
                {
                    //TDManager.TD.SellShort(instrumentID, Math.Abs(hedgeVolume), "市场价");
                }
            }
        }

        void TD_OnCanceled(ThostFtdcOrderField pOrder)
        {
            
        }

        /// <summary>
        /// 报单全部成交反馈
        /// </summary>
        /// <param name="pOrder"></param>
        void TD_OnTraded(ThostFtdcOrderField pOrder)
        {
            if (pOrder.InstrumentID == "1406")
            {

            }
            else if (pOrder.InstrumentID == "1407")
            {

            }
        }

        /// <summary>
        /// 有报单成交反馈
        /// </summary>
        /// <param name="pOrder"></param>
        void TD_OnTrading(ThostFtdcOrderField pOrder)
        {
            if(pOrder.InstrumentID == "1406")
            {

            }
            else if(pOrder.InstrumentID == "1407")
            {

            }
        }

    }//class
}//namespace