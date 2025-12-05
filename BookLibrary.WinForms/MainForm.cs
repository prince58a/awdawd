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
    public partial class MainForm : Form, IBookView
    {
        private readonly BookPresenter _presenter;

        private SoundPlayer soundPlayer;
        private SoundPlayer hellyeahSound;

        public MainForm()
        {
            InitializeComponent();

            // Создаём BookLogic через Ninject и оборачиваем в модель
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            var logic = ninjectKernel.Get<BookLogic>();
            IBookModel model = new BookModel(logic);

            _presenter = new BookPresenter(this, model);

            // Привязка UI‑событий к событиям View (MVP)
            add.Click += (s, e) => AddBookRequested?.Invoke(this, EventArgs.Empty);
            edit.Click += (s, e) => EditBookRequested?.Invoke(this, EventArgs.Empty);
            del.Click += (s, e) => DeleteBookRequested?.Invoke(this, EventArgs.Empty);
            searchByIdButton.Click += (s, e) => SearchByIdRequested?.Invoke(this, EventArgs.Empty);
            resetSearchButton.Click += (s, e) => ResetSearchRequested?.Invoke(this, EventArgs.Empty);
            btnNextPage.Click += (s, e) => NextPageRequested?.Invoke(this, EventArgs.Empty);
            btnPrevPage.Click += (s, e) => PrevPageRequested?.Invoke(this, EventArgs.Empty);

            InitializeSound();
            StartFileFlagListener();

            pictureBox1.Click += pictureBox1_Click;
            button1.Click += button1_Click;
        }

        // ================= IBookView: данные от пользователя =================

        public int? SelectedBookId =>
            dataGridView1.SelectedRows.Count > 0
                ? (int?)dataGridView1.SelectedRows[0].Cells["Id"].Value
                : null;

        public string SearchIdText => idSearchTextBox.Text.Trim();

        // ================= IBookView: события =================

        public event EventHandler AddBookRequested;
        public event EventHandler EditBookRequested;
        public event EventHandler DeleteBookRequested;
        public event EventHandler SearchByIdRequested;
        public event EventHandler ResetSearchRequested;
        public event EventHandler NextPageRequested;
        public event EventHandler PrevPageRequested;

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
            gifForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PlayBackgroundSound();
            OpenGifWindow();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayHELLYEAHSound();
            BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            button1.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            label4.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            add.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            del.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            edit.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            author.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            genre.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            year.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            searchByIdButton.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            resetSearchButton.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            pictureBox1.Image = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            btnPrevPage.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
            btnNextPage.BackgroundImage = Properties.Resources.ДА_ЧЕРТ_ПОБЕРИ;
        }

        private void resetSearchButton_Click(object sender, EventArgs e)
        {
            idSearchTextBox.Text = "";

            BackgroundImage = null;
            button1.BackgroundImage = null;
            label4.BackgroundImage = null;
            add.BackgroundImage = null;
            del.BackgroundImage = null;
            edit.BackgroundImage = null;
            author.BackgroundImage = null;
            genre.BackgroundImage = null;
            year.BackgroundImage = null;
            searchByIdButton.BackgroundImage = null;
            resetSearchButton.BackgroundImage = null;
            pictureBox1.Image = Properties.Resources.jhoe;
            btnPrevPage.BackgroundImage = null;
            btnNextPage.BackgroundImage = null;
        }
    }
}
