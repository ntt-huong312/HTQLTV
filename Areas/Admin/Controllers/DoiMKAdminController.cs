using System;
using System.Threading.Tasks;
using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HTQLTV.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using System.Linq;


namespace HTQLTV.Areas.Admin.Controllers
{
    
    
    public class DoiMKAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [Area("admin")]
        [HttpGet]
        [Route("admin/DoiMKAdmin")]
        public IActionResult DoiMK()
        {
            return View();
        }



        [Area("admin")]
        [HttpPost]
        [Route("admin/DoiMKAdmin")]
        public IActionResult DoiMK(ChangePasswordViewModel model)
        {
            var user =db.Users.FirstOrDefault(c=> c.Email == model.Email);
            if (user != null && model.CurrentPassword != null && model.NewPassword != null && model.ConfirmPassword != null )
            {
                if (user != null && user.Password == model.CurrentPassword)
                {
                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        TempData["ErrorMessage"] = "Mật khẩu mới không đúng";
                        return View(model);
                    }

                    user.Password = model.NewPassword;
                    db.SaveChanges();

                    return RedirectToAction("Index", "StatisticAdmin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }
            return View(model);
        }

      
        /****************************************
        [Route("DoiMK")]
        public IActionResult DoiMK()
        {
            return View();
        }

        [HttpPost("DoiMK")]
        public IActionResult DoiMK(User model)
        {
            if (ModelState.IsValid)
            {
                
            }
            return View(model);
        }
        ****************************/
        
        /*[Route("DoiMK")]
        [HttpGet]

        public IActionResult DoiMK(int id)
        {
            

            var sp = db.Users.Find(id);
            return View(sp);
        }

        [Route("DoiMK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]
        public IActionResult DoiMK(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "HomeAdmin");

            }

            return View(user);
        }*/
    }
}
