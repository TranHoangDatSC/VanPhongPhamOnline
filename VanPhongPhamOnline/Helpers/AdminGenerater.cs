using Microsoft.AspNetCore.Mvc;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Helpers
{
    // DELETE AFTER ADMIN CREATED
    public class AdminGenerater : Controller
    {
        private readonly MultiShopContext _context;

        public AdminGenerater(MultiShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string email = "admin@gmail.com";
            string password = "123456";

            // Kiểm tra nếu đã tồn tại admin
            var existing = _context.NhanViens.FirstOrDefault(x => x.EmailNv == email);
            if (existing != null)
            {
                return Content("Tài khoản admin đã tồn tại.");
            }

            // Tạo admin
            string randomKey = MyUlti.GenerateRandomKey();
            string hashedPassword = password.ToMd5Hash(randomKey);

            var admin = new NhanVien
            {
                MaNv = "AD001",
                HoTenNv = "Admin",
                EmailNv = email,
                MatKhauNv = hashedPassword,
                NgaySinhNv = new DateTime(2005, 11, 23),
                GioiTinhNv = true,
                DiaChiNv = "Hệ thống",
                HieuLuc = true,
                RandomKeyNv = randomKey
            };

            _context.NhanViens.Add(admin);

            // Gán phân công phòng ban ADMIN
            var phanCong = new PhanCong
            {
                MaNv = "AD001",
                MaPb = "ADMIN",
                NgayPc = DateTime.Now,
                HieuLuc = true
            };

            _context.PhanCongs.Add(phanCong);
            _context.SaveChanges();

            return Content("Tài khoản admin đã được tạo và phân công vào phòng ban ADMIN.");
        }
    }
}
