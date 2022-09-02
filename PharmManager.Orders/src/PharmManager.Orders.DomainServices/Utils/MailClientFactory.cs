using MailKit;
using PharmManager.Orders.Domain.Utils;

namespace PharmManager.Orders.DomainServices.Utils
{
    public class MailClientFactory : IMailClientFactory
    {
        private readonly Func<IMailTransport> _mailClientCreator;

        public MailClientFactory(Func<IMailTransport> mailClientCreator)
        {
            _mailClientCreator = mailClientCreator;
        }

        public IMailTransport CreateMailClient()
        {
            return _mailClientCreator();
        }
    }
}
