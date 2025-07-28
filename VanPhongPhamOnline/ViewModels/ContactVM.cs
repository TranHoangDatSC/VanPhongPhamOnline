using System.ComponentModel.DataAnnotations;

namespace VanPhongPhamOnline.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Subject { get; set; } // Có thể không cần nếu đã fix "Liên hệ"

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Message { get; set; }
    }
}
