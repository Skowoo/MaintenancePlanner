﻿using EventBus.Events;

namespace ActionServiceAPI.Application.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        void PublishIntegrationEvent(IntegrationEventBase evt);
    }
}