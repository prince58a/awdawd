using Microsoft.EntityFrameworkCore;
using BookLibrary.Core;
using System;
using System.Linq;

namespace BookLibrary.DataAccessLayer
{
    public class BookDbContext : DbContext
    {
        private readonly string _dbPath;

        public BookDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

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
                new Genre(1, "Фантастика"),
                new Genre(2, "Детектив"),
                new Genre(3, "Роман"),
                new Genre(4, "Фэнтези"),
                new Genre(5, "Ужасы"),
                new Genre(6, "Приключения"),
                new Genre(7, "Научная литература"),
                new Genre(8, "Биография"),
                new Genre(9, "Поэзия"),
                new Genre(10, "Роман-антиутопия")
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
