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
    class myTS1 : TSBase
    {
        Contract[] Contracts = null;


        public myTS1(string TSName)
            : base(TSName)
        {
            //初始化合约contracts
        }

        public void SetContracts(Contract[] contracts)
        {
            Contracts = contracts;
            Contracts[0].OnTick += new TickHandle(OnOptionTick);
            //Contracts[1].OnTick += new TickHandle(OnUnderlyingTick);
            Contracts[0].OnCanceled += new OrderHandle(OnOptionCanceled);
            //Contracts[1].OnCanceled += new OrderHandle(OnUnderlyingCanceled);
            Contracts[0].OnTraded += new OrderHandle(OnOptionTraded);
            //Contracts[1].OnTraded += new OrderHandle(OnUnderlyingTraded);
            Contracts[0].OnTrading += new OrderHandle(OnOptionTrading);
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

        void OnOptionTick(ThostFtdcDepthMarketDataField md)
        {
            if(bRun)
            {
                Contracts[0].option.price = md.LastPrice;
                if (Contracts[0].option.OptionProperties != null)
                {
                    //计算出的做市价格。
                    double MMBid = 0;
                    double MMAsk = 0;
                    //double kcyz = Contracts[0].option
                    //根据理论价格和实际价格报价
                    //Contracts[0].Buy(Contracts[0].option.instrumentID, );

                    if (Contracts[0].option.optionValue.Price - Contracts[0].option.price > Contracts[0].option.optionPositionThreshold)
                    {
                        ////计算开仓手数 股指手数定义 期权手数 = 1/隐含波动率 向上取整 * 3, 买入期权，//卖出股指。
                        //报10手期权
                        MMBid = Math.Round(md.BidPrice1 - 0.1, 2);
                        MMAsk = MMPrice.GetAskPriceThisMonth(MMBid);
                    }
                    else if (Contracts[0].option.price - Contracts[0].option.optionValue.Price > Contracts[0].option.optionPositionThreshold)
                    {
                        //double MMBid = Math.Round(Contracts[0].option.price, 2);
                        MMAsk = Math.Round(md.AskPrice1 + 0.1, 2);
                        if (MMAsk < 0.6)
                        {
                            MMAsk = 0.6;
                        }
                        MMBid = MMPrice.GetBidPriceThisMonth(MMAsk);
                    }
                    Contracts[0].option.mmQuotation.AskPrice = MMAsk;
                    Contracts[0].option.mmQuotation.BidPrice = MMBid;
                    Contracts[0].option.mmQuotation.AskLots = 10;
                    Contracts[0].option.mmQuotation.BidLots = 10;
                    //判断是否撤单
                    if (Contracts[0].CurrentBidOptionOrder != null && (Contracts[0].option.mmQuotation.BidPrice != Contracts[0].CurrentBidOptionOrder.LimitPrice || Contracts[0].CurrentBidOptionOrder.VolumeTraded > 0))
                    {
                        //需要重新报单
                        if (Contracts[0].CurrentBidOptionOrder.VolumeTraded != Contracts[0].CurrentBidOptionOrder.VolumeTotalOriginal)
                        {
                            Contracts[0].CancelOrder(Contracts[0].BidOptionOrderRef);
                        }
                        Contracts[0].BidOptionOrderRef = null;
                        Contracts[0].CurrentBidOptionOrder = null;
                    }
                    if (Contracts[0].CurrentAskOptionOrder != null && (Contracts[0].option.mmQuotation.AskPrice != Contracts[0].CurrentAskOptionOrder.LimitPrice || Contracts[0].CurrentAskOptionOrder.VolumeTraded > 0))
                    {
                        if (Contracts[0].CurrentAskOptionOrder.VolumeTraded != Contracts[0].CurrentAskOptionOrder.VolumeTotalOriginal)
                        {
                            Contracts[0].CancelOrder(Contracts[0].AskOptionOrderRef);
                        }
                        Contracts[0].AskOptionOrderRef = null;
                        Contracts[0].CurrentAskOptionOrder = null;
                    }
                    //没有报出报单
                    if (Contracts[0].BidOptionOrderRef == null)
                    {
                        Contracts[0].BidOptionOrderRef = Contracts[0].Buy(Contracts[0].option.instrumentID, Contracts[0].option.mmQuotation.BidLots, Contracts[0].option.mmQuotation.BidPrice);
                    }
                    if (Contracts[0].AskOptionOrderRef == null)
                    {
                        Contracts[0].AskOptionOrderRef = Contracts[0].SellShort(Contracts[0].option.instrumentID, Contracts[0].option.mmQuotation.AskLots, Contracts[0].option.mmQuotation.AskPrice);
                    }
                }
            }
        }

        void OnUnderlyingTick(ThostFtdcDepthMarketDataField md)
        {
            if (bRun)
            {
                Contracts[0].option.underlyingPrice = md.LastPrice;
                Contracts[0].option.optionValue = OptionPricingModel.EuropeanBS(Contracts[0].option.OptionProperties);
            }
        }

        void OnOptionCanceled(ThostFtdcOrderField order)
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
        void OnUnderlyingCanceled(ThostFtdcOrderField order)
        {
            //CurrentOptionOrder = order;
        }

        void OnOptionTrading(ThostFtdcOrderField order)
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

        void OnUnderlyingTrading(ThostFtdcOrderField order)
        {

        }

        void OnOptionTraded(ThostFtdcOrderField order)
        {

        }
        void OnUnderlyingTraded(ThostFtdcOrderField order)
        {

        }

        protected override void InitFromXML(string strFile)
        {
            
        }
    }
}
