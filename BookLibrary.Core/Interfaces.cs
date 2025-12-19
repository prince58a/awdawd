using System.Collections.Generic;
namespace BookLibrary.Core

{
    public interface IDomainObject
    {
        int Id { get; set; }
    }

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

    public interface IBookView
    {
        int? SelectedBookId { get; }
        string SearchIdText { get; }

        string? SelectedGenre { get; }
        string? SelectedAuthor { get; }
        int? SelectedYear { get; }

        void ShowBooks(IEnumerable<Book> books);
        void ShowMessage(string message);
        void UpdatePageInfo(int currentPage, int totalPages);
        Book? ShowBookDialog(Book? existing, IEnumerable<Genre> availableGenres);
    }


    public interface IBookModel
    {
        IEnumerable<Book> GetBooksPage(int page, int pageSize, out int totalBooks);
        Book? GetBook(int id);

        bool CreateBook(string title, string author, int year, int genreId);
        bool UpdateBook(int id, string title, string author, int year, int genreId);
        bool DeleteBook(int id);

        IEnumerable<Genre> GetAvailableGenres();
    }
}
