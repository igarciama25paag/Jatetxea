using Jatetxea.Conexions;
using Jatetxea.Windows.Pages;
using System.Windows;

namespace Jatetxea.Windows
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Produktuak(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProduktuakPage());
        }

        private void Erabiltzaileak(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ErabiltzaileakPage());
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            User.Login(null);
            new LoginWindow().Show();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
