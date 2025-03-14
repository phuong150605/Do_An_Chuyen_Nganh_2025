USE [QLNhanSu]
GO
/****** Object:  Table [dbo].[ChamCong]    Script Date: 3/11/2025 4:40:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChamCong](
	[chamcong_id] [int] IDENTITY(1,1) NOT NULL,
	[ngay] [date] NULL,
	[giovao] [time](7) NULL,
	[giora] [time](7) NULL,
	[nhanvien_id] [char](9) NOT NULL,
	[dimuon] [nvarchar](10) NULL,
	[vesom] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[chamcong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_nhanvien_khoantru]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_nhanvien_khoantru](
	[nhanvien_id] [char](9) NOT NULL,
	[tru_id] [char](4) NOT NULL,
	[thoigian] [date] NOT NULL,
 CONSTRAINT [PK_CT_nhanvien_khoantru] PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC,
	[tru_id] ASC,
	[thoigian] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_nhanvien_phucap]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_nhanvien_phucap](
	[nhanvien_id] [char](9) NOT NULL,
	[phucap_id] [char](4) NOT NULL,
	[thoigian] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC,
	[phucap_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_nhanvien_thuong]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_nhanvien_thuong](
	[nhanvien_id] [char](9) NOT NULL,
	[thuong_id] [char](4) NOT NULL,
	[thoigian] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC,
	[thuong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhoanTru]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhoanTru](
	[tru_id] [char](4) NOT NULL,
	[loaikhoantru] [nvarchar](200) NOT NULL,
	[sotien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tru_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Luong]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Luong](
	[luong_id] [int] IDENTITY(1,1) NOT NULL,
	[thoigian] [date] NOT NULL,
	[nhanvien_id] [char](9) NOT NULL,
	[tongluong] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[luong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[nhanvien_id] [char](9) NOT NULL,
	[tennhanvien] [nvarchar](50) NOT NULL,
	[gioitinh] [nvarchar](10) NOT NULL,
	[ngaysinh] [date] NOT NULL,
	[diachi] [varchar](80) NOT NULL,
	[sdt] [varchar](10) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[luongcoban] [decimal](10, 2) NOT NULL,
	[phongban_id] [char](3) NOT NULL,
	[ngayvaolam] [date] NOT NULL,
	[chucvu] [nvarchar](50) NOT NULL,
	[trangthailv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__NhanVien__4B5DC987A248DF01] PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__NhanVien__AB6E61643500110F] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__NhanVien__DDDFB48352849C26] UNIQUE NONCLUSTERED 
(
	[sdt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhanQuyen]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanQuyen](
	[quyen_id] [char](4) NOT NULL,
	[loaiquyen] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[quyen_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[loaiquyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhongBan]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongBan](
	[phongban_id] [char](3) NOT NULL,
	[tenphongban] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[phongban_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[tenphongban] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhuCap]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhuCap](
	[phucap_id] [char](4) NOT NULL,
	[loaiphucap] [nvarchar](200) NOT NULL,
	[sotien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[phucap_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[taikhoan_id] [int] IDENTITY(1,1) NOT NULL,
	[tendangnhap] [varchar](50) NOT NULL,
	[matkhau] [varchar](80) NOT NULL,
	[quyen_id] [char](4) NOT NULL,
	[nhanvien_id] [char](9) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[taikhoan_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[matkhau] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[tendangnhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thuong]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thuong](
	[thuong_id] [char](4) NOT NULL,
	[loaithuong] [nvarchar](100) NOT NULL,
	[sotien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[thuong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_khoantru_khoantru] FOREIGN KEY([tru_id])
REFERENCES [dbo].[KhoanTru] ([tru_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru] CHECK CONSTRAINT [FK_CT_nhanvien_khoantru_khoantru]
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_khoantru_nhanvien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru] CHECK CONSTRAINT [FK_CT_nhanvien_khoantru_nhanvien]
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_phucap_nhanvien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap] CHECK CONSTRAINT [FK_CT_nhanvien_phucap_nhanvien]
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_phucap_phucap] FOREIGN KEY([phucap_id])
REFERENCES [dbo].[PhuCap] ([phucap_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap] CHECK CONSTRAINT [FK_CT_nhanvien_phucap_phucap]
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_thuong_nhanvien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong] CHECK CONSTRAINT [FK_CT_nhanvien_thuong_nhanvien]
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_thuong_thuong] FOREIGN KEY([thuong_id])
REFERENCES [dbo].[Thuong] ([thuong_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong] CHECK CONSTRAINT [FK_CT_nhanvien_thuong_thuong]
GO
ALTER TABLE [dbo].[Luong]  WITH CHECK ADD  CONSTRAINT [FK_Luong_NhanVien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Luong] CHECK CONSTRAINT [FK_Luong_NhanVien]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_NhanVien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_NhanVien]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_PhanQuyen] FOREIGN KEY([quyen_id])
REFERENCES [dbo].[PhanQuyen] ([quyen_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_PhanQuyen]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [CK__NhanVien__gioiti__6EF57B66] CHECK  (([gioitinh]=N'Khác' OR [gioitinh]=N'Nữ' OR [gioitinh]=N'Nam'))
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [CK__NhanVien__gioiti__6EF57B66]
GO
