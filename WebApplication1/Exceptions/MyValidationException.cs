namespace WebApplication1.Exceptions
{
    public class MyValidationException : Exception
    {
        public MyValidationException() : base() { }
        public MyValidationException(string message) : base(message) { }
        public MyValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
