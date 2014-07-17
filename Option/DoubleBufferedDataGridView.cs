using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionMM
{
    class DoubleBufferedDataGridView : DataGridView
    {
        /// <summary>
        /// 构造一个新实例
        /// </summary>
        public DoubleBufferedDataGridView()
        {
            this.DoubleBuffered = true;
        }
    }
}
