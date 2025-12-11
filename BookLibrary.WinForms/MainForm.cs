using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
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
    public partial class MainForm : Form
    {
        private readonly IBookModel _model;

        private int _currentPage = 1;
        private readonly int _booksPerPage = 10;

        private SoundPlayer soundPlayer;
        private SoundPlayer hellyeahSound;

        public MainForm()
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            var logic = ninjectKernel.Get<BookLogic>();
            _model = new BookModel(logic);

            add.Click += Add_Click;
            edit.Click += Edit_Click;
            del.Click += Del_Click;
            searchByIdButton.Click += SearchByIdButton_Click;
            resetSearchButton.Click += ResetSearchButton_Click;
            btnNextPage.Click += BtnNextPage_Click;
            btnPrevPage.Click += BtnPrevPage_Click;

            InitializeSound();
            StartFileFlagListener();

            pictureBox1.Click += pictureBox1_Click;
            LoadPage();
        }

        private int? SelectedBookId =>
            dataGridView1.SelectedRows.Count > 0
                ? (int?)dataGridView1.SelectedRows[0].Cells["Id"].Value
                : null;

        private string SearchIdText => idSearchTextBox.Text.Trim();

        private void ShowBooks(IEnumerable<Book> books)
        {
            dataGridView1.DataSource = books.ToList();
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void UpdatePageInfo(int currentPage, int totalPages)
        {
            labelPageInfo.Text = $"Страница {currentPage} из {totalPages}";
        }

        private void LoadPage()
        {
            int totalBooks;
            var books = _model.GetBooksPage(_currentPage, _booksPerPage, out totalBooks);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)_booksPerPage));

            ShowBooks(books);
            UpdatePageInfo(_currentPage, totalPages);
        }

        private void BtnNextPage_Click(object? sender, EventArgs e)
        {
            _currentPage++;
            LoadPage();
        }

        private void BtnPrevPage_Click(object? sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPage();
            }
        }

        private void SearchByIdButton_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(SearchIdText, out var id) || id <= 0)
            {
                ShowMessage("ID должен быть положительным числом!");
                return;
            }

            var book = _model.GetBook(id);
            if (book == null)
            {
                ShowMessage($"Книга с ID {id} не найдена!");
                return;
            }

            ShowBooks(new[] { book });
        }

        private void ResetSearchButton_Click(object? sender, EventArgs e)
        {
            idSearchTextBox.Text = "";
            LoadPage();
        }

        private void Del_Click(object? sender, EventArgs e)
        {
            if (!SelectedBookId.HasValue)
            {
                ShowMessage("Выберите книгу!");
                return;
            }

            int id = SelectedBookId.Value;
            _model.DeleteBook(id, out var message, out var success);
            ShowMessage(message);
            if (success)
                LoadPage();
        }

        private void Add_Click(object? sender, EventArgs e)
        {
            var genres = _model.GetAvailableGenres();
            var newBook = ShowBookDialog(null, genres);
            if (newBook == null)
                return;

            _model.CreateBook(newBook.Title, newBook.Author, newBook.Year, newBook.GenreId,
                              out var message, out var success);
            ShowMessage(message);
            if (success)
                LoadPage();
        }

        private void Edit_Click(object? sender, EventArgs e)
        {
            if (!SelectedBookId.HasValue)
            {
                ShowMessage("Выберите книгу!");
                return;
            }

            var existing = _model.GetBook(SelectedBookId.Value);
            if (existing == null)
            {
                ShowMessage("Книга не найдена!");
                return;
            }

            var genres = _model.GetAvailableGenres();
            var edited = ShowBookDialog(existing, genres);
            if (edited == null)
                return;

            _model.UpdateBook(edited.Id, edited.Title, edited.Author, edited.Year, edited.GenreId,
                              out var message, out var success);
            ShowMessage(message);
            if (success)
                LoadPage();
        }

        private Book? ShowBookDialog(Book? existing, IEnumerable<Genre> availableGenres)
        {
            var genresArray = availableGenres.ToArray();
            BookForm form = existing == null
                ? new BookForm(genresArray)
                : new BookForm(existing, genresArray);

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

        // дальше оставляешь твой звук, гифку и resetSearchButton_Click,
        // только не забудь переименовать обработчик в ResetSearchButton_Click
        // или поменять привязку в Designer



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

        private void InitializeSound()
        {
            try
            {
                soundPlayer = new SoundPlayer(@"Files\sound.wav");
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
            gifForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PlayBackgroundSound();
            OpenGifWindow();
        }

        private void resetSearchButton_Click(object sender, EventArgs e)
        {
            idSearchTextBox.Text = "";            
        }
    }
}
