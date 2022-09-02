using System.ComponentModel.DataAnnotations;

namespace PharmManager.Products.Repositories.Entities
{
    public class BaseEntityWithId
    {
        [Key]
        public Guid Id { get; set; }
    }
}
