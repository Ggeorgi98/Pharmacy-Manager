using PharmManager.Products.Domain.Utils;

namespace PharmManager.Products.Domain.Services
{
    public interface IBaseService
    {

        IValidationDictionary ValidationDictionary { get; set; }
    }
}
