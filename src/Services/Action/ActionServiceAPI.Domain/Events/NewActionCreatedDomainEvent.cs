using ActionServiceAPI.Domain.Models;
using MediatR;

namespace ActionServiceAPI.Domain.Events
{
    public class NewActionCreatedDomainEvent(string creatorId, string conductorId, IEnumerable<UsedPart> parts) : INotification
    {
        public string CreatorId { get; set; } = creatorId;

        public string ConductorId { get; set; } = conductorId;

        public IEnumerable<UsedPart> Parts { get; set; } = parts;
    }
}
