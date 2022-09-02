namespace PharmManager.Orders.API.Utils.Settings
{
    public class MassTransitConfig
    {
        public string Host { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool SSLActive { get; set; }
        public string SSLThumbprint { get; set; } = string.Empty;
    }
}
