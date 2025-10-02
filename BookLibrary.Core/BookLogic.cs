using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Core
{
    public class BookLogic
    {
        private BookRepository repository;
        private static readonly string[] AvailableGenres = {
            "Фантастика", "Детектив", "Роман", "Фэнтези", "Ужасы",
            "Приключения", "Научная литература", "Биография", "Поэзия", "Роман-антиутопия"
        };

        public BookLogic(BookRepository repo)
        {
            repository = repo;
        }

        public string[] GetAvailableGenres() => AvailableGenres;

        public (bool Success, Book Book, string Message) CreateBook(string title, string author, int year, string genre)
        {
            if (!AvailableGenres.Contains(genre))
            {
                return (false, null, "Неверный жанр! Выберите из доступных.");
            }

            if (repository.BookExists(title, author, year, genre))
            {
                return (false, null, "Книга с такими параметрами уже существует!");
            }

            if (year > DateTime.Now.Year || year < 1000)
            {
                return (false, null, $"Год должен быть между 1000 и {DateTime.Now.Year}!");
            }

            var book = new Book(0, title, author, year, genre);
            repository.Create(book);
            return (true, book, "Книга успешно добавлена!");
        }

        public bool DeleteBook(int id) => repository.Delete(id);

        public Book GetBook(int id) => repository.Read(id);

        public List<Book> GetAllBooks() => repository.ReadAll();

        public bool UpdateBook(int id, string title, string author, int year, string genre)
        {
            if (!AvailableGenres.Contains(genre))
                return false;

            var book = new Book(id, title, author, year, genre);
            return repository.Update(book);
        }

        public List<Book> GetBooksByAuthor(string author)
        {
            return repository.ReadAll()
                .Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            return repository.ReadAll()
                .Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Dictionary<string, List<Book>> GroupBooksByGenre()
        {
            return repository.ReadAll()
                .GroupBy(b => b.Genre)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Book> GetBooksAfterYear(int year)
        {
            return repository.ReadAll()
                .Where(b => b.Year >= year)
                .OrderBy(b => b.Year)
                .ToList();
        }
    }
}