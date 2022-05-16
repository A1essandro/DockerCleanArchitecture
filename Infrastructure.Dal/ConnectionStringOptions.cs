namespace Infrastructure.Dal
{
    public class ConnectionStringOptions
    {

        public string Host { get; set; } = string.Empty;

        public string User { get; set; } = string.Empty;

        public string Pass { get; set; } = string.Empty;

        public string Database { get; set; } = string.Empty;

        public int Port { get; set; } = 5432;

    }
}