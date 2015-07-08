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
    public partial class Box : Form
    {
        public Box()
        {
            InitializeComponent();
        }

        private void Box_Load(object sender, EventArgs e)
        {
        }

        // Form toolboxes cannot be accessed/modified from another thread >:(
        private void SomethingToSend_Tick(object sender, EventArgs e)
        {
            if(Program.wattosend.Count > 0)
            {
                for (int i = 0; i < Program.wattosend.Count; i++)
                {
                    //OutputBox.Text += System.Convert.ToChar(Program.wattosend[i]);
                    OutputBox.Focus();
                    OutputBox.AppendText(System.Convert.ToString(System.Convert.ToChar(Program.wattosend[i])));
                }
                Program.wattosend.Clear();
                InputBox.Focus();
            }
        }

        private void Button_Send_Click(object sender, EventArgs e)
        {
            Program.socket.Send(InputBox.Text);
            InputBox.Text = "";
        }

        private void Box_Closed(object sender, FormClosedEventArgs e)
        {
            if(!Program._LoginBox.Visible)
                Application.Exit();
        }

        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\r')
            {
                Button_Send_Click(sender, null);
                InputBox.Clear();
            }
        }
    }
}
