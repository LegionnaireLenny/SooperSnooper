using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SooperSnooper.Models.Twitter
{
    public class Scraper
    {
        public static async Task<string> ScrapeUserLink(string accountUrl)
        {

            string baseUrl = "https://mobile.twitter.com";
            //string userURL = "/JeremyECrawford";

            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = await context.OpenAsync(baseUrl + accountUrl);

            string username = document.QuerySelector("span.screen-name").TextContent.Trim();
            string displayname = document.QuerySelector("div.fullname").TextContent.Trim();
            string location = document.QuerySelector("div.location").TextContent.Trim();

            //var tweets = document.QuerySelectorAll("div.tweet-text");

            var tweetContainer = document.QuerySelectorAll("div.tweet-test");
            foreach (var tweet in tweetContainer)
            {
                var tweetId = tweet.GetAttribute("data-id");

                StringBuilder builder = new StringBuilder();
                foreach (var child in tweet.FirstChild.ChildNodes)
                {
                    builder.Append($"{child.TextContent.Trim()} ");
                }
                Console.WriteLine(tweetId);
                Console.WriteLine($"{builder}\n");
            }

            var tweetContent = document.QuerySelectorAll("div.dir-ltr");
            foreach (var tweet in tweetContent)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var child in tweet.ChildNodes)
                {
                    builder.Append($"{child.TextContent.Trim()} ");
                }
                Console.WriteLine($"{builder}\n");
            }

            return document.QuerySelector("div.w-button-more")?.QuerySelector("a")?.GetAttribute("href");
        }
    }
}