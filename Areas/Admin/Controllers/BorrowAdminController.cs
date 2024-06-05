

using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/BorrowAdmin")]
    public class BorrowAdminController : Controller
    {
       HtqltvContext db = new HtqltvContext();

        // GET: Borrow
        [HttpGet]
        [Route("ListBorrow")]
        public IActionResult ListBorrow(int ?page, int?readerId)
        {

            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var borrowReturns = db.BorrowReturns.Include(br => br.Book)
                                                      .Include(br => br.Reader)
                                                      .Include(br => br.Staff)
                                                      .Include(br=>br.Stat)
                                                      .AsQueryable();



            if (readerId.HasValue)
            {
                borrowReturns = borrowReturns.Where(br => br.ReaderId == readerId.Value);
            }

            var pagedList = borrowReturns.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: Borrow/Create
        [HttpGet]
        [Route("CreateBorrow")]
        public IActionResult CreateBorrow()
        {
            ViewBag.ReaderId = new SelectList(db.Readers.ToList(), "ReaderId","ReaderId");
            ViewBag.BookId = new SelectList(db.Books.ToList(), "BookId", "BookId");
            ViewBag.StaffId = new SelectList(db.Staff.ToList(), "StaffId", "StaffId");
          //  ViewBag.StatId = new SelectList(db.Statistics.ToList(), "StatId", "StatId");
            return View();
        }

        // POST: Borrow/Create

        //  [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("CreateBorrow")]
       
        [ValidateAntiForgeryToken]
        public IActionResult CreateBorrow(BorrowReturn borrowReturn)
        {
            
          //  if (ModelState.IsValid)
           // {
                db.BorrowReturns.Add(borrowReturn);
                db.SaveChanges();
                return RedirectToAction("ListBorrow");
          //  }
            // Ghi lại các lỗi trong ModelState
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            //foreach (var error in errors)
            //{
            //    Console.WriteLine(error.ErrorMessage);
            //}

            ViewBag.ReaderId = new SelectList(db.Readers.ToList(), "ReaderId", "ReaderId");
            ViewBag.BookId = new SelectList(db.Books.ToList(), "BookId", "BookId");
            ViewBag.StaffId = new SelectList(db.Staff.ToList(), "StaffId", "StaffId");
         //   ViewBag.StatId = new SelectList(db.Statistics.ToList(), "StatId", "StatId");

            return View(borrowReturn);
        }

    }
}

