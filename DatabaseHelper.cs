using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace ZooApp
{
    public class DatabaseHelper
    {
        private string connectionString;
        public DatabaseHelper()
        {
            // XAMPP Standard-Verbindung
            connectionString = "server=localhost;port=3306;database=zoo_verwaltung;uid=root;pwd=;";
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
        // Methode zum Testen der Verbindung
        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            

        }
        // Allgemeine Methode zum Ausführen von Non-Query-Befehlen
        public int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        // Methode zum Abrufen von Daten
        public DataTable GetData(string query, MySqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;

        }
    }
}
