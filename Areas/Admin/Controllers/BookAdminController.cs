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
        [Route("CreateNewBook")]
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
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
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

        //[Route("DeleteBook")]
        //[HttpGet]
        //public IActionResult DeleteBook(int bookId)
        //{
        //    TempData["Message"] = "";

        //    // Lấy các bản ghi Borrow liên quan đến BookID
        //    var borrows = db.Borrows.Where(x => x.BookId == bookId).ToList();

        //    if (borrows.Any())
        //    {
        //        // Lấy các bản ghi Returns liên quan đến BorrowId
        //        var borrowIds = borrows.Select(b => b.BorrowId).ToList();
        //        var returns = db.Returns.Where(r => borrowIds.Contains(r.BorrowId)).ToList();

        //        // Xóa các bản ghi Returns liên quan
        //        if (returns.Any())
        //        {
        //            db.Returns.RemoveRange(returns);
        //        }

        //        // Xóa các bản ghi Borrow
        //        db.Borrows.RemoveRange(borrows);
        //    }

        //    // Xóa các bản ghi Statistic liên quan đến BookID
        //    var statistics = db.Statistics.Where(s => s.BookId == bookId).ToList();
        //    if (statistics.Any())
        //    {
        //        db.Statistics.RemoveRange(statistics);
        //    }

        //    // Xóa bản ghi Book
        //    var book = db.Books.Find(bookId);
        //    if (book != null)
        //    {
        //        db.Books.Remove(book);
        //    }

        //    db.SaveChanges();
        //    TempData["Message"] = "Sách đã được xóa";
        //    return RedirectToAction("BookAdmin", "admin");
        //}

        //[Route("BookDetail")]
        //[HttpGet]
        //public IActionResult BookDetail(int bookId)
        //{
        //    var book = db.Books.FirstOrDefault(m => m.BookId == bookId);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(book);

        //}
       
       

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