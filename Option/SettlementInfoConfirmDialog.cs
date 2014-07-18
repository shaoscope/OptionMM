using System;

namespace OptionMM
{
	/// <summary>
	/// 结算结果确认对话框
	/// </summary>
	public partial class SettlementInfoConfirmDialog : Dialog
	{
		/// <summary>
		/// 获取或设置结算结果内容
		/// </summary>
		public string Content
		{
			get { return this.txContent.Text; }
			set { this.txContent.Text = value; }
		}

		/// <summary>
		/// 构造一个结算结果确认对话框的新实例
		/// </summary>
		public SettlementInfoConfirmDialog()
		{
			this.InitializeComponent();
		}
	}
}
