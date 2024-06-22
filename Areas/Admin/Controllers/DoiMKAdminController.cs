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
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                return View(model);
            }

            // Tìm người dùng theo email
            var user = db.Users.FirstOrDefault(c => c.Email == model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng với email này.";
                return View(model);
            }

            // Kiểm tra mật khẩu hiện tại
            if (user.Password != model.CurrentPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không chính xác.";
                return View(model);
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu có khớp không
            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới không khớp.";
                return View(model);
            }

            // Cập nhật mật khẩu mới
            user.Password = model.NewPassword;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công.";
            return RedirectToAction("Index", "StatisticAdmin");
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
