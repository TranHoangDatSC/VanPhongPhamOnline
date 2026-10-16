using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Controllers.Admin
{
    [Authorize(Policy = AppAuth.StaffPolicy)]
    public class NhanVienController : Controller
    {
        private const string ViewFolder = "~/Views/Admin/NhanVien/";

        private readonly MultiShopContext _context;

        public NhanVienController(MultiShopContext context)
        {
            _context = context;
        }

        // GET: NhanVien
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public async Task<IActionResult> Index()
        {
            return View(ViewFolder + "Index.cshtml", await _context.NhanViens.ToListAsync());
        }

        // GET: NhanVien/Details/5
        [Authorize(Policy = AppAuth.AdminPolicy)]
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

            return View(ViewFolder + "Details.cshtml", nhanVien);
        }

        // GET: NhanVien/Create
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public IActionResult Create()
        {
            return View(ViewFolder + "Create.cshtml");
        }

        // POST: NhanVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public async Task<IActionResult> Create([Bind("MaNv,HoTenNv,NgaySinhNv,GioiTinhNv,DiaChiNv,EmailNv,MatKhauNv,DienThoaiNv")] NhanVien nhanVien)
        {
            if (string.IsNullOrWhiteSpace(nhanVien.MatKhauNv))
            {
                ModelState.AddModelError(nameof(NhanVien.MatKhauNv), "Vui lòng nhập mật khẩu.");
            }

            if (await _context.NhanViens.AnyAsync(n => n.MaNv == nhanVien.MaNv))
            {
                ModelState.AddModelError(nameof(NhanVien.MaNv), "Mã nhân viên đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                string randomKey = MyUlti.GenerateRandomKey();
                nhanVien.RandomKeyNv = randomKey;
                nhanVien.MatKhauNv = nhanVien.MatKhauNv!.ToMd5Hash(randomKey);
                nhanVien.HieuLuc = true;
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ViewFolder + "Create.cshtml", nhanVien);
        }

        // GET: NhanVien/Edit/5
        [Authorize(Policy = AppAuth.AdminPolicy)]
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
            return View(ViewFolder + "Edit.cshtml", nhanVien);
        }

        // POST: NhanVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public async Task<IActionResult> Edit(string id, [Bind("MaNv,HoTenNv,NgaySinhNv,GioiTinhNv,DiaChiNv,EmailNv,MatKhauNv,DienThoaiNv")] NhanVien nhanVien)
        {
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(ViewFolder + "Edit.cshtml", nhanVien);
            }

            var existing = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaNv == id);
            if (existing == null)
                return NotFound();

            existing.HoTenNv = nhanVien.HoTenNv;
            existing.NgaySinhNv = nhanVien.NgaySinhNv;
            existing.GioiTinhNv = nhanVien.GioiTinhNv;
            existing.DiaChiNv = nhanVien.DiaChiNv;
            existing.EmailNv = nhanVien.EmailNv;
            existing.DienThoaiNv = nhanVien.DienThoaiNv;

            // Để trống mật khẩu = giữ nguyên mật khẩu cũ
            if (!string.IsNullOrEmpty(nhanVien.MatKhauNv))
            {
                existing.RandomKeyNv = MyUlti.GenerateRandomKey();
                existing.MatKhauNv = nhanVien.MatKhauNv.ToMd5Hash(existing.RandomKeyNv);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NhanVien/Delete/5
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id == AppAuth.AdminId)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản quản trị viên!";
                return RedirectToAction(nameof(Index));
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(ViewFolder + "Delete.cshtml", nhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == AppAuth.AdminId)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản quản trị viên!";
                return RedirectToAction(nameof(Index));
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Không thể xóa nhân viên đang có dữ liệu liên quan (hóa đơn, phân công...).";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: NhanVien/Search
        [HttpGet]
        [Authorize(Policy = AppAuth.AdminPolicy)]
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

            return View(ViewFolder + "Index.cshtml", result);
        }

        #region Profile (nhân viên tự cập nhật thông tin)
        [HttpGet]
        public IActionResult Profile()
        {
            var maNV = User.FindFirst("MaNV")?.Value;
            var nv = _context.NhanViens.FirstOrDefault(n => n.MaNv == maNV);
            if (nv == null) return NotFound();

            return View(ViewFolder + "Profile.cshtml", nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(NhanVien model)
        {
            var maNV = User.FindFirst("MaNV")?.Value;
            var nv = _context.NhanViens.FirstOrDefault(k => k.MaNv == maNV);
            if (nv == null) return NotFound();

            // Mã nhân viên lấy từ tài khoản đăng nhập, không lấy từ form
            ModelState.Remove(nameof(NhanVien.MaNv));

            if (!ModelState.IsValid)
            {
                return View(ViewFolder + "Profile.cshtml", nv);
            }

            // Để trống mật khẩu = giữ nguyên mật khẩu cũ
            if (!string.IsNullOrEmpty(model.MatKhauNv))
            {
                nv.RandomKeyNv = MyUlti.GenerateRandomKey();
                nv.MatKhauNv = model.MatKhauNv.ToMd5Hash(nv.RandomKeyNv);
            }

            nv.HoTenNv = model.HoTenNv;
            nv.EmailNv = model.EmailNv;
            nv.DiaChiNv = model.DiaChiNv;
            nv.DienThoaiNv = model.DienThoaiNv;
            nv.GioiTinhNv = model.GioiTinhNv;
            nv.NgaySinhNv = model.NgaySinhNv;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }
        #endregion
    }
}
