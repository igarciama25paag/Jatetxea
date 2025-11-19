using Jatetxea.Data;
using Npgsql;
using System.Diagnostics;
using System.Windows;
using static Jatetxea.Data.Erreserba;

namespace Jatetxea.Conexions
{
    static class JatetxeaDB
    {
        private const string HOST = "localhost";
        private const string PORT = "5432";
        private const string USERNAME = "admin";
        private const string PASSWORD = "Peio";
        private const string DATABASE = "jatetxea";

        private const string CONNECTION = "" +
                $"Host={HOST};" +
                $"Port={PORT};" +
                $"Username={USERNAME};" +
                $"Password={PASSWORD};" +
                $"Database={DATABASE}";

        // Trace.WriteLine(); for logs

        private static void DBDispatch(string query)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await using var dataSource = NpgsqlDataSource.Create(CONNECTION);
                    dataSource.CreateCommand(query).ExecuteNonQuery();
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show(e.Message, "SQL exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    Trace.WriteLine("SQL exception: " + e.Message);
                }
            });
        }

        private delegate Task<T> Request<T>(NpgsqlDataSource dataSource);
        private static async Task<T> DBRequest<T>(Request<T> request)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(CONNECTION);
                return await request(dataSource);
            } catch (NpgsqlException e)
            {
                MessageBox.Show(e.Message, "SQL exception", MessageBoxButton.OK, MessageBoxImage.Error);
                Trace.WriteLine("SQL exception: " + e.Message);
                return default!;
            }
        }

        /**
         * LOGIN
         * **/

        public static async Task<Erabiltzailea> GetErabiltzailea(string user, string pass)
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT * FROM \"Erabiltzaileak\" " +
                    $"WHERE izena = '{user}' " +
                    $"AND pasahitza = '{pass}'"
                    );
                await using var reader = await cmd.ExecuteReaderAsync();
                {
                    await reader.ReadAsync();

                    return new Erabiltzailea(
                        reader.GetInt16(0),
                        reader.GetString(1).Trim(),
                        reader.GetString(2).Trim(),
                        Enum.Parse<Erabiltzailea.ErabiltzaileMotak>(reader.GetString(3).Trim())
                        );
                }
            });
        }

        /**
         * PRODUKTUAK
         * **/

        public static async Task<List<Produktua>> GetProduktuak()
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT * FROM \"Produktuak\" " +
                    "ORDER BY id"
                    );
                await using var reader = await cmd.ExecuteReaderAsync();
                {
                    List<Produktua> produktuak = [];
                    while (await reader.ReadAsync())
                        produktuak.Add(new(
                            reader.GetInt16(0),
                            reader.GetString(1).Trim(),
                            reader.GetString(2).Trim(),
                            reader.GetDecimal(3),
                            reader.GetInt16(4)
                            ));

                    return produktuak;
                }
            });
        }

        public static void SaveProduktua(Produktua produktua)
        {
            DBDispatch(
                "UPDATE \"Produktuak\" " +
                $"SET izena = '{produktua.Izena}', " +
                $"mota = '{produktua.Mota}', " +
                $"prezioa = {produktua.Prezioa.ToString().Replace(',','.')}, " +
                $"stock = {produktua.Stock} " +
                $"WHERE id = {produktua.Id}"
                );
        }
        
        public static void DeleteProduktuak(List<Produktua> produktuak)
        {
            if (produktuak.Count > 0) {
                string ids = $"id = {produktuak[0].Id}";
                for (int i = 1; i < produktuak.Count; i++)
                    ids += $" OR id = {produktuak[i].Id}";

                DBDispatch(
                    "DELETE FROM \"Produktuak\" " +
                    $"WHERE {ids}"
                    );
            }
        }

        public static void NewProduktua()
        {
            DBDispatch(
                "INSERT INTO \"Produktuak\" (izena,mota,prezioa,stock) " +
                $"VALUES ('produktu_berria','produktu_mota',0,0)"
                );
        }

        /**
         * ERABILTZAILEAK
         * **/

        public static async Task<List<Erabiltzailea>> GetErabiltzaileak()
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT * FROM \"Erabiltzaileak\" " +
                    "ORDER BY mota"
                    );
                await using var reader = await cmd.ExecuteReaderAsync();
                {
                    List<Erabiltzailea> erabiltzaileak = [];
                    while (await reader.ReadAsync())
                        erabiltzaileak.Add(new(
                            reader.GetInt16(0),
                            reader.GetString(1).Trim(),
                            reader.GetString(2).Trim(),
                            Enum.Parse<Erabiltzailea.ErabiltzaileMotak>(reader.GetString(3).Trim())
                            ));

                    return erabiltzaileak;
                }
            });
        }

        public static void SaveErabiltzailea(Erabiltzailea erabiltzailea)
        {
            DBDispatch(
                "UPDATE \"Erabiltzaileak\" " +
                $"SET izena = '{erabiltzailea.Izena}', " +
                $"pasahitza = '{erabiltzailea.Pasahitza}', " +
                $"mota = '{erabiltzailea.Mota}' " +
                $"WHERE id = '{erabiltzailea.Id}'"
                );
        }

        public static void NewErabiltzailea(List<Erabiltzailea> erabiltzaileak)
        {
            int max = 0;
            foreach (Erabiltzailea u in erabiltzaileak)
                if (u.Izena.StartsWith("erabiltzaile_berria"))
                {
                    string n = u.Izena.Replace("erabiltzaile_berria", "");
                    if (int.TryParse(n, out int num) && num > max)
                        max = num;
                }

            DBDispatch(
                "INSERT INTO \"Erabiltzaileak\" (izena,pasahitza,mota) " +
                $"VALUES ('{"erabiltzaile_berria" + (max + 1)}','pasahitza','arrunta')"
                );
        }

        public static void DeleteErabiltzaileak(List<Erabiltzailea> erabiltzaileak)
        {
            if (erabiltzaileak.Count > 0)
            {
                string ids = $"id = {erabiltzaileak[0].Id}";
                for (int i = 1; i < erabiltzaileak.Count; i++)
                    ids += $" OR id = {erabiltzaileak[i].Id}";

                DBDispatch(
                    "DELETE FROM \"Erabiltzaileak\" " +
                    $"WHERE {ids}"
                    );
            }
        }

        /**
         * ERRESERBAK
         * **/

        public static async Task<List<Erreserba>> GetErreserbak(DateTime data, Erreserba.Janordua janordua)
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT U.izena, E.mahaia FROM \"Erreserbak\" E " +
                    "INNER JOIN \"Erabiltzaileak\" U " +
                    "ON E.erabiltzailea = U.id " +
                    $"WHERE data = '{data:yyyy-MM-dd}' " +
                    $"AND janordua = '{janordua}' " +
                    $"ORDER BY mahaia"
                    );
                await using var reader = await cmd.ExecuteReaderAsync();
                {
                    Dictionary<string, List<string>> map = [];
                    while (await reader.ReadAsync())
                    {
                        string username = reader.GetString(0).Trim();
                        if (!map.ContainsKey(username)) map.Add(username, []);
                        map[username].Add(reader.GetString(1));
                    }

                    List<Erreserba> erreserbak = [];
                    foreach (string key in map.Keys)
                        erreserbak.Add(new(key, map[key]));

                    return erreserbak;
                }
            });
        }

        public static void SaveErreserba(DateTime data, Erreserba.Janordua janordua, Erreserba erreserba)
        {
            foreach (var m in erreserba.Mahaiak)
                DBDispatch(
                    "INSERT INTO \"Erreserbak\" (data,janordua,mahaia,erabiltzailea)" +
                    $"VALUES ('{data:yyyy-MM-dd}', '{janordua}', '{m}', {User.user!.Id})"
                    );
        }

        public static void DeleteErreserba(DateTime data, Erreserba.Janordua janordua)
        {
            DBDispatch(
                "DELETE FROM \"Erreserbak\" " +
                $"WHERE data = '{data:yyyy-MM-dd}' " +
                $"AND janordua = '{janordua}' " +
                $"AND erabiltzailea = {User.user!.Id}"
                );
        }

        /**
         * ORDAINKETA
         * **/

        public static async Task<List<string>> GetProduktuMotak()
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT mota FROM \"Produktuak\" " +
                    "GROUP BY mota " +
                    "ORDER BY mota"
                    );
                await using var reader = await cmd.ExecuteReaderAsync();
                {
                    List<string> motak = [];
                    while (await reader.ReadAsync())
                        motak.Add(reader.GetString(0));
                    return motak;
                }
            });
        }

        public static async Task<List<Produktua>> GetMotatakoProduktuak(string mota)
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT * FROM \"Produktuak\" " +
                    $"WHERE mota = '{mota}' " +
                    "ORDER BY id"
                    );
                await using var reader = await cmd.ExecuteReaderAsync();
                {
                    List<Produktua> produktuak = [];
                    while (await reader.ReadAsync())
                        produktuak.Add(new(
                            reader.GetInt16(0),
                            reader.GetString(1).Trim(),
                            reader.GetString(2).Trim(),
                            reader.GetDecimal(3),
                            reader.GetInt16(4)
                            ));

                    return produktuak;
                }
            });
        }

        public static void RemoveProduktuStock(Dictionary<Produktua, int> produktuak)
        {
            foreach (var p in produktuak.Keys)
                DBDispatch(
                    "UPDATE \"Produktuak\" " +
                    $"SET stock = stock - {produktuak[p]} " +
                    $"WHERE id = {p.Id}"
                    );
        }
    }
}