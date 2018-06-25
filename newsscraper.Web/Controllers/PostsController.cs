using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newsscraper.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/posts")]
    public class PostsController : Controller
    {
        readonly DAL.IArticlesRepository _articlesRepository;

        public PostsController(DAL.IArticlesRepository articlesRepository)
        {
            if (articlesRepository == null)
                throw new ArgumentNullException(nameof(articlesRepository));
            _articlesRepository = articlesRepository;
        }

        [HttpGet]
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            if (from == null || to == null)
                return NotFound();

            var toDate = to.GetValueOrDefault().AddDays(1);
            var result = _articlesRepository.GetByDate(from.GetValueOrDefault(), toDate);

            return this.Ok(result);
        }
    }
}