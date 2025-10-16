using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class ChiTietDonHang
{
    public int IddonHang { get; set; }

    public int IdbienThe { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public virtual HangHoaBienThe IdbienTheNavigation { get; set; } = null!;

    public virtual DonHang IddonHangNavigation { get; set; } = null!;
}
