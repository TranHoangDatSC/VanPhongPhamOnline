using Microsoft.AspNetCore.Mvc;

namespace VanPhongPhamOnline.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult IndexAdmin()
        {
            return View("~/Views/Admin/IndexAdmin.cshtml");
        }
    }
}
