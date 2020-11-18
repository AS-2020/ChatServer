using ChatProtocol;
using System;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class ConnectMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            ConnectMessage connectMessage = message as ConnectMessage;
            ConnectResponseMessage connectResponseMessage = new ConnectResponseMessage();


            // @TODO Benutzer authentifizieren
            bool existent = false;
            if (server.ClientExists(connectMessage.ClientName, connectMessage.ClientPassword))
            {
                existent = true;
            }

            bool authenticated = true;
            if (server.HasPassword())
            {
                authenticated = server.CheckPassword(connectMessage.ServerPassword);
            }

            if (authenticated && existent)
            {
                server.AddClient(client);
                server.Zaehler++;
                connectResponseMessage.Zaehler = server.Zaehler;
                Console.WriteLine("Client connected.");
                
            }

            //ConnectResponseMessage connectResponseMessage = new ConnectResponseMessage();
            connectResponseMessage.Success = authenticated;
            string json = JsonSerializer.Serialize(connectResponseMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(msg, 0, msg.Length);
            
        }
    }
}
