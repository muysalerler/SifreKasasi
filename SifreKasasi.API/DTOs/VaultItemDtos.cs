using System.ComponentModel.DataAnnotations;

namespace SifreKasasi.API.DTOs
{
     //Yeni kayıt eklerken
    public class VaultItemCreateDto
    {
        [Required(ErrorMessage = "Bu alan zorunlu.")]
        public string SiteName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunlu.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunlu.")]
        public string Password { get; set; } = string.Empty;
    }
    //O kaydı güncellerken 
    public class VaultItemUpdateDto
    {
        [Required(ErrorMessage = "Bu alan zorunlu.")]
        public string SiteName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunlu.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan zorunlu.")]
        public string Password { get; set; } = string.Empty;
    }
    //O kaydı gösterirken
    public class VaultItemResponseDto
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
    }
}
