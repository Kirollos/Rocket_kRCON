namespace kRCON_WindowsClient
{
    partial class Login
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
            this.label1 = new System.Windows.Forms.Label();
            this.Text_IP = new System.Windows.Forms.TextBox();
            this.Text_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Text_Pass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Button_Connect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // Text_IP
            // 
            this.Text_IP.Location = new System.Drawing.Point(81, 39);
            this.Text_IP.Name = "Text_IP";
            this.Text_IP.Size = new System.Drawing.Size(100, 20);
            this.Text_IP.TabIndex = 1;
            // 
            // Text_Port
            // 
            this.Text_Port.Location = new System.Drawing.Point(81, 76);
            this.Text_Port.Name = "Text_Port";
            this.Text_Port.Size = new System.Drawing.Size(100, 20);
            this.Text_Port.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // Text_Pass
            // 
            this.Text_Pass.Location = new System.Drawing.Point(81, 115);
            this.Text_Pass.Name = "Text_Pass";
            this.Text_Pass.Size = new System.Drawing.Size(100, 20);
            this.Text_Pass.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // Button_Connect
            // 
            this.Button_Connect.Location = new System.Drawing.Point(81, 193);
            this.Button_Connect.Name = "Button_Connect";
            this.Button_Connect.Size = new System.Drawing.Size(75, 23);
            this.Button_Connect.TabIndex = 6;
            this.Button_Connect.Text = "Connect";
            this.Button_Connect.UseVisualStyleBackColor = true;
            this.Button_Connect.Click += new System.EventHandler(this.Button_Connect_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 228);
            this.Controls.Add(this.Button_Connect);
            this.Controls.Add(this.Text_Pass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Text_Port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Text_IP);
            this.Controls.Add(this.label1);
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Text_IP;
        private System.Windows.Forms.TextBox Text_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Text_Pass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Button_Connect;
    }
}