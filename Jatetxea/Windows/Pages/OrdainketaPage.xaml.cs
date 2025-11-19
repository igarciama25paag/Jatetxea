using Jatetxea.Conexions;
using Jatetxea.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jatetxea.Windows.Pages
{
    /// <summary>
    /// Interaction logic for OrdainketaPage.xaml
    /// </summary>
    public partial class OrdainketaPage : Page
    {
        public ObservableCollection<string> ProduktuMotakList { get; } = [];
        public ObservableCollection<Produktua> ProduktuakList { get; } = [];
        private readonly object ProduktuUpdate = new();
        public ObservableCollection<string> Ticket { get; } = [];
        private readonly Dictionary<Produktua, int> ErosketaList = [];

        public OrdainketaPage()
        {
            DataContext = this;
            InitializeComponent();
            ProduktuMotakKargatu();
        }

        private async void ProduktuMotakKargatu()
        {
            foreach (var m in await JatetxeaDB.GetProduktuMotak())
                ProduktuMotakList.Add(m);
        }

        private async void UpdateProduktuak(string mota)
        {
            var produktuak = await JatetxeaDB.GetMotatakoProduktuak(mota);
            lock(ProduktuUpdate)
            {
                ProduktuakList.Clear();
                foreach (var p in produktuak)
                    ProduktuakList.Add(p);
            }
        }

        private void ProduktuMota_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (produktuMotakListBox.SelectedItem != null)
                UpdateProduktuak(produktuMotakListBox.SelectedItem.ToString());
        }

        private void KenduClick(object sender, RoutedEventArgs e)
        {
            var count = ErosketaList.Count;
            var selection = ticketListBox.SelectedIndex;
            var produktua = ErosketaList.Keys.ToArray()[selection];
            ErosketaList[produktua]--;
            if (ErosketaList[produktua] == 0)
                ErosketaList.Remove(produktua);
            UpdateTicket();
            if(ErosketaList.Count == count)
                ticketListBox.SelectedIndex = selection;
        }

        private void OrdainduEtaInprimatuClick(object sender, RoutedEventArgs e)
        {
            JatetxeaDB.RemoveProduktuStock(ErosketaList);
            MessageBox.Show($"{totalaBox.Text}-ko ordainketa totala egin da", "Ordainketa", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Thread.Sleep(500);
            ErosketaList.Clear();
            UpdateTicket();
            if(produktuMotakListBox.SelectedItem != null)
                UpdateProduktuak(produktuMotakListBox.SelectedItem.ToString());
        }

        private void Ticket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            kenduButton.IsEnabled = e.AddedItems.Count > 0;
        }

        private void DoubleClick_ProduktuakList(object sender, MouseButtonEventArgs e)
        {
            var produktua = (e.Source as ListBox)!.SelectedItem as Produktua;
            if (ErosketaList.TryGetValue(produktua, out int _))
                if (produktua.Stock > ErosketaList[produktua])
                    ErosketaList[produktua]++;
                else MessageBox.Show($"Ez dago {produktua}-(r)en stock gehiago", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (produktua.Stock > 0)
                ErosketaList.Add(produktua, 1);
            else MessageBox.Show($"Ez dago {produktua}-(r)en stock gehiago", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            UpdateTicket();
        }

        private void UpdateTicket()
        {
            Ticket.Clear();
            decimal totala = 0;
            foreach(var key in ErosketaList.Keys)
            {
                var prez = key.Prezioa;
                var count = ErosketaList[key];
                var tabs = new string('\t', 4 - key.ToString().Length/9);
                Ticket.Add($"{key}{tabs}{prez} €\t\t{count}\t{prez * count} €");
                totala += prez * count;
            }
            totalaBox.Text = totala + " €";
            ordainduButton.IsEnabled = ticketListBox.Items.Count > 0;
        }
    }
}
