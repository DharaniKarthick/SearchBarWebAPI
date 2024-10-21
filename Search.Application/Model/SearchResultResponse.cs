namespace SearchBarWebAPI.Search.Application.Model
{
    public class SearchResultResponse
    {
        public int Id { get; set; }
        public int SearchHistoryId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        public string Genre { get; set; }
        public decimal Ratings { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Price { get; set; }
        public double RelevanceScore { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
