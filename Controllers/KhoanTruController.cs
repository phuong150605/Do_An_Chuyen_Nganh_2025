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
    public class KhoanTruController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public KhoanTruController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/KhoanTru
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhoanTruDto>>> GetKhoanTrus()
        {
            var khoanTrus = await _context.KhoanTrus
                .Select(k => new KhoanTruDto
                {
                    TruId = k.TruId,
                    Loaikhoantru = k.Loaikhoantru,
                    Sotien = k.Sotien
                })
                .ToListAsync();

            if (khoanTrus == null || !khoanTrus.Any())
            {
                return NotFound("Không có khoản trừ nào trong hệ thống.");
            }

            return Ok(khoanTrus);
        }

        // GET: api/KhoanTru/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KhoanTruDto>> GetKhoanTru(string id)
        {
            var khoanTru = await _context.KhoanTrus
                .Where(k => k.TruId == id)
                .Select(k => new KhoanTruDto
                {
                    TruId = k.TruId,
                    Loaikhoantru = k.Loaikhoantru,
                    Sotien = k.Sotien
                })
                .FirstOrDefaultAsync();

            if (khoanTru == null)
            {
                return NotFound($"Khoản trừ với ID {id} không tồn tại.");
            }

            return Ok(khoanTru);
        }


        // PUT: api/KhoanTru/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhoanTru(string id, KhoanTruDto khoanTruDto)
        {
            // Kiểm tra nếu ID trong URL không khớp với ID trong dữ liệu DTO
            if (id != khoanTruDto.TruId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Tìm khoản trừ trong cơ sở dữ liệu
            var khoanTru = await _context.KhoanTrus.FindAsync(id);

            // Nếu không tìm thấy khoản trừ, trả về lỗi NotFound
            if (khoanTru == null)
            {
                return NotFound($"Khoản trừ với ID {id} không tồn tại.");
            }

            // Cập nhật các thông tin của khoản trừ từ DTO
            khoanTru.Loaikhoantru = khoanTruDto.Loaikhoantru;
            khoanTru.Sotien = khoanTruDto.Sotien;

            // Đánh dấu đối tượng đã được thay đổi
            _context.Entry(khoanTru).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra lại nếu khoản trừ không tồn tại
                if (!KhoanTruExists(id))
                {
                    return NotFound($"Khoản trừ với ID {id} không tồn tại.");
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

        // POST: api/KhoanTru
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KhoanTru>> PostKhoanTru(KhoanTruDto khoanTruDto)
        {
            if (khoanTruDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Chuyển đổi từ DTO sang Entity KhoanTru
            var khoanTru = new KhoanTru
            {
                TruId = khoanTruDto.TruId,
                Loaikhoantru = khoanTruDto.Loaikhoantru,
                Sotien = khoanTruDto.Sotien
            };

            _context.KhoanTrus.Add(khoanTru);

            try
            {
                // Lưu khoản trừ vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Nếu có ngoại lệ khi lưu, kiểm tra sự tồn tại của khoản trừ với cùng ID (nếu có)
                if (KhoanTruExists(khoanTru.TruId))
                {
                    return Conflict($"Khoản trừ với ID {khoanTru.TruId} đã tồn tại.");
                }
                else
                {
                    // Nếu có lỗi khác, ném ngoại lệ
                    throw;
                }
            }

            // Trả về kết quả với thông tin khoản trừ vừa được tạo
            return CreatedAtAction("GetKhoanTru", new { id = khoanTru.TruId }, khoanTru);
        }

        // DELETE: api/KhoanTru/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhoanTru(string id)
        {
            // Tìm khoản trừ trong cơ sở dữ liệu theo ID
            var khoanTru = await _context.KhoanTrus.FindAsync(id);

            // Nếu không tìm thấy khoản trừ, trả về lỗi NotFound với thông báo chi tiết
            if (khoanTru == null)
            {
                return NotFound($"Khoản trừ với ID {id} không tồn tại.");
            }

            // Xóa khoản trừ khỏi cơ sở dữ liệu
            _context.KhoanTrus.Remove(khoanTru);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trả về lỗi 500 nếu gặp phải vấn đề trong quá trình xóa
                return StatusCode(500, $"Lỗi khi xóa khoản trừ: {ex.Message}");
            }

            // Trả về thông báo thành công khi xóa
            return NoContent();
        }

        private bool KhoanTruExists(string id)
        {
            return _context.KhoanTrus.Any(e => e.TruId == id);
        }
    }
}
