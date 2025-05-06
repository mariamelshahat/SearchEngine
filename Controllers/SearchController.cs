using Microsoft.AspNetCore.Mvc;

public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    //public async Task<IActionResult> Search(string query)
    //{
    //    if (string.IsNullOrEmpty(query))
    //    {
    //        return View("Index", new List<SearchResult>());
    //    }

    //    var results = await _searchService.SearchAsync(query);
    //    return View("Index", results);
    //}
    public async Task<IActionResult> Search ( string query, string sortBy )
    {
        if (string.IsNullOrEmpty ( query ))
        {
            return View ( "Index", new List<SearchResult> () );
        }

        var results = await _searchService.SearchAsync ( query );

        if (sortBy == "count")
        {
            results = results.OrderByDescending ( r => r.Count ).ToList ();
        }
        if (sortBy == "pagerank")
        {
            results = results.OrderByDescending ( r => r.PageRank ).ToList ();
        }

        return View ( "Index", results );
    }

}