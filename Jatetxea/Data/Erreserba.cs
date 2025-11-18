using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatetxea.Data
{
    public record Erreserba(string UserName, List<string> Mahaiak)
    {
        public override string ToString()
        {
            if(Mahaiak.Count == 0) return UserName;
            string list = Mahaiak[0];
            for(int i = 1; i < Mahaiak.Count; i++)
                list += ", " + Mahaiak[i];
            return $"{UserName}: {list}";
        }
        public enum Janordua
        {
            bazkaria,
            afaria
        }
    }
}
