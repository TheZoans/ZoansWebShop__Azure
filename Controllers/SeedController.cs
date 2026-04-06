using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using Zoans.Models;
using Zoans.Services;


//this class is also just for seeding user and admin
//ill delte it maybe afterwards bcz ill seed directly from the ui once its made
namespace Zoans.Controllers
{
    public class SeedController : Controller
    {
        private MongoDbService db;

        public SeedController(MongoDbService m)
        {
            db = m;
        }

        public async Task<IActionResult> Index()
        {
            User admin = new User();
            admin.Username = "admin";
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
            admin.Role = "admin";
            await db.GetUsers().InsertOneAsync(admin);

            User Dometic = new User();
            Dometic.Username = "Dometic";
            Dometic.PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234");
            Dometic.Role = "user";
            await db.GetUsers().InsertOneAsync(Dometic);

            User XYZ = new User();
            XYZ.Username = "XYZ";
            XYZ.PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234");
            XYZ.Role = "user";
            await db.GetUsers().InsertOneAsync(XYZ);

            return Content("Users created successfully!");
        }
    }
}
