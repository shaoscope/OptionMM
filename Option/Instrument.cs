using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class Instrument
    {
        public string InstrumentID = "";
        public int LongPosition = 0;
        public int LongInputLots = 0;
        public int ShortPosition = 0;
        public int ShortInputLots = 0;

        public Instrument(string name)
        {
            InstrumentID = name;
        }
    }
}
