using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;

namespace VanPhongPhamOnline.Controllers
{
    public class PhanCongController : Controller
    {
        private readonly MultiShopContext _context;

        public PhanCongController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: PhanCong
        public async Task<IActionResult> Index()
        {
            var multiShopContext = _context.PhanCongs.Include(p => p.MaNvNavigation).Include(p => p.MaPbNavigation);
            return View(await multiShopContext.ToListAsync());
        }

        // GET: PhanCong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanCong = await _context.PhanCongs
                .Include(p => p.MaNvNavigation)
                .Include(p => p.MaPbNavigation)
                .FirstOrDefaultAsync(m => m.MaPc == id);
            if (phanCong == null)
            {
                return NotFound();
            }

            return View(phanCong);
        }

        // GET: PhanCong/Create
        public IActionResult Create()
        {
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            ViewData["MaPb"] = new SelectList(_context.PhongBans, "MaPb", "MaPb");
            return View();
        }

        // POST: PhanCong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPc,MaNv,MaPb,NgayPc,HieuLuc")] PhanCong phanCong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phanCong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", phanCong.MaNv);
            ViewData["MaPb"] = new SelectList(_context.PhongBans, "MaPb", "MaPb", phanCong.MaPb);
            return View(phanCong);
        }

        // GET: PhanCong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanCong = await _context.PhanCongs.FindAsync(id);
            if (phanCong == null)
            {
                return NotFound();
            }
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", phanCong.MaNv);
            ViewData["MaPb"] = new SelectList(_context.PhongBans, "MaPb", "MaPb", phanCong.MaPb);
            return View(phanCong);
        }

        // POST: PhanCong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPc,MaNv,MaPb,NgayPc,HieuLuc")] PhanCong phanCong)
        {
            if (id != phanCong.MaPc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanCong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhanCongExists(phanCong.MaPc))
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
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", phanCong.MaNv);
            ViewData["MaPb"] = new SelectList(_context.PhongBans, "MaPb", "MaPb", phanCong.MaPb);
            return View(phanCong);
        }

        // GET: PhanCong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanCong = await _context.PhanCongs
                .Include(p => p.MaNvNavigation)
                .Include(p => p.MaPbNavigation)
                .FirstOrDefaultAsync(m => m.MaPc == id);
            if (phanCong == null)
            {
                return NotFound();
            }

            return View(phanCong);
        }

        // POST: PhanCong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phanCong = await _context.PhanCongs.FindAsync(id);
            if (phanCong != null)
            {
                _context.PhanCongs.Remove(phanCong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhanCongExists(int id)
        {
            return _context.PhanCongs.Any(e => e.MaPc == id);
        }
    }
}
