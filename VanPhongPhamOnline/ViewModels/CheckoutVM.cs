using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.ViewModels
{
    public class CheckoutVM
    {
        public bool GiongKhachHang { get; set; }

        [MaxLength(50, ErrorMessage = "Họ tên tối đa 50 ký tự")]
        public string? HoTen { get; set; }

        [MaxLength(60, ErrorMessage = "Địa chỉ tối đa 60 ký tự")]
        public string? DiaChi { get; set; }

        [MaxLength(24, ErrorMessage = "Số điện thoại tối đa 24 ký tự")]
        public string? DienThoai { get; set; }

        [MaxLength(50, ErrorMessage = "Ghi chú tối đa 50 ký tự")]
        public string? GhiChu { get; set; }
    }
}
