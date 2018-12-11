using PagedList;
using SooperSnooper.Models;
using SooperSnooper.Models.Exceptions;
using SooperSnooper.Models.Twitter;
using SooperSnooper.Models.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SooperSnooper.Controllers
{
    public class SnoopController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Snoop/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Snoop()
        {
            return View();
        }

        // POST: Snoop/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Snoop(SnoopModel snoop)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Snoop", snoop);
                }

                string accountUrl = $"/{snoop.Username}";
                UserTweet scrapedTweets;

                int loops = snoop.Loops == 0 ? int.MaxValue : snoop.Loops;

                int counter = 1;
                while (!string.IsNullOrEmpty(accountUrl) && loops > 0)
                {
                    scrapedTweets = await Scraper.ScrapeUserLink(accountUrl);
                    accountUrl = scrapedTweets.NextUrl;

                    if (db.TwitterUsers.Find(scrapedTweets.User.Username) == null)
                    {
                        db.TwitterUsers.Add(scrapedTweets.User);
                        db.Tweets.AddRange(scrapedTweets.Tweets);
                    }
                    else
                    {
                        var dbTweets = db.Tweets.Where(m => m.Username == scrapedTweets.User.Username).ToList();
                        var newTweets = scrapedTweets.Tweets
                                        .Where(scraped => dbTweets
                                            .All(database => database.Id != scraped.Id))
                                        .ToList();

                        db.Tweets.AddRange(newTweets);
                    }

                    db.SaveChanges();

                    loops--;
                    counter++;
                    await Task.Delay(1250);
                }

                return RedirectToAction("Details", new { snoop.Username });
            }
            catch (SuspendedAccountException e)
            {
                ModelState.AddModelError("Suspended", e.Message);
                return View();
            }
            catch (UserNotFoundException e)
            {
                ModelState.AddModelError("NotFound", e.Message);
                return View();
            }
            catch (ProtectedAccountException e)
            {
                ModelState.AddModelError("Protected", e.Message);
                return View();
            }
            catch (NoTweetsFoundException e)
            {
                ModelState.AddModelError("NoTweets", e.Message);
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return View();
            }
        }

        //// GET: Snoop/Details/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Details(string username,
        //                            string currentFilter,
        //                            string searchString,
        //                            string sortOrder,
        //                            DateTime? startDate,
        //                            DateTime? endDate,
        //                            int? page)
        //{
        //    try
        //    {
        //        ViewBag.CurrentSort = sortOrder;
        //        ViewBag.DateSort = string.IsNullOrEmpty(sortOrder) ? "Date" : "";

        //        if (string.IsNullOrEmpty(username))
        //        {
        //            ModelState.AddModelError("Null Username", "A username must be selected");
        //            return RedirectToAction("Scoops", "Snoop");
        //        }

        //        startDate = startDate ?? DateTime.MinValue;
        //        endDate = endDate ?? DateTime.MaxValue;


        //        if (searchString != null)
        //        {
        //            page = 1;
        //        }
        //        else
        //        {
        //            searchString = currentFilter;
        //        }

        //        ViewBag.Username = username;
        //        ViewBag.CurrentFilter = searchString;
        //        ViewBag.DateStart = startDate;
        //        ViewBag.DateEnd = endDate;

        //        var tweets = db.Tweets.Where(m => m.Username == username);
        //        var test = tweets.ToList();
        //        if (startDate != DateTime.MinValue || endDate != DateTime.MaxValue)
        //        {
        //            tweets = tweets.Where(d => d.PostDate >= startDate && d.PostDate <= endDate);
        //        }
        //        test = tweets.ToList();
        //        if (!string.IsNullOrEmpty(searchString))
        //        {
        //            tweets = tweets.Where(s => s.MessageBody.Contains(searchString));
        //        }

        //        switch (sortOrder)
        //        {
        //            case "Date":
        //                tweets = tweets.OrderBy(t => t.PostDate);
        //                //tweets = tweets.OrderByDescending(t => t.PostDate);
        //                break;
        //            default:
        //                //tweets = tweets.OrderBy(t => t.PostDate);
        //                tweets = tweets.OrderByDescending(t => t.PostDate);
        //                break;
        //        }

        //        int pageSize = 20;
        //        int pageNumber = (page ?? 1);

        //        return View(tweets.ToPagedList(pageNumber, pageSize));

        //    }
        //    catch (Exception e)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //}

        // GET: Snoop/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(DetailsViewModel details)
        {
            try
            {
                if (string.IsNullOrEmpty(details.Username))
                {
                    ModelState.AddModelError("Null Username", "A username must be selected");
                    return RedirectToAction("Scoops", "Snoop");
                }

                details.SortOrder = !string.IsNullOrEmpty(details.SortOrder) ? "" : "Date";
                details.StartDate = details.StartDate ?? DateTime.MinValue;
                details.EndDate = details.EndDate ?? DateTime.MaxValue;

                if (details.SearchString != null)
                {
                    details.Page = 1;
                }
                //else
                //{
                //    details.SearchString = details.CurrentFilter;
                //}

                var tweets = db.Tweets.Where(m => m.Username == details.Username);

                if (details.StartDate != DateTime.MinValue || details.EndDate != DateTime.MaxValue)
                {
                    tweets = tweets.Where(d => d.PostDate >= details.StartDate && d.PostDate <= details.EndDate);
                }
                if (!string.IsNullOrEmpty(details.SearchString))
                {
                    tweets = tweets.Where(s => s.MessageBody.Contains(details.SearchString));
                }

                switch (details.SortOrder)
                {
                    case "Date":
                        tweets = tweets.OrderBy(t => t.PostDate);
                        //tweets = tweets.OrderByDescending(t => t.PostDate);
                        break;
                    default:
                        //tweets = tweets.OrderBy(t => t.PostDate);
                        tweets = tweets.OrderByDescending(t => t.PostDate);
                        break;
                }

                int pageSize = 20;
                int pageNumber = (details.Page ?? 1);

                details.Tweets = tweets.ToPagedList(pageNumber, pageSize);

                return View(details);

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Snoop
        [Authorize(Roles = "Admin")]
        public ActionResult Scoops()
        {
            try
            {
                UserList userList;
                if (!db.TwitterUsers.Any())
                {
                    userList = new UserList()
                    {
                        Users = Enumerable.Empty<User>()
                    };
                }
                else
                {

                    userList = new UserList()
                    {
                        Users = db.TwitterUsers.ToList()
                    };
                }
                return View(userList);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
