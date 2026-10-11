using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình cục bộ (secret, connection string riêng) - không commit lên git
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<MyUlti>();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // Cho phép controller quản trị tìm view trong Views/Admin/{Controller}/
        options.ViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");
    });
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddDbContext<MultiShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MultiShop"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = AppAuth.CustomerScheme;
    options.DefaultChallengeScheme = AppAuth.CustomerScheme;
})
.AddCookie(AppAuth.CustomerScheme, options =>
{
    options.LoginPath = "/KhachHang/DangNhap";
    options.AccessDeniedPath = "/KhachHang/AccessDenied";
    options.Cookie.Name = "KhachHangCookie";
})
.AddCookie(AppAuth.StaffScheme, options =>
{
    options.LoginPath = "/DangNhap/Index";
    options.AccessDeniedPath = "/DangNhap/AccessDenied";
    options.Cookie.Name = "AdminCookie";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppAuth.StaffPolicy, policy => policy
        .AddAuthenticationSchemes(AppAuth.StaffScheme)
        .RequireAuthenticatedUser());

    options.AddPolicy(AppAuth.AdminPolicy, policy => policy
        .AddAuthenticationSchemes(AppAuth.StaffScheme)
        .RequireRole(AppAuth.AdminRole));
});

// Register Paypal type Singleton() - just have one instance
builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOptions:AppId"] ?? string.Empty,
    builder.Configuration["PaypalOptions:AppSecret"] ?? string.Empty,
    builder.Configuration["PaypalOptions:Mode"] ?? "sandbox"
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/PageNotFound");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
