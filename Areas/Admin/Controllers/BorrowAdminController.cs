
using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using HTQLTV.Models.Authentication;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/BorrowAdmin")]
    public class BorrowAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        // GET: Borrow w
        [HttpGet]
        [Route("ListBorrow")]
        public IActionResult ListBorrow(int? page, int? readerId)
        {

            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var borrowReturns = db.BorrowReturns.Include(br => br.Book)
                                                      .Include(br => br.Reader)

                                                      .Include(br => br.Staff)


                                                      .Include(br => br.Staff)

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
            ViewBag.ReaderId = new SelectList(db.Readers.ToList(), "ReaderId", "ReaderId");
            ViewBag.BookId = new SelectList(db.Books.ToList(), "BookId", "BookId");
            ViewBag.StaffId = new SelectList(db.Staff.ToList(), "StaffId", "StaffId");

            return View();
        }

        // POST: Borrow/Create

        //  [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("CreateBorrow")]

        [ValidateAntiForgeryToken]
        public IActionResult CreateBorrow(BorrowReturn borrowReturn)
        {

            //if (ModelState.IsValid)
            //{
            db.BorrowReturns.Add(borrowReturn);
            db.SaveChanges();
            return RedirectToAction("ListBorrow");
            //   }

            ViewBag.ReaderId = new SelectList(db.Readers.ToList(), "ReaderId", "ReaderId");
            ViewBag.BookId = new SelectList(db.Books.ToList(), "BookId", "BookId");
            ViewBag.StaffId = new SelectList(db.Staff.ToList(), "StaffId", "StaffId");


            return View(borrowReturn);

        }

        // GET: ReturnAdmin/Return/{id}
        [HttpGet]
        [Route("EditBorrow/{id}")]
        public IActionResult EditBorrow(int id)
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
        [Route("EditBorrow/{id}")]
        public IActionResult EditBorrow(BorrowReturn borrowReturn)
        {



            db.Entry(borrowReturn).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListBorrow");


            ViewBag.ReaderId = new SelectList(db.Readers, "ReaderId", "ReaderId", borrowReturn.ReaderId);
            ViewBag.BookId = new SelectList(db.Books, "BookId", "BookId", borrowReturn.BookId);
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "StaffId", borrowReturn.StaffId);

            return View(borrowReturn);
        }




        [Route("DetailsBorrow")]
        [HttpGet]
        [Authentication]
        public IActionResult DetailsBorrow(int id)
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


        [HttpGet]
        [Route("DeleteBorrow/{id}")]
        public IActionResult DeleteBorrow(int id)
        {
            var muonsach = db.BorrowReturns
                             .Include(br => br.Book)
                             .Include(br => br.Reader)
                             .Include(br => br.Staff)
                             .FirstOrDefault(br => br.BorrowReturnId == id);

            if (muonsach == null)
            {
                return NotFound();
            }

            return View(muonsach);
        }

        [HttpPost]
        [Route("DeleteBorrow/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBorrowConfirmed(int id)
        {
            var muonsach = db.BorrowReturns.Find(id);
            if (muonsach != null)
            {
                db.BorrowReturns.Remove(muonsach);
                db.SaveChanges();
                TempData["Message"] = "Thông tin mượn sách của độc giả đã được xóa";
            }
            return RedirectToAction("ListBorrow");
        }
    }
}