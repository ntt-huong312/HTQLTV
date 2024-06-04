using HTQLTV.Models;
using HTQLTV.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTQLTV.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        [Route("")]
        [Route("index")]
        [Authentication]
        public IActionResult Index(string searchString)
        {
            return View();



        }

    }
}
