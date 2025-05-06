public interface ISearchService
{
    Task<List<SearchResult>> SearchAsync(string searchTerm);
}