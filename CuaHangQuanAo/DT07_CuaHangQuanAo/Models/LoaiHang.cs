using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class LoaiHang
{
    public int IdloaiHang { get; set; }

    public string TenLoaiHang { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
