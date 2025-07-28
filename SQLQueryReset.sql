USE [MultiShop]
GO
-- Bảng phụ (dùng khóa ngoại)
DELETE FROM ChiTietHD;
DELETE FROM YeuThich;
DELETE FROM BanBe;
DELETE FROM GopY;
DELETE FROM HoiDap;
DELETE FROM PhanCong;
DELETE FROM PhanQuyen;
DELETE FROM ChuDe;

-- Bảng trung gian
DELETE FROM HoaDon;
DELETE FROM HangHoa;

-- Bảng gốc
DELETE FROM KhachHang;
DELETE FROM NhanVien;
DELETE FROM Loai;
DELETE FROM NhaCungCap;
DELETE FROM PhongBan;
DELETE FROM TrangThai;
DELETE FROM TrangWeb;
