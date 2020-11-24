using ChatProtocol;
using ChatServer.Extension;
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

        private string errorMessage;
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            RegisterMessage registerMessage = message as RegisterMessage;

            User test = server.GetUsers().Find(u => u.Username == registerMessage.Username);

            RegisterResponseMessage registerResponseMessage = new RegisterResponseMessage
            {
                Success = false
            };

            bool authenticatedServerPassword = true;

            if (server.HasPassword())
            {
                authenticatedServerPassword = server.CheckPassword(registerMessage.ServerPassword);
            }

            if (authenticatedServerPassword)
            {

                if (!IsValid(server, registerMessage.Username, registerMessage.Password))
                {
                    registerResponseMessage.Content = errorMessage;
                }
                else
                {
                    //int id = server.GetUsers().OrderBy(u => u.Id).Last().Id + 1;

                    User user = new User
                    {
                        Id = server.GetNextUserId(),
                        Username = registerMessage.Username.Trim(),
                        Password = registerMessage.Password,
                        SessionIds = new List<string>(),
                        tcpClients = new List<TcpClient>()
                    };

                    server.AddUser(user);

                    user.tcpClients.Add(client);

                    server.SaveUsers();

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
            bool authenticated = IsValid(server, registerMessage.Username, registerMessage.Password) && authenticatedServerPassword;
            registerResponseMessage.Success = authenticated;
            string json_response = JsonSerializer.Serialize(registerResponseMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json_response);

            client.GetStream().Write(msg, 0, msg.Length);
        }

        private bool IsValid(Server server, string username, string password)
        {
            var stringBuilder = new StringBuilder();

            if (!username.IsOneWord())
            {
                stringBuilder.AppendLine("Username must have one word.");
            }

            if (password.Length < server.PasswordMinLength)
            {
                stringBuilder.AppendLine($"Password length must be at least {server.PasswordMinLength}.");
            }

            if (username.Length < server.UsernameMinLength)
            {
                stringBuilder.AppendLine($"Username length must be at least {server.UsernameMinLength}.");
            }

            if (server.GetUsers().Exists(u => u.Username == username))
            {
                stringBuilder.AppendLine("Username already exists.");
            }

            errorMessage = stringBuilder.ToString();

            return errorMessage.Length == 0;
        }
    }
}
