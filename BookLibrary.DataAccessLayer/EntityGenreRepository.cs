using System.Collections.Generic;
using System.Linq;
using BookLibrary.Core;

namespace BookLibrary.DataAccessLayer
{
    public class EntityGenreRepository : IRepository<Genre>
    {
        private readonly BookDbContext _context;
        public EntityGenreRepository(BookDbContext context)
        {
            _context = context;
            _context.InitializeDatabase();
        }

        public void Add(Genre item)
        {
            _context.Genres.Add(item);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var g = _context.Genres.Find(id);
            if (g != null)
            {
                _context.Genres.Remove(g);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Genre> ReadAll() => _context.Genres.OrderBy(g => g.Name).ToList();

        public Genre ReadById(int id) => _context.Genres.Find(id);

        public bool Update(Genre item)
        {
            var existing = _context.Genres.Find(item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
