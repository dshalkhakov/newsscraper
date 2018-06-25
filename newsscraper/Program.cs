using System;
using System.Linq;
using System.Collections;
using Microsoft.Extensions.Configuration;

namespace newsscraper
{
    using System.Net.Http;

    public class Program
    {
        static System.Globalization.CultureInfo KkCultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("kk-KZ");

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var ctxFactory = new DAL.ScraperContextFactory();
            var context = ctxFactory.CreateDbContext(config.GetConnectionString("ScraperContext"));
            var repo = new DAL.ArticlesRepository(context);
            Func<DAL.Infrastructure.IUnitOfWork> uowFactory = () => new DAL.Infrastructure.UnitOfWork(context);

            var handler = new HttpClientHandler()
            {
                //AutomaticDecompression = System.Net.DecompressionMethods.None,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            };
            var client = new HttpClient();

            var spider = new TengrinewsSpider(repo, uowFactory, client);

            spider.Crawl();

            Console.ReadLine();
        }

        public static DateTime ParseDateTime(string dateText)
        {
            var text = dateText.Split(",");
            var date = DateTime.MinValue;
            if (text.Length < 2)
                return date;    // found unparseable text

            if (text[0] == "сегодня")
                date = DateTime.Now;
            else if (text[0] == "вчера")
                date = DateTime.Now.AddDays(-1);
            else
            {
                // 10 июня 2018
                var dateComponents = text[0].Split(" ");
                if (dateComponents.Length < 2)
                    return date;    // unparseable, return min date
                var day = dateComponents[0];
                var month = dateComponents[1];

                var monthD = Array.IndexOf(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames, month);
                if (monthD == -1)
                {
                    var lowercased = month.ToLower(KkCultureInfo);
                    monthD = Array.IndexOf(KkCultureInfo.DateTimeFormat.MonthNames, lowercased);
                }

                date = new DateTime(DateTime.Now.Year, monthD + 1, Int32.Parse(day));
            }
            var timeString = text[1];
            var time = TimeSpan.ParseExact(timeString, "g", System.Globalization.CultureInfo.InvariantCulture);

            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }
}
