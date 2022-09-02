namespace PharmManager.Orders.Domain.Utils
{
    public class MailSettings
    {
        public string Sender { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool UseSsl { get; set; }
    }
}
