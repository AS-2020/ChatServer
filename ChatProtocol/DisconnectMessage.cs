namespace ChatProtocol
{
    public class DisconnectMessage : IMessage
    {
        public string Username { get; set; }
        public string SessionId { get; set; }
        public int MessageId
        {
            get
            {
                return 3;
            }
            set { }
        }
    }
}
