using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using BookLibrary.Core;

namespace BookLibrary.DataAccessLayer
{
    public class EntityRepository : IRepository<Book>
    {
        private readonly BookDbContext _context;

        public EntityRepository(BookDbContext context)
        {
            _context = context;
            _context.InitializeDatabase();
        }

        public void Add(Book item)
        {
            _context.Books.Add(item);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var item = _context.Books.Find(id);
            if (item != null)
            {
                _context.Books.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Book> ReadAll()
        {
            return _context.Books.Include(b => b.Genre).ToList();
        }

        public Book ReadById(int id)
        {
            return _context.Books.Include(b => b.Genre).FirstOrDefault(b => b.Id == id);
        }

        public bool Update(Book item)
        {
            var existing = _context.Books.Find(item.Id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.Author = item.Author;
                existing.Year = item.Year;
                existing.GenreId = item.GenreId;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
