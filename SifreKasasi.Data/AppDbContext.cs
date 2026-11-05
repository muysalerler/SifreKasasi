using Microsoft.EntityFrameworkCore;
using SifreKasasi.Data.Entities;

namespace SifreKasasi.Data
{
    public class AppDbContext : DbContext
    {
        //Veri tabanı ayarlarını dbcontexte gönderiyor.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VaultItem> VaultItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Kullanıcı isimleri özel olmalı!!!!!!!
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}