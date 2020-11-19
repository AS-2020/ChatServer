﻿namespace ChatProtocol
{
    public class ConnectResponseMessage : IMessage
    {
        public bool Success { get; set; }
        public int Zaehler { get; set; } // löschen?
        public string SessionId { get; set; }

        public int MessageId
        {
            get
            {
                return 4;
            }
            set { }
        }
    }
}
