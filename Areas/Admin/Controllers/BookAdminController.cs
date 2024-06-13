using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Data;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using HTQLTV.Models.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;


namespace HTQLTV.Areas.Admin.Controllers
{


    [Area("admin")]
    [Route("admin/BookAdmin")]
    //[Authorize(Roles = "Admin,Staff")]
    public class BookAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [Route("ListBook")]
        [Authentication]
        public IActionResult ListBook(int? page, string searchString)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var listbook = db.Books.Include(b => b.Category).AsNoTracking().OrderBy(x => x.Title);

            if (!string.IsNullOrEmpty(searchString))
            {
                listbook = listbook.Where(s => s.Title.Contains(searchString)).OrderBy(x => x.Title);
            }

            PagedList<Book> lst = new PagedList<Book>(listbook, pageNumber, pageSize);
            return View(lst);
        }

        //public IActionResult ImageUpload(string? path)
        //{
        //    Book model = new Book { BookImage = path };
        //    return View(model);
        //}
        [HttpGet]
        [Route("CreateBook")]
        public IActionResult CreateBook()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }


        [HttpPost]
        [Route("CreateBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(Book book)
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            if (book == null)
            {
                TempData["ErrorMessage"] = "Sách đã tồn tại.";
                return BadRequest("Book object is null.");
            }
            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ, trả về cùng với các lỗi xác thực
                return View(book);
            }

            if (book.file != null && book.file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/books", book.file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await book.file.CopyToAsync(stream);
                }
                book.BookImage =  book.file.FileName; 
            }
            
            db.Books.Add(book);
            await db.SaveChangesAsync();
            TempData["Message"] = "Thêm thành công";
            return RedirectToAction("ListBook");
        }

        [Route("DeleteBook/{bookId}")]
        [HttpGet]
        public IActionResult DeleteBook(int bookId)
        {
            var book = db.Books.Find(bookId);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [Route("DeleteBook/{bookId}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBookConfirmed(int bookId)
        {
            // Kiểm tra xem có bất kỳ bản ghi nào trong Borrow_Return liên quan đến BookID và chưa trả sách không
            var borrows = db.BorrowReturns.Any(x => x.BookId == bookId && x.ReturnDate == null);

            if (borrows)
            {
                TempData["Message"] = "Không thể xóa sách này vì có độc giả đang mượn";
                return RedirectToAction("ListBook", "BookAdmin");
            }

            var book = db.Books.Find(bookId);
            if (book != null)
            {
                db.Books.Remove(book);
                try
                {
                    db.SaveChanges();
                    TempData["Message"] = "Xóa thành công";
                }
                catch (DbUpdateException ex)
                {// Hiển thị chi tiết lỗi nội bộ
                    var innerException = ex.InnerException?.Message ?? ex.Message;
                    TempData["Message"] = "Đã xảy ra lỗi khi xóa sách: " + innerException;
                    return RedirectToAction("ListBook", "BookAdmin");
                }
            }

            return RedirectToAction("ListBook", "BookAdmin");
        }

        [Route("BookDetail")]
        [HttpGet]
        public IActionResult BookDetail(int bookId)
        {
            var book = db.Books.FirstOrDefault(m => m.BookId == bookId);
            if (book == null)
            {
                return NotFound();
            }
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == book.CategoryId);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.CategoryName = category.CategoryName;

            return View(book);

        }

        [HttpGet]
        [Route("EditBook")]
        public IActionResult EditBook(int bookId)
        {
            var book = db.Books.Include(x => x.Category).FirstOrDefault(x => x.BookId == bookId);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            return View(book);
        }


        [HttpPost]
        [Route("EditBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(Book book)
        {
            if (book == null)
            {
                return BadRequest("Book object is null.");
            }

            var existingBook = await db.Books.FindAsync(book.BookId);
            if (existingBook == null)
            {
                return NotFound("Book not found.");
            }

            // Update properties
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Publisher = book.Publisher;
            existingBook.YearPublished = book.YearPublished;
            existingBook.CategoryId = book.CategoryId;
            existingBook.Quantity = book.Quantity;
            existingBook.Available = book.Available;
            // Update other properties as needed

            // Handle file upload if a new file is provided
            if (book.file != null && book.file.Length > 0)
            {
                // Delete the old file if exists
                if (!string.IsNullOrEmpty(existingBook.BookImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/books", existingBook.BookImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save the new file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/books", book.file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await book.file.CopyToAsync(stream);
                }
                existingBook.BookImage = book.file.FileName;
            }

            // Save changes to the database
            db.Books.Update(existingBook);
            await db.SaveChangesAsync();
            TempData["Message"] = "Sửa thành công";

            return RedirectToAction("ListBook");
        }

    }   
}