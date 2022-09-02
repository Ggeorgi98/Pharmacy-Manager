using PharmManager.Products.Domain.Services;
using PharmManager.Products.Domain.Utils;

namespace PharmManager.Products.DomainServices
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
