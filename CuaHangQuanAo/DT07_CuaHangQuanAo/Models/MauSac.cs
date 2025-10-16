using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class MauSac
{
    public int IdmauSac { get; set; }

    public string TenMau { get; set; } = null!;

    public string? MaMauHex { get; set; }

    public virtual ICollection<HangHoaBienThe> HangHoaBienThes { get; set; } = new List<HangHoaBienThe>();
}
