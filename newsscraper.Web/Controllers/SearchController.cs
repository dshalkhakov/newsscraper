using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace newsscraper.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        readonly DAL.IArticlesRepository _articlesRepository;

        public SearchController(DAL.IArticlesRepository articlesRepository)
        {
            if (articlesRepository == null)
                throw new ArgumentNullException(nameof(articlesRepository));
            _articlesRepository = articlesRepository;
        }

        // GET api/search?text=query
        [HttpGet]
        public IEnumerable<DAL.SearchHit> Index(string text)
        {
            return _articlesRepository.GetContainingText(text);
        }
    }
}
