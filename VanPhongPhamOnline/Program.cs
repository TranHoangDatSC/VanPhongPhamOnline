using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VanPhongPhamOnline.Data;
using VanPhongPhamOnline.Helpers;

var builder = WebApplication.CreateBuilder(args);

//
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<MyUlti>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<MultiShopContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MultiShop"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "KhachHang";
    options.DefaultChallengeScheme = "KhachHang";
})
.AddCookie("KhachHang", options =>
 {
     options.LoginPath = "/KhachHang/DangNhap";
     options.AccessDeniedPath = "/KhachHang/AccessDenied";
     options.Cookie.Name = "KhachHangCookie";
 })
.AddCookie("AdminCookie", options =>
{
    options.LoginPath = "/Admin/DangNhap/Index";
    options.AccessDeniedPath = "/Admin/DangNhap/AccessDenied";
    options.Cookie.Name = "AdminCookie";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
