namespace SearchBarWebAPI.Search.Application.Utility
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
