using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class HangHoa
{
    public int Idhang { get; set; }

    public string TenHang { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? IdloaiHang { get; set; }

    public int? IdthuongHieu { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<HangHoaBienThe> HangHoaBienThes { get; set; } = new List<HangHoaBienThe>();

    public virtual LoaiHang? IdloaiHangNavigation { get; set; }

    public virtual ThuongHieu? IdthuongHieuNavigation { get; set; }
}
