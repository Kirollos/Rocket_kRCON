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
using Rocket.Unturned.Logging;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using SDG;

namespace kRCONPlugin
{
    class kRCONCommands : Rocket.Unturned.Commands.IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "rcon"; }
        }

        public string Help
        {
            get { return "kRCON commands"; }
        }

        public string Syntax
        {
            get { return "<listclients>|<kick> <connection id>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length > 0)
            {
                if (command[0] == "listclients")
                {
                    RocketChat.Say(caller, "ID\t||\tIP:PORT\t||\tCONNECTION TIME");
                    foreach(var client in kRCON.dis.rcon.Clients)
                    {
                        RocketChat.Say(caller, "#" + client.uniqueID + "\t||\t" + client.IPPort + "\t||\t" +client.OnlineFor);
                    }
                }
                else if(command[0] == "kick")
                {
                    if(command.Length == 2)
                    {
                        int id;
                        int.TryParse(command[1], out id);
                        var client = kRCON.dis.rcon.GetClientFromuID(id);
                        if(client != null)
                        {
                            client.Send("Notice: Your connection was force closed.");
                            client.GracefulClose();
                            RocketChat.Say(caller, "Connection closed.");
                        }
                        else
                        {
                            RocketChat.Say(caller, "Error: client not found.");
                        }
                    }
                }
            }
        }
    }
}
