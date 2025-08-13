using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.ViewModels;
namespace VanPhongPhamOnline.Controllers.Admin
{
    public class QLHH : Controller
    {
        private readonly MultiShopContext _context;

        public QLHH(MultiShopContext context)
        {
            _context = context;
        }

        // GET: QuanLyHangHoa
        public async Task<IActionResult> Index()
        {
            var hangHoas = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .ToListAsync();

            return View("~/Views/Admin/QuanLyHangHoa/Index.cshtml", hangHoas);
        }

        // GET: QuanLyHangHoa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);

            if (hangHoa == null)
                return NotFound();

            return View("~/Views/Admin/QuanLyHangHoa/Details.cshtml", hangHoa);
        }

        // GET: QuanLyHangHoa/Create
        public IActionResult Create()
        {
            ViewBag.MenuLoai = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NccList = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy");

            var model = new HangHoa
            {
                NgaySx = DateTime.Today,
                SoLanXem = 0,
                GiamGia = 0
            };

            return View("~/Views/Admin/QuanLyHangHoa/Create.cshtml", model);
        }

        // POST: QuanLyHangHoa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HangHoa model, IFormFile HinhUpload)
        {
            if (ModelState.IsValid)
            {
                // Gán mặc định
                model.GiamGia = 0;
                model.SoLanXem = 0;

                // Xử lý hình ảnh
                if (HinhUpload != null && HinhUpload.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(HinhUpload.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/HangHoa", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhUpload.CopyToAsync(stream);
                    }

                    model.Hinh = fileName;
                }

                _context.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm hàng hóa thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi thì load lại ViewBag để dropdown hoạt động
            ViewBag.MenuLoai = new SelectList(_context.Loais, "MaLoai", "TenLoai", model.MaLoai);
            ViewBag.NccList = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy", model.MaNcc);
            return View("~/Views/Admin/QuanLyHangHoa/Create.cshtml", model);
        }

        // GET: QuanLyHangHoa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
                return NotFound();

            // Loại hàng
            ViewBag.MenuLoai = new SelectList(
                _context.Loais.ToList(),
                "MaLoai",       // Value
                "TenLoai",      // Text
                hangHoa.MaLoai  // Selected value
            );

            // Nhà cung cấp
            ViewBag.NccList = new SelectList(
                _context.NhaCungCaps.ToList(),
                "MaNcc",         // Value (string)
                "TenCongTy",     // Text
                hangHoa.MaNcc    // Selected value
            );

            ViewBag.MenuLoai = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai",hangHoa.MaLoai);
            ViewBag.NccList = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy",hangHoa.MaNcc);


            return View("~/Views/Admin/QuanLyHangHoa/Edit.cshtml", hangHoa);
        }

        // POST: QuanLyHangHoa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HangHoa hangHoa, IFormFile Hinh)
        {
            if (id != hangHoa.MaHh)
            {
                return NotFound();
            }

            // Nếu model không hợp lệ → load lại dropdown + return view
            if (!ModelState.IsValid)
            {
                ViewBag.MenuLoai = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai", hangHoa.MaLoai);
                ViewBag.NccList = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenNcc", hangHoa.MaNcc);
                return View("~/Views/Admin/QuanLyHangHoa/Edit.cshtml", hangHoa);
            }

            try
            {
                // Lấy dữ liệu cũ từ DB
                var existingHangHoa = await _context.HangHoas.FindAsync(id);
                if (existingHangHoa == null)
                {
                    return NotFound();
                }

                // Cập nhật các thuộc tính
                existingHangHoa.TenHh = hangHoa.TenHh;
                existingHangHoa.MaLoai = hangHoa.MaLoai;
                existingHangHoa.MaNcc = hangHoa.MaNcc.ToString();
                existingHangHoa.DonGia = hangHoa.DonGia;
                existingHangHoa.GiamGia = hangHoa.GiamGia;
                existingHangHoa.NgaySx = hangHoa.NgaySx;
                existingHangHoa.SoLanXem = hangHoa.SoLanXem;
                existingHangHoa.MoTaDonVi = hangHoa.MoTaDonVi;
                existingHangHoa.TenAlias = hangHoa.TenAlias;
                existingHangHoa.MoTa = hangHoa.MoTa;

                // Nếu có upload ảnh mới
                if (Hinh != null && Hinh.Length > 0)
                {
                    var fileName = Path.GetFileName(Hinh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/HangHoa", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Hinh.CopyToAsync(stream);
                    }

                    existingHangHoa.Hinh = fileName; // Lưu tên file mới vào DB
                }

                // Lưu thay đổi
                _context.Update(existingHangHoa);
                await _context.SaveChangesAsync();

                // Thông báo + chuyển hướng
                TempData["Success"] = "Cập nhật hàng hóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
            }

            // Nếu có lỗi → load lại dropdown + return view
            ViewBag.MenuLoai = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai", hangHoa.MaLoai);
            ViewBag.NccList = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy", hangHoa?.MaNcc);
            return View("~/Views/Admin/QuanLyHangHoa/Edit.cshtml", hangHoa);
        }



        // GET: QuanLyHangHoa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);

            if (hangHoa == null)
                return NotFound();

            return View("~/Views/Admin/QuanLyHangHoa/Delete.cshtml", hangHoa);
        }

        // POST: QuanLyHangHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa != null)
            {
                // Xoá hình ảnh nếu có
                if (!string.IsNullOrEmpty(hangHoa.Hinh))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/HangHoa", hangHoa.Hinh);
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                _context.HangHoas.Remove(hangHoa);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa hàng hóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy hàng hóa!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HangHoaExists(int id)
        {
            return _context.HangHoas.Any(e => e.MaHh == id);
        }
    }
}
