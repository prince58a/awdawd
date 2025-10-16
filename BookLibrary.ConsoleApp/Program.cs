using BookLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BookLibrary.ConsoleApp
{
    internal class Program
    {
        private static BookLogic logic;
        private static BookRepository repository;
        private static WatcherClient watcher;

        static void Main(string[] args)
        {
            repository = RepositoryManager.GetRepository();
            logic = new BookLogic(repository);

            watcher = new WatcherClient();
            watcher.Connect();
            watcher.DataChanged += (s, e) =>
            {
                Console.WriteLine("\n[Watcher] Обнаружено внешнее изменение данных!");
                RefreshData();
            };

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
                    case "0": Exit(); break;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static void RefreshData()
        {
            RepositoryManager.ResetRepository();
            repository = RepositoryManager.GetRepository();
            logic = new BookLogic(repository);
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

            watcher.NotifyChange(); // уведомляем Watcher
        }

        private static string SelectGenre()
        {
            var genres = logic.GetAvailableGenres();

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
                watcher.NotifyChange(); // уведомляем Watcher
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
                watcher.NotifyChange(); // уведомляем Watcher
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

        private static void Exit()
        {
            string[][] frames = {
                new string[] { "Сохранение.", "           _.,._", "        ,d$$$$$SSIi.", "      ,$$$SSSS$$SSIi:.", "     j$$$$SSSS$$$SIIi·.", "     S$$$$SS$$$$$SSIi·.", "    j?*`‾`?S$SI7”`“?IL:.", "    ?:     $$S?     `?i’", "    iL    j$?$L      I7", "    $$$b%d$’  `$b,__d$:", "    ?SSIiS?    S$?I?$SI", "     ‾`?IS$L_,d$SIi:`^’", "        ?$$$SS$SIi’", "        j:?i:i?·•:", "        ”` `^" },
                new string[] { "Сохранение..", "           _.,,._", "        ,d$$$$$SSIi:", "      ,$$$SSSS$$$S$Ii::", "     d$$$$SSSS$$$$SSiiI:.", "    j$$$$SS$$$$$SSSi:iII:.", "   j°`^?SSI7°”^?IL:iiISIi:", "   ?   :$I?     ?$:iIS$I:·", "  jL _,$?$L     j7b:iISi:", "  ?$d$’  `$b,_,d$$$:iIi?", "  i$$:    S$?I?$$$S%u.?’", "   ‾?L_,d$SIi:`^?S?^` ’", "    I$$$S$$SIi’", "    :i?i:i?·i·", "        ”` `^" },
                new string[] { "Сохранение...", "          _.,,._", "      ,dS$$$$$SSii:,", "   ,dS$S$$$$$$$$$$SIi:,", "  dIS$$$$$$$$$$$$$$SSSik", " jIS$$$$$$$$$$$$$$$$SIiiL", "·IIS$$$$$S$$$$$$$$$$SI:?$", ":iS$$$$$$7S$$$$SS$$SIii:?k", ":iS$$$$$7jIS$$$$SS$SIS::·?", "·iIS$$S7j$SI?S$$$SSii7 · L", " :iS$SSi$$$SL`?S$SIi?_.o$$", "  ?ISi7 `°^?Sb,`^°’‾`  _`”", "   ”?°’··:::·`?S$i’    :", "           ··  `?’", "" }
            };

            int currentFrame = 0;
            for (int i = 0; i < 30; i++)
            {
                Console.Clear();
                foreach (string line in frames[currentFrame])
                {
                    Console.WriteLine(line);
                }
                currentFrame = (currentFrame + 1) % frames.Length;
                Thread.Sleep(120);
            }
            Console.Clear();
            Console.WriteLine("До скорой встречи!");
            Thread.Sleep(800);
            Environment.Exit(0);
        }
    }
}
