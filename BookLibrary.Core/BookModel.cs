
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    /// <summary>Тонкая активная модель: хранит состояние + события.</summary>
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
