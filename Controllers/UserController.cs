using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Zoans.Services;
using Zoans.Models;
using MongoDB.Driver;
using System.Security.Claims;

namespace Zoans.Controllers
{
    
    [Authorize(Roles = "user")]//only logged in user can access thier profile
    public class UserController : Controller
    {
        private MongoDbService db;

        public UserController(MongoDbService m)
        {
            db = m;
        }

        public async Task<IActionResult> Profile()//getting ucurrent user id from claims
        {
            string uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //well fetch full user doc for logged in user
            User currentUser = await db.GetUsers().Find(u => u.Id == uid).FirstOrDefaultAsync();

            List<Items> items = await db.GetItems().Find(_ => true).ToListAsync();
            
            List<Review> userReviews = new List<Review>();//collecting all reviews submitted
            //using nested loops since each item has multiple reviews
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].Reviews.Count; j++)
                {
                    if (items[i].Reviews[j].UserId == uid)
                    {
                        userReviews.Add(items[i].Reviews[j]);
                    }
                }
            }
            //calculating avg rating user has given
            double total = 0;
            foreach (int r in currentUser.RatingsGiven.Values)
            {
                total = total + r;
            }

            double avg = 0;
            if (currentUser.RatingsGiven.Count > 0)
            {
                avg = total / currentUser.RatingsGiven.Count;
            }

            ViewBag.User = currentUser;
            ViewBag.Reviews = userReviews;
            ViewBag.AvgRating = avg;

            return View();
        }
    }
}
