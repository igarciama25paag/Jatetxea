using Jatetxea.Data;
using Npgsql;
using System.Diagnostics;
using System.Windows;

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

        private delegate void Dispatch(NpgsqlDataSource dataSource);
        private static void DBDispatch(Dispatch dispatch)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await using var dataSource = NpgsqlDataSource.Create(CONNECTION);
                    dispatch(dataSource);
                }
                catch (NpgsqlException e)
                {
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
                Trace.WriteLine("SQL exception: " + e.Message);
                return default!;
            }
        }

        public static async Task<User.UserTypes> GetUserType(string user, string pass)
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT mota FROM \"Erabiltzaileak\" " +
                    $"WHERE erabiltzailea = {user} " +
                    $"AND pasahitza = {pass}"
                    );
                {
                    await using var reader = await cmd.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    return Enum.Parse<User.UserTypes>(reader.GetString(0).Trim());
                }
            });
        }

        public static async Task<List<Produktua>> GetProduktuak()
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT * FROM \"Produktuak\" " +
                    "ORDER BY id"
                    );
                {
                    await using var reader = await cmd.ExecuteReaderAsync();

                    List<Produktua> produktuak = [];
                    while (await reader.ReadAsync())
                        produktuak.Add(new(
                            reader.GetInt32(0),
                            reader.GetString(1).Trim(),
                            reader.GetString(2).Trim(),
                            reader.GetDecimal(3),
                            reader.GetInt32(4)
                            ));

                    return produktuak;
                }
            });
        }

        public static void SaveProduktua(Produktua produktua)
        {
            DBDispatch(dataSource => {
                dataSource.CreateCommand(
                    "UPDATE \"Produktuak\" " +
                    $"SET izena = '{produktua.Izena}', " +
                    $"mota = '{produktua.Mota}', " +
                    $"prezioa = {produktua.Prezioa.ToString().Replace(',','.')}, " +
                    $"stock = {produktua.Stock} " +
                    $"WHERE id = {produktua.Id}"
                    ).ExecuteNonQuery();
            });
        }
        
        public static void DeleteProduktuak(List<Produktua> produktuak)
        {
            if (produktuak.Count > 0) {
                string ids = $"id = {produktuak[0].Id}";
                for (int i = 1; i < produktuak.Count; i++)
                    ids += $" OR id = {produktuak[i].Id}";

                DBDispatch(dataSource => {
                    dataSource.CreateCommand(
                        "DELETE FROM \"Produktuak\" " +
                        $"WHERE {ids}"
                        ).ExecuteNonQuery();
                });
            }
        }
        public static void NewProduktua()
        {
            DBDispatch(dataSource => {
                dataSource.CreateCommand(
                    "INSERT INTO \"Produktuak\" (izena,mota,prezioa,stock) " +
                    $"VALUES ('produktu_berria','produktu_mota',0,0)"
                    ).ExecuteNonQuery();
            });
        }
    }
}