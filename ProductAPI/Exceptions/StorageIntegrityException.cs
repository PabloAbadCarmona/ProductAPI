namespace ProductAPI.Exceptions
{
    public class StorageIntegrityException : Exception
    {
        public StorageIntegrityException(string message) : base(message) { }
    }
}
