using Microsoft.EntityFrameworkCore;

public class SearchService : ISearchService
{
    private readonly SearchEngineContext _context;

    public SearchService ( SearchEngineContext context )
    {
        _context = context;
    }

    public async Task<List<SearchResult>> SearchAsync ( string searchTerm, string sortBy = "pagerank" )
    {
        var words = searchTerm
            .Split ( ' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries )
            .Select ( w => w.ToLower () )
            .ToList ();

        if (words.Count == 0)
            return new List<SearchResult> ();

        if (words.Count == 1)
        {
            var word = words[0];
            var query = _context.inverted_index
                .Include ( w => w.UrlWithRank )
                .Where ( w => w.Word == word );

            query = sortBy?.ToLower () == "count"
                ? query.OrderByDescending ( w => w.Count )
                : query.OrderByDescending ( w => w.UrlWithRank.PageRank );

            return await query.Select ( w => new SearchResult
            {
                Url = w.UrlWithRank.URL,
                PageRank = w.UrlWithRank.PageRank,
                FirstWord = w.Word,
                FirstWordCount = w.Count
            } ).ToListAsync ();
        }
        else if (words.Count == 2)
        {
            var word1 = words[0];
            var word2 = words[1];

            var results1 = await _context.inverted_index
                .Include ( w => w.UrlWithRank )
                .Where ( w => w.Word == word1 )
                .ToListAsync ();

            var results2 = await _context.inverted_index
                .Include ( w => w.UrlWithRank )
                .Where ( w => w.Word == word2 )
                .ToListAsync ();

            var joined = results1.Join (
                results2,
                r1 => r1.UrlWithRank,
                r2 => r2.UrlWithRank,
                ( r1, r2 ) => new SearchResult
                {
                    Url = r1.UrlWithRank.URL,
                    PageRank = r1.UrlWithRank.PageRank,
                    FirstWord = word1,
                    FirstWordCount = r1.Count,
                    SecondWord = word2,
                    SecondWordCount = r2.Count
                } );

            return sortBy?.ToLower () == "count"
                ? joined.OrderByDescending ( r => r.FirstWordCount + r.SecondWordCount ?? 0 ).ToList ()
                : joined.OrderByDescending ( r => r.PageRank ).ToList ();
        }
        return new List<SearchResult> ();

    }

}
