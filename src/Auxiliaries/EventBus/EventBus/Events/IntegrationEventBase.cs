namespace EventBus.Events
{
    public record IntegrationEventBase
    {
        public IntegrationEventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        public Guid Id { get; init; }

        public DateTime CreationDate { get; init; }
    }
}
