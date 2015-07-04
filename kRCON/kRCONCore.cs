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
        public List<kRCON_WhitelistIP> WhitelistIPs;
        //public List<TcpClient> Clients;
        public List<kRCONClient> Clients;
        public Thread _thread;

        public TcpListener _listener;

        public kRCONCore(short port, string password, string bindip, int maxcon, List<kRCON_WhitelistIP> wips)
        {
            Port = port;
            Password = password;
            BindIP = bindip;
            WhitelistIPs = wips;
            Clients = new List<kRCONClient>() { };
            Enabled = true;

            _listener = new TcpListener(IPAddress.Parse(bindip), port);
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
                        Rocket.Unturned.Logging.Logger.Log("Cannot accept more connections.");
                        while(this.Clients.Count >= maxcon && this.Enabled)
                        {

                        }
                    }
                    Rocket.Unturned.Logging.Logger.Log("***kRCON*** Waiting for new connection...");
                    //TcpClient newclient = _listener.AcceptTcpClient();
                    kRCONClient newclient = new kRCONClient(_listener.AcceptTcpClient(), this);

                    Clients.Add(newclient);
                    //Clients.Add(new kRCONClient(newclient, this));

                    Rocket.Unturned.Logging.Logger.Log("***kRCON*** A new client has connected! (IP: " + newclient.client.Client.RemoteEndPoint + ")...");

                    newclient.Send( 
                        "Welcome to your server's RCON. Server title: " + Steam.serverName + "\r\n" +
                        "Please login using command \"login\"\r\n"
                        );

                    newclient.SendThread(() => { 
                    
                        string command = "";

                        while(newclient.client.Client.Connected)
                        {
                            Thread.Sleep(100);
                            new ConsoleInput().redrawInputLine();
                            command = newclient.Read();
                            command = command.TrimEnd(new[] { '\n', '\r', ' ' });
                            /*this.Send(newclient, Convert.ToString(System.Convert.ToChar(8)));
                            this.Send(newclient, Convert.ToString(System.Convert.ToChar(27)));
                            this.Send(newclient, "[2K");
                            this.Send(newclient, "\r\n");
                            this.Send(newclient, Convert.ToString(System.Convert.ToChar(27)));
                            this.Send(newclient, "[1A");
                            this.Send(newclient, ">"+command+"\r\n");*/
                            newclient.Send(kRCONUtils.Console_Redrawcommand(command));
                            if (command == "quit") break;
                            if (command == "") continue;
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
                                        continue;
                                    }
                                    else
                                    {
                                        newclient.Send("Error: Invalid password!\r\n");
                                        break;
                                    }
                                }
                            }
                            if(!newclient.identified)
                            {
                                newclient.Send("Error: You have not logged in yet!\r\n");
                                continue;
                            }
                            kRCON.dis.docommand.Add(command);
                            command = "";
                        }

                        Clients.Remove(newclient);
                        newclient.Send("Good bye!");
                        Thread.Sleep(1500);
                        Rocket.Unturned.Logging.Logger.Log("***kRCON*** A Client has disconnected! (IP: " + newclient.client.Client.RemoteEndPoint + ")");
                        newclient.Close();
                    });
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
