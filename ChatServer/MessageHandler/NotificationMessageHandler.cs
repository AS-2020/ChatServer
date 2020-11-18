using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    class NotificationMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            ConnectNotification connectNotification = message as ConnectNotification;
            string json = JsonSerializer.Serialize(connectNotification);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);
            foreach (TcpClient remoteClient in server.GetClients())
            {
                remoteClient.GetStream().Write(msg, 0, msg.Length);
            }
        }
    }
}
