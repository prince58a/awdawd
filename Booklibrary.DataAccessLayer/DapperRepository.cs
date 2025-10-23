using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace Booklibrary.DataAccessLayer
{
    public class DapperRepository<T> : Irepository<T> where T : IDomainObject
    {
        private readonly string _connectionString;

        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T Add(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO books(Title, Author, Year, Genre) VALUES (@Title, @Author, @Year, @Genre); SELECT CAST(SCOPE_IDENTIFY() as int();";
                var id = connection.Query<int>(sql, entity).Single();
                entity.Id = id;
                return entity;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM books WHERE id = @id";
                var rows = connection.Execute(sql, new {Id = id});
                return rows > 0;
            }
        }

        public IEnumerable<T> ReadAll()
        {
            using ( var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM books";
                return connection.Query<T>(sql);
            }
        }

        public T ReadById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM books WHERE Id = @Id";
                return connection.Query<T> (sql, new {Id = id}).FirstOrDefault();
            }
        }

        public bool Update( T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE books SET Title = @Title, Author = @Author, Year = @Year, Genre = @Genre WHERE Id = @Id";
                var rows = connection.Execute(sql, entity);
                return rows > 0;
            }
        }

    }
}
