public interface ISearchService
{
    Task<List<SearchResult>> SearchAsync(string searchTerm, string sortBy = "pagerank");
}