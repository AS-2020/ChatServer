using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> SessionIds = new List<string>();
        public List<TcpClient> tcpClients = new List<TcpClient>();
    }
}
