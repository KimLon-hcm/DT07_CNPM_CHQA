
-- TẠO CÁC BẢNG KHÔNG CÓ KHÓA NGOẠI (BẢNG GỐC)
--==============================================================
CREATE DATABASE QL_SHOPQUANAO
USE QL_SHOPQUANAO
GO
-- 1. Bảng Loại Hàng (Category)
CREATE TABLE LoaiHang (
    IDLoaiHang INT PRIMARY KEY IDENTITY(1,1),
    TenLoaiHang NVARCHAR(100) NOT NULL
);

-- 2. Bảng Thương Hiệu (Brand)
CREATE TABLE ThuongHieu (
    IDThuongHieu INT PRIMARY KEY IDENTITY(1,1),
    TenThuongHieu NVARCHAR(100) NOT NULL
);

-- 3. Bảng Màu Sắc (Color)
CREATE TABLE MauSac (
    IDMauSac INT PRIMARY KEY IDENTITY(1,1),
    TenMau NVARCHAR(50) NOT NULL,
    MaMauHex VARCHAR(7)
);

-- 4. Bảng Kích Thước (Size)
CREATE TABLE KichThuoc (
    IDKichThuoc INT PRIMARY KEY IDENTITY(1,1),
    TenKichThuoc VARCHAR(50) NOT NULL 
);

-- 5. Bảng Người Dùng (User)
CREATE TABLE NguoiDung (
    IDNguoiDung INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    SoDienThoai VARCHAR(15) UNIQUE,
    MatKhauHash VARCHAR(255) NOT NULL, -- Sẽ lưu mật khẩu đã được mã hóa
    LoaiTK VARCHAR(20) NOT NULL DEFAULT 'Customer',
    NgayTao DATETIME DEFAULT GETDATE()
);

-- 6. Bảng Địa Chỉ (Address)
CREATE TABLE DiaChi (
    IDDiaChi INT PRIMARY KEY IDENTITY(1,1),
    TinhThanh NVARCHAR(100),
    QuanHuyen NVARCHAR(100),
    PhuongXa NVARCHAR(100),
    ChiTiet NVARCHAR(255) NOT NULL 
);

-- 7. Bảng Phương Thức Thanh Toán (Payment Method)
CREATE TABLE PhuongThucThanhToan (
    ID INT PRIMARY KEY IDENTITY(1,1),
    TenPhuongThuc NVARCHAR(100) NOT NULL
);

-- 8. Bảng Giảm Giá (Voucher/Discount)
CREATE TABLE GiamGia (
    IDGiamGia INT PRIMARY KEY IDENTITY(1,1),
    MaGiamGia VARCHAR(50) NOT NULL UNIQUE,
    TenChuongTrinh NVARCHAR(255),
    LoaiGiamGia INT NOT NULL, -- 1: Giảm theo %, 2: Giảm số tiền cố định
    GiaTri DECIMAL(18, 2) NOT NULL,
    DonHangToiThieu DECIMAL(18, 2) DEFAULT 0,
    GiamToiDa DECIMAL(18, 2), -- Số tiền giảm tối đa (quan trọng khi giảm theo %)
    SoLuong INT NOT NULL,
    DaSuDung INT NOT NULL DEFAULT 0,
    NgayBatDau DATETIME,
    NgayKetThuc DATETIME,
    TrangThai BIT NOT NULL DEFAULT 1 -- 1: Active, 0: Inactive
);


-- TẠO CÁC BẢNG CÓ KHÓA NGOẠI
--==============================================================

-- 9. Bảng Hàng Hóa (Product)
CREATE TABLE HangHoa (
    IDHang INT PRIMARY KEY IDENTITY(1,1),
    TenHang NVARCHAR(255) NOT NULL,
    MoTa NTEXT,
    IDLoaiHang INT,
    IDThuongHieu INT,
    TrangThai BIT DEFAULT 1, -- 1: Đang bán, 0: Ngừng bán
    FOREIGN KEY (IDLoaiHang) REFERENCES LoaiHang(IDLoaiHang),
    FOREIGN KEY (IDThuongHieu) REFERENCES ThuongHieu(IDThuongHieu)
);
select * from HangHoa
-- 10. Bảng Địa Chỉ của Người Dùng (Linking Table)
CREATE TABLE DiaChi_NguoiDung (
    IDNguoiDung INT,
    IDDiaChi INT,
    LaMacDinh BIT DEFAULT 0, -- 1: Là địa chỉ mặc định
    PRIMARY KEY (IDNguoiDung, IDDiaChi),
    FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung),
    FOREIGN KEY (IDDiaChi) REFERENCES DiaChi(IDDiaChi)
);

-- 11. Bảng Biến Thể Hàng Hóa (Product Variant)
CREATE TABLE HangHoa_BienThe (
    IDBienThe INT PRIMARY KEY IDENTITY(1,1),
    IDHang INT,
    IDMauSac INT,
    IDKichThuoc INT,
    SKU VARCHAR(100) UNIQUE, 
    Gia DECIMAL(18, 2) NOT NULL,
    GiaKhuyenMai DECIMAL(18, 2), -- Cho phép NULL, nếu có thì là giá sale
    SoLuongTon INT NOT NULL DEFAULT 0,
    FOREIGN KEY (IDHang) REFERENCES HangHoa(IDHang),
    FOREIGN KEY (IDMauSac) REFERENCES MauSac(IDMauSac),
    FOREIGN KEY (IDKichThuoc) REFERENCES KichThuoc(IDKichThuoc)
);

-- 12. Bảng Hình Ảnh của Biến Thể (Variant Image)
CREATE TABLE HinhAnh_BienThe (
    IDHinhAnh INT PRIMARY KEY IDENTITY(1,1),
    IDBienThe INT,
    DuongDan VARCHAR(500) NOT NULL,
    LaAnhChinh BIT DEFAULT 0, -- 1: Là ảnh chính
    FOREIGN KEY (IDBienThe) REFERENCES HangHoa_BienThe(IDBienThe)
);

-- 13. Bảng Giỏ Hàng (Cart)
CREATE TABLE GioHang (
    IDNguoiDung INT,
    IDBienThe INT,
    SoLuong INT NOT NULL DEFAULT 1,
    PRIMARY KEY (IDNguoiDung, IDBienThe),
    FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung),
    FOREIGN KEY (IDBienThe) REFERENCES HangHoa_BienThe(IDBienThe)
);

-- 14. Bảng Đơn Hàng (Order)
CREATE TABLE DonHang (
    IDDonHang INT PRIMARY KEY IDENTITY(1,1),
    IDNguoiDung INT,
    IDGiamGia INT NULL, -- Cho phép NULL vì đơn hàng có thể không dùng mã giảm giá
    TenNguoiNhan NVARCHAR(100) NOT NULL,
    DiaChiGiao NVARCHAR(500) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    TongTienHang DECIMAL(18, 2) NOT NULL,
    PhiVanChuyen DECIMAL(18, 2) DEFAULT 0,
    GiamGia DECIMAL(18, 2) DEFAULT 0, -- Số tiền được giảm từ voucher
    TongThanhToan DECIMAL(18, 2) NOT NULL,
    IDPhuongThucThanhToan INT,
    TrangThai NVARCHAR(50), -- Ví dụ: 'Chờ xác nhận', 'Đang giao', 'Hoàn thành', 'Đã hủy'
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung),
    FOREIGN KEY (IDGiamGia) REFERENCES GiamGia(IDGiamGia),
    FOREIGN KEY (IDPhuongThucThanhToan) REFERENCES PhuongThucThanhToan(ID)
);

-- 15. Bảng Chi Tiết Đơn Hàng (Order Detail)
CREATE TABLE ChiTiet_DonHang (
    IDDonHang INT,
    IDBienThe INT,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2) NOT NULL, -- Lưu lại giá tại thời điểm mua
    PRIMARY KEY (IDDonHang, IDBienThe),
    FOREIGN KEY (IDDonHang) REFERENCES DonHang(IDDonHang),
    FOREIGN KEY (IDBienThe) REFERENCES HangHoa_BienThe(IDBienThe)
);

-- SỬ DỤNG DATABASE
--==============================================================
USE QL_SHOPQUANAO
GO

-- CHÈN DỮ LIỆU CHO CÁC BẢNG KHÔNG CÓ KHÓA NGOẠI (BẢNG GỐC)
--==============================================================

-- 1. Bảng Loại Hàng (Category)
PRINT '1. Inserting data into LoaiHang...';
INSERT INTO LoaiHang (TenLoaiHang) VALUES
(N'Áo Thun'),
(N'Áo Sơ Mi'),
(N'Quần Jeans'),
(N'Váy')
select * from Loaihang
delete from LoaiHang
where IDLoaiHang = 9

-- 2. Bảng Thương Hiệu (Brand)
PRINT '2. Inserting data into ThuongHieu...';
INSERT INTO ThuongHieu (TenThuongHieu) VALUES
(N'Uniqlo'),
(N'Zara'),
(N'H&M'),
(N'Levi''s'),
(N'Adidas');
select * from ThuongHieu

-- 3. Bảng Màu Sắc (Color)
PRINT '3. Inserting data into MauSac...';
INSERT INTO MauSac (TenMau, MaMauHex) VALUES
(N'Trắng', '#FFFFFF'),
(N'Đen', '#000000'),
(N'Xanh Navy', '#000080'),
(N'Beige', '#F5F5DC'),
(N'Đỏ', '#FF0000');
select * from MauSac
-- 4. Bảng Kích Thước (Size)
PRINT '4. Inserting data into KichThuoc...';
INSERT INTO KichThuoc (TenKichThuoc) VALUES
('S'),
('M'),
('L'),
('XL'),

-- 5. Bảng Người Dùng (User)
PRINT '5. Inserting data into NguoiDung...';
INSERT INTO NguoiDung (HoTen, Email, SoDienThoai, MatKhauHash, LoaiTK) VALUES
(N'Nguyễn Văn An', 'an.nguyen@example.com', '0912345678', 'hashed_password_123', 'Admin'),
(N'Trần Thị Bích', 'bich.tran@example.com', '0987654321', 'hashed_password_456', 'Customer'),
(N'Lê Minh Cường', 'cuong.le@example.com', '0905112233', 'hashed_password_789', 'Customer'),
(N'Phạm Thị Dung', 'dung.pham@example.com', '0334556677', 'hashed_password_012', 'Customer');
select * from NguoiDung
-- 6. Bảng Địa Chỉ (Address)
PRINT '6. Inserting data into DiaChi...';
INSERT INTO DiaChi (TinhThanh, QuanHuyen, PhuongXa, ChiTiet) VALUES
(N'Hà Nội', N'Cầu Giấy', N'Dịch Vọng Hậu', N'Số 144, Đường Xuân Thủy'),
(N'TP. Hồ Chí Minh', N'Quận 1', N'Bến Nghé', N'22, Đường Nguyễn Huệ'),
(N'Đà Nẵng', N'Hải Châu', N'Bình Hiên', N'101, Đường 2 Tháng 9'),
(N'Cần Thơ', N'Ninh Kiều', N'An Phú', N'50, Đại lộ Hòa Bình');

-- 7. Bảng Phương Thức Thanh Toán (Payment Method)
PRINT '7. Inserting data into PhuongThucThanhToan...';
INSERT INTO PhuongThucThanhToan (TenPhuongThuc) VALUES
(N'Thanh toán khi nhận hàng (COD)'),
(N'Chuyển khoản ngân hàng'),
(N'Ví điện tử Momo'),
(N'Thẻ tín dụng/ghi nợ');

-- 8. Bảng Giảm Giá (Voucher/Discount)
PRINT '8. Inserting data into GiamGia...';
INSERT INTO GiamGia (MaGiamGia, TenChuongTrinh, LoaiGiamGia, GiaTri, DonHangToiThieu, GiamToiDa, SoLuong, NgayBatDau, NgayKetThuc) VALUES
('WELCOME20', N'Giảm giá cho khách hàng mới', 1, 20, 500000, 100000, 1000, '2024-01-01', '2024-12-31'),
('FREESHIP', N'Miễn phí vận chuyển', 2, 30000, 200000, 30000, 500, '2024-05-01', '2024-05-31'),
('SALE50K', N'Giảm trực tiếp 50K', 2, 50000, 1000000, 50000, 200, '2024-06-01', '2024-06-15'),
('BIGSALE15', N'Siêu sale giữa năm', 1, 15, 800000, 150000, 300, '2024-06-10', '2024-06-20');


-- CHÈN DỮ LIỆU CHO CÁC BẢNG CÓ KHÓA NGOẠI
--==============================================================

-- 9. Bảng Hàng Hóa (Product)
PRINT '9. Inserting data into HangHoa...';
INSERT INTO HangHoa (TenHang, MoTa, IDLoaiHang, IDThuongHieu) VALUES
(N'Áo Thun Cổ Tròn Basic', N'Chất liệu cotton thoáng mát, phù hợp mặc hàng ngày.', 1, 1), -- IDLoaiHang: Áo Thun, IDThuongHieu: Uniqlo
(N'Quần Jeans Skinny Fit', N'Dáng ôm, co giãn tốt, tôn dáng người mặc.', 3, 4), -- IDLoaiHang: Quần Jeans, IDThuongHieu: Levi's
(N'Áo Sơ Mi Lụa Tay Dài', N'Vải lụa mềm mại, thiết kế thanh lịch cho môi trường công sở.', 2, 2), -- IDLoaiHang: Áo Sơ Mi, IDThuongHieu: Zara
(N'Váy Hoa Mùa Hè', N'Họa tiết hoa nhí, chất vải voan nhẹ nhàng, thích hợp đi biển, dạo phố.', 4, 3); -- IDLoaiHang: Váy, IDThuongHieu: H&M

-- 10. Bảng Địa Chỉ của Người Dùng (Linking Table)
PRINT '10. Inserting data into DiaChi_NguoiDung...';
INSERT INTO DiaChi_NguoiDung (IDNguoiDung, IDDiaChi, LaMacDinh) VALUES
(1, 1, 1), -- Nguyễn Văn An - Hà Nội (Mặc định)
(2, 2, 1), -- Trần Thị Bích - TP. HCM (Mặc định)
(3, 3, 1), -- Lê Minh Cường - Đà Nẵng (Mặc định)
(4, 4, 1); -- Phạm Thị Dung - Cần Thơ (Mặc định)

-- 11. Bảng Biến Thể Hàng Hóa (Product Variant)
PRINT '11. Inserting data into HangHoa_BienThe...';
INSERT INTO HangHoa_BienThe (IDHang, IDMauSac, IDKichThuoc, SKU, Gia, GiaKhuyenMai, SoLuongTon) VALUES
(1, 1, 1, 'UNI-AT-TR-S', 299000, NULL, 100), -- Áo Thun Uniqlo, Trắng, S
(1, 2, 2, 'UNI-AT-DE-M', 299000, 249000, 150), -- Áo Thun Uniqlo, Đen, M
(2, 3, 1, 'LEVI-QJ-NV-S', 1290000, NULL, 80), -- Quần Jeans Levi's, Xanh Navy, S
(3, 4, 2, 'ZARA-SM-BE-M', 899000, NULL, 50),  -- Áo Sơ Mi Zara, Beige, M
(4, 5, 5, 'HM-VH-DO-FS', 750000, 699000, 60); -- Váy Hoa H&M, Đỏ, Free Size

-- 12. Bảng Hình Ảnh của Biến Thể (Variant Image)
PRINT '12. Inserting data into HinhAnh_BienThe...';
INSERT INTO HinhAnh_BienThe (IDBienThe, DuongDan, LaAnhChinh) VALUES
(1, '/images/uni-at-tr-s-1.jpg', 1),
(1, '/images/uni-at-tr-s-2.jpg', 0),
(2, '/images/uni-at-de-m-1.jpg', 1),
(3, '/images/levi-qj-nv-s-1.jpg', 1),
(4, '/images/zara-sm-be-m-1.jpg', 1);

-- 13. Bảng Giỏ Hàng (Cart)
PRINT '13. Inserting data into GioHang...';
INSERT INTO GioHang (IDNguoiDung, IDBienThe, SoLuong) VALUES
(2, 1, 2), -- User Bích thêm 2 Áo thun Uniqlo Trắng S
(2, 3, 1), -- User Bích thêm 1 Quần Jeans Levi's Xanh Navy S
(3, 2, 1), -- User Cường thêm 1 Áo thun Uniqlo Đen M
(4, 4, 1); -- User Dung thêm 1 Áo sơ mi Zara Beige M

-- 14. Bảng Đơn Hàng (Order)
PRINT '14. Inserting data into DonHang...';
INSERT INTO DonHang (IDNguoiDung, IDGiamGia, TenNguoiNhan, DiaChiGiao, SoDienThoai, TongTienHang, PhiVanChuyen, GiamGia, TongThanhToan, IDPhuongThucThanhToan, TrangThai) VALUES
-- Đơn hàng 1: User 2, không dùng giảm giá
(2, NULL, N'Trần Thị Bích', N'22, Đường Nguyễn Huệ, Bến Nghé, Quận 1, TP. Hồ Chí Minh', '0987654321', 1589000.00, 30000.00, 0, 1619000.00, 1, N'Đã hoàn thành'),
-- Đơn hàng 2: User 3, dùng mã giảm giá WELCOME20
(3, 1, N'Lê Minh Cường', N'101, Đường 2 Tháng 9, Bình Hiên, Hải Châu, Đà Nẵng', '0905112233', 899000.00, 30000.00, 100000.00, 829000.00, 3, N'Đang giao'),
-- Đơn hàng 3: User 4, dùng mã FREESHIP
(4, 2, N'Phạm Thị Dung', N'50, Đại lộ Hòa Bình, An Phú, Ninh Kiều, Cần Thơ', '0334556677', 750000.00, 30000.00, 30000.00, 750000.00, 2, N'Chờ xác nhận'),
-- Đơn hàng 4: User 2, đơn hàng thứ 2
(2, NULL, N'Trần Thị Bích', N'22, Đường Nguyễn Huệ, Bến Nghé, Quận 1, TP. Hồ Chí Minh', '0987654321', 299000.00, 30000.00, 0, 329000.00, 1, N'Đã hủy');


-- 15. Bảng Chi Tiết Đơn Hàng (Order Detail)
PRINT '15. Inserting data into ChiTiet_DonHang...';
INSERT INTO ChiTiet_DonHang (IDDonHang, IDBienThe, SoLuong, DonGia) VALUES
-- Chi tiết cho đơn hàng 1
(1, 1, 1, 299000.00), -- Áo thun Uniqlo
(1, 2, 1, 299000.00),
(1, 3, 1, 1290000.00),
-- Chi tiết cho đơn hàng 2
(2, 4, 1, 899000.00), -- Áo sơ mi Zara
-- Chi tiết cho đơn hàng 3
(3, 5, 1, 750000.00), -- Váy hoa H&M
-- Chi tiết cho đơn hàng 4
(4, 1, 1, 299000.00); -- Áo thun Uniqlo
go