namespace kRCON_WindowsClient
{
    partial class Box
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
            this.components = new System.ComponentModel.Container();
            this.OutputBox = new System.Windows.Forms.RichTextBox();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.Button_Send = new System.Windows.Forms.Button();
            this.SomethingToSend = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(12, 12);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.Size = new System.Drawing.Size(476, 256);
            this.OutputBox.TabIndex = 0;
            this.OutputBox.Text = "";
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(13, 275);
            this.InputBox.Multiline = true;
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(403, 40);
            this.InputBox.TabIndex = 1;
            // 
            // Button_Send
            // 
            this.Button_Send.Location = new System.Drawing.Point(423, 275);
            this.Button_Send.Name = "Button_Send";
            this.Button_Send.Size = new System.Drawing.Size(75, 40);
            this.Button_Send.TabIndex = 2;
            this.Button_Send.Text = "Send";
            this.Button_Send.UseVisualStyleBackColor = true;
            this.Button_Send.Click += new System.EventHandler(this.Button_Send_Click);
            // 
            // SomethingToSend
            // 
            this.SomethingToSend.Tick += new System.EventHandler(this.SomethingToSend_Tick);
            // 
            // Box
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 318);
            this.Controls.Add(this.Button_Send);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.OutputBox);
            this.Name = "Box";
            this.Text = "kRCON Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Box_Closed);
            this.Load += new System.EventHandler(this.Box_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox OutputBox;
        public System.Windows.Forms.TextBox InputBox;
        public System.Windows.Forms.Button Button_Send;
        public System.Windows.Forms.Timer SomethingToSend;
    }
}

