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
    public class CtNhanvienPhucapController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public CtNhanvienPhucapController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/CtNhanvienPhucap
        [HttpGet]
        public async Task<ActionResult> GetCtNhanvienPhucaps()
        {
            if (_context.CtNhanvienPhucaps == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienPhucaps
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join pc in _context.PhuCaps on ct.PhucapId equals pc.PhucapId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            pc.Loaiphucap, // Hiển thị tên phụ cấp thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có phụ cấp nào cho nhân viên!",
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


        // PUT: api/CtNhanvienPhucap/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCtNhanvienPhucap(string id, CtNhanvienPhucapDto ctNhanvienPhucapDto)
        {
            // Kiểm tra nếu ID trong URL không khớp với ID trong dữ liệu DTO
            if (id != ctNhanvienPhucapDto.NhanvienId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Tìm khoản phụ cấp trong cơ sở dữ liệu
            var ctNhanvienPhucap = await _context.CtNhanvienPhucaps.FindAsync(id);

            // Nếu không tìm thấy, trả về lỗi NotFound
            if (ctNhanvienPhucap == null)
            {
                return NotFound($"Khoản phụ cấp của nhân viên với ID {id} không tồn tại.");
            }

            // Cập nhật các thông tin của khoản phụ cấp từ DTO
            ctNhanvienPhucap.PhucapId = ctNhanvienPhucapDto.PhucapId;
            ctNhanvienPhucap.Thoigian = ctNhanvienPhucapDto.Thoigian;

            // Đánh dấu đối tượng đã thay đổi
            _context.Entry(ctNhanvienPhucap).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra lại nếu khoản phụ cấp không tồn tại
                if (!CtNhanvienPhucapExists(id))
                {
                    return NotFound($"Khoản phụ cấp của nhân viên với ID {id} không tồn tại.");
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

        // POST: api/CtNhanvienPhucap
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CtNhanvienPhucapDto>> PostCtNhanvienPhucap(CtNhanvienPhucapDto ctNhanvienPhucapDto)
        {
            // Kiểm tra nếu dữ liệu đầu vào là null
            if (ctNhanvienPhucapDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Chuyển đổi từ DTO sang Entity CtNhanvienPhucap
            var ctNhanvienPhucap = new CtNhanvienPhucap
            {
                NhanvienId = ctNhanvienPhucapDto.NhanvienId,
                PhucapId = ctNhanvienPhucapDto.PhucapId,
                Thoigian = ctNhanvienPhucapDto.Thoigian
            };

            // Thêm khoản phụ cấp vào cơ sở dữ liệu
            _context.CtNhanvienPhucaps.Add(ctNhanvienPhucap);
            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Kiểm tra nếu đã có khoản phụ cấp cho nhân viên
                if (CtNhanvienPhucapExists(ctNhanvienPhucap.NhanvienId))
                {
                    return Conflict($"Khoản phụ cấp của nhân viên với ID {ctNhanvienPhucap.NhanvienId} đã tồn tại.");
                }
                else
                {
                    // Ném ngoại lệ nếu có lỗi khác
                    throw;
                }
            }

            // Trả về mã 201 Created và thông tin của khoản phụ cấp vừa được thêm
            return CreatedAtAction("GetCtNhanvienPhucap", new { id = ctNhanvienPhucap.NhanvienId }, ctNhanvienPhucapDto);
        }

        // DELETE: api/CtNhanvienPhucap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCtNhanvienPhucap(string id)
        {
            // Tìm kiếm khoản phụ cấp theo ID
            var ctNhanvienPhucap = await _context.CtNhanvienPhucaps.FindAsync(id);

            // Nếu không tìm thấy, trả về lỗi NotFound
            if (ctNhanvienPhucap == null)
            {
                return NotFound($"Khoản phụ cấp của nhân viên với ID {id} không tồn tại.");
            }

            // Xóa khoản phụ cấp
            _context.CtNhanvienPhucaps.Remove(ctNhanvienPhucap);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trả về lỗi với thông báo chi tiết nếu gặp vấn đề khi xóa
                return StatusCode(500, $"Lỗi khi xóa khoản phụ cấp: {ex.Message}");
            }

            // Trả về kết quả thành công, không có nội dung
            return NoContent();
        }


        private bool CtNhanvienPhucapExists(string id)
        {
            return _context.CtNhanvienPhucaps.Any(e => e.NhanvienId == id);
        }
    }
}
