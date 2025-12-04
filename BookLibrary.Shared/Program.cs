using BookLibrary.Core;
using System;
using System.Collections.Generic;

namespace BookLibrary.Shared
{
    public interface IBookView
    {
        // Свойства/методы для отображения
        void ShowBooks(IEnumerable<Book> books);
        void ShowMessage(string message);
        void UpdatePageInfo(int currentPage, int totalPages);

        // Доступ к введённым данным
        int? SelectedBookId { get; }
        string SearchIdText { get; }

        // События, генерируемые View
        event EventHandler AddBookRequested;
        event EventHandler EditBookRequested;
        event EventHandler DeleteBookRequested;
        event EventHandler SearchByIdRequested;
        event EventHandler ResetSearchRequested;
        event EventHandler NextPageRequested;
        event EventHandler PrevPageRequested;
    }
}
