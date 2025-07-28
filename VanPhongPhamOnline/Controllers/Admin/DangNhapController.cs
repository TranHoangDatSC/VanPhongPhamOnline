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

        public DangNhapController(MultiShopContext context)
        {
            _context = context;
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

            var nv = _context.NhanViens.FirstOrDefault(n => n.EmailNv == model.UserName);

            if (nv == null || nv.MatKhauNv != model.Password.ToMd5Hash(nv.RandomKeyNv))
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View("~/Views/Admin/DangNhap/Index.cshtml");
            }

            if (nv == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View("~/Views/Admin/DangNhap/Index.cshtml");
            }

            var pc = _context.PhanCongs
                .FirstOrDefault(p => p.MaNv == nv.MaNv && p.HieuLuc == true);

            if (pc == null)
            {
                ModelState.AddModelError("", "Tài khoản chưa được phân công!");
                return View("~/Views/Admin/DangNhap/Index.cshtml");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nv.HoTenNv),
                new Claim("MaNV", nv.MaNv),
                new Claim("MaPB", pc.MaPb)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(principal);

            if(pc.MaPb == "ADMIN")
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
    }
}
