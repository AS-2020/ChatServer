using ChatProtocol;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class ChatMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            ChatMessage chatMessage = message as ChatMessage;

            string json = JsonSerializer.Serialize(chatMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);
            //string name = 

            foreach (TcpClient remoteClient in server.GetClients())
            {
                //if (remoteClient != client)
                //{
                //    remoteClient.GetStream().Write(
                //}
                remoteClient.GetStream().Write(msg, 0, msg.Length);
            }
        }
    }
}
