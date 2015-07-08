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
    public class kRCONCore
    {
        public bool Enabled = false;
        public short Port;
        public string Password;
        public string BindIP;
        public List<string> WhitelistIPs;
        //public List<TcpClient> Clients;
        public List<kRCONClient> Clients;
        public Thread _thread;

        public TcpListener _listener;

        public kRCONCore(short port, string password, string bindip, int maxcon, List<kRCON_WhitelistIP> wips)
        {
            Port = port;
            Password = password;
            BindIP = bindip;
            WhitelistIPs = new List<string>() { };
            if (wips.Count > 0)
            {
                foreach (var ip in wips)
                {
                    WhitelistIPs.Add(ip.IP);
                }
            }
            Clients = new List<kRCONClient>() { };
            Enabled = true;

            _listener = new TcpListener(IPAddress.Parse(bindip), port);
            _listener.Server.ReceiveTimeout = 1000 * 60 * 10; // Setting receive timeout to 10 minutes.
            Rocket.Unturned.Logging.Logger.Log("Setting receive timeout to " + _listener.Server.ReceiveTimeout + "ms");
            _thread = new Thread(() =>
            {
                _listener.Start();
                //Thread.Sleep(5000);
                while(!kRCON.dis.ready)
                {
                }
                while(Enabled)
                {
                    Enabled = true;
                    if(this.Clients.Count >= maxcon)
                    {
                        kRCONUtils.Rocket_Log("Cannot accept more connections.");
                        while(this.Clients.Count >= maxcon && this.Enabled)
                        {

                        }
                    }
                    kRCONUtils.Rocket_Log("Waiting for new connection...");
                    //TcpClient newclient = _listener.AcceptTcpClient();
                    kRCONClient newclient = new kRCONClient(_listener.AcceptTcpClient(), this);

                    if(this.WhitelistIPs.Count > 0 && !this.WhitelistIPs.Contains(newclient.IP))
                    {
                        newclient.Send("Error: Your IP is not whitelisted.");
                        Thread.Sleep(550);
                        kRCONUtils.Rocket_Log(newclient.IPPort+" has failed to connect!");
                        newclient.Close();
                        continue;
                    }

                    Clients.Add(newclient);
                    //Clients.Add(new kRCONClient(newclient, this));

                    kRCONUtils.Rocket_Log("A new client has connected! (ID: #"+this.CID(newclient)+", IP: " + newclient.IPPort + ")...");

                    newclient.Send( 
                        "Welcome to your server's RCON. \r\n" + 
                        "Connection #" + this.CID(newclient) + "\r\n" +
                        "Server title: \"" + Steam.serverName + "\"\r\n" +
                        "Please login using command \"login\"\r\n" +
                        "Notice: If you don't receive messages for approx " + _listener.Server.ReceiveTimeout/1000/60 +" minutes, you will be automatically disconnected."
                        );

                    newclient.SendThread(() => { 
                    
                        string command = "";

                        while(newclient.client.Client.Connected)
                        {
                            Thread.Sleep(100);
                            command = newclient.Read();
                            if (command == "") break;
                            command = command.TrimEnd(new[] { '\n', '\r', ' ' });
                            /*this.Send(newclient, Convert.ToString(System.Convert.ToChar(8)));
                            this.Send(newclient, Convert.ToString(System.Convert.ToChar(27)));
                            this.Send(newclient, "[2K");
                            this.Send(newclient, "\r\n");
                            this.Send(newclient, Convert.ToString(System.Convert.ToChar(27)));
                            this.Send(newclient, "[1A");
                            this.Send(newclient, ">"+command+"\r\n");*/
                            if(newclient.options["redrawcmd"] == "true")
                                newclient.Send(kRCONUtils.Console_Redrawcommand(command));
                            if (command == "quit") break;
                            if (command == "") continue;
                            if(command == "login")
                            {
                                if (newclient.identified)
                                    newclient.Send("Notice: You are already logged in!\r\n");
                                else
                                    newclient.Send("Syntax: login <password>");
                                continue;
                            }
                            if (command.Split(new[] { ' ' }).Length > 1 && command.Split(new[] { ' ' })[0] == "login")
                            {
                                if(newclient.identified)
                                {
                                    newclient.Send("Notice: You are already logged in!\r\n");
                                    continue;
                                }
                                else
                                {
                                    if(command.Split(new[]{' '})[1] == this.Password)
                                    {
                                        newclient.identified = true;
                                        newclient.Send("Success: You have logged in!\r\n");
                                        kRCONUtils.Rocket_Log("Client #" + this.CID(newclient) + " has logged in!");
                                        continue;
                                    }
                                    else
                                    {
                                        newclient.Send("Error: Invalid password!\r\n");
                                        kRCONUtils.Rocket_Log("Client #" + this.CID(newclient) + " has failed to log in.");
                                        break;
                                    }
                                }
                            }
                            
                            if(command == "set")
                            {
                                newclient.Send("Syntax: set [option] [value]");
                                continue;
                            }
                            if (command.Split(new[] { ' ' }).Length > 1 && command.Split(new[] { ' ' })[0] == "set")
                            {
                                string[] args = command.Split(new[] { ' ' });
                                if(args.Length != 3)
                                {
                                    newclient.Send("Syntax: set [option] [value]");
                                    continue;
                                }

                                if(!newclient.options.ContainsKey(args[1]))
                                {
                                    newclient.Send("Error: option unknown.");
                                    continue;
                                }
                                newclient.options[args[1]] = args[2];
                                newclient.Send("Success: '"+args[1]+"' set to '"+args[2]+"'.");
                                continue;
                            }
                            if(!newclient.identified)
                            {
                                newclient.Send("Error: You have not logged in yet!\r\n");
                                continue;
                            }
                            kRCONUtils.Rocket_Log("Client #" + this.CID(newclient) + " has executed command \""+command+"\"");
                            kRCON.dis.docommand.Add(command);
                            command = "";
                        }

                        Clients.Remove(newclient);
                        newclient.Send("Good bye!");
                        Thread.Sleep(1500);
                        kRCONUtils.Rocket_Log("Client #"+this.CID(newclient)+" has disconnected! (IP: " + newclient.client.Client.RemoteEndPoint + ")");
                        newclient.Close();
                    });
                }
                Enabled = false;
            });
            _thread.Start();
        }

        public int Destruct()
        {
            this.Enabled = false;
            this._thread.Abort();
            int counter = this.Clients.Count;
            foreach(var leclient in this.Clients)
            {
                leclient.Send("Good bye!");
                Thread.Sleep(150);
                leclient.Close();
            }
            this.Clients.Clear();
            this._listener.Stop();
            return counter;
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

        public int CID(kRCONClient client) { return this.Clients.IndexOf(client); }
    }
    
}
