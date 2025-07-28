using System;
using System.Collections.Generic;

namespace VanPhongPhamOnline.Data;

public partial class KhachHang
{
    public string MaKh { get; set; } = null!;

    public string? MatKhauKh { get; set; }

    public string HoTenKh { get; set; } = null!;

    public bool GioiTinhKh { get; set; }

    public DateTime NgaySinhKh { get; set; }

    public string? DiaChiKh { get; set; }

    public string? DienThoaiKh { get; set; }

    public string EmailKh { get; set; } = null!;

    public string? HinhKh { get; set; }

    public string? RandomKeyKh { get; set; }

    public virtual ICollection<GopY> Gopies { get; set; } = new List<GopY>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}
