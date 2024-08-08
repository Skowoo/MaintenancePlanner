namespace WarehouseServiceAPI.Exceptions
{
    public class WarehouseDomainException : Exception
    {
        public WarehouseDomainException() : base() { }

        public WarehouseDomainException(string message) : base(message) { }

        public WarehouseDomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
