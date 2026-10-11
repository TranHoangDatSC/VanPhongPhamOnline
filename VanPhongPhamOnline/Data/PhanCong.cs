using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VanPhongPhamOnline.Data;

public partial class PhanCong
{
    public int MaPc { get; set; }

    public string MaNv { get; set; } = null!;

    public string MaPb { get; set; } = null!;

    public DateTime? NgayPc { get; set; }

    public bool? HieuLuc { get; set; }

    [ValidateNever]
    public virtual NhanVien MaNvNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual PhongBan MaPbNavigation { get; set; } = null!;
}
