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
    public class kRCONPlugin : RocketPlugin<kRCONConfig>
    {
        public static kRCONPlugin dis;
        kRCONCore rcon = null;
        public List<string> docommand = new List<string>() { };
        public static ConsoleWriter __console;
        public bool ready = false;

        protected override void Load()
        {
            dis = this;
            ready = false;
            if (!this.Configuration.Enabled)
            {
                Rocket.Unturned.Logging.Logger.Log("kRCON is set to disabled.");
                return;
            }
            
            Rocket.Unturned.Logging.Logger.Log("kRCON is set to enabled.");
            Rocket.Unturned.Logging.Logger.Log("Initializing....");

            rcon = new kRCONCore(this.Configuration.Port, this.Configuration.Password, this.Configuration.BindIP, this.Configuration.WhitelistIPs);
            if(rcon.Enabled)
            {
                Rocket.Unturned.Logging.Logger.Log("Loaded!");
            }
            else
            {
                Rocket.Unturned.Logging.Logger.LogError("Failed!");
                rcon.Destruct();
            }
            __console = new ConsoleWriter(Console.Out);
            __console.WriteLineEvent += __console_WriteLineEvent;
            Console.SetOut(__console);
            return;
        }

        void __console_WriteLineEvent(object sender, ConsoleWriterEventArgs e)
        {
            if(this.Loaded && this.ready && rcon.Enabled)
            {
                foreach(var leclient in rcon.Clients)
                {
                    string[] param = e.Value.Trim(new[] { '\r', '\n' }).TrimEnd(new[] { '\t' }).Split(new[] { '\r', '\n' });
                    //rcon.Send(leclient, e.Value + "\r\n");
                    foreach(var str in param)
                    {
                        rcon.Send(leclient, str + "\r\n");
                    }
                    rcon.Send(leclient, "\r\n");
                }
            }
        }

        protected override void Unload()
        {
            if(rcon != null && rcon.Enabled)
            {
                rcon.Destruct();
            }
        }

        public void FixedUpdate()
        {
            if (!this.Loaded) return;
            if (!this.ready)
                this.ready = true;
            if (this.docommand.Count > 0)
            {
                for (int i = 0; i < this.docommand.Count; i++)
                {
                    string command = this.docommand[i];
                    InputText myinputtext = Steam.ConsoleInput.onInputText;
                    if (command.Contains('\n'))
                        command = command.Remove(command.IndexOf('\n'));
                    if (command.Contains('\r'))
                        command = command.Remove(command.IndexOf('\r'));
                    myinputtext(command);
                    this.docommand.Remove(this.docommand[i]);
                }
            }
        }
        
    }

    public class kRCONUtils
    {
        public static string Console_Redrawcommand(string command) // Works on putty, don't know about others.
        {
            string _command = "";
            _command += Convert.ToString(System.Convert.ToChar(8));
            _command += Convert.ToString(System.Convert.ToChar(27));
            _command += "[2K";
            _command += "\r\n";
            _command += Convert.ToString(System.Convert.ToChar(27));
            _command += "[1A";
            _command += ">" + command + "\r\n";
            return _command;
        }
    }
}
