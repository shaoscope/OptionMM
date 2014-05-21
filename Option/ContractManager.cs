using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class ContractManager
    {
        static Dictionary<int, CTPEvents> ContractDictionary = new Dictionary<int, CTPEvents>();
        static Dictionary<string, List<CTPEvents>> UnderlyingMap = new Dictionary<string, List<CTPEvents>>();

        public static void CreatContract(Strategy op)
        {
            try
            {
                CTPEvents contract = new CTPEvents(op);
                ContractDictionary.Add(ContractDictionary.Count + 1, contract);
                if(UnderlyingMap.Keys.Contains(op.underlyingInstrumentID))
                {
                    UnderlyingMap[op.underlyingInstrumentID].Add(contract);
                }
                else
                {
                    List<CTPEvents> contractList = new List<CTPEvents>();
                    contractList.Add(contract);
                    UnderlyingMap.Add(op.underlyingInstrumentID, contractList);
                }
            }
            catch
            {

            }
        }

        public static void CreatContract(Instrument instrument)
        {
            try
            {
                CTPEvents contract = new CTPEvents(instrument);
                ContractDictionary.Add(ContractDictionary.Count + 1, contract);
            }
            catch
            {

            }
        }

        public static CTPEvents GetContract(int i)
        {
            return ContractDictionary[i];
        }

        public static List<CTPEvents> GetContract(string strUnderlyingID)
        {
            return UnderlyingMap[strUnderlyingID];
        }

        public static void Init()
        {
            ContractDictionary.Clear();
        }
    }
}
