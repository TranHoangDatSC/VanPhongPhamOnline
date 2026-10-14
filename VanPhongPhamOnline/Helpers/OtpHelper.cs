using System.Globalization;
using System.Security.Cryptography;

namespace VanPhongPhamOnline.Helpers
{
    /// <summary>
    /// Quản lý mã OTP đặt lại mật khẩu (lưu trong Session).
    /// Luồng: Tạo OTP -> Xác nhận OTP -> Đặt lại mật khẩu.
    /// </summary>
    public static class OtpHelper
    {
        private const string OtpKey = "OTP";
        private const string EmailKey = "OTP_Email";
        private const string ExpireKey = "OTP_Expire";
        private const string VerifiedKey = "OTP_Verified";

        public const int ExpireMinutes = 5;

        public static string Generate(ISession session, string email)
        {
            var otp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            session.SetString(OtpKey, otp);
            session.SetString(EmailKey, email);
            session.SetString(ExpireKey, DateTime.UtcNow.AddMinutes(ExpireMinutes).ToString("O"));
            session.Remove(VerifiedKey);

            return otp;
        }

        /// <summary>
        /// Kiểm tra mã OTP. Trả về null nếu hợp lệ, ngược lại trả về thông báo lỗi.
        /// </summary>
        public static string? Verify(ISession session, string email, string code)
        {
            var otp = session.GetString(OtpKey);
            var otpEmail = session.GetString(EmailKey);
            var otpExpireStr = session.GetString(ExpireKey);

            if (otp == null || otpEmail == null || otpExpireStr == null)
                return "Mã xác nhận đã hết hạn hoặc không hợp lệ.";

            if (!string.Equals(otpEmail, email, StringComparison.OrdinalIgnoreCase))
                return "Email không khớp với mã xác nhận.";

            if (!DateTime.TryParse(otpExpireStr, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var otpExpire)
                || DateTime.UtcNow > otpExpire)
                return "Mã xác nhận đã hết hạn.";

            if (otp != code?.Trim())
                return "Mã xác nhận không chính xác.";

            session.SetString(VerifiedKey, email);
            return null;
        }

        /// <summary>
        /// Email đã xác nhận OTP thành công và được phép đặt lại mật khẩu hay chưa.
        /// </summary>
        public static bool IsVerified(ISession session, string email)
        {
            var verifiedEmail = session.GetString(VerifiedKey);
            return verifiedEmail != null && string.Equals(verifiedEmail, email, StringComparison.OrdinalIgnoreCase);
        }

        public static void Clear(ISession session)
        {
            session.Remove(OtpKey);
            session.Remove(EmailKey);
            session.Remove(ExpireKey);
            session.Remove(VerifiedKey);
        }
    }
}
