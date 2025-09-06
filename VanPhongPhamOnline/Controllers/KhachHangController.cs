using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly MyUlti _ulti;
        public KhachHangController(MultiShopContext context, IMapper mapper, MyUlti ulti)
        {
            db = context;
            _mapper = mapper;
            _ulti = ulti;
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
                    khachHang.MaKh = model.MaKh;
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
            return View("~/Views/KhachHang/DangNhap.cshtml");
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
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.EmailKh == model.UserName);

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

                kh.IsDeleted = true;
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

        public IActionResult QuenMatKhau(QuenMatKhauVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var kh = db.KhachHangs.FirstOrDefault(x => x.EmailKh == model.Email);
            if (kh == null)
            {
                ModelState.AddModelError("", "Email này chưa được đăng ký.");
                return View(model);
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
            var content = $@"Xin chào {kh.HoTenKh}, mã OTP đặt lại mật khẩu của bạn là: {otp}. Mã có hiệu lực trong 5 phút. Đừng chia sẻ mã này cho bất kỳ ai.";

            _ulti.SendMail(kh.EmailKh, subject, content);


            TempData["Email"] = model.Email;
            return RedirectToAction("XacNhanMa");
        }

        [HttpGet]
        public IActionResult XacNhanMa()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("QuenMatKhau");

            return View(new XacNhanMaVM { Email = email });
        }

        [HttpPost]
        public IActionResult XacNhanMa(XacNhanMaVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var otp = HttpContext.Session.GetString("OTP");
            var otpEmail = HttpContext.Session.GetString("OTP_Email");
            var otpExpireStr = HttpContext.Session.GetString("OTP_Expire");

            if (otp == null || otpEmail == null || otpExpireStr == null)
            {
                ModelState.AddModelError("", "Mã xác nhận đã hết hạn hoặc không hợp lệ.");
                return View(model);
            }

            if (otpEmail != model.Email)
            {
                ModelState.AddModelError("", "Email không khớp với mã xác nhận.");
                return View(model);
            }

            if (otp != model.MaXacNhan)
            {
                ModelState.AddModelError("", "Mã xác nhận không chính xác.");
                return View(model);
            }

            if (DateTime.TryParse(otpExpireStr, out var otpExpire) && DateTime.Now > otpExpire)
            {
                ModelState.AddModelError("", "Mã xác nhận đã hết hạn.");
                return View(model);
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

            return View(new DatLaiMatKhauVM { Email = email });
        }

        [HttpPost]
        public IActionResult DatLaiMatKhau(DatLaiMatKhauVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var kh = db.KhachHangs.FirstOrDefault(x => x.EmailKh == model.Email);
            if (kh == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(model);
            }

            kh.RandomKeyKh = MyUlti.GenerateRandomKey();
            kh.MatKhauKh = model.MatKhauMoi.ToMd5Hash(kh.RandomKeyKh);
            db.SaveChanges();

            // Xoá OTP trong session sau khi dùng
            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("OTP_Email");
            HttpContext.Session.Remove("OTP_Expire");

            TempData["Success"] = "Mật khẩu đã được cập nhật. Vui lòng đăng nhập lại.";
            return RedirectToAction("DangNhap");
        }
        [Authorize]
        public IActionResult Details(int pageNumber = 1, int pageSize = 4)
        {
            var maKH = User.FindFirst("MaKH")?.Value;
            if (string.IsNullOrEmpty(maKH)) return Unauthorized();

            var query = db.HoaDons
                .Include(h => h.ChiTietHds)
                    .ThenInclude(ct => ct.MaHhNavigation)
                .Include(h => h.MaTrangThaiNavigation)
                .Include(h => h.MaNvNavigation)
                .Where(h => h.MaKh == maKH)
                .OrderByDescending(h => h.NgayDat);

            // tổng số hóa đơn
            int totalRecords = query.Count();

            // lấy dữ liệu phân trang
            var hoaDons = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = hoaDons.Select(h => new BillVM
            {
                MaHd = h.MaHd,
                MaKh = h.MaKh,
                TenKhachHang = h.MaKhNavigation?.HoTenKh,
                NgayDat = h.NgayDat,
                NgayCan = h.NgayCan,
                NgayGiao = h.NgayGiao,
                HoTenNguoiNhan = h.HoTenNguoiNhan,
                DiaChi = h.DiaChi,
                CachThanhToan = h.CachThanhToan,
                CachVanChuyen = h.CachVanChuyen,
                PhiVanChuyen = h.PhiVanChuyen,
                TenTrangThai = h.MaTrangThaiNavigation?.TenTrangThai,
                TenNhanVien = h.MaNvNavigation?.HoTenNv,
                GhiChu = h.GhiChu,
                TongTien = h.TongTien,
                ChiTietHds = h.ChiTietHds.Select(ct => new BillDetailVM
                {
                    MaHh = ct.MaHh,
                    TenHh = ct.MaHhNavigation?.TenHh ?? "Không rõ",
                    DonGia = ct.DonGia,
                    SoLuong = ct.SoLuong
                }).ToList()
            }).ToList();

            // truyền thêm thông tin phân trang qua ViewBag
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View("DetailsBill", result);
        }


    }

}
