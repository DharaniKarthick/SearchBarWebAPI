namespace SearchBarWebAPI.Search.Core.Model
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; }
        public string? FilterTerm { get; set; }
        public int UserId { get; set; }
        public DateTime SearchDate { get; set; }
        public int ResultCount { get; set; }
    }
}
