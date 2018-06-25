using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper
{
    using System.Net.Http;

    public abstract class AbstractSpider
    {
        readonly HttpClient _client;

        public AbstractSpider(HttpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            _client = client;
        }

        public abstract string BaseUri { get; }

        public void Crawl()
        {
            Crawl("", Parse);
        }

        public void Crawl(string relativeUri, Action<HtmlAgilityPack.HtmlNode, string> parse)
        {
            var fullUri = new Uri(new Uri(BaseUri), relativeUri);
            Console.WriteLine("Crawling {0}...", fullUri);

#if false
            var webRequest = System.Net.WebRequest.Create(fullUri);
            webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "identity");

            var response = webRequest.GetResponse();

            var dataStream = response.GetResponseStream();
            var reader = new System.IO.StreamReader(dataStream);

            string text = reader.ReadToEnd();

#else
            string text;

            var request = new HttpRequestMessage(HttpMethod.Get, fullUri);
            try
            {
                using (var task = _client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                {
                    task.Wait();
                    var response = task.Result;
                    response.EnsureSuccessStatusCode();

                    if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                    {
                        using (var streamTask = response.Content.ReadAsStreamAsync())
                        {
                            streamTask.Wait();
                            var stream = streamTask.Result;
                            using (var decompressed = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                            using (var reader = new System.IO.StreamReader(decompressed))
                            {
                                text = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        using (var readTask = response.Content.ReadAsStringAsync())
                        {
                            readTask.Wait();

                            text = readTask.Result;
                        }
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Console.WriteLine("Error occured during crawling: {0}", ex.Message);
                return;
            }
#endif
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(text);

            Console.WriteLine("Parsing...");
            parse(doc.DocumentNode, fullUri.AbsoluteUri);
        }

        public abstract void Parse(HtmlAgilityPack.HtmlNode rootNode, string docUri);
    }
}
