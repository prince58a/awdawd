using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using BookLibrary.Wpf.Infrastructure;

namespace BookLibrary.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BookLogic _logic;

        public ObservableCollection<Book> Books { get; } = new();

        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set { _selectedBook = value; OnPropertyChanged(); }
        }

        // простые свойства для биндингов
        public string NewTitle { get; set; } = "";
        public string NewAuthor { get; set; } = "";
        public int NewYear { get; set; }
        public Genre? SelectedGenre { get; set; }

        public ObservableCollection<Genre> Genres { get; } = new();


        private const int PageSize = 10;
        private int _currentPage = 1;
        private int _totalPages = 1;

        public string PageInfo => $"Страница {_currentPage} из {_totalPages}";


        // Команды
        public ICommand AddBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        public MainViewModel(BookLogic logic)
        {
            _logic = logic;

            AddBookCommand = new RelayCommand(_ => AddBook(), _ => CanAddBook());
            DeleteBookCommand = new RelayCommand(_ => DeleteBook(), _ => SelectedBook != null);
            RefreshCommand = new RelayCommand(_ => LoadBooks());

            NextPageCommand = new RelayCommand(_ => GoNextPage(), _ => _currentPage < _totalPages);
            PrevPageCommand = new RelayCommand(_ => GoPrevPage(), _ => _currentPage > 1);
            
            LoadPage();

            LoadGenres();
        }

        private void LoadPage()
        {
            Books.Clear();

            // общее количество
            int totalBooks = _logic.GetBooksCount();   // у тебя уже есть этот метод
            _totalPages = Math.Max(1, (int)Math.Ceiling(totalBooks / (double)PageSize));

            var pageBooks = _logic.GetBooksPage(_currentPage, PageSize);  // тоже есть в логике

            foreach (var b in pageBooks)
                Books.Add(b);

            OnPropertyChanged(nameof(PageInfo));
            CommandManager.InvalidateRequerySuggested(); // обновить CanExecute для кнопок
        }

        private void GoNextPage()
        {
            if (_currentPage >= _totalPages)
                return;

            _currentPage++;
            LoadPage();
        }

        private void GoPrevPage()
        {
            if (_currentPage <= 1)
                return;

            _currentPage--;
            LoadPage();
        }


        private void LoadGenres()
        {
            Genres.Clear();
            foreach (var g in _logic.GetAvailableGenres())
                Genres.Add(g);
        }

        private void LoadBooks()
        {
            Books.Clear();
            foreach (var b in _logic.GetAllBooks())
                Books.Add(b);
        }

        private bool CanAddBook()
        {
            return !string.IsNullOrWhiteSpace(NewTitle)
                   && !string.IsNullOrWhiteSpace(NewAuthor)
                   && SelectedGenre != null
                   && NewYear > 0;
        }

        private void AddBook()
        {
            var result = _logic.CreateBook(NewTitle, NewAuthor, NewYear, SelectedGenre!.Id);
            if (result.Success)
            {
                Books.Add(result.Book);
                // можно очистить поля
                NewTitle = "";
                NewAuthor = "";
                OnPropertyChanged(nameof(NewTitle));
                OnPropertyChanged(nameof(NewAuthor));
            }
            // сообщения об ошибке — либо через отдельное свойство ErrorMessage,
            // либо через MessageBox в View (по требованиям препода).
        }

        private void DeleteBook()
        {
            if (SelectedBook == null)
                return;

            if (_logic.DeleteBook(SelectedBook.Id))
                Books.Remove(SelectedBook);
        }
    }
}
