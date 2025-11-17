using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErreserbakLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Mahaia : UserControl
    {
        public bool Erreserbatuta { get; set; } = false;
        public bool Aukeratuta { get; set; } = false;
        public string Izena { get; set; } = string.Empty;
        public int LetraTamaina { get; set; } = 16;
        public int Tamaina { get; set; } = 60;
        public Mahaia()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Mahaia(string izena, bool erreserbatuta)
        {
            InitializeComponent();
            DataContext = this;
            Izena = izena;
            Erreserbatu(erreserbatuta);
        }

        private void MahaiaClick(object sender, RoutedEventArgs e)
        {
            if (!Erreserbatuta) Aukeratu(!Aukeratuta);
        }

        private void Aukeratu(bool aukeratu)
        {
            Aukeratuta = aukeratu;
            if (Aukeratuta) mahaia.Background = Brushes.LightGreen;
            else mahaia.Background = Brushes.LightGray;
        }

        public void Erreserbatu(bool erreserbatu)
        {
            Erreserbatuta = erreserbatu;
            Aukeratu(false);
            IsEnabled = !Erreserbatuta;
        }
    }

}
