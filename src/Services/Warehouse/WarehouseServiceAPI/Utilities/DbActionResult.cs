namespace WarehouseServiceAPI.Utilities
{
    /// <summary>
    /// Structure which stores result of CRUD operations on database
    /// </summary>
    /// <param name="result"> boolean - true if succeeded </param>
    /// <param name="errors"> List of eventual exceptions </param>
    public readonly struct DbActionResult(bool result, Exception error = null!)
    {
        /// <summary>
        /// Indentification of the result
        /// </summary>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// Result of the operation - true if succeeded
        /// </summary>
        public bool IsSuccess { get; init; } = result;

        /// <summary>
        /// List of exceptions that occured during the operation
        /// </summary>
        public Exception? Exception { get; init; } = error ??= new Exception();
    }
}