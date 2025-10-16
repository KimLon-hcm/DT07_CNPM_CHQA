using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class KichThuoc
{
    public int IdkichThuoc { get; set; }

    public string TenKichThuoc { get; set; } = null!;

    public virtual ICollection<HangHoaBienThe> HangHoaBienThes { get; set; } = new List<HangHoaBienThe>();
}
