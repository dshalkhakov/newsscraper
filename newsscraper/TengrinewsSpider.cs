using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace newsscraper
{
    public class TengrinewsSpider : AbstractSpider
    {
        readonly DAL.IArticlesRepository _articlesRepository;
        readonly Func<DAL.Infrastructure.IUnitOfWork> _uowFactory;
        
        public TengrinewsSpider(DAL.IArticlesRepository articlesRepository, Func<DAL.Infrastructure.IUnitOfWork> uowFactory, System.Net.Http.HttpClient client)
            : base(client)
        {
            if (articlesRepository == null)
                throw new ArgumentNullException(nameof(articlesRepository));
            if (uowFactory == null)
                throw new ArgumentNullException(nameof(uowFactory));
            _articlesRepository = articlesRepository;
            _uowFactory = uowFactory;
        }

        public override string BaseUri => "https://tengrinews.kz/";

        public override void Parse(HtmlNode rootNode, string docUri)
        {
            // get all divs having class .item
            var items = rootNode.SelectNodes("//div[contains(@class, 'ten')]/div[contains(@class, 'item')]");
            if (items == null)
            {
                Console.WriteLine("Failed to find articles block");
                return;
            }

            foreach (var item in items)
            {
                var uri = ParseNewsItem(item);
                if (uri.Contains("fotoarchive/"))
                    continue;   // skip fotoarchive links

                Crawl(uri, ParseArticleInternal);
            }
        }

        void ParseArticleInternal(HtmlAgilityPack.HtmlNode htmlNode, string uri)
        {
            var expressions = TengrinewsScrapingExpressions.ExpressionsForUri(uri);
            var article = ParseArticle(htmlNode, expressions);
            if (article == null)
                return;
            article.Uri = uri;
            using (var uow = _uowFactory())
            {
                var art = _articlesRepository.GetByUri(uri);
                if (art == null)
                    _articlesRepository.Create(article);
                else
                    Console.WriteLine("Article already added, skipping");
                uow.SaveChanges();
            }
        }

        public string ParseNewsItem(HtmlAgilityPack.HtmlNode item)
        {
            // to visualize, news items have this structure:
            // <p>сегодня, 12:30</p
            // <a href="/relative-uri">
            //  <span>title</span>
            // </a>
            try
            {
                var link = item.SelectSingleNode("a");
                var uriText = link.GetAttributeValue("href", "");
                return uriText;
            }
            catch (NullReferenceException ex)
            {
                // something wrong with markup, log
                Console.WriteLine("Error parsing item {0}, malformed markup: {1}, {2}", item.XPath, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DAL.Entities.Article ParseArticle(HtmlAgilityPack.HtmlNode rootNode, ScrapingExpressions expressions)
        {
            var title = rootNode.SelectSingleNode(expressions.TitleXPath);
            var post = rootNode.SelectSingleNode(expressions.PostXPath);
            var date = rootNode.SelectSingleNode(expressions.DateXPath);

            if (title == null || post == null || date == null)
            {
                Console.WriteLine("Failed to parse document");
                return null;
            }

            return new DAL.Entities.Article()
            {
                Contents = post.InnerText,
                Title = title.InnerText,
                CreationDate = Program.ParseDateTime(date.InnerText),
            };
        }
    }
}
