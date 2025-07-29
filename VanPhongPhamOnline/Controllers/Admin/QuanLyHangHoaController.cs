using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class QuanLyHangHoaController : Controller
    {
        private readonly MultiShopContext _context;

        public QuanLyHangHoaController(MultiShopContext context)
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
            ViewBag.LoaiList = _context.Loais.ToList();
            ViewBag.NccList = _context.NhaCungCaps.ToList();
            return View("~/Views/Admin/QuanLyHangHoa/Create.cshtml");
        }

        // POST: QuanLyHangHoa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HangHoa hangHoa, IFormFile? HinhAnh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var fileName = $"{hangHoa.MaHh}_{HinhAnh.FileName}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/HangHoa", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        hangHoa.Hinh = fileName;
                    }

                    _context.Add(hangHoa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm hàng hóa thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Lỗi khi thêm hàng hóa: " + ex.Message;
                }
            }

            ViewBag.LoaiList = _context.Loais.ToList();
            ViewBag.NccList = _context.NhaCungCaps.ToList();
            return View("~/Views/Admin/QuanLyHangHoa/Create.cshtml", hangHoa);
        }

        // GET: QuanLyHangHoa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
                return NotFound();

            ViewBag.LoaiList = _context.Loais.ToList();
            ViewBag.NccList = _context.NhaCungCaps.ToList();

            return View("~/Views/Admin/QuanLyHangHoa/Edit.cshtml", hangHoa);
        }

        // POST: QuanLyHangHoa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HangHoa hangHoa, IFormFile? HinhAnh)
        {
            if (id != hangHoa.MaHh)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var oldHangHoa = await _context.HangHoas.AsNoTracking().FirstOrDefaultAsync(h => h.MaHh == id);

                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var fileName = $"{hangHoa.MaHh}_{HinhAnh.FileName}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/HangHoa", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        // Xoá ảnh cũ
                        if (!string.IsNullOrEmpty(oldHangHoa?.Hinh))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/HangHoa", oldHangHoa.Hinh);
                            if (System.IO.File.Exists(oldImagePath))
                                System.IO.File.Delete(oldImagePath);
                        }

                        hangHoa.Hinh = fileName;
                    }
                    else
                    {
                        hangHoa.Hinh = oldHangHoa?.Hinh;
                    }

                    _context.Update(hangHoa);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật hàng hóa thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangHoaExists(hangHoa.MaHh))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.LoaiList = _context.Loais.ToList();
            ViewBag.NccList = _context.NhaCungCaps.ToList();
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
