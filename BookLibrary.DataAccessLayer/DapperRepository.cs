using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using Dapper;
using BookLibrary.Core;
using System;

namespace BookLibrary.DataAccessLayer
{
    public class DapperRepository : IRepository<Book>
    {
        private readonly string _connectionString;
        public List<Book> GetBooksPage(int page, int pageSize)
        {
            string sql = "SELECT * FROM Books ORDER BY Id LIMIT @PageSize OFFSET @Offset";
            using IDbConnection db = new SqliteConnection(_connectionString);
            return db.Query<Book>(sql, new { PageSize = pageSize, Offset = (page - 1) * pageSize }).ToList();
        }
        public int GetBooksCount()
        {
            string sql = "SELECT COUNT(*) FROM Books";
            using IDbConnection db = new SqliteConnection(_connectionString);
            return db.ExecuteScalar<int>(sql);
        }

        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                var tableExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Books'");

                if (tableExists == 0)
                {
                    var createTableSql = @"
                        CREATE TABLE Books (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Author TEXT NOT NULL,
                            Year INTEGER NOT NULL,
                            GenreId INTEGER NOT NULL,
                            FOREIGN KEY (GenreId) REFERENCES Genres(Id)
                        )";
                    connection.Execute(createTableSql);
                    Console.WriteLine("Таблица Books создана успешно");
                }

                var genresExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Genres'");

                if (genresExists == 0)
                {
                    var createGenres = @"
                        CREATE TABLE Genres (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL UNIQUE
                        )";
                    connection.Execute(createGenres);

                    var names = new[]
                    {
                        "Фантастика","Детектив","Роман","Фэнтези","Ужасы",
                        "Приключения","Научная литература","Биография","Поэзия","Роман-антиутопия"
                    };

                    foreach (var n in names)
                    {
                        connection.Execute("INSERT INTO Genres (Name) VALUES (@Name)", new { Name = n });
                    }

                    Console.WriteLine("Таблица Genres создана и заполнена");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
                throw;
            }
        }

        public void Add(Book item)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = @"INSERT INTO Books (Title, Author, Year, GenreId) 
                           VALUES (@Title, @Author, @Year, @GenreId);
                           SELECT last_insert_rowid();";

            var id = db.Query<long>(sql, item).Single();
            item.Id = (int)id;
        }

        public bool Delete(int id)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = "DELETE FROM Books WHERE Id = @Id";
            var affectedRows = db.Execute(sql, new { Id = id });
            return affectedRows > 0;
        }

        public IEnumerable<Book> ReadAll()
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = @"SELECT b.Id, b.Title, b.Author, b.Year, b.GenreId,
                               g.Id, g.Name
                        FROM Books b
                        LEFT JOIN Genres g ON b.GenreId = g.Id";

            var list = db.Query<Book, Genre, Book>(
                sql,
                (book, genre) =>
                {
                    book.Genre = genre;
                    return book;
                },
                splitOn: "Id"
            );

            return list;
        }

        public Book ReadById(int id)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = @"SELECT b.Id, b.Title, b.Author, b.Year, b.GenreId,
                               g.Id, g.Name
                        FROM Books b
                        LEFT JOIN Genres g ON b.GenreId = g.Id
                        WHERE b.Id = @Id";

            var result = db.Query<Book, Genre, Book>(
                sql,
                (book, genre) => { book.Genre = genre; return book; },
                new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();

            return result;
        }

        public bool Update(Book item)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = @"UPDATE Books 
                           SET Title = @Title, Author = @Author, Year = @Year, GenreId = @GenreId 
                           WHERE Id = @Id";
            var affectedRows = db.Execute(sql, item);
            return affectedRows > 0;
        }
    }
}
