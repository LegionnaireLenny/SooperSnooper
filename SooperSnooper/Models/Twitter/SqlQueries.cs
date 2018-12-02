using System;

namespace SooperSnooper.Models.Twitter
{
    public class SqlQueries
    {
        public void InsertUser(string username, string displayname, string location, DateTime joindate)
        {
            Sql("insert into " +
                "TwitterUsers(Username, DisplayName, Location, JoinDate) " +
                "values(username, displayname, location, joindate)");
        }
        public void InsertTweet(string id, string username, string messagebody, DateTime postdate)
        {
            Sql("insert into " +
                "Tweets(Id, Username, MessageBody, PostDate) " +
                "values(id, username, messagebody, postdate)");
        }
    }
}