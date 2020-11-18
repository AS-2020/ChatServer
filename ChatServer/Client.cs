using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    public class Client
    {
        public string ClientName { get; set; }
        public string Password { get; set; }

        public Client(string name, string password)
        {
            ClientName = name;
            Password = password;
        }
        public Client()
        {

        }
    }
}
