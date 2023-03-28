using Microsoft.EntityFrameworkCore;
using PhotoPicker.Entities;

namespace PhotoPicker.Infrastructure.Database
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
