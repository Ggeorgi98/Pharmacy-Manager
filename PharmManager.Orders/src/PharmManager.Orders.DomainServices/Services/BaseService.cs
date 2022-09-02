using PharmManager.Orders.Domain.Services;
using PharmManager.Orders.Domain.Utils;

namespace PharmManager.Orders.DomainServices.Services
{
    public abstract class BaseService : IBaseService
    {
        private IValidationDictionary _validationDictionary;

        public IValidationDictionary ValidationDictionary
        {
            get { return _validationDictionary; }
            set { _validationDictionary = value; }
        }
    }
}
