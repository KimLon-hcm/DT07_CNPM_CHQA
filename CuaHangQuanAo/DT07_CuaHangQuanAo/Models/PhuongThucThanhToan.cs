using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class PhuongThucThanhToan
{
    public int Id { get; set; }

    public string TenPhuongThuc { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
