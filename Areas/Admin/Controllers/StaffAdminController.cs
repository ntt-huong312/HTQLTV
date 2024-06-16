using HTQLTV.Models;
using HTQLTV.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/StaffAdmin")]
    [Route("StaffAdmin")]
    //[Authorize(Policy = "AdminPolicy")]
    public class StaffAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [Route("")]
        [Route("ListStaff")]
        [Authentication]
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
        [Authentication]
        public IActionResult CreateStaff()
        {
            return View();
        }
        [Route("CreateStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]
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
        [Authentication]
        public IActionResult EditStaff(int maNhanVien)
        {
            var nhanvien = db.Staff.Find(maNhanVien);
            return View(nhanvien);
        }
        [Route("EditStaff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]
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


        [HttpGet]
        [Route("DeleteStaff")]
        // [Authentication]
        public IActionResult DeleteStaff(int maNhanVien)
        {
            var staff = db.Staff
                          .Include(s => s.BorrowReturns) // Include related BorrowReturns
                          .FirstOrDefault(s => s.StaffId == maNhanVien);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteStaff")]
        public IActionResult DeleteStaffConfirmed(int maNhanVien)
        {
            TempData["Message"] = "";

            // Lấy các bản ghi Borrow liên quan đến StaffId
            var borrowRecords = db.BorrowReturns.Where(x => x.StaffId == maNhanVien).ToList();

            // Kiểm tra xem có bản ghi nào có ReturnDate là null không
            if (borrowRecords.Any(br => br.ReturnDate == null))
            {
                TempData["ErrorMessage"] = "Không thể xóa nhân viên này vì có sách chưa được trả.";
                return RedirectToAction("DeleteStaff", new { maNhanVien = maNhanVien });
            }

            // Xóa các bản ghi Borrow
            db.BorrowReturns.RemoveRange(borrowRecords);

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
        [Authentication]
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
