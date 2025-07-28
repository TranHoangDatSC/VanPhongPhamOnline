using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers
{
    public class ContactController : Controller
    {
        private readonly MultiShopContext db;

        public ContactController(MultiShopContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index(ContactVM model)
        {
            if (ModelState.IsValid)
            {
                var gopY = new GopY
                {
                    MaGy = Guid.NewGuid().ToString(),
                    NoiDung = model.Subject + " - " + model.Message,
                    NgayGy = DateOnly.FromDateTime(DateTime.Now),
                    DienThoai = "",
                    CanTraLoi = false,
                    TraLoi = null,
                    NgayTl = null
                };

                db.Gopies.Add(gopY);
                db.SaveChanges();

                TempData["Success"] = "Gửi liên hệ thành công!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // (Optional) Admin xem danh sách góp ý
        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public IActionResult DanhSach()
        //{
        //    var list = db.Gopies
        //                .Where(x => x.MaGy == MaGy)
        //                .OrderByDescending(x => x.NgayGy)
        //                .ToList();
        //    return View(list);
        //}

    }
}
