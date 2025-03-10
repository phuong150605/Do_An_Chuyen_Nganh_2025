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
    public class PhuCapController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public PhuCapController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/PhuCap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhuCapDto>>> GetPhuCaps()
        {
            try
            {
                var phuCaps = await _context.PhuCaps
                    .Select(p => new PhuCapDto
                    {
                        PhucapId = p.PhucapId,
                        Loaiphucap = p.Loaiphucap,
                        Sotien = p.Sotien
                    }).ToListAsync();

                if (phuCaps == null || !phuCaps.Any())
                {
                    return NotFound("Không có phụ cấp nào trong hệ thống.");
                }

                return Ok(phuCaps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi truy xuất dữ liệu: {ex.Message}");
            }
        }

        // GET: api/PhuCap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhuCapDto>> GetPhuCap(string id)
        {
            var phuCap = await _context.PhuCaps
                .Where(p => p.PhucapId == id)
                .Select(p => new PhuCapDto
                {
                    PhucapId = p.PhucapId,
                    Loaiphucap = p.Loaiphucap,
                    Sotien = p.Sotien
                })
                .FirstOrDefaultAsync();

            if (phuCap == null)
            {
                return NotFound($"Không tìm thấy phụ cấp với ID: {id}");
            }

            return Ok(phuCap);
        }


        // PUT: api/PhuCap/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhuCap(string id, PhuCapDto phuCapDto)
        {
            if (id != phuCapDto.PhucapId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            var phuCap = await _context.PhuCaps.FindAsync(id);
            if (phuCap == null)
            {
                return NotFound($"Không tìm thấy phụ cấp với ID: {id}");
            }

            phuCap.Loaiphucap = phuCapDto.Loaiphucap;
            phuCap.Sotien = phuCapDto.Sotien;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhuCapExists(id))
                {
                    return NotFound($"Phụ cấp với ID: {id} không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // POST: api/PhuCap
        [HttpPost]
        public async Task<ActionResult<PhuCapDto>> PostPhuCap(PhuCapDto phuCapDto)
        {
            try
            {
                var phuCap = new PhuCap
                {
                    PhucapId = phuCapDto.PhucapId,
                    Loaiphucap = phuCapDto.Loaiphucap,
                    Sotien = phuCapDto.Sotien
                };

                _context.PhuCaps.Add(phuCap);
                await _context.SaveChangesAsync();

                var createdPhuCapDto = new PhuCapDto
                {
                    PhucapId = phuCap.PhucapId,
                    Loaiphucap = phuCap.Loaiphucap,
                    Sotien = phuCap.Sotien
                };

                return CreatedAtAction("GetPhuCap", new { id = phuCap.PhucapId }, createdPhuCapDto);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Lỗi khi tạo phụ cấp: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi lưu dữ liệu: {ex.Message}");
            }
        }



        // DELETE: api/PhuCap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhuCap(string id)
        {
            var phuCap = await _context.PhuCaps.FindAsync(id);
            if (phuCap == null)
            {
                return NotFound($"Không tìm thấy phụ cấp với ID: {id}");
            }

            _context.PhuCaps.Remove(phuCap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhuCapExists(string id)
        {
            return _context.PhuCaps.Any(e => e.PhucapId == id);
        }

    }
}
