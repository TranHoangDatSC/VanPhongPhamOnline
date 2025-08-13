using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.ViewModels
{
    public class XacNhanMaVM
    {
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mã xác nhận.")]
        public string MaXacNhan { get; set; } = null!;
    }
}
