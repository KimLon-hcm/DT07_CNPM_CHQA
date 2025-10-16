using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class HinhAnhBienThe
{
    public int IdhinhAnh { get; set; }

    public int? IdbienThe { get; set; }

    public string DuongDan { get; set; } = null!;

    public bool? LaAnhChinh { get; set; }

    public virtual HangHoaBienThe? IdbienTheNavigation { get; set; }
}
