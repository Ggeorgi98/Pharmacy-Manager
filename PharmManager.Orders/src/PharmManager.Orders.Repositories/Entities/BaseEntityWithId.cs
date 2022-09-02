using System.ComponentModel.DataAnnotations;

namespace PharmManager.Orders.Repositories.Entities
{
    public class BaseEntityWithId
    {
        [Key]
        public Guid Id { get; set; }
    }
}
