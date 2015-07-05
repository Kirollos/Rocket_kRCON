/*
    See LICENSE file for license info.
*/

using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using SDG;

namespace kRCONPlugin
{
    /*
     * Borrowed idea from (http://stackoverflow.com/questions/11911660/redirect-console-writeline-from-windows-application-to-a-string)
    */
    public class ConsoleWriterEventArgs : EventArgs
    {
        public string Value { get; private set; }
        public ConsoleWriterEventArgs(string value)
        {
            Value = value;
        }
    }

    public class ConsoleWriter : TextWriter
    {
        private TextWriter stdout = null;
        public ConsoleWriter(TextWriter __stdout)
        {
            stdout = __stdout;
        }

        public override Encoding Encoding { get { return Encoding.UTF8; } }

        public override void Write(string value)
        {
            if (WriteEvent != null) WriteEvent(this, new ConsoleWriterEventArgs(value));
            //base.Write(value);
            stdout.Write(value.Trim());
            if (String.IsNullOrEmpty(value.Trim()))
                stdout.WriteLine();
        }

        public override void WriteLine(string value)
        {
            if (WriteLineEvent != null) WriteLineEvent(this, new ConsoleWriterEventArgs(value));
            //base.WriteLine(value);
            stdout.WriteLine(value);
        }

        public event EventHandler<ConsoleWriterEventArgs> WriteEvent;
        public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;
    }
}
