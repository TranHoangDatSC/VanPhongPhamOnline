using Microsoft.AspNetCore.Mvc;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly MultiShopContext db;
        public MenuLoaiViewComponent(MultiShopContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);

            return View(data); /** Default.cshtml */
            /** return View("Default",data); */
        }
    }
}
