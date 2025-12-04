using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookLibrary.Core
{
    public class BookModel : IBookModel
    {
        private readonly BookLogic _logic;

        public BookModel(BookLogic logic)
        {
            _logic = logic;
        }

        public IEnumerable<Book> GetBooksPage(int page, int pageSize, out int totalBooks)
        {
            var booksPage = _logic.GetBooksPage(page, pageSize);
            totalBooks = _logic.GetBooksCount();
            return booksPage;
        }

        public Book? GetBook(int id) => _logic.GetBook(id);

        public void CreateBook(string title, string author, int year, int genreId,
                       out string message, out bool success)
        {
            var result = _logic.CreateBook(title, author, year, genreId);
            success = result.Success;
            message = result.Message;
        }


        public void UpdateBook(int id, string title, string author, int year, int genreId,
                       out string message, out bool success)
        {
            var result = _logic.UpdateBook(id, title, author, year, genreId);
            success = _logic.UpdateBook(id, title, author, year, genreId);
            message = result ? "Книга обновлена" : "Ошибка при обновлении книги";
        }

        public void DeleteBook(int id, out string message, out bool success)
        {
            var result = _logic.DeleteBook(id);
            success = _logic.DeleteBook(id);
            message = result ? "Книга удалена" : "Ошибка при удалении книги";
        }

    }
}
