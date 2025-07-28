using System.Net;
using System.Net.Mail;
using System.Text;

namespace VanPhongPhamOnline.Helpers
{
    public class MyUlti
    {
        public static string UploadHinh(IFormFile Hinh, string folder, string prefix = "")
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = Path.GetExtension(Hinh.FileName);
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
            catch (Exception ex)
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
        public static void SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.From = new MailAddress("your-email@gmail.com", "MultiShop");
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("your-email@gmail.com", "app-password-16-ký-tự"),
                EnableSsl = true
            };

            smtp.Send(message);
        }
    }
}
