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

namespace HTQLTV.Areas.Admin.Controllers
{


    [Area("admin")]
    [Route("admin/BookAdmin")]
    public class BookAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [Route("admin/BookAdmin")]
        [Route("")]
        [Route("index")]
        //public IActionResult Index()
        //{
        //    return View(db.Books.ToList());
        //}

        [Route("ListBook")]
        [Authentication]
        public IActionResult ListBook(int? page, string searchString)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listbook = db.Books.AsNoTracking().OrderBy(x => x.Title);
            if (!string.IsNullOrEmpty(searchString))
            {
                listbook = listbook.Where(s => s.Title.Contains(searchString)).OrderBy(x => x.Title);
            }
            PagedList<Book> lst = new PagedList<Book>(listbook, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult ImageUpload(string? path)
        {
            Book model = new Book { BookImage = path };
            return View(model);
        }
        [HttpGet]
        [Route("CreateBook")]
        public IActionResult CreateBook()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }


        [Route("CreateBook")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("ListBook");
            } 
            return View(book);
        }

        //[Route("EditBook")]
        //[HttpGet]
        //public IActionResult EditBook(int bookId)
        //{
        //    var book = db.Books.Find(bookId);
        //    return View(book);
        //}
        //[Route("EditBook")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult EditBook(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Books.Update(book);
        //        db.SaveChanges();
        //        return RedirectToAction("ListBook");
        //    }
        //    return View(book);
        //}

        [Route("DeleteBook")]
        [HttpGet]
        public IActionResult DeleteBook(int bookId)
        {
            TempData["Message"] = "";

            // Lấy các bản ghi Borrow liên quan đến BookID
            var borrows = db.BorrowReturns.Any(x => x.BookId == bookId);

            if (borrows)
            {

                TempData["Message"] = "Không thể xóa sách này vì có độc giả đang mượn";
                return RedirectToAction("ListBook", "BookAdmin");
            }

            var book = db.Books.Find(bookId);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
                TempData["Message"] = "Xóa thành công";
            }

            return RedirectToAction("ListCategory", "admin");
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



            //[HttpPost]
            //[Route("CreateBook")]
            //public async Task<IActionResult> CreateBook(Book book, IFormFile imageFile)
            //{
            //    //    if (book.file.Length > 0)
            //    //{
            //    //    var BookImage = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\books", book.file.FileName);
            //    //    using (var stream = new FileStream(BookImage, FileMode.Create))
            //    //    {
            //    //        await book.file.CopyToAsync(stream);
            //    //    }
            //    //}
            //    //if (ModelState.IsValid)
            //    //{
            //    //    db.Books.Add(book);
            //    //    db.SaveChanges();
            //    //    return RedirectToAction("ListBook");
            //    //}
            //    //return View(book);
            //    //return RedirectToAction("ImageUpload", new { path = "/img/books/" + book.file.FileName });
            //    if (ModelState.IsValid)
            //    {
            //        if (imageFile != null && imageFile.Length > 0)
            //        {
            //            var fileName = Path.GetFileName(imageFile.FileName);
            //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                imageFile.CopyTo(stream);
            //            }

            //            book.BookImage = "/images/" + fileName; // Lưu đường dẫn hình ảnh vào đối tượng Book
            //        }

            //        db.Books.Add(book);
            //        db.SaveChanges();
            //        return RedirectToAction("ListBook");
            //    }

            //    return View(book);
            //}


        

        


        }
}