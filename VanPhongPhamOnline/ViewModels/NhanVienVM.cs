using Microsoft.AspNetCore.Mvc.Rendering;
using VanPhongPhamOnline.Data;

namespace VanPhongPhamOnline.ViewModels
{
    public class NhanVienVM
    {
        public string MaNv { get; set; }
        public string HoTenNv { get; set; }
        public string EmailNv { get; set; }
        public string MatKhauNv { get; set; }
        public DateTime NgaySinhNv { get; set; }
        public bool GioiTinhNv { get; set; } // true = Nam, false = Nữ
        public string? DiaChiNv { get; set; }

        // Từ bảng phân công
        public string MaPb { get; set; }
        public string TenPb { get; set; }
        public DateTime? NgayPc { get; set; }
        public bool HieuLuc { get; set; }
    }
}
