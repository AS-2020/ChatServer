﻿namespace ChatServer.MessageHandler
{
    public static class MessageHandlerFactory
    {
        public static IMessageHandler GetMessageHandler(int messageId)
        {
            switch (messageId)
            {
                case 1:
                    return new ChatMessageHandler();
                case 2:
                    return new ConnectMessageHandler();
                case 3:
                    return new DisconnectMessageHandler();
                case 6:
                    return new DisconnectMessageHandler();
                case 7:
                    return new RegisterMessageHandler();
                case 9:
                    return new UserListRequestMessageHandler();
            }

            return null;
        }
    }
}
