using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanPhongPhamOnline.Helpers;

namespace VanPhongPhamOnline.Controllers.Admin
{
    [Authorize(Policy = AppAuth.StaffPolicy)]
    public class AdminController : Controller
    {
        [Authorize(Policy = AppAuth.AdminPolicy)]
        public IActionResult IndexAdmin()
        {
            return View("~/Views/Admin/IndexAdmin.cshtml");
        }

        public IActionResult IndexNhanVien()
        {
            return View("~/Views/Admin/IndexNhanVien.cshtml");
        }
    }
}
