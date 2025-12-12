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
            totalBooks = _logic.GetBooksCount();
            return _logic.GetBooksPage(page, pageSize);
        }

        public Book? GetBook(int id)
        {
            return _logic.GetBook(id);
        }

        public IEnumerable<Genre> GetAvailableGenres()
        {
            return _logic.GetAvailableGenres();
        }

        public bool CreateBook(Book book)
        {
            var result = _logic.CreateBook(
                book.Title,
                book.Author,
                book.Year,
                book.GenreId);

            return result.Success;
        }

        public bool UpdateBook(Book book)
        {
            return _logic.UpdateBook(
                book.Id,
                book.Title,
                book.Author,
                book.Year,
                book.GenreId);
        }

        public bool DeleteBook(int id)
        {
            return _logic.DeleteBook(id);
        }
    }
}
