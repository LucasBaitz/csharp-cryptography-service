using Cryptography.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptography.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<CryptData> CryptData { get; set; }
    }
}
