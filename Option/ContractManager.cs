using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class ContractManager
    {
        static Dictionary<int, Contract> ContractDictionary = new Dictionary<int, Contract>();
        static Dictionary<string, List<Contract>> UnderlyingMap = new Dictionary<string, List<Contract>>();

        public static void CreatContract(Option op)
        {
            try
            {
                Contract contract = new Contract(op);
                ContractDictionary.Add(ContractDictionary.Count + 1, contract);
                if(UnderlyingMap.Keys.Contains(op.underlyingInstrumentID))
                {
                    UnderlyingMap[op.underlyingInstrumentID].Add(contract);
                }
                else
                {
                    List<Contract> contractList = new List<Contract>();
                    contractList.Add(contract);
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
                Contract contract = new Contract(instrument);
                ContractDictionary.Add(ContractDictionary.Count + 1, contract);
            }
            catch
            {

            }
        }

        public static Contract GetContract(int i)
        {
            return ContractDictionary[i];
        }

        public static List<Contract> GetContract(string strUnderlyingID)
        {
            return UnderlyingMap[strUnderlyingID];
        }

        public static void Init()
        {
            ContractDictionary.Clear();
        }
    }
}
