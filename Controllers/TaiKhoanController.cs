﻿using System;
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
    public class TaiKhoanController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public TaiKhoanController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/TaiKhoan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoanDto>>> GetTaiKhoans()
        {
            var taiKhoans = await _context.TaiKhoans.ToListAsync();

            if (taiKhoans == null || !taiKhoans.Any())
            {
                return NotFound("Không có tài khoản nào trong hệ thống.");
            }

            // Chuyển đổi từ entity TaiKhoan sang DTO (TaiKhoanDto) để trả về thông tin cần thiết
            var taiKhoansDto = taiKhoans.Select(t => new TaiKhoanDto
            {
                TaikhoanId = t.TaikhoanId,
                Tendangnhap = t.Tendangnhap,
                Matkhau = t.Matkhau,
                QuyenId = t.QuyenId,
                NhanvienId = t.NhanvienId
            }).ToList();

            return Ok(taiKhoansDto);
        }


        // PUT: api/TaiKhoan/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaiKhoan(int id, TaiKhoanDto taiKhoanDto)
        {
            // Kiểm tra xem ID trong URL có khớp với ID trong đối tượng truyền lên không
            if (id != taiKhoanDto.TaikhoanId)
            {
                return BadRequest("ID tài khoản trong URL không khớp với ID trong dữ liệu.");
            }

            // Chuyển đổi từ DTO sang entity TaiKhoan
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound($"Tài khoản với ID {id} không tồn tại.");
            }

            // Cập nhật các thuộc tính của tài khoản từ DTO
            taiKhoan.Tendangnhap = taiKhoanDto.Tendangnhap;
            taiKhoan.Matkhau = taiKhoanDto.Matkhau;
            taiKhoan.QuyenId = taiKhoanDto.QuyenId;
            taiKhoan.NhanvienId = taiKhoanDto.NhanvienId;

            _context.Entry(taiKhoan).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Xử lý trường hợp xảy ra lỗi khi cập nhật đồng thời (concurrency issue)
                if (!TaiKhoanExists(id))
                {
                    return NotFound($"Tài khoản với ID {id} không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            // Trả về 204 No Content nếu cập nhật thành công
            return NoContent();
        }


        // POST: api/TaiKhoan
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaiKhoan>> PostTaiKhoan(TaiKhoanDto taiKhoanDto)
        {
            // Kiểm tra xem tài khoản đã tồn tại hay chưa
            var existingTaiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(tk => tk.Tendangnhap == taiKhoanDto.Tendangnhap);

            if (existingTaiKhoan != null)
            {
                return Conflict("Tài khoản với tên đăng nhập này đã tồn tại.");
            }

            // Chuyển đổi từ DTO sang entity TaiKhoan
            var taiKhoan = new TaiKhoan
            {
                Tendangnhap = taiKhoanDto.Tendangnhap,
                Matkhau = taiKhoanDto.Matkhau,
                QuyenId = taiKhoanDto.QuyenId,
                NhanvienId = taiKhoanDto.NhanvienId
            };

            // Thêm tài khoản vào context
            _context.TaiKhoans.Add(taiKhoan);

            try
            {
                // Lưu tài khoản vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi lưu, trả về thông báo lỗi
                return StatusCode(500, $"Lỗi khi tạo tài khoản: {ex.Message}");
            }

            // Trả về tài khoản đã được tạo mới kèm theo URL để truy cập
            return Ok(taiKhoan);

        }


        // DELETE: api/TaiKhoan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaiKhoan(int id)
        {
            // Kiểm tra xem tài khoản có tồn tại trong cơ sở dữ liệu không
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                // Trường hợp không tìm thấy tài khoản với id này
                return NotFound($"Tài khoản với ID {id} không tồn tại.");
            }

            try
            {
                // Xóa tài khoản khỏi cơ sở dữ liệu
                _context.TaiKhoans.Remove(taiKhoan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trường hợp có lỗi khi xóa dữ liệu, ví dụ, vi phạm ràng buộc khóa ngoại
                return StatusCode(500, $"Lỗi khi xóa tài khoản: {ex.Message}");
            }

            // Trả về mã trạng thái NoContent (204) sau khi xóa thành công
            return NoContent();
        }

        // Phương thức kiểm tra xem tài khoản có tồn tại trong cơ sở dữ liệu không
        private bool TaiKhoanExists(int id)
        {
            return _context.TaiKhoans.Any(e => e.TaikhoanId == id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Login user)
        {
            // Tìm tài khoản theo tên đăng nhập
            var _user = (from tk in _context.TaiKhoans
                         where tk.Tendangnhap == user.tenDangNhap
                         select new
                         {
                             tk.TaikhoanId,
                             tk.Tendangnhap,
                             tk.Matkhau,
                             tk.NhanvienId,
                             tk.QuyenId,
                             role = _context.PhanQuyens
                                 .Where(x => x.QuyenId == tk.QuyenId)
                                 .Select(x => x.Loaiquyen)
                                 .FirstOrDefault() // Tránh lỗi nếu không tìm thấy quyền
                         }).FirstOrDefault(); // Chỉ lấy một tài khoản, tránh sử dụng `.ToList()` không cần thiết

            // Kiểm tra tài khoản có tồn tại không
            if (_user == null)
            {
                return Ok(new
                {
                    message = "Tài khoản không tồn tại",
                    status = 404
                });
            }

            // Kiểm tra mật khẩu có khớp không
            if (user.matKhau != _user.Matkhau) // Có thể dùng Hash mật khẩu nếu cần bảo mật
            {
                return Ok(new
                {
                    message = "Sai mật khẩu",
                    status = 400
                });
            }

            // Trả về kết quả thành công
            return Ok(new
            {
                message = "Thành công",
                status = 200,
                data = new
                {
                    _user.TaikhoanId,
                    _user.Tendangnhap,
                    _user.NhanvienId,
                    _user.QuyenId,
                    _user.role
                }
            });
        }
        public class Login
        {
            public string tenDangNhap { get; set; }
            public string matKhau { get; set; }
        }
    }
}
