using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Zoans.Services;
using Zoans.Models;
using MongoDB.Driver;
using System.Security.Claims;

namespace Zoans.Controllers
{
    public class ItemController : Controller
    {
        private MongoDbService mongo;
        //injecting MDB so this controller load and modify items in MDB database
        public ItemController(MongoDbService m)
        {
            mongo = m;
        }

        public async Task<IActionResult> Details(string id)
        {
            //load item using unique id
            var item = await mongo.GetItems().Find(i => i.Id == id).FirstOrDefaultAsync();
            return View(item);
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> Rate(string itemId, int rating)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //fetching the item being rated from MDB
            var item = await mongo.GetItems().Find(i => i.Id == itemId).FirstOrDefaultAsync();
            //dictionary <userId, rating)
            item.UserRatings[userId] = rating;
            
            double sum = 0;//summing manually so it doesntt break and i have control
            foreach (int r in item.UserRatings.Values)
            {
                sum += r;
            }
            // updating caluclated rating feilds
            item.RatingSum = sum;
            item.RatingCount = item.UserRatings.Count;
            item.AverageRating = sum / item.RatingCount;
            // saving updated item back to MDB
            await mongo.GetItems().ReplaceOneAsync(i => i.Id == itemId, item);

            var update = Builders<User>.Update.Set("RatingsGiven." + itemId, rating);
            await mongo.GetUsers().UpdateOneAsync(u => u.Id == userId, update);

            return RedirectToAction("Details", new { id = itemId });
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> Review(string itemId, string content)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string username = User.FindFirstValue(ClaimTypes.Name);
            //loading item so we can add review on it
            var item = await mongo.GetItems().Find(i => i.Id == itemId).FirstOrDefaultAsync();

            Review existing = null;// checking if we have already gave a review on that item
            foreach (Review r in item.Reviews)
            {
                if (r.UserId == userId)
                {
                    existing = r;
                }
            }

            if (existing != null)
            {
                // time stampped added just bcz to make it a little more clearer
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
                existing.Content = $"{existing.Content} (edited on {timestamp}): {content}";
            }
            else
            {
                var newReview = new Review//user is writing review for 1st time
                {
                    UserId = userId,
                    Username = username,
                    Content = content
                };

                item.Reviews.Add(newReview);
            }

            await mongo.GetItems().ReplaceOneAsync(i => i.Id == itemId, item);

            return RedirectToAction("Details", new { id = itemId });
        }
    }
}
