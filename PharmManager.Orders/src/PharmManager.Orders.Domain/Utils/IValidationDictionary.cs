using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PharmManager.Orders.Domain.Utils
{
    public interface IValidationDictionary
    {
        bool IsValid();

        void AddModelError(string key, string message);

        ModelStateDictionary GetModelState();
    }
}
