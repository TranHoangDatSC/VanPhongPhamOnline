using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace VanPhongPhamOnline.Data;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public string MaKh { get; set; } = null!;

    public DateTime NgayDat { get; set; }

    public DateTime? NgayCan { get; set; }

    public DateTime? NgayGiao { get; set; }

    public string? HoTenNguoiNhan { get; set; }

    public string DiaChi { get; set; } = null!;

    public string CachThanhToan { get; set; } = null!;

    public string CachVanChuyen { get; set; } = null!;

    public double PhiVanChuyen { get; set; }

    public int MaTrangThai { get; set; }

    public string? MaNv { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    [BindNever]
    public virtual KhachHang? MaKhNavigation { get; set; } = null!;
    [BindNever]
    public virtual NhanVien? MaNvNavigation { get; set; }
    [BindNever]
    public virtual TrangThai? MaTrangThaiNavigation { get; set; } = null!;

    public decimal TongTien
    {
        get
        {
            return ChiTietHds?.Sum(ct => (decimal)ct.SoLuong * (decimal)ct.DonGia) ?? 0;
        }
    }
}
