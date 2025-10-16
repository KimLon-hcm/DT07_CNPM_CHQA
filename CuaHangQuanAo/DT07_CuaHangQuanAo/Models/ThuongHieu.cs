using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class ThuongHieu
{
    public int IdthuongHieu { get; set; }

    public string TenThuongHieu { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
