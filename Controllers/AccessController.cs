using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;

namespace HTQLTV.Controllers
{
    public class AccessController : Controller
    {
        private readonly HtqltvContext db;
        public AccessController(HtqltvContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("Username") ==null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public IActionResult Login(User user) 
        {
            if(HttpContext.Session.GetString("Username")== null)
            {
                var u = db.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
                if(u!=null)
                {
                    HttpContext.Session.SetString("Username", u.Username);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            //HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}
