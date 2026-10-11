namespace VanPhongPhamOnline.Helpers
{
    /// <summary>
    /// Hằng số dùng chung cho xác thực / phân quyền.
    /// </summary>
    public static class AppAuth
    {
        // Cookie scheme
        public const string CustomerScheme = "KhachHang";
        public const string StaffScheme = "AdminCookie";

        // Role
        public const string AdminRole = "Admin";
        public const string StaffRole = "NhanVien";

        // Policy
        public const string AdminPolicy = "AdminOnly";
        public const string StaffPolicy = "StaffOnly";

        // Mã nhân viên quản trị mặc định
        public const string AdminId = "AD001";
        public const string AdminDepartment = "ADMIN";
    }
}
