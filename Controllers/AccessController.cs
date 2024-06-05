using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;

namespace HTQLTV.Controllers
{
    public class AccessController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UserName")==null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "admin");
            }
        }

        [HttpPost]
        public IActionResult Login(User user) 
        {
            if(HttpContext.Session.GetString("Username")== null)
            {
                var u=db.Users.Where(x=>x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
                if(u!=null)
                {
                    HttpContext.Session.SetString("Username", u.Username.ToString());
                    return RedirectToAction("Index", "admin");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Access");
        }
    }
}
