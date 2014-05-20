using System;
using System.Collections.Generic;
using System.Linq;
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
            Login login = new Login();
            if (login.ShowDialog() == DialogResult.OK)
            {
                MainForm form = new MainForm();
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
            //Application.Run(new MainForm());
        }
    }
}
