using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.ViewModels;

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
            var hangHoas = await _context.HangHoas.ToListAsync();
            return View(hangHoas);
        }

        // GET: QuanLyHangHoa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .FirstOrDefaultAsync(m => m.MaHh == id);

            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // GET: QuanLyHangHoa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuanLyHangHoa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHh,TenHh,TenAlias,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa")] HangHoa hangHoa)
        {
            // Hardcode ID
            ModelState.Remove("MaLoaiNavigation");
            ModelState.Remove("MaNccNavigation");
            ModelState.Remove("MaNcc");
            hangHoa.MaLoai = 1;
            hangHoa.MaNcc = "NCC01";
            hangHoa.MaLoaiNavigation = await _context.Loais.FindAsync(1);
            hangHoa.MaNccNavigation = await _context.NhaCungCaps.FindAsync("NCC01");

            if (ModelState.IsValid)
            {
                _context.Add(hangHoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hangHoa);
        }

        // GET: QuanLyHangHoa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                return NotFound();
            }
            return View(hangHoa);
        }

        // POST: QuanLyHangHoa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHh,TenHh,TenAlias,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa")] HangHoa hangHoa)
        {
            if (id != hangHoa.MaHh)
            {
                return NotFound();
            }

            // Hardcode ID
            ModelState.Remove("MaLoaiNavigation");
            ModelState.Remove("MaNccNavigation");
            ModelState.Remove("MaNcc");
            hangHoa.MaLoai = 1;
            hangHoa.MaNcc = "NCC01";
            hangHoa.MaLoaiNavigation = await _context.Loais.FindAsync(1);
            hangHoa.MaNccNavigation = await _context.NhaCungCaps.FindAsync("NCC01");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hangHoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangHoaExists(hangHoa.MaHh))
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
            return View(hangHoa);
        }


        // GET: QuanLyHangHoa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .FirstOrDefaultAsync(m => m.MaHh == id);

            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // POST: QuanLyHangHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa != null)
            {
                _context.HangHoas.Remove(hangHoa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HangHoaExists(int id)
        {
            return _context.HangHoas.Any(e => e.MaHh == id);
        }
        public async Task<IActionResult> Search(string? query)
        {
            var hangHoas = _context.HangHoas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Trim();
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            // Giữ lại từ khóa để view hiển thị lại trong ô search
            ViewBag.Query = query;

            return View("Index", await hangHoas.ToListAsync());
        }

    }
}
