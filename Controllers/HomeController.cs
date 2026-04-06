using Microsoft.AspNetCore.Mvc;
using Zoans.Services;
using Zoans.Models;
using MongoDB.Driver;

namespace Zoans.Controllers
{
    public class HomeController : Controller
    {
        private MongoDbService mongo;
        // injecting MDB service so controller load items from MDB
        public HomeController(MongoDbService m)
        {
            mongo = m;
        }

        public async Task<IActionResult> Index(string category)
        {
            List<Items> items;

            // no category selected, load all 
            if (category == null)
            {
                items = await mongo.GetItems().Find(_ => true).ToListAsync();// i think Find(_=> true) is simplest for fetching
            }
            else
            {
                items = await mongo.GetItems().Find(i => i.Category == category).ToListAsync();
            }
            //passing the category to view so frontend can highlight it 
            ViewBag.Category = category;
            return View(items);
        }
    }
}
