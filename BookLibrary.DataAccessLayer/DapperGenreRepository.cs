using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Linq;
using BookLibrary.Core;

namespace BookLibrary.DataAccessLayer
{
    public class DapperGenreRepository : IRepository<Genre>
    {
        private readonly string _connectionString;
        public DapperGenreRepository(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }
        public List<Genre> GetBooksPage(int page, int pageSize)
        {
            using var db = new SqliteConnection(_connectionString);
            string sql = "SELECT * FROM Genres ORDER BY Id LIMIT @PageSize OFFSET @Offset";
            return db.Query<Genre>(sql, new { PageSize = pageSize, Offset = (page - 1) * pageSize }).ToList();
            // Для EF:
            // return _context.Genres.OrderBy(g => g.Id).Skip((page-1)*pageSize).Take(pageSize).ToList();
        }
        public int GetBooksCount()
        {
            using var db = new SqliteConnection(_connectionString);
            string sql = "SELECT COUNT(*) FROM Genres";
            return db.ExecuteScalar<int>(sql);
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var tableExists = connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Genres'");

            if (tableExists == 0)
            {
                var createSql = @"
                    CREATE TABLE Genres (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    )";
                connection.Execute(createSql);

                var names = new[]
                {
                    "Биография", "Детектив", "Научная литература", "Поэзия", "Приключения",
                    "Роман", "Роман-антиутопия", "Ужасы", "Фантастика", "Фэнтези"
                };

                foreach (var n in names)
                {
                    connection.Execute("INSERT INTO Genres (Name) VALUES (@Name)", new { Name = n });
                }
            }
        }

        public void Add(Genre item)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = @"INSERT INTO Genres (Name) VALUES (@Name); SELECT last_insert_rowid();";
            var id = db.Query<long>(sql, item).Single();
            item.Id = (int)id;
        }

        public bool Delete(int id)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = "DELETE FROM Genres WHERE Id = @Id";
            var rows = db.Execute(sql, new { Id = id });
            return rows > 0;
        }

        public IEnumerable<Genre> ReadAll()
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            return db.Query<Genre>("SELECT Id, Name FROM Genres ORDER BY Name");
        }

        public Genre ReadById(int id)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            return db.QueryFirstOrDefault<Genre>("SELECT Id, Name FROM Genres WHERE Id = @Id", new { Id = id });
        }

        public bool Update(Genre item)
        {
            using IDbConnection db = new SqliteConnection(_connectionString);
            var sql = "UPDATE Genres SET Name = @Name WHERE Id = @Id";
            var rows = db.Execute(sql, item);
            return rows > 0;
        }
    }
}
