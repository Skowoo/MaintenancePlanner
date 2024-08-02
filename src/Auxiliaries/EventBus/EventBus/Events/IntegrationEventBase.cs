namespace EventBus.Events
{
    public record IntegrationEventBase
    {
        public Guid Id { get; init; }

        public DateTime CreationDate { get; init; }
    }
}
