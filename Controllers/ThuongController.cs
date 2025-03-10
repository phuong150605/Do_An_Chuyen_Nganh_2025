using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuongController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public ThuongController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/Thuong
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThuongDto>>> GetThuongs()
        {
            var thuongs = await _context.Thuongs.ToListAsync();

            // Kiểm tra nếu không có dữ liệu
            if (thuongs == null || thuongs.Count == 0)
            {
                return NotFound("Không tìm thấy dữ liệu Thưởng nào.");
            }

            // Chuyển đổi từ entity Thuong sang ThuongDto để chỉ trả về thông tin cần thiết
            var thuongDtos = thuongs.Select(t => new ThuongDto
            {
                ThuongId = t.ThuongId,
                Loaithuong = t.Loaithuong,
                Sotien = t.Sotien
            }).ToList();

            return Ok(thuongDtos);
        }

        // GET: api/Thuong/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThuongDto>> GetThuong(string id)
        {
            // Tìm Thưởng theo ID
            var thuong = await _context.Thuongs.FindAsync(id);

            // Kiểm tra nếu không tìm thấy
            if (thuong == null)
            {
                return NotFound($"Không tìm thấy dữ liệu Thưởng với ID: {id}");
            }

            // Chuyển đổi từ entity Thuong sang ThuongDto
            var thuongDto = new ThuongDto
            {
                ThuongId = thuong.ThuongId,
                Loaithuong = thuong.Loaithuong,
                Sotien = thuong.Sotien
            };

            return Ok(thuongDto);
        }


        // PUT: api/Thuong/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThuong(string id, [FromBody] ThuongDto thuongDto)
        {
            if (id != thuongDto.ThuongId)
            {
                return BadRequest("ID trong URL không khớp với ID của Thưởng.");
            }

            // Chuyển đổi từ DTO sang entity Thuong
            var thuong = new Thuong
            {
                ThuongId = thuongDto.ThuongId,
                Loaithuong = thuongDto.Loaithuong,
                Sotien = thuongDto.Sotien
            };

            // Cập nhật trạng thái của entity
            _context.Entry(thuong).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThuongExists(id))
                {
                    // Nếu không tìm thấy Thưởng với ID
                    return NotFound($"Không tìm thấy dữ liệu Thưởng với ID: {id}");
                }
                else
                {
                    // Nếu có lỗi khác trong quá trình cập nhật
                    throw;
                }
            }

            // Nếu thành công, trả về thông báo NoContent (204)
            return NoContent();
        }


        // POST: api/Thuong
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Thuong
        [HttpPost]
        public async Task<ActionResult<Thuong>> PostThuong(ThuongDto thuongDto)
        {
            // Kiểm tra nếu thuongDto là null
            if (thuongDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ. Vui lòng kiểm tra lại dữ liệu bạn gửi.");
            }

            // Chuyển đổi từ DTO sang entity Thuong
            var thuong = new Thuong
            {
                ThuongId = thuongDto.ThuongId,
                Loaithuong = thuongDto.Loaithuong,
                Sotien = thuongDto.Sotien
            };

            // Thêm đối tượng Thuong vào DbContext
            _context.Thuongs.Add(thuong);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Kiểm tra nếu có lỗi trùng lặp (Conflict)
                if (ThuongExists(thuong.ThuongId))
                {
                    return Conflict($"Thuong với mã ID {thuong.ThuongId} đã tồn tại.");
                }
                else
                {
                    // Trường hợp lỗi không xác định
                    return StatusCode(500, $"Đã xảy ra lỗi khi lưu dữ liệu: {ex.Message}");
                }
            }

            // Trả về thông tin đối tượng vừa tạo thành công
            return CreatedAtAction("GetThuong", new { id = thuong.ThuongId }, thuong);
        }




        // DELETE: api/Thuong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThuong(string id)
        {
            var thuong = await _context.Thuongs.FindAsync(id);
            if (thuong == null)
            {
                return NotFound();
            }

            _context.Thuongs.Remove(thuong);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThuongExists(string id)
        {
            return _context.Thuongs.Any(e => e.ThuongId == id);
        }
    }
}
