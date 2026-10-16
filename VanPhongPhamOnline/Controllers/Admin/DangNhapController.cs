using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class DangNhapController : Controller
    {
        private const string LoginView = "~/Views/Admin/DangNhap/Index.cshtml";
        private const string QuenMatKhauView = "~/Views/Admin/DangNhap/QuenMatKhau.cshtml";
        private const string XacNhanMaView = "~/Views/Admin/DangNhap/XacNhanMa.cshtml";
        private const string DatLaiMatKhauView = "~/Views/Admin/DangNhap/DatLaiMatKhau.cshtml";

        private readonly MultiShopContext _context;
        private readonly MyUlti _ulti;
        public DangNhapController(MultiShopContext context, MyUlti ulti)
        {
            _context = context;
            _ulti = ulti;
        }

        #region Login
        [HttpGet]
        public IActionResult Index()
        {
            return View(LoginView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVM model, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
                return View(LoginView);

            var nv = _context.NhanViens.FirstOrDefault(x => x.EmailNv == model.UserName);
            if (nv == null || nv.MatKhauNv != model.Password.ToMd5Hash(nv.RandomKeyNv))
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                return View(LoginView);
            }

            if (nv.HieuLuc == false)
            {
                ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                return View(LoginView);
            }

            var isAdmin = nv.MaNv == AppAuth.AdminId
                || _context.PhanCongs.Any(p => p.MaNv == nv.MaNv && p.MaPb == AppAuth.AdminDepartment && p.HieuLuc == true);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nv.HoTenNv),
                new Claim(ClaimTypes.Email, nv.EmailNv),
                new Claim("MaNV", nv.MaNv),
                new Claim(ClaimTypes.Role, isAdmin ? AppAuth.AdminRole : AppAuth.StaffRole)
            };

            var claimsIdentity = new ClaimsIdentity(claims, AppAuth.StaffScheme);
            await HttpContext.SignInAsync(AppAuth.StaffScheme, new ClaimsPrincipal(claimsIdentity));
            HttpContext.Session.SetString("MaNV", nv.MaNv);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return isAdmin
                ? RedirectToAction("IndexAdmin", "Admin")
                : RedirectToAction("IndexNhanVien", "Admin");
        }

        public async Task<IActionResult> DangXuat()
        {
            HttpContext.Session.Remove("MaNV");
            await HttpContext.SignOutAsync(AppAuth.StaffScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
        #endregion

        #region Forgot password
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View(QuenMatKhauView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QuenMatKhau(QuenMatKhauVM model)
        {
            if (!ModelState.IsValid)
                return View(QuenMatKhauView, model);

            var nv = _context.NhanViens.FirstOrDefault(x => x.EmailNv == model.Email);
            if (nv == null)
            {
                ModelState.AddModelError("", "Email này chưa được đăng ký.");
                return View(QuenMatKhauView, model);
            }

            var otp = OtpHelper.Generate(HttpContext.Session, model.Email);

            var subject = "Xác nhận đặt lại mật khẩu - MultiShop";
            var content = $@"Xin chào {nv.HoTenNv}, mã OTP đặt lại mật khẩu của bạn là: <b>{otp}</b>. Mã có hiệu lực trong {OtpHelper.ExpireMinutes} phút. Đừng chia sẻ mã này cho bất kỳ ai.";
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

            return View(XacNhanMaView, new XacNhanMaVM { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XacNhanMa(XacNhanMaVM model)
        {
            if (!ModelState.IsValid)
                return View(XacNhanMaView, model);

            var error = OtpHelper.Verify(HttpContext.Session, model.Email, model.MaXacNhan);
            if (error != null)
            {
                ModelState.AddModelError("", error);
                return View(XacNhanMaView, model);
            }

            TempData["Email"] = model.Email;
            return RedirectToAction("DatLaiMatKhau");
        }

        [HttpGet]
        public IActionResult DatLaiMatKhau()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email) || !OtpHelper.IsVerified(HttpContext.Session, email))
                return RedirectToAction("QuenMatKhau");

            return View(DatLaiMatKhauView, new DatLaiMatKhauVM { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DatLaiMatKhau(DatLaiMatKhauVM model)
        {
            // Chỉ cho phép đặt lại mật khẩu khi email đã xác nhận OTP thành công
            if (!OtpHelper.IsVerified(HttpContext.Session, model.Email))
                return RedirectToAction("QuenMatKhau");

            if (!ModelState.IsValid)
                return View(DatLaiMatKhauView, model);

            var nv = _context.NhanViens.FirstOrDefault(x => x.EmailNv == model.Email);
            if (nv == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(DatLaiMatKhauView, model);
            }

            nv.RandomKeyNv = MyUlti.GenerateRandomKey();
            nv.MatKhauNv = model.MatKhauMoi.ToMd5Hash(nv.RandomKeyNv);
            _context.SaveChanges();

            OtpHelper.Clear(HttpContext.Session);

            TempData["Success"] = "Mật khẩu đã được cập nhật. Vui lòng đăng nhập lại.";
            return RedirectToAction("Index");
        }
        #endregion
    }
}
