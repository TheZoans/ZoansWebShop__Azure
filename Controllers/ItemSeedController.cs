using Microsoft.AspNetCore.Mvc;
using Zoans.Services;
using Zoans.Models;

namespace Zoans.Controllers
{

    // This class is just for seeing items to db initially
    // Maybe ill delete it after seeding or keep it.. IDK
    public class ItemSeedController : Controller
    {
        private MongoDbService db;

        public ItemSeedController(MongoDbService m)
        {
            db = m;
        }

        public async Task<IActionResult> Index()
        {
            
            Items i1 = new Items();
            i1.Name = "Vinyl CD";
            i1.Description = "CD";
            i1.Price = 40;
            i1.Seller = "Rock Music";
            i1.ImageURL = "";
            i1.Category = "Vinyls";
            i1.Condition = "Used";
            i1.Age = 30;
            await db.GetItems().InsertOneAsync(i1);

            
            Items i2 = new Items();
            i2.Name = "Wooden Table";
            i2.Description = "Vintage wooden Table";
            i2.Price = 45;
            i2.Seller = "ZM Furniture";
            i2.ImageURL = "";
            i2.Category = "AntiqueFurniture";
            i2.Condition = "Used";
            i2.Age = 54;
            i2.Material = "Wood";
            await db.GetItems().InsertOneAsync(i2);

            
            Items i3 = new Items();
            i3.Name = "Apple Watch";
            i3.Description = "Sports GPS watch";
            i3.Price = 349;
            i3.Seller = "XYZ";
            i3.ImageURL = "";
            i3.Category = "GPSSportWatches";
            i3.Condition = "New";
            i3.BatteryLife = "15 hours";
            await db.GetItems().InsertOneAsync(i3);

            
            Items i4 = new Items();
            i4.Name = "Adidas Sambas";
            i4.Description = "Good for jogging";
            i4.Price = 90;
            i4.Seller = "Adidas";
            i4.ImageURL = "";
            i4.Category = "RunningShoes";
            i4.Size = "42";
            i4.Material = "Leather suede";
            await db.GetItems().InsertOneAsync(i4);

            
            Items i5 = new Items();
            i5.Name = "Dometic Camping Tent";
            i5.Description = "2-person tent";
            i5.Price = 1199;
            i5.Seller = "Dometic";
            i5.ImageURL = "";
            i5.Category = "CampingTents";
            i5.Condition = "New";
            await db.GetItems().InsertOneAsync(i5);

            return Content("Items added!");
        }
    }
}