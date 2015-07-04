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
    public class kRCONClient
    {
        public TcpClient client;
        public bool identified;
        public kRCONCore listenerc;
        public Thread rthread;

        public kRCONClient(TcpClient _client, kRCONCore _core)
        {
            client = _client;
            listenerc = _core;
            identified = false;
        }

        public void SendThread(ThreadStart watdo)
        {
            rthread = new Thread(watdo);
            rthread.Start();
        }

        public void Send(string command)
        {
            listenerc.Send(client, command);
            return;
        }

        public string Read()
        {
            return listenerc.Read(client);
        }

        public void Close()
        {
            this.client.Close();
            this.rthread.Abort();
            return;
        }
    }
}
