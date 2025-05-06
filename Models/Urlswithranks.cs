public class Urlswithranks
{
    public string URL { get; set; }
    public int FileName { get; set; }
    public double PageRank { get; set; }
    public virtual ICollection<inverted_index> InvertedIndices { get; set; }
}