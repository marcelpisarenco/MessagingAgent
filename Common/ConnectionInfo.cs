using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class ConnectionInfo
    {
        public const int Buff_Size = 1024;
        public byte[] Data { get; set; }
        public Socket socket { get; set; }
        public string Adress { get; set; }
        public string Topic { get; set; }
        public ConnectionInfo()
        {
            Data = new byte[Buff_Size];
        }
    }
}
