using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookPresenter
    {
        private readonly IBookView _view;
        private readonly IBookModel _model;

        private int _currentPage = 1;
        private readonly int _booksPerPage = 10;

        public BookPresenter(IBookView view, IBookModel model)
        {
            _view = view;
            _model = model;

            _view.AddBookRequested += OnAddBookRequested;
            _view.EditBookRequested += OnEditBookRequested;
            _view.DeleteBookRequested += OnDeleteBookRequested;
            _view.SearchByIdRequested += OnSearchByIdRequested;
            _view.ResetSearchRequested += OnResetSearchRequested;
            _view.NextPageRequested += OnNextPageRequested;
            _view.PrevPageRequested += OnPrevPageRequested;

            LoadPage();
        }

        private void LoadPage()
        {
            int totalBooks;
            var books = _model.GetBooksPage(_currentPage, _booksPerPage, out totalBooks);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)_booksPerPage));

            _view.ShowBooks(books);
            _view.UpdatePageInfo(_currentPage, totalPages);
        }

        private void OnNextPageRequested(object? s, EventArgs e)
        {
            _currentPage++;
            LoadPage();
        }

        private void OnPrevPageRequested(object? s, EventArgs e)
        {
            if (_currentPage > 1)
                _currentPage--;
            LoadPage();
        }

        private void OnSearchByIdRequested(object? s, EventArgs e)
        {
            if (!int.TryParse(_view.SearchIdText, out var id))
            {
                _view.ShowMessage("Некорректный ID");
                return;
            }

            var book = _model.GetBook(id);
            if (book == null)
            {
                _view.ShowMessage($"Книга с ID {id} не найдена");
                return;
            }

            _view.ShowBooks(new[] { book });
        }

        private void OnResetSearchRequested(object? s, EventArgs e)
        {
            LoadPage();
        }

        private void OnDeleteBookRequested(object? s, EventArgs e)
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Не выбрана книга");
                return;
            }

            _model.DeleteBook(_view.SelectedBookId.Value, out var msg, out var ok);
            _view.ShowMessage(msg);
            if (ok) LoadPage();
        }

        private void OnAddBookRequested(object? s, EventArgs e)
        {
            // Потом добавим: показать диалог BookForm через методы View.
        }

        private void OnEditBookRequested(object? s, EventArgs e)
        {
            // Потом добавим: получить книгу, показать BookForm и обновить.
        }
    }
}
