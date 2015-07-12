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
    public class RCONConnection
    {
        public TcpClient client;
        public bool identified;
        private RCONServer listenerc;
        private Thread rthread;
        public int uniqueID = -1;
        public Dictionary<string, string> options;
        private DateTime connection_time;
        public bool alive;

        public RCONConnection(TcpClient _client, RCONServer _core)
        {
            client = _client;
            listenerc = _core;
            identified = false;
            alive = true;

            options = new Dictionary<string, string>()
            {
                //{"colours", "true"}, // Won't even going to bother adding it for now
                {"redrawcmd", "true"}
            };
            connection_time = DateTime.Now;
        }

        public void SendThread(ThreadStart watdo)
        {
            rthread = new Thread(watdo);
            rthread.Start();
        }

        public void Send(string command, bool nonewline = false)
        {
            if(nonewline == true)
                listenerc.Send(client, command);
            else
                listenerc.Send(client, command + ( !command.Contains('\n') ? "\r\n" : "" ));
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

        public void GracefulClose()
        {
            this.client.GetStream().Close(1);
            this.alive = false;
            return;
        }

        public string IPPort { get { return this.client.Client.RemoteEndPoint.ToString(); } }
        public string IP { get { return IPPort.Split(':')[0]; } }
        public string Port { get { return IPPort.Split(':')[1]; } }
        public string OnlineFor { get { 
            TimeSpan diff = DateTime.Now.Subtract(this.connection_time);
            return String.Format("{0} hours, {1} minutes and {2} seconds", diff.Hours, diff.Minutes, diff.Seconds);
        } }
    }
}
