namespace SearchBarWebAPI.Search.Core.Utility
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
