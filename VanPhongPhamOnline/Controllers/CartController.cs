using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers
{
    public class CartController : Controller
    {
        private readonly MultiShopContext db;
        public CartController(MultiShopContext context)
        {
            db = context;
        }

        private List<CartItem> Cart => GetToCart();
        private List<CartItem> GetToCart()
        {
            var jsonCart = HttpContext.Session.GetString(MySetting.CART_KEY);
            if (string.IsNullOrEmpty(jsonCart))
                return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(jsonCart);
        }
        private void SaveToCart(List<CartItem> cart)
        {
            var jsonCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(MySetting.CART_KEY, jsonCart);
        }
        [Authorize(AuthenticationSchemes = "KhachHang")]
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Khong tim thay hang hoa co ma {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHH = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateCart(int id, int change)
        {
            var cart = GetToCart();
            var item = cart.FirstOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                item.SoLuong += change;
                if (item.SoLuong <= 0) cart.Remove(item);
            }
            SaveToCart(cart); // cập nhật session
            return RedirectToAction("Index"); // refresh lại trang giỏ hàng
        }

        public IActionResult RemoveToCart(int id)
        {
            var gioHang = Cart;

            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var gioHang = GetToCart();
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);

            if (item != null)
            {
                if (quantity <= 0)
                    gioHang.Remove(item);
                else
                    item.SoLuong = quantity;

                SaveToCart(gioHang);
            }

            return RedirectToAction("Index");
        }
        public IActionResult ClearCart()
        {
            SaveToCart(new List<CartItem>());
            return RedirectToAction("Index");
        }
    }
}
