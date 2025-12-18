using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using Ninject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookLibrary.WinForms
{
    public partial class MainForm : Form, IBookView
    {
        // Было:
        // private readonly BookController _controller;
        // private readonly BookLogic _logic;

        private readonly BookControllerActive _controller;
        private readonly BookLogic _logic;
        private readonly BookModelActive _model;

        public MainForm()
        {
            InitializeComponent();

            IKernel kernel = new StandardKernel(new SimpleConfigModule());
            _logic = kernel.Get<BookLogic>();

            _model = new BookModelActive();

            _controller = new BookControllerActive(this, _model, _logic);

            _model.BooksChanged += Model_BooksChanged;
            _model.PageInfoChanged += Model_PageInfoChanged;

            add.Click += (s, e) => _controller.AddBook();
            edit.Click += (s, e) => _controller.EditBook();
            del.Click += (s, e) => _controller.DeleteBook();
            searchByIdButton.Click += (s, e) => _controller.SearchById();
            resetSearchButton.Click += (s, e) => _controller.ResetSearch();
            btnNextPage.Click += (s, e) => _controller.NextPage();
            btnPrevPage.Click += (s, e) => _controller.PrevPage();
            genre.Click += (s, e) => _controller.SortByGenre();
            author.Click += (s, e) => _controller.SortByAuthor();
            year.Click += (s, e) => _controller.SortByYear();

            StartFileFlagListener();
            LoadYears();
            LoadGenres();
            LoadAuthors();
            _controller.ResetSearch();
        }

        private void Model_BooksChanged(object? sender, EventArgs e)
        {
            ShowBooks(_model.Books);
        }

        private void Model_PageInfoChanged(object? sender, EventArgs e)
        {
            UpdatePageInfo(_model.CurrentPage, _model.TotalPages);
        }

        // ===== IBookView: входные данные =====

        public int? SelectedBookId =>
            dataGridView1.SelectedRows.Count > 0
                ? (int?)dataGridView1.SelectedRows[0].Cells["Id"].Value
                : null;

        public string SearchIdText => idSearchTextBox.Text.Trim();

        public string? SelectedGenre => GenereSearchComboBox.SelectedItem?.ToString();

        public string? SelectedAuthor => AuthorSearchComboBox.SelectedItem?.ToString();

        public int? SelectedYear =>
            int.TryParse(YearSearchComboBox.SelectedItem?.ToString(), out var y)
                ? y
                : (int?)null;

        // ===== IBookView: вывод =====

        public void ShowBooks(System.Collections.Generic.IEnumerable<Book> books)
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

        public Book? ShowBookDialog(Book? existing,
            System.Collections.Generic.IEnumerable<Genre> availableGenres)
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

        // ===== Остальной UI =====

        private void StartFileFlagListener()
        {
            Task.Run(() =>
            {
                const string flagPath = @"C:\Temp\refresh.signal";
                while (true)
                {
                    if (System.IO.File.Exists(flagPath))
                    {
                        Invoke(new Action(() => _controller.ResetSearch()));
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
    }
}

