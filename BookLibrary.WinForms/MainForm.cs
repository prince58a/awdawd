using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Windows.Forms;
using Ninject;

namespace BookLibrary.WinForms
{
    public partial class MainForm : Form
    {
        private readonly BookLogic logic;
        private readonly IRepository<Genre> genreRepo;
        private SoundPlayer soundPlayer;
        private SoundPlayer hellyeahSound;
        private int currentPage = 1;
        private int booksPerPage = 10;
        private int totalPages = 1;


        public MainForm()
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            logic = ninjectKernel.Get<BookLogic>();
            genreRepo = ninjectKernel.Get<IRepository<Genre>>();

            InitializeComponent();
            LoadPaginatedBooks();
            InitializeSound();
            StartFileFlagListener();
        }

        /// <summary>
        /// Возвращает 2 объекта типа IRepo исходя из запроса Dapper или EF
        /// </summary>
        private static (IRepository<Book>, IRepository<Genre>) CreateRepositories(string repositoryType)
        {
            var dataFolder = Path.Combine(@"C:\Users\egorg\Documents\GitHub", "awdawd");                     //ПОМЕНЯТЬ ПУТЬ!!!!!!!
            Directory.CreateDirectory(dataFolder);

            var dbPath = Path.Combine(dataFolder, "BookLibrary.db");
            var connectionString = $"Data Source={dbPath}";

            if (repositoryType == "EF")
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

        /// <summary>
        /// Хз, это егор делал
        /// </summary>
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

        #region Кнопки
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

        private void BtnADD_Click(object sender, EventArgs e)
        {
            var genres = logic.GetAvailableGenres().ToArray();
            var form = new BookForm(genres);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var result = logic.CreateBook(form.BookTitle, form.BookAuthor, form.BookYear, form.BookGenreId);
                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                }
                else
                {
                    LoadPaginatedBooks();
                }
            }
        }

        private void BtnEDIT_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            var id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
            var book = logic.GetBook(id);
            var genres = logic.GetAvailableGenres().ToArray();

            var form = new BookForm(book, genres);
            if (form.ShowDialog() == DialogResult.OK)
            {
                logic.UpdateBook(book.Id, form.BookTitle, form.BookAuthor, form.BookYear, form.BookGenreId);
                LoadPaginatedBooks();
            }
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

            List<string> kostil = new List<string> { "Биография","Детектив","Научная литература","Фэнтези","Поэзия",
                    "Приключения","Роман","Роман-антиутопия","Ужасы","Фантастика" };

            var genre = GenereSearchComboBox.SelectedItem.ToString();
            int genreId = kostil.IndexOf(genre) + 1;
            var books = logic.GetBooksByGenre(genreId);

            if (books.Count == 0)
            {
                MessageBox.Show("Книги не найдены!");
                return;
            }

            var result = string.Join("\n", books.Select(b => b.ToString()));
            MessageBox.Show(result, $"Книги жанра {genre}");
            LoadPaginatedBooks();
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
            LoadPaginatedBooks();
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
            LoadPaginatedBooks();
        }

        private void BtnSearchById_Click(object sender, EventArgs e)
        {
            SearchBookById();
        }

        private void BtnResetSearch_Click(object sender, EventArgs e)
        {
            ResetSearch();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PlayBackgroundSound();

            OpenGifWindow();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PlayHELLYEAHSound();
            BackgroundImage = Properties.Resources.HELLYEAH;
            button1.Image = Properties.Resources.HELLYEAH;
            label4.Image = Properties.Resources.HELLYEAH;
            add.Image = Properties.Resources.HELLYEAH;
            del.Image = Properties.Resources.HELLYEAH;
            edit.Image = Properties.Resources.HELLYEAH;
            author.Image = Properties.Resources.HELLYEAH;
            genre.Image = Properties.Resources.HELLYEAH;
            year.Image = Properties.Resources.HELLYEAH;
            searchByIdButton.Image = Properties.Resources.HELLYEAH;
            resetSearchButton.Image = Properties.Resources.HELLYEAH;
            label1.Image = Properties.Resources.HELLYEAH;
            label2.Image = Properties.Resources.HELLYEAH;
            label3.Image = Properties.Resources.HELLYEAH;
            pictureBox1.Image = Properties.Resources.HELLYEAH;
            btnPrevPage.Image = Properties.Resources.HELLYEAH;
            btnNextPage.Image = Properties.Resources.HELLYEAH;
            labelPageInfo.Image = Properties.Resources.HELLYEAH;
            //BackgroundImageLayout = ImageLayout.Stretch; // или Zoom, Tile, Center, None

            //Thread.Sleep(16000);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Вывод книг из бд на винформу, учитывая ограничение кол-ва книг на странице
        /// </summary>
        private void LoadPaginatedBooks()
        {
            var booksPage = logic.GetBooksPage(currentPage, booksPerPage);
            int totalBooks = logic.GetBooksCount();
            totalPages = (int)Math.Ceiling(totalBooks / (double)booksPerPage);

            dataGridView1.DataSource = booksPage;
            labelPageInfo.Text = $"Страница {currentPage} из {totalPages}";

            LoadAuthors();
            LoadYears();
            LoadGenres();

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
                .Select(b => b.Genre.Name)
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
            LoadPaginatedBooks();

            BackgroundImage = null;
            button1.Image = null;
            label4.Image = null;
            add.Image = null;
            del.Image = null;
            edit.Image = null;
            author.Image = null;
            genre.Image = null;
            year.Image = null;
            searchByIdButton.Image = null;
            resetSearchButton.Image = null;
            label1.Image = null;
            label2.Image = null;
            label3.Image = null;
            pictureBox1.Image = Properties.Resources.jhoe;
            btnPrevPage.Image = null;
            btnNextPage.Image = null;
            labelPageInfo.Image = null;
        }



        /// <summary>
        /// Загрузка звукового файла
        /// </summary>
        private void InitializeSound()
        {
            try
            {
                soundPlayer = new SoundPlayer(@"Files\sound.wav");
                hellyeahSound = new SoundPlayer(@"Files\HELL YEAH.wav");
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

        private void PlayHELLYEAHSound()
        {
            try
            {
                hellyeahSound?.Play();
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
        #endregion

    }
}
