using System;
using System.Collections.Generic;

namespace VanPhongPhamOnline.Data;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string HoTenNv { get; set; } = null!;

    public string EmailNv { get; set; } = null!;

    public string? MatKhauNv { get; set; }

    public DateOnly? NgaySinhNv { get; set; }

    public bool? GioiTinhNv { get; set; }

    public string? DiaChiNv { get; set; }

    public bool? HieuLuc { get; set; }

    public string? RandomKeyNv { get; set; }

    public virtual ICollection<GopY> Gopies { get; set; } = new List<GopY>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();

    public virtual ICollection<TrangThai> TrangThais { get; set; } = new List<TrangThai>();
}
