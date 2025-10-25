using Microsoft.AspNetCore.Mvc;
using DT07_CuaHangQuanAo.Models;

namespace DT07_CuaHangQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
