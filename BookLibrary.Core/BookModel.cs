
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class Book : IDomainObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public Book() { }

        public Book(int id, string title, string author, int year, int genreId)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            GenreId = genreId;
        }

        public override string ToString()
        {
            var genreName = Genre?.Name ?? $"(Id {GenreId})";
            return $"Id-{Id}: \"{Title}\" - {Author} ({Year}), {genreName}";
        }
    }

    /// <summary>Тонкая активная модель: хранит состояние + события.</summary>
    ///
    public class BookModelActive
    {
        public event EventHandler? BooksChanged;
        public event EventHandler? PageInfoChanged;

        public IReadOnlyList<Book> Books { get; private set; } = new List<Book>();
        public int CurrentPage { get; private set; } = 1;
        public int TotalPages { get; private set; } = 1;

        public void SetPageData(IEnumerable<Book> books, int currentPage, int totalPages)
        {
            Books = books.ToList();
            CurrentPage = currentPage;
            TotalPages = totalPages;

            BooksChanged?.Invoke(this, EventArgs.Empty);
            PageInfoChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
