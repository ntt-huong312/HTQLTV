using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HTQLTV.Areas.Admin.Controllers
{

    [Area("admin")]
    [Route("admin/CategoryAdmin")]
    public class CategoryAdminController : Controller
    {
        HtqltvContext db = new HtqltvContext();

        [Route("ListCategory")]
        public IActionResult ListCategory(int? page, string searchString)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listcate = db.Categories.AsNoTracking().OrderBy(x => x.CategoryName);
            if (!string.IsNullOrEmpty(searchString))
            {
                listcate = listcate.Where(s => s.CategoryName.Contains(searchString)).OrderBy(x => x.CategoryName);
            }
            PagedList<Category> lst = new PagedList<Category>(listcate, pageNumber, pageSize);
            return View(lst);
        }
        [Route("CreateCategory")]
        [HttpGet]
        public IActionResult CreateCategory()
        {
            
            return View();
        }


        [Route("CreateCategory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ListCategory");
            }
            return View(category);
        }


        [Route("EditCategory")]
        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            var category = db.Categories.Find(categoryId);
            return View(category);
        }

        [Route("EditCategory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("ListCategory");
            }
            return View(category);
        }
        [Route("DeleteCategory")]
        [HttpGet]
        public IActionResult DeleteCategory(int categoryId)
        {
            TempData["Message"] = "";
            // Kiểm tra xem có sách nào đang sử dụng thể loại này không
            var book = db.Books.Any(x => x.CategoryId == categoryId);
            if (book)
            {
                TempData["Message"] = "Không thể xóa thể loại này vì có sách đang sử dụng nó";
                return RedirectToAction("ListCategory", "CategoryAdmin");
            }

            var category = db.Categories.Find(categoryId);
            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["Message"] = "Xóa thành công";
            }

            return RedirectToAction("ListCategory", "admin");
        }

    }
}
