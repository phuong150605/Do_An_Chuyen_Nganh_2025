namespace QLNhanSu.MyModels

{
    public class LuongDto
    {
        public int LuongId { get; set; }
        public DateOnly Thoigian { get; set; }
        public string NhanvienId { get; set; } = null!;
        public decimal tongluong { get; set; }
    }
}

