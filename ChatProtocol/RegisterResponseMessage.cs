using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProtocol
{
    public class RegisterResponseMessage : IMessage
    {
        public bool Success { get; set; }
        public string Content { get; set; }
        public string SessionId { get; set; }


        public int MessageId
        {
            get
            {
                return 8;
            }
            set { }
        }
    }
}
