using HTQLTV.Models;
using HTQLTV.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace HTQLTV.Controllers
{
    public class HomeController : Controller
    {

        HtqltvContext db = new HtqltvContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        
        public IActionResult Index(int? page, string searchQuery, string searchType)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            var lstBook = db.Books.AsNoTracking().OrderBy(x => x.Title);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (searchType == "author")
                {
                    lstBook = lstBook.Where(s => s.Author.Contains(searchQuery)).OrderBy(x => x.Title);
                }
                else // searchType == "title"
                {
                    lstBook = lstBook.Where(s => s.Title.Contains(searchQuery)).OrderBy(x => x.Title);
                }
                ViewBag.CurrentFilter = searchQuery; //L?u giá tr? tìm ki?m trong ViewBag
            }

            PagedList<Book> lst = new PagedList<Book>(lstBook, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult BookDetail(int bookId)
        {
            var book = db.Books.SingleOrDefault(x => x.BookId == bookId);
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

            var bookImg = db.Books.Where(x => x.BookId == bookId).ToList();
            ViewBag.bookImg = bookImg;
            return View(book);
        }

        public IActionResult ReaderDetail(int readerId)
        {
            var reader = db.Readers.SingleOrDefault(x => x.ReaderId == readerId);
            if (reader == null)
            {
                return NotFound();
            }

            var readerImg = db.Readers.Where(x => x.ReaderId == readerId).ToList();
            ViewBag.readerImg = readerImg;
            return View(reader);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}