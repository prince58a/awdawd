using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookController
    {
        private readonly IBookView _view;
        private readonly IBookModel _model;

        private int _currentPage = 1;
        private const int PageSize = 10;

        public BookController(IBookView view, IBookModel model)
        {
            _view = view;
            _model = model;

            LoadPage();
        }

        private void LoadPage()
        {
            int totalBooks;
            var books = _model.GetBooksPage(_currentPage, PageSize, out totalBooks);

            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)PageSize));

            _view.ShowBooks(books);
            _view.UpdatePageInfo(_currentPage, totalPages);
        }

        // ========= Пагинация =========

        public void NextPage()
        {
            _currentPage++;
            LoadPage();
        }

        public void PrevPage()
        {
            if (_currentPage <= 1)
                return;

            _currentPage--;
            LoadPage();
        }

        public void ResetSearch()
        {
            _currentPage = 1;
            LoadPage();
        }

        // ========= Поиск =========

        public void SearchById()
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

            _view.ShowBooks(new[] { book });
        }

        // ========= CRUD =========

        public void AddBook()
        {
            var genres = _model.GetAvailableGenres();
            var newBook = _view.ShowBookDialog(null, genres);
            if (newBook == null)
                return;

            bool ok = _model.CreateBook(newBook.Title, newBook.Author,
                                        newBook.Year, newBook.GenreId);

            _view.ShowMessage(ok ? "Книга добавлена" : "Не удалось добавить книгу");
            if (ok)
                LoadPage();
        }

        public void EditBook()
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

            bool ok = _model.UpdateBook(edited.Id, edited.Title,
                                        edited.Author, edited.Year, edited.GenreId);

            _view.ShowMessage(ok ? "Книга обновлена" : "Не удалось обновить книгу");
            if (ok)
                LoadPage();
        }

        public void DeleteBook()
        {
            if (!_view.SelectedBookId.HasValue)
            {
                _view.ShowMessage("Выберите книгу!");
                return;
            }

            int id = _view.SelectedBookId.Value;

            bool ok = _model.DeleteBook(id);
            _view.ShowMessage(ok ? "Книга удалена" : "Не удалось удалить книгу");
            if (ok)
                LoadPage();
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

            var all = _model.GetBooksPage(1, int.MaxValue, out _);
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

            var all = _model.GetBooksPage(1, int.MaxValue, out _);
            var filtered = all.Where(b => b.Author == author).ToList();

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

            var all = _model.GetBooksPage(1, int.MaxValue, out _);
            var filtered = all.Where(b => b.Year >= year).ToList();

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
