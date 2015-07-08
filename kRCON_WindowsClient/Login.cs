using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kRCON_WindowsClient
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Button_Connect_Click(object sender, EventArgs e)
        {
            if
            (
            String.IsNullOrEmpty(Text_IP.Text)      ||
            String.IsNullOrEmpty(Text_Port.Text)    ||
            String.IsNullOrEmpty(Text_Pass.Text)
            )
            {
                MessageBox.Show("Error: one of the text boxes are empty.");
                return;
            }

            Program._RconBox.Show();
            Program.socket = new kSocket(Text_IP.Text, (short)int.Parse(Text_Port.Text), Text_Pass.Text);
            if(Program.socket.Connect())
                this.Hide();
        }
    }
}
