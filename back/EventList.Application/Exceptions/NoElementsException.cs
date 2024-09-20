namespace EventList.Application.Exceptions
{
    public class NoElementException : Exception
    {
        public NoElementException() : base() { }
        public NoElementException(string message) : base(message) { }
        public NoElementException(string message, Exception innerException) : base(message, innerException) { }
    }
}
