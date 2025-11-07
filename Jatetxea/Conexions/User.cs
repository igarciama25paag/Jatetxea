using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatetxea.Conexions
{
    internal static class User
    {
        private static string? Name;
        private static UserTypes? UserType;

        public static void SetUser(string name, UserTypes type)
        {
            ArgumentNullException.ThrowIfNull(name);

            Name = name;
            UserType = type;
        }

        public static string? GetName() => Name;
        public static UserTypes? GetUserType() => UserType;

        public enum UserTypes
        {
            admin,
            arrunta
        }
    }
}
