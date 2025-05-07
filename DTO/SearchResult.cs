public class SearchResult
{
    public string Url { get; set; }
    public double PageRank { get; set; }

    public string FirstWord { get; set; }
    public int FirstWordCount { get; set; }

    public string? SecondWord { get; set; }  // Nullable for single-word queries
    public int? SecondWordCount { get; set; }
}
