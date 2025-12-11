using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookLibrary.WinForms
{
    public partial class MainForm : Form, IBookView
    {
        private readonly BooksController _controller;
        private readonly BookLogic _logic;

        public MainForm()
        {
            InitializeComponent();

            // DI‑инициализация
            IKernel kernel = new StandardKernel(new SimpleConfigModule());
            _logic = kernel.Get<BookLogic>();
            IBookModel model = new BookModel(_logic);

            _controller = new BooksController(this, model, _logic);

            // Проброс событий во View
            add.Click += (s, e) => AddBookRequested?.Invoke(this, EventArgs.Empty);
            edit.Click += (s, e) => EditBookRequested?.Invoke(this, EventArgs.Empty);
            del.Click += (s, e) => DeleteBookRequested?.Invoke(this, EventArgs.Empty);
            searchByIdButton.Click += (s, e) => SearchByIdRequested?.Invoke(this, EventArgs.Empty);
            resetSearchButton.Click += (s, e) => ResetSearchRequested?.Invoke(this, EventArgs.Empty);

            btnNextPage.Click += (s, e) => NextPageRequested?.Invoke(this, EventArgs.Empty);
            btnPrevPage.Click += (s, e) => PrevPageRequested?.Invoke(this, EventArgs.Empty);

            genre.Click += (s, e) => SortByGenreRequested?.Invoke(this, EventArgs.Empty);
            author.Click += (s, e) => SortByAuthorRequested?.Invoke(this, EventArgs.Empty);
            year.Click += (s, e) => SortByYearRequested?.Invoke(this, EventArgs.Empty);

            StartFileFlagListener();
            LoadYears();
            LoadGenres();
            LoadAuthors();
        }

        #region ============== IBookView: входные данные ==============

        public int? SelectedBookId =>
            dataGridView1.SelectedRows.Count > 0
                ? (int?)dataGridView1.SelectedRows[0].Cells["Id"].Value
                : null;

        public string SearchIdText => idSearchTextBox.Text.Trim();

        public string? SelectedGenre => GenereSearchComboBox.SelectedItem?.ToString();
        public string? SelectedAuthor => AuthorSearchComboBox.SelectedItem?.ToString();

        public int? SelectedYear =>
            int.TryParse(YearSearchComboBox.SelectedItem?.ToString(), out var yearValue)
                ? yearValue
                : (int?)null;
        #endregion

        #region ============== IBookView: события ==============

        public event EventHandler AddBookRequested;
        public event EventHandler EditBookRequested;
        public event EventHandler DeleteBookRequested;
        public event EventHandler SearchByIdRequested;
        public event EventHandler ResetSearchRequested;
        public event EventHandler NextPageRequested;
        public event EventHandler PrevPageRequested;

        public event EventHandler SortByGenreRequested;
        public event EventHandler SortByAuthorRequested;
        public event EventHandler SortByYearRequested;

        #endregion

        #region ============== IBookView: вывод ==============

        public void ShowBooks(IEnumerable<Book> books)
        {
            dataGridView1.DataSource = books.ToList();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void UpdatePageInfo(int currentPage, int totalPages)
        {
            labelPageInfo.Text = $"Страница {currentPage} из {totalPages}";
        }

        #endregion

        // Диалог добавления/редактирования книги
        public Book? ShowBookDialog(Book? existing, IEnumerable<Genre> availableGenres)
        {
            var genresArray = availableGenres.ToArray();

            BookForm form = existing == null
                ? new BookForm(genresArray)
                : new BookForm(existing, genresArray);

            if (form.ShowDialog() != DialogResult.OK)
                return null;

            return new Book
            {
                Id = existing?.Id ?? 0,
                Title = form.BookTitle,
                Author = form.BookAuthor,
                Year = form.BookYear,
                GenreId = form.BookGenreId
            };
        }

        #region ============== Прочий UI‑код ==============

        private void StartFileFlagListener()
        {
            Task.Run(() =>
            {
                const string flagPath = @"C:\Temp\refresh.signal";

                while (true)
                {
                    if (System.IO.File.Exists(flagPath))
                    {
                        Invoke(new Action(() =>
                        {
                            ResetSearchRequested?.Invoke(this, EventArgs.Empty);
                        }));

                        System.IO.File.Delete(flagPath);
                    }

                    Thread.Sleep(500);
                }
            });
        }

        private void resetSearchButton_Click(object sender, EventArgs e)
        {
            idSearchTextBox.Text = string.Empty;
        }

        private void LoadYears()
        {
            var years = _logic.GetAllBooks()
                .Select(b => b.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToArray();

            YearSearchComboBox.Items.Clear();
            YearSearchComboBox.Items.AddRange(years.Cast<object>().ToArray());

            if (years.Length > 0)
                YearSearchComboBox.SelectedIndex = 0;
        }

        private void LoadGenres()
        {
            var genres = _logic.GetAllBooks()
                .Select(b => b.Genre.Name)
                .Distinct()
                .OrderBy(g => g)
                .ToArray();

            GenereSearchComboBox.Items.Clear();
            GenereSearchComboBox.Items.AddRange(genres);

            if (genres.Length > 0)
                GenereSearchComboBox.SelectedIndex = 0;
        }

        private void LoadAuthors()
        {
            var authors = _logic.GetAllBooks()
                .Select(b => b.Author)
                .Distinct()
                .OrderBy(a => a)
                .ToArray();

            AuthorSearchComboBox.Items.Clear();
            AuthorSearchComboBox.Items.AddRange(authors);

            if (authors.Length > 0)
                AuthorSearchComboBox.SelectedIndex = 0;
        }
        #endregion
    }
}
