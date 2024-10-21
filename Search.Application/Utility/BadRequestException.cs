namespace SearchBarWebAPI.Search.Application.Utility
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
