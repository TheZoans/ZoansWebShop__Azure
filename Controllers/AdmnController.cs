using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Zoans.Services;
using Zoans.Models;
using MongoDB.Driver;

namespace Zoans.Controllers
{

    // only fo user with the admin controlcan access this 
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private MongoDbService mongo;
        //injecting MDBService so controller can access both items and users collection
        public AdminController(MongoDbService m)
        {
            mongo = m;
        }

        public async Task<IActionResult> Index()
        {
            //load all items and user so the admin dashboard can see it together
            ViewBag.Items = await mongo.GetItems().Find(_ => true).ToListAsync();
            ViewBag.Users = await mongo.GetUsers().Find(_ => true).ToListAsync();
            return View();
        }

        public IActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(string name, string description, decimal price, string seller, string imageUrl, string category, string condition, string batteryLife, int? age, string size, string material)
        {

            // ill create item obj manually bcz each feild comes individually from the assigment. i think i can use model binding aswell but ill stick to creating manually
            Items item = new Items();
            item.Name = name;
            item.Seller = seller;
            item.Price = price;
            item.ImageURL = imageUrl;
            item.Category = category;
            item.Condition = condition;
            item.BatteryLife = batteryLife;
            item.Age = age;
            item.Size = size;
            item.Material = material;
            item.Description = description;
            // inserting new item to MDB
            await mongo.GetItems().InsertOneAsync(item);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(string id)
        {
            await mongo.GetItems().DeleteOneAsync(i => i.Id == id);
            return RedirectToAction("Index");
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string username, string password, string role)
        {
            User user = new User();
            user.Username = username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);// passwd is hashed for security
            user.Role = role;

            await mongo.GetUsers().InsertOneAsync(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(string id)
        {
            await mongo.GetUsers().DeleteOneAsync(u => u.Id == id);
            return RedirectToAction("Index");
        }
    }
}