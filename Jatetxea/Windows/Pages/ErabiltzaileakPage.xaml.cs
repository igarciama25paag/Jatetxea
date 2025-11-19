using Jatetxea.Conexions;
using Jatetxea.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using static Jatetxea.Data.Erabiltzailea;

namespace Jatetxea.Windows.Pages
{
    /// <summary>
    /// Interaction logic for ErabiltzaileakPage.xaml
    /// </summary>
    public partial class ErabiltzaileakPage : Page
    {
        ObservableCollection<Erabiltzailea> ErabiltzaileakList { get; set; } = [];
        public ErabiltzaileakPage()
        {
            DataContext = this;
            InitializeComponent();
            Loaded += Page_Loaded;
            ErabiltzaileakDataGrid.ItemsSource = ErabiltzaileakList;
            colMota.ItemsSource = new List<ErabiltzaileMotak>
            {
                ErabiltzaileMotak.admin,
                ErabiltzaileMotak.arrunta
            };
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private async void UpdateTable()
        {
            ErabiltzaileakList.Clear();
            var erabiltzaileak = await JatetxeaDB.GetErabiltzaileak();
            foreach (Erabiltzailea u in erabiltzaileak)
                ErabiltzaileakList.Add(u);
        }

        private void ErabiltzaileakDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EzabatuButton.IsEnabled = ErabiltzaileakDataGrid.SelectedItem != null;
        }

        private void ErabiltzaileakDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.Row.Item as Erabiltzailea is Erabiltzailea erabiltzailea)
                    JatetxeaDB.SaveErabiltzailea(erabiltzailea);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void Berria(object sender, RoutedEventArgs e)
        {
            JatetxeaDB.NewErabiltzailea(ErabiltzaileakList.ToList());
            Thread.Sleep(500);
            UpdateTable();
        }

        private void Ezabatu(object sender, RoutedEventArgs e)
        {
            var erabiltzaileak = ErabiltzaileakDataGrid.SelectedItems.Cast<Erabiltzailea>().ToList();
            string message = erabiltzaileak[0].Izena;
            string plur = "";
            string verb = "duzu";
            if (erabiltzaileak.Count > 1)
            {
                message = $"{erabiltzaileak[1].Izena} eta {message}";
                plur = "k";
                verb = "dituzu";

                for (int i = 2; i < erabiltzaileak.Count; i++)
                    message = $"{erabiltzaileak[i].Izena}, {message}";
            }

            if (MessageBox.Show($"{message} erabiltzailea{plur} ezabatu nahi al {verb}?",
                $"Erabiltzailea{plur} ezabatu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                JatetxeaDB.DeleteErabiltzaileak(erabiltzaileak);
                Thread.Sleep(500);
                UpdateTable();
            }
        }
    }
}
