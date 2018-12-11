using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SooperSnooper.Models.Twitter
{
    public class DetailsViewModel
    {
        public string Username { get; set; }
        //public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
        //public string DateSort { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Page { get; set; }
        public IPagedList<Tweet> Tweets { get; set; }
    }
}