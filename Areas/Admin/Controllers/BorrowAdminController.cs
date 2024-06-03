using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Azure;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/BorrowAdmin")]
    public class BorrowAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        // GET: Borrow
        [HttpGet]
        [Route("ListBorrowReturn")]
        public IActionResult ListBorrowReturn(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var borrowReturns = db.BorrowReturns.Include(br => br.Book)
                                                 .Include(br => br.Reader)
                                                 .Include(br => br.Staff)
                                                 .ToPagedList(pageNumber, pageSize);
            return View(borrowReturns);
        }

        // GET: Borrow/Create
        [HttpGet]
        [Route("CreateBorrow")]
        public IActionResult CreateBorrow()
        {
            return View();
        }

        // POST: Borrow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBorrow(BorrowReturn borrowReturn)
        {
            if (ModelState.IsValid)
            {
                // Giả sử User.Identity.Name là tên đăng nhập của nhân viên hiện tại
                var username = User.Identity.Name;
                var currentStaff = db.Staff.FirstOrDefault(s => s.Email == username);

                if (currentStaff != null)
                {
                    borrowReturn.StaffId = currentStaff.StaffId;
                    // Tạo StatId mới hoặc thiết lập giá trị phù hợp
                    borrowReturn.StatId = Guid.NewGuid().ToString();
                    db.BorrowReturns.Add(borrowReturn);
                    db.SaveChanges();
                    return RedirectToAction("ListBorrowReturn");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể xác định nhân viên hiện tại.");
                }
            }

            return View(borrowReturn);
        }
    }
}
