namespace ActionServiceAPI.Domain.Exceptions
{
    /// <summary>
    /// Custom marker exception to use with domain issues
    /// </summary>
    public class ActionDomainException : Exception
    {
        public ActionDomainException() { }

        public ActionDomainException(string message) : base(message) { }

        public ActionDomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
