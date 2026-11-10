namespace PersonalProjectApi.ExceptionHandler.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string? error) : base("Conflict occured.") { }
    }
}
