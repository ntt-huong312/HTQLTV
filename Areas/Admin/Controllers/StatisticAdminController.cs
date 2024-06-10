using HTQLTV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/StatisticAdmin")]
    [Route("StatisticAdmin")]
    public class StatisticAdminController : Controller
    {
        private readonly HtqltvContext _db;
        private HtqltvContext db = new HtqltvContext();

        public StatisticAdminController(HtqltvContext db)
        {
            _db = db;
        }

        [Route("")]
        [Route("Index")]

        public IActionResult Index()
        {
            var tongsosach = _db.BorrowReturns
                .Where(br => br.ReaderId != null && br.BookId != null)
                .Select(g => g.BookNumber == null ? 0 : g.BookNumber).Sum();
            ViewBag.tongsosach = tongsosach;

            var tongdocgia = _db.Readers
                .Where(br => br.Email != null && br.FullName != null)
                .Select(g => new
                {
                    ReaderId = g
                }).Count();
            ViewBag.tongdocgia = tongdocgia;


            var tongmuonsach = _db.BorrowReturns
            .Where(br => br.ReaderId != null && br.ReturnDate == null)
            .Select(g => g.BookNumber).Sum()/*Sum(br => EF.Functions.DateDiffDay(DateOnly.MinValue, br.ReturnDate.Value))*/;

            ViewBag.tongmuonsach = tongmuonsach;

            var tongtrasach = _db.BorrowReturns
            .Where(br => br.ReaderId != null)
            .Select(g => g.TotalReturned == null ? 0 : g.TotalReturned) // Chuyển đổi giá trị null sang 0
            .Sum();
            ViewBag.tongtrasach = tongtrasach;

            return View();
        }


    }
}