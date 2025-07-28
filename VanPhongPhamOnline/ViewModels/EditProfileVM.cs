using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.ViewModels
{
    public class EditProfileVM
    {
        [Display(Name = "Họ Tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string HoTen { get; set; }

        [Display(Name = "Địa chỉ")]
        [MaxLength(60)]
        public string? DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        [MaxLength(24)]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string? DienThoai { get; set; }

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; } = "";
        public IFormFile? Hinh { get; set; }
    }
}
