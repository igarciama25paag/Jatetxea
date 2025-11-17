using ErreserbakLibrary;
using System;
using System.Collections;
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

namespace Jatetxea.Windows.Pages
{
    /// <summary>
    /// Interaction logic for ErreserbakPage.xaml
    /// </summary>
    public partial class ErreserbakPage : Page
    {
        private readonly ArrayList mahaiakList = [];
        public ErreserbakPage()
        {
            InitializeComponent();
            MahaiakSortu();
        }

        private void MahaiakSortu()
        {
            int count = 0;
            int lerroak = 2;
            int luzeera = 5;
            for (int i = 0; i < lerroak; i++)
            {
                StackPanel lerro = new();
                mahaiak.Children.Add(lerro);
                lerro.Orientation = Orientation.Horizontal;

                for (int j = 0; j < luzeera; j++)
                {
                    count++;
                    Mahaia mahaia = new(count.ToString(), false);
                    mahaiakList.Add(mahaia);
                    lerro.Children.Add(mahaia);
                    mahaia.Margin = new(10);
                }
            }
        }

        private void ErreserbatuClick(object sender, RoutedEventArgs e)
        {

        }

        private void ErreserbaEzabatuClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
