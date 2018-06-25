using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper
{
    public class TengrinewsScrapingExpressions
    {
        static ScrapingExpressions MixNewsExpressions = new ScrapingExpressions("//h1[contains(@class, 'h1')]",
            "//div[contains(@class, 'js-mediator-article')]",
            "//div[contains(@class, 'date')]/p");
        static ScrapingExpressions RegularNewsArticleExpressions = new ScrapingExpressions("//h1[contains(@class, 'h1')]",
            "//div[@class='block']/div[contains(@class, 'data')]/div[contains(@class, 'js-mediator-article')]",
            "//div[@class='block']/div[contains(@class, 'data')]/span[@class='date']");
        static ScrapingExpressions Fifa2018Expressions = new ScrapingExpressions("//div[contains(@class, 'content')]/div[@class='h']",
            "//div[contains(@class, 'content')]/div[@class='txt']",
            "//div[contains(@class, 'content')]/div[@class='date']");

        public static ScrapingExpressions ExpressionsForUri(string uri)
        {
            if (uri.Contains("mixnews"))
            {
                return MixNewsExpressions;
            }
            else if (uri.Contains("fifa2018"))
            {
                return Fifa2018Expressions;
            }
            return RegularNewsArticleExpressions;
        }
    }
}
