using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class DangNhapController : Controller
    {
        private readonly MultiShopContext _context;
        private readonly MyUlti _ulti;
        public DangNhapController(MultiShopContext context, MyUlti ulti)
        {
            _context = context;
            _ulti = ulti;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Admin/DangNhap/Index.cshtml");
        }
        public IActionResult IndexPartial()
        {
            var list = _context.NhanViens.ToList();
            return PartialView("Index", list); // Index.cshtml dùng layout = null hoặc check isPartial
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/DangNhap/Index.cshtml");

            var nv = _context.NhanViens.FirstOrDefault(x => x.EmailNv == model.UserName);

            if (nv == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                return View("~/Views/Admin/DangNhap/Index.cshtml"); // trả về trang login kèm thông báo
            }
            
            if (nv.MatKhauNv != model.Password.ToMd5Hash(nv.RandomKeyNv))
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View("~/Views/Admin/DangNhap/Index.cshtml");
            }
            //var pc = _context.PhanCongs
            //    .FirstOrDefault(p => p.MaNv == nv.MaNv && p.HieuLuc == true);

            //if (pc == null)
            //{
            //    ModelState.AddModelError("", "Tài khoản chưa được phân công!");
            //    return View("~/Views/Admin/DangNhap/Index.cshtml");
            //}

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nv.HoTenNv),
                new Claim("MaNV", nv.MaNv)
                //new Claim("MaPB", pc.MaPb)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.Session.SetString("MaNV", nv.MaNv);
            await HttpContext.SignInAsync(principal);

            if(nv.MaNv== "AD001")
            {
                return View("~/Views/Admin/IndexAdmin.cshtml");
            }
            else
            {
                return View("~/Views/Admin/IndexNhanVien.cshtml");
            }
        }

        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            await HttpContext.SignOutAsync("KhachHang");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View("~/Views/Admin/DangNhap/QuenMatKhau.cshtml");

        }
        [HttpPost]
        public IActionResult QuenMatKhau(QuenMatKhauVM model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/DangNhap/Index.cshtml");

            var nv = _context.NhanViens.FirstOrDefault(x => x.EmailNv == model.Email);
            if (nv == null)
            {
                ModelState.AddModelError("", "Email này chưa được đăng ký.");
                return View("~/Views/Admin/DangNhap/Index.cshtml");
            }

            // Tạo mã OTP ngẫu nhiên
            var otp = new Random().Next(100000, 999999).ToString();
            var expireTime = DateTime.Now.AddMinutes(5);

            // Lưu vào session
            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("OTP_Email", model.Email);
            HttpContext.Session.SetString("OTP_Expire", expireTime.ToString("O")); // ISO 8601 format

            // Gửi email
            var subject = "Xác nhận đặt lại mật khẩu - MultiShop";
            var content = $@"Xin chào {nv.HoTenNv}, mã OTP đặt lại mật khẩu của bạn là: {otp}. Mã có hiệu lực trong 5 phút. Đừng chia sẻ mã này cho bất kỳ ai.";

            _ulti.SendMail(nv.EmailNv, subject, content);


            TempData["Email"] = model.Email;
            return RedirectToAction("XacNhanMa");
        }



        [HttpGet]
        public IActionResult XacNhanMa()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("QuenMatKhau");

            return View("~/Views/Admin/DangNhap/XacNhanMa.cshtml", new XacNhanMaVM { Email = email });
        }

        [HttpPost]
        public IActionResult XacNhanMa(XacNhanMaVM model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/DangNhap/XacNhanMa.cshtml", model);

            var otp = HttpContext.Session.GetString("OTP");
            var otpEmail = HttpContext.Session.GetString("OTP_Email");
            var otpExpireStr = HttpContext.Session.GetString("OTP_Expire");

            if (otp == null || otpEmail == null || otpExpireStr == null)
            {
                ModelState.AddModelError("", "Mã xác nhận đã hết hạn hoặc không hợp lệ.");
                return View("~/Views/Admin/DangNhap/XacNhanMa.cshtml", model);
            }

            if (otpEmail != model.Email)
            {
                ModelState.AddModelError("", "Email không khớp với mã xác nhận.");
                return View("~/Views/Admin/DangNhap/XacNhanMa.cshtml", model);
            }

            if (otp != model.MaXacNhan)
            {
                ModelState.AddModelError("", "Mã xác nhận không chính xác.");
                return View("~/Views/Admin/DangNhap/XacNhanMa.cshtml", model);
            }

            if (DateTime.TryParse(otpExpireStr, out var otpExpire) && DateTime.Now > otpExpire)
            {
                ModelState.AddModelError("", "Mã xác nhận đã hết hạn.");
                return View("~/Views/Admin/DangNhap/XacNhanMa.cshtml", model);
            }

            // Thành công → lưu lại email qua TempData
            TempData["Email"] = model.Email;
            return RedirectToAction("DatLaiMatKhau");
        }

        [HttpGet]
        public IActionResult DatLaiMatKhau()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("QuenMatKhau");

            return View("~/Views/Admin/DangNhap/DatLaiMatKhau.cshtml", new DatLaiMatKhauVM { Email = email });
        }

        [HttpPost]
        public IActionResult DatLaiMatKhau(DatLaiMatKhauVM model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/DangNhap/DatLaiMatKhau.cshtml", model);

            var kh = _context.NhanViens.FirstOrDefault(x => x.EmailNv == model.Email);
            if (kh == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View("~/Views/Admin/DangNhap/DatLaiMatKhau.cshtml", model);
            }

            kh.RandomKeyNv = MyUlti.GenerateRandomKey();
            kh.MatKhauNv = model.MatKhauMoi.ToMd5Hash(kh.RandomKeyNv);
            _context.SaveChanges();

            // Xoá OTP trong session sau khi dùng
            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("OTP_Email");
            HttpContext.Session.Remove("OTP_Expire");

            TempData["Success"] = "Mật khẩu đã được cập nhật. Vui lòng đăng nhập lại.";
            return RedirectToAction("Index");
        }
    }
}
