using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OptionMM
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //Login login = new Login();
            //login.ShowDialog();
            //if(login.DialogResult == DialogResult.OK)
            //{
            //    Application.Run(new MainForm());
            //}
        }
    }
}
