using Jatetxea.Conexions;
using Jatetxea.Windows.Pages;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

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
            if (MessageBox.Show("Saioa itxi nahi al duzu?",
                "Saioa itxi",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                User.Login(null);
                new LoginWindow().Show();
                Close();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Aplikazioa itxi nahi al duzu?",
                "Irten",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            Close();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            produktuakButton.Background = Brushes.LightGray;
            erabiltzaileakButton.Background = Brushes.LightGray;

            switch (MainFrame.NavigationService.Content)
            {
                case ProduktuakPage _:
                    produktuakButton.Background = Brushes.LightGreen;
                    break;
                case ErabiltzaileakPage _:
                    erabiltzaileakButton.Background = Brushes.LightGreen;
                    break;
            }
            
        }
    }
}
