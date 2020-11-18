namespace ChatProtocol
{
    public class ConnectNotification : IMessage
    {
        public string Name { get; set; }
        public int MessageId
        {
            get
            {
                return 5;
            }
            set { }
        }
    }
}