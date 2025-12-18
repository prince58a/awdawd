
using System;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookControllerActive
    {
        private readonly IBookView _view;
        private readonly BookModelActive _model;
        private readonly BookLogic _logic;

        private const int PageSize = 10;

        public BookControllerActive(IBookView view, BookModelActive model, BookLogic logic)
        {
            _view = view;
            _model = model;
            _logic = logic;

            LoadPage(1);
        }

        private void LoadPage(int page)
        {
            int totalBooks = _logic.GetBooksCount();
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)PageSize));

            page = Math.Clamp(page, 1, totalPages);

            var books = _logic.GetBooksPage(page, PageSize);
            _model.SetPageData(books, page, totalPages);
        }

        // ========= Пагинация =========

        public void NextPage()
        {
            if (_model.CurrentPage < _model.TotalPages)
                LoadPage(_model.CurrentPage + 1);
        }

        public void PrevPage()
        {
            if (_model.CurrentPage > 1)
                LoadPage(_model.CurrentPage - 1);
        }

        public void ResetSearch()
        {
            LoadPage(1);
        }

        // ========= Поиск =========

        public void SearchById()
        {
            if (!int.TryParse(_view.SearchIdText, out int id) || id <= 0)
            {
                _view.ShowMessage("ID должен быть положительным числом!");
                return;
            }

            var book = _logic.GetBook(id);
            if (book == null)
            {
                _view.ShowMessage($"Книга с ID {id} не найдена!");
                return;
            }

            // Тут можно либо обновить модель одной книгой, либо просто показать сообщение.
            _view.ShowBooks(new[] { book });
        }

        // ========= CRUD =========

        public void AddBook()
        {
            var genres = _logic.GetAvailableGenres();
            var newBook = _view.ShowBookDialog(null, genres);
            if (newBook == null)
                return;

            var result = _logic.CreateBook(newBook.Title, newBook.Author,
                                           newBook.Year, newBook.GenreId);

            _view.ShowMessage(result.Success ? "Книга добавлена" : result.Message);

            if (result.Success)
                LoadPage(_model.CurrentPage);
        }

        public void EditBook()
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Выберите книгу!");
                return;
            }

            var existing = _logic.GetBook(_view.SelectedBookId.Value);
            if (existing == null)
            {
                _view.ShowMessage("Книга не найдена!");
                return;
            }

            var genres = _logic.GetAvailableGenres();
            var edited = _view.ShowBookDialog(existing, genres);
            if (edited == null)
                return;

            bool ok = _logic.UpdateBook(edited.Id, edited.Title,
                                        edited.Author, edited.Year, edited.GenreId);

            _view.ShowMessage(ok ? "Книга обновлена" : "Не удалось обновить книгу");

            if (ok)
                LoadPage(_model.CurrentPage);
        }

        public void DeleteBook()
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Выберите книгу!");
                return;
            }

            int id = _view.SelectedBookId.Value;
            bool ok = _logic.DeleteBook(id);

            _view.ShowMessage(ok ? "Книга удалена" : "Не удалось удалить книгу");

            if (ok)
                LoadPage(_model.CurrentPage);
        }

        // ========= Сортировки / фильтры =========

        public void SortByGenre()
        {
            if (string.IsNullOrEmpty(_view.SelectedGenre))
            {
                _view.ShowMessage("Выберите жанр!");
                return;
            }

            string genreName = _view.SelectedGenre;
            var all = _logic.GetAllBooks();
            var filtered = all.Where(b => b.Genre?.Name == genreName).ToList();

            if (filtered.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            string result = string.Join(Environment.NewLine,
                                        filtered.Select(b => b.ToString()));
            _view.ShowMessage($"Книги жанра {genreName}:{Environment.NewLine}{Environment.NewLine}{result}");
        }

        public void SortByAuthor()
        {
            if (string.IsNullOrEmpty(_view.SelectedAuthor))
            {
                _view.ShowMessage("Выберите автора!");
                return;
            }

            string author = _view.SelectedAuthor;
            var filtered = _logic.GetBooksByAuthor(author);

            if (filtered.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            string result = string.Join(Environment.NewLine,
                                        filtered.Select(b => b.ToString()));
            _view.ShowMessage($"Книги автора {author}:{Environment.NewLine}{Environment.NewLine}{result}");
        }

        public void SortByYear()
        {
            if (!_view.SelectedYear.HasValue)
            {
                _view.ShowMessage("Выберите год!");
                return;
            }

            int year = _view.SelectedYear.Value;
            var filtered = _logic.GetBooksAfterYear(year);

            if (filtered.Count == 0)
            {
                _view.ShowMessage("Книги не найдены!");
                return;
            }

            string result = string.Join(Environment.NewLine,
                                        filtered.Select(b => b.ToString()));
            _view.ShowMessage($"Книги после {year} года:{Environment.NewLine}{Environment.NewLine}{result}");
        }
    }
}
