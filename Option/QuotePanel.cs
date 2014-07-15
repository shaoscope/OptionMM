using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CTP;

namespace OptionMM
{
    /// <summary>
    /// 报价信息面板
    /// </summary>
    [DefaultEvent("SelectedIndexChanged")]
    internal partial class QuotePanel : UserControl
    {

		/// <summary>
		/// 获取选择的项目
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Quote SelectedItem
		{
			get
			{
                if (this.dataTable.SelectedRows.Count == 1)
                {
                    return this.dataTable.SelectedRows[0].Tag as Quote;
                }
				return null;
			}
		}

		/// <summary>
		/// 构造一个新实例
		/// </summary>
        public QuotePanel()
		{
			this.InitializeComponent();
            this.DoubleBuffered = true;
		}

		/// <summary>
		/// 添加合约
		/// </summary>
		/// <param name="quote">合约信息</param>
		public void AddQuote(Quote quote)
		{
            quote.SetPanel(this);
            quote.CreateCells(this.dataTable);
            DataGridViewCellCollection cells = quote.Cells;
            cells[0].Value = 0;
            
            this.dataTable.Rows.Add(quote);
		}

    }
}
