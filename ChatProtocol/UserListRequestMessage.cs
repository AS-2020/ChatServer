using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProtocol
{
    public class UserListRequestMessage : IMessage
    {
        public string SessionId { get; set; }

        public int MessageId
        {
            get
            {
                return 9;
            }
            set { }
        }
    }
}
