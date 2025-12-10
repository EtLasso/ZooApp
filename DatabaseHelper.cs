using System.Data;

namespace ZooApp
{
    /// <summary>
    /// Spezialisierte Datenbankklasse für Zoo-spezifische Abfragen
    /// Erweitert die Basis-DB-Klasse um komplexere Zoo-Operationen
    /// Enthält Methoden für Futter, Bestellungen, Fütterungspläne und Statistiken
    /// </summary>
    public class ZooDB : DB
    {
        #region Futter-Verwaltung

        /// <summary>
        /// Gibt eine Liste aller Futtersorten zurück, die nachbestellt werden müssen
        /// Verwendet die View "View_Nachbestellung" die Futtersorten mit Lagerbestand < Mindestbestand zeigt
        /// </summary>
        /// <returns>DataTable mit nachzubestellenden Futtersorten inkl. Status</returns>
        public DataTable GetNachbestellListe()
        {
            return Get("SELECT * FROM View_Nachbestellung ORDER BY Status_Text, Lagerbestand ASC");
        }

        /// <summary>
        /// Berechnet den gesamten Futterbedarf pro Tag über alle Tiere
        /// Zeigt auch die Reichweite des aktuellen Lagerbestands in Tagen
        /// </summary>
        /// <returns>DataTable mit Futterbedarf und Reichweite pro Futtersorte</returns>
        public DataTable GetFutterbedarfGesamt()
        {
            return Get("SELECT * FROM View_Futterbedarf_Gesamt WHERE Bedarf_pro_Tag > 0 ORDER BY Reichweite_Tage ASC");
        }

        /// <summary>
        /// Lädt den Fütterungsplan für eine bestimmte Tierart
        /// Zeigt welches Futter, in welcher Menge und zu welcher Zeit gefüttert werden muss
        /// </summary>
        /// <param name="tierartID">ID der Tierart (z.B. 1 für "Löwe")</param>
        /// <returns>DataTable mit Futtersorte, Menge, Einheit und Fütterungszeit</returns>
        public DataTable GetFutterplanFuerTierart(int tierartID)
        {
            string sql = @"
                SELECT 
                    f.Bezeichnung AS Futtersorte,
                    tf.Menge_pro_Tag,
                    f.Einheit,
                    tf.Fütterungszeit,
                    CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit, ' (', tf.Fütterungszeit, ')') AS Fütterungsplan
                FROM Tierart_Futter tf
                JOIN Futter f ON tf.futterID = f.futterID
                WHERE tf.tierartID = @tid
                ORDER BY tf.Fütterungszeit";

            return Get(sql, ("@tid", tierartID));
        }

        /// <summary>
        /// Lädt alle verfügbaren Futtersorten mit allen Details
        /// Wird für die Futterverwaltung verwendet
        /// </summary>
        /// <returns>DataTable mit ID, Bezeichnung, Einheit, Lagerbestand, Mindestbestand, Preis, Bestellmenge</returns>
        public DataTable GetAlleFuttersorten()
        {
            return Get(@"SELECT futterID, Bezeichnung, Einheit, Lagerbestand, 
                         Mindestbestand, Preis_pro_Einheit, Bestellmenge
                         FROM Futter 
                         ORDER BY Bezeichnung");
        }

        #endregion

        #region Bestellungen

        /// <summary>
        /// Erstellt eine neue Bestellung
        /// </summary>
        /// <param name="bestelldatum">Datum der Bestellung</param>
        /// <param name="lieferant">Name des Lieferanten</param>
        /// <returns>ID der neu erstellten Bestellung (für weitere Bestellpositionen)</returns>
        public int CreateBestellung(DateTime bestelldatum, string lieferant)
        {
            string sql = @"
                INSERT INTO Bestellung (Bestelldatum, Status, Lieferant, Gesamtpreis)
                VALUES (@datum, 'offen', @lieferant, 0);
                SELECT LAST_INSERT_ID();";

            DataTable dt = Get(sql, 
                ("@datum", bestelldatum.ToString("yyyy-MM-dd")), 
                ("@lieferant", lieferant));
            
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        /// <summary>
        /// Fügt einer bestehenden Bestellung eine Position hinzu
        /// </summary>
        /// <param name="bestellungID">ID der Bestellung</param>
        /// <param name="futterID">ID der Futtersorte</param>
        /// <param name="menge">Bestellmenge</param>
        /// <param name="preis">Preis pro Einheit</param>
        public void AddBestellPosition(int bestellungID, int futterID, int menge, decimal preis)
        {
            string sql = @"
                INSERT INTO Bestellung_Position (bestellungID, futterID, Menge, Einzelpreis, Gesamtpreis)
                VALUES (@bid, @fid, @menge, @preis, @menge * @preis)";

            Execute(sql,
                ("@bid", bestellungID),
                ("@fid", futterID),
                ("@menge", menge),
                ("@preis", preis));
        }

        /// <summary>
        /// Aktualisiert den Status einer Bestellung
        /// </summary>
        /// <param name="bestellungID">ID der Bestellung</param>
        /// <param name="status">Neuer Status ("offen", "bestellt", "geliefert")</param>
        /// <param name="lieferdatum">Optional: Lieferdatum (nur wenn Status = "geliefert")</param>
        public void UpdateBestellStatus(int bestellungID, string status, DateTime? lieferdatum = null)
        {
            string sql = @"
                UPDATE Bestellung 
                SET Status = @status, 
                    Lieferdatum = @ldatum 
                WHERE bestellungID = @id";

            Execute(sql,
                ("@status", status),
                ("@ldatum", lieferdatum?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value),
                ("@id", bestellungID));
        }

        /// <summary>
        /// Lädt alle Bestellungen mit ihren zugehörigen Positionen
        /// Zeigt Details wie Datum, Status, Lieferant, Futter und Preise
        /// </summary>
        /// <returns>DataTable mit allen Bestellungen und deren Positionen</returns>
        public DataTable GetBestellungenMitPositionen()
        {
            string sql = @"
                SELECT 
                    b.bestellungID,
                    b.Bestelldatum,
                    b.Lieferdatum,
                    b.Status,
                    b.Gesamtpreis,
                    b.Lieferant,
                    f.Bezeichnung AS Futtersorte,
                    bp.Menge,
                    f.Einheit,
                    bp.Einzelpreis,
                    bp.Gesamtpreis AS Positionspreis
                FROM Bestellung b
                LEFT JOIN Bestellung_Position bp ON b.bestellungID = bp.bestellungID
                LEFT JOIN Futter f ON bp.futterID = f.futterID
                ORDER BY b.Bestelldatum DESC, b.Status";

            return Get(sql);
        }

        #endregion

        #region Fütterungsplan und Tagesbedarf

        /// <summary>
        /// Berechnet den täglichen Futterbedarf für jedes einzelne Tier
        /// Verknüpft Tier → Tierart → Fütterungsplan → Futter
        /// </summary>
        /// <returns>DataTable mit Tiername, Gehege, Futtersorte, Menge und Fütterungszeit</returns>
        public DataTable GetTagesbedarfProTier()
        {
            string sql = @"
                SELECT 
                    t.Name AS Tiername,
                    ta.TABezeichnung AS Tierart,
                    g.GBezeichnung AS Gehege,
                    f.Bezeichnung AS Futtersorte,
                    tf.Menge_pro_Tag,
                    f.Einheit,
                    tf.Fütterungszeit
                FROM Tiere t
                JOIN Tierart ta ON t.TierartID = ta.tierartID
                JOIN Gehege g ON t.GehegeID = g.gID
                JOIN Tierart_Futter tf ON ta.tierartID = tf.tierartID
                JOIN Futter f ON tf.futterID = f.futterID
                ORDER BY t.Name, tf.Fütterungszeit";

            return Get(sql);
        }

        #endregion

        #region Statistiken

        /// <summary>
        /// Erstellt eine Monatsstatistik über gelieferte Bestellungen
        /// Zeigt Anzahl, Gesamtkosten und Durchschnitt pro Monat
        /// </summary>
        /// <returns>DataTable mit Monat, Jahr, Anzahl, Gesamtkosten und Durchschnitt</returns>
        public DataTable GetMonatsStatistik()
        {
            string sql = @"
                SELECT 
                    MONTH(Bestelldatum) AS Monat,
                    YEAR(Bestelldatum) AS Jahr,
                    COUNT(*) AS Anzahl_Bestellungen,
                    SUM(Gesamtpreis) AS Gesamtkosten,
                    AVG(Gesamtpreis) AS Durchschnitt
                FROM Bestellung
                WHERE Status = 'geliefert'
                GROUP BY YEAR(Bestelldatum), MONTH(Bestelldatum)
                ORDER BY Jahr DESC, Monat DESC";

            return Get(sql);
        }

        #endregion
    }
}
