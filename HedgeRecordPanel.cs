using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Option
{
    /// <summary>
    /// 显示期权回测报告面板
    /// </summary>
    internal partial class HedgeRecordPanel : UserControl
    {
        /// <summary>
        /// 构造一个新实例
        /// </summary>
        public HedgeRecordPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 窗体载入时
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        

        /// <summary>
        /// 清楚所有记录
        /// </summary>
        public void ClearRecord()
        {
            this.dataTable.Rows.Clear();
        }

        //释放用到的资源
        private void Release()
        {
            foreach (DataGridViewRow row in this.dataTable.Rows)
                row.Dispose();
        }
    }
}
