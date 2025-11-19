using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Jatetxea.Data
{
    public record Produktua(int Id, string Izena, string Mota, decimal Prezioa, int Stock)
    {
        public string DisplayName { get; set; } = $"{Stock} - {Izena}";
        public override string ToString() => Izena;
    }
}
