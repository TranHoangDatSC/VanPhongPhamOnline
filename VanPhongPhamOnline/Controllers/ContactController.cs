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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactVM model)
        {
            if (ModelState.IsValid)
            {
                var gopY = new GopY
                {
                    MaGy = Guid.NewGuid().ToString(),
                    MaKh = User.FindFirst("MaKH")?.Value,
                    HoTenKh = model.Name,
                    EmailKh = model.Email,
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
    }
}
