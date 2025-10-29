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
                                Genre TEXT NOT NULL
                            )";

                    connection.Execute(createTableSql);
                    Console.WriteLine("Таблица Books создана успешно");
                }
                else
                {
                    Console.WriteLine("Таблица Books уже существует");
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
            var sql = @"INSERT INTO Books (Title, Author, Year, Genre) 
                           VALUES (@Title, @Author, @Year, @Genre);
                           SELECT last_insert_rowid();";

            var id = db.Query<int>(sql, item).Single();
            item.Id = id;
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
            return db.Query<Book>("SELECT * FROM Books");
        }

        public Book ReadById(int id)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            return db.QueryFirstOrDefault<Book>("SELECT * FROM Books WHERE Id = @Id", new { Id = id });
        }

        public bool Update(Book item)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = @"UPDATE Books 
                           SET Title = @Title, Author = @Author, Year = @Year, Genre = @Genre 
                           WHERE Id = @Id";
            var affectedRows = db.Execute(sql, item);
            return affectedRows > 0;
        }
    }
}