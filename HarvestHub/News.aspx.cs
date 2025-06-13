using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;

namespace HarvestHub
{
    public partial class News : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<NewsItem> GetNewsFromRss()
        {
            List<NewsItem> newsList = new List<NewsItem>();

            var rssSources = new Dictionary<string, string>
            {
                { "The Hindu", "https://www.thehindu.com/sci-tech/agriculture/feeder/default.rss" },
                { "Times of India", "https://timesofindia.indiatimes.com/rssfeeds/754968914.cms" }
                // Add more RSS sources here as needed
            };

            foreach (var source in rssSources)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(source.Value))
                    {
                        var feed = SyndicationFeed.Load(reader);
                        foreach (var item in feed.Items)
                        {
                            string summary = item.Summary?.Text ?? "";
                            string imageUrl = ExtractImageFromHtml(summary);

                            if (string.IsNullOrEmpty(imageUrl))
                                imageUrl = GetDefaultImageForSource(source.Key);

                            newsList.Add(new NewsItem
                            {
                                Title = Truncate(item.Title.Text, 100),
                                Description = Truncate(StripHtml(summary), 200),
                                Link = item.Links[0].Uri.ToString(),
                                ImageUrl = imageUrl,
                                Source = source.Key
                            });

                            if (newsList.Count >= 12) break;
                        }
                    }
                }
                catch
                {
                    // Skip broken sources silently
                }
            }

            return newsList;
        }

        private static string ExtractImageFromHtml(string html)
        {
            var match = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        private static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        private static string GetDefaultImageForSource(string source)
        {
            if (source.Contains("Hindu")) return "https://via.placeholder.com/300x150?text=The+Hindu+News";
            if (source.Contains("India")) return "https://via.placeholder.com/300x150?text=India+News";
            return "https://via.placeholder.com/300x150?text=Agri+News";
        }

        private static string Truncate(string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Length <= length ? input : input.Substring(0, length) + "...";
        }

        public class NewsItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public string ImageUrl { get; set; }
            public string Source { get; set; }
        }
    }
}
