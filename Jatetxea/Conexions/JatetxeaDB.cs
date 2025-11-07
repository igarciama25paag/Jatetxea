using Jatetxea.Data;
using Npgsql;

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

        private delegate void Dispatch(NpgsqlDataSource dataSource);
        private static async void DBDispatch(Dispatch dispatch)
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(CONNECTION);
                dispatch(dataSource);
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("SQL exception: " + e.Message);
            }
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
                Console.WriteLine("SQL exception: " + e.Message);
                return default!;
            }
        }

        public static async Task<User.UserTypes> GetUserType(string user, string pass)
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT mota FROM \"Erabiltzaileak\" " +
                    $"WHERE \"erabiltzailea\" = '{user}' " +
                    $"AND \"pasahitza\" = '{pass}'"
                    );
                {
                    await using var reader = await cmd.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    return Enum.Parse<User.UserTypes>(reader.GetString(0));
                }
            });
        }

        public static async Task<List<Produktua>> GetProduktuak()
        {
            return await DBRequest(async dataSource => {
                await using var cmd = dataSource.CreateCommand(
                    "SELECT *  FROM \"Produktuak\""
                    );
                {
                    await using var reader = await cmd.ExecuteReaderAsync();

                    List<Produktua> produktuak = [];
                    while (await reader.ReadAsync())
                        produktuak.Add(new(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDecimal(3),
                            reader.GetInt32(4)
                            ));

                    return produktuak;
                }
            });
        }
    }
}