namespace PharmManager.Orders.Contracts.Utils
{
    public class QueueNames
    {
        public const string SagaOrderPayment = "withdraw-customer-credit";
        private const string rabbitUri = "queue:";

        public static Uri GetMessageUri(string key)
        {
            return new Uri(rabbitUri + key.PascalToKebabCaseMessage());
        }

        public static Uri GetActivityUri(string key)
        {
            key = key.PascalToKebabCaseActivity();
            if (key.EndsWith('-'))
            {
                key = key.Remove(key.Length - 1);
            }
            return new Uri(rabbitUri + key + '_' + "execute");
        }
    }
}
