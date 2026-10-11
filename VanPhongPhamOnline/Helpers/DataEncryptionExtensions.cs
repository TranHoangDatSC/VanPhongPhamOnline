using System.Security.Cryptography;
using System.Text;

namespace VanPhongPhamOnline.Helpers
{
    public static class DataEncryptionExtensions
    {
        #region [Hashing Extension]
        /// <summary>
        /// Băm mật khẩu bằng MD5 + salt (RandomKey).
        /// Giữ MD5 để tương thích với dữ liệu mật khẩu đã có trong CSDL.
        /// </summary>
        public static string ToMd5Hash(this string password, string? saltKey)
        {
            using var md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(password, saltKey)));
            var sBuilder = new StringBuilder();
            foreach (var b in data)
            {
                sBuilder.Append(b.ToString("x2"));
            }

            return sBuilder.ToString();
        }
        #endregion
    }
}
