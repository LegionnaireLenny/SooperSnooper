using SooperSnooper.Models;
using SooperSnooper.Models.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SooperSnooper.Controllers
{
    public class SnoopController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Snoop/Create
        public ActionResult Snoop()
        {
            return View();
        }

        // POST: Snoop/Create
        [HttpPost]
        public async Task<ActionResult> Snoop(string username)
        {
            try
            {
                string accountUrl = $"/{username}";
                UserTweet scrapedTweets = await Scraper.ScrapeUserLink(accountUrl);

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
                return RedirectToAction("Index", "Home");
                //return RedirectToAction("Details", new { id = user.Username });
            }
            catch (ArgumentNullException e)
            {
                ModelState.AddModelError("Error", e.Message);
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return View();
            }
        }

        // GET: Snoop/Details/5
        public ActionResult Details(string username)
        {
            var tweets = db.Tweets.Where(m => m.Username == username).ToList();

            return View(tweets);
        }

        // GET: Snoop
        public ActionResult Scoops()
        {
            try
            {
                if (!db.TwitterUsers.Any())
                {
                    throw new ArgumentNullException();
                }

                UserList userList = new UserList()
                {
                    Users = db.TwitterUsers.ToList()
                };

                return View(userList);
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        // GET: Snoop/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Snoop/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
