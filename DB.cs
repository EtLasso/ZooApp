using System.Data;
using MySql.Data.MySqlClient;

namespace ZooApp
{
    /// <summary>
    /// Basis-Datenbankklasse für alle MySQL-Operationen
    /// Stellt grundlegende Methoden für Verbindung und Datenbankabfragen bereit
    /// </summary>
    public class DB
    {
        /// <summary>
        /// Verbindungsstring zur MySQL-Datenbank
        /// Server: localhost, Datenbank: zoo_verwaltung, User: root, kein Passwort
        /// </summary>
        private readonly string connectionString = "server=localhost;database=zoo_verwaltung;uid=root;pwd=;";

        /// <summary>
        /// Testet die Verbindung zur Datenbank
        /// </summary>
        /// <returns>true wenn Verbindung erfolgreich, false bei Fehler</returns>
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

        /// <summary>
        /// Führt eine SELECT-Abfrage aus und gibt die Ergebnisse als DataTable zurück
        /// Verwendet parametrisierte Queries zur Vermeidung von SQL-Injection
        /// </summary>
        /// <param name="sql">SQL SELECT-Befehl</param>
        /// <param name="parameters">Parameter für die Abfrage im Format ("@name", wert)</param>
        /// <returns>DataTable mit den Abfrageergebnissen</returns>
        /// <example>
        /// DataTable dt = db.Get("SELECT * FROM Kontinent WHERE kID=@id", ("@id", 5));
        /// </example>
        public DataTable Get(string sql, params (string, object)[] parameters)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Sichere Parameter hinzufügen (verhindert SQL-Injection)
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

        /// <summary>
        /// Führt INSERT, UPDATE oder DELETE-Befehle aus
        /// Verwendet parametrisierte Queries zur Sicherheit
        /// </summary>
        /// <param name="sql">SQL-Befehl (INSERT, UPDATE, DELETE)</param>
        /// <param name="parameters">Parameter für den Befehl im Format ("@name", wert)</param>
        /// <returns>Anzahl der betroffenen Zeilen</returns>
        /// <example>
        /// int affected = db.Execute("INSERT INTO Kontinent (Kbezeichnung) VALUES (@name)", ("@name", "Europa"));
        /// </example>
        public int Execute(string sql, params (string, object)[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Sichere Parameter hinzufügen
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Führt eine Abfrage aus und gibt einen einzelnen Wert zurück
        /// Nützlich für COUNT, MAX, MIN, LAST_INSERT_ID() etc.
        /// </summary>
        /// <param name="sql">SQL-Befehl der einen einzelnen Wert zurückgibt</param>
        /// <param name="parameters">Parameter für den Befehl</param>
        /// <returns>Einzelner Wert aus der Datenbank</returns>
        /// <example>
        /// object count = db.Scalar("SELECT COUNT(*) FROM Tiere");
        /// int tierAnzahl = Convert.ToInt32(count);
        /// </example>
        public object Scalar(string sql, params (string, object)[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // Sichere Parameter hinzufügen
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
