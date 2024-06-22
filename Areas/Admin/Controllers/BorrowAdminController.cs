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
        [HttpPost]
        [Route("CreateBorrow")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBorrow(BorrowReturn borrowReturn)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu ngày mượn không phải hôm nay hoặc lớn hơn hôm nay
                if (borrowReturn.BorrowDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    TempData["ErrorMessage"] = "Ngày mượn phải là ngày hiện tại hoặc lớn hơn.";
                    return RedirectToAction("CreateBorrow");
                }

                // Kiểm tra nếu hạn trả không lớn hơn ngày mượn
                if (borrowReturn.DueDate <= borrowReturn.BorrowDate)
                {
                    TempData["ErrorMessage"] = "Hạn trả phải lớn hơn ngày mượn.";
                    return RedirectToAction("CreateBorrow");
                }

                // Truy vấn số lượng sách trong bảng Books
                var book = db.Books.FirstOrDefault(b => b.BookId == borrowReturn.BookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = "Sách không tồn tại.";
                    return RedirectToAction("CreateBorrow");
                }

                // Kiểm tra tổng số lượng sách độc giả đang mượn
                var totalBooksBorrowedByReader = db.BorrowReturns
                    .Where(br => br.ReaderId == borrowReturn.ReaderId && br.ReturnDate == null)
                    .Sum(br => br.BookNumber);

                // Nếu tổng số lượng sách mượn cộng thêm số lượng sách mới vượt quá 3, trả về lỗi
                if (totalBooksBorrowedByReader + borrowReturn.BookNumber > 3)
                {
                    TempData["ErrorMessage"] = "Mỗi độc giả chỉ được mượn tối đa 3 cuốn sách.";
                    return RedirectToAction("CreateBorrow");
                }
                // Truy vấn số lượng sách đang mượn và chưa trả trong bảng BorrowReturns
                var totalBorrowed = db.BorrowReturns
                                      .Where(br => br.BookId == borrowReturn.BookId && br.ReturnDate == null)
                                      .Sum(br => br.BookNumber);

                // Kiểm tra nếu số lượng sách đủ để mượn
                if (book.Quantity - totalBorrowed < borrowReturn.BookNumber)
                {
                    TempData["ErrorMessage"] = "Không đủ sách để mượn.";
                    return RedirectToAction("CreateBorrow");
                }

                // Nếu đủ sách, thêm bản ghi mượn sách mới
                db.BorrowReturns.Add(borrowReturn);

                // Cập nhật trường Available
                book.Available = book.Quantity - (totalBorrowed + borrowReturn.BookNumber);

                db.SaveChanges();
                TempData["Message"] = "Mượn sách thành công.";
                return RedirectToAction("ListBorrow");
            }

            // Nếu ModelState không hợp lệ, trả về lại view với các thông báo lỗi
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



        [HttpPost]
        [Route("EditBorrow/{id}")]
       
        public IActionResult EditBorrow(int id, BorrowReturn updatedBorrowReturn)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu hạn trả không lớn hơn ngày mượn
                if (updatedBorrowReturn.DueDate <= updatedBorrowReturn.BorrowDate)
                {
                    TempData["ErrorMessage"] = "Hạn trả phải lớn hơn ngày mượn.";
                    return RedirectToAction("EditBorrow", new { id = id });
                }

                // Truy vấn phiếu mượn sách cần chỉnh sửa
                var existingBorrowReturn = db.BorrowReturns.Find(id);
                if (existingBorrowReturn == null)
                {
                    return NotFound();
                }

                // Kiểm tra nếu sách đã trả
                if (existingBorrowReturn.ReturnDate != null)
                {
                    TempData["ErrorMessage"] = "Không thể sửa thông tin mượn sách đã trả.";
                    return RedirectToAction("EditBorrow", new { id = id });
                }

                // Truy vấn sách cũ và sách mới
                var oldBook = db.Books.FirstOrDefault(b => b.BookId == existingBorrowReturn.BookId);
                var newBook = db.Books.FirstOrDefault(b => b.BookId == updatedBorrowReturn.BookId);

                if (oldBook == null || newBook == null)
                {
                    TempData["ErrorMessage"] = "Sách không tồn tại.";
                    return RedirectToAction("EditBorrow", new { id = id });
                }

                // Số lượng sách đã mượn trước khi cập nhật
                var previousBorrowNumber = existingBorrowReturn.BookNumber;

                // Tổng số lượng sách đang được mượn (trừ phiếu mượn cần chỉnh sửa)
                var totalBorrowedOldBook = db.BorrowReturns
                                              .Where(br => br.BookId == oldBook.BookId && br.ReturnDate == null && br.BorrowReturnId != id)
                                              .Sum(br => br.BookNumber);
                var totalBorrowedNewBook = db.BorrowReturns
                                              .Where(br => br.BookId == newBook.BookId && br.ReturnDate == null)
                                              .Sum(br => br.BookNumber);

                // Kiểm tra nếu số lượng sách đủ để mượn sau khi cập nhật
                if (newBook.Quantity - totalBorrowedNewBook < updatedBorrowReturn.BookNumber)
                {
                    TempData["ErrorMessage"] = "Không đủ sách để mượn.";
                    return RedirectToAction("EditBorrow", new { id = id });
                }

                // Kiểm tra tổng số lượng sách độc giả đang mượn (trừ phiếu mượn cần chỉnh sửa)
                var totalBooksBorrowedByReader = db.BorrowReturns
                    .Where(br => br.ReaderId == updatedBorrowReturn.ReaderId && br.BorrowReturnId != id && br.ReturnDate == null)
                    .Sum(br => br.BookNumber);

                // Nếu tổng số lượng sách mượn cộng thêm số lượng sách mới vượt quá 3, trả về lỗi
                if (totalBooksBorrowedByReader + updatedBorrowReturn.BookNumber > 3)
                {
                    TempData["ErrorMessage"] = "Mỗi độc giả chỉ được mượn tối đa 3 cuốn sách.";
                    return RedirectToAction("EditBorrow", new { id = id });
                }

                // Cập nhật thông tin mượn sách
                existingBorrowReturn.BorrowDate = updatedBorrowReturn.BorrowDate;
                existingBorrowReturn.DueDate = updatedBorrowReturn.DueDate;
                existingBorrowReturn.BookNumber = updatedBorrowReturn.BookNumber;
                existingBorrowReturn.BookId = updatedBorrowReturn.BookId;
                existingBorrowReturn.StaffId = updatedBorrowReturn.StaffId;
                existingBorrowReturn.ReaderId = updatedBorrowReturn.ReaderId;

                // Cập nhật lại số sách có sẵn cho sách cũ và sách mới
                oldBook.Available += previousBorrowNumber;
                newBook.Available -= updatedBorrowReturn.BookNumber;

                db.SaveChanges();
                TempData["Message"] = "Cập nhật mượn sách thành công.";
                return RedirectToAction("ListBorrow");
            }

            // Nếu ModelState không hợp lệ, trả về lại view với các thông báo lỗi
            ViewBag.ReaderId = new SelectList(db.Readers.ToList(), "ReaderId", "ReaderId");
            ViewBag.BookId = new SelectList(db.Books.ToList(), "BookId", "BookId");
            ViewBag.StaffId = new SelectList(db.Staff.ToList(), "StaffId", "StaffId");

            return View(updatedBorrowReturn);
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
                if (muonsach.ReturnDate == null)
                {
                    TempData["ErrorMessage"] = "Sách chưa được trả nên không thể xóa.";
                    return RedirectToAction("DeleteBorrow", new { id = id });
                }

                db.BorrowReturns.Remove(muonsach);
                db.SaveChanges();
                TempData["Message"] = "Thông tin mượn sách của độc giả đã được xóa";
            }
            return RedirectToAction("ListBorrow");
        }
    }
}