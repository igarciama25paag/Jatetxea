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
        public ObservableCollection<Produktua> ProduktuakList = [];
        public ProduktuakPage()
        {
            DataContext = this;
            InitializeComponent();
            Loaded += MyPage_Loaded;
        }

        private async void MyPage_Loaded(object sender, RoutedEventArgs e)
        {
            ProduktuakList = new(await JatetxeaDB.GetProduktuak());
        }
    }
}
