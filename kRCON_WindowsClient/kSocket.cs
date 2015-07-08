using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace kRCON_WindowsClient
{
    class kSocket
    {
        private TcpClient _socket;
        private Stream _stream;
        private UTF8Encoding _encoding;

        private string host,pass;
        private short port;

        public bool isConnected;

        public kSocket(string _host, short _port, string _pass)
        {
            host = _host;
            port = _port;
            pass = _pass;
            isConnected = false;
            _socket = new TcpClient();
            _encoding = new UTF8Encoding();
        }

        public void Destruct(string reason = "", bool showloginbox = false)
        {
            if (isConnected)
                _socket.Close();
            isConnected = false;
            Program._RconBox.SomethingToSend.Enabled = false;
            Program.wattosend.Clear();
            System.Windows.Forms.MessageBox.Show(reason == "" ? "Disconnected." : reason);
            if (showloginbox)
            {
                Program._LoginBox.Show();
                Program._RconBox.Hide();
            }
            else
            {
                try
                {
                    Program._RconBox.InputBox.ReadOnly = true;
                    Program._RconBox.Button_Send.Enabled = false;
                }
                catch { }
            }
        }

        public bool Connect()
        {
            try
            {
                //_socket.Connect(host, port);
                var connect_result = _socket.BeginConnect(host, port, null, null);
                if(!connect_result.AsyncWaitHandle.WaitOne(1500))
                {
                    throw new Exception();
                }
                _socket.EndConnect(connect_result);
            }
            catch
            {
                this.Destruct("Failed to connect.", true);
                return false;
            }
            _stream = _socket.GetStream();
            isConnected = true;
            Program._RconBox.SomethingToSend.Enabled = true;
            byte[] buff = new byte[1];
            AsyncCallback callback = null;
            callback = ar =>
            {
                try
                {
                    int bytes = _stream.EndRead(ar);

                    //Program._RconBox.OutputBox.Text += buff;
                    Program.wattosend.Add(buff[0]);
                    buff[0] = 0;

                    if (!this.isConnected || bytes == 0)
                    {
                        this.Destruct("Disconnected!");
                        return;
                    }

                    _stream.BeginRead(buff, 0, 1, callback, null);
                }
                catch
                {
                    this.Destruct("Reading has failed");
                }
            };
            _stream.BeginRead(buff, 0, 1, callback, null);
            this.Send("set redrawcmd false");
            this.Send("login " + this.pass);
            return true;
        }

        
        public void Send(string data)
        {
            if (!this.isConnected) return;
            if (!data.Contains("\n"))
                data += "\r\n";
            byte[] _data = _encoding.GetBytes(data);

            try
            {
                _stream.Write(_data, 0, _data.Length);
            }
            catch
            {
                this.Destruct("Failed to send.");
            }
        }
    }
}
