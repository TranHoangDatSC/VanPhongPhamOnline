using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.Data;

public partial class KhachHang
{
    [Display(Name = "Mã khách hàng")]
    [Required(ErrorMessage = "Mã khách hàng là bắt buộc")]
    public string MaKh { get; set; } = null!;

    [Display(Name = "Mật khẩu")]
    public string? MatKhauKh { get; set; }

    [Display(Name = "Họ tên")]
    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    public string HoTenKh { get; set; } = null!;

    [Display(Name = "Giới tính")]
    public bool GioiTinhKh { get; set; }

    [Display(Name = "Ngày sinh")]
    [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
    public DateTime NgaySinhKh { get; set; }

    [Display(Name = "Địa chỉ")]
    public string? DiaChiKh { get; set; }

    [Display(Name = "Điện thoại")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số")]
    public string? DienThoaiKh { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string EmailKh { get; set; } = null!;

    [Display(Name = "Hình ảnh")]
    public string? HinhKh { get; set; }

    public string? RandomKeyKh { get; set; }

    public virtual ICollection<GopY> Gopies { get; set; } = new List<GopY>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}
