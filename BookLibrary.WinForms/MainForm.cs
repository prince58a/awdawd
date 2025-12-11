using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using Microsoft.VisualBasic.Logging;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookLibrary.WinForms
{
    public partial class MainForm : Form, IBookView
    {
        private readonly BooksController _controller;
        private readonly BookLogic logic;


        public MainForm()
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            logic = ninjectKernel.Get<BookLogic>();
            IBookModel model = new BookModel(logic);

            _controller = new BooksController(this, model, logic);

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

        // ================= IBookView: данные от пользователя =================

        public int? SelectedBookId =>
            dataGridView1.SelectedRows.Count > 0
                ? (int?)dataGridView1.SelectedRows[0].Cells["Id"].Value
                : null;

        public string SearchIdText => idSearchTextBox.Text.Trim();


        public string? SelectedGenre => GenereSearchComboBox.SelectedItem?.ToString();
        public string? SelectedAuthor => AuthorSearchComboBox.SelectedItem?.ToString();
        public int? SelectedYear => int.TryParse(YearSearchComboBox.SelectedItem?.ToString(), out var y) ? y : (int?)null;

        // ================= IBookView: события =================

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


        // ================= IBookView: вывод =================

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

        // Диалог добавления/редактирования книги.
        // existing == null -> создание новой книги.
        public Book? ShowBookDialog(Book? existing, IEnumerable<Genre> availableGenres)
        {
            var genresArray = availableGenres.ToArray();

            BookForm form;

            if (existing == null)
            {
                // конструктор BookForm(Genre[] genres)
                form = new BookForm(genresArray);
            }
            else
            {
                // конструктор BookForm(Book book, Genre[] genres)
                form = new BookForm(existing, genresArray);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                return new Book
                {
                    Id = existing?.Id ?? 0,
                    Title = form.BookTitle,
                    Author = form.BookAuthor,
                    Year = form.BookYear,
                    GenreId = form.BookGenreId
                };
            }

            return null;
        }

        // ================= Прочий UI‑код (звук, гифка, флаг‑файл) =================

        private void StartFileFlagListener()
        {
            Task.Run(() =>
            {
                string flagPath = @"C:\Temp\refresh.signal";
                while (true)
                {
                    if (System.IO.File.Exists(flagPath))
                    {
                        this.Invoke(new Action(() =>
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
            idSearchTextBox.Text = "";
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

        
    }
}
