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

        public bool CreateBook(string title, string author, int year, int genreId)
            => _logic.CreateBook(title, author, year, genreId).Success;

        public bool UpdateBook(int id, string title, string author, int year, int genreId)
            => _logic.UpdateBook(id, title, author, year, genreId);

        public bool DeleteBook(int id)
            => _logic.DeleteBook(id);

        public IEnumerable<Genre> GetAvailableGenres()
            => _logic.GetAvailableGenres();
    }
}
