using EventBus.Abstractions;
using WarehouseServiceAPI.IntegrationEvents.Events;
using WarehouseServiceAPI.Services;

namespace WarehouseServiceAPI.IntegrationEvents.EventHandlers
{
    public class SparePartsUsedInActionIntegrationEventHandler(IPartService partService) : IIntegrationEventHandler<SparePartsUsedInActionIntegrationEvent>
    {
        private readonly IPartService _partService = partService;

        public Task Handle(SparePartsUsedInActionIntegrationEvent evt)
        {
            foreach (var part in evt.UsedParts)
            {
                var result = _partService.DecreasePartQuantity(part.PartId, part.Quantity);

                if (!result.Result.IsSuccess)
                    throw result.Result.Exception!;
            }

            return Task.CompletedTask;
        }
    }
}
