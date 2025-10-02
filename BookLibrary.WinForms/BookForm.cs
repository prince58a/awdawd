using BookLibrary.Core;
using System;
using System.Windows.Forms;

namespace BookLibrary.WinForms
{
    public partial class BookForm : Form
    {
        private string[] availableGenres;

        public string BookTitle { get; private set; }
        public string BookAuthor { get; private set; }
        public int BookYear { get; private set; }
        public string BookGenre { get; private set; }

        public BookForm(string[] genres)
        {
            availableGenres = genres;
            InitializeComponent();
            InitializeGenreComboBox();
            Text = "Добавить книгу";
        }

        public BookForm(Book book, string[] genres)
        {
            availableGenres = genres;
            InitializeComponent();
            InitializeGenreComboBox();
            Text = "Редактировать книгу";
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtYear.Text = book.Year.ToString();
            cmbGenre.SelectedItem = book.Genre;
        }

        private void InitializeGenreComboBox()
        {
            cmbGenre.Items.Clear();
            cmbGenre.Items.AddRange(availableGenres);
            if (cmbGenre.Items.Count > 0)
                cmbGenre.SelectedIndex = 0;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) ||
                string.IsNullOrEmpty(txtAuthor.Text) ||
                string.IsNullOrEmpty(txtYear.Text) ||
                cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!");
                DialogResult = DialogResult.None;
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year))
            {
                MessageBox.Show("Неверный год!");
                DialogResult = DialogResult.None;
                return;
            }

            BookTitle = txtTitle.Text;
            BookAuthor = txtAuthor.Text;
            BookYear = year;
            BookGenre = cmbGenre.SelectedItem.ToString();

            DialogResult = DialogResult.OK;
        }
    }
}