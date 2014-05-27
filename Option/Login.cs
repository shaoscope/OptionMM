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


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        MDAPI md = null;
        TraderAPI td = null;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (td != null)
                td = null;
            if (md != null)
                md = null;
            td = new TraderAPI(textBox5.Text,textBox3.Text,textBox1.Text,textBox2.Text);
            md = new MDAPI(textBox4.Text, textBox3.Text, textBox1.Text, textBox2.Text);
            this.loginStatusLabel.Text = "正在验证登陆信息...";
            md.Connect();
            td.Connect();
            Thread.Sleep(2000);
            if (td.g_logined)
            {
                //登陆成功
                this.loginStatusLabel.Text = "交易登陆成功！";
                TDManager.TD = td;
                MDManager.MD = md;
                //查持仓
                TDManager.TD.ReqQryInvestorPosition();
                while (!TDManager.TD.bCanReq)
                {
                    this.loginStatusLabel.Text = "正在查持仓...";
                    Thread.Sleep(50);
                }
                this.loginStatusLabel.Text = "查询持仓成功！";
                MainForm.PositionList = TDManager.TD.positionList;
                //登陆成功
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                //登陆失败
                button1.Text = "登陆";
                button1.Enabled = true;
                this.DialogResult = DialogResult.No;
            }
        }
    }
}
