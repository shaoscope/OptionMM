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
            cells[0].Value = quote.call.LongPosition.Position;
            cells[1].Value = quote.call.LongPosition.TodayPosition;
            cells[2].Value = "";
            cells[3].Value = quote.call.MarketData.BidPrice1;
            cells[4].Value = "";
            cells[5].Value = quote.call.MarketData.AskPrice1;
            cells[6].Value = "";
            cells[7].Value = quote.call.ShortPosition.TodayPosition;
            cells[8].Value = quote.call.ShortPosition.Position;
            cells[9].Value = "";
            string[] temp = quote.call.Contract.InstrumentID.Split('-');
            cells[10].Value = temp[0] + " " + temp[2];
            cells[11].Value = "";
            cells[12].Value = quote.put.LongPosition.Position;
            cells[13].Value = quote.put.LongPosition.TodayPosition;
            cells[14].Value = "";
            cells[15].Value = quote.put.MarketData.BidPrice1;
            cells[16].Value = "";
            cells[17].Value = quote.put.MarketData.AskPrice1;
            cells[18].Value = "";
            cells[19].Value = quote.put.ShortPosition.TodayPosition;
            cells[20].Value = quote.put.ShortPosition.Position;            
            this.dataTable.Rows.Add(quote);
		}

        /// <summary>
        /// 重写窗体被载入时的事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.dataTable.Sort(this.dataTable.Columns[10], ListSortDirection.Ascending);
        }

    }
}
