using DT07_CuaHangQuanAo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
            ViewBag.IdmauSac = new SelectList(_context.MauSacs.ToList(), "IdmauSac", "TenMau");
            ViewBag.IdthuongHieu = new SelectList(_context.ThuongHieus.ToList(), "IdthuongHieu", "TenThuongHieu");
            ViewBag.IdkichThuoc = new SelectList(_context.KichThuocs.ToList(), "IdkichThuoc", "TenKichThuoc");

            return View(new SanPhamCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(SanPhamCreateViewModel model)
        {
            ViewBag.IdloaiHang = new SelectList(_context.LoaiHangs.ToList(), "IdloaiHang", "TenLoaiHang", model.IdloaiHang);
            ViewBag.IdmauSac = new SelectList(_context.MauSacs.ToList(), "IdmauSac", "TenMau");
            ViewBag.IdthuongHieu = new SelectList(_context.ThuongHieus.ToList(), "IdthuongHieu", "TenThuongHieu", model.IdthuongHieu);
            ViewBag.IdkichThuoc = new SelectList(_context.KichThuocs.ToList(), "IdkichThuoc", "TenKichThuoc");

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Bước 1: Tạo và lưu đối tượng HangHoa
                        var newHangHoa = new HangHoa
                        {
                            TenHang = model.TenHang,
                            MoTa = model.MoTa,
                            IdloaiHang = model.IdloaiHang,
                            IdthuongHieu = model.IdthuongHieu,
                            TrangThai = model.TrangThai
                        };
                        _context.HangHoas.Add(newHangHoa);
                        await _context.SaveChangesAsync(); // Lưu để EF gán ID cho newHangHoa

                        // Bước 2: Lặp qua danh sách biến thể và lưu chúng
                        if (model.BienThes != null && model.BienThes.Any())
                        {
                            foreach (var bienTheVM in model.BienThes)
                            {
                                var newBienThe = new HangHoaBienThe
                                {
                                    Idhang = newHangHoa.Idhang, // Gán ID của hàng hóa vừa tạo
                                    IdmauSac = bienTheVM.IdmauSac,
                                    IdkichThuoc = bienTheVM.IdkichThuoc,
                                    Sku = bienTheVM.Sku,
                                    Gia = bienTheVM.Gia,
                                    GiaKhuyenMai = bienTheVM.GiaKhuyenMai,
                                    SoLuongTon = bienTheVM.SoLuongTon
                                };
                                _context.HangHoaBienThes.Add(newBienThe);
                            }
                            await _context.SaveChangesAsync(); // Lưu tất cả biến thể vào DB
                        }

                        // Nếu tất cả thành công, commit transaction
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex) // Bắt ngoại lệ và gán nó vào biến 'ex'
                    {
                        Console.WriteLine("!!!!!!!!!!!!!! DATABASE UPDATE EXCEPTION !!!!!!!!!!!!!!");
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình tạo sản phẩm. Vui lòng thử lại.");
                    }
                }
            }

            return View(model);
        }
    }
}
