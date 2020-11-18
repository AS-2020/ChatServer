﻿namespace ChatProtocol
{
    public class ConnectMessage : IMessage
    {
        public string ServerPassword { get; set; }
        public string ClientName { get; set; }
        public string ClientPassword { get; set; }
        public int MessageId
        {
            get
            {
                return 2;
            }
            set { }
        }
    }
}
