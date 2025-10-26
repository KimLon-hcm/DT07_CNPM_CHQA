
namespace DT07_CuaHangQuanAo.Models
{
    public class SanPhamCreateViewModel
    {
        // Các thuộc tính cho bảng HangHoa
        public string TenHang { get; set; }
        public string? MoTa { get; set; }
        public int IdloaiHang { get; set; }
        public int IdthuongHieu { get; set; }
        public bool TrangThai { get; set; } 

        // Danh sách các biến thể
        public List<HangHoaBienTheViewModel> BienThes { get; set; }

        // Constructor để khởi tạo list, tránh lỗi null
        public SanPhamCreateViewModel()
        {
            BienThes = new List<HangHoaBienTheViewModel>();
        }
    }
}