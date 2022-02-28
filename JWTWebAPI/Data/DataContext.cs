using JWTWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
        
        public DbSet<User> Users { get; set; }
        
    }
}