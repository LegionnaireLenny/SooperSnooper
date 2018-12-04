using PagedList;
using SooperSnooper.Models;
using SooperSnooper.Models.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SooperSnooper.Controllers
{
    public class SnoopController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Snoop/Create
        [Authorize]
        public ActionResult Snoop()
        {
            return View();
        }

        // POST: Snoop/Create
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Snoop(string username, int? loops)
        {
            try
            {
                string accountUrl = $"/{username}";
                UserTweet scrapedTweets;

                loops = loops ?? int.MaxValue;

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

                    if (counter * 20 % 100 == 0)
                    {
                        db.SaveChanges();
                    }

                    counter++;
                    await Task.Delay(1500);
                }

                db.SaveChanges();
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Details", new { username });
            }
            catch (ArgumentNullException e)
            {
                ModelState.AddModelError("Error", "Invalid username");
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return View();
            }
        }

        // GET: Snoop/Details/5
        [Authorize]
        public ActionResult Details(string username,
                                    string currentFilter,
                                    string searchString,
                                    int? page)
        {
            try
            {
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

                return RedirectToAction("Scoop", "Snoop");
            }
        }

        // GET: Snoop
        [Authorize]
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
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
