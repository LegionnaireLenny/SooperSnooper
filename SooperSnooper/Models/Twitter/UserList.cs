using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SooperSnooper.Models.Twitter
{
    public class UserList
    {
        public string Username { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}