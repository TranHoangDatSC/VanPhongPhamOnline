using ECommerceMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers
{
    public class CartController : Controller
    {
        private readonly MultiShopContext db;
        private readonly PaypalClient _paypalClient;
        public CartController(MultiShopContext context, PaypalClient paypalClient)
        {
            _paypalClient = paypalClient;
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = "KhachHang")]
        public IActionResult Checkout()
        {
            if(Cart.Count == 0)
            {
                return Redirect("/");
            }
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(Cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var maKH = User.FindFirst("MaKH")?.Value;
            if (string.IsNullOrEmpty(maKH))
            {
                return Content("Không xác định được khách hàng.");
            }

            var hoaDon = new HoaDon
            {
                MaKh = maKH,
                NgayDat = DateTime.Now,
                NgayGiao = DateTime.Now.AddDays(1),
                DiaChi = "Sài Gòn",
                CachThanhToan = "PAYPAL",
                CachVanChuyen = "GRAB",
                PhiVanChuyen = 10000,
                MaTrangThai = 1,
                HoTenNguoiNhan = model.HoTen,
                GhiChu = model.GhiChu,
                MaNv = null
            };

            db.HoaDons.Add(hoaDon);
            db.SaveChanges();

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                var cthds = new List<ChiTietHd>();
                foreach (var item in cart)
                {
                    cthds.Add(new ChiTietHd
                    {
                        MaHd = hoaDon.MaHd,
                        MaHh = item.MaHH,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia,
                        GiamGia = 0
                    });
                }

                db.ChiTietHds.AddRange(cthds);
                db.SaveChanges(); 

                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("Success", "Cart");
        }
        public IActionResult Success()
        {
            return View(); // /Views/Cart/Success.cshtml
        }

        #region Paypal payment
        [Authorize]
        [HttpPost("Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            // Thong tin don hang gui qua Paypal
            var tongTien = Cart.Sum(p => p.ThanhTien).ToString();
            var donvitiente = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();
            try
            {
                var response = await _paypalClient.CreateOrder(tongTien, donvitiente, maDonHangThamChieu);
                return Ok(response);
            } 
            catch(Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Authorize]
        [HttpPost("Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID,CancellationToken cancellationToken, CheckoutVM model)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                // ==== Lưu hóa đơn sau khi PayPal thanh toán thành công ====
                var maKH = User.FindFirst("MaKH")?.Value;
                if (string.IsNullOrEmpty(maKH))
                {
                    return BadRequest(new { message = "Không xác định được khách hàng." });
                }

                var hoaDon = new HoaDon
                {
                    MaKh = maKH,
                    NgayDat = DateTime.Now,
                    NgayGiao = DateTime.Now.AddDays(1),
                    DiaChi = "Sài Gòn",
                    CachThanhToan = "PAYPAL",
                    CachVanChuyen = "GRAB",
                    PhiVanChuyen = 10000,
                    MaTrangThai = 1,
                    HoTenNguoiNhan = model.HoTen, // hoặc lấy từ session/form
                    GhiChu = model.GhiChu,
                    MaNv = null
                };

                db.HoaDons.Add(hoaDon);
                db.SaveChanges();

                var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY);
                if (cart != null)
                {
                    var cthds = cart.Select(item => new ChiTietHd
                    {
                        MaHd = hoaDon.MaHd,
                        MaHh = item.MaHH,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia,
                        GiamGia = 0
                    }).ToList();

                    db.ChiTietHds.AddRange(cthds);
                    db.SaveChanges();

                    HttpContext.Session.Remove(MySetting.CART_KEY);
                }
                // ==========================================================

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        #endregion
    }
}
