using System.Data;
using MySql.Data.MySqlClient;

namespace ZooApp
{
    /// <summary>
    /// Basis-Datenbankklasse für MySQL-Operationen.
    /// Stellt grundlegende CRUD-Operationen (Create, Read, Update, Delete) bereit.
    /// </summary>
    public class DB
    {
        // Verbindungsstring zur lokalen MySQL-Datenbank
        // Format: server=Host;database=Datenbankname;uid=Benutzer;pwd=Passwort
        private readonly string connectionString = "server=localhost;database=zoo_verwaltung;uid=root;pwd=;";

        /// <summary>
        /// Testet die Verbindung zur Datenbank.
        /// </summary>
        /// <returns>True wenn Verbindung erfolgreich, sonst False</returns>
        public bool Test()
        {
            try
            {
                // Versuche Verbindung zu öffnen
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    return true;  // Verbindung erfolgreich
                }
            }
            catch
            {
                return false;  // Verbindung fehlgeschlagen
            }
        }

        /// <summary>
        /// Führt eine SELECT-Abfrage aus und gibt die Ergebnisse als DataTable zurück.
        /// </summary>
        /// <param name="sql">SQL-SELECT-Befehl</param>
        /// <param name="parameters">Optionale Parameter als Tupel (Name, Wert)</param>
        /// <returns>DataTable mit den Abfrageergebnissen</returns>
        public DataTable Get(string sql, params (string, object)[] parameters)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parameter hinzufügen (verhindert SQL-Injection!)
                    // Beispiel: Get("SELECT * FROM Tiere WHERE tierID = @id", ("@id", 5))
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    // Daten in DataTable laden
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Führt INSERT-, UPDATE- oder DELETE-Befehle aus.
        /// </summary>
        /// <param name="sql">SQL-Befehl (INSERT, UPDATE oder DELETE)</param>
        /// <param name="parameters">Optionale Parameter als Tupel (Name, Wert)</param>
        /// <returns>Anzahl betroffener Zeilen</returns>
        public int Execute(string sql, params (string, object)[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parameter hinzufügen
                    // Beispiel: Execute("DELETE FROM Tiere WHERE tierID = @id", ("@id", 5))
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    // Befehl ausführen und Anzahl betroffener Zeilen zurückgeben
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Führt eine Abfrage aus und gibt einen einzelnen Wert zurück.
        /// Nützlich für COUNT, MAX, MIN, SUM, etc.
        /// </summary>
        /// <param name="sql">SQL-Befehl der einen einzelnen Wert zurückgibt</param>
        /// <param name="parameters">Optionale Parameter als Tupel (Name, Wert)</param>
        /// <returns>Der erste Wert der ersten Zeile</returns>
        public object Scalar(string sql, params (string, object)[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Parameter hinzufügen
                    // Beispiel: Scalar("SELECT COUNT(*) FROM Tiere")
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    // Ersten Wert zurückgeben
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
