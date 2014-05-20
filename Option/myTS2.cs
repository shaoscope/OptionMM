using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Data;

namespace OptionMM
{
    class myTS2 : TSBase
    {
        Contract[] Contracts = null;

        public myTS2(string TSName)
            : base(TSName)
        {

        }

        public void SetContracts(Contract[] contracts)
        {
            Contracts = contracts;
            Contracts[0].OnTick += new TickHandle(OnUnderlyingTick);
            //Contracts[1].OnTick += new TickHandle(OnUnderlyingTick);
            Contracts[0].OnCanceled += new OrderHandle(OnUnderlyingCanceled);
            //Contracts[1].OnCanceled += new OrderHandle(OnUnderlyingCanceled);
            Contracts[0].OnTraded += new OrderHandle(OnUnderlyingTraded);
            //Contracts[1].OnTraded += new OrderHandle(OnUnderlyingTraded);
            Contracts[0].OnTrading += new OrderHandle(OnUnderlyingTrading);
            //Contracts[1].OnTrading += new OrderHandle(OnUnderlyingTrading);
        }

        protected override void TSInit()
        {
            
        }

        public override void Run()
        {
            string[] inst = new string[1] { Contracts[0].option.underlyingInstrumentID };
            Contracts[0].SubMD(inst);
            inst = new string[1] { Contracts[0].option.instrumentID };
            Contracts[0].SubMD(inst);
            bRun = true;
        }

        public override void Stop()
        {
            bRun = false;
        }

        protected override void InitFromXML(string strFile)
        {

        }

        int volumeMutiDiff = 3;
        int volumeThreshold = 10;
        void OnUnderlyingTick(ThostFtdcDepthMarketDataField md)
        {
            if (bRun)
            {
                //CallPut 分开算
                List<Contract> optionContracts = ContractManager.GetContract(md.InstrumentID);
                //本期货对应的期权合约的净对冲头寸
                double netLots = 0;
                foreach (Contract cont in optionContracts)
                {
                    //实时计算BS
                    cont.option.underlyingPrice = md.LastPrice;
                    cont.option.optionValue = OptionPricingModel.EuropeanBS(cont.option.OptionProperties);
                    netLots = netLots - cont.option.optionValue.Delta * (cont.LongPosition - cont.ShortPosition);
                    //netLots = netLots + cont.LongPosition - cont.ShortPosition;
                }
                //头寸对冲
                int inputedLots = Contracts[0].LongInputLots - Contracts[0].ShortInputLots;   //目前已经报单的期货头寸
                int needInputLots = (int)(netLots / volumeMutiDiff);
                int thisImputLost = needInputLots - inputedLots;

                if (Math.Abs(thisImputLost) >= volumeThreshold)
                {
                    //买开或卖平
                    if(Contracts[0].ShortPosition >= needInputLots)
                    {
                        Contracts[0].BuyToCover(md.InstrumentID, needInputLots, md.AskPrice1);
                    }
                    else if (Contracts[0].ShortPosition >= 0)
                    {
                        Contracts[0].BuyToCover(md.InstrumentID, Contracts[0].ShortPosition, md.AskPrice1);
                        Contracts[0].Buy(md.InstrumentID, needInputLots - Contracts[0].ShortPosition, md.AskPrice1);
                    }
                    else
                    {
                        Contracts[0].Buy(md.InstrumentID, needInputLots, md.AskPrice1);
                    }
                }
                else if (needInputLots < 0)
                {
                    //卖开或买平
                    if (Contracts[0].LongPosition >= needInputLots)
                    {
                        Contracts[0].Sell(md.InstrumentID, needInputLots, md.BidPrice1);
                    }
                    else if (Contracts[0].LongPosition >= 0)
                    {
                        Contracts[0].Sell(md.InstrumentID, Contracts[0].LongPosition, md.BidPrice1);
                        Contracts[0].SellShort(md.InstrumentID, needInputLots - Contracts[0].LongPosition, md.BidPrice1);
                    }
                    else
                    {
                        Contracts[0].SellShort(md.InstrumentID, needInputLots, md.BidPrice1);
                    }
                }
            }
        }

        void OnUnderlyingCanceled(ThostFtdcOrderField order)
        {
            //CurrentOptionOrder = order;
            if (order.CombOffsetFlag_0 == EnumOffsetFlagType.Open && order.Direction == EnumDirectionType.Buy)
            {
                //买开
                Contracts[0].AddLongInputLots(-1 * order.VolumeTotal);
            }
            else if (order.CombOffsetFlag_0 == EnumOffsetFlagType.Open && order.Direction == EnumDirectionType.Sell)
            {
                //卖开
                Contracts[0].AddShortInputLots(-1 * order.VolumeTotal);
            }
            else if (order.CombOffsetFlag_0 == EnumOffsetFlagType.CloseToday && order.Direction == EnumDirectionType.Buy)
            {
                //买平
                int nonTraded = order.VolumeTotal;
                Contracts[0].AddShortInputLots(order.VolumeTotal);
            }
            else if (order.CombOffsetFlag_0 == EnumOffsetFlagType.CloseToday && order.Direction == EnumDirectionType.Sell)
            {
                //买平
                Contracts[0].AddLongInputLots(order.VolumeTotal);
            }
        }


        void OnUnderlyingTrading(ThostFtdcOrderField order)
        {
            if (Contracts[0].BidOptionOrderRef == order.OrderRef)
            {
                Contracts[0].CurrentBidOptionOrder = order;
            }
            else if (Contracts[0].AskOptionOrderRef == order.OrderRef)
            {
                Contracts[0].CurrentAskOptionOrder = order;
            }
        }

        void OnUnderlyingTraded(ThostFtdcOrderField order)
        {

        }

    }
}
