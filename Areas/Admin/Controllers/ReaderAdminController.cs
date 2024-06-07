using Azure;
using HTQLTV.Models;
using HTQLTV.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/ReaderAdmin")]
    public class ReaderAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();
        [Route("")]
        [Route("index")]
        [Route("ListReader")]
        [Authentication]
        public IActionResult ListReader(int? page, string searchString)
        {
            int pageSize = 4;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listreader = db.Readers.AsNoTracking().OrderBy(x => x.FullName);
            if (!string.IsNullOrEmpty(searchString))
            {
                listreader = listreader.Where(s => s.FullName.Contains(searchString)).OrderBy(x => x.FullName);
            }
            PagedList<Reader> lst = new PagedList<Reader>(listreader, pageNumber, pageSize);
            return View(lst);
        }



        [Route("CreateReader")]
        [HttpGet]
        [Authentication]
        public IActionResult CreateReader()
        {
            return View();
        }
        [Route("CreateReader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]
        public IActionResult CreateReader(Reader docgia)
        {
            if (ModelState.IsValid)
            {
                db.Readers.Add(docgia);
                db.SaveChanges();
                return RedirectToAction("ListReader");
            }
            return View(docgia);
        }

        [Route("EditReader")]
        [HttpGet]
        [Authentication]
        public IActionResult EditReader(int maDocGia)
        {
            var docgia = db.Readers.Find(maDocGia);
            return View(docgia);
        }
        [Route("EditReader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authentication]
        public IActionResult EditReader(Reader docgia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docgia).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("ListReader");
            }
            return View(docgia);
        }

        [Route("DetailsReader")]
        [HttpGet]
        [Authentication]
        public IActionResult DetailsReader(int maDocGia)
        {
            var docgia = db.Readers.FirstOrDefault(m => m.ReaderId == maDocGia);
            if (docgia == null)
            {
                return NotFound();
            }
            return View(docgia);

        }

        
        [Route("DeleteReader")]
        [HttpGet]
        public IActionResult DeleteReader(int maDocGia)
        {
            var docGia = db.Readers.Find(maDocGia);
            if (docGia == null)
            {
                return NotFound();
            }

            return View(docGia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteReader")]
        public IActionResult DeleteReaderConfirmed(int maDocGia)
        {
            TempData["Message"] = "";

            // Lấy các bản ghi Borrow liên quan đến ReaderId
            var borrowRecords = db.BorrowReturns.Where(x => x.ReaderId == maDocGia).ToList();

            // Kiểm tra xem có bản ghi nào có ReturnDate là null không
            if (borrowRecords.Any(br => br.ReturnDate == null))
            {
                TempData["ErrorMessage"] = "Không thể xóa độc giả này vì có sách chưa được trả.";
                return RedirectToAction("DeleteReader", new { maDocGia = maDocGia });
            }

            // Xóa các bản ghi Borrow
            db.BorrowReturns.RemoveRange(borrowRecords);

            // Xóa bản ghi Reader
            var docGia = db.Readers.Find(maDocGia);
            if (docGia != null)
            {
                db.Readers.Remove(docGia);
                db.SaveChanges();
                TempData["Message"] = "Độc giả đã được xóa";
            }

            return RedirectToAction("ReaderAdmin", "admin");

        }

    }
}
