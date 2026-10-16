using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Controllers.Admin
{
    [Authorize(Policy = AppAuth.AdminPolicy)]
    public class QuanLyHangHoaController : Controller
    {
        private const string ViewFolder = "~/Views/Admin/QuanLyHangHoa/";
        private const string HinhFolder = "HangHoa";

        private readonly MultiShopContext _context;

        public QuanLyHangHoaController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: QuanLyHangHoa
        public async Task<IActionResult> Index()
        {
            return await DanhSachHangHoa(null);
        }

        // GET: QuanLyHangHoa/Search?query=...
        public async Task<IActionResult> Search(string? query)
        {
            return await DanhSachHangHoa(query);
        }

        private async Task<IActionResult> DanhSachHangHoa(string? query)
        {
            var hangHoas = _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            // Giữ lại từ khóa để view hiển thị lại trong ô search
            ViewBag.Query = query;

            return View(ViewFolder + "Index.cshtml", await hangHoas.ToListAsync());
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

            return View(ViewFolder + "Details.cshtml", hangHoa);
        }

        // GET: QuanLyHangHoa/Create
        public IActionResult Create()
        {
            var model = new HangHoa
            {
                NgaySx = DateTime.Today,
                SoLanXem = 0,
                GiamGia = 0
            };

            LoadSelectLists(model);
            return View(ViewFolder + "Create.cshtml", model);
        }

        // POST: QuanLyHangHoa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HangHoa model, IFormFile? HinhUpload)
        {
            if (ModelState.IsValid)
            {
                // Gán mặc định
                model.GiamGia = 0;
                model.SoLanXem = 0;

                if (HinhUpload != null && HinhUpload.Length > 0)
                {
                    model.Hinh = MyUlti.UploadHinh(HinhUpload, HinhFolder);
                }

                _context.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm hàng hóa thành công!";
                return RedirectToAction(nameof(Index));
            }

            LoadSelectLists(model);
            return View(ViewFolder + "Create.cshtml", model);
        }

        // GET: QuanLyHangHoa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
                return NotFound();

            LoadSelectLists(hangHoa);
            return View(ViewFolder + "Edit.cshtml", hangHoa);
        }

        // POST: QuanLyHangHoa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HangHoa hangHoa, IFormFile? Hinh)
        {
            if (id != hangHoa.MaHh)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                LoadSelectLists(hangHoa);
                return View(ViewFolder + "Edit.cshtml", hangHoa);
            }

            var existingHangHoa = await _context.HangHoas.FindAsync(id);
            if (existingHangHoa == null)
            {
                return NotFound();
            }

            existingHangHoa.TenHh = hangHoa.TenHh;
            existingHangHoa.MaLoai = hangHoa.MaLoai;
            existingHangHoa.MaNcc = hangHoa.MaNcc;
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
                existingHangHoa.Hinh = MyUlti.UploadHinh(Hinh, HinhFolder);
            }

            try
            {
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật hàng hóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.GetBaseException().Message);
            }

            LoadSelectLists(hangHoa);
            return View(ViewFolder + "Edit.cshtml", hangHoa);
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

            return View(ViewFolder + "Delete.cshtml", hangHoa);
        }

        // POST: QuanLyHangHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hàng hóa!";
                return RedirectToAction(nameof(Index));
            }

            if (await _context.ChiTietHds.AnyAsync(ct => ct.MaHh == id))
            {
                TempData["ErrorMessage"] = "Không thể xóa hàng hóa đã có trong hóa đơn!";
                return RedirectToAction(nameof(Index));
            }

            _context.HangHoas.Remove(hangHoa);
            await _context.SaveChangesAsync();

            // Xoá hình ảnh sau khi xóa dữ liệu thành công
            if (!string.IsNullOrEmpty(hangHoa.Hinh))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", HinhFolder, hangHoa.Hinh);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            TempData["SuccessMessage"] = "Xóa hàng hóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        private void LoadSelectLists(HangHoa hangHoa)
        {
            ViewBag.MenuLoai = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai", hangHoa.MaLoai);
            ViewBag.NccList = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy", hangHoa.MaNcc);
        }
    }
}
