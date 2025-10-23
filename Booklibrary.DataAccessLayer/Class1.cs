using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booklibrary.DataAccessLayer
{
    public interface IDomainObject
    {
        int Id { get; set;  }
    }

    public interface Irepository<T> where T : IDomainObject
    {
        T Add(T entity);
        bool Delete(int  id);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        bool Update(T entity);
    }

    public class Book : IDomainObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
    }

}
