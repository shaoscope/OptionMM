﻿namespace OptionMM
{
    partial class RiskManagementForm
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
            this.greeksPanel = new OptionMM.GreeksPanel();
            this.SuspendLayout();
            // 
            // greeksPanel
            // 
            this.greeksPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.greeksPanel.Location = new System.Drawing.Point(12, 12);
            this.greeksPanel.Name = "greeksPanel";
            this.greeksPanel.Size = new System.Drawing.Size(622, 268);
            this.greeksPanel.TabIndex = 0;
            // 
            // RiskManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(813, 575);
            this.Controls.Add(this.greeksPanel);
            this.Name = "RiskManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RiskManagementForm";
            this.Load += new System.EventHandler(this.RiskManagementForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GreeksPanel greeksPanel;

    }
}