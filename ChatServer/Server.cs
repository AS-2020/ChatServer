using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer
{
    public class Server
    {
        private int port;
        private string ipAddress;
        private string password;
        //public int Zaehler { get; set; }

        private TcpListener tcpListener;

        private List<TcpClient> clients = new List<TcpClient>();

        private List<User> users = new List<User>();

        public const string USERS_PATH = "users.json";

        public Server(int port, string ipAddress)
        {
            this.port = port;
            this.ipAddress = ipAddress;
            //Zaehler = -1;
        }

        public void Start()
        {
            IPAddress localAddress = IPAddress.Parse(ipAddress);
            tcpListener = new TcpListener(localAddress, port);
            tcpListener.Start();
            string userJson = File.ReadAllText(USERS_PATH);
            users = JsonSerializer.Deserialize<List<User>>(userJson);
        }

        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(password);
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        public bool CheckPassword(string password)
        {
            return this.password == password;
        }

        public void Stop()
        {
            foreach (var client in clients)
            {
                client.Close();
            }
            tcpListener.Stop();
        }

        public void AddClient(TcpClient client)
        {
            clients.Add(client);
        }

        public List<TcpClient> GetClients()
        {
            return clients;
        }

        public TcpClient AcceptTcpClient()
        {
            return tcpListener.AcceptTcpClient();
        }

        public void AddUsers(User user)
        {
            users.Add(user);
        }

        public List<User> GetUsers()
        {
            return users;
        }
        public void RemoveClient(TcpClient client)
        {
            clients.Remove(client);
        }
        public void RemoveUsers(User user, string sessionId)
        {
            user.SessionIds.Remove(sessionId);
        }
    }
}
