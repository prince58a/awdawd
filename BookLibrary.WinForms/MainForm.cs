using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookLibrary.WinForms
{
    public partial class MainForm : Form
    {
        private BookLogic logic;

        public MainForm()
        {
            var repository = CreateRepository("Dapper"); // "Dapper" или "EF"
            logic = new BookLogic(repository);

            InitializeComponent();
            LoadBooks();
        }

        private static IRepository<Book> CreateRepository(string repositoryType)
        {
            var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BookLibrary");
            Directory.CreateDirectory(dataFolder);

            var dbPath = Path.Combine(dataFolder, "BookLibrary.db");
            var connectionString = $"Data Source={dbPath}";

            if (repositoryType == "EF")
            {
                var context = new BookDbContext(dbPath);
                return new EntityRepository(context);
            }
            else if (repositoryType == "Dapper")
            {
                return new DapperRepository(connectionString);
            }
            else
            {
                throw new ArgumentException("Неизвестный тип репозитория");
            }
        }




        private void OnDataFileChanged(object sender, FileSystemEventArgs e)
        {
            // код пробует несколько раз проверить файл, тк он может быть просто не доступен, хихи хаха, костыль кароче
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Thread.Sleep(100);

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            LoadBooks();
                            Console.WriteLine($"[WinForms] Данные обновлены: {DateTime.Now:HH:mm:ss}");
                        }));
                    }
                    else
                    {
                        LoadBooks();
                    }
                    break; // чтобы не было лишних телодвижений то закрытие
                }
                catch (Exception ex)
                {
                    if (i == 2) // Леди джентльмены, у нас есть последний шанс...
                    {
                        Console.WriteLine($"[WinForms] Ошибка обновления: {ex.Message}");
                    }
                    Thread.Sleep(50);
                }
            }
        }
        
        private void Repository_DataChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadBooks));
            }
            else
            {
                LoadBooks();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

        private void LoadBooks()
        {
            var books = logic.GetAllBooks();
            dataGridView1.DataSource = books;
            LoadAuthors();
            LoadGenres();
            LoadYears();
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            var form = new BookForm(logic.GetAvailableGenres());
            if (form.ShowDialog() == DialogResult.OK)
            {
                var result = logic.CreateBook(form.BookTitle, form.BookAuthor, form.BookYear, form.BookGenre);
                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                }
            }
        }

        private void btnEDIT_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            var book = (Book)dataGridView1.SelectedRows[0].DataBoundItem;
            var form = new BookForm(book, logic.GetAvailableGenres());
            if (form.ShowDialog() == DialogResult.OK)
            {
                logic.UpdateBook(book.Id, form.BookTitle, form.BookAuthor, form.BookYear, form.BookGenre);
            }
        }

        private void btnDEL_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            var book = (Book)dataGridView1.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show($"Удалить книгу '{book.Title}'?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                logic.DeleteBook(book.Id);
            }
        }

        private void btnSORTGenre_Click(object sender, EventArgs e)
        {
            if (GenereSearchComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр!");
                return;
            }

            var genre = GenereSearchComboBox.SelectedItem.ToString();
            var books = logic.GetBooksByGenre(genre);

            if (books.Count == 0)
            {
                MessageBox.Show("Книги не найдены!");
                return;
            }

            var result = string.Join("\n", books.Select(b => b.ToString()));
            MessageBox.Show(result, $"Книги жанра {genre}");
        }

        private void btnSORTAuthor_Click(object sender, EventArgs e)
        {
            if (AuthorSearchComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите автора!");
                return;
            }

            var author = AuthorSearchComboBox.SelectedItem.ToString();
            var books = logic.GetBooksByAuthor(author);

            if (books.Count == 0)
            {
                MessageBox.Show("Книги не найдены!");
                return;
            }

            var result = string.Join("\n", books.Select(b => b.ToString()));
            MessageBox.Show(result, $"Книги автора {author}");
        }

        private void btnSORTYear_Click(object sender, EventArgs e)
        {
            if (YearSearchComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите год!");
                return;
            }

            var year = int.Parse(YearSearchComboBox.SelectedItem.ToString());
            var books = logic.GetBooksAfterYear(year);

            if (books.Count == 0)
            {
                MessageBox.Show("Книги не найдены!");
                return;
            }

            var result = string.Join("\n", books.Select(b => b.ToString()));
            MessageBox.Show(result, $"Книги вышедшие после {year} года");
        }

        private void LoadAuthors()
        {
            var authors = logic.GetAllBooks()
                .Select(b => b.Author)
                .Distinct()
                .OrderBy(a => a)
                .ToArray();

            AuthorSearchComboBox.Items.Clear();
            AuthorSearchComboBox.Items.AddRange(authors);

            if (authors.Length > 0)
                AuthorSearchComboBox.SelectedIndex = 0;
        }

        private void LoadGenres()
        {
            var genres = logic.GetAllBooks()
                .Select(b => b.Genre)
                .Distinct()
                .OrderBy(a => a)
                .ToArray();

            GenereSearchComboBox.Items.Clear();
            GenereSearchComboBox.Items.AddRange(genres);

            if (genres.Length > 0)
                GenereSearchComboBox.SelectedIndex = 0;
        }

        private void LoadYears()
        {
            var years = logic.GetAllBooks()
                .Select(b => b.Year)
                .Distinct()
                .OrderBy(a => a)
                .ToArray();

            YearSearchComboBox.Items.Clear();
            YearSearchComboBox.Items.AddRange(years.Select(y => (object)y).ToArray());

            if (years.Length > 0)
                YearSearchComboBox.SelectedIndex = 0;
        }

        private void btnSearchById_Click(object sender, EventArgs e)
        {
            SearchBookById();
        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void SearchBookById()
        {
            string input = idSearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Введите ID книги!");
                return;
            }

            if (!int.TryParse(input, out int bookId) || bookId <= 0)
            {
                MessageBox.Show("ID должен быть положительным числом!");
                return;
            }

            var book = logic.GetBook(bookId);
            if (book == null)
            {
                MessageBox.Show($"Книга с ID {bookId} не найдена!");
                return;
            }

            dataGridView1.DataSource = new List<Book> { book };
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows[0].Selected = true;
        }

        private void ResetSearch()
        {
            idSearchTextBox.Text = "";
            LoadBooks();
        }
    }
}
