using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class HoaDonController : Controller
    {
        private readonly MultiShopContext _context;

        public HoaDonController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: HoaDon
        public async Task<IActionResult> Index()
        {
            var multiShopContext = _context.HoaDons.Include(h => h.MaKhNavigation).Include(h => h.MaNvNavigation).Include(h => h.MaTrangThaiNavigation);
            return View("~/Views/Admin/QuanLyHoaDon/Index.cshtml", multiShopContext);
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
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/QuanLyHoaDon/Details.cshtml",hoaDon);
        }

        // GET: HoaDon/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "MaTrangThai");
            return View();
        }

        // POST: HoaDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHd,MaKh,NgayDat,NgayCan,NgayGiao,HoTenNguoiNhan,DiaChi,CachThanhToan,CachVanChuyen,PhiVanChuyen,MaTrangThai,MaNv,GhiChu")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDon.MaNv);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "MaTrangThai", hoaDon.MaTrangThai);
            return View("~/Views/Admin/QuanLyHoaDon/Create.cshtml");
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
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDon.MaNv);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "MaTrangThai", hoaDon.MaTrangThai);

            return View("~/Views/Admin/QuanLyHoaDon/Edit.cshtml", hoaDon);
        }

        // POST: HoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.MaHd))
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
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
                // Xem lỗi ở đây (debug, log hoặc trả về View cùng lỗi)
                // Ví dụ: truyền lỗi ra ViewData
                ViewData["Errors"] = errors;
            }

            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "TenKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "TenNv", hoaDon.MaNv);
            ViewData["MaTrangThai"] = new SelectList(
                _context.TrangThais.Select(t => new { t.MaTrangThai, TenTrangThai = t.MaTrangThai == 1 ? "Chưa xác nhận" : "Đã xác nhận" }),
                "MaTrangThai",
                "TenTrangThai",
                hoaDon.MaTrangThai
            );

            return View("~/Views/Admin/QuanLyHoaDon/Edit.cshtml", hoaDon);
        }

        // GET: HoaDon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/QuanLyHoaDon/Delete.cshtml");
        }

        // POST: HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.MaHd == id);
        }
    }
}
