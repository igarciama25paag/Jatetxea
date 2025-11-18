using Jatetxea.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatetxea.Conexions
{
    internal static class User
    {
        public static Erabiltzailea? user { get; set; }

        public static void Login(Erabiltzailea? erabiltzailea) => user = erabiltzailea;

        public static Erabiltzailea.ErabiltzaileMotak? GetUserType() => user?.Mota;
    }
}
