using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class MMQuotation
    {

        private double askPrice;

        public double AskPrice
        {
            get { return this.askPrice; }
            set { this.askPrice = value; }
        }

        private double bidPrice;

        public double BidPrice
        {
            get { return this.bidPrice; }
            set { this.bidPrice = value; }
        }

        private int askLots;

        public int AskLots
        {
            get { return this.askLots; }
            set { this.askLots = value; }
        }

        private int bidLots;

        public int BidLots
        {
            get { return this.bidLots; }
            set { this.bidLots = value; }
        }

        public MMQuotation()
        {

        }
    }
}
