using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookLogic(IRepository<Book> repository)
    {
        private readonly IRepository<Book> _repository = repository;
        private static readonly string[] AvailableGenres = [
            "Фантастика", "Детектив", "Роман", "Фэнтези", "Ужасы",
            "Приключения", "Научная литература", "Биография", "Поэзия", "Роман-антиутопия"
        ];

        public static string[] GetAvailableGenres() => AvailableGenres;

        public (bool Success, Book Book, string Message) CreateBook(string title, string author, int year, string genre)
        {
            if (!AvailableGenres.Contains(genre))
            {
                return (false, null, "Неверный жанр! Выберите из доступных.");
            }

            if (BookExists(title, author, year, genre))
            {
                return (false, null, "Книга с такими параметрами уже существует!");
            }

            if (year > DateTime.Now.Year || year < 1000)
            {
                return (false, null, $"Год должен быть между 1000 и {DateTime.Now.Year}!");
            }

            var book = new Book(0, title, author, year, genre);
            _repository.Add(book);
            return (true, book, "Книга успешно добавлена!");
        }

        public bool DeleteBook(int id) => _repository.Delete(id);

        public Book GetBook(int id) => _repository.ReadById(id);

        public List<Book> GetAllBooks() => [.. _repository.ReadAll()];

        public bool UpdateBook(int id, string title, string author, int year, string genre)
        {
            if (!AvailableGenres.Contains(genre))
                return false;

            var existingBook = _repository.ReadById(id);
            if (existingBook == null)
                return false;

            var updatedBook = new Book(id, title, author, year, genre);
            return _repository.Update(updatedBook);
        }

        public bool BookExists(string title, string author, int year, string genre)
        {
            return _repository.ReadAll().Any(book =>
                string.Equals(book.Title, title, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(book.Author, author, StringComparison.OrdinalIgnoreCase) &&
                book.Year == year &&
                string.Equals(book.Genre, genre, StringComparison.OrdinalIgnoreCase));
        }
        public List<Book> GetBooksPage(int page, int pageSize)
        {
            return _repository.ReadAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public List<Book> GetBooksByAuthor(string author)
        {
            return [.. _repository.ReadAll().Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))];
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            return [.. _repository.ReadAll().Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))];
        }

        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return _repository.ReadAll().GroupBy(b => b.Genre).ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Book> GetBooksAfterYear(int year)
        {
            return [.. _repository.ReadAll().Where(b => b.Year >= year).OrderBy(b => b.Year)];
        }

        public override bool Equals(object? obj)
        {
            return obj is BookLogic logic &&
                   EqualityComparer<IRepository<Book>>.Default.Equals(_repository, logic._repository);
        }
    }
}