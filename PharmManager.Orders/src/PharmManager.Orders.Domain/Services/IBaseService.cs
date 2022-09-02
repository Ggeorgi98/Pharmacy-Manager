using PharmManager.Orders.Domain.Utils;

namespace PharmManager.Orders.Domain.Services
{
    public interface IBaseService
    {

        IValidationDictionary ValidationDictionary { get; set; }
    }
}
