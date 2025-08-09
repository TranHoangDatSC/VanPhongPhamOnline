using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class QuanLyDonHangController : Controller
    {
        private readonly MultiShopContext _context;

        public QuanLyDonHangController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: QuanLyDonHang
        public async Task<IActionResult> Index()
        {
            var hoaDons = await _context.HoaDons
                .Include(h => h.MaTrangThaiNavigation)
                .Include(h => h.MaKhNavigation)
                .ToListAsync();

            return View("~/Views/Admin/QuanLyDonHang/Index.cshtml", hoaDons);
        }

        // GET: QuanLyDonHang/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.MaTrangThaiNavigation)
                .Include(h => h.MaKhNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/QuanLyDonHang/Details.cshtml", hoaDon);
        }

        // GET: QuanLyDonHang/Create
        public IActionResult Create()
        {
            return View("~/Views/Admin/QuanLyDonHang/Create.cshtml");
        }

        // POST: QuanLyDonHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/QuanLyDonHang/Create.cshtml", hoaDon);
        }

        // GET: QuanLyDonHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/QuanLyDonHang/Edit.cshtml", hoaDon);
        }

        // POST: QuanLyDonHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHd,MaKh,NgayDat,NgayCan,NgayGiao,HoTenNguoiNhan,DiaChi,CachThanhToan,CachVanChuyen,PhiVanChuyen,MaTrangThai,MaNv,GhiChu")]
 HoaDon hoaDon)
        {
            if (id != hoaDon.MaHd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.HoaDons.Any(e => e.MaHd == hoaDon.MaHd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/QuanLyDonHang/Edit.cshtml");
        }


        // GET: QuanLyDonHang/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var hoaDon = await _context.HoaDons
                .Include(h => h.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/QuanLyDonHang/Delete.cshtml", hoaDon);
        }

        // POST: QuanLyDonHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa đơn hàng thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
