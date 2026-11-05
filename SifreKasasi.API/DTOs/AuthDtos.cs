//Required kullanmak için gerekli
using System.ComponentModel.DataAnnotations;


namespace SifreKasasi.API.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad zorunludur.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        public string LastName { get; set; } = string.Empty;
    }
    public class LoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; } = string.Empty;
    }
}
