namespace SifreKasasi.Data.Entities
{
    public class VaultItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SiteName { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty; 
        public string EncryptedPassword { get; set; } = string.Empty;
        public string EncryptionIv { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}