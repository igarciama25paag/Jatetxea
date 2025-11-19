using ErreserbakLibrary;
using Jatetxea.Conexions;
using Jatetxea.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Jatetxea.Windows.Pages
{
    /// <summary>
    /// Interaction logic for ErreserbakPage.xaml
    /// </summary>
    public partial class ErreserbakPage : Page
    {
        private readonly bool initiated = false;
        private readonly List<Mahaia> mahaiakList = [];
        public ObservableCollection<Erreserba> ErreserbakList { get; set; } = [];

        public ErreserbakPage()
        {
            DataContext = this;
            InitializeComponent();
            MahaiakSortu();
            initiated = true;
            UpdateMahaiak();
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
                    foreach (var e in ErreserbakList)
                        if(e.Mahaiak.Contains(mahaia.Izena))
                            mahaia.Erreserbatu(true);
                }
            }
        }

        private Erreserba.Janordua GetJanordua()
        {
            if (bazkariaCheckBox is not null && bazkariaCheckBox.IsChecked.GetValueOrDefault())
                return Erreserba.Janordua.bazkaria;
            else
                return Erreserba.Janordua.afaria;
        }

        private void ErreserbatuClick(object sender, RoutedEventArgs e)
        {
            List<Mahaia> trueMahaiak = [];
            foreach (var m in mahaiakList)
                if(m.Aukeratuta) trueMahaiak.Add(m);

            if(trueMahaiak.Count > 0)
            {
                string message = trueMahaiak[0].Izena;
                string plur = "";
                string verb = "duzu";
                if (trueMahaiak.Count > 1)
                {
                    message = $"{trueMahaiak[1].Izena} eta {message}";
                    plur = "k";
                    verb = "dituzu";

                    for (int i = 2; i < trueMahaiak.Count; i++)
                        message = $"{trueMahaiak[i].Izena}, {message}";
                }   
                if (MessageBox.Show($"{message} mahia{plur} erreserbatu nahi al {verb}?",
                    $"Mahaia{plur} erreserbatu",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Erreserba ersBerria = new(User.user!.Izena, []);
                    foreach (var m in trueMahaiak)
                    {
                        m.Erreserbatu(true);
                        ersBerria.Mahaiak.Add(m.Izena);
                    }
                    JatetxeaDB.SaveErreserba(datePicker.SelectedDate!.Value, GetJanordua(), ersBerria);
                }
            } else MessageBox.Show("Ez da erreserbatzeko mahirik aukeratu",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Thread.Sleep(500);
            UpdateMahaiak();
        }

        private void Erreserbak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ezabatuButton.IsEnabled = listBoxErreserbak.SelectedItem != null;
        }

        private void ErreserbaEzabatuClick(object sender, RoutedEventArgs e)
        {
            if (listBoxErreserbak.SelectedItem is Erreserba erreserba)
                if (erreserba.UserName == User.user!.Izena)
                {
                    if (MessageBox.Show($"Erreserba ezabatu nahi al duzu?",
                        $"Erreserba ezabatu",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                        JatetxeaDB.DeleteErreserba(datePicker.SelectedDate!.Value, GetJanordua());
                }
                else MessageBox.Show("Ezin duzu zurea ez den erreserba bat ezabatu",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Thread.Sleep(500);
            UpdateMahaiak();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initiated)
            {
                datePicker.SelectedDate ??= DateTime.Today;
                UpdateMahaiak();
            }
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (initiated) UpdateMahaiak();
        }

        private async void UpdateMahaiak()
        {
            DateTime date = datePicker.SelectedDate!.Value;

            ErreserbakList.Clear();
            var erreserbak = await JatetxeaDB.GetErreserbak(date, GetJanordua());
            foreach (var m in mahaiakList) m.Erreserbatu(false);
            foreach (var erreserba in erreserbak)
            {
                ErreserbakList.Add(erreserba);
                foreach (var i in erreserba.Mahaiak)
                    foreach (var m in mahaiakList)
                        if(m.Izena == i.ToString()) m.Erreserbatu(true);
            }
        }
    }
}
