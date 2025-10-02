using BookLibrary.Core;
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
        private BookRepository repository;
        private FileSystemWatcher fileWatcher;

        public MainForm()
        {
            repository = RepositoryManager.GetRepository();
            logic = new BookLogic(repository);

            InitializeComponent();
            this.MinimumSize = new Size(800, 540);

            repository.DataChanged += Repository_DataChanged;
            SetupFileWatcher();
            LoadBooks();
        }

        private void SetupFileWatcher()
        {
            try
            {
                string dataFilePath = @"C:\Users\egorg\Documents\sem3lab1\books_data.json";
                string directory = Path.GetDirectoryName(dataFilePath);
                string fileName = Path.GetFileName(dataFilePath);

                fileWatcher = new FileSystemWatcher();
                fileWatcher.Path = directory;
                fileWatcher.Filter = fileName;
                fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                fileWatcher.Changed += OnDataFileChanged;
                fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка настройки FileSystemWatcher: {ex.Message}");
            }
        }

        private void OnDataFileChanged(object sender, FileSystemEventArgs e)
        {
            // Пробуем несколько раз, так как файл может быть заблокирован
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
                    break; // Если успешно, выходим из цикла
                }
                catch (Exception ex)
                {
                    if (i == 2) // Последняя попытка
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
            repository.DataChanged -= Repository_DataChanged;
            fileWatcher?.Dispose();
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
                // Данные автоматически сохранятся в файл и обновятся благодаря событию
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
                // Данные автоматически сохранятся в файл и обновятся
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
                // Данные автоматически сохранятся в файл и обновятся
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
