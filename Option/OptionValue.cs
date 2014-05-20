using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionMM
{
    class OptionValue
    {
        //期权的价格
        private double price;

        /// <summary>
        /// 获取或者设置期权的价格
        /// </summary>
        public double Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        //Delta
        private double delta;

        /// <summary>
        /// 获取或者设置Delta
        /// </summary>
        public double Delta
        {
            get { return this.delta; }
            set { this.delta = value; }
        }

        //Gamma
        private double gamma;

        /// <summary>
        /// 获取或者设置Gamma
        /// </summary>
        public double Gamma
        {
            get { return this.gamma; }
            set { this.gamma = value; }
        }

        //Vega
        private double vega;

        /// <summary>
        /// 获取或者设置期权的Vega
        /// </summary>
        public double Vega
        {
            get { return this.vega; }
            set { this.vega = value; }
        }

        //Theta
        private double theta;

        /// <summary>
        /// 获取或者设置期权的Theta
        /// </summary>
        public double Theta
        {
            get { return this.theta; }
            set { this.theta = value; }
        }

        //Rho
        private double rho;

        /// <summary>
        /// 获取或者设置期权的Rho
        /// </summary>
        public double Rho
        {
            get { return this.rho; }
            set { this.rho = value; }
        }

        //距离期权到期日
        private double maturity;

        /// <summary>
        /// 获取或者设置距离期权的到期日
        /// </summary>
        public double Maturity
        {
            get { return this.maturity; }
            set { this.maturity = value; }
        }

        public OptionValue()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="price"></param>
        /// <param name="delta"></param>
        /// <param name="gamma"></param>
        /// <param name="vega"></param>
        /// <param name="theta"></param>
        /// <param name="rho"></param>
        public OptionValue(double price, double delta, double gamma, double vega, double theta, double rho, double maturity)
        {
            this.price = price;
            this.delta = delta;
            this.gamma = gamma;
            this.vega = vega;
            this.theta = theta;
            this.rho = rho;
            this.maturity = maturity;
        }
    }
}
