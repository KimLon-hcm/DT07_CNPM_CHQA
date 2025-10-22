using Microsoft.AspNetCore.Mvc;

namespace DT07_CuaHangQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
