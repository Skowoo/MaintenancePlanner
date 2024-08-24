using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Domain.Events
{
    public class SparePartsTakenDomainEvent(IEnumerable<UsedPart> parts) : INotification
    {
        public IEnumerable<UsedPart> Parts { get; set; } = parts;
    }
}
