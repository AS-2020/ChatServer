using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProtocol
{
    public class UserListResponseMessage : IMessage
    {
        public string UserListJson { get; set; }

        public int MessageId
        {
            get
            {
                return 10;
            }
            set { }
        }
    }
}
