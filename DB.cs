using System.Data;
using MySql.Data.MySqlClient;

namespace ZooApp
{
    // Basis-Datenbankklasse für MySQL-Operationen
    public class DB
    {
        // Verbindungsstring zur MySQL-Datenbank
        private readonly string connectionString = "server=localhost;database=zoo_verwaltung;uid=root;pwd=;";

        // Testet Datenbankverbindung
        public bool Test()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Führt SELECT-Abfrage aus und gibt DataTable zurück
        public DataTable Get(string sql, params (string, object)[] parameters)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parameter hinzufügen (verhindert SQL-Injection)
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Führt INSERT, UPDATE oder DELETE aus
        public int Execute(string sql, params (string, object)[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parameter hinzufügen
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Gibt einzelnen Wert zurück (z.B. COUNT, MAX, MIN)
        public object Scalar(string sql, params (string, object)[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parameter hinzufügen
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
