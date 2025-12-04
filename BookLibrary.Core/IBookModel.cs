using System.Collections.Generic;

namespace BookLibrary.Core
{
    public interface IBookModel
    {
        IEnumerable<Book> GetBooksPage(int page, int pageSize, out int totalBooks);
        Book? GetBook(int id);

        void CreateBook(string title, string author, int year, int genreId,
                        out string message, out bool success);

        void UpdateBook(int id, string title, string author, int year, int genreId,
                        out string message, out bool success);

        void DeleteBook(int id, out string message, out bool success);

        IEnumerable<Genre> GetAvailableGenres();
    }
}
