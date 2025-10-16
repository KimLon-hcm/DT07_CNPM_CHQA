using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class HangHoaBienThe
{
    public int IdbienThe { get; set; }

    public int? Idhang { get; set; }

    public int? IdmauSac { get; set; }

    public int? IdkichThuoc { get; set; }

    public string? Sku { get; set; }

    public decimal Gia { get; set; }

    public decimal? GiaKhuyenMai { get; set; }

    public int SoLuongTon { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<HinhAnhBienThe> HinhAnhBienThes { get; set; } = new List<HinhAnhBienThe>();

    public virtual HangHoa? IdhangNavigation { get; set; }

    public virtual KichThuoc? IdkichThuocNavigation { get; set; }

    public virtual MauSac? IdmauSacNavigation { get; set; }
}
