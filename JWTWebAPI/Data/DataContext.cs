using JWTWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Username must be unique to make sure there are no duplicate users.
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            // Since the roles of the user has to be stored in database and databases
            // don't take kindly to lists inside them, then the list will be made into a single
            // string. This large string can then later be made into smaller string by splitting the ','.
            builder.Entity<User>()
                .Property(e => e.Roles)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
        
        public DbSet<User> Users { get; set; }
        
    }
}