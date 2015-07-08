using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace kRCON_WindowsClient
{
    static class Program
    {
        public static Login _LoginBox;
        public static Box _RconBox;
        public static kSocket socket;
        public static List<byte> wattosend = new List<byte>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _LoginBox = new Login();
            _RconBox = new Box();
            Application.Run(_LoginBox);
        }
    }
}
