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
            var (bookRepo, genreRepo) = CreateRepositories("Dapper"); // или "EF"
            logic = new BookLogic(bookRepo, genreRepo);

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
                "\n8. Управление жанрами" +
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
                    case "8": ManageGenres(); break;
                    case "0": Environment.Exit(0); break;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static (IRepository<Book>, IRepository<Genre>) CreateRepositories(string repositoryType)
        {
            var dataFolder = Path.Combine(@"C:\Users\dshel\Документы", "awdawd");
            Directory.CreateDirectory(dataFolder);

            var dbPath = Path.Combine(dataFolder, "BookLibrary.db");
            var connectionString = $"Data Source={dbPath}";

            Console.WriteLine($"База данных: {dbPath}");

            if (repositoryType.ToUpper() == "EF")
            {
                var context = new BookDbContext(dbPath);
                var bookRepo = new EntityRepository(context);
                var genreRepo = new EntityGenreRepository(context);
                return (bookRepo, genreRepo);
            }
            else
            {
                var bookRepo = new DapperRepository(connectionString);
                var genreRepo = new DapperGenreRepository(connectionString);
                return (bookRepo, genreRepo);
            }
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

            int genreId = SelectGenre();

            var result = logic.CreateBook(title, author, year, genreId);
            Console.WriteLine(result.Message);

            string tempFolder = @"C:\Temp";
            Directory.CreateDirectory(tempFolder);
            File.Create(@"C:\Temp\refresh.signal").Dispose();
        }

        private static int SelectGenre()
        {
            var genres = logic.GetAvailableGenres();

            while (true)
            {
                Console.WriteLine("\nВыберите жанр:");
                foreach (var g in genres)
                {
                    Console.WriteLine($"{g.Id}. {g.Name}");
                }

                Console.Write($"Введите ID жанра: ");

                if (!int.TryParse(Console.ReadLine(), out int genreChoice) ||
                    !genres.Any(g => g.Id == genreChoice))
                {
                    Console.WriteLine($"Ошибка: введите корректный ID жанра!");
                    continue;
                }

                return genreChoice;
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

            int genreId = SelectGenre();

            if (logic.UpdateBook(id, title, author, year, genreId))
            {
                Console.WriteLine("Книга обновлена!");
            }
            else
            {
                Console.WriteLine("Ошибка обновления!");
            }

            string tempFolder = @"C:\Temp";
            Directory.CreateDirectory(tempFolder);
            File.Create(@"C:\Temp\refresh.signal").Dispose();
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

            string tempFolder = @"C:\Temp";
            Directory.CreateDirectory(tempFolder);
            File.Create(@"C:\Temp\refresh.signal").Dispose();
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

        // Простое управление жанрами в консоли (CRUD)
        private static void ManageGenres()
        {
            while (true)
            {
                Console.WriteLine("\n=== УПРАВЛЕНИЕ ЖАНРАМИ ===");
                Console.WriteLine("1. Показать все жанры");
                Console.WriteLine("2. Добавить жанр");
                Console.WriteLine("3. Редактировать жанр");
                Console.WriteLine("4. Удалить жанр");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите: ");
                var c = Console.ReadLine();
                switch (c)
                {
                    case "1":
                        var gs = logic.GetAvailableGenres();
                        foreach (var g in gs) Console.WriteLine(g);
                        break;
                    case "2":
                        Console.Write("Название жанра: ");
                        var name = Console.ReadLine();
                        var genreRepo = GetGenreRepository();
                        genreRepo.Add(new Genre { Name = name });
                        Console.WriteLine("Добавлено.");
                        break;
                    case "3":
                        var genres = logic.GetAvailableGenres();
                        foreach (var g in genres) Console.WriteLine(g);
                        Console.Write("ID для редактирования: ");
                        if (int.TryParse(Console.ReadLine(), out int gidEdit))
                        {
                            var grRepo = GetGenreRepository();
                            var g = grRepo.ReadById(gidEdit);
                            if (g != null)
                            {
                                Console.Write("Новое имя: ");
                                g.Name = Console.ReadLine();
                                grRepo.Update(g);
                                Console.WriteLine("Обновлено.");
                            }
                            else Console.WriteLine("Не найдено.");
                        }
                        break;
                    case "4":
                        var gg = logic.GetAvailableGenres();
                        foreach (var g in gg) Console.WriteLine(g);
                        Console.Write("ID для удаления: ");
                        if (int.TryParse(Console.ReadLine(), out int gidDel))
                        {
                            var rr = GetGenreRepository();
                            if (rr.Delete(gidDel)) Console.WriteLine("Удалено.");
                            else Console.WriteLine("Ошибка удаления.");
                        }
                        break;
                    case "0": return;
                }
            }
        }

        private static IRepository<Genre> GetGenreRepository()
        {
            // Создадим временный репозиторий такой же, как при старте
            var dataFolder = Path.Combine(@"C:\Users\Gosha\Documents\GitHub", "awdawd");
            var dbPath = Path.Combine(dataFolder, "BookLibrary.db");
            var connectionString = $"Data Source={dbPath}";
            return new DapperGenreRepository(connectionString);
        }
    }
}
