<div align="center">

# 🖊️ VanPhongPham Online

**A full-stack e-commerce web app for stationery & office supplies**

Built with ASP.NET Core MVC · Entity Framework Core · SQL Server

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-8.0-6DB33F)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-4-7952B3?logo=bootstrap&logoColor=white)
![PayPal](https://img.shields.io/badge/PayPal-Sandbox-00457C?logo=paypal&logoColor=white)

*Software Engineering course project, graded **9/10***

</div>

---

## 📑 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [Demo Accounts](#-demo-accounts)
- [Project Structure](#-project-structure)
- [Security Notes](#-security-notes)
- [Roadmap](#-roadmap)
- [Credits](#-credits)

---

## 🔎 Overview

**VanPhongPham Online** (codename *MultiShop*) is an online store for stationery and office supplies.
It is made of two parts:

| Area | Who uses it | What they can do |
|---|---|---|
| 🛒 **Storefront** | Customers | Browse and search products, manage a cart, check out (Cash on Delivery or PayPal), manage their account and order history |
| 🛠️ **Back office** | Staff & Admins | Manage products, customers, employees, orders, departments and staff assignments |

---

## ✨ Features

### 🛒 Customer storefront
- **Authentication**: register (with avatar upload), log in and log out with cookie authentication.
- **Password recovery**: a one-time password (OTP) is emailed via SMTP and expires after 5 minutes. A password can only be reset after the OTP has been verified.
- **Product catalog**: filter by category, paginate, search by name, view product details.
- **Shopping cart**: kept in the session; add, update the quantity and remove items.
- **Checkout**:
  - Cash on Delivery (COD)
  - **PayPal Sandbox**, with automatic VND → USD conversion and server-side capture verification
- **Account**: edit profile, view paginated order history, delete account (soft delete).
- **Contact form**: send feedback that is stored in the database.

### 🛠️ Back office
- **Separate staff login**, using its own cookie scheme so staff sessions are independent of customer sessions.
- **Role-based access control**:

  | Capability | Admin | Staff |
  |---|:---:|:---:|
  | Manage products (image upload, category and supplier) | ✅ | ❌ |
  | Manage customers | ✅ | ❌ |
  | Manage employees | ✅ | ❌ |
  | Manage departments & assignments | ✅ | ❌ |
  | View and confirm orders | ✅ | ✅ |
  | Edit own profile | ✅ | ✅ |

- Search for products, customers, employees and orders.
- Toast notifications for CRUD actions.

---

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| **Backend** | ASP.NET Core MVC 8, C# |
| **Data access** | Entity Framework Core 8 (database-first) |
| **Database** | Microsoft SQL Server / LocalDB |
| **Auth** | Cookie authentication (two schemes) with role-based authorization policies |
| **Payments** | PayPal REST API v2 (Sandbox) |
| **Email** | SMTP via `System.Net.Mail` (Gmail) |
| **Mapping** | AutoMapper |
| **Frontend** | Razor Views, View Components, Bootstrap 4, jQuery |
| **UI template** | [MultiShop](https://htmlcodex.com/online-shop-website-template) by HTML Codex |

---

## 🏗️ Architecture

```
            ┌──────────────────────────── ASP.NET Core MVC ───────────────────────────┐
 Browser ──►│  Razor Views / View Components                                           │
            │          ▲                                                               │
            │          │                                                               │
            │  Controllers ──────────────► Helpers (OTP, Mail, PayPal client, Upload)   │──► PayPal API
            │   ├─ Storefront (Cart, HangHoa, KhachHang, Contact)                       │──► SMTP (Gmail)
            │   └─ Admin (DangNhap, NhanVien, QuanLyHangHoa, HoaDon, ...)               │
            │          │                                                               │
            │          ▼                                                               │
            │  MultiShopContext (EF Core)                                              │
            └──────────┼────────────────────────────────────────────────────────────────┘
                       ▼
                 SQL Server (MultiShop)
```

**Authentication flow**

- `KhachHang` cookie scheme: customers, the default scheme.
- `AdminCookie` cookie scheme: staff, carrying an `Admin` or `NhanVien` role claim.
- Policies `AdminOnly` / `StaffOnly` guard every back-office controller.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or **SQL Server Express LocalDB** (bundled with Visual Studio)
- Visual Studio 2022 (recommended), VS Code or JetBrains Rider

### 1. Clone the repository

```bash
git clone https://github.com/TranHoangDatSC/VanPhongPhamOnline.git
cd VanPhongPhamOnline
```

### 2. Create the database

Run the scripts in order, using SSMS or `sqlcmd`:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -i database/01_create_schema.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -i database/02_seed_data.sql
```

The seed script creates sample categories, suppliers and products, the order statuses, and a default admin account.

> Not using LocalDB? Update `ConnectionStrings:MultiShop` in `appsettings.json`, or override it in `appsettings.Local.json`.

### 3. Configure secrets (optional)

Secrets are **never committed**. Copy the example file and fill in your own credentials:

```bash
cd VanPhongPhamOnline
cp appsettings.Local.example.json appsettings.Local.json
```

```json
{
  "PaypalOptions": {
    "AppId": "<PAYPAL_SANDBOX_CLIENT_ID>",
    "AppSecret": "<PAYPAL_SANDBOX_SECRET>"
  },
  "EmailSettings": {
    "Mail": "<your-email@gmail.com>",
    "Password": "<GMAIL_APP_PASSWORD>"
  }
}
```

`appsettings.Local.json` is git-ignored and loaded automatically at startup.

> The app runs without it. Only PayPal checkout and OTP emails are disabled.

### 4. Run

```bash
dotnet run --project VanPhongPhamOnline
```

Or open `VanPhongPhamOnline.sln` in Visual Studio and press **F5**.

---

## 👤 Demo Accounts

| Role | Login page | Email | Password |
|---|---|---|---|
| Admin | `/DangNhap` | `admin@gmail.com` | `123456` |
| Customer | `/KhachHang/DangKy` | *register a new account* | |

---

## 📁 Project Structure

```
VanPhongPhamOnline/
├── VanPhongPhamOnline.sln
├── database/
│   ├── 01_create_schema.sql          # Database & tables
│   └── 02_seed_data.sql              # Sample data + admin account
└── VanPhongPhamOnline/
    ├── Controllers/                  # Storefront controllers
    │   └── Admin/                    # Back-office controllers
    ├── Data/                         # EF Core entities + DbContext
    ├── Helpers/                      # Auth constants, OTP, mail, PayPal client, hashing, session
    ├── ViewComponents/               # Mini cart, category menu
    ├── ViewModels/                   # View-specific models
    ├── Views/
    │   ├── Admin/                    # Back-office pages
    │   └── ...                       # Storefront pages
    ├── wwwroot/                      # Static assets & product images
    ├── appsettings.json              # Shared config (no secrets)
    ├── appsettings.Local.example.json
    └── Program.cs                    # DI, auth schemes, policies, middleware
```

---

## 🔐 Security Notes

- **CSRF protection** on every form POST (`ValidateAntiForgeryToken`).
- **Role-based authorization policies** on all back-office endpoints.
- **OTP-gated password reset**: the reset endpoint rejects requests whose OTP has not been verified.
- **Upload whitelist**: only `.jpg`, `.jpeg`, `.png`, `.gif` and `.webp` files are accepted.
- **No secrets in source control**: credentials live in a git-ignored local config file.
- ⚠️ Passwords use **salted MD5** to stay compatible with existing data. This is a known limitation (see the roadmap).

---

## 🗺️ Roadmap

- [ ] Migrate password hashing to ASP.NET Core `PasswordHasher` (PBKDF2) or BCrypt
- [ ] Add unit and integration tests (xUnit)
- [ ] Track stock levels and product ratings (currently placeholders)
- [ ] Add a VNPay payment gateway
- [ ] Containerize with Docker and add CI (GitHub Actions)

---

## 🙏 Credits

- UI template: [MultiShop](https://htmlcodex.com/online-shop-website-template) by **HTML Codex** (CC BY 4.0)
- Developed as a team project for the *Introduction to Software Engineering* course

<div align="center">

⭐ If you find this project useful, consider giving it a star!

</div>
