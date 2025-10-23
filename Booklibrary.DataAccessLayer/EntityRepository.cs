using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Booklibrary.DataAccessLayer
{
    public class EntityRepository<T> : Irepository<T> where T : class, IDomainObject
    {
        private readonly LibraryDbContext _Context;
        public EntityRepository(LibraryDbContext context)
        {
            _Context = context;
        }

        public T Add(T entity)
        {
            var Added = _Context.Set<T>().Add(entity);
            _Context.SaveChanges();
            return Added;
        }

        public bool Delete(int  id)
        {
            var entity = _Context.Set<T>().Find(id);
            if (entity != null) return false;
            _Context.Set<T>().Remove(entity);
            _Context.SaveChanges();
            return true;
        }

        public IEnumerable<T> ReadAll()
        {
            return _Context.Set<T>().ToList();
        }

        public T ReadById(int id)
        {
            return _Context.Set<T>().Find(id);
        }

        public bool Update(T entity)
        {
            var entry = _Context.Entry(entity);
            entry.State = EntityState.Modified;
            _Context.SaveChangesAsync();
            return true;
        }
    }
}
