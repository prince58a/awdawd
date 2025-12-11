using Microsoft.EntityFrameworkCore;
using BookLibrary.Core;
using System;
using System.Linq;

namespace BookLibrary.DataAccessLayer
{
    public class BookDbContext(string dbPath) : DbContext
    {
        private readonly string _dbPath = dbPath;

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.HasOne(b => b.Genre).WithMany().HasForeignKey(b => b.GenreId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Genre>().HasData(
                new Genre(1, "Биография"),
                new Genre(2, "Детектив"),
                new Genre(3, "Научная литература"),
                new Genre(4, "Поэзия"),
                new Genre(5, "Приключения"),
                new Genre(6, "Роман"),
                new Genre(7, "Роман-антиутопия"),
                new Genre(8, "Ужасы"),
                new Genre(9, "Фантастика"),
                new Genre(10, "Фэнтези")
            );
        }

        public void InitializeDatabase()
        {
            try
            {
                Database.EnsureCreated();
                Console.WriteLine("База данных успешно инициализирована (EF)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
                throw;
            }
        }
    }
}
