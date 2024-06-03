using Azure;
using HTQLTV.Models;
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
        public IActionResult CreateReader()
        {
            return View();
        }
        [Route("CreateReader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public IActionResult EditReader(int maDocGia)
        {
            var docgia = db.Readers.Find(maDocGia);
            return View(docgia);
        }
        [Route("EditReader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            TempData["Message"] = "";
            var docgia = db.Readers.Find(maDocGia); // Tìm độc giả dựa trên ReaderID
            if (docgia != null)
            {
                db.Readers.Remove(docgia); // Xóa độc giả khỏi DbSet
                db.SaveChanges(); // Lưu các thay đổi vào cơ sở dữ liệu
                TempData["Message"] = "Độc giả đã được xóa";
            }
            return RedirectToAction("ReaderAdmin", "admin");
        }
    }
}
