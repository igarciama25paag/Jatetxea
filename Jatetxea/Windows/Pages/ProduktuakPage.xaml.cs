using Jatetxea.Conexions;
using Jatetxea.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class ProduktuakPage : Page
    {
        public ObservableCollection<Produktua> ProduktuakList { get; set; } = [];
        public ProduktuakPage()
        {
            DataContext = this;
            InitializeComponent();
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }
        
        private async void UpdateTable()
        {
            ProduktuakList.Clear();
            var produktuak = await JatetxeaDB.GetProduktuak();
            foreach (Produktua p in produktuak)
                ProduktuakList.Add(p);
        }

        private void ProduktuakDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EzabatuButton.IsEnabled = ProduktuakDataGrid.SelectedItem != null;
        }

        private void ProduktuakDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.Row.Item as Produktua is Produktua produktua)
                    JatetxeaDB.SaveProduktua(produktua);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void Sortu(object sender, RoutedEventArgs e)
        {
            JatetxeaDB.NewProduktua();
            Thread.Sleep(1000);
            UpdateTable();
        }

        private void Ezabatu(object sender, RoutedEventArgs e)
        {
            var produktuak = ProduktuakDataGrid.SelectedItems.Cast<Produktua>().ToList();
            string message = produktuak[0].Izena;
            if (produktuak.Count > 1)
                message = $"{produktuak[1].Izena} eta {message}";
            for (int i = 2; i < produktuak.Count; i++)
                message = $"{produktuak[1].Izena}, {message}";
            string verb = (produktuak.Count > 1) ? "dituzu" : "duzu";
            if (MessageBox.Show($"{message} ezabatu nahi al {verb}?",
                "Produktuak ezabatu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                JatetxeaDB.DeleteProduktuak(produktuak);
                Thread.Sleep(1000);
                UpdateTable();
            }
        }
    }
}
