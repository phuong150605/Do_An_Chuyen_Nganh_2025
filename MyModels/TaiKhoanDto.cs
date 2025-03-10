namespace QLNhanSu.MyModels
{
        public class TaiKhoanDto
        {
            public int TaikhoanId { get; set; }
            public string Tendangnhap { get; set; } = null!;
            public string Matkhau { get; set; } = null!;
            public string QuyenId { get; set; } = null!;
            public string NhanvienId { get; set; } = null!;
        }
}
