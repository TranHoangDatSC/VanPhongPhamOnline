using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly MultiShopContext db;
        public HangHoaController(MultiShopContext context)
        {
            db = context;
        }
        public IActionResult Index(int? loai, int page = 1)
        {
            LoadMenuLoai();
            const int pageSize = 12;
            var hangHoas = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }
            var totalItems = hangHoas.Count();
            var result = hangHoas
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new HangHoaVM
            {
                MaHH = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }).ToList();

            /* Đưa thông tin phân trang vào ViewBag */
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.Loai = loai;

            //var result = hangHoas.Select(p => new HangHoaVM
            //{
            //    MaHH = p.MaHh,
            //    TenHH = p.TenHh,
            //    DonGia = p.DonGia ?? 0,
            //    Hinh = p.Hinh ?? "",
            //    MoTaNgan = p.MoTaDonVi ?? "",
            //    TenLoai = p.MaLoaiNavigation.TenLoai
            //});

            return View(result);
        }
        public IActionResult Search(string? query)
        {
            LoadMenuLoai();

            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHH = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }
        
        public IActionResult Detail(int id)
        {
            LoadMenuLoai();

            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không thể tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }

            var result = new HangHoaVM
            {
                MaHH = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10, // Check again
                DiemDanhGia = 5, // Check again
            };
            return View(result);
        }
        private void LoadMenuLoai()
        {
            var data = db.Loais
                .Select(loai => new MenuLoaiVM
                {
                    MaLoai = loai.MaLoai,
                    TenLoai = loai.TenLoai,
                    SoLuong = loai.HangHoas.Count()
                }).ToList();

            ViewBag.MenuLoai = data;
        }
    }
}
