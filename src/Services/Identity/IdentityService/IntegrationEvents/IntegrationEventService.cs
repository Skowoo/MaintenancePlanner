﻿using EventBus.Abstractions;
using EventBus.Events;

namespace IdentityServiceAPI.IntegrationEvents
{
    public class IntegrationEventService(IEventBus eventBus) : IIntegrationEventService
    {
        readonly IEventBus _eventBus = eventBus;

        public void PublishIntegrationEvent(IntegrationEventBase evt) => _eventBus.Publish(evt);
    }
}