using Microsoft.EntityFrameworkCore;
using BookLibrary.Core;
using System;

namespace BookLibrary.DataAccessLayer
{
    public class BookDbContext(string dbPath) : DbContext
    {
        private readonly string _dbPath = dbPath;

        public DbSet<Book> Books { get; set; }

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
                entity.Property(e => e.Genre).IsRequired().HasMaxLength(50);
            });
        }

        public void InitializeDatabase()
        {
            try
            {
                Database.EnsureCreated();

                var tableExists = Database.ExecuteSqlRaw(@"
                    SELECT name FROM sqlite_master 
                    WHERE type='table' AND name='Books'");

                Console.WriteLine("База данных успешно инициализирована");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
                throw;
            }
        }
    }
}