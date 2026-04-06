using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Zoans.Services;
using Zoans.Models;
using MongoDB.Driver;

namespace Zoans.Controllers
{

    // this controller will handle things related to the user accounts like login, logout etc
    public class AccountController : Controller
    {
        private MongoDbService mongo;
        // here im will inject my custom MongoDbServive to access usr collection
        public AccountController(MongoDbService m)
        {
            mongo = m;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await mongo.GetUsers().Find(u => u.Username == username).FirstOrDefaultAsync();
            // simple login check and BCrypt to verify the hashed psswd in mongodb
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Wrong username or password";
                return View();
            }

            // if login succeeds, i create claims set that will become part of authentication cookie
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);
            var loginTime = DateTime.Now; // keeping it incase i need to edit and use it later somewhere
            await HttpContext.SignInAsync("Cookies", principal);

            return RedirectToAction("Index", "Home");
        }
        // logss out user by removing authentication cookie
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
