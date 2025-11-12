using DT07_CuaHangQuanAo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
// Thêm các using cần thiết
using Microsoft.AspNetCore.Hosting; // <-- THÊM USING NÀY
using System.IO; // <-- THÊM USING NÀY
using System; // <-- THÊM USING NÀY

namespace DT07_CuaHangQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly QlShopquanaoContext _context;
        // *** THAY ĐỔI 1: Thêm IWebHostEnvironment ***
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Cập nhật constructor để inject IWebHostEnvironment
        public SanPhamController(QlShopquanaoContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; // Gán giá trị
        }

        // Action Index (Giữ nguyên)
        public async Task<IActionResult> Index()
        {
            var danhSachSanPham = await _context.HangHoas
                                                 .Include(p => p.IdloaiHangNavigation)
                                                 .Include(p => p.IdthuongHieuNavigation)
                                                 .ToListAsync();
            return View(danhSachSanPham);
        }

        // Action Them [GET] (Giữ nguyên)
        public IActionResult Them()
        {
            ViewBag.IdloaiHang = new SelectList(_context.LoaiHangs.ToList(), "IdloaiHang", "TenLoaiHang");
            ViewBag.IdmauSac = new SelectList(_context.MauSacs.ToList(), "IdmauSac", "TenMau");
            ViewBag.IdthuongHieu = new SelectList(_context.ThuongHieus.ToList(), "IdthuongHieu", "TenThuongHieu");
            ViewBag.IdkichThuoc = new SelectList(_context.KichThuocs.ToList(), "IdkichThuoc", "TenKichThuoc");
            return View(new SanPhamCreateViewModel());
        }

        // *** THAY ĐỔI 2: Cập nhật toàn bộ Action Them [POST] ***
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(SanPhamCreateViewModel model)
        {
            // Load lại ViewBag phòng trường hợp model không hợp lệ và phải return View
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
                        // Bước 1: Tạo và lưu đối tượng HangHoa (Giữ nguyên)
                        var newHangHoa = new HangHoa
                        {
                            TenHang = model.TenHang,
                            MoTa = model.MoTa,
                            IdloaiHang = model.IdloaiHang,
                            IdthuongHieu = model.IdthuongHieu,
                            TrangThai = model.TrangThai
                        };
                        _context.HangHoas.Add(newHangHoa);
                        await _context.SaveChangesAsync(); // Lưu để có Idhang

                        // Bước 2: Lặp qua danh sách biến thể, lưu biến thể VÀ hình ảnh
                        if (model.BienThes != null && model.BienThes.Any())
                        {
                            foreach (var bienTheVM in model.BienThes)
                            {
                                // 2.1. Tạo và lưu biến thể để lấy ID
                                var newBienThe = new HangHoaBienThe
                                {
                                    Idhang = newHangHoa.Idhang,
                                    IdmauSac = bienTheVM.IdmauSac,
                                    IdkichThuoc = bienTheVM.IdkichThuoc,
                                    Sku = bienTheVM.Sku,
                                    Gia = bienTheVM.Gia,
                                    GiaKhuyenMai = bienTheVM.GiaKhuyenMai,
                                    SoLuongTon = bienTheVM.SoLuongTon
                                };
                                _context.HangHoaBienThes.Add(newBienThe);
                                await _context.SaveChangesAsync(); // Lưu ngay để có IdbienThe

                                // 2.2. Xử lý upload và lưu hình ảnh cho biến thể vừa tạo
                                if (bienTheVM.HinhAnhs != null && bienTheVM.HinhAnhs.Any())
                                {
                                    bool isFirstImage = true; // Dùng để set ảnh đầu tiên làm ảnh chính
                                    foreach (var fileAnh in bienTheVM.HinhAnhs)
                                    {
                                        // Tạo tên file độc nhất
                                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fileAnh.FileName);

                                        // Tạo đường dẫn tuyệt đối để lưu file
                                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/product");
                                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                        // Lưu file vào thư mục
                                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                                        {
                                            await fileAnh.CopyToAsync(fileStream);
                                        }

                                        // Tạo đối tượng HinhAnhBienThe để lưu vào DB
                                        var hinhAnh = new HinhAnhBienThe
                                        {
                                            IdbienThe = newBienThe.IdbienThe, // ID của biến thể vừa tạo
                                            DuongDan = "images/product" + uniqueFileName, // Đường dẫn tương đối
                                            LaAnhChinh = isFirstImage
                                        };
                                        _context.HinhAnhBienThes.Add(hinhAnh);

                                        isFirstImage = false; // Các ảnh sau không phải là ảnh chính
                                    }
                                    await _context.SaveChangesAsync(); // Lưu tất cả hình ảnh của biến thể này
                                }
                            }
                        }

                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                    }
                }
            }

            return View(model);
        }
    }
}