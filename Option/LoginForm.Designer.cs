namespace OptionMM
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.investorID = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.brokerID = new System.Windows.Forms.TextBox();
            this.MDFrontAddress = new System.Windows.Forms.TextBox();
            this.TDFrontAddress = new System.Windows.Forms.TextBox();
            this.login_Button = new System.Windows.Forms.Button();
            this.exit_Button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.loginStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "帐号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "BROKERID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "MDADDR";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(49, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "TDADDR";
            // 
            // investorID
            // 
            this.investorID.Location = new System.Drawing.Point(120, 42);
            this.investorID.Name = "investorID";
            this.investorID.Size = new System.Drawing.Size(100, 21);
            this.investorID.TabIndex = 5;
            this.investorID.Text = "60002968";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(120, 80);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 21);
            this.password.TabIndex = 6;
            this.password.Text = "60002968";
            // 
            // brokerID
            // 
            this.brokerID.Enabled = false;
            this.brokerID.Location = new System.Drawing.Point(120, 116);
            this.brokerID.Name = "brokerID";
            this.brokerID.Size = new System.Drawing.Size(100, 21);
            this.brokerID.TabIndex = 7;
            this.brokerID.Text = "66666";
            // 
            // MDFrontAddress
            // 
            this.MDFrontAddress.Enabled = false;
            this.MDFrontAddress.Location = new System.Drawing.Point(120, 151);
            this.MDFrontAddress.Name = "MDFrontAddress";
            this.MDFrontAddress.Size = new System.Drawing.Size(304, 21);
            this.MDFrontAddress.TabIndex = 8;
            this.MDFrontAddress.Text = "tcp://27.115.78.201:31213";
            // 
            // TDFrontAddress
            // 
            this.TDFrontAddress.Enabled = false;
            this.TDFrontAddress.Location = new System.Drawing.Point(120, 184);
            this.TDFrontAddress.Name = "TDFrontAddress";
            this.TDFrontAddress.Size = new System.Drawing.Size(304, 21);
            this.TDFrontAddress.TabIndex = 9;
            this.TDFrontAddress.Text = "tcp://27.115.78.201:31205";
            // 
            // login_Button
            // 
            this.login_Button.Location = new System.Drawing.Point(51, 227);
            this.login_Button.Name = "login_Button";
            this.login_Button.Size = new System.Drawing.Size(75, 23);
            this.login_Button.TabIndex = 10;
            this.login_Button.Text = "登陆";
            this.login_Button.UseVisualStyleBackColor = true;
            this.login_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // exit_Button
            // 
            this.exit_Button.Location = new System.Drawing.Point(150, 227);
            this.exit_Button.Name = "exit_Button";
            this.exit_Button.Size = new System.Drawing.Size(75, 23);
            this.exit_Button.TabIndex = 11;
            this.exit_Button.Text = "退出";
            this.exit_Button.UseVisualStyleBackColor = true;
            this.exit_Button.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(51, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(192, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "永安期货-牛逼哄哄做市商";
            // 
            // loginStatusLabel
            // 
            this.loginStatusLabel.AutoSize = true;
            this.loginStatusLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loginStatusLabel.Location = new System.Drawing.Point(264, 230);
            this.loginStatusLabel.Name = "loginStatusLabel";
            this.loginStatusLabel.Size = new System.Drawing.Size(0, 16);
            this.loginStatusLabel.TabIndex = 13;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 262);
            this.Controls.Add(this.loginStatusLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.exit_Button);
            this.Controls.Add(this.login_Button);
            this.Controls.Add(this.TDFrontAddress);
            this.Controls.Add(this.MDFrontAddress);
            this.Controls.Add(this.brokerID);
            this.Controls.Add(this.password);
            this.Controls.Add(this.investorID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox investorID;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox brokerID;
        private System.Windows.Forms.TextBox MDFrontAddress;
        private System.Windows.Forms.TextBox TDFrontAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label loginStatusLabel;
        public System.Windows.Forms.Button login_Button;
        public System.Windows.Forms.Button exit_Button;
    }
}