using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BookLibrary.Core
{
    public class BookRepository
    {
        private readonly string dataFilePath;
        private readonly object fileLock = new object();

        public event EventHandler DataChanged;

        public BookRepository()
        {
            //поменять путь
            //пк дима C:\Users\dshel\Документы\awdawd
            //ноут егор C:\Users\egorg\Documents\GitHub\awdawd
            string projectRoot = @"C:\Users\dshel\Документы\awdawd";
            dataFilePath = Path.Combine(projectRoot, "books_data.json");
        }

        private List<Book> LoadBooksFromFile()
        {
            lock (fileLock)
            {
                if (File.Exists(dataFilePath))
                {
                    try
                    {
                        var json = File.ReadAllText(dataFilePath);
                        var loadedBooks = JsonSerializer.Deserialize<List<Book>>(json);
                        return loadedBooks ?? new List<Book>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                        return new List<Book>();
                    }
                }
            }
            return new List<Book>();
        }

        private void SaveBooksToFile(List<Book> books)
        {
            lock (fileLock)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    var json = JsonSerializer.Serialize(books, options);
                    File.WriteAllText(dataFilePath, json);

                    // Уведомляем об изменении
                    OnDataChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
                }
            }
        }

        private int GetNextId(List<Book> books)
        {
            return books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
        }

        public Book Create(Book book)
        {
            var books = LoadBooksFromFile();
            book.Id = GetNextId(books);
            books.Add(book);
            SaveBooksToFile(books);
            return book;
        }

        protected virtual void OnDataChanged()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool Delete(int id)
        {
            var books = LoadBooksFromFile();
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                SaveBooksToFile(books);
                return true;
            }
            return false;
        }

        public Book Read(int id)
        {
            var books = LoadBooksFromFile();
            return books.FirstOrDefault(b => b.Id == id);
        }

        public List<Book> ReadAll()
        {
            return LoadBooksFromFile();
        }

        public bool Update(Book book)
        {
            var books = LoadBooksFromFile();
            var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Year = book.Year;
                existingBook.Genre = book.Genre;
                SaveBooksToFile(books);
                return true;
            }
            return false;
        }

        public bool BookExists(string title, string author, int year, string genre)
        {
            var books = LoadBooksFromFile();
            return books.Any(book =>
                string.Equals(book.Title, title, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(book.Author, author, StringComparison.OrdinalIgnoreCase) &&
                book.Year == year &&
                string.Equals(book.Genre, genre, StringComparison.OrdinalIgnoreCase));
        }
    }
}