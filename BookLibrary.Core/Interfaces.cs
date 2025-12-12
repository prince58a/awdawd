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

        event EventHandler AddBookRequested;
        event EventHandler EditBookRequested;
        event EventHandler DeleteBookRequested;
        event EventHandler SearchByIdRequested;
        event EventHandler ResetSearchRequested;
        event EventHandler NextPageRequested;
        event EventHandler PrevPageRequested;

        event EventHandler SortByGenreRequested;
        event EventHandler SortByAuthorRequested;
        event EventHandler SortByYearRequested;
    }

    public interface IBookModel
    {
        IEnumerable<Book> GetBooksPage(int page, int pageSize, out int totalBooks);

        Book? GetBook(int id);
        IEnumerable<Genre> GetAvailableGenres();

        bool CreateBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(int id);
    }
}
