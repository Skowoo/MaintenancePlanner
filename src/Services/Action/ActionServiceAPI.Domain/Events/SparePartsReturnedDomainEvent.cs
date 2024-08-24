using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Domain.Events
{
    public class SparePartsReturnedDomainEvent(IEnumerable<UsedPart> parts) : INotification
    {
        public IEnumerable<UsedPart> Parts { get; set; } = parts;
    }
}
