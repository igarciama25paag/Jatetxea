using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatetxea.Data
{
    public record Produktua(int Id, string Izena, string Mota, decimal Prezioa, int Stock)
    {
    }
}
