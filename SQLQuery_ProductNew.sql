USE [MultiShop]
GO

-- Nhập dữ liệu bảng NhaCungCap (đã chuẩn hóa tên cột)
INSERT INTO NhaCungCap (MaNCC, TenCongTy, Logo, NguoiLienLac, EmailNCC, DienThoaiNCC, DiaChiNCC, MoTaNCC)
VALUES 
(N'NCC01', N'Công ty Thiên Long', N'thienlong_logo.png', N'Nguyễn Văn An', N'thienlong@vanphongpham.vn', N'0909123456', N'Quận 1, TP.HCM', N'Chuyên cung cấp bút bi xanh, bút bi đỏ, bút chì, hộp bút, bóp viết và các dụng cụ học tập khác.'),
(N'NCC02', N'Công ty Hồng Hà', N'hongha_logo.png', N'Lê Thị Bình', N'hongha@vanphongpham.vn', N'0909988777', N'Quận 7, TP.HCM', N'Nhà sản xuất tập vở, sổ tay, giấy note với chất lượng cao, mẫu mã đa dạng.'),
(N'NCC03', N'Công ty Deli Việt Nam', N'deli_logo.png', N'Trần Văn Cường', N'deli@deli.vn', N'0909555444', N'Quận 5, TP.HCM', N'Cung cấp hộp bút, bóp viết, compas, thước kẻ, kẹp giấy và bìa hồ sơ.'),
(N'NCC04', N'Công ty Plus Stationery', N'plus_logo.png', N'Phạm Thị Duyên', N'plus@plus.vn', N'0909333222', N'Hà Nội', N'Nhà cung cấp bìa hồ sơ, file trình ký, giấy note, kẹp giấy và dụng cụ văn phòng chất lượng.'),
(N'NCC05', N'Công ty Colokit', N'colokit_logo.png', N'Vũ Văn Em', N'colokit@mauve.vn', N'0909222111', N'Quận 3, TP.HCM', N'Chuyên sản xuất màu sáp, màu nước, bút dạ quang và các sản phẩm phục vụ mỹ thuật học sinh.'),
(N'NCC06', N'Công ty Artline Việt Nam', N'artline_logo.png', N'Nguyễn Thị Phương', N'artline@butmau.vn', N'0909111222', N'Bình Thạnh, TP.HCM', N'Cung cấp bút lông bảng, bút dạ quang, bút marker cho học sinh, sinh viên và văn phòng.'),
(N'NCC07', N'Công ty Staedtler Việt Nam', N'staedtler_logo.png', N'Hoàng Văn Giang', N'staedtler@butchi.vn', N'0909000111', N'Quận 1, TP.HCM', N'Nhà cung cấp bút chì gỗ, compas, gôm tẩy, thước kỹ thuật chất lượng cao.'),
(N'NCC08', N'Công ty Faber-Castell', N'fabercastell_logo.png', N'Nguyễn Hữu Long', N'fabercastell@vanphongpham.vn', N'0909345678', N'TP. HCM', N'Cung cấp các loại màu vẽ, bút chì màu, compas và các sản phẩm mỹ thuật cao cấp.'),
(N'NCC09', N'Công ty M&G Stationery', N'mg_logo.png', N'Lê Văn Minh', N'mg@vanphongpham.vn', N'0909456123', N'Quận 10, TP.HCM', N'Cung cấp bút bi, bút gel, thước kẻ, kẹp giấy, gôm tẩy và sổ tay tiện lợi.'),
(N'NCC10', N'Công ty Xukiva', N'xukiva_logo.png', N'Phạm Thị Lan', N'xukiva@xukiva.vn', N'0909876543', N'Quận Bình Thạnh, TP.HCM', N'Sản xuất phấn viết, bảng trắng mini, bảng từ, phục vụ cho lớp học và văn phòng.'),
(N'NCC11', N'Công ty Officetex', N'officetex_logo.png', N'Đặng Văn Khoa', N'officetex@vanphongpham.vn', N'0909123789', N'Cần Thơ', N'Cung cấp các loại keo dán, giấy note, bìa hồ sơ, file trình ký, phục vụ văn phòng và học sinh.'),
(N'NCC12', N'Công ty Bến Nghé', N'bennghe_logo.png', N'Nguyễn Thị Hương', N'bennghe@bennghe.vn', N'0909123000', N'Quận 5, TP.HCM', N'Nhà cung cấp tập vở, bút bi học sinh, bút dạ quang và dụng cụ học tập cơ bản.');

-- Nhập dữ liệu bảng Loai (Danh mục sản phẩm)
SET IDENTITY_INSERT Loai ON;

INSERT INTO Loai (MaLoai, TenLoai, MoTa, TenLoaiAlias, Hinh)
VALUES 
(1, N'Bút bi xanh', N'Bút bi màu xanh nước biển, thiết kế thân thiện, trơn mượt, dùng để ghi chú, làm bài thi và công việc văn phòng.', NULL, NULL),
(2, N'Bút bi đỏ', N'Bút bi màu đỏ thường dùng để sửa bài, đánh dấu, hoặc ký hiệu các nội dung quan trọng. Ngòi trơn, mực đều màu.', NULL, NULL),
(3, N'Bút chì', N'Bút chì gỗ, bút chì bấm, các loại bút chì kỹ thuật phục vụ viết, vẽ và phác thảo.', NULL, NULL),
(4, N'Hộp bút', N'Hộp bút bằng nhựa, vải hoặc da, nhiều ngăn, giúp học sinh, sinh viên bảo quản dụng cụ học tập gọn gàng.', NULL, NULL),
(5, N'Thước kẻ', N'Thước thẳng 15cm, 30cm, thước đo góc, thước mica, thước gỗ phục vụ học tập và kỹ thuật.', NULL, NULL),
(6, N'Gôm tẩy', N'Gôm tẩy chì, gôm tẩy màu, tẩy sạch không lem, thân thiện với môi trường và an toàn cho học sinh.', NULL, NULL),
(7, N'Cặp sách', N'Cặp sách học sinh, balo laptop, balo chống gù với thiết kế thời trang, tiện lợi.', NULL, NULL),
(8, N'Bóp viết', N'Bóp viết nhiều ngăn, nhỏ gọn, giúp bảo vệ bút và các vật dụng học tập nhỏ như thước, compas, tẩy.', NULL, NULL),
(9, N'Sổ tay', N'Sổ tay lò xo, sổ bìa da, sổ mini, dùng ghi chú, ghi nhớ công việc hoặc học tập.', NULL, NULL),
(10, N'Tập vở', N'Tập 96 trang, 200 trang, tập kẻ ngang, tập caro phù hợp cho học sinh và sinh viên.', NULL, NULL),
(11, N'Màu vẽ', N'Màu sáp, màu nước, màu chì màu phục vụ cho môn mỹ thuật và các hoạt động sáng tạo.', NULL, NULL),
(12, N'Bảng viết', N'Bảng trắng mini, bảng từ, bảng dán tường dùng cho học sinh tiểu học, giáo viên và văn phòng.', NULL, NULL),
(13, N'Phấn viết', N'Phấn trắng, phấn màu không bụi, giúp việc giảng dạy trên bảng dễ dàng và sạch sẽ hơn.', NULL, NULL),
(14, N'Kẹp giấy', N'Kẹp bướm, kẹp tài liệu nhiều kích cỡ giúp giữ giấy tờ học tập hoặc hồ sơ văn phòng gọn gàng.', NULL, NULL),
(15, N'Bìa hồ sơ', N'Bìa nhựa A4, bìa còng, bìa trình ký tiện lợi để lưu trữ tài liệu học tập và hành chính.', NULL, NULL),
(16, N'Keo dán', N'Keo khô, keo nước, keo dán giấy an toàn cho học sinh tiểu học, mẫu giáo hoặc sử dụng văn phòng.', NULL, NULL),
(17, N'Compas', N'Compas kim loại, compas nhựa dùng để vẽ hình học chính xác, tiện dụng cho học sinh.', NULL, NULL),
(18, N'Giấy note', N'Giấy ghi chú màu, giấy note dán giúp đánh dấu thông tin quan trọng trong sách vở, tài liệu.', NULL, NULL),
(19, N'File trình ký', N'File trình ký cứng, nhựa mica trong suốt, hỗ trợ việc ký nhận hoặc trình bày hồ sơ.', NULL, NULL),
(20, N'Bút dạ quang', N'Bút đánh dấu dạ quang nhiều màu, dùng để tô sáng những phần cần ghi nhớ trong sách, tài liệu.', NULL, NULL);

SET IDENTITY_INSERT Loai OFF;

/* Blue pen */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC) VALUES 
(N'Bút bi Thiên Long TL-027 xanh', N'but-bi-thien-long-tl-027-xanh', 1, N'Cây', 5000, N'butbi_xanh_tl027.jpg', '2025-06-01', 0, 0,
N'Bút bi Thiên Long TL-027 xanh viết trơn, mực đều, thân bút thiết kế vừa tay, thích hợp sử dụng cho học sinh, sinh viên và nhân viên văn phòng.', N'NCC01'),

(N'Bút bi Thiên Long Flex Office FO-04 xanh', N'but-bi-thien-long-flex-office-fo-04-xanh', 1, N'Cây', 6000, N'butbi_xanh_flexoffice.jpg', '2025-06-10', 0, 0,
N'Bút bi Flex Office FO-04 xanh có thiết kế thân tròn, bấm nhẹ tay, mực ra đều, ngòi bút êm ái phù hợp cho việc ghi chú lâu dài.', N'NCC01'),

(N'Bút bi M&G xanh G1-702', N'but-bi-mg-xanh-g1-702', 1, N'Cây', 4500, N'butbi_xanh_mg702.jpg', '2025-06-05', 0, 0,
N'Bút bi M&G G1-702 màu xanh, ngòi nhỏ 0.5mm, viết trơn, mực khô nhanh, phù hợp cho học sinh luyện chữ và ghi chép hằng ngày.', N'NCC09'),

(N'Bút bi Bến Nghé BN-01 xanh', N'but-bi-ben-nghe-bn-01-xanh', 1, N'Cây', 4000, N'butbi_xanh_bn01.jpg', '2025-05-30', 0, 0,
N'Bút bi Bến Nghé BN-01 màu xanh, thân nhựa trong suốt, ngòi bút siêu mảnh giúp viết rõ nét, sử dụng bền lâu và tiết kiệm chi phí.', N'NCC12');

/* Red pen */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Bút bi Thiên Long TL-027 đỏ', N'but-bi-thien-long-tl-027-do', 2, N'Cây', 5000, N'butbi_do_tl027.jpg', '2025-06-01', 0, 0,
N'Bút bi Thiên Long TL-027 màu đỏ, thiết kế thân bút nhẹ, mực ra đều, không lem, thường dùng để sửa bài hoặc ghi chú quan trọng.', N'NCC01'),

(N'Bút bi Thiên Long Flex Office FO-04 đỏ', N'but-bi-thien-long-flex-office-fo-04-do', 2, N'Cây', 6000, N'butbi_do_flexoffice.jpg', '2025-06-10', 0, 0,
N'Bút bi Flex Office FO-04 đỏ có cơ chế bấm nhẹ nhàng, ngòi 0.7mm cho nét chữ rõ ràng, mực đỏ tươi dễ nhìn khi sửa bài hoặc đánh dấu.', N'NCC01'),

(N'Bút bi M&G đỏ G1-702', N'but-bi-mg-do-g1-702', 2, N'Cây', 4500, N'butbi_do_mg702.jpg', '2025-06-05', 0, 0,
N'Bút bi M&G G1-702 màu đỏ, thân bút chắc chắn, ngòi bút trơn tru, thích hợp dùng cho giáo viên hoặc ghi chú các nội dung nổi bật.', N'NCC09'),

(N'Bút bi Bến Nghé BN-01 đỏ', N'but-bi-ben-nghe-bn-01-do', 2, N'Cây', 4000, N'butbi_do_bn01.jpg', '2025-05-30', 0, 0,
N'Bút bi Bến Nghé BN-01 màu đỏ với chất lượng ổn định, mực đậm, đều màu, phù hợp sử dụng để sửa bài tập hoặc ký chú thích.', N'NCC12');

/* Pencil */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Bút chì gỗ Thiên Long HB-03', N'but-chi-go-thien-long-hb-03', 3, N'Cây', 4000, N'butchi_thienlong_hb03.jpg', '2025-06-02', 0, 0,
N'Bút chì gỗ Thiên Long HB-03 với độ cứng HB, nét viết đậm rõ ràng, tẩy dễ, vỏ bút làm từ gỗ tự nhiên, an toàn cho học sinh.', N'NCC01'),

(N'Bút chì M&G 2B MG-556', N'but-chi-mg-2b-mg-556', 3, N'Cây', 5000, N'butchi_mg_2b.jpg', '2025-06-07', 0, 0,
N'Bút chì M&G MG-556 loại 2B giúp nét viết đậm, thích hợp cho luyện chữ đẹp hoặc vẽ kỹ thuật, chất lượng ổn định, dễ chuốt.', N'NCC09'),

(N'Bút chì Staedtler Noris 120 2B', N'but-chi-staedtler-noris-120-2b', 3, N'Cây', 9000, N'butchi_staedtler_noris120.jpg', '2025-05-29', 0, 0,
N'Bút chì Staedtler Noris 120 có thân lục giác, bọc sơn chất lượng cao, ruột bút siêu bền, phù hợp cho học sinh và kiến trúc sư.', N'NCC07'),

(N'Bút chì Faber-Castell Classic 9000 HB', N'but-chi-faber-castell-classic-9000-hb', 3, N'Cây', 10000, N'butchi_faber_castell_9000.jpg', '2025-06-12', 0, 0,
N'Bút chì Faber-Castell 9000 HB với chất lượng quốc tế, ruột chì bền, nét viết mượt, thường dùng trong học tập và vẽ kỹ thuật.', N'NCC08');

/* Plastic Pen Case */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Hộp bút nhựa Deli 2 ngăn 9132', N'hop-but-nhua-deli-2-ngan-9132', 4, N'Cái', 35000, N'hopbutnhua_deli_9132.jpg', '2025-06-03', 0, 0,
N'Hộp bút nhựa Deli 2 ngăn 9132 làm từ nhựa cứng bền đẹp, ngăn chứa tiện lợi, khóa mở dễ dàng, thích hợp cho học sinh cấp 1 và cấp 2.', N'NCC03'),

(N'Hộp bút nhựa Hồng Hà HH-2025', N'hop-but-nhua-hong-ha-hh-2025', 4, N'Cái', 38000, N'hopbutnhua_hongha_2025.jpg', '2025-06-05', 0, 0,
N'Hộp bút nhựa Hồng Hà HH-2025 có thiết kế đơn giản, chất liệu nhựa ABS an toàn, chống va đập, giúp đựng bút, thước gọn gàng.', N'NCC02'),

(N'Hộp bút nhựa Faber-Castell FC-1100', N'hop-but-nhua-faber-castell-fc-1100', 4, N'Cái', 40000, N'hopbutnhua_faber_castell.jpg', '2025-06-08', 0, 0,
N'Hộp bút nhựa Faber-Castell FC-1100 với bề mặt nhám chống trượt, ngăn chính lớn, giúp bảo quản dụng cụ học tập sạch sẽ, gọn gàng.', N'NCC08'),

(N'Hộp bút nhựa Staedtler ST-400', N'hop-but-nhua-staedtler-st-400', 4, N'Cái', 42000, N'hopbutnhua_staedtler.jpg', '2025-06-04', 0, 0,
N'Hộp bút nhựa Staedtler ST-400 chất liệu cao cấp, nắp đóng mở chắc chắn, thiết kế hiện đại, phù hợp với học sinh các cấp.', N'NCC07');

/* Ruler */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Thước nhựa Deli 15cm', N'thuoc-nhua-deli-15cm', 5, N'Cây', 8000, N'thuoc_deli_15cm.jpg', '2025-06-05', 0, 0,
N'Thước nhựa Deli 15cm làm từ nhựa trong suốt, vạch chia rõ ràng, bền chắc, không dễ gãy khi rơi, phù hợp cho học sinh sử dụng hằng ngày.', N'NCC03'),

(N'Thước thẳng Hồng Hà 20cm', N'thuoc-thang-hong-ha-20cm', 5, N'Cây', 9000, N'thuoc_hongha_20cm.jpg', '2025-06-07', 0, 0,
N'Thước thẳng Hồng Hà 20cm bằng nhựa cao cấp, chịu lực tốt, vạch chia chuẩn xác, thích hợp cho học sinh và sinh viên.', N'NCC02'),

(N'Thước kẻ Staedtler 562 30cm', N'thuoc-ke-staedtler-562-30cm', 5, N'Cây', 18000, N'thuoc_staedtler_30cm.jpg', '2025-06-03', 0, 0,
N'Thước Staedtler 562 dài 30cm, chất liệu nhựa dẻo cao cấp, không bị nứt khi va đập, phù hợp cho học sinh và kiến trúc sư.', N'NCC07'),

(N'Thước kỹ thuật Faber-Castell FC-30cm', N'thuoc-ky-thuat-faber-castell-fc-30cm', 5, N'Cây', 20000, N'thuoc_faber_castell_30cm.jpg', '2025-06-08', 0, 0,
N'Thước kỹ thuật Faber-Castell FC-30cm với vạch đo in khắc laser bền đẹp, đảm bảo độ chính xác cao, thích hợp cho vẽ kỹ thuật.', N'NCC08');

/* Erase */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Gôm tẩy Thiên Long E-08', N'gom-tay-thien-long-e-08', 6, N'Cục', 5000, N'gom_thienlong_e08.jpg', '2025-06-04', 0, 0,
N'Gôm tẩy Thiên Long E-08 làm từ chất liệu cao su dẻo, tẩy sạch nét chì, không làm rách giấy, thích hợp dùng trong học tập và văn phòng.', N'NCC01'),

(N'Gôm tẩy Deli EH20800', N'gom-tay-deli-eh20800', 6, N'Cục', 6000, N'gom_deli_eh20800.jpg', '2025-06-06', 0, 0,
N'Gôm tẩy Deli EH20800 có thiết kế nhỏ gọn, dễ cầm nắm, tẩy sạch các nét viết chì mà không để lại vết bẩn hay vụn nhiều.', N'NCC03'),

(N'Gôm tẩy Staedtler Mars Plastic', N'gom-tay-staedtler-mars-plastic', 6, N'Cục', 15000, N'gom_staedtler_mars.jpg', '2025-06-08', 0, 0,
N'Gôm Staedtler Mars Plastic chất lượng cao, tẩy sạch nét chì dễ dàng, không lem giấy, thân thiện với môi trường và không chứa PVC.', N'NCC07'),

(N'Gôm tẩy Faber-Castell 7085-20', N'gom-tay-faber-castell-7085-20', 6, N'Cục', 12000, N'gom_faber_castell_7085.jpg', '2025-06-09', 0, 0,
N'Gôm tẩy Faber-Castell 7085-20 với chất liệu sạch, dễ tẩy nét chì, ít vụn, không làm rách giấy, phù hợp cho học sinh và sinh viên.', N'NCC08');


/* Study Bag */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Cặp học sinh chống gù Hồng Hà 8085', N'cap-hoc-sinh-chong-gu-hong-ha-8085', 7, N'Cái', 350000, N'cap_hongha_8085.jpg', '2025-06-05', 0, 0,
N'Cặp học sinh Hồng Hà 8085 thiết kế chống gù lưng, đệm lưng êm ái, quai đeo bản lớn, phù hợp cho học sinh tiểu học mang theo sách vở.', N'NCC02'),

(N'Cặp học sinh Delune DL-216', N'cap-hoc-sinh-delune-dl-216', 7, N'Cái', 450000, N'cap_delune_dl216.jpg', '2025-06-08', 0, 0,
N'Cặp học sinh Delune DL-216 kiểu dáng hiện đại, màu sắc tươi sáng, chất liệu chống thấm nước, ngăn chứa rộng rãi.', N'NCC10'),

(N'Balo học sinh Deli 70031', N'balo-hoc-sinh-deli-70031', 7, N'Cái', 320000, N'balo_deli_70031.jpg', '2025-06-06', 0, 0,
N'Balo học sinh Deli 70031 với chất liệu vải dù cao cấp, chống thấm tốt, ngăn chính rộng rãi, thiết kế phù hợp học sinh cấp 1, cấp 2.', N'NCC03'),

(N'Cặp học sinh Miti MT-234', N'cap-hoc-sinh-miti-mt-234', 7, N'Cái', 300000, N'cap_miti_mt234.jpg', '2025-06-04', 0, 0,
N'Cặp học sinh Miti MT-234 sản xuất tại Việt Nam, quai đeo êm vai, chống gù, thích hợp cho học sinh tiểu học đến trung học.', N'NCC11');

/* Fabric pen case */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Bóp viết vải Canvas Hồng Hà HH-3050', N'bop-viet-vai-canvas-hong-ha-hh-3050', 8, N'Cái', 48000, N'bopviet_vai_canvas_hongha.jpg', '2025-06-10', 0, 0,
N'Bóp viết vải Canvas Hồng Hà HH-3050 với chất liệu vải canvas bền đẹp, họa tiết dễ thương, ngăn lớn đựng nhiều dụng cụ học tập.', N'NCC02'),

(N'Bóp viết vải Deli Zipper 7025', N'bop-viet-vai-deli-zipper-7025', 8, N'Cái', 42000, N'bopviet_vai_deli_7025.jpg', '2025-06-07', 0, 0,
N'Bóp viết vải Deli Zipper 7025 chất liệu vải dù cao cấp, khóa kéo bền, nhiều ngăn nhỏ bên trong, tiện lợi mang theo khi đi học.', N'NCC03'),

(N'Bóp viết vải M&G Simple MG-500', N'bop-viet-vai-mg-simple-mg-500', 8, N'Cái', 45000, N'bopviet_vai_mg_simple.jpg', '2025-06-09', 0, 0,
N'Bóp viết vải M&G Simple MG-500 có thiết kế đơn giản, nhẹ nhàng, chất liệu mềm mại, phù hợp với học sinh và sinh viên.', N'NCC09'),

(N'Bóp viết vải Faber-Castell Soft 9002', N'bop-viet-vai-faber-castell-soft-9002', 8, N'Cái', 52000, N'bopviet_vai_faber_9002.jpg', '2025-06-11', 0, 0,
N'Bóp viết vải Faber-Castell Soft 9002 chất liệu mềm chống nước, khóa kéo chắc chắn, ngăn chứa rộng cho bút viết và thước.', N'NCC08');


/* Notes Nook */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Sổ tay gáy lò xo Thiên Long A5', N'so-tay-gay-lo-xo-thien-long-a5', 9, N'Cuốn', 45000, N'sotay_thienlong_a5.jpg', '2025-06-05', 0, 0,
N'Sổ tay Thiên Long A5 gáy lò xo tiện dụng, giấy dày 80gsm không lem mực, thiết kế nhỏ gọn phù hợp ghi chú khi học tập và làm việc.', N'NCC01'),

(N'Sổ tay bìa da Hồng Hà HH-520', N'so-tay-bia-da-hong-ha-hh-520', 9, N'Cuốn', 85000, N'sotay_hongha_biada.jpg', '2025-06-07', 0, 0,
N'Sổ tay Hồng Hà HH-520 bìa da sang trọng, giấy chất lượng cao, phù hợp cho nhân viên văn phòng, sinh viên ghi chép lịch trình.', N'NCC02'),

(N'Sổ tay Faber-Castell Premium A6', N'so-tay-faber-castell-premium-a6', 9, N'Cuốn', 60000, N'sotay_faber_a6.jpg', '2025-06-09', 0, 0,
N'Sổ tay Faber-Castell Premium A6 nhỏ gọn, giấy mịn không thấm mực, thiết kế hiện đại, thích hợp để ghi chú cá nhân hoặc công việc.', N'NCC08'),

(N'Sổ tay M&G Classic A5', N'so-tay-mg-classic-a5', 9, N'Cuốn', 50000, N'sotay_mg_classic.jpg', '2025-06-06', 0, 0,
N'Sổ tay M&G Classic A5 với bìa cứng chống cong, giấy trắng tự nhiên, không gây mỏi mắt khi viết lâu, phù hợp với học sinh, sinh viên.', N'NCC09');


/* Book */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Tập 200 trang Campus Kokuyo', N'tap-200-trang-campus-kokuyo', 10, N'Cuốn', 28000, N'tap_campus_200tr.jpg', '2025-06-05', 0, 0,
N'Tập 200 trang Campus Kokuyo giấy dày 80gsm, viết mực không thấm, bìa mềm cao cấp, dùng cho học sinh sinh viên ghi chép bài học.', N'NCC10'),

(N'Tập Hồng Hà 96 trang HH-312', N'tap-96-trang-hong-ha-hh-312', 10, N'Cuốn', 12000, N'tap_hongha_96tr.jpg', '2025-06-07', 0, 0,
N'Tập Hồng Hà 96 trang HH-312 giấy trắng tự nhiên, đường kẻ rõ nét, bìa màu sắc tươi sáng, phù hợp học sinh tiểu học và trung học.', N'NCC02'),

(N'Tập vở Thiên Long TL-212', N'tap-vo-thien-long-tl-212', 10, N'Cuốn', 15000, N'tap_thienlong_212.jpg', '2025-06-06', 0, 0,
N'Tập vở Thiên Long TL-212 với giấy trắng mịn, viết êm tay, không lem mực, bìa vở thiết kế trẻ trung phù hợp cho học sinh.', N'NCC01'),

(N'Tập M&G Classic 200 trang', N'tap-mg-classic-200-trang', 10, N'Cuốn', 25000, N'tap_mg_200tr.jpg', '2025-06-09', 0, 0,
N'Tập M&G Classic 200 trang, bìa cứng dẻo dai, giấy chất lượng cao, không thấm mực, phù hợp ghi chú bài học và công việc.', N'NCC09');

/* Crayon Box */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Hộp màu sáp Thiên Long Colokit CR-C016', N'hop-mau-sap-thien-long-colokit-cr-c016', 3, N'Hộp', 45000, N'mau_sap_thienlong_colokit.jpg', '2025-06-05', 0, 0,
N'Hộp màu sáp Thiên Long Colokit CR-C016 gồm 16 màu tươi sáng, chất liệu an toàn cho trẻ em, màu dễ bám giấy, không lem tay.', N'NCC01'),

(N'Hộp màu nước Hồng Hà HH-320', N'hop-mau-nuoc-hong-ha-hh-320', 3, N'Hộp', 55000, N'mau_nuoc_hongha_320.jpg', '2025-06-07', 0, 0,
N'Hộp màu nước Hồng Hà HH-320 gồm 12 màu cơ bản, dễ pha trộn, chất lượng màu an toàn, không độc hại, thích hợp cho học sinh.', N'NCC02'),

(N'Hộp màu chì Faber-Castell Classic 12 màu', N'hop-mau-chi-faber-castell-classic-12', 3, N'Hộp', 90000, N'mau_chi_faber_12.jpg', '2025-06-09', 0, 0,
N'Hộp màu chì Faber-Castell Classic 12 màu có độ bám giấy tốt, dễ tô, màu sắc chuẩn, phù hợp cho học sinh luyện tập và tô màu.', N'NCC08'),

(N'Hộp màu sáp Deli EC00100 24 màu', N'hop-mau-sap-deli-ec00100-24-mau', 3, N'Hộp', 75000, N'mau_sap_deli_24mau.jpg', '2025-06-06', 0, 0,
N'Hộp màu sáp Deli EC00100 với 24 màu sắc đa dạng, chất liệu an toàn, không bụi, không lem, giúp trẻ em phát triển tư duy sáng tạo.', N'NCC03'),

/* Drawing Board */
(N'Bảng vẽ mini Deli 78620', N'bang-ve-mini-deli-78620', 5, N'Cái', 65000, N'bangve_deli_78620.jpg', '2025-06-04', 0, 0,
N'Bảng vẽ mini Deli 78620 kích thước nhỏ gọn, mặt bảng trơn nhẵn dễ lau sạch, khung nhựa bền chắc, phù hợp cho học sinh luyện tập.', N'NCC03'),

(N'Bảng vẽ mini Hồng Hà HH-450', N'bang-ve-mini-hong-ha-hh-450', 5, N'Cái', 70000, N'bangve_hongha_450.jpg', '2025-06-06', 0, 0,
N'Bảng vẽ mini Hồng Hà HH-450 với mặt bảng trắng, dễ viết và dễ xóa, khung viền nhựa chắc chắn, thích hợp cho các bé tập vẽ tại nhà.', N'NCC02'),

(N'Bảng vẽ mini Thiên Long TL-MB01', N'bang-ve-mini-thien-long-tl-mb01', 5, N'Cái', 68000, N'bangve_thienlong_mb01.jpg', '2025-06-05', 0, 0,
N'Bảng vẽ mini Thiên Long TL-MB01 có bề mặt phủ chống trầy xước, dễ viết bằng bút lông, tiện lợi khi mang theo đi học hoặc vẽ tại nhà.', N'NCC01'),

(N'Bảng vẽ mini M&G MG-2302', N'bang-ve-mini-mg-2302', 5, N'Cái', 72000, N'bangve_mg_2302.jpg', '2025-06-08', 0, 0,
N'Bảng vẽ mini M&G MG-2302 thiết kế nhẹ, dễ mang đi, bề mặt bảng không bám bụi, dễ lau chùi, phù hợp cho học sinh luyện chữ, vẽ tranh.', N'NCC09'),

/* Chalk */
(N'Hộp phấn trắng Thiên Long TL-C01', N'hop-phan-trang-thien-long-tl-c01', 7, N'Hộp', 15000, N'phan_trang_thienlong_c01.jpg', '2025-06-05', 0, 0,
N'Hộp phấn trắng Thiên Long TL-C01 gồm 10 viên, ít bụi, không gãy vụn khi viết, phù hợp cho học sinh, giáo viên sử dụng hàng ngày.', N'NCC01'),

(N'Hộp phấn màu Hồng Hà HH-MC10', N'hop-phan-mau-hong-ha-hh-mc10', 7, N'Hộp', 18000, N'phan_mau_hongha_mc10.jpg', '2025-06-07', 0, 0,
N'Hộp phấn màu Hồng Hà HH-MC10 gồm 10 viên màu tươi sáng, dễ viết lên bảng đen và bảng xanh, không bụi, an toàn cho sức khỏe.', N'NCC02'),

(N'Hộp phấn trắng M&G MG-C08', N'hop-phan-trang-mg-c08', 7, N'Hộp', 14000, N'phan_trang_mg_c08.jpg', '2025-06-06', 0, 0,
N'Hộp phấn trắng M&G MG-C08 chất lượng cao, viết êm tay, không lem, không gây ngứa tay khi cầm lâu, thích hợp cho trường học.', N'NCC09'),

(N'Hộp phấn màu Deli EC20200', N'hop-phan-mau-deli-ec20200', 7, N'Hộp', 19000, N'phan_mau_deli_ec20200.jpg', '2025-06-08', 0, 0,
N'Hộp phấn màu Deli EC20200 gồm 12 viên, màu sắc rực rỡ, bám bảng tốt, không tạo bụi bay, an toàn cho học sinh và giáo viên.', N'NCC03');

/* Paper Clip */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Kẹp giấy Thiên Long 32mm TL-CL32', N'kep-giay-thien-long-32mm-tl-cl32', 12, N'Hộp', 15000, N'kepgiay_thienlong_32mm.jpg', '2025-06-05', 0, 0,
N'Kẹp giấy Thiên Long TL-CL32 cỡ 32mm, giữ tài liệu chắc chắn, thép không gỉ, phù hợp cho học sinh và văn phòng sử dụng.', N'NCC01'),

(N'Kẹp giấy Deli 41mm EC01100', N'kep-giay-deli-41mm-ec01100', 12, N'Hộp', 20000, N'kepgiay_deli_41mm.jpg', '2025-06-06', 0, 0,
N'Kẹp giấy Deli EC01100 kích thước 41mm, chất liệu thép sơn tĩnh điện bền đẹp, giữ chặt nhiều tờ giấy, không làm rách mép.', N'NCC03'),

(N'Kẹp giấy M&G 25mm MG-BG25', N'kep-giay-mg-25mm-mg-bg25', 12, N'Hộp', 12000, N'kepgiay_mg_25mm.jpg', '2025-06-08', 0, 0,
N'Kẹp giấy M&G MG-BG25 loại nhỏ 25mm, phù hợp kẹp giấy tờ số lượng ít, tiện lợi sử dụng trong học tập và công việc văn phòng.', N'NCC09');

/* Cover */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Bìa hồ sơ Thiên Long FO-A4', N'bia-ho-so-thien-long-fo-a4', 13, N'Cái', 12000, N'biahoso_thienlong_foa4.jpg', '2025-06-07', 0, 0,
N'Bìa hồ sơ Thiên Long FO-A4 làm từ nhựa PP cứng, không thấm nước, giúp bảo quản giấy tờ học tập, hồ sơ cá nhân an toàn.', N'NCC01'),

(N'Bìa hồ sơ Deli 5551', N'bia-ho-so-deli-5551', 13, N'Cái', 15000, N'biahoso_deli_5551.jpg', '2025-06-05', 0, 0,
N'Bìa hồ sơ Deli 5551 có khóa bấm tiện lợi, chất liệu nhựa mềm dẻo, bảo vệ tốt tài liệu, phù hợp mang theo đến lớp hoặc văn phòng.', N'NCC03'),

(N'Bìa hồ sơ Hồng Hà HH-920', N'bia-ho-so-hong-ha-hh-920', 13, N'Cái', 14000, N'biahoso_hongha_920.jpg', '2025-06-09', 0, 0,
N'Bìa hồ sơ Hồng Hà HH-920 thiết kế đơn giản, màu sắc nhã nhặn, chất liệu bền, dùng đựng bài tập, giấy tờ khi đi học hoặc làm việc.', N'NCC02');

/* Glue */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Keo dán giấy Thiên Long FO-G01', N'keo-dan-giay-thien-long-fo-g01', 14, N'Cây', 12000, N'keodan_thienlong_g01.jpg', '2025-06-05', 0, 0,
N'Keo dán giấy Thiên Long FO-G01 dạng thỏi, dễ sử dụng, không lem, không độc hại, an toàn cho học sinh sử dụng trong học tập.', N'NCC01'),

(N'Keo dán nước Deli 30ml EC20300', N'keo-dan-nuoc-deli-30ml-ec20300', 14, N'Lọ', 10000, N'keodan_deli_30ml.jpg', '2025-06-07', 0, 0,
N'Keo dán nước Deli EC20300 dung tích 30ml, dạng lỏng dễ bôi, không màu, không độc hại, phù hợp dán giấy, thủ công học sinh.', N'NCC03'),

(N'Keo dán M&G thỏi MG-GS20', N'keo-dan-mg-thoi-mg-gs20', 14, N'Cây', 11000, N'keodan_mg_gs20.jpg', '2025-06-08', 0, 0,
N'Keo dán M&G thỏi MG-GS20 chất lượng cao, bám dính tốt, không để lại vết ố giấy, tiện lợi cho học sinh và nhân viên văn phòng.', N'NCC09'),

(N'Keo dán giấy Hồng Hà HH-4040', N'keo-dan-giay-hong-ha-hh-4040', 14, N'Cây', 13000, N'keodan_hongha_4040.jpg', '2025-06-06', 0, 0,
N'Keo dán giấy Hồng Hà HH-4040 thân thiện môi trường, không mùi, dễ sử dụng cho học sinh trong các hoạt động thủ công, mỹ thuật.', N'NCC02');

/* Compas */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Compa Deli Student 8564', N'compa-deli-student-8564', 15, N'Cái', 25000, N'compa_deli_8564.jpg', '2025-06-05', 0, 0,
N'Compa Deli Student 8564 bằng kim loại chắc chắn, dễ điều chỉnh độ rộng, thích hợp cho học sinh cấp 2, cấp 3 luyện hình tròn.', N'NCC03'),

(N'Compa Thiên Long TL-CP01', N'compa-thien-long-tl-cp01', 15, N'Cái', 22000, N'compa_thienlong_cp01.jpg', '2025-06-06', 0, 0,
N'Compa Thiên Long TL-CP01 thiết kế nhỏ gọn, dễ dàng sử dụng, đầu compa có đệm chống trượt, giúp vẽ hình tròn chính xác.', N'NCC01'),

(N'Compa M&G MG-CP20', N'compa-mg-mg-cp20', 15, N'Cái', 24000, N'compa_mg_cp20.jpg', '2025-06-08', 0, 0,
N'Compa M&G MG-CP20 làm từ thép bền chắc, đầu nhọn chống trượt, đi kèm bút chì tiện lợi, phù hợp cho học sinh luyện vẽ hình học.', N'NCC09'),

(N'Compa Hồng Hà HH-8820', N'compa-hong-ha-hh-8820', 15, N'Cái', 23000, N'compa_hongha_8820.jpg', '2025-06-07', 0, 0,
N'Compa Hồng Hà HH-8820 có tay cầm tiện lợi, thân kim loại chắc chắn, phù hợp vẽ hình học trong học tập và làm việc.', N'NCC02');

/* Note Paper */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Giấy note Deli 76x76mm 100 tờ', N'giay-note-deli-76x76mm-100-to', 18, N'Tập', 18000, N'giaynote_deli_76x76.jpg', '2025-06-05', 0, 0,
N'Giấy note Deli 76x76mm gồm 100 tờ, màu sắc tươi sáng, bám dính tốt, dễ ghi chú nhanh, phù hợp để dán lên sách vở hoặc màn hình máy tính.', N'NCC03'),
(N'Giấy note Hồng Hà HH-5500', N'giay-note-hong-ha-hh-5500', 18, N'Tập', 16000, N'giaynote_hongha_5500.jpg', '2025-06-07', 0, 0,
N'Giấy note Hồng Hà HH-5500 với keo dán chắc chắn, dễ bóc không để lại keo, nhiều màu sắc bắt mắt giúp phân loại thông tin hiệu quả.', N'NCC02'),
(N'Giấy note Thiên Long FO-N01', N'giay-note-thien-long-fo-n01', 18, N'Tập', 15000, N'giaynote_thienlong_fo_n01.jpg', '2025-06-06', 0, 0,
N'Giấy note Thiên Long FO-N01 có chất lượng giấy tốt, không thấm mực, dùng để ghi chú công việc, học tập, dễ dán lên bề mặt phẳng.', N'NCC01'),
(N'Giấy note M&G MG-N100', N'giay-note-mg-mg-n100', 18, N'Tập', 17000, N'giaynote_mg_n100.jpg', '2025-06-08', 0, 0,
N'Giấy note M&G MG-N100 gồm nhiều màu sắc nổi bật, kích thước tiêu chuẩn, tiện lợi khi ghi chú và đánh dấu tài liệu quan trọng.', N'NCC09');

/* File for signature */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'File trình ký Thiên Long FO-C01', N'file-trinh-ky-thien-long-fo-c01', 19, N'Cái', 25000, N'filetrinhky_thienlong_c01.jpg', '2025-06-05', 0, 0,
N'File trình ký Thiên Long FO-C01 làm từ nhựa mica trong suốt, kẹp giấy chắc chắn, giúp ký nhận tài liệu dễ dàng, tiện lợi trong hội họp.', N'NCC01'),
(N'File trình ký Deli 5501', N'file-trinh-ky-deli-5501', 19, N'Cái', 28000, N'filetrinhky_deli_5501.jpg', '2025-06-07', 0, 0,
N'File trình ký Deli 5501 chất liệu nhựa cứng, bền bỉ, mặt mica chống xước, dùng để ký nhận hoặc trình bày hồ sơ tại văn phòng.', N'NCC03'),
(N'File trình ký Hồng Hà HH-660', N'file-trinh-ky-hong-ha-hh-660', 19, N'Cái', 26000, N'filetrinhky_hongha_660.jpg', '2025-06-06', 0, 0,
N'File trình ký Hồng Hà HH-660 thiết kế đơn giản, kẹp chắc tay, hỗ trợ ký kết văn bản hay trình bày tài liệu trong học tập và công việc.', N'NCC02');

/* Highlighter */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES 
(N'Bút dạ quang Thiên Long HL-01 vàng', N'but-da-quang-thien-long-hl-01-vang', 20, N'Cây', 12000, N'butdaquang_thienlong_vang.jpg', '2025-06-05', 0, 0,
N'Bút dạ quang Thiên Long HL-01 màu vàng, mực sáng, không lem, thân bút thiết kế dễ cầm, thích hợp để đánh dấu nội dung quan trọng.', N'NCC01'),
(N'Bút dạ quang Deli 6200 hồng', N'but-da-quang-deli-6200-hong', 20, N'Cây', 13000, N'butdaquang_deli_hong.jpg', '2025-06-07', 0, 0,
N'Bút dạ quang Deli 6200 màu hồng, mực chất lượng cao, nét tô đều, không phai màu sau thời gian dài, phù hợp cho học sinh, sinh viên.', N'NCC03'),
(N'Bút dạ quang M&G MG-HL03 xanh', N'but-da-quang-mg-mg-hl03-xanh', 20, N'Cây', 11000, N'butdaquang_mg_xanh.jpg', '2025-06-06', 0, 0,
N'Bút dạ quang M&G MG-HL03 màu xanh, đầu bút vát giúp tô chính xác từng dòng, mực nhanh khô, không lem ra giấy mỏng.', N'NCC09'),
(N'Bút dạ quang Hồng Hà HH-HL02 cam', N'but-da-quang-hong-ha-hh-hl02-cam', 20, N'Cây', 12500, N'butdaquang_hongha_cam.jpg', '2025-06-08', 0, 0,
N'Bút dạ quang Hồng Hà HH-HL02 màu cam, thiết kế nhỏ gọn, mực bền màu, không lem nhòe, phù hợp để ghi chú và đánh dấu bài học.', N'NCC02');

/* Glue */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES
(N'Keo dán Thiên Long G-05', N'keo-dan-thien-long-g-05', 16, N'Chai', 9000, N'keodan_g05.jpg', '2025-06-15', 0, 0,
N'Keo dán Thiên Long G-05 dung tích 60ml, keo trắng dính chắc, an toàn với học sinh, dùng trong học tập và văn phòng.', N'NCC01'),

(N'Keo dán khô Hồng Hà 20g', N'keo-dan-kho-hong-ha-20g', 16, N'Cây', 8000, N'keodan_hongha20g.jpg', '2025-06-12', 0, 0,
N'Keo khô Hồng Hà 20g, không lem, không độc hại, tiện lợi khi sử dụng cho học sinh tiểu học.', N'NCC02')

/* Compas */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES
(N'Bộ compa Deli Student 2 chi tiết', N'bo-compa-deli-student-2-chi-tiet', 17, N'Bộ', 18000, N'compa_deli2.jpg', '2025-06-20', 0, 0,
N'Bộ compa Deli Student gồm 1 compa và 1 cây chì, thiết kế nhỏ gọn, độ chính xác cao, phù hợp cho học sinh.', N'NCC03'),

(N'Compa Hồng Hà HH-CM01', N'compa-hong-ha-hh-cm01', 17, N'Cây', 15000, N'compa_hongha_cm01.jpg', '2025-06-18', 0, 0,
N'Compa Hồng Hà HH-CM01 bằng kim loại bền, độ xoay mượt, dùng để vẽ hình tròn chính xác cho học sinh trung học.', N'NCC02')

/* Crayon Box */
INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, Hinh, NgaySX, GiamGia, SoLanXem, MoTa, MaNCC)
VALUES
(N'Hộp màu sáp Thiên Long CR-C01', N'hop-mau-sap-thien-long-cr-c01', 11, N'Hộp', 25000, N'mau_sap_crc01.jpg', '2025-06-10', 0, 0,
N'Hộp màu sáp CR-C01 gồm 12 màu, dễ tô, màu sắc tươi sáng, không độc hại, phù hợp cho trẻ em.', N'NCC01'),

(N'Hộp màu nước Faber-Castell FC-WP12', N'hop-mau-nuoc-faber-castell-fc-wp12', 11, N'Hộp', 45000, N'mau_nuoc_fcwp12.jpg', '2025-06-05', 0, 0,
N'Màu nước Faber-Castell FC-WP12 gồm 12 màu kèm cọ, màu bám tốt, tán đều, dùng trong mỹ thuật học đường.', N'NCC08'),

(N'Hộp màu chì M&G 24 màu', N'hop-mau-chi-mg-24-mau', 11, N'Hộp', 30000, N'mau_chi_mg24.jpg', '2025-06-08', 0, 0,
N'Màu chì M&G gồm 24 cây, vỏ gỗ chất lượng, màu sắc tươi tắn, bền, không gãy ngòi khi chuốt.', N'NCC09');
