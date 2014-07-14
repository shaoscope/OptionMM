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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 所有合约列表
        /// </summary>
        Dictionary<string, ThostFtdcInstrumentField> instrumentsDictionary = new Dictionary<string, ThostFtdcInstrumentField>();

        List<ThostFtdcInvestorPositionField> investorPositionList = new List<ThostFtdcInvestorPositionField>();

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        MDAPI md = null;
        TraderAPI td = null;

        private void button1_Click(object sender, EventArgs e)
        {
            login_Button.Enabled = false;
            if (td != null)
            {
                td = null;
            }
            if (md != null)
            {
                md = null;
            }
            md = new MDAPI(MDFrontAddress.Text, brokerID.Text, investorID.Text, password.Text);
            md.MD.OnFrontConnected +=MD_OnFrontConnected;
            md.MD.OnRspUserLogin += MD_OnRspUserLogin;
            td = new TraderAPI(TDFrontAddress.Text, brokerID.Text, investorID.Text, password.Text);
            td.TD.OnFrontConnected += TD_OnFrontConnected;
            td.TD.OnRspUserLogin += TD_OnRspUserLogin;
            td.TD.OnRspOrderInsert += TD_OnRspOrderInsert;
            td.TD.OnRspQryInstrument += TD_OnRspQryInstrument;
            td.TD.OnRspQryInvestorPosition += TD_OnRspQryInvestorPosition;
            td.TD.OnRspQryTradingAccount += TD_OnRspQryTradingAccount;
            td.TD.OnRspSettlementInfoConfirm += TD_OnRspSettlementInfoConfirm;
            td.TD.OnRspQrySettlementInfo += TD_OnRspQrySettlementInfo;
            td.TD.OnRtnOrder += td.OnRtnOrder;
            td.TD.OnRtnTrade += td.OnRtnTrade;
            td.Connect();
            //Thread.Sleep(2000);
            //if (td.g_logined)
            //{
            //    //登陆成功
            //    this.loginStatusLabel.Text = "交易登陆成功！";
            //    TDManager.TD = td;
            //    MDManager.MD = md;
            //    //查合约
            //    TDManager.TD.ReqQryInstrument();

            //    //查持仓
            //    TDManager.TD.ReqQryInvestorPosition();
            //    while (!TDManager.TD.bCanReq)
            //    {
            //        this.loginStatusLabel.Text = "正在查持仓...";
            //        Thread.Sleep(50);
            //    }
            //    this.loginStatusLabel.Text = "查询持仓成功！";
            //    MainForm.PositionList = TDManager.TD.positionList;
            //    //登陆成功
            //    this.DialogResult = DialogResult.OK;
            //}
            //else
            //{
            //    //登陆失败
            //    login_Button.Text = "登陆";
            //    login_Button.Enabled = true;
            //    this.DialogResult = DialogResult.No;
            //}
        }



        private void TD_OnFrontConnected()
        {
            this.SetMsg("正在登录交易主机……");
            td.ReqUserLogin();
        }

        void TD_OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            this.SetMsg("正在查询合约代码……");
            try
            {
                this.td.ReqQryInstrument();
            }
            catch(Exception exp)
            {
                this.SetMsg("合约代码查询失败，" + exp.Message);
            }
        }

        void TD_OnRspQryInstrument(ThostFtdcInstrumentField pInstrument, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if(!bIsLast)
            {
                instrumentsDictionary.Add(pInstrument.InstrumentID, pInstrument);
                return;
            }
            this.SetMsg("正在查询投资者持仓……");
            try
            {
                this.td.ReqQryInvestorPosition();
            }
            catch (Exception exp)
            {
                this.SetMsg("投资者持仓查询失败，" + exp.Message);
            }

        }
        void TD_OnRspQryInvestorPosition(ThostFtdcInvestorPositionField pInvestorPosition, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if(!bIsLast)
            {
                investorPositionList.Add(pInvestorPosition);
                return;
            }
            this.SetMsg("正在连接行情主机……");
            md.Connect();
        }

        void TD_OnRspQrySettlementInfo(ThostFtdcSettlementInfoField pSettlementInfo, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            
        }

        void TD_OnRspSettlementInfoConfirm(ThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            
        }

        void TD_OnRspQryTradingAccount(ThostFtdcTradingAccountField pTradingAccount, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            
        }



        void TD_OnRspOrderInsert(ThostFtdcInputOrderField pInputOrder, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            
        }

        private void MD_OnFrontConnected()
        {
            this.SetMsg("正在登录行情主机……");
            try
            {
                md.ReqUserLogin(); 
            }
            catch (Exception exp)
            {
                this.SetMsg("行情主机登录失败，" + exp.Message);
            }
        }

        private void MD_OnRspUserLogin(ThostFtdcRspUserLoginField pRspUserLogin, ThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
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
