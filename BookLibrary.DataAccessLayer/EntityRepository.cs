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
            return _context.Books.ToList();
        }

        public Book ReadById(int id)
        {
            return _context.Books.Find(id);
        }

        public bool Update(Book item)
        {
            var existingBook = _context.Books.Find(item.Id);
            if (existingBook != null)
            {
                _context.Entry(existingBook).CurrentValues.SetValues(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}