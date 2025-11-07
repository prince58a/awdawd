using System.Collections.Generic;

namespace BookLibrary.Core
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T item);
        bool Delete(int id);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        bool Update(T item);
        List<T> GetBooksPage(int page, int pageSize);
        int GetBooksCount();

    }
}
