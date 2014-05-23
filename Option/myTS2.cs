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
    class myTS2
    {

        int volumeMutiDiff = 3;
        int volumeThreshold = 10;
        void OnUnderlyingTick(ThostFtdcDepthMarketDataField md)
        {
            //if (bRun)
            //{
            //    //CallPut 分开算
            //    List<CTPEvents> optionContracts = ContractManager.GetContract(md.InstrumentID);
            //    //本期货对应的期权合约的净对冲头寸
            //    double netLots = 0;
            //    foreach (CTPEvents cont in optionContracts)
            //    {
            //        //实时计算BS
            //        cont.stragety.underlyingPrice = md.LastPrice;
            //        cont.stragety.optionValue = OptionPricingModel.EuropeanBS(cont.stragety.OptionProperties);
            //        netLots = netLots - cont.stragety.optionValue.Delta * (cont.LongPosition - cont.ShortPosition);
            //    }
            //    //头寸对冲
            //    int inputedLots = Contracts[0].LongInputLots - Contracts[0].ShortInputLots;   //目前已经报单的期货头寸
            //    int needInputLots = (int)(netLots / volumeMutiDiff);
            //    int thisImputLost = needInputLots - inputedLots;

            //    if (Math.Abs(thisImputLost) >= volumeThreshold)
            //    {
            //        //买开或卖平
            //        if(Contracts[0].ShortPosition >= needInputLots)
            //        {
            //            Contracts[0].BuyToCover(md.InstrumentID, needInputLots, md.AskPrice1);
            //        }
            //        else if (Contracts[0].ShortPosition >= 0)
            //        {
            //            Contracts[0].BuyToCover(md.InstrumentID, Contracts[0].ShortPosition, md.AskPrice1);
            //            Contracts[0].Buy(md.InstrumentID, needInputLots - Contracts[0].ShortPosition, md.AskPrice1);
            //        }
            //        else
            //        {
            //            Contracts[0].Buy(md.InstrumentID, needInputLots, md.AskPrice1);
            //        }
            //    }
            //    else if (needInputLots < 0)
            //    {
            //        //卖开或买平
            //        if (Contracts[0].LongPosition >= needInputLots)
            //        {
            //            Contracts[0].Sell(md.InstrumentID, needInputLots, md.BidPrice1);
            //        }
            //        else if (Contracts[0].LongPosition >= 0)
            //        {
            //            Contracts[0].Sell(md.InstrumentID, Contracts[0].LongPosition, md.BidPrice1);
            //            Contracts[0].SellShort(md.InstrumentID, needInputLots - Contracts[0].LongPosition, md.BidPrice1);
            //        }
            //        else
            //        {
            //            Contracts[0].SellShort(md.InstrumentID, needInputLots, md.BidPrice1);
            //        }
            //    }
            //}
        }


        void OnUnderlyingTraded(ThostFtdcOrderField order)
        {

        }
    }
}
