using System;
using System.Linq;
using System.Reflection.Emit;

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

        private void LoadPage()
        {
            int totalBooks;
            var books = _model.GetBooksPage(_currentPage, _booksPerPage, out totalBooks);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)_booksPerPage));

            _view.ShowBooks(books);
            _view.UpdatePageInfo(_currentPage, totalPages);
        }

        private void OnSortByGenreRequested(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_view.SelectedGenre))
            {
                _view.ShowMessage("Выберите жанр!");
                return;
            }

            var kostil = new List<string>{ "Биография","Детектив","Научная литература",
                "Фэнтези","Поэзия","Приключения","Роман","Роман-антиутопия","Ужасы","Фантастика" };

            var genre = _view.SelectedGenre;
            int genreId = kostil.IndexOf(genre) + 1;

            var books = _logic.GetBooksByGenre(genreId);
            if (books.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            var result = string.Join("\n", books.Select(b => b.ToString()));
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

            var result = string.Join("\n", books.Select(b => b.ToString()));
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

            var result = string.Join("\n", books.Select(b => b.ToString()));
            _view.ShowMessage($"Книги вышедшие после {year} года\n\n{result}");

            LoadPage();
        }

        private void OnNextPageRequested(object? sender, EventArgs e)
        {
            int totalBooks;
            var books = _model.GetBooksPage(_currentPage, _booksPerPage, out totalBooks);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)_booksPerPage));

            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadPage();
            }
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
