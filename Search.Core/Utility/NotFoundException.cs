namespace SearchBarWebAPI.Search.Core.Utility
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
