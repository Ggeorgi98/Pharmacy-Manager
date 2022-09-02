using MailKit;

namespace PharmManager.Orders.Domain.Utils
{
    public interface IMailClientFactory
    {
        IMailTransport CreateMailClient();
    }
}
