using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/StaffAdmin")]
    [Route("StaffAdmin")]
    public class StaffAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [Route("")]
        [Route("ListStaff")]

        public IActionResult ListStaff(int? page, string searchString)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var nhanvien = db.Staff.AsNoTracking().OrderBy(x => x.FullName);

            if (!String.IsNullOrEmpty(searchString))
            {
                nhanvien = nhanvien.Where(s => s.FullName.Contains(searchString)).OrderBy(x => x.FullName);
            }

            PagedList<Staff> lst = new PagedList<Staff>(nhanvien, pageNumber, pageSize);
            return View(lst);
        }

        [Route("CreateStaff")]
        [HttpGet]
        public IActionResult CreateStaff()
        {
            return View();
        }
        [Route("CreateStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStaff(Staff nhanvien)
        {
            if (ModelState.IsValid)
            {
                db.Staff.Add(nhanvien);
                db.SaveChanges();
                return RedirectToAction("ListStaff");
            }
            return View(nhanvien);
        }


        [Route("EditStaff")]
        [HttpGet]
        public IActionResult EditStaff(int maNhanVien)
        {
            var nhanvien = db.Staff.Find(maNhanVien);
            return View(nhanvien);
        }
        [Route("EditStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStaff(Staff nhanvien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanvien).State = EntityState.Modified;
                // db.Update(nhanvien);
                db.SaveChanges();
                return RedirectToAction("ListStaff");
            }
            return View(nhanvien);
        }



        [Route("DeleteStaff")]
        [HttpGet]
        public IActionResult DeleteStaff(int maNhanVien)

        {
            TempData["Message"] = "";
            // Lấy các bản ghi Borrow liên quan đến StaffId
            var borrow = db.BorrowReturns.Where(x => x.StaffId == maNhanVien).ToList();

            if (borrow.Any())
            {
                
                var borrowIds = borrow.Select(b => b.BorrowReturnId).ToList();
               

                // Xóa các bản ghi Returns liên quan
               

                // Xóa các bản ghi Borrow
                db.BorrowReturns.RemoveRange(borrow);
            }

            // Xóa bản ghi Staff
            var staff = db.Staff.Find(maNhanVien);
            if (staff != null)
            {
                db.Staff.Remove(staff);
            }

            db.SaveChanges();
            TempData["Message"] = "Nhân viên đã được xóa";
            return RedirectToAction("StaffAdmin", "admin");
        }

        [Route("DetailsStaff")]
        [HttpGet]
        public IActionResult DetailsStaff(int maNhanVien)
        {
            var nhanvien = db.Staff.FirstOrDefault(m => m.StaffId == maNhanVien);
            if (nhanvien == null)
            {
                return NotFound();
            }
            return View(nhanvien);

        }


    }


}
