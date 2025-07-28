using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VanPhongPhamOnline.Data;

public partial class MultiShopContext : DbContext
{
    public MultiShopContext()
    {
    }

    public MultiShopContext(DbContextOptions<MultiShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHd> ChiTietHds { get; set; }

    public virtual DbSet<GopY> Gopies { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhanCong> PhanCongs { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<PhongBan> PhongBans { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<TrangWeb> TrangWebs { get; set; }

    public virtual DbSet<VChiTietHoaDon> VChiTietHoaDons { get; set; }

    public virtual DbSet<YeuThich> YeuThiches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MultiShop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietHd>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK__ChiTietH__27258E7414067DB9");

            entity.ToTable("ChiTietHD");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaHD__3D5E1FD2");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHD__MaHH__3E52440B");
        });

        modelBuilder.Entity<GopY>(entity =>
        {
            entity.HasKey(e => e.MaGy).HasName("PK__GopY__2725AEF4EB4D00B1");

            entity.ToTable("GopY");

            entity.Property(e => e.MaGy)
                .HasMaxLength(50)
                .HasColumnName("MaGY");
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.EmailKh)
                .HasMaxLength(50)
                .HasColumnName("EmailKH");
            entity.Property(e => e.HoTenKh)
                .HasMaxLength(50)
                .HasColumnName("HoTenKH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayGy).HasColumnName("NgayGY");
            entity.Property(e => e.NgayTl).HasColumnName("NgayTL");
            entity.Property(e => e.TraLoi).HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Gopies)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__GopY__MaKH__45F365D3");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Gopies)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__GopY__MaNV__44FF419A");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK__HangHoa__2725A6E4F5D52C64");

            entity.ToTable("HangHoa");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.MaNcc)
                .HasMaxLength(50)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MoTaDonVi).HasMaxLength(50);
            entity.Property(e => e.NgaySx)
                .HasColumnType("datetime")
                .HasColumnName("NgaySX");
            entity.Property(e => e.TenAlias).HasMaxLength(50);
            entity.Property(e => e.TenHh)
                .HasMaxLength(50)
                .HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HangHoa__MaLoai__398D8EEE");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaNcc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HangHoa__MaNCC__3A81B327");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HoaDon__2725A6E0A2BD4F6A");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.CachThanhToan).HasMaxLength(50);
            entity.Property(e => e.CachVanChuyen).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.GhiChu).HasMaxLength(50);
            entity.Property(e => e.HoTenNguoiNhan).HasMaxLength(50);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayCan).HasColumnType("datetime");
            entity.Property(e => e.NgayDat).HasColumnType("datetime");
            entity.Property(e => e.NgayGiao).HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaKH__34C8D9D1");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__HoaDon__MaNV__36B12243");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaTrangT__35BCFE0A");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E9DDFA215");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChiKh)
                .HasMaxLength(60)
                .HasColumnName("DiaChiKH");
            entity.Property(e => e.DienThoaiKh)
                .HasMaxLength(24)
                .HasColumnName("DienThoaiKH");
            entity.Property(e => e.EmailKh)
                .HasMaxLength(50)
                .HasColumnName("EmailKH");
            entity.Property(e => e.GioiTinhKh).HasColumnName("GioiTinhKH");
            entity.Property(e => e.HinhKh)
                .HasMaxLength(50)
                .HasColumnName("HinhKH");
            entity.Property(e => e.HoTenKh)
                .HasMaxLength(50)
                .HasColumnName("HoTenKH");
            entity.Property(e => e.MatKhauKh)
                .HasMaxLength(50)
                .HasColumnName("MatKhauKH");
            entity.Property(e => e.NgaySinhKh)
                .HasColumnType("datetime")
                .HasColumnName("NgaySinhKH");
            entity.Property(e => e.RandomKeyKh)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](50),newid()))")
                .HasColumnName("RandomKeyKH");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__Loai__730A57592B2928F7");

            entity.ToTable("Loai");

            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
            entity.Property(e => e.TenLoaiAlias).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEBADC344CE");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc)
                .HasMaxLength(50)
                .HasColumnName("MaNCC");
            entity.Property(e => e.DiaChiNcc)
                .HasMaxLength(50)
                .HasColumnName("DiaChiNCC");
            entity.Property(e => e.DienThoaiNcc)
                .HasMaxLength(50)
                .HasColumnName("DienThoaiNCC");
            entity.Property(e => e.EmailNcc)
                .HasMaxLength(50)
                .HasColumnName("EmailNCC");
            entity.Property(e => e.Logo).HasMaxLength(50);
            entity.Property(e => e.MoTaNcc).HasColumnName("MoTaNCC");
            entity.Property(e => e.NguoiLienLac).HasMaxLength(50);
            entity.Property(e => e.TenCongTy).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A468350DF");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.DiaChiNv)
                .HasMaxLength(100)
                .HasColumnName("DiaChiNV");
            entity.Property(e => e.EmailNv)
                .HasMaxLength(50)
                .HasColumnName("EmailNV");
            entity.Property(e => e.GioiTinhNv).HasColumnName("GioiTinhNV");
            entity.Property(e => e.HoTenNv)
                .HasMaxLength(50)
                .HasColumnName("HoTenNV");
            entity.Property(e => e.MatKhauNv)
                .HasMaxLength(50)
                .HasColumnName("MatKhauNV");
            entity.Property(e => e.NgaySinhNv).HasColumnName("NgaySinhNV");
            entity.Property(e => e.RandomKeyNv)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](50),newid()))")
                .HasColumnName("RandomKeyNV");
        });

        modelBuilder.Entity<PhanCong>(entity =>
        {
            entity.HasKey(e => e.MaPc).HasName("PK__PhanCong__2725E7E5F8AACE32");

            entity.ToTable("PhanCong");

            entity.Property(e => e.MaPc).HasColumnName("MaPC");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.MaPb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MaPB");
            entity.Property(e => e.NgayPc)
                .HasColumnType("datetime")
                .HasColumnName("NgayPC");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.PhanCongs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanCong__MaNV__48CFD27E");

            entity.HasOne(d => d.MaPbNavigation).WithMany(p => p.PhanCongs)
                .HasForeignKey(d => d.MaPb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanCong__MaPB__49C3F6B7");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaPq).HasName("PK__PhanQuye__2725E7F3010EA77B");

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.MaPq).HasColumnName("MaPQ");
            entity.Property(e => e.MaPb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MaPB");

            entity.HasOne(d => d.MaPbNavigation).WithMany(p => p.PhanQuyens)
                .HasForeignKey(d => d.MaPb)
                .HasConstraintName("FK__PhanQuyen__MaPB__4CA06362");

            entity.HasOne(d => d.MaTrangNavigation).WithMany(p => p.PhanQuyens)
                .HasForeignKey(d => d.MaTrang)
                .HasConstraintName("FK__PhanQuyen__MaTra__4D94879B");
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.MaPb).HasName("PK__PhongBan__2725E7E4DB6C21C6");

            entity.ToTable("PhongBan");

            entity.Property(e => e.MaPb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MaPB");
            entity.Property(e => e.TenPb)
                .HasMaxLength(50)
                .HasColumnName("TenPB");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("PK__TrangTha__AADE4138F768ED66");

            entity.ToTable("TrangThai");

            entity.Property(e => e.MaTrangThai).ValueGeneratedNever();
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenTrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.TrangThais)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__TrangThai__MaNV__2B3F6F97");
        });

        modelBuilder.Entity<TrangWeb>(entity =>
        {
            entity.HasKey(e => e.MaTrang).HasName("PK__TrangWeb__399828AF68F482C2");

            entity.ToTable("TrangWeb");

            entity.Property(e => e.TenTrang).HasMaxLength(50);
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .HasColumnName("URL");
        });

        modelBuilder.Entity<VChiTietHoaDon>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vChiTietHoaDon");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.TenHh)
                .HasMaxLength(50)
                .HasColumnName("TenHH");
        });

        modelBuilder.Entity<YeuThich>(entity =>
        {
            entity.HasKey(e => e.MaYt).HasName("PK__YeuThich__272330D4AF3E1298");

            entity.ToTable("YeuThich");

            entity.Property(e => e.MaYt).HasColumnName("MaYT");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayChon).HasColumnType("datetime");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK__YeuThich__MaHH__412EB0B6");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__YeuThich__MaKH__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
