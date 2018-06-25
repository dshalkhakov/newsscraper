using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper
{
    public class ScrapingExpressions
    {
        public ScrapingExpressions(string titleXPath, string postXPath, string dateXPath)
        {
            TitleXPath = titleXPath;
            PostXPath = postXPath;
            DateXPath = dateXPath;
        }

        public string TitleXPath { get; private set; }
        public string PostXPath { get; private set; }
        public string DateXPath { get; private set; }
    }
}
