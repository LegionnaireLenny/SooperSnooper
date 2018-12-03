using System.Collections.Generic;

namespace SooperSnooper.Models.Twitter
{
    public class UserTweet
    {
        public User User { get; set; }

        public List<Tweet> Tweets { get; set; }

        public string NextUrl { get; set; }
    }
}