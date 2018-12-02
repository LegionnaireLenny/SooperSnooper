using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SooperSnooper.Models.Twitter
{
    public class Scraper
    {
        public static async void ScrapeLink()
        {

            string baseURL = "https://mobile.twitter.com";
            string userURL = "/JeremyECrawford";

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(baseURL + userURL);

            var fullname = document.QuerySelector("div.fullname").TextContent.Trim();
            var username = document.QuerySelector("span.screen-name").TextContent.Trim();
            var location = document.QuerySelector("div.location").TextContent.Trim();

            var nextURL = document.QuerySelector("div.w-button-more").QuerySelector("a").GetAttribute("href");

            //Console.WriteLine($"{nextURL}");
            //Console.WriteLine($"{fullname} : {username} : {location}");

            var tweets = document.QuerySelectorAll("div.tweet-text");
            //var tweetId = tweets.;

            var tweetContent = document.QuerySelectorAll("div.dir-ltr");

            //Console.WriteLine($"{tweets.Any()} : {tweets.Count()}");
            //Console.WriteLine(tweetContent.Any());
            foreach (var tweet in tweetContent)
            {
                //Console.WriteLine(tweet.GetType().Name);
                //Console.WriteLine(tweet.NodeName);
                StringBuilder builder = new StringBuilder();
                foreach (var child in tweet.ChildNodes)
                {
                    //Console.WriteLine(child.GetType().Name);
                    //Console.WriteLine(child.NodeName);
                    //Console.WriteLine(child.HasChildNodes);
                    builder.Append($"{child.TextContent.Trim()} ");
                    //Console.Write(child.TextContent.Trim());
                }
                Console.WriteLine($"{builder}\n");
            }
        }

    }
}