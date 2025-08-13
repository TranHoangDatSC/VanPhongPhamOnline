using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class QuanLyKhachHangController : Controller
    {
        private readonly MultiShopContext _context;

        public QuanLyKhachHangController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: QuanLyKhachHang
        public async Task<IActionResult> Index()
        {
            var khachHangs = await _context.KhachHangs
                .Where(kh => !kh.IsDeleted) // Chỉ lấy khách hàng chưa xóa
                .ToListAsync();
            return View("~/Views/Admin/QuanLyKhachHang/Index.cshtml", khachHangs);
        }

        // GET: QuanLyKhachHang/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/QuanLyKhachHang/Details.cshtml", khachHang);
        }

        // GET: QuanLyKhachHang/Create
        public IActionResult Create()
        {
            return View("~/Views/Admin/QuanLyKhachHang/Create.cshtml");
        }

        // POST: QuanLyKhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKh,HoTenKh,GioiTinhKh,NgaySinhKh,DiaChiKh,DienThoaiKh,EmailKh,MatKhauKh")] KhachHang khachHang, IFormFile? HinhKh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo random key và hash mật khẩu
                    string randomKey = MyUlti.GenerateRandomKey();
                    khachHang.RandomKeyKh = randomKey;
                    if (!string.IsNullOrEmpty(khachHang.MatKhauKh))
                    {
                        khachHang.MatKhauKh = khachHang.MatKhauKh.ToMd5Hash(randomKey);
                    }

                    // Xử lý upload hình ảnh
                    if (HinhKh != null && HinhKh.Length > 0)
                    {
                        khachHang.HinhKh = MyUlti.UploadHinh(HinhKh, "KhachHang", khachHang.MaKh);
                    }

                    _context.Add(khachHang);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    TempData["ErrorMessage"] = "Lỗi khi thêm khách hàng: " + errorMessage;
                }
            }
            return View("~/Views/Admin/QuanLyKhachHang/Create.cshtml", khachHang);
        }

        // GET: QuanLyKhachHang/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/QuanLyKhachHang/Edit.cshtml", khachHang);
        }

        // POST: QuanLyKhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKh,HoTenKh,GioiTinhKh,NgaySinhKh,DiaChiKh,DienThoaiKh,EmailKh,MatKhauKh,RandomKeyKh")] KhachHang khachHang, IFormFile? HinhKh)
        {
            if (id != khachHang.MaKh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin khách hàng hiện tại từ DB
                    var existingKhachHang = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(k => k.MaKh == id);
                    if (existingKhachHang != null)
                    {
                        // Nếu mật khẩu trống hoặc không thay đổi, giữ nguyên mật khẩu cũ
                        if (string.IsNullOrEmpty(khachHang.MatKhauKh))
                        {
                            khachHang.MatKhauKh = existingKhachHang.MatKhauKh;
                            khachHang.RandomKeyKh = existingKhachHang.RandomKeyKh;
                        }
                        else if (!string.IsNullOrEmpty(khachHang.MatKhauKh))
                        {
                            // Mật khẩu mới được nhập, hash lại
                            string randomKey = MyUlti.GenerateRandomKey();
                            khachHang.RandomKeyKh = randomKey;
                            khachHang.MatKhauKh = khachHang.MatKhauKh.ToMd5Hash(randomKey);
                        }

                        // Xử lý hình ảnh
                        if (HinhKh != null && HinhKh.Length > 0)
                        {
                            // Xóa hình cũ nếu có
                            if (!string.IsNullOrEmpty(existingKhachHang.HinhKh) && existingKhachHang.HinhKh != "default-customer.jpg")
                            {
                                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/KhachHang", existingKhachHang.HinhKh);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }
                            // Upload hình mới
                            khachHang.HinhKh = MyUlti.UploadHinh(HinhKh, "KhachHang", khachHang.MaKh);
                        }
                        else
                        {
                            // Giữ nguyên hình ảnh cũ
                            khachHang.HinhKh = existingKhachHang.HinhKh;
                        }
                    }

                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật thông tin khách hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKh))
                    {
                        TempData["ErrorMessage"] = "Khách hàng không tồn tại!";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Lỗi cập nhật dữ liệu!";
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật khách hàng: " + errorMessage;
                }
            }

            return View("~/Views/Admin/QuanLyKhachHang/Edit.cshtml", khachHang);
        }

        // GET: QuanLyKhachHang/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/QuanLyKhachHang/Delete.cshtml", khachHang);
        }

        // POST: QuanLyKhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var khachHang = await _context.KhachHangs.FindAsync(id);
                if (khachHang != null)
                {
                    // Xóa hình ảnh nếu không phải hình mặc định
                    if (!string.IsNullOrEmpty(khachHang.HinhKh) && khachHang.HinhKh != "default-customer.jpg")
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/KhachHang", khachHang.HinhKh);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    khachHang.IsDeleted = true;
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy khách hàng!";
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["ErrorMessage"] = "Lỗi khi xóa khách hàng: " + errorMessage;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKh == id);
        }
    }
}
