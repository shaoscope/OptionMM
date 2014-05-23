using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class ContractManager
    {
        static Dictionary<int, Position> ContractDictionary = new Dictionary<int, Position>();
        static Dictionary<string, List<Position>> UnderlyingMap = new Dictionary<string, List<Position>>();

        public static void CreatContract(Strategy op)
        {
            try
            {
                //CTPEvents contract = new CTPEvents(op);
                //ContractDictionary.Add(ContractDictionary.Count + 1, contract);
                //if(UnderlyingMap.Keys.Contains(op.underlyingInstrumentID))
                //{
                //    UnderlyingMap[op.underlyingInstrumentID].Add(contract);
                //}
                //else
                //{
                //    List<CTPEvents> contractList = new List<CTPEvents>();
                //    contractList.Add(contract);
                //    UnderlyingMap.Add(op.underlyingInstrumentID, contractList);
                //}
            }
            catch
            {

            }
        }

        public static void CreatContract(Instrument instrument)
        {
            try
            {
                //CTPEvents contract = new CTPEvents(instrument);
                //ContractDictionary.Add(ContractDictionary.Count + 1, contract);
            }
            catch
            {

            }
        }

        public static Position GetContract(int i)
        {
            return ContractDictionary[i];
        }

        public static List<Position> GetContract(string strUnderlyingID)
        {
            return UnderlyingMap[strUnderlyingID];
        }

        public static void Init()
        {
            ContractDictionary.Clear();
        }
    }
}
