USE MultiShop
GO

INSERT INTO PhongBan (MaPB, TenPB, ThongTin)
VALUES 
    ('ADMIN', N'Phòng Quản trị', N'Quản lý toàn bộ hệ thống'),
    ('KT',    N'Phòng Kế toán', N'Quản lý thu chi, tài chính'),
    ('BH',    N'Phòng Bán hàng', N'Hỗ trợ và thực hiện bán hàng'),
    ('KHO',   N'Phòng Kho',     N'Quản lý kho hàng và xuất nhập'),
    ('NS',    N'Phòng Nhân sự', N'Quản lý nhân sự và tuyển dụng');

-- Giả sử đã có nhân viên ADMIN01 thuộc phòng ADMIN
INSERT INTO PhanCong (MaNV, MaPB, NgayPC, HieuLuc)
VALUES 
('AD001', 'ADMIN', GETDATE(), 1);