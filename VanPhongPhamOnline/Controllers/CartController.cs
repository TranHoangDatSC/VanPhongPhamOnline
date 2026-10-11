using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;
using VanPhongPhamOnline.ViewModels;

namespace VanPhongPhamOnline.Controllers
{
    public class CartController : Controller
    {
        private const double PhiVanChuyen = 10000;

        private readonly MultiShopContext db;
        private readonly PaypalClient _paypalClient;
        private readonly IConfiguration _configuration;

        public CartController(MultiShopContext context, PaypalClient paypalClient, IConfiguration configuration)
        {
            _paypalClient = paypalClient;
            _configuration = configuration;
            db = context;
        }

        private List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.Set(MySetting.CART_KEY, cart);
        }

        [Authorize(AuthenticationSchemes = AppAuth.CustomerScheme)]
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            if (quantity <= 0) quantity = 1;

            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return RedirectToAction("PageNotFound", "Home");
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
            SaveCart(gioHang);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(int id, int change)
        {
            var cart = Cart;
            var item = cart.FirstOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                item.SoLuong += change;
                if (item.SoLuong <= 0) cart.Remove(item);
            }
            SaveCart(cart); // cập nhật session
            return RedirectToAction("Index"); // refresh lại trang giỏ hàng
        }

        public IActionResult RemoveToCart(int id)
        {
            var gioHang = Cart;

            var item = gioHang.SingleOrDefault(p => p.MaHH == id);
            if (item != null)
            {
                gioHang.Remove(item);
                SaveCart(gioHang);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);

            if (item != null)
            {
                if (quantity <= 0)
                    gioHang.Remove(item);
                else
                    item.SoLuong = quantity;

                SaveCart(gioHang);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = AppAuth.CustomerScheme)]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(Cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = AppAuth.CustomerScheme)]
        public IActionResult Checkout(CheckoutVM model)
        {
            var cart = Cart;
            if (cart.Count == 0)
            {
                return Redirect("/");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.PaypalClientId = _paypalClient.ClientId;
                return View(cart);
            }

            var hoaDon = TaoHoaDon(model, cart, "COD");
            if (hoaDon == null)
            {
                return Content("Không xác định được khách hàng.");
            }

            return RedirectToAction("Success", "Cart");
        }

        public IActionResult Success()
        {
            return View(); // /Views/Cart/Success.cshtml
        }

        /// <summary>
        /// Lưu hóa đơn + chi tiết hóa đơn từ giỏ hàng, sau đó xóa giỏ hàng.
        /// </summary>
        private HoaDon? TaoHoaDon(CheckoutVM model, List<CartItem> cart, string cachThanhToan)
        {
            var maKH = User.FindFirst("MaKH")?.Value;
            if (string.IsNullOrEmpty(maKH))
            {
                return null;
            }

            // Nếu tick "Giống thông tin khách hàng" hoặc bỏ trống thì lấy thông tin từ tài khoản
            var dungThongTinKhachHang = model.GiongKhachHang || string.IsNullOrWhiteSpace(model.HoTen);
            var hoTen = dungThongTinKhachHang ? User.Identity?.Name : model.HoTen;
            var diaChi = dungThongTinKhachHang || string.IsNullOrWhiteSpace(model.DiaChi)
                ? User.FindFirst("DiaChi")?.Value
                : model.DiaChi;

            var hoaDon = new HoaDon
            {
                MaKh = maKH,
                NgayDat = DateTime.Now,
                NgayGiao = DateTime.Now.AddDays(1),
                DiaChi = string.IsNullOrWhiteSpace(diaChi) ? "Chưa cập nhật" : diaChi,
                CachThanhToan = cachThanhToan,
                CachVanChuyen = "GRAB",
                PhiVanChuyen = PhiVanChuyen,
                MaTrangThai = 1,
                HoTenNguoiNhan = hoTen,
                GhiChu = model.GhiChu,
                MaNv = null
            };

            foreach (var item in cart)
            {
                hoaDon.ChiTietHds.Add(new ChiTietHd
                {
                    MaHh = item.MaHH,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                    GiamGia = 0
                });
            }

            db.HoaDons.Add(hoaDon);
            db.SaveChanges();

            HttpContext.Session.Remove(MySetting.CART_KEY);
            return hoaDon;
        }

        #region Paypal payment
        [Authorize(AuthenticationSchemes = AppAuth.CustomerScheme)]
        [IgnoreAntiforgeryToken]
        [HttpPost("Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            var cart = Cart;
            if (cart.Count == 0)
            {
                return BadRequest(new { Message = "Giỏ hàng trống." });
            }

            // Giá trong hệ thống là VND, PayPal không hỗ trợ VND nên quy đổi sang USD
            var tyGia = _configuration.GetValue<double?>("PaypalOptions:VndPerUsd") ?? 25000;
            var tongTienVnd = cart.Sum(p => p.ThanhTien) + PhiVanChuyen;
            var tongTien = Math.Round(tongTienVnd / tyGia, 2).ToString("F2", CultureInfo.InvariantCulture);
            var donvitiente = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();
            try
            {
                var response = await _paypalClient.CreateOrder(tongTien, donvitiente, maDonHangThamChieu);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Authorize(AuthenticationSchemes = AppAuth.CustomerScheme)]
        [IgnoreAntiforgeryToken]
        [HttpPost("Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken, CheckoutVM model)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);
                if (!string.Equals(response.status, "COMPLETED", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { Message = "Thanh toán PayPal chưa hoàn tất." });
                }

                // Lưu hóa đơn sau khi PayPal thanh toán thành công
                var hoaDon = TaoHoaDon(model, Cart, "PAYPAL");
                if (hoaDon == null)
                {
                    return BadRequest(new { Message = "Không xác định được khách hàng." });
                }

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
