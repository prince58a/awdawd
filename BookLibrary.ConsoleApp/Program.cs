using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using System;
using System.IO;

namespace BookLibrary.ConsoleApp
{
    internal class Program
    {
        private static BookLogic logic;

        static void Main(string[] args)
        {
            var repository = CreateRepository("Dapper"); // "Dapper" или "EF"
            logic = new BookLogic(repository);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== КНИЖНАЯ БИБЛИОТЕКА ===" +
                "\n1. Показать все книги" +
                "\n2. Добавить книгу" +
                "\n3. Редактировать книгу" +
                "\n4. Удалить книгу" +
                "\n5. Поиск по автору" +
                "\n6. Группировка по жанрам" +
                "\n7. Книги после указанного года" +
                "\n0. Выход");

                Console.Write("Выберите действие: ");
                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1": ShowAllBooks(); break;
                    case "2": AddBook(); break;
                    case "3": EditBook(); break;
                    case "4": DeleteBook(); break;
                    case "5": SearchByAuthor(); break;
                    case "6": GroupByGenre(); break;
                    case "7": ShowBooksAfterYear(); break;
                    case "0": Environment.Exit(0); break;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static IRepository<Book> CreateRepository(string repositoryType)
        {
            var dataFolder = Path.Combine(@"C:\Users\Gosha\Documents\GitHub", "awdawd");
            Directory.CreateDirectory(dataFolder);

            var dbPath = Path.Combine(dataFolder, "BookLibrary.db");
            var connectionString = $"Data Source={dbPath}";

            Console.WriteLine($"База данных: {dbPath}");

            return repositoryType.ToUpper() switch
            {
                "EF" => CreateEntityFrameworkRepository(dbPath),
                "DAPPER" => CreateDapperRepository(connectionString),
                _ => CreateEntityFrameworkRepository(dbPath)
            };
        }

        private static IRepository<Book> CreateEntityFrameworkRepository(string dbPath)
        {
            var context = new BookDbContext(dbPath);
            return new EntityRepository(context);
        }

        private static IRepository<Book> CreateDapperRepository(string connectionString)
        {
            return new DapperRepository(connectionString);
        }

        private static void ShowAllBooks()
        {
            var books = logic.GetAllBooks();
            if (books.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            Console.WriteLine("\n=== ВСЕ КНИГИ ===");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        private static void AddBook()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ КНИГИ ===");

            Console.Write("Название: ");
            var title = Console.ReadLine();

            Console.Write("Автор: ");
            var author = Console.ReadLine();

            int year;
            while (true)
            {
                Console.Write("Год: ");
                if (!int.TryParse(Console.ReadLine(), out year))
                {
                    Console.WriteLine("Ошибка: введите число для года!");
                    continue;
                }
                break;
            }

            string genre = SelectGenre();

            var result = logic.CreateBook(title, author, year, genre);
            Console.WriteLine(result.Message);
        }

        private static string SelectGenre()
        {
            var genres = BookLogic.GetAvailableGenres();

            while (true)
            {
                Console.WriteLine("\nВыберите жанр:");
                for (int i = 0; i < genres.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {genres[i]}");
                }

                Console.Write($"Введите номер жанра (1-{genres.Length}): ");

                if (!int.TryParse(Console.ReadLine(), out int genreChoice) ||
                    genreChoice < 1 || genreChoice > genres.Length)
                {
                    Console.WriteLine($"Ошибка: введите число от 1 до {genres.Length}!");
                    continue;
                }

                return genres[genreChoice - 1];
            }
        }

        private static void EditBook()
        {
            ShowAllBooks();

            Console.Write("ID книги для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID!");
                return;
            }

            var book = logic.GetBook(id);
            if (book == null)
            {
                Console.WriteLine("Книга не найдена!");
                return;
            }

            Console.WriteLine($"Редактирование: {book}");

            Console.Write("Новое название: ");
            var title = Console.ReadLine();

            Console.Write("Новый автор: ");
            var author = Console.ReadLine();

            Console.Write("Новый год: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Неверный год!");
                return;
            }

            string genre = SelectGenre();

            if (logic.UpdateBook(id, title, author, year, genre))
            {
                Console.WriteLine("Книга обновлена!");
            }
            else
            {
                Console.WriteLine("Ошибка обновления!");
            }
        }

        private static void DeleteBook()
        {
            Console.Write("ID книги для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID!");
                return;
            }

            if (logic.DeleteBook(id))
            {
                Console.WriteLine("Книга удалена!");
            }
            else
            {
                Console.WriteLine("Книга не найдена!");
            }
        }

        private static void SearchByAuthor()
        {
            var books = logic.GetAllBooks();
            var authors = books.Select(b => b.Author).Distinct().ToList();

            Console.WriteLine("Список авторов:");
            foreach (string author in authors)
            {
                Console.WriteLine(author);
            }

            Console.Write("Автор для поиска: ");
            var authorSearch = Console.ReadLine();

            var authorBooks = logic.GetBooksByAuthor(authorSearch);
            if (authorBooks.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            Console.WriteLine($"\n=== КНИГИ АВТОРА {authorSearch} ===");
            foreach (var book in authorBooks)
            {
                Console.WriteLine(book);
            }
        }

        private static void GroupByGenre()
        {
            var groups = logic.GroupBooksByGenre();
            if (groups.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            Console.WriteLine("\n=== ГРУППИРОВКА ПО ЖАНРАМ ===");
            foreach (var group in groups)
            {
                Console.WriteLine($"\n--- {group.Key} ---");
                foreach (var book in group.Value)
                {
                    Console.WriteLine($"  {book}");
                }
            }
        }

        private static void ShowBooksAfterYear()
        {
            Console.Write("Год: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Неверный год!");
                return;
            }

            var books = logic.GetBooksAfterYear(year);
            if (books.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            Console.WriteLine($"\n=== КНИГИ ПОСЛЕ {year} ГОДА ===");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }
    }
}