using Microsoft.EntityFrameworkCore;
using ToDoListManager.Models.CategoryModels;
using ToDoListManager.Models.NoteModels;

namespace ToDoListManager.Date
{
    public class AppDbContext : DbContext
    {
        private readonly string _path;
        public AppDbContext(string path) => _path = path;

        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштовуємо зв'язок один-до-багатьох
            modelBuilder.Entity<CategoryEntity>()
                .HasMany(c => c.Notes)
                .WithOne(n => n.Category)
                .HasForeignKey(n => n.CategoryName);
        }
    }
}
