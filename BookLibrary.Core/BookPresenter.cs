using System;
using System.Linq;
using System.Reflection.Emit;

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

        private void OnNextPageRequested(object? sender, EventArgs e)
        {
            _currentPage++;
            LoadPage();
        }

        private void OnPrevPageRequested(object? sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPage();
            }
        }

        private void OnSearchByIdRequested(object? sender, EventArgs e)
        {
            if (!int.TryParse(_view.SearchIdText, out var id) || id <= 0)
            {
                _view.ShowMessage("ID должен быть положительным числом!");
                return;
            }

            var book = _model.GetBook(id);
            if (book == null)
            {
                _view.ShowMessage($"Книга с ID {id} не найдена!");
                return;
            }

            _view.ShowBooks(new[] { book });
        }

        private void OnResetSearchRequested(object? sender, EventArgs e)
        {
            LoadPage();
            
        }

        private void OnDeleteBookRequested(object? sender, EventArgs e)
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Выберите книгу!");
                return;
            }

            int id = _view.SelectedBookId.Value;
            _model.DeleteBook(id, out var message, out var success);
            _view.ShowMessage(message);

            if (success)
                LoadPage();
        }

        private void OnAddBookRequested(object? sender, EventArgs e)
        {
            var genres = _model.GetAvailableGenres();
            var newBook = _view.ShowBookDialog(null, genres);
            if (newBook == null)
                return;

            _model.CreateBook(newBook.Title, newBook.Author, newBook.Year, newBook.GenreId,
                              out var message, out var success);
            _view.ShowMessage(message);
            if (success)
                LoadPage();
        }

        private void OnEditBookRequested(object? sender, EventArgs e)
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Выберите книгу!");
                return;
            }

            var existing = _model.GetBook(_view.SelectedBookId.Value);
            if (existing == null)
            {
                _view.ShowMessage("Книга не найдена!");
                return;
            }

            var genres = _model.GetAvailableGenres();
            var edited = _view.ShowBookDialog(existing, genres);
            if (edited == null)
                return;

            _model.UpdateBook(edited.Id, edited.Title, edited.Author, edited.Year, edited.GenreId,
                              out var message, out var success);
            _view.ShowMessage(message);
            if (success)
                LoadPage();
        }
    }
}
