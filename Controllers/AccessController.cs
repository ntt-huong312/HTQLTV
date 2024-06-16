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
                return RedirectToAction("StatisticAdmin", "Admin");
            }
        }

        [HttpPost]
        public IActionResult Login(User user) 
        {
            if(HttpContext.Session.GetString("Username")== null)
            {
               if(user.Username != null && user.Password != null)
                {
                    var u = db.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
                    if (u != null)
                    {
                        HttpContext.Session.SetString("Username", u.Username);
                        HttpContext.Session.SetInt32("RoleID", u.RoleID);
                        return RedirectToAction("StatisticAdmin", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập và mật khẩu phải được điền");
                }
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}
