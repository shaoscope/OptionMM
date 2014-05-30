using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class MMQuotation
    {

        private double askQuote;

        public double AskQuote
        {
            get { return this.askQuote; }
            set { this.askQuote = value; }
        }

        /// <summary>
        /// 买入报价
        /// </summary>
        private double bidQuote;

        public double BidQuote
        {
            get { return this.bidQuote; }
            set { this.bidQuote = value; }
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
