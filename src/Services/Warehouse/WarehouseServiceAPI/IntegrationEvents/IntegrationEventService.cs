using EventBus.Abstractions;
using EventBus.Events;

namespace WarehouseServiceAPI.IntegrationEvents
{
    public class IntegrationEventService(IEventBus eventBus, ILogger<IntegrationEventService> logger) : IIntegrationEventService
    {
        readonly IEventBus _eventBus = eventBus;
        readonly ILogger<IntegrationEventService> _logger = logger;

        public void PublishIntegrationEvent(IntegrationEventBase evt)
        {
            try
            {
                _eventBus.Publish(evt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while publishing integration event.");
                throw;
            }
        }
    }
}