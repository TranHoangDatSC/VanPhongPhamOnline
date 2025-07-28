using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20,ErrorMessage = "Tối đa 20 ký tự!")]
        public string MaKh { get; set; } = null;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string? MatKhauKh { get; set; }

        [Display(Name = "Họ Tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự!")]
        public string HoTenKh { get; set; }
        public bool GioiTinhKh { get; set; } = true;

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng chọn ngày sinh!")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinhKh { get; set; }

        [MaxLength(60, ErrorMessage = "Tối đa 60 ký tự!")]

        [Display(Name = "Địa Chỉ")]
        public string DiaChiKh { get; set; }
        [MaxLength(24, ErrorMessage = "Tối đa 24 ký tự!")]
        [RegularExpression(@"0[9875]\d{8}",ErrorMessage = "Chưa đúng định dạng của di động Việt Nam!")]

        [Display(Name = "Điện thoại")]
        public string? DienThoaiKh { get; set; }
        [EmailAddress(ErrorMessage ="Chưa đúng định dạng email!" )]
        public string EmailKh { get; set; } = null;
        public string? HinhKh { get; set; }
    }
}
