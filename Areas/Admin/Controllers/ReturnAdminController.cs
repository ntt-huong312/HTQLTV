

using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/ReturnAdmin")]
    public class ReturnAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        
        [HttpGet]
        [Route("ListReturn")]
        public IActionResult ListReturn(int? page, int? readerId)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var borrowReturns = db.BorrowReturns.Include(br => br.Book)
                                                      .Include(br => br.Reader)
                                                      .Include(br => br.Staff)
                                                      .AsQueryable();

            if (readerId.HasValue)
            {
                borrowReturns = borrowReturns.Where(br => br.ReaderId == readerId.Value);
            }

            var pagedList = borrowReturns.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: ReturnAdmin/Return/{id}
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

            ViewBag.ReaderId = new SelectList(db.Readers, "ReaderId", "ReaderId", borrowReturn.ReaderId);
            ViewBag.BookId = new SelectList(db.Books, "BookId", "BookId", borrowReturn.BookId);
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "StaffId", borrowReturn.StaffId);
          

            return View(borrowReturn);
        }

        // POST: ReturnAdmin/Return/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("return/{id}")]
        public IActionResult Return(int id, BorrowReturn borrowReturn)
        {
          
                var existingBorrowReturn = db.BorrowReturns.Find(id);
                if (existingBorrowReturn != null)
                {
                    existingBorrowReturn.ReturnDate = borrowReturn.ReturnDate;
                    db.SaveChanges();
                    return RedirectToAction("ListReturn");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy bản ghi mượn trả.");
                }
           

            ViewBag.ReaderId = new SelectList(db.Readers, "ReaderId", "ReaderId", borrowReturn.ReaderId);
            ViewBag.BookId = new SelectList(db.Books, "BookId", "BookId", borrowReturn.BookId);
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "StaffId", borrowReturn.StaffId);
         

            return View(borrowReturn);
        }
    }
}

