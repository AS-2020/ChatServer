namespace ChatProtocol
{
    public class ChatMessage : IMessage
    {
        public string Content { get; set; }
        public string SessionId { get; set; }
        public int UserId { get; set; }

        public int MessageId
        {
            get
            {
                return 1;
            }
            set { }
        }
    }
}
