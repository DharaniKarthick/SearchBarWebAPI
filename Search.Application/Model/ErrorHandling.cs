namespace SearchBarWebAPI.Search.Application.Model
{
    public class ErrorHandling
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string SystemMessage { get; set; }
        public string Type { get; set; }
        public int Line { get; set; }
    }
}
