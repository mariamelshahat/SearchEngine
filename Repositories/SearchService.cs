using Microsoft.EntityFrameworkCore;

public class SearchService : ISearchService
{
    private readonly SearchEngineContext _context;

    public SearchService(SearchEngineContext context)
    {
        _context = context;
    }

    public async Task<List<SearchResult>> SearchAsync(string searchTerm, string sortBy = "pagerank")
    {
        var query = _context.inverted_index
            .Include(w => w.UrlWithRank)
            .Where(w => w.Word.Contains(searchTerm));

        if (sortBy?.ToLower() == "count")
        {
            query = query.OrderByDescending(w => w.Count);
        }
        else
        {
            query = query.OrderByDescending(w => w.UrlWithRank.PageRank);
        }

        var results = await query
            .Select(w => new SearchResult
            {
                Url = w.UrlWithRank.URL,
                PageRank = w.UrlWithRank.PageRank,
                Word = w.Word,
                Count = w.Count
            })
            .ToListAsync();

        return results;
    }
}