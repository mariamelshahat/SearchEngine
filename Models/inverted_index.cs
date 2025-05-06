public class inverted_index
{
    public string Word { get; set; }
    public int FileId { get; set; }
    public int Count { get; set; }
    public virtual Urlswithranks UrlWithRank { get; set; }
}