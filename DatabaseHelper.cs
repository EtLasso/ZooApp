using System.Data;

namespace ZooApp
{
    public class ZooDB : DB
    {
        // ==================== FUTTERVERWALTUNG ====================

        /// <summary>
        /// Holt alle Futtersorten aus der Datenbank
        /// </summary>
        public DataTable GetAlleFuttersorten()
        {
            return Get(@"SELECT * FROM Futter ORDER BY Bezeichnung");
        }

        /// <summary>
        /// Holt alle Futtersorten, die nachbestellt werden müssen
        /// (Lagerbestand unter Mindestbestand)
        /// </summary>
        public DataTable GetNachbestellListe()
        {
            string sql = @"
                SELECT 
                    f.*,
                    CASE 
                        WHEN f.Lagerbestand <= 0 THEN 'LEER 🔴'
                        WHEN f.Lagerbestand < f.Mindestbestand THEN 'NIEDRIG 🟡'
                        ELSE 'OK 🟢'
                    END AS Status,
                    (f.Mindestbestand - f.Lagerbestand) AS Fehlmenge,
                    CASE 
                        WHEN f.Lagerbestand < f.Mindestbestand 
                        THEN f.Bestellmenge 
                        ELSE 0 
                    END AS ZuBestellen
                FROM Futter f
                WHERE f.Lagerbestand < f.Mindestbestand
                ORDER BY f.Lagerbestand ASC";

            return Get(sql);
        }

        // ==================== FÜTTERUNGSPLAN ====================

        /// <summary>
        /// Holt den Fütterungsplan für eine bestimmte Tierart
        /// </summary>
        public DataTable GetFutterplanFuerTierart(int tierartID)
        {
            string sql = @"
                SELECT 
                    ta.TABezeichnung AS Tierart,
                    f.Bezeichnung AS Futtersorte,
                    tf.Menge_pro_Tag,
                    f.Einheit,
                    tf.Fütterungszeit,
                    CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit, ' (', tf.Fütterungszeit, ')') AS Fütterungsplan
                FROM Tierart_Futter tf
                JOIN Tierart ta ON tf.tierartID = ta.tierartID
                JOIN Futter f ON tf.futterID = f.futterID
                WHERE ta.tierartID = @tierartID
                ORDER BY tf.Fütterungszeit";

            return Get(sql, ("@tierartID", tierartID));
        }

        // ==================== TAGESBEDARF ====================

        /// <summary>
        /// Holt den täglichen Futterbedarf für jedes Tier
        /// </summary>
        public DataTable GetTagesbedarfProTier()
        {
            string sql = @"
                SELECT 
                    t.tierID,
                    t.Name AS Tiername,
                    ta.TABezeichnung AS Tierart,
                    g.GBezeichnung AS Gehege,
                    f.Bezeichnung AS Futtersorte,
                    tf.Menge_pro_Tag AS Tagesmenge,
                    f.Einheit,
                    tf.Fütterungszeit,
                    CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit, ' (', tf.Fütterungszeit, ')') AS Fütterung
                FROM Tiere t
                JOIN Tierart ta ON t.TierartID = ta.tierartID
                JOIN Tierart_Futter tf ON ta.tierartID = tf.tierartID
                JOIN Futter f ON tf.futterID = f.futterID
                JOIN Gehege g ON t.GehegeID = g.gID
                ORDER BY t.Name, tf.Fütterungszeit";

            return Get(sql);
        }

        // ==================== BESTELLUNGEN ====================

        /// <summary>
        /// Holt alle Bestellungen mit ihren Positionen
        /// </summary>
        public DataTable GetBestellungenMitPositionen()
        {
            return Get(@"
                SELECT 
                    b.bestellungID,
                    b.Bestelldatum,
                    b.Lieferdatum,
                    b.Status,
                    b.Gesamtpreis,
                    b.Lieferant,
                    b.Notizen,
                    f.Bezeichnung AS Futtersorte,
                    bp.Menge,
                    f.Einheit,
                    bp.Einzelpreis,
                    bp.Gesamtpreis AS Positionspreis
                FROM Bestellung b
                LEFT JOIN Bestellung_Position bp ON b.bestellungID = bp.bestellungID
                LEFT JOIN Futter f ON bp.futterID = f.futterID
                ORDER BY b.Bestelldatum DESC, b.Status");
        }

        // ==================== TIERVERWALTUNG ====================

        public DataTable GetTiereUebersicht()
        {
            return Get("SELECT * FROM View_Tiere_Uebersicht ORDER BY Name");
        }

        public DataTable GetTierDetails(int tierID)
        {
            string sql = @"
                SELECT 
                    t.*,
                    ta.TABezeichnung,
                    ta.Wissenschaftlicher_Name,
                    ta.Gefaehrdungsstatus,
                    g.GBezeichnung,
                    k.Kbezeichnung
                FROM Tiere t
                LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                LEFT JOIN Gehege g ON t.GehegeID = g.gID
                LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                WHERE t.tierID = @id";

            return Get(sql, ("@id", tierID));
        }

        public DataTable GetTiereImGehege(int gehegeID)
        {
            return Get("SELECT * FROM View_Tiere_Uebersicht WHERE gID = @id ORDER BY Name",
                ("@id", gehegeID));
        }

        // ==================== GEHEGE ====================

        public DataTable GetGehegeStatistik()
        {
            return Get("SELECT * FROM View_Gehege_Statistik ORDER BY Gehege");
        }

        public DataTable GetAlleGehege()
        {
            return Get(@"
                SELECT g.*, k.Kbezeichnung 
                FROM Gehege g
                LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                ORDER BY g.GBezeichnung");
        }

        // ==================== FUTTERVERWALTUNG ====================

        public DataTable GetFutterLager()
        {
            return Get("SELECT * FROM View_Futter_Lager ORDER BY Status, Lagerbestand ASC");
        }

        public DataTable GetNachbestellungen()
        {
            return Get("SELECT * FROM View_Futter_Lager WHERE Status IN ('KRITISCH 🔴', 'NIEDRIG 🟡') ORDER BY Lagerbestand ASC");
        }

        // ==================== FÜTTERUNG ====================

        public DataTable GetTaeglicherFutterbedarf()
        {
            return Get("SELECT * FROM View_Taeglicher_Futterbedarf ORDER BY Uhrzeit, Tiername");
        }

        public DataTable GetHeutigeFuetterungen()
        {
            return Get("SELECT * FROM View_Heutige_Fuetterungen ORDER BY Uhrzeit DESC");
        }

        public DataTable GetFuetterungsprotokoll(int tage = 7)
        {
            string sql = @"
                SELECT 
                    DATE(fp.Fuetterungszeit) AS Datum,
                    t.Name AS Tier,
                    ta.TABezeichnung AS Tierart,
                    f.Bezeichnung AS Futtersorte,
                    fp.Menge,
                    f.Einheit,
                    fp.Pfleger_Name AS Pfleger,
                    TIME(fp.Fuetterungszeit) AS Uhrzeit,
                    fp.Bemerkungen
                FROM Fuetterungsprotokoll fp
                JOIN Tiere t ON fp.tierID = t.tierID
                JOIN Futter f ON fp.futterID = f.futterID
                JOIN Tierart ta ON t.TierartID = ta.tierartID
                WHERE fp.Fuetterungszeit >= CURDATE() - INTERVAL @tage DAY
                ORDER BY fp.Fuetterungszeit DESC";

            return Get(sql, ("@tage", tage));
        }

        public int FutterHinzufuegen(int tierID, int futterID, decimal menge, string pfleger, string bemerkungen = "")
        {
            string sql = @"
                INSERT INTO Fuetterungsprotokoll 
                (tierID, futterID, Menge, Fuetterungszeit, Pfleger_Name, Bemerkungen)
                VALUES (@tid, @fid, @menge, NOW(), @pfleger, @bem)
                ON DUPLICATE KEY UPDATE 
                Menge = VALUES(Menge),
                Pfleger_Name = VALUES(Pfleger_Name),
                Bemerkungen = VALUES(Bemerkungen);
                
                UPDATE Futter 
                SET Lagerbestand = Lagerbestand - @menge 
                WHERE futterID = @fid;
                
                SELECT LAST_INSERT_ID();";

            DataTable dt = Get(sql,
                ("@tid", tierID),
                ("@fid", futterID),
                ("@menge", menge),
                ("@pfleger", pfleger),
                ("@bem", bemerkungen));

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        // ==================== BESTELLUNGEN ====================

        public DataTable GetAktiveBestellungen()
        {
            return Get(@"
                SELECT 
                    b.*,
                    COUNT(bp.positionID) AS Anzahl_Positionen,
                    SUM(bp.Gesamtpreis) AS Summe_Positionen
                FROM Bestellung b
                LEFT JOIN Bestellung_Position bp ON b.bestellungID = bp.bestellungID
                WHERE b.Status IN ('offen', 'bestellt')
                GROUP BY b.bestellungID
                ORDER BY b.Bestelldatum DESC");
        }

        public int BestellungAnlegen(string lieferant, string notizen = "")
        {
            string sql = @"
                INSERT INTO Bestellung (Bestelldatum, Status, Lieferant, Notizen, Gesamtpreis)
                VALUES (CURDATE(), 'offen', @lieferant, @notizen, 0);
                SELECT LAST_INSERT_ID();";

            DataTable dt = Get(sql,
                ("@lieferant", lieferant),
                ("@notizen", notizen));

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public void BestellungPositionHinzufuegen(int bestellungID, int futterID, decimal menge, decimal preis)
        {
            string sql = @"
                INSERT INTO Bestellung_Position (bestellungID, futterID, Menge, Einzelpreis, Gesamtpreis)
                VALUES (@bid, @fid, @menge, @preis, @menge * @preis);
                
                UPDATE Bestellung 
                SET Gesamtpreis = Gesamtpreis + (@menge * @preis)
                WHERE bestellungID = @bid";

            Execute(sql,
                ("@bid", bestellungID),
                ("@fid", futterID),
                ("@menge", menge),
                ("@preis", preis));
        }

        // ==================== STATISTIKEN ====================

        public DataTable GetTierStatistiken()
        {
            string sql = @"
                SELECT 
                    ta.TABezeichnung AS Tierart,
                    COUNT(t.tierID) AS Anzahl_Tiere,
                    AVG(t.Gewicht) AS Durchschnittsgewicht,
                    MIN(TIMESTAMPDIFF(YEAR, t.Geburtsdatum, CURDATE())) AS Min_Alter,
                    MAX(TIMESTAMPDIFF(YEAR, t.Geburtsdatum, CURDATE())) AS Max_Alter
                FROM Tierart ta
                LEFT JOIN Tiere t ON ta.tierartID = t.TierartID
                GROUP BY ta.tierartID
                ORDER BY Anzahl_Tiere DESC";

            return Get(sql);
        }

        public DataTable GetFutterStatistiken(int monate = 12)
        {
            string sql = @"
                SELECT 
                    MONTH(fp.Fuetterungszeit) AS Monat,
                    YEAR(fp.Fuetterungszeit) AS Jahr,
                    f.Bezeichnung AS Futtersorte,
                    SUM(fp.Menge) AS Verbrauch,
                    f.Einheit,
                    SUM(fp.Menge * f.Preis_pro_Einheit) AS Kosten
                FROM Fuetterungsprotokoll fp
                JOIN Futter f ON fp.futterID = f.futterID
                WHERE fp.Fuetterungszeit >= CURDATE() - INTERVAL @monate MONTH
                GROUP BY YEAR(fp.Fuetterungszeit), MONTH(fp.Fuetterungszeit), f.futterID
                ORDER BY Jahr DESC, Monat DESC, Verbrauch DESC";

            return Get(sql, ("@monate", monate));
        }

        public DataTable GetDashboardDaten()
        {
            string sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM Tiere) AS Anzahl_Tiere,
                    (SELECT COUNT(*) FROM Gehege) AS Anzahl_Gehege,
                    (SELECT COUNT(*) FROM Tierart) AS Anzahl_Tierarten,
                    (SELECT COUNT(*) FROM View_Futter_Lager WHERE Status IN ('KRITISCH 🔴', 'NIEDRIG 🟡')) AS Nachbestellungen,
                    (SELECT COUNT(*) FROM Bestellung WHERE Status IN ('offen', 'bestellt')) AS Offene_Bestellungen,
                    (SELECT COUNT(*) FROM View_Heutige_Fuetterungen) AS Heutige_Fuetterungen,
                    (SELECT SUM(Gesamtpreis) FROM Bestellung WHERE YEAR(Bestelldatum) = YEAR(CURDATE())) AS Jahresumsatz";

            return Get(sql);
        }
    }
}