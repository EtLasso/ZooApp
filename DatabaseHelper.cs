using MySql.Data.MySqlClient;
using System.Data;

namespace ZooApp
{
    public class DB
    {
        private readonly string connStr =
            "server=localhost;port=3306;database=zoo_verwaltung;uid=root;pwd=;";

        // ----------------------------------------------------------
        // Verbindung testen
        // ----------------------------------------------------------
        public bool Test()
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
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

        // ----------------------------------------------------------
        // SELECT → DataTable
        // ----------------------------------------------------------
        public DataTable Get(string sql, params (string, object)[] parameters)
        {
            DataTable dt = new DataTable();

            using (var conn = new MySqlConnection(connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                foreach ((string name, object value) in parameters)
                    cmd.Parameters.AddWithValue(name, value);

                conn.Open();

                using (var ad = new MySqlDataAdapter(cmd))
                    ad.Fill(dt);
            }

            return dt;
        }

        // ----------------------------------------------------------
        // INSERT / UPDATE / DELETE → Anzahl betroffene Zeilen
        // ----------------------------------------------------------
        public int Execute(string sql, params (string, object)[] parameters)
        {
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                foreach ((string name, object val) in parameters)
                    cmd.Parameters.AddWithValue(name, val);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
