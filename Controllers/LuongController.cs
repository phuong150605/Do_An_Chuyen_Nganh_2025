using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLNhanSu.MyModels;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuongController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public LuongController(KetNoiCSDL context)
        {
            _context = context;
        }

        private decimal TinhLuong(NhanVien nhanVien)
        {
            decimal luongCoBan = nhanVien.Luongcoban;

            decimal tongThuong = nhanVien.CtNhanvienThuongs?
                .Where(t => t.NhanvienId == nhanVien.NhanvienId)
                .Sum(t => t.Thuong?.Sotien ?? 0) ?? 0;

            decimal tongPhuCap = nhanVien.CtNhanvienPhucaps?
                .Where(p => p.NhanvienId == nhanVien.NhanvienId)
                .Sum(p => p.Phucap?.Sotien ?? 0) ?? 0;

            decimal tongKhoanTru = nhanVien.CtNhanvienKhoantrus?
                .Where(k => k.NhanvienId == nhanVien.NhanvienId)
                .Sum(k => k.Tru?.Sotien ?? 0) ?? 0;

            decimal thue = luongCoBan * 0.1m;

            return luongCoBan + tongThuong + tongPhuCap - thue - tongKhoanTru;
        }

        [HttpPost]
        public async Task<ActionResult<LuongDto>> PostLuong()
        {
            try
            {
                var nhanViens = await _context.NhanViens
                    .Include(nv => nv.CtNhanvienThuongs).ThenInclude(ct => ct.Thuong)
                    .Include(nv => nv.CtNhanvienPhucaps).ThenInclude(ct => ct.Phucap)
                    .Include(nv => nv.CtNhanvienKhoantrus).ThenInclude(ct => ct.Tru)
                    .ToListAsync();

                if (nhanViens == null || !nhanViens.Any())
                {
                    return NotFound("Không có nhân viên nào trong hệ thống.");
                }

                foreach (var nv in nhanViens)
                {
                    decimal tongLuong = TinhLuong(nv);
                    var luong = await _context.Luongs.FirstOrDefaultAsync(l => l.NhanvienId == nv.NhanvienId);

                    if (luong != null)
                    {
                        luong.Tongluong = tongLuong;
                        luong.Thoigian = DateOnly.FromDateTime(DateTime.Now);
                        _context.Entry(luong).State = EntityState.Modified;
                    }
                    else
                    {
                        _context.Luongs.Add(new Luong
                        {
                            NhanvienId = nv.NhanvienId,
                            Thoigian = DateOnly.FromDateTime(DateTime.Now),
                            Tongluong = tongLuong
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return Ok("Lương của nhân viên đã được cập nhật thành công.");
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Lỗi khi cập nhật lương: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lưu dữ liệu: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLuongs()
        {
            if (_context.Luongs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from l in _context.Luongs
                        join nv in _context.NhanViens on l.NhanvienId equals nv.NhanvienId
                        join pb in _context.PhongBans on nv.PhongbanId equals pb.PhongbanId
                        orderby l.Thoigian descending
                        select new
                        {
                            l.NhanvienId,
                            nv.Tennhanvien,
                            pb.Tenphongban,
                            l.Thoigian,
                            l.Tongluong
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có dữ liệu lương trong hệ thống!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }

        [HttpGet]
        [Route("Luong/Search")]
        public async Task<ActionResult> TimKiemLuong(string s)
        {
            if (_context.Luongs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from l in _context.Luongs
                        join nv in _context.NhanViens on l.NhanvienId equals nv.NhanvienId
                        join pb in _context.PhongBans on nv.PhongbanId equals pb.PhongbanId
                        where nv.Tennhanvien.Contains(s) || pb.Tenphongban.Contains(s) || l.NhanvienId.Contains(s)
                        orderby l.Thoigian descending
                        select new
                        {
                            l.NhanvienId,
                            nv.Tennhanvien,
                            pb.Tenphongban,
                            l.Thoigian,
                            l.Tongluong
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không tìm thấy kết quả phù hợp!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLuong(int id)
        {
            var luong = await _context.Luongs.FindAsync(id);

            if (luong == null)
            {
                return NotFound($"Lương với ID {id} không tồn tại.");
            }

            _context.Luongs.Remove(luong);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Lỗi khi xóa lương: {ex.Message}");
            }

            return NoContent();
        }

    }
}
