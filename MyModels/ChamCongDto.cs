namespace QLNhanSu.MyModels

{
    public class ChamCongDto
    {
        public int ChamcongId { get; set; }
        public DateOnly Ngay { get; set; }
        public TimeOnly? Giovao { get; set; }
        public TimeOnly? Giora { get; set; }
        public string NhanvienId { get; set; } = null!;
        public string? Dimuon { get; set; }
        public string? Vesom { get; set; }
    }
}

