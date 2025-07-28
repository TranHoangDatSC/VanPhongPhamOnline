using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly MultiShopContext db;
        private readonly IMapper _mapper;
        public KhachHangController(MultiShopContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        #region Register       
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKeyKh = MyUlti.GenerateRandomKey();
                    khachHang.MatKhauKh = model.MatKhauKh.ToMd5Hash(khachHang.RandomKeyKh);

                    if (Hinh != null)
                    {
                        khachHang.HinhKh = MyUlti.UploadHinh(Hinh, "KhachHang", khachHang.MaKh);
                    }
                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "HangHoa");
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + errorMessage);
                    return View(model);
                }
            }
            return View();
        }
        #endregion

        #region Login in
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);

                if (khachHang == null)
                {
                    ModelState.AddModelError("", "Không tồn tại khách hàng này!");
                    return View();
                }

                var matKhauHash = model.Password.ToMd5Hash(khachHang.RandomKeyKh);

                if (khachHang.MatKhauKh != matKhauHash)
                {
                    ModelState.AddModelError("", "Sai thông tin đăng nhập!");
                    return View();
                }

                // Tạo claims cho người dùng
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, khachHang.EmailKh),
                    new Claim(ClaimTypes.Name, khachHang.HoTenKh),
                    new Claim("MaKH", khachHang.MaKh.ToString()),
                    new Claim("HinhKH", string.IsNullOrEmpty(khachHang.HinhKh) ? "default-customer.png" : khachHang.HinhKh),
                    new Claim("DienThoaiKH", khachHang.DienThoaiKh ?? ""),
                    new Claim("DiaChi", khachHang.DiaChiKh ?? ""),
                    new Claim("GioiTinh", khachHang.GioiTinhKh ? "Nam" : "Nữ"),
                    new Claim("NgaySinh", khachHang.NgaySinhKh.ToString("yyyy-MM-dd")),
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "KhachHang");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("KhachHang", new ClaimsPrincipal(claimsIdentity));


                // Điều hướng về URL cũ hoặc về trang chủ
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "HangHoa");
                }
            }

            return View();
        }

        #endregion

        [Authorize]
        public IActionResult Profile()
        {
            var maKH = User.FindFirst("MaKh")?.Value;
            var kh = db.KhachHangs.FirstOrDefault(k => k.MaKh == maKH);
            if (kh == null) return NotFound();

            var profile = new
            {
                kh.HoTenKh,
                kh.EmailKh,
                kh.DienThoaiKh,
                kh.DiaChiKh,
                GioiTinhKh = kh.GioiTinhKh ? "Nam" : "Nữ",
                NgaySinhKh = kh.NgaySinhKh == DateTime.MinValue ? "Chưa cập nhật" : kh.NgaySinhKh.ToString("dd/MM/yyyy"),
                HinhKh = string.IsNullOrEmpty(kh.HinhKh) ? "default.png" : kh.HinhKh,
                MaKH = kh.MaKh
            };

            ViewBag.Profile = profile;
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult EditProfile()
        {
            var maKH = User.FindFirst("MaKH")?.Value;
            var kh = db.KhachHangs.FirstOrDefault(k => k.MaKh == maKH);
            if (kh == null) return NotFound();

            var model = new EditProfileVM
            {
                HoTen = kh.HoTenKh,
                DiaChi = kh.DiaChiKh,
                DienThoai = kh.DienThoaiKh,
                GioiTinh = kh.GioiTinhKh,
                NgaySinh = kh.NgaySinhKh,
                Email = kh.EmailKh
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditProfile(EditProfileVM model)
        {
            var maKH = User.FindFirst("MaKH")?.Value;
            var kh = db.KhachHangs.FirstOrDefault(k => k.MaKh == maKH);
            if (kh == null) return NotFound();

            if (ModelState.IsValid)
            {
                kh.HoTenKh = model.HoTen;
                kh.DiaChiKh = model.DiaChi;
                kh.DienThoaiKh = model.DienThoai;
                kh.GioiTinhKh = model.GioiTinh;
                kh.NgaySinhKh = model.NgaySinh ?? DateTime.MinValue;
                kh.EmailKh = model.Email;

                if (model.Hinh != null)
                {
                    kh.HinhKh = MyUlti.UploadHinh(model.Hinh, "KhachHang", kh.MaKh);
                }

                db.SaveChanges();
                return RedirectToAction("Profile");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("KhachHang");
            return Redirect("/");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> XoaTaiKhoan()
        {
            var maKH = User.FindFirst("MaKH").Value;
            var kh = db.KhachHangs.FirstOrDefault(x => x.MaKh == maKH);

            if (kh != null)
            {
                // Xóa hình nếu không phải hình mặc định
                if (!string.IsNullOrEmpty(kh.HinhKh) && kh.HinhKh != "default-customer.png")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/KhachHang", kh.HinhKh);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                db.KhachHangs.Remove(kh);
                db.SaveChanges();
            }

            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QuenMatKhau(string Email)
        {
            var kh = db.KhachHangs.FirstOrDefault(x => x.EmailKh == Email);

            if (kh == null)
            {
                ModelState.AddModelError("", "Email này không tồn tại trong hệ thống.");
                return View();
            }

            // Tạo mật khẩu mới ngẫu nhiên
            var matKhauMoi = MyUlti.GenerateRandomKey(8);
            kh.RandomKeyKh = MyUlti.GenerateRandomKey();
            kh.MatKhauKh = matKhauMoi.ToMd5Hash(kh.RandomKeyKh);

            db.SaveChanges();

            // Gửi email
            var subject = "Cấp lại mật khẩu mới - MultiShop";
            var content = $@"Xin chào {kh.HoTenKh}, Mật khẩu mới của bạn là: {matKhauMoi} Vui lòng đăng nhập tại https://yourdomain.com/KhachHang/DangNhap và đổi mật khẩu ngay.";

            MyUlti.SendMail(kh.EmailKh, subject, content);

            ViewBag.Message = "Mật khẩu mới đã được gửi về email của bạn.";
            return View();
        }
    }
}
