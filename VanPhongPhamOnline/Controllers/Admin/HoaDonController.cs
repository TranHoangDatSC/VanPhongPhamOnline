using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Controllers.Admin
{
    [Authorize(Policy = AppAuth.StaffPolicy)]
    public class HoaDonController : Controller
    {
        private const string ViewFolder = "~/Views/Admin/QuanLyHoaDon/";

        private readonly MultiShopContext _context;

        public HoaDonController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: HoaDon
        public async Task<IActionResult> Index()
        {
            return await DanhSachHoaDon(null);
        }

        // GET: HoaDon/Search?query=KH001
        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            return await DanhSachHoaDon(query);
        }

        private async Task<IActionResult> DanhSachHoaDon(string? query)
        {
            ViewBag.IsAdmin = User.IsInRole(AppAuth.AdminRole);

            var hoaDonsQuery = _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaTrangThaiNavigation)
                .Include(h => h.ChiTietHds)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                hoaDonsQuery = hoaDonsQuery.Where(h => h.MaKh.Contains(query));
            }

            var hoaDons = await hoaDonsQuery
                .OrderByDescending(h => h.NgayDat)
                .ToListAsync();

            // Tổng tiền = tiền hàng + phí vận chuyển
            ViewBag.TongTienDict = hoaDons.ToDictionary(
                h => h.MaHd,
                h => h.TongTien + (decimal)h.PhiVanChuyen
            );
            ViewBag.Query = query;

            return View(ViewFolder + "Index.cshtml", hoaDons);
        }

        // GET: HoaDon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaTrangThaiNavigation)
                .Include(h => h.ChiTietHds)
                    .ThenInclude(ct => ct.MaHhNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(ViewFolder + "Details.cshtml", hoaDon);
        }

        // GET: HoaDon/Edit/5
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

            LoadSelectLists(hoaDon);
            return View(ViewFolder + "Edit.cshtml", hoaDon);
        }

        // POST: HoaDon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHd,MaKh,NgayDat,NgayCan,NgayGiao,HoTenNguoiNhan,DiaChi,CachThanhToan,CachVanChuyen,PhiVanChuyen,MaTrangThai,MaNv,GhiChu")] HoaDon hoaDon)
        {
            if (id != hoaDon.MaHd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingHoaDon = await _context.HoaDons.FindAsync(id);
                if (existingHoaDon == null)
                    return NotFound();

                existingHoaDon.MaKh = hoaDon.MaKh;
                existingHoaDon.NgayDat = hoaDon.NgayDat;
                existingHoaDon.NgayCan = hoaDon.NgayCan;
                existingHoaDon.NgayGiao = hoaDon.NgayGiao;
                existingHoaDon.HoTenNguoiNhan = hoaDon.HoTenNguoiNhan;
                existingHoaDon.DiaChi = hoaDon.DiaChi;
                existingHoaDon.CachThanhToan = hoaDon.CachThanhToan;
                existingHoaDon.CachVanChuyen = hoaDon.CachVanChuyen;
                existingHoaDon.PhiVanChuyen = hoaDon.PhiVanChuyen;
                existingHoaDon.MaTrangThai = hoaDon.MaTrangThai;
                existingHoaDon.MaNv = hoaDon.MaNv;
                existingHoaDon.GhiChu = hoaDon.GhiChu;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Errors"] = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            LoadSelectLists(hoaDon);
            return View(ViewFolder + "Edit.cshtml", hoaDon);
        }

        private void LoadSelectLists(HoaDon hoaDon)
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "HoTenKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "HoTenNv", hoaDon.MaNv);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai", hoaDon.MaTrangThai);
        }
    }
}
