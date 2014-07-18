using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace OptionMM
{
	/// <summary>
	/// 对话框通用基类
	/// </summary>
	/// <remarks>当设计对话框时应该从该类继承而不是Form</remarks>
	public class Dialog : System.Windows.Forms.Form
	{
		/// <summary>
		/// 构造一个对话框通用基类的新实例
		/// </summary>
		public Dialog()
		{
			this.StartPosition = FormStartPosition.CenterParent;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.ShowInTaskbar = false;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
		}

		/// <summary>
		/// 向用户弹出消息
		/// </summary>
		/// <param name="Message">消息文本</param>
		protected void ShowMessage(string Message)
		{
			MessageBox.Show(this, Message, "参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}//class Dialog
}
