using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newsscraper.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/topten")]
    public class TopTenController : Controller
    {
        readonly DAL.IArticlesRepository _articlesRepository;

        public TopTenController(DAL.IArticlesRepository articlesRepository)
        {
            if (articlesRepository == null)
                throw new ArgumentNullException(nameof(articlesRepository));

            _articlesRepository = articlesRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var result = _articlesRepository.GetTopTenWords();

            return Ok(result);
        }
    }
}