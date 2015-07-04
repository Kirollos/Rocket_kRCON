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
    public class kRCONConfig : IRocketPluginConfiguration
    {
        public bool Enabled;
        public short Port;
        public string Password;
        public string BindIP;
        public int maxconnections;
        [XmlArrayItem(ElementName = "WhitelistIP")]
        public List<kRCON_WhitelistIP> WhitelistIPs;
        
        public IRocketPluginConfiguration DefaultConfiguration
        {
            get
            {
                return new kRCONConfig()
                {
                    Enabled = false,
                    Port = 0,
                    Password = "changeme",
                    BindIP = "0.0.0.0",
                    maxconnections = 5,
                    WhitelistIPs = new List<kRCON_WhitelistIP>() {}
                };
            }
        }
    }

    public class kRCON_WhitelistIP
    {
        [XmlText()]
        public string IP;
        kRCON_WhitelistIP() { IP = ""; }
    }
}
