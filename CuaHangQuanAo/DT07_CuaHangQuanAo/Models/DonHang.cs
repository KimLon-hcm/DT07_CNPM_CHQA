using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class DonHang
{
    public int IddonHang { get; set; }

    public int? IdnguoiDung { get; set; }

    public int? IdgiamGia { get; set; }

    public string TenNguoiNhan { get; set; } = null!;

    public string DiaChiGiao { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public decimal TongTienHang { get; set; }

    public decimal? PhiVanChuyen { get; set; }

    public decimal? GiamGia { get; set; }

    public decimal TongThanhToan { get; set; }

    public int? IdphuongThucThanhToan { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual GiamGium? IdgiamGiaNavigation { get; set; }

    public virtual NguoiDung? IdnguoiDungNavigation { get; set; }

    public virtual PhuongThucThanhToan? IdphuongThucThanhToanNavigation { get; set; }
}
