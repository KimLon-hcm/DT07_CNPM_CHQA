﻿
namespace DT07_CuaHangQuanAo.Models
{
    public class HangHoaBienTheViewModel
    {
        public int IdmauSac { get; set; }
        public int IdkichThuoc { get; set; }
        public string? Sku { get; set; }
        public decimal Gia { get; set; }
        public decimal? GiaKhuyenMai { get; set; }
        public int SoLuongTon { get; set; }
    }
}