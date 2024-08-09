﻿using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents.Events
{
    public record NewPartAddedIntegrationEvent(int PartId, int Quantity) : IntegrationEventBase
    {
        public int PartId { get; init; } = PartId;

        public int Quantity { get; init; } = Quantity;
    }
}
