using System;
using System.Collections.Generic;

namespace DT07_CuaHangQuanAo.Models;

public partial class DiaChi
{
    public int IddiaChi { get; set; }

    public string? TinhThanh { get; set; }

    public string? QuanHuyen { get; set; }

    public string? PhuongXa { get; set; }

    public string ChiTiet { get; set; } = null!;

    public virtual ICollection<DiaChiNguoiDung> DiaChiNguoiDungs { get; set; } = new List<DiaChiNguoiDung>();
}
