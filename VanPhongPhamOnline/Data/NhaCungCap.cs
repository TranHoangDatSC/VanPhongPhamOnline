using System;
using System.Collections.Generic;

namespace VanPhongPhamOnline.Data;

public partial class NhaCungCap
{
    public string MaNcc { get; set; } = null!;

    public string TenCongTy { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string? NguoiLienLac { get; set; }

    public string EmailNcc { get; set; } = null!;

    public string? DienThoaiNcc { get; set; }

    public string? DiaChiNcc { get; set; }

    public string? MoTaNcc { get; set; }

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
