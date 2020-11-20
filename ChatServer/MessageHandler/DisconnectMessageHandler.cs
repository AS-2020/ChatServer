using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class DisconnectMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            DisconnectMessage disconnectMessage = message as DisconnectMessage;

            User user = server.GetUsers().Find(u => u.Username == disconnectMessage.Username);

            if (user != null)
            {
                server.RemoveClient(client);
                server.RemoveUsers(user, disconnectMessage.SessionId);

            }

            DisconnectResponseMessage disconnectResponseMessage = new DisconnectResponseMessage
            {
                Username = user.Username
            };

            string disconnectResponseMessageJson = JsonSerializer.Serialize(disconnectResponseMessage);
            byte[] disconnectResponseMessageBytes = System.Text.Encoding.UTF8.GetBytes(disconnectResponseMessageJson);

            foreach (TcpClient remoteClient in server.GetClients())
            {
                remoteClient.GetStream().Write(disconnectResponseMessageBytes, 0, disconnectResponseMessageBytes.Length);
            }

        }
    }
}
