using System;
using System.Collections.Generic;

namespace VanPhongPhamOnline.Data;

public partial class TrangThai
{
    public int MaTrangThai { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public bool? DaThanhToan { get; set; }

    public string? MoTa { get; set; }

    public string? MaNv { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual NhanVien? MaNvNavigation { get; set; }
}
