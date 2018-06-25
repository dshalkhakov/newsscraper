using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace newsscraper.DAL
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IArticlesRepository
    {
        void Create(Article article);
        void Update(Article article);
        Article GetById(int articleId);
        Article GetByUri(string uri);

        IEnumerable<string> GetTopTenWords();

        IEnumerable<Article> GetByDate(DateTime from, DateTime to);
        IEnumerable<SearchHit> GetContainingText(string text);
    }

    public class ArticlesRepository : IArticlesRepository
    {
        ScraperContext _ctx;

        /// <summary>
        /// Returns tuple word, ndoc, nentry
        /// </summary>
        const string TopTenWordsQuery = @"SELECT * FROM ts_stat('SELECT to_tsvector(''russian'', ""Title"") || to_tsvector(''russian'', ""Contents"") FROM ""Articles""')
ORDER BY nentry DESC, ndoc DESC, word
LIMIT 10";
        const string SearchQuery = @"SELECT ""Uri"", ts_headline('russian',
  ""Title"" || ' ' || ""Contents"", phraseto_tsquery(@Phrase))
from ""Articles""
where to_tsvector('russian', ""Title"" || ' ' || ""Contents"")
  @@ phraseto_tsquery(@Phrase)";

        public ArticlesRepository(ScraperContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public void Create(Article article)
        {
            _ctx.Articles.Add(article);
        }

        public void Update(Article article)
        {
            _ctx.Articles.Update(article);
        }

        public IEnumerable<Article> GetByDate(DateTime from, DateTime to)
        {
            return _ctx.Articles
                .AsNoTracking()
                .Where(e => from <= e.CreationDate && e.CreationDate <= to)
                .ToList();
        }

        public IEnumerable<SearchHit> GetContainingText(string text)
        {
#if false
            return _ctx.Articles
                .AsNoTracking()
                .Where(e => EF.Functions.ToTsVector("russian", e.Title + " " + e.Contents).Matches(EF.Functions.PhraseToTsQuery(text)))
                .ToList();
#else
            Func<System.Data.IDataReader, SearchHit> map =
                reader => new SearchHit()
                {
                    Uri = reader["Uri"].ToString(),
                    Headline = reader["ts_headline"].ToString()
                };

            return _ctx.RawQuery<SearchHit>(SearchQuery, map, Tuple.Create("Phrase", text));
#endif

        }

        public Article GetById(int articleId)
        {
            return _ctx.Articles.FirstOrDefault(e => e.ArticleID == articleId);
        }

        public Article GetByUri(string uri)
        {
            return _ctx.Articles.FirstOrDefault(e => e.Uri == uri);
        }

        public IEnumerable<Article> GetList()
        {
            return _ctx.Articles.OrderBy(e => e.ArticleID)
                .ToList();
        }

        public IEnumerable<string> GetTopTenWords()
        {
            Func<System.Data.IDataReader, string> map = reader => reader["word"].ToString();

            return _ctx.RawQuery<string>(TopTenWordsQuery, map);
        }
    }
}
