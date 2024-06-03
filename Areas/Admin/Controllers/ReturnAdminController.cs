using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/ReturnAdmin")]
    public class ReturnAdminController : Controller
    {
        
        HtqltvContext db = new HtqltvContext();
        // GET: Return/ListBorrowReturn
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

        // GET: Return/Edit/{id}
        [HttpGet]
        [Route("return/{id}")]
        public IActionResult Return(int id)
        {
            var borrowReturn = db.BorrowReturns.Include(br => br.Book)
                                                .Include(br => br.Reader)
                                                .Include(br => br.Staff)
                                                .FirstOrDefault(br => br.BorrowReturnId == id);
            if (borrowReturn == null)
            {
                return NotFound();
            }

            return View(borrowReturn);
        }

        // POST: Return/Edit/{id}]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("return/{id}")]
        public IActionResult Return(int id, BorrowReturn borrowReturn)
        {
            if (ModelState.IsValid)
            {
                var existingBorrowReturn = db.BorrowReturns.Find(id);
                if (existingBorrowReturn != null)
                {
                    existingBorrowReturn.ReturnDate = borrowReturn.ReturnDate;
                    db.SaveChanges();
                    return RedirectToAction("ListBorrowReturn");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy bản ghi mượn trả.");
                }
            }

            return View(borrowReturn);
        }

    }
}
