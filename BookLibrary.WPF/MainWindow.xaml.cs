using BookLibrary.Core;
using BookLibrary.Wpf.Infrastructure;
using BookLibrary.Wpf.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BookLibrary.WPF
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.HandleKey(e.Key);

            base.OnKeyDown(e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}