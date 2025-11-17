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
        public int LetraTamaina { get; set; } = 0;
        public int Tamaina { get; set; } = 0;
        public Mahaia()
        {
            InitializeComponent();
        }
    }

}
