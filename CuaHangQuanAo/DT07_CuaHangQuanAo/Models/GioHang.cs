using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class GioHang
{
    public int IdnguoiDung { get; set; }

    public int IdbienThe { get; set; }

    public int SoLuong { get; set; }

    public virtual HangHoaBienThe IdbienTheNavigation { get; set; } = null!;

    public virtual NguoiDung IdnguoiDungNavigation { get; set; } = null!;
}
