using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatetxea.Data
{
    record Erabiltzailea(int Id, string Izena, string Pasahitza, Erabiltzailea.ErabiltzaileMotak Mota)
    {
        public enum ErabiltzaileMotak
        {
            admin,
            arrunta
        }
    }
}
