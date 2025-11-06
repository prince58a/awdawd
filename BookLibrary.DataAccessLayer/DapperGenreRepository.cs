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
                        Name TEXT NOT NULL UNIQUE
                    )";
                connection.Execute(createSql);

                var names = new[]
                {
                    "Фантастика","Детектив","Роман","Фэнтези","Ужасы",
                    "Приключения","Научная литература","Биография","Поэзия","Роман-антиутопия"
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
