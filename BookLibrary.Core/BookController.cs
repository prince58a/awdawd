using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class BooksController
    {
        private readonly IBookView _view;
        private readonly IBookModel _model;
        private readonly BookLogic _logic;

        private int _currentPage = 1;
        private readonly int _booksPerPage = 10;

        public BooksController(IBookView view, IBookModel model, BookLogic logic)
        {
            _view = view;
            _model = model;
            _logic = logic;

            // Подписка на события View
            _view.AddBookRequested += OnAddBookRequested;
            _view.EditBookRequested += OnEditBookRequested;
            _view.DeleteBookRequested += OnDeleteBookRequested;
            _view.SearchByIdRequested += OnSearchByIdRequested;
            _view.ResetSearchRequested += OnResetSearchRequested;
            _view.NextPageRequested += OnNextPageRequested;
            _view.PrevPageRequested += OnPrevPageRequested;

            _view.SortByAuthorRequested += OnSortByAuthorRequested;
            _view.SortByGenreRequested += OnSortByGenreRequested;
            _view.SortByYearRequested += OnSortByYearRequested;

            LoadPage();
        }

        #region =============== Пагинация =============== 

        private void LoadPage()
        {

            var books = _model.GetBooksPage(_currentPage, _booksPerPage, out int totalBooks);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)_booksPerPage));

            _view.ShowBooks(books);
            _view.UpdatePageInfo(_currentPage, totalPages);
        }

        private void OnNextPageRequested(object? sender, EventArgs e)
        {
            _ = _model.GetBooksPage(_currentPage, _booksPerPage, out int totalBooks);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)_booksPerPage));
            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadPage();
            }
        }

        private void OnPrevPageRequested(object? sender, EventArgs e)
        {
            if (_currentPage <= 1)
                return;

            _currentPage--;
            LoadPage();
        }
        #endregion

        #region =============== Сортировки / фильтры ===============

        private void OnSortByGenreRequested(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_view.SelectedGenre))
            {
                _view.ShowMessage("Выберите жанр!");
                return;
            }

            var genreNames = new List<string>
            {
                "Биография","Детектив","Научная литература","Поэзия","Приключения",
                "Роман","Роман-антиутопия","Ужасы","Фантастика", "Фэнтези"
            };

            string genre = _view.SelectedGenre;
            int genreId = genreNames.IndexOf(genre) + 1;

            var books = _logic.GetBooksByGenre(genreId);
            if (books.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            string result = string.Join("\n", books.Select(b => b.ToString()));
            _view.ShowMessage($"Книги жанра {genre}\n\n{result}");
        }

        private void OnSortByAuthorRequested(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_view.SelectedAuthor))
            {
                _view.ShowMessage("Выберите автора!");
                return;
            }

            string author = _view.SelectedAuthor;
            var books = _logic.GetBooksByAuthor(author);

            if (books.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            string result = string.Join("\n", books.Select(b => b.ToString()));
            _view.ShowMessage($"Книги автора {author}\n\n{result}");

            LoadPage();
        }

        private void OnSortByYearRequested(object? sender, EventArgs e)
        {
            if (!_view.SelectedYear.HasValue)
            {
                _view.ShowMessage("Выберите год!");
                return;
            }

            int year = _view.SelectedYear.Value;
            var books = _logic.GetBooksAfterYear(year);

            if (books.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            string result = string.Join("\n", books.Select(b => b.ToString()));
            _view.ShowMessage($"Книги вышедшие после {year} года\n\n{result}");

            LoadPage();
        }
        #endregion

        #region =============== Поиск / сброс ===============

        private void OnSearchByIdRequested(object? sender, EventArgs e)
        {
            if (!int.TryParse(_view.SearchIdText, out int id) || id <= 0)
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

            _view.ShowBooks([book]);
        }

        private void OnResetSearchRequested(object? sender, EventArgs e)
        {
            _currentPage = 1;
            LoadPage();
        }

        #endregion

        #region =============== CRUD ===============

        private void OnDeleteBookRequested(object? sender, EventArgs e)
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Выберите книгу!");
                return;
            }

            int id = _view.SelectedBookId.Value;
            _model.DeleteBook(id, out string message, out bool success);

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
                              out string message, out bool success);

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
                              out string message, out bool success);

            _view.ShowMessage(message);
            if (success)
                LoadPage();
        }

        #endregion
    }
}
