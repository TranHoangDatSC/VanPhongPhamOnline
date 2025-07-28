using System;
using System.Collections.Generic;

namespace VanPhongPhamOnline.Data;

public partial class GopY
{
    public string MaGy { get; set; } = null!;

    public string? MaNv { get; set; }

    public string? MaKh { get; set; }

    public string NoiDung { get; set; } = null!;

    public DateOnly NgayGy { get; set; }

    public string? HoTenKh { get; set; }

    public string? EmailKh { get; set; }

    public string? DienThoai { get; set; }

    public bool CanTraLoi { get; set; }

    public string? TraLoi { get; set; }

    public DateOnly? NgayTl { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
