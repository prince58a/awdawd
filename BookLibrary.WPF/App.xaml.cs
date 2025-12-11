using BookLibrary.Core;
using BookLibrary.DataAccessLayer;
using BookLibrary.Wpf.ViewModels;
using System.Windows;

namespace BookLibrary.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new BookDbContext("C:\\Users\\dshel\\Документы\\awdawd\\BookLibrary.db");
            var bookRepo = new EntityRepository(dbContext);
            var genreRepo = new EntityGenreRepository(dbContext);
            var logic = new BookLogic(bookRepo, genreRepo);

            var vm = new MainViewModel(logic);
            var window = new MainWindow { DataContext = vm };

            window.Show();
        }
    }
}
