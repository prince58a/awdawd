using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookLogic
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Genre> _genreRepository;

        public BookLogic(IRepository<Book> bookRepository, IRepository<Genre> genreRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
        }

        public List<Genre> GetAvailableGenres()
        {
            return _genreRepository.ReadAll().ToList();
        }

        public (bool Success, Book Book, string Message) CreateBook(string title, string author, int year, int genreId)
        {
            var genre = _genreRepository.ReadById(genreId);
            if (genre == null)
            {
                return (false, null, "Неверный ID жанра!");
            }

            if (BookExists(title, author, year, genreId))
            {
                return (false, null, "Книга с такими параметрами уже существует!");
            }

            if (year > DateTime.Now.Year)
            {
                return (false, null, $"Год должен быть не позже {DateTime.Now.Year}!");
            }

            var book = new Book(0, title, author, year, genreId) { Genre = genre };
            _bookRepository.Add(book);
            return (true, book, "Книга успешно добавлена!");
        }

        public bool DeleteBook(int id) => _bookRepository.Delete(id);

        public Book GetBook(int id) => _bookRepository.ReadById(id);

        public List<Book> GetAllBooks() => _bookRepository.ReadAll().ToList();

        public bool UpdateBook(int id, string title, string author, int year, int genreId)
        {
            var genre = _genreRepository.ReadById(genreId);
            if (genre == null) return false;

            var existingBook = _bookRepository.ReadById(id);
            if (existingBook == null)
                return false;

            var updatedBook = new Book(id, title, author, year, genreId) { Genre = genre };
            return _bookRepository.Update(updatedBook);
        }

        public bool BookExists(string title, string author, int year, int genreId)
        {
            return _bookRepository.ReadAll().Any(book =>
                string.Equals(book.Title, title, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(book.Author, author, StringComparison.OrdinalIgnoreCase) &&
                book.Year == year &&
                book.GenreId == genreId);
        }
        public List<Book> GetBooksByAuthor(string author)
        {
            return _bookRepository.ReadAll().Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Book> GetBooksByGenre(int genreId)
        {
            return _bookRepository.ReadAll().Where(b => b.GenreId == genreId).ToList();
        }

        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return _bookRepository.ReadAll()
                .GroupBy(b => b.Genre?.Name ?? b.GenreId.ToString())
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Book> GetBooksAfterYear(int year)
        {
            return _bookRepository.ReadAll().Where(b => b.Year >= year).OrderBy(b => b.Year).ToList();
        }
        public List<Book> GetBooksPage(int page, int pageSize)
    => _bookRepository.GetBooksPage(page, pageSize);

        public int GetBooksCount()
            => _bookRepository.GetBooksCount();

    }
}
