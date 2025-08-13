namespace VanPhongPhamOnline.ViewModels
{
    public class HangHoaEditVM
    {
        public int MaHh { get; set; }
        public string TenHh { get; set; }
        public string MoTa { get; set; }
        public int MaLoai { get; set; }
        public int? MaNcc { get; set; }
        public double? DonGia { get; set; }
        public IFormFile? HinhAnh { get; set; }
    }
}
