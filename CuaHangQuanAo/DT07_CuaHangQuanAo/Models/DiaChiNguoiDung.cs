using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class DiaChiNguoiDung
{
    public int IdnguoiDung { get; set; }

    public int IddiaChi { get; set; }

    public bool? LaMacDinh { get; set; }

    public virtual DiaChi IddiaChiNavigation { get; set; } = null!;

    public virtual NguoiDung IdnguoiDungNavigation { get; set; } = null!;
}
