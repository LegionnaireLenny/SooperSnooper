using AngleSharp;
using AngleSharp.Dom;
using SooperSnooper.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SooperSnooper.Models.Twitter
{
    public class Scraper
    {
        public static DateTime ConvertTimestamp(string timestamp)
        {
            DateTime convertedStamp = DateTime.Now;

            if (timestamp.Length <= 3)
            {
                if (timestamp.Contains("m"))
                {
                    int minutes = int.Parse(timestamp.Replace("m", ""));
                    convertedStamp = convertedStamp.AddMinutes(-minutes);
                }
                else if (timestamp.Contains("h"))
                {
                    int hours = int.Parse(timestamp.Replace("h", ""));
                    convertedStamp = convertedStamp.AddHours(-hours);
                }
                else
                {
                    throw new FormatException($"{timestamp} has unexpected format");
                }
            }
            else
            {
                if (!DateTime.TryParse(timestamp, out convertedStamp))
                {
                    throw new FormatException($"{timestamp} has unexpected format");
                }
            }

            return convertedStamp;
        }

        public static async Task<UserTweet> ScrapeUserLink(string accountUrl)
        {

            string baseUrl = "https://mobile.twitter.com";

            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument document = await context.OpenAsync(baseUrl + accountUrl);

            if (document?.QuerySelector("div.title")?.TextContent?.Equals("This account has been suspended.") == true)
            {
                throw new SuspendedAccountException("User has been suspended");
            }

            string username = document?.QuerySelector("span.screen-name")?.TextContent.Trim() ?? throw new UserNotFoundException("User does not exist");
            string displayname = document?.QuerySelector("div.fullname")?.TextContent.Trim();
            string location = document?.QuerySelector("div.location")?.TextContent.Trim();

            if (document?.QuerySelector("div.protected") != null)
            {
                throw new ProtectedAccountException("User has a protected account");
            }

            List<Tweet> tweets = new List<Tweet>();
            var queryTweets = document?.QuerySelectorAll("div.tweet-text");

            if(queryTweets.Length == 0)
            {
                throw new NoTweetsFoundException("User has no tweets");
            }

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
    }
}