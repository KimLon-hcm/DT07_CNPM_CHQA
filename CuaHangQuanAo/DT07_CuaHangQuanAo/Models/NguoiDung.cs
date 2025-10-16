using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class NguoiDung
{
    public int IdnguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string MatKhauHash { get; set; } = null!;

    public string LoaiTk { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DiaChiNguoiDung> DiaChiNguoiDungs { get; set; } = new List<DiaChiNguoiDung>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
}
