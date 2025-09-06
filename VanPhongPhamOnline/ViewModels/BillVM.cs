using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.ViewModels
{
    public class BillVM
    {
        [Display(Name = "Mã hóa đơn")]
        public int MaHd { get; set; }

        [Display(Name = "Mã khách hàng")]
        public string MaKh { get; set; } = null!;

        [Display(Name = "Tên khách hàng")]
        public string? TenKhachHang { get; set; }

        [Display(Name = "Ngày đặt")]
        [DataType(DataType.DateTime)]
        public DateTime NgayDat { get; set; }

        [Display(Name = "Ngày cần")]
        [DataType(DataType.DateTime)]
        public DateTime? NgayCan { get; set; }

        [Display(Name = "Ngày giao")]
        [DataType(DataType.DateTime)]
        public DateTime? NgayGiao { get; set; }

        [Display(Name = "Người nhận")]
        [MaxLength(50)]
        public string? HoTenNguoiNhan { get; set; }

        [Display(Name = "Địa chỉ nhận hàng")]
        [MaxLength(60)]
        public string DiaChi { get; set; } = null!;

        [Display(Name = "Cách thanh toán")]
        public string CachThanhToan { get; set; } = null!;

        [Display(Name = "Cách vận chuyển")]
        public string CachVanChuyen { get; set; } = null!;

        [Display(Name = "Phí vận chuyển")]
        [DataType(DataType.Currency)]
        public double PhiVanChuyen { get; set; }

        [Display(Name = "Trạng thái")]
        public string? TenTrangThai { get; set; }

        [Display(Name = "Nhân viên phụ trách")]
        public string? TenNhanVien { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Tổng tiền")]
        [DataType(DataType.Currency)]
        public decimal TongTien { get; set; }

        [Display(Name = "Chi tiết hóa đơn")]
        public List<BillDetailVM> ChiTietHds { get; set; } = new();
    }

    public class BillDetailVM
    {
        [Display(Name = "Mã hàng hóa")]
        public int MaHh { get; set; }

        [Display(Name = "Tên hàng hóa")]
        public string TenHh { get; set; } = null!;

        [Display(Name = "Đơn giá")]
        [DataType(DataType.Currency)]
        public double DonGia { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Thành tiền")]
        [DataType(DataType.Currency)]
        public decimal ThanhTien => (decimal)SoLuong * (decimal)DonGia;
    }
}
