using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace BookLibrary.WinForms
{
    public partial class MainForm : Form
    {
        private readonly BookLogic logic;
        private SoundPlayer soundPlayer;


        public MainForm()
        {
            var repository = CreateRepository("Dapper"); // "Dapper" или "EF"
            logic = new BookLogic(repository);

            InitializeComponent();
            LoadPaginatedBooks();
            InitializeSound();
            StartFileFlagListener();

        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPaginatedBooks();
            }
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPaginatedBooks();
            }
        }


        private void StartFileFlagListener()
        {
            Task.Run(() =>
            {
                string flagPath = @"C:\Temp\refresh.signal";
                while (true)
                {
                    if (File.Exists(flagPath))
                    {
                        this.Invoke(new Action(LoadPaginatedBooks));
                        File.Delete(flagPath);
                    }
                    Thread.Sleep(500); // чтобы не грузить процессор
                }
            });
        }

        private int currentPage = 1;
        private int booksPerPage = 10;
        private int totalPages = 1;

        private void LoadPaginatedBooks()
        {
            var booksList = logic.GetAllBooks();
            totalPages = (int)Math.Ceiling(booksList.Count / (double)booksPerPage);
            var booksPage = booksList
                .Skip((currentPage - 1) * booksPerPage)
                .Take(booksPerPage)
                .ToList();
            dataGridView1.DataSource = booksPage;
            labelPageInfo.Text = $"Страница {currentPage} из {totalPages}";
        }


        // На кнопках вперёд/назад увеличивать/уменьшать currentPage и вызывать LoadPaginatedBooks.


        private static IRepository<Book> CreateRepository(string repositoryType)
        {
            var dataFolder = Path.Combine(@"C:\Users\egorg\Documents\GitHub", "awdawd"); 
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

        private void Repository_DataChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadBooks));
            }
            else
            {
                LoadPaginatedBooks();
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

        private void BtnADD_Click(object sender, EventArgs e)
        {
            var form = new BookForm(BookLogic.GetAvailableGenres());
            if (form.ShowDialog() == DialogResult.OK)
            {
                var result = logic.CreateBook(form.BookTitle, form.BookAuthor, form.BookYear, form.BookGenre);
                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                }
            }
            LoadPaginatedBooks();
        }

        private void BtnEDIT_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            var book = (Book)dataGridView1.SelectedRows[0].DataBoundItem;
            var form = new BookForm(book, BookLogic.GetAvailableGenres());
            if (form.ShowDialog() == DialogResult.OK)
            {
                logic.UpdateBook(book.Id, form.BookTitle, form.BookAuthor, form.BookYear, form.BookGenre);
            }
            LoadPaginatedBooks();
        }

        private void BtnDEL_Click(object sender, EventArgs e)
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
            LoadPaginatedBooks();
        }

        private void BtnSORTGenre_Click(object sender, EventArgs e)
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
            LoadBooks();
        }

        private void BtnSORTAuthor_Click(object sender, EventArgs e)
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
            LoadBooks();
        }

        private void BtnSORTYear_Click(object sender, EventArgs e)
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
            LoadBooks();
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
            YearSearchComboBox.Items.AddRange([.. years.Select(y => (object)y)]);

            if (years.Length > 0)
                YearSearchComboBox.SelectedIndex = 0;
        }

        private void BtnSearchById_Click(object sender, EventArgs e)
        {
            SearchBookById();
            LoadPaginatedBooks();
        }

        private void BtnResetSearch_Click(object sender, EventArgs e)
        {
            ResetSearch();
            LoadPaginatedBooks();
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
            LoadBooks();
        }

        private void ResetSearch()
        {
            idSearchTextBox.Text = "";
            LoadBooks();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PlayBackgroundSound();

            OpenGifWindow();
        }

        private void InitializeSound()
        {
            try
            {
                soundPlayer = new SoundPlayer(@"C:\Users\Gosha\Documents\GitHub\awdawd\BookLibrary.WinForms\Files\sound.wav");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки звука: {ex.Message}");
            }
        }

        private void PlayBackgroundSound()
        {
            try
            {
                soundPlayer?.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения звука: {ex.Message}");
            }
        }

        private void OpenGifWindow()
        {
            GifForm gifForm = new GifForm();
            //gifForm.ShowDialog(); // Модальное окно
            gifForm.Show(); // Немодальное окно
        }

        private void update_Click(object sender, EventArgs e)
        {
            LoadBooks();
        }
    }
}
