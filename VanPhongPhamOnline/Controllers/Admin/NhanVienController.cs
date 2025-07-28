using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class NhanVienController : Controller
    {
        private readonly MultiShopContext _context;

        public NhanVienController(MultiShopContext context)
        {
            _context = context;
        }



        // GET: NhanVien
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Admin/NhanVien/Index.cshtml", await _context.NhanViens.ToListAsync());
        }

        // GET: NhanVien/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/NhanVien/Details.cshtml", nhanVien);
        }

        // GET: NhanVien/Create
        public IActionResult Create()
        {
            return View("~/Views/Admin/NhanVien/Create.cshtml");
        }

        // POST: NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNv,HoTenNv,NgaySinhNv,GioiTinhNv,DiaChiNv,EmailNv,MatKhauNv")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                string randomKey = MyUlti.GenerateRandomKey();
                nhanVien.MatKhauNv = nhanVien.MatKhauNv.ToMd5Hash(randomKey);
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/NhanVien/Index.cshtml", nhanVien);
        }

        // GET: NhanVien/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View("~/Views/Admin/NhanVien/Edit.cshtml", nhanVien);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNv,HoTenNv,NgaySinhNv,GioiTinhNv,DiaChiNv,EmailNv,MatKhauNv")] NhanVien nhanVien)
        {
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.MaNv))
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

            return View("~/Views/Admin/NhanVien/Edit.cshtml", nhanVien);
        }

        // GET: NhanVien/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/NhanVien/Delete.cshtml", nhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(string id)
        {
            return _context.NhanViens.Any(e => e.MaNv == id);
        }
    }
}
