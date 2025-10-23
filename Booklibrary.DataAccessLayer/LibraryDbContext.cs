using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Booklibrary.DataAccessLayer
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext() : base("name=DeafaultConnection")
        {

        }
        public DbSet<Book> Books { get; set; }
    }
}
