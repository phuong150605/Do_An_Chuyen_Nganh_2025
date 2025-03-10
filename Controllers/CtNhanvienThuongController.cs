using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;

namespace DACN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CtNhanvienThuongController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public CtNhanvienThuongController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/CtNhanvienThuong
        [HttpGet]
        public async Task<ActionResult> GetCtNhanvienThuongs()
        {
            if (_context.CtNhanvienThuongs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienThuongs
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join t in _context.Thuongs on ct.ThuongId equals t.ThuongId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            t.Loaithuong, // Hiển thị tên loại thưởng thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có khoản thưởng nào cho nhân viên!",
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
        [Route("Search")]
        public async Task<ActionResult> TimKiemCtNhanvienThuong(string s)
        {
            if (_context.CtNhanvienThuongs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienThuongs
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join t in _context.Thuongs on ct.ThuongId equals t.ThuongId
                        where nv.Tennhanvien.Contains(s) ||
                              t.Loaithuong.Contains(s) ||
                              ct.NhanvienId.Contains(s) ||
                              ct.Thoigian.ToString().Contains(s)
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            t.Loaithuong, // Hiển thị tên loại thưởng thay vì ID
                            ct.Thoigian
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
        // PUT: api/CtNhanvienThuong/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCtNhanvienThuong(string id, CtNhanvienThuongDto ctNhanvienThuongDto)
        {
            // Kiểm tra nếu ID trong URL không khớp với ID trong dữ liệu DTO
            if (id != ctNhanvienThuongDto.NhanvienId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Tìm khoản thưởng trong cơ sở dữ liệu
            var ctNhanvienThuong = await _context.CtNhanvienThuongs.FindAsync(id);

            // Nếu không tìm thấy khoản thưởng, trả về lỗi NotFound
            if (ctNhanvienThuong == null)
            {
                return NotFound($"Khoản thưởng của nhân viên với ID {id} không tồn tại.");
            }

            // Cập nhật các thông tin của khoản thưởng từ DTO
            ctNhanvienThuong.ThuongId = ctNhanvienThuongDto.ThuongId;
            ctNhanvienThuong.Thoigian = ctNhanvienThuongDto.Thoigian;

            // Đánh dấu đối tượng đã được thay đổi
            _context.Entry(ctNhanvienThuong).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra lại nếu khoản thưởng không tồn tại
                if (!CtNhanvienThuongExists(id))
                {
                    return NotFound($"Khoản thưởng của nhân viên với ID {id} không tồn tại.");
                }
                else
                {
                    // Ném ra lỗi nếu gặp phải ngoại lệ đồng thời
                    throw;
                }
            }

            // Trả về kết quả thành công khi cập nhật dữ liệu
            return NoContent();
        }


        // POST: api/CtNhanvienThuong
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CtNhanvienThuong>> PostCtNhanvienThuong(CtNhanvienThuongDto ctNhanvienThuongDto)
        {
            if (ctNhanvienThuongDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Chuyển đổi từ DTO sang Entity CtNhanvienThuong
            var ctNhanvienThuong = new CtNhanvienThuong
            {
                NhanvienId = ctNhanvienThuongDto.NhanvienId,
                ThuongId = ctNhanvienThuongDto.ThuongId,
                Thoigian = ctNhanvienThuongDto.Thoigian
            };

            _context.CtNhanvienThuongs.Add(ctNhanvienThuong);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Kiểm tra nếu đã tồn tại khoản thưởng cho nhân viên
                if (CtNhanvienThuongExists(ctNhanvienThuong.NhanvienId))
                {
                    return Conflict($"Khoản thưởng của nhân viên với ID {ctNhanvienThuong.NhanvienId} đã tồn tại.");
                }
                else
                {
                    // Ném ngoại lệ nếu gặp lỗi khác
                    throw;
                }
            }

            // Trả về kết quả với thông tin khoản thưởng vừa được tạo
            return CreatedAtAction("GetCtNhanvienThuong", new { id = ctNhanvienThuong.NhanvienId }, ctNhanvienThuong);
        }

        // DELETE: api/CtNhanvienThuong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCtNhanvienThuong(string id)
        {
            // Tìm kiếm khoản thưởng theo ID
            var ctNhanvienThuong = await _context.CtNhanvienThuongs.FindAsync(id);

            // Nếu không tìm thấy, trả về lỗi NotFound
            if (ctNhanvienThuong == null)
            {
                return NotFound($"Khoản thưởng của nhân viên với ID {id} không tồn tại.");
            }

            // Xóa khoản thưởng
            _context.CtNhanvienThuongs.Remove(ctNhanvienThuong);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trả về lỗi với thông báo chi tiết nếu gặp vấn đề khi xóa
                return StatusCode(500, $"Lỗi khi xóa khoản thưởng: {ex.Message}");
            }

            // Trả về kết quả thành công, không có nội dung
            return NoContent();
        }


        private bool CtNhanvienThuongExists(string id)
        {
            return _context.CtNhanvienThuongs.Any(e => e.NhanvienId == id);
        }
    }
}
