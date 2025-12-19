using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookLogic(IRepository<Book> bookRepository,IRepository<Genre> genreRepository)
    {
        private readonly IRepository<Book> _bookRepository = bookRepository;
        private readonly IRepository<Genre> _genreRepository = genreRepository;

        #region ============== Жанры ==============

        public List<Genre> GetAvailableGenres()
        {
            return [.. _genreRepository.ReadAll()];
        }
        #endregion

        #region ============== CRUID ==============

        public (bool Success, Book? Book, string Message) CreateBook(
            string title,
            string author,
            int year,
            int genreId)
        {
            var genre = _genreRepository.ReadById(genreId);
            if (genre == null)
                return (false, null, "Неверный ID жанра!");

            if (BookExists(title, author, year, genreId))
                return (false, null, "Книга с такими параметрами уже существует!");

            int currentYear = DateTime.Now.Year;
            if (year > currentYear)
                return (false, null, $"Год должен быть не позже {currentYear}!");

            var book = new Book(0, title, author, year, genreId)
            {
                Genre = genre
            };

            _bookRepository.Add(book);
            return (true, book, "Книга успешно добавлена!");
        }

        public bool UpdateBook(int id, string title, string author, int year, int genreId)
        {
            var genre = _genreRepository.ReadById(genreId);
            if (genre == null)
                return false;

            var existingBook = _bookRepository.ReadById(id);
            if (existingBook == null)
                return false;

            var updatedBook = new Book(id, title, author, year, genreId)
            {
                Genre = genre
            };

            return _bookRepository.Update(updatedBook);
        }

        public bool DeleteBook(int id)
        {
            return _bookRepository.Delete(id);
        }

        #endregion

        #region ============== Чтение ==============

        public Book? GetBook(int id)
        {
            return _bookRepository.ReadById(id);
        }

        public List<Book> GetAllBooks()
        {
            return [.. _bookRepository.ReadAll()];
        }

        public bool BookExists(string title, string author, int year, int genreId)
        {
            return _bookRepository.ReadAll().Any(book =>
                    string.Equals(book.Title, title, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(book.Author, author, StringComparison.OrdinalIgnoreCase) &&
                    book.Year == year &&
                    book.GenreId == genreId);
        }

        #endregion

        #region ============== Отборы / группировки ==============

        public List<Book> GetBooksByAuthor(string author)
        {
            return [.. _bookRepository.ReadAll().Where(b => string.Equals(b.Author, author, StringComparison.OrdinalIgnoreCase))];
        }

        public List<Book> GetBooksByGenre(int genreId)
        {
            return [.. _bookRepository.ReadAll().Where(b => b.GenreId == genreId)];
        }

        public List<Book> GetBooksAfterYear(int year)
        {
            return [.. _bookRepository.ReadAll().Where(b => b.Year >= year).OrderBy(b => b.Year)];
        }

        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return _bookRepository.ReadAll().GroupBy(b => b.Genre?.Name ?? b.GenreId.ToString()).ToDictionary(g => g.Key, g => g.ToList());
        }
        #endregion

        #region ============== Пагинация ==============

        public List<Book> GetBooksPage(int page, int pageSize)
        {
            return _bookRepository.GetBooksPage(page, pageSize);
        }

        public int GetBooksCount()
        {
            return _bookRepository.GetBooksCount();
        }
        #endregion
    }
}
