using EventBus.Abstractions;
using WarehouseServiceAPI.IntegrationEvents.Events;
using WarehouseServiceAPI.Services;

namespace WarehouseServiceAPI.IntegrationEvents.EventHandlers
{
    public class SparePartsUsedInActionIntegrationEventHandler
        (IPartService partService, 
        ILogger<SparePartsUsedInActionIntegrationEventHandler> logger) 
        : IIntegrationEventHandler<SparePartsUsedInActionIntegrationEvent>
    {
        private readonly IPartService _partService = partService;
        private readonly ILogger<SparePartsUsedInActionIntegrationEventHandler> _logger = logger;

        public Task Handle(SparePartsUsedInActionIntegrationEvent evt)
        {
            _logger.LogTrace("Handling event with Id: {}", evt.Id);

            foreach (var part in evt.UsedParts)
            {
                var result = _partService.DecreasePartQuantity(part.PartId, part.Quantity);

                if (!result.Result.IsSuccess)
                {
                    _logger.LogError("Error while decreasing part quantity. Exception: {}", result.Result.Exception);
                    throw result.Result.Exception!;
                }
            }

            return Task.CompletedTask;
        }
    }
}
