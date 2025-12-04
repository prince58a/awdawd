using System;
using System.Collections.Generic;

namespace BookLibrary.Core
{
    public interface IBookView
    {
        int? SelectedBookId { get; }
        string SearchIdText { get; }

        void ShowBooks(IEnumerable<Book> books);
        void ShowMessage(string message);
        void UpdatePageInfo(int currentPage, int totalPages);

        event EventHandler AddBookRequested;
        event EventHandler EditBookRequested;
        event EventHandler DeleteBookRequested;
        event EventHandler SearchByIdRequested;
        event EventHandler ResetSearchRequested;
        event EventHandler NextPageRequested;
        event EventHandler PrevPageRequested;
    }
}
