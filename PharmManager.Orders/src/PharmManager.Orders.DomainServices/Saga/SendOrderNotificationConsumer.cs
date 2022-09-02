using MailKit.Security;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using PharmManager.Orders.Contracts.Messages;
using PharmManager.Orders.Domain.Utils;

namespace PharmManager.Orders.DomainServices.Saga
{
    public class SendOrderNotificationConsumer : IConsumer<SendOrderNotification>
    {
        private readonly ILogger<SendOrderNotificationConsumer> _logger;
        private readonly IMailClientFactory _mailClientFactory;
        private readonly MailSettings _mailSettings;

        private const string MAIL_SUBJECT = "Order completed";

        public SendOrderNotificationConsumer(ILogger<SendOrderNotificationConsumer> logger, IOptions<MailSettings> mailSettings,
            IMailClientFactory mailClientFactory)
        {
            _logger = logger;
            _mailClientFactory = mailClientFactory;
            _mailSettings = mailSettings.Value;
        }

        public async Task Consume(ConsumeContext<SendOrderNotification> context)
        {
            _logger.LogInformation("Send order notification email");
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_mailSettings.Sender));
            message.To.Add(MailboxAddress.Parse(context.Message.ReceiverEmail));
            message.Subject = MAIL_SUBJECT;

            message.Body = new TextPart()
            {
                Text = "Your order has been successfully completed!"
            };

            using var mailClient = _mailClientFactory.CreateMailClient();

            mailClient.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            await mailClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(_mailSettings.Username))
            {
                await mailClient.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password)
                    .ConfigureAwait(false);
            }

            await mailClient.SendAsync(message).ConfigureAwait(false);

            await mailClient.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}
