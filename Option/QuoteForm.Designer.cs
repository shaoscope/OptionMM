namespace OptionMM
{
    internal partial class QuoteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.quotePanel = new OptionMM.QuotePanel();
            this.SuspendLayout();
            // 
            // quotePanel
            // 
            this.quotePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quotePanel.Location = new System.Drawing.Point(0, 0);
            this.quotePanel.Name = "quotePanel";
            this.quotePanel.Size = new System.Drawing.Size(505, 416);
            this.quotePanel.TabIndex = 0;
            // 
            // QuoteForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(505, 416);
            this.Controls.Add(this.quotePanel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "QuoteForm";
            this.Text = "报价";
            this.ResumeLayout(false);

        }

        #endregion

        public QuotePanel quotePanel;

    }
}