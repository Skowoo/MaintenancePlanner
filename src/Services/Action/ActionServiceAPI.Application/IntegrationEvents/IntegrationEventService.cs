﻿using EventBus.Abstractions;
using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents
{
    public class IntegrationEventService(IEventBus eventBus) : IIntegrationEventService
    {
        readonly IEventBus _eventBus = eventBus;

        public void PublishIntegrationEvent(IntegrationEventBase evt) => _eventBus.Publish(evt);
    }
}