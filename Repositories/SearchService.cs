using Microsoft.EntityFrameworkCore;

public class SearchService : ISearchService
{
    private readonly SearchEngineContext _context;

    public SearchService(SearchEngineContext context)
    {
        _context = context;
    }

    public async Task<List<SearchResult>> SearchAsync(string searchTerm)
    {
        var results = await _context.inverted_index
            .Include(w => w.UrlWithRank)
            .Where(w => w.Word.Contains(searchTerm))
            .Select(w => new SearchResult
            {
                Url = w.UrlWithRank.URL,
                PageRank = w.UrlWithRank.PageRank,
                Word = w.Word,
                Count = w.Count
            })
            .OrderByDescending(r => r.PageRank)
            .ToListAsync();

        return results;
    }
}