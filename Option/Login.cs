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
            button1.Text = "登陆中...";
            button1.Enabled = false;
            if (td != null)
                td = null;
            if (md != null)
                md = null;
            td = new TraderAPI(textBox5.Text,textBox3.Text,textBox1.Text,textBox2.Text);
            md = new MDAPI(textBox4.Text, textBox3.Text, textBox1.Text, textBox2.Text);
            md.Connect();
            td.Connect();
            //Thread.Sleep(5000);
            if (td.g_logined)
            {
                //登陆成功
                TDManager.TD = td;
                MDManager.MD = md;
                //this.DialogResult = DialogResult.OK;
                //this.Close();
                MainForm form = new MainForm();
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
            else
            {
                //登陆失败
                button1.Text = "登陆";
                button1.Enabled = true;
                this.DialogResult = DialogResult.No;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
