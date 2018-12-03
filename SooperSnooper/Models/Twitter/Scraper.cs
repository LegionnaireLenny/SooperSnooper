using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SooperSnooper.Models.Twitter
{
    public class Scraper
    {
        public static async Task<UserTweet> ScrapeUserLink(string accountUrl)
        {

            string baseUrl = "https://mobile.twitter.com";
            //string accountUrl = "/JeremyECrawford";

            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = await context.OpenAsync(baseUrl + accountUrl);

            string username = document?.QuerySelector("span.screen-name")?.TextContent.Trim() ?? throw new ArgumentNullException("username", "Username not found");
            string displayname = document?.QuerySelector("div.fullname")?.TextContent.Trim() ?? throw new ArgumentNullException("displayname", "No display name found");
            string location = document?.QuerySelector("div.location")?.TextContent.Trim();

            #region
            //var tweetContainer = document.QuerySelectorAll("div.tweet-text");
            //foreach (var tweet in tweetContainer)
            //{
            //    var tweetId = tweet.GetAttribute("data-id");

            //    StringBuilder builder = new StringBuilder();
            //    foreach (var child in tweet.FirstChild.ChildNodes)
            //    {
            //        builder.Append($"{child.TextContent.Trim()} ");
            //    }
            //    Console.WriteLine(tweetId);
            //    Console.WriteLine($"{builder}\n");
            //}
            #endregion

            List<Tweet> tweets = new List<Tweet>();
            var queryTweets = document.QuerySelectorAll("div.tweet-text");

            User user = new User()
            {
                Username = username,
                DisplayName = displayname,
                Location = location
            };

            foreach (var tweet in queryTweets)
            {
                tweets.Add(new Tweet()
                {
                    Id = tweet.GetAttribute("data-id"),
                    Username = username,
                    MessageBody = tweet.TextContent.Trim()
                });
            }

            UserTweet userTweet = new UserTweet()
            {
                User = user,
                Tweets = tweets,
                NextUrl = document
                            ?.QuerySelector("div.w-button-more")
                            ?.QuerySelector("a")
                            ?.GetAttribute("href")
            };

            return userTweet;
        }

        public static async Task<UserTweet> ScrapeUserLink(User user, string accountUrl)
        {

            string baseUrl = "https://mobile.twitter.com";
            //string accountUrl = "/JeremyECrawford";

            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = await context.OpenAsync(baseUrl + accountUrl);

            string username = document.QuerySelector("span.screen-name").TextContent.Trim();
            string displayname = document.QuerySelector("div.fullname").TextContent.Trim();
            string location = document.QuerySelector("div.location").TextContent.Trim();

            #region
            //var tweetContainer = document.QuerySelectorAll("div.tweet-text");
            //foreach (var tweet in tweetContainer)
            //{
            //    var tweetId = tweet.GetAttribute("data-id");

            //    StringBuilder builder = new StringBuilder();
            //    foreach (var child in tweet.FirstChild.ChildNodes)
            //    {
            //        builder.Append($"{child.TextContent.Trim()} ");
            //    }
            //    Console.WriteLine(tweetId);
            //    Console.WriteLine($"{builder}\n");
            //}
            #endregion

            List<Tweet> tweets = new List<Tweet>();
            var queryTweets = document.QuerySelectorAll("div.tweet-text");

            foreach (var tweet in queryTweets)
            {
                tweets.Add(new Tweet()
                {
                    Id = tweet.GetAttribute("data-id"),
                    User = user,
                    MessageBody = tweet.TextContent.Trim()
                });
            }

            UserTweet userTweet = new UserTweet()
            {
                User = user,
                Tweets = tweets,
                NextUrl = document
                            ?.QuerySelector("div.w-button-more")
                            ?.QuerySelector("a")
                            ?.GetAttribute("href")
            };

            return userTweet;
        }


    }
}