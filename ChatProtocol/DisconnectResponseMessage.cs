using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProtocol
{
    public class DisconnectResponseMessage : IMessage
    {
        public string Username { get; set; }

        public int MessageId
        {
            get
            {
                return 6;
            }
            set { }
        }
    }
}
