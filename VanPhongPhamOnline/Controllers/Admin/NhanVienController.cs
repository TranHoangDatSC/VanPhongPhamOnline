using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

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
        public async Task<IActionResult> Create([Bind("MaNv,HoTenNv,NgaySinhNv,GioiTinhNv,DiaChiNv,EmailNv,MatKhauNv,DienThoaiNv")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                string randomKey = MyUlti.GenerateRandomKey();
                nhanVien.RandomKeyNv = randomKey;
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
        public async Task<IActionResult> Edit(string id, [Bind("MaNv,HoTenNv,NgaySinhNv,GioiTinhNv,DiaChiNv,EmailNv,MatKhauNv,DienThoaiNv")] NhanVien nhanVien)
        {
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.NhanViens.AsNoTracking().FirstOrDefaultAsync(n => n.MaNv == id);
                    if (existing == null)
                        return NotFound();

                    // Nếu người dùng không thay đổi mật khẩu (giữ nguyên), thì không mã hóa lại
                    if (nhanVien.MatKhauNv != existing.MatKhauNv)
                    {
                        // 👇 Tạo random key để mã hóa mới nếu có thay đổi
                        string randomKey = MyUlti.GenerateRandomKey();
                        nhanVien.RandomKeyNv = randomKey;
                        nhanVien.MatKhauNv = nhanVien.MatKhauNv.ToMd5Hash(randomKey); // 👈 Mã hóa mật khẩu mới
                    }
                    else
                    {
                        nhanVien.MatKhauNv = existing.MatKhauNv; // giữ nguyên mật khẩu cũ
                        nhanVien.RandomKeyNv = existing.RandomKeyNv;
                    }
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
            else
            {
                var allErrors = ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .Select(ms => new
                {
                    Field = ms.Key,
                    Errors = ms.Value.Errors.Select(e => e.ErrorMessage)
                });

                foreach (var fieldError in allErrors)
                {
                    Console.WriteLine($"Field: {fieldError.Field}");
                    foreach (var err in fieldError.Errors)
                    {
                        Console.WriteLine($"  Error: {err}");
                    }
                }
                return View("~/Views/Admin/NhanVien/Edit.cshtml", nhanVien);
            }
            //return View("~/Views/Admin/NhanVien/Edit.cshtml", nhanVien);
        }

        // GET: NhanVien/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == "AD001")
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản quản trị viên!";
                return RedirectToAction("Index");
            }

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
        [HttpGet]
        public IActionResult Profile()
        {
            var maNV = User.FindFirst("MaNV")?.Value;
            var nv = _context.NhanViens.FirstOrDefault(n => n.MaNv == maNV);
            if (nv == null) return NotFound();

            return View("~/Views/Admin/NhanVien/Profile.cshtml", nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(NhanVien model)
        {
            var maNV = User.FindFirst("MaNV")?.Value;
            var nv = _context.NhanViens.FirstOrDefault(k => k.MaNv == maNV);
            if (nv == null) return NotFound();
            ModelState.Remove("MaNv");
            ModelState.Remove("EmailNv");
            if (ModelState.IsValid)
            {
                // So sánh mật khẩu
                if (model.MatKhauNv != nv.MatKhauNv)
                {
                    string randomKey = MyUlti.GenerateRandomKey();
                    nv.RandomKeyNv = randomKey;
                    nv.MatKhauNv = model.MatKhauNv.ToMd5Hash(randomKey);
                }
                // Cập nhật các field khác
                nv.HoTenNv = model.HoTenNv;
                nv.EmailNv = model.EmailNv;
                nv.DiaChiNv = model.DiaChiNv;
                nv.DienThoaiNv = model.DienThoaiNv;
                nv.GioiTinhNv = model.GioiTinhNv;
                nv.NgaySinhNv = model.NgaySinhNv;

                _context.SaveChanges();
                ViewBag.Success = "Cập nhật thành công!";
                return View("~/Views/Admin/IndexNhanVien.cshtml", nv);
            }
            else
            {
                var allErrors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage)
                    });

                foreach (var fieldError in allErrors)
                {
                    Console.WriteLine($"Field: {fieldError.Field}");
                    foreach (var err in fieldError.Errors)
                    {
                        Console.WriteLine($"  Error: {err}");
                    }
                }
            }

            return View("~/Views/Admin/NhanVien/Profile.cshtml", nv);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            var nhanViens = _context.NhanViens.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                nhanViens = nhanViens.Where(nv =>
                    nv.MaNv.Contains(query) ||
                    nv.HoTenNv.Contains(query));
            }

            var result = await nhanViens.ToListAsync();

            return View("~/Views/Admin/NhanVien/Index.cshtml", result);
        }

    }
}
