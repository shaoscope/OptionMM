using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class InstrumentManager
    {
        static Dictionary<string, Instrument> InstrumentMap = new Dictionary<string, Instrument>();
        public static void CreatInstrument(string instrmentID)
        {
            try
            {
                if (!InstrumentMap.Keys.Contains(instrmentID))
                {
                    Instrument instrument = new Instrument(instrmentID);
                    InstrumentMap.Add(instrmentID, instrument);
                }
            }
            catch
            {

            }
        }

        public static Instrument GetInstrument(string strID)
        {
            Instrument insRet = null;
            insRet = InstrumentMap[strID];
            return insRet;
        }

        public static Dictionary<string, Instrument> GetAllInstrument()
        {
            if(InstrumentMap.Count > 0)
            {
                return InstrumentMap;
            }
            else
            {
                return null;
            }
        }
    }
}
