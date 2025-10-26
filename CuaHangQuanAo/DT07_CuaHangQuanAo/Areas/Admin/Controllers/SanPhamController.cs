using DT07_CuaHangQuanAo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DT07_CuaHangQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly QlShopquanaoContext _context;
        public SanPhamController(QlShopquanaoContext context)
        {
            _context = context;
        }

        // GET: Admin/SanPham
        // Action này sẽ chịu trách nhiệm lấy và hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var danhSachSanPham = await _context.HangHoas
                                                 .Include(p => p.IdloaiHangNavigation) // Lấy kèm LoaiHang
                                                 .Include(p => p.IdthuongHieuNavigation) // Lấy kèm ThuongHieu
                                                 .ToListAsync();


            return View(danhSachSanPham);
        }


        // GET: Admin/SanPham/Them
        public IActionResult Them()
        {
           
            ViewBag.IdloaiHang = new SelectList(_context.LoaiHangs.ToList(), "IdloaiHang", "TenLoaiHang");

            ViewBag.IdthuongHieu = new SelectList(_context.ThuongHieus.ToList(), "IdthuongHieu", "TenThuongHieu");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Them([Bind("TenHang,MoTa,IdloaiHang,IdthuongHieu,TrangThai")] HangHoa hangHoa)
        {
            ViewBag.IdloaiHang = new SelectList(_context.LoaiHangs.ToList(), "IdloaiHang", "TenLoaiHang", hangHoa.IdloaiHang);
            ViewBag.IdthuongHieu = new SelectList(_context.ThuongHieus.ToList(), "IdthuongHieu", "TenThuongHieu", hangHoa.IdthuongHieu);
            if (ModelState.IsValid)
            {
                _context.Add(hangHoa);
                _context.SaveChanges();
                return RedirectToAction("Index", "SanPham", new { area = "Admin" });
            }
            
            return View(hangHoa);
        }
    }
}
