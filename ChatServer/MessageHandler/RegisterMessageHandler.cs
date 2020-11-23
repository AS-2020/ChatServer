using ChatProtocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class RegisterMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            RegisterMessage registerMessage = message as RegisterMessage;

            User test = server.GetUsers().Find(u => u.Username == registerMessage.Username);

            RegisterResponseMessage registerResponseMessage = new RegisterResponseMessage();

            bool authenticatedServerPassword = true;
            if (server.HasPassword())
            {
                authenticatedServerPassword = server.CheckPassword(registerMessage.ServerPassword);
            }

            if (authenticatedServerPassword)
            {

                if (test != null)
                {
                    registerResponseMessage.Content = "Username already exists!";
                }
                else
                {
                    int id = server.GetUsers().OrderBy(u => u.Id).Last().Id + 1;

                    User user = new User
                    {
                        Id = id,
                        Username = registerMessage.Username,
                        Password = registerMessage.Password
                    };

                    server.AddUsers(user);

                    string json = JsonSerializer.Serialize(server.GetUsers());
                    File.WriteAllText(Server.USERS_PATH, json);

                    registerResponseMessage.Content = "New User created";

                    string sessionId = Guid.NewGuid().ToString();
                    user.SessionIds.Add(sessionId);
                    registerResponseMessage.SessionId = sessionId;
                    server.AddClient(client);

                    UserCountMessage userCountMessage = new UserCountMessage
                    {
                        UserCount = server.GetUsers().Count,
                        UserOnlineCount = server.GetUsers().Count(u => u.SessionIds.Count > 0)
                    };

                    string userCountMessageJson = JsonSerializer.Serialize(userCountMessage);
                    byte[] userCountMessageBytes = System.Text.Encoding.UTF8.GetBytes(userCountMessageJson);

                    UserListRequestMessage userListRequestMessage = message as UserListRequestMessage;

                    var query = from u in server.GetUsers() select new { u.Id, u.Username };
                    var userList = query.ToList();
                    string userListJson = JsonSerializer.Serialize(userList);
                    UserListResponseMessage userListResponseMessage = new UserListResponseMessage
                    {
                        UserListJson = userListJson
                    };

                    string messageJson = JsonSerializer.Serialize(userListResponseMessage);
                    byte[] byteMessage = Encoding.UTF8.GetBytes(messageJson);

                    foreach (TcpClient remoteClient in server.GetClients())
                    {
                        if (remoteClient != client)
                        {
                            remoteClient.GetStream().Write(byteMessage, 0, byteMessage.Length);
                        }
                        remoteClient.GetStream().Write(userCountMessageBytes, 0, userCountMessageBytes.Length);
                    }

                }
            }
            bool authenticated = test == null && authenticatedServerPassword;
            registerResponseMessage.Success = authenticated;
            string json_response = JsonSerializer.Serialize(registerResponseMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json_response);


            client.GetStream().Write(msg, 0, msg.Length);



        }

    }
}
