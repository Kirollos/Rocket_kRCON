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

namespace kRCON
{
    public class kRCONCore
    {
        public bool Enabled = false;
        public short Port;
        public string Password;
        public string BindIP;
        public List<kRCON_WhitelistIP> WhitelistIPs;
        public List<TcpClient> Clients;
        public Thread _thread;

        public TcpListener _listener;

        private byte[] __Readd = new byte[1024];

        public kRCONCore(short port, string password, string bindip, List<kRCON_WhitelistIP> wips)
        {
            Port = port;
            Password = password;
            BindIP = bindip;
            WhitelistIPs = wips;
            Clients = new List<TcpClient>() { };
            Enabled = true;

            _listener = new TcpListener(IPAddress.Parse(bindip), port);
            _thread = new Thread(() =>
            {
                _listener.Start();
                //Thread.Sleep(5000);
                while(!kRCONPlugin.dis.ready)
                {
                }
                while(Enabled)
                {
                    Enabled = true;
                    Rocket.Unturned.Logging.Logger.Log("***kRCON*** Waiting for new connection...");
                    TcpClient newclient = _listener.AcceptTcpClient();

                    Clients.Add(newclient);

                    Rocket.Unturned.Logging.Logger.Log("***kRCON*** A new client has connected! (IP: " + newclient.Client.RemoteEndPoint + ")...");
                    
                    string command = "";

                    while(newclient.Client.Connected)
                    {
                        Thread.Sleep(100);
                        new ConsoleInput().redrawInputLine();
                        command = this.Read(newclient);
                        command = command.TrimEnd(new[] { '\n', '\r', ' ' });
                        /*this.Send(newclient, Convert.ToString(System.Convert.ToChar(8)));
                        this.Send(newclient, Convert.ToString(System.Convert.ToChar(27)));
                        this.Send(newclient, "[2K");
                        this.Send(newclient, "\r\n");
                        this.Send(newclient, Convert.ToString(System.Convert.ToChar(27)));
                        this.Send(newclient, "[1A");
                        this.Send(newclient, ">"+command+"\r\n");*/
                        this.Send(newclient, kRCONUtils.Console_Redrawcommand(command));
                        if (command == "quit") break;
                        if (command == "") continue;
                        kRCONPlugin.dis.docommand.Add(command);
                        command = "";
                    }

                    Clients.Remove(newclient);
                    this.Send(newclient, "Good bye!");
                    Thread.Sleep(1500);
                    Rocket.Unturned.Logging.Logger.Log("***kRCON*** A Client has disconnected! (IP: " + newclient.Client.RemoteEndPoint + ")");
                    newclient.Close();
                }
                Enabled = false;
            });
            _thread.Start();
        }

        public void Destruct()
        {
            this.Enabled = false;
            this._thread.Abort();
            this._listener.Stop();
        }

        public void dowork()
        {
        }

        public string Read(TcpClient client)
        {
            byte[] _data = new byte[1];
            string data = "";
            NetworkStream _stream = client.GetStream();

            while (true)
            {
                try
                {
                    int k = _stream.Read(_data, 0, 1);
                    if (k == 0)
                        return "";
                    char kk = Convert.ToChar(_data[0]);
                    data += kk;
                    if (kk == '\n')
                        break;
                }
                catch
                {
                    return "";
                }
            }
            return data;
        }

        public void Send(TcpClient client, string text)
        {
            byte[] data = new UTF8Encoding().GetBytes(text);

            client.GetStream().Write(data, 0, text.Length);
        }
    }
    
}
