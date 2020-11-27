using ChatProtocol;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class ChatMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            ChatMessage chatMessage = message as ChatMessage;

            User user = server.GetUsers().Find(u => u.SessionIds.Contains(chatMessage.SessionId));

            if (user != null)
            {
                chatMessage.SessionId = string.Empty;
                chatMessage.UserId = user.Id;
                string json = JsonSerializer.Serialize(chatMessage);
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);

                if (chatMessage.PrivateId == 0)
                {
                    foreach (TcpClient remoteClient in server.GetClients())
                    {
                        //if (remoteClient != client)

                        remoteClient.GetStream().Write(msg, 0, msg.Length);

                    }
                }
                else
                {
                    User privateUser = server.GetUsers().Find(u => u.Id == chatMessage.PrivateId);
                    foreach (TcpClient remoteClient in privateUser.tcpClients)
                    {
                        remoteClient.GetStream().Write(msg, 0, msg.Length);
                    }

                    // TcpClient privateClient;
                    // privateClient = server.GetUsers().Find(u => u.tcpClients.Contains(client)).tcpClients;
                    // privateClient.GetStream().Write(msg, 0, msg.Length);
                    // var privateclient = from u in server.GetUsers() where u.tcpClients.Contains(c => c.Equals(client)) select u.privateId;
                }

            }

        }
    }
}
