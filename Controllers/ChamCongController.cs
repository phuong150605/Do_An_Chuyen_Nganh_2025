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
    public class ChamCongController : ControllerBase
    {
            private readonly KetNoiCSDL _context;

            public ChamCongController(KetNoiCSDL context)
            {
                _context = context;
            }

            // GET: api/ChamCong
            [HttpGet]
            public async Task<ActionResult<IEnumerable<ChamCongDto>>> GetChamCongs()
            {
                // Lấy tất cả bản ghi ChamCong và chuyển đổi chúng sang ChamCongDto
                var chamCongs = await _context.ChamCongs
                    .Select(ch => new ChamCongDto
                    {
                        ChamcongId = ch.ChamcongId,
                        Ngay = (DateOnly)ch.Ngay,
                        Giovao = ch.Giovao,
                        Giora = ch.Giora,
                        NhanvienId = ch.NhanvienId,
                        Dimuon = ch.Dimuon,
                        Vesom = ch.Vesom
                    })
                    .ToListAsync();

                // Trả về danh sách ChamCongDto
                return Ok(chamCongs);
            }

            // GET: api/ChamCong/5
            [HttpGet("{id}")]
            public async Task<ActionResult<ChamCongDto>> GetChamCong(int id)
            {
                // Tìm ChamCong theo ID và chuyển thành ChamCongDto
                var chamCong = await _context.ChamCongs
                    .Where(c => c.ChamcongId == id)
                    .Select(ch => new ChamCongDto
                    {
                        ChamcongId = ch.ChamcongId,
                        Ngay = (DateOnly)ch.Ngay,
                        Giovao = ch.Giovao,
                        Giora = ch.Giora,
                        NhanvienId = ch.NhanvienId,
                        Dimuon = ch.Dimuon,
                        Vesom = ch.Vesom
                    })
                    .FirstOrDefaultAsync();

                // Kiểm tra nếu không tìm thấy bản ghi
                if (chamCong == null)
                {
                    return NotFound($"Không tìm thấy bản ghi ChamCong với ID {id}.");
                }

                // Trả về ChamCongDto nếu tìm thấy
                return Ok(chamCong);
            }


            // PUT: api/ChamCong/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutChamCong(int id, ChamCongDto chamCongDto)
            {
                // Kiểm tra xem ID trong URL có khớp với ID trong DTO không
                if (id != chamCongDto.ChamcongId)
                {
                    return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
                }

                // Tạo một đối tượng ChamCong từ DTO
                var chamCong = new ChamCong
                {
                    ChamcongId = chamCongDto.ChamcongId,
                    Ngay = chamCongDto.Ngay,
                    Giovao = chamCongDto.Giovao,
                    Giora = chamCongDto.Giora,
                    NhanvienId = chamCongDto.NhanvienId,
                    Dimuon = chamCongDto.Dimuon,
                    Vesom = chamCongDto.Vesom
                };

                // Thực hiện cập nhật bản ghi ChamCong trong cơ sở dữ liệu
                _context.Entry(chamCong).State = EntityState.Modified;

                try
                {
                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Kiểm tra nếu không tìm thấy bản ghi cần cập nhật
                    if (!ChamCongExists(id))
                    {
                        return NotFound($"Không tìm thấy ChamCong với ID {id}.");
                    }
                    else
                    {
                        throw;
                    }
                }

                // Trả về trạng thái NoContent nếu cập nhật thành công
                return NoContent();
            }


            // POST: api/ChamCong
            [HttpPost]
            public async Task<ActionResult<ChamCongDto>> PostChamCong(ChamCongDto chamCongDto)
            {
                // Kiểm tra xem dữ liệu có hợp lệ không
                if (chamCongDto == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                // Tạo đối tượng ChamCong từ DTO
                var chamCong = new ChamCong
                {
                    ChamcongId = chamCongDto.ChamcongId,
                    Ngay = chamCongDto.Ngay,
                    Giovao = chamCongDto.Giovao,
                    Giora = chamCongDto.Giora,
                    NhanvienId = chamCongDto.NhanvienId,
                    Dimuon = chamCongDto.Dimuon,
                    Vesom = chamCongDto.Vesom
                };

                // Thêm vào DbContext và lưu vào cơ sở dữ liệu
                _context.ChamCongs.Add(chamCong);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // Nếu gặp lỗi khi thêm dữ liệu, kiểm tra xem ChamCong đã tồn tại chưa
                    if (ChamCongExists(chamCong.ChamcongId))
                    {
                        return Conflict("Bản ghi ChamCong với ID đã tồn tại.");
                    }
                    else
                    {
                        throw;
                    }
                }

                // Trả về kết quả sau khi thêm thành công, bao gồm ID của bản ghi mới
                return CreatedAtAction("GetChamCong", new { id = chamCong.ChamcongId }, chamCong);
            }


            // DELETE: api/ChamCong/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteChamCong(int id)
            {
                // Tìm bản ghi ChamCong theo ID
                var chamCong = await _context.ChamCongs.FindAsync(id);

                // Nếu không tìm thấy bản ghi ChamCong, trả về NotFound
                if (chamCong == null)
                {
                    return NotFound($"Không tìm thấy ChamCong với ID {id}.");
                }

                // Xóa bản ghi ChamCong khỏi cơ sở dữ liệu
                _context.ChamCongs.Remove(chamCong);

                try
                {
                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // Nếu có lỗi khi xóa dữ liệu, trả về InternalServerError
                    return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi xóa bản ghi.");
                }

                // Trả về NoContent sau khi xóa thành công
                return NoContent();
            }

            private bool ChamCongExists(int id)
            {
                return _context.ChamCongs.Any(e => e.ChamcongId == id);
            }
        }
}
