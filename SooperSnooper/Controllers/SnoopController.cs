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

        // GET: Snoop/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string username,
                                    string currentFilter,
                                    string searchString,
                                    int? page)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    ModelState.AddModelError("Null Username", "A username must be selected");
                    return RedirectToAction("Scoops", "Snoop");
                }

                ViewBag.Username = username;

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var tweets = db.Tweets.Where(m => m.Username == username);

                if (!string.IsNullOrEmpty(searchString))
                {
                    tweets = tweets.Where(s => s.MessageBody.Contains(searchString));
                }


                int pageSize = 20;
                int pageNumber = (page ?? 1);

                return View(tweets.OrderByDescending(m => m.Id).ToPagedList(pageNumber, pageSize));

            }
            catch (Exception)
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
