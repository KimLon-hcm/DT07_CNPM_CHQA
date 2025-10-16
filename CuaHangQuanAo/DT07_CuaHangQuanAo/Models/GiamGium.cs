using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class GiamGium
{
    public int IdgiamGia { get; set; }

    public string MaGiamGia { get; set; } = null!;

    public string? TenChuongTrinh { get; set; }

    public int LoaiGiamGia { get; set; }

    public decimal GiaTri { get; set; }

    public decimal? DonHangToiThieu { get; set; }

    public decimal? GiamToiDa { get; set; }

    public int SoLuong { get; set; }

    public int DaSuDung { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
