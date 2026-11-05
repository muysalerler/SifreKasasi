using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SifreKasasi.API.DTOs;
using SifreKasasi.Business.Services;
using SifreKasasi.Data;
using SifreKasasi.Data.Entities;
using System.Security.Claims;

namespace SifreKasasi.API.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class VaultController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AesEncryptionService _aesService;

        public VaultController(AppDbContext context, AesEncryptionService aesService)
        {
            _context = context;
            _aesService = aesService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<IActionResult> GetVaultItems()
        {
            int userId = GetCurrentUserId();
            var items = await _context.VaultItems.Where(v => v.UserId == userId).ToListAsync();

            var responseList = items.Select(item => new VaultItemResponseDto
            {
                Id = item.Id,
                SiteName = item.SiteName,
                Username = item.Username,
                Password = _aesService.Decrypt(item.EncryptedPassword, item.EncryptionIv)
            }).ToList();

            return Ok(responseList);
        }

        [HttpPost]
        public async Task<IActionResult> AddVaultItem(VaultItemCreateDto dto)
        {
            var (encryptedText, iv) = _aesService.Encrypt(dto.Password);

            var vaultItem = new VaultItem
            {
                UserId = GetCurrentUserId(),
                SiteName = dto.SiteName,
                Username = dto.Username,
                EncryptedPassword = encryptedText, 
                EncryptionIv = iv 
            };

            _context.VaultItems.Add(vaultItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Şifre kasaya güvenle eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaultItem(int id, VaultItemUpdateDto dto)
        {
            var item = await _context.VaultItems.FirstOrDefaultAsync(v => v.Id == id && v.UserId == GetCurrentUserId());

            if (item == null)
                return NotFound(new { message = "Kayıt bulunamadı veya size ait değil." });

            var (encryptedText, iv) = _aesService.Encrypt(dto.Password);

            item.SiteName = dto.SiteName;
            item.Username = dto.Username;
            item.EncryptedPassword = encryptedText;
            item.EncryptionIv = iv;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Şifre başarıyla güncellendi!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaultItem(int id)
        {
            var item = await _context.VaultItems.FirstOrDefaultAsync(v => v.Id == id && v.UserId == GetCurrentUserId());

            if (item == null)
                return NotFound(new { message = "Kayıt bulunamadı veya size ait değil." });

            _context.VaultItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Şifre başarıyla silindi!" });
        }
    }
}