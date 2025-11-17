using Jatetxea.Conexions;
using Jatetxea.Windows.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jatetxea.Windows
{
    /// <summary>
    /// Interaction logic for ArruntaWindow.xaml
    /// </summary>
    public partial class ArruntaWindow : Window
    {
        public ArruntaWindow()
        {
            InitializeComponent();
        }
        private void Ordainketa(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdainketaPage());
        }

        private void Erreserbak(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ErreserbakPage());
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

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            ordainketaButton.Background = Brushes.LightGray;
            erreserbakButton.Background = Brushes.LightGray;

            switch (MainFrame.NavigationService.Content)
            {
                case OrdainketaPage _:
                    ordainketaButton.Background = Brushes.LightGreen;
                    break;
                case ErreserbakPage _:
                    erreserbakButton.Background = Brushes.LightGreen;
                    break;
            }

        }
    }
}
