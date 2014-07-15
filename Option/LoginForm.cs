using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTP;
using System.Threading;

namespace OptionMM
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 所有合约列表
        /// </summary>
        public List<ThostFtdcInstrumentField> instrumentsList { get; private set; }

        /// <summary>
        /// 投资者持仓
        /// </summary>
        public List<ThostFtdcInvestorPositionField> investorPositionList { get; private set; }

        string MDFrontAddr;
        string TDFrontAddr;
        string BrokerID;
        string InvestorID;
        string Password;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if(this.trader != null)
            {
                trader.OnFrontConnected -= trader_OnFrontConnected;
                trader.OnRspUserLogin -= trader_OnRspUserLogin;
                trader.OnRspQryInstrument -= trader_OnRspQryInstrument;
                trader.OnRspQryInvestorPosition -= trader_OnRspQryInvestorPosition;
                trader.OnRspQryTradingAccount -= trader_OnRspQryTradingAccount;
            }
            if(this.marketer != null)
            {
                marketer.OnFrontConnected -= marketer_OnFrontConnected;
                marketer.OnRspUserLogin -= marketer_OnRspUserLogin;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.MDFrontAddr = MDFrontAddress.Text;
            this.TDFrontAddr = TDFrontAddress.Text;
            this.BrokerID = brokerID.Text;
            this.InvestorID = investorID.Text;
            this.Password = password.Text;
            this.marketer = new CTPMDAdapter();
            this.trader = new CTPTraderAdapter();
            this.instrumentsList = new List<ThostFtdcInstrumentField>();
            this.investorPositionList = new List<ThostFtdcInvestorPositionField>();
        }

        /// <summary>
        /// 行情接口
        /// </summary>
        public CTPMDAdapter marketer { get; private set; }

        /// <summary>
        /// 交易接口
        /// </summary>
        public CTPTraderAdapter trader { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            login_Button.Enabled = false;
            marketer.OnFrontConnected += marketer_OnFrontConnected;
            marketer.OnRspUserLogin += marketer_OnRspUserLogin;
            trader.OnFrontConnected += trader_OnFrontConnected;
            trader.OnRspUserLogin += trader_OnRspUserLogin;
            trader.OnRspQryInstrument += trader_OnRspQryInstrument;
            trader.OnRspQryInvestorPosition += trader_OnRspQryInvestorPosition;
            trader.OnRspQryTradingAccount += trader_OnRspQryTradingAccount;
            //trader.OnRtnOrder +=  TDManager.TD.OnRtnOrder;
            //trader.OnRtnTrade += TDManager.TD.OnRtnTrade;
            trader.SubscribePublicTopic(EnumTeResumeType.THOST_TERT_QUICK);
            trader.SubscribePrivateTopic(EnumTeResumeType.THOST_TERT_QUICK);
            trader.RegisterFront(TDFrontAddr);
            trader.Init();
        }
        
        private void trader_OnFrontConnected()
        {
            this.SetMsg("正在登录交易主机……");
            try
            {
                ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
                req.BrokerID = BrokerID;
                req.UserID = InvestorID;
                req.Password = Password;
                trader.ReqUserLogin(req, 0);
            }
            catch (Exception exp)
            {
                this.SetMsg("交易主机登录失败，" + exp.Message);
            }
        }

        void trader_OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            this.SetMsg("正在查询资金账户……");
            try
            {
                trader.ReqQryTradingAccount(new ThostFtdcQryTradingAccountField(), 0);
            }
            catch (Exception exp)
            {
                this.SetMsg("资金账户查询失败，" + exp.Message);
            }
        }

        void trader_OnRspQryTradingAccount(ThostFtdcTradingAccountField pTradingAccount, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            this.SetMsg("正在查询投资者持仓……");
            try
            {
                Thread.Sleep(1000);
                trader.ReqQryInvestorPosition(new ThostFtdcQryInvestorPositionField(), 0);
            }
            catch (Exception exp)
            {
                this.SetMsg("投资者持仓查询失败，" + exp.Message);
            }
        }

        void trader_OnRspQryInvestorPosition(ThostFtdcInvestorPositionField pInvestorPosition, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                investorPositionList.Add(pInvestorPosition);
                return;
            }
            this.SetMsg("正在查询合约代码……");
            try
            {
                Thread.Sleep(1000);
                trader.ReqQryInstrument(new ThostFtdcQryInstrumentField(), 0);
            }
            catch (Exception exp)
            {
                this.SetMsg("合约代码查询失败，" + exp.Message);
            }
        }

        void trader_OnRspQryInstrument(ThostFtdcInstrumentField pInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if(!bIsLast)
            {
                instrumentsList.Add(pInstrument);
                return;
            }
            this.SetMsg("正在连接行情主机……");
            marketer.RegisterFront(MDFrontAddr);
            marketer.Init();
        }
        void marketer_OnFrontConnected()
        {
            this.SetMsg("正在登录行情主机……");
            try
            {
                ThostFtdcReqUserLoginField req = new ThostFtdcReqUserLoginField();
                req.BrokerID = BrokerID;
                req.UserID = InvestorID;
                req.Password = Password;
                marketer.ReqUserLogin(req, 0);
            }
            catch (Exception exp)
            {
                this.SetMsg("行情主机登录失败，" + exp.Message);
            }
        }

        void marketer_OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            this.SetMsg("行情主机登陆成功！");
            this.DialogResult = DialogResult.OK;
            
        }


        private void LoginForm_Load(object sender, EventArgs e)
        {
            //this.login_Button.PerformClick();
        }

        private void SetMsg(string Msg)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(this.SetMsg), Msg);
                return;
            }
            this.loginStatusLabel.Text = Msg;
        }

    }//class
}
