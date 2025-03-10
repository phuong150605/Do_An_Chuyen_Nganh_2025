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
    public class CtNhanvienKhoantruController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public CtNhanvienKhoantruController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/CtNhanvienKhoantru
        [HttpGet]
        public async Task<ActionResult> GetCtNhanvienKhoantrus()
        {
            if (_context.CtNhanvienKhoantrus == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienKhoantrus
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join tru in _context.KhoanTrus on ct.TruId equals tru.TruId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            tru.Loaikhoantru, // Hiển thị tên khoản trừ thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có khoản trừ nào cho nhân viên!",
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
        public async Task<ActionResult> TimKiemCtNhanvienKhoantru(string s)
        {
            if (_context.CtNhanvienKhoantrus == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienKhoantrus
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join tru in _context.KhoanTrus on ct.TruId equals tru.TruId
                        where nv.Tennhanvien.Contains(s) ||
                              tru.Loaikhoantru.Contains(s) ||
                              ct.NhanvienId.Contains(s) ||
                              ct.Thoigian.ToString().Contains(s)
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            tru.Loaikhoantru, // Hiển thị tên khoản trừ thay vì ID
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

        // PUT: api/CtNhanvienKhoantru/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCtNhanvienKhoantru(string id, CtNhanvienKhoantruDto ctNhanvienKhoantruDto)
        {
            // Kiểm tra nếu ID trong URL không khớp với ID trong dữ liệu
            if (id != ctNhanvienKhoantruDto.NhanvienId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Tìm khoản trừ nhân viên theo ID
            var ctNhanvienKhoantru = await _context.CtNhanvienKhoantrus.FindAsync(id);

            // Nếu không tìm thấy, trả về lỗi NotFound
            if (ctNhanvienKhoantru == null)
            {
                return NotFound($"Khoản trừ của nhân viên với ID {id} không tồn tại.");
            }

            // Cập nhật thông tin khoản trừ từ DTO
            ctNhanvienKhoantru.TruId = ctNhanvienKhoantruDto.TruId;
            ctNhanvienKhoantru.Thoigian = ctNhanvienKhoantruDto.Thoigian;

            // Đánh dấu trạng thái của entity là Modified để cập nhật
            _context.Entry(ctNhanvienKhoantru).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra xem có bị lỗi khi đồng bộ dữ liệu không
                if (!CtNhanvienKhoantruExists(id))
                {
                    return NotFound($"Khoản trừ của nhân viên với ID {id} không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            // Trả về thành công mà không có nội dung (NoContent)
            return NoContent();
        }

        // POST: api/CtNhanvienKhoantru
        [HttpPost]
        public async Task<ActionResult<CtNhanvienKhoantru>> PostCtNhanvienKhoantru(CtNhanvienKhoantruDto ctNhanvienKhoantruDto)
        {
            // Chuyển đổi từ DTO sang entity CtNhanvienKhoantru
            var ctNhanvienKhoantru = new CtNhanvienKhoantru
            {
                NhanvienId = ctNhanvienKhoantruDto.NhanvienId,
                TruId = ctNhanvienKhoantruDto.TruId,
                Thoigian = ctNhanvienKhoantruDto.Thoigian
            };

            // Thêm khoản trừ mới vào cơ sở dữ liệu
            _context.CtNhanvienKhoantrus.Add(ctNhanvienKhoantru);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Kiểm tra nếu đã tồn tại khoản trừ cho nhân viên này
                if (CtNhanvienKhoantruExists(ctNhanvienKhoantru.NhanvienId))
                {
                    return Conflict("Khoản trừ của nhân viên này đã tồn tại.");
                }
                else
                {
                    throw;  // Ném lại lỗi nếu gặp vấn đề khác
                }
            }

            // Trả về mã trạng thái Created và thông tin về khoản trừ mới thêm
            return CreatedAtAction("GetCtNhanvienKhoantru", new { id = ctNhanvienKhoantru.NhanvienId }, ctNhanvienKhoantru);
        }


        // DELETE: api/CtNhanvienKhoantru/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCtNhanvienKhoantru(string id)
        {
            // Tìm khoản trừ theo ID (NhanvienId)
            var ctNhanvienKhoantru = await _context.CtNhanvienKhoantrus.FindAsync(id);

            // Kiểm tra nếu không tìm thấy khoản trừ, trả về NotFound
            if (ctNhanvienKhoantru == null)
            {
                return NotFound($"Khoản trừ của nhân viên với ID {id} không tồn tại.");
            }

            // Xóa khoản trừ
            _context.CtNhanvienKhoantrus.Remove(ctNhanvienKhoantru);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có vấn đề trong quá trình xóa
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi xóa khoản trừ: {ex.Message}");
            }

            // Trả về thành công, không có nội dung (NoContent)
            return NoContent();
        }
        private bool CtNhanvienKhoantruExists(string id)
        {
            return _context.CtNhanvienKhoantrus.Any(e => e.NhanvienId == id);
        }
    }
}
