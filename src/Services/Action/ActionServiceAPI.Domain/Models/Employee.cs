using ActionServiceAPI.Domain.Models.SeedWork;

namespace ActionServiceAPI.Domain.Models
{
    public class Employee(string id) : Entity
    {
        public string UserId { get; set; } = id;
    }
}
