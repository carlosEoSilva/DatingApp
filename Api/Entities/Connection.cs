namespace Api.Entities
{
    public class Connection
    {
        //-26
        public Connection(){ }

        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
}