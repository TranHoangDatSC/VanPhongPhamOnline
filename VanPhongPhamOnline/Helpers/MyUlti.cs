using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace VanPhongPhamOnline.Helpers
{
    public class MyUlti
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        /// <summary>
        /// Lưu hình vào wwwroot/Hinh/{folder}. Trả về tên file, hoặc chuỗi rỗng nếu file không hợp lệ.
        /// </summary>
        public static string UploadHinh(IFormFile Hinh, string folder, string prefix = "")
        {
            try
            {
                var extension = Path.GetExtension(Hinh.FileName).ToLowerInvariant();
                if (!AllowedImageExtensions.Contains(extension))
                {
                    return string.Empty;
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var randomPart = Guid.NewGuid().ToString().Substring(0, 8);

                var fileName = string.IsNullOrEmpty(prefix)
                    ? $"{randomPart}{extension}"
                    : $"{prefix}_{randomPart}{extension}";

                var fullPath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    Hinh.CopyTo(stream);
                }

                return fileName;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }

        public static string GenerateRandomKey(int length = 5)
        {
            var pattern = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!@#$%^&*()";
            var sb = new StringBuilder();
            var rd = new Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }

        private readonly EmailSettings _emailSettings;
        private readonly ILogger<MyUlti> _logger;

        public MyUlti(IOptions<EmailSettings> options, ILogger<MyUlti> logger)
        {
            _emailSettings = options.Value;
            _logger = logger;
        }

        public void SendMail(string to, string subject, string body)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.Mail, _emailSettings.DisplayName);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(_emailSettings.Mail, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSsl
                };

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gửi email tới {To} thất bại", to);
            }
        }
    }
}
