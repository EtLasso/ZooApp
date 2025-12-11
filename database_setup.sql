-- ============================================
-- Zoo Verwaltung - Vollständiges Datenbankschema
-- ============================================

CREATE DATABASE IF NOT EXISTS zoo_verwaltung;
USE zoo_verwaltung;

-- ============================================
-- BASIS-TABELLEN
-- ============================================

-- Tabelle: Kontinent
CREATE TABLE IF NOT EXISTS Kontinent (
    kID INT AUTO_INCREMENT PRIMARY KEY,
    Kbezeichnung VARCHAR(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Gehege
CREATE TABLE IF NOT EXISTS Gehege (
    gID INT AUTO_INCREMENT PRIMARY KEY,
    GBezeichnung VARCHAR(100) NOT NULL,
    kontinentID INT NOT NULL,
    FOREIGN KEY (kontinentID) REFERENCES Kontinent(kID)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Tierart
CREATE TABLE IF NOT EXISTS Tierart (
    tierartID INT AUTO_INCREMENT PRIMARY KEY,
    TABezeichnung VARCHAR(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Tiere
CREATE TABLE IF NOT EXISTS Tiere (
    tierID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Gewicht DECIMAL(10,2) NOT NULL,
    Geburtsdatum DATE NOT NULL,
    TierartID INT NOT NULL,
    GehegeID INT NOT NULL,
    FOREIGN KEY (TierartID) REFERENCES Tierart(tierartID)
        ON DELETE RESTRICT ON UPDATE CASCADE,
    FOREIGN KEY (GehegeID) REFERENCES Gehege(gID)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================
-- FUTTER-TABELLEN
-- ============================================

-- Tabelle: Futter
CREATE TABLE IF NOT EXISTS Futter (
    futterID INT AUTO_INCREMENT PRIMARY KEY,
    Bezeichnung VARCHAR(100) NOT NULL,
    Einheit VARCHAR(20) NOT NULL DEFAULT 'kg',
    Preis_pro_Einheit DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Lagerbestand INT NOT NULL DEFAULT 0,
    Mindestbestand INT NOT NULL DEFAULT 50,
    Bestellmenge INT NOT NULL DEFAULT 100
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Tierart_Futter (Fütterungsplan)
CREATE TABLE IF NOT EXISTS Tierart_Futter (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tierartID INT NOT NULL,
    futterID INT NOT NULL,
    Menge_pro_Tag DECIMAL(10,2) NOT NULL,
    Fütterungszeit TIME NOT NULL DEFAULT '08:00:00',
    FOREIGN KEY (tierartID) REFERENCES Tierart(tierartID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (futterID) REFERENCES Futter(futterID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    UNIQUE KEY unique_tierart_futter_zeit (tierartID, futterID, Fütterungszeit)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Bestellung
CREATE TABLE IF NOT EXISTS Bestellung (
    bestellungID INT AUTO_INCREMENT PRIMARY KEY,
    Bestelldatum DATE NOT NULL,
    Lieferdatum DATE NULL,
    Status ENUM('offen', 'bestellt', 'geliefert', 'storniert') NOT NULL DEFAULT 'offen',
    Gesamtpreis DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Lieferant VARCHAR(200) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Bestellung_Position
CREATE TABLE IF NOT EXISTS Bestellung_Position (
    positionID INT AUTO_INCREMENT PRIMARY KEY,
    bestellungID INT NOT NULL,
    futterID INT NOT NULL,
    Menge INT NOT NULL,
    Einzelpreis DECIMAL(10,2) NOT NULL,
    Gesamtpreis DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (bestellungID) REFERENCES Bestellung(bestellungID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (futterID) REFERENCES Futter(futterID)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabelle: Fuetterungsprotokoll
CREATE TABLE IF NOT EXISTS Fuetterungsprotokoll (
    protokollID INT AUTO_INCREMENT PRIMARY KEY,
    tierID INT NOT NULL,
    futterID INT NOT NULL,
    Menge DECIMAL(10,2) NOT NULL,
    Fuetterungszeit DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Pfleger_Name VARCHAR(100) NULL,
    Bemerkungen TEXT NULL,
    FOREIGN KEY (tierID) REFERENCES Tiere(tierID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (futterID) REFERENCES Futter(futterID)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ============================================
-- VIEWS (Ansichten für komplexe Abfragen)
-- ============================================

-- View: Tiere-Übersicht
CREATE OR REPLACE VIEW View_Tiere_Uebersicht AS
SELECT 
    t.tierID,
    t.Name,
    t.Gewicht,
    t.Geburtsdatum,
    ta.TABezeichnung AS Tierart,
    g.gID,
    g.GBezeichnung AS Gehege,
    k.Kbezeichnung AS Kontinent
FROM Tiere t
LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
LEFT JOIN Gehege g ON t.GehegeID = g.gID
LEFT JOIN Kontinent k ON g.kontinentID = k.kID;

-- View: Gehege-Statistik
CREATE OR REPLACE VIEW View_Gehege_Statistik AS
SELECT 
    g.gID,
    g.GBezeichnung AS Gehege,
    k.Kbezeichnung AS Kontinent,
    COUNT(t.tierID) AS Anzahl_Tiere,
    COALESCE(SUM(t.Gewicht), 0) AS Gesamtgewicht
FROM Gehege g
LEFT JOIN Kontinent k ON g.kontinentID = k.kID
LEFT JOIN Tiere t ON g.gID = t.GehegeID
GROUP BY g.gID, g.GBezeichnung, k.Kbezeichnung;

-- View: Futter-Lager mit Status
CREATE OR REPLACE VIEW View_Futter_Lager AS
SELECT 
    futterID,
    Bezeichnung,
    Einheit,
    Preis_pro_Einheit,
    Lagerbestand,
    Mindestbestand,
    Bestellmenge,
    CASE 
        WHEN Lagerbestand <= 0 THEN 'KRITISCH 🔴'
        WHEN Lagerbestand < Mindestbestand THEN 'NIEDRIG 🟡'
        ELSE 'OK 🟢'
    END AS Status,
    (Mindestbestand - Lagerbestand) AS Fehlmenge
FROM Futter;

-- View: Täglicher Futterbedarf
CREATE OR REPLACE VIEW View_Taeglicher_Futterbedarf AS
SELECT 
    f.Bezeichnung AS Futtersorte,
    f.Einheit,
    SUM(tf.Menge_pro_Tag * (SELECT COUNT(*) FROM Tiere WHERE TierartID = tf.tierartID)) AS Gesamtmenge_pro_Tag,
    tf.Fütterungszeit AS Uhrzeit,
    COUNT(DISTINCT tf.tierartID) AS Anzahl_Tierarten
FROM Tierart_Futter tf
JOIN Futter f ON tf.futterID = f.futterID
GROUP BY f.futterID, tf.Fütterungszeit
ORDER BY tf.Fütterungszeit;

-- View: Heutige Fütterungen
CREATE OR REPLACE VIEW View_Heutige_Fuetterungen AS
SELECT 
    fp.protokollID,
    t.Name AS Tiername,
    ta.TABezeichnung AS Tierart,
    f.Bezeichnung AS Futtersorte,
    fp.Menge,
    f.Einheit,
    fp.Fuetterungszeit AS Uhrzeit,
    fp.Pfleger_Name AS Pfleger,
    fp.Bemerkungen
FROM Fuetterungsprotokoll fp
JOIN Tiere t ON fp.tierID = t.tierID
JOIN Tierart ta ON t.TierartID = ta.tierartID
JOIN Futter f ON fp.futterID = f.futterID
WHERE DATE(fp.Fuetterungszeit) = CURDATE()
ORDER BY fp.Fuetterungszeit DESC;

-- ============================================
-- BEISPIELDATEN
-- ============================================

-- Kontinente
INSERT IGNORE INTO Kontinent (kID, Kbezeichnung) VALUES
(1, 'Afrika'),
(2, 'Asien'),
(3, 'Europa'),
(4, 'Nordamerika'),
(5, 'Südamerika'),
(6, 'Australien');

-- Gehege
INSERT IGNORE INTO Gehege (gID, GBezeichnung, kontinentID) VALUES
(1, 'Savanne', 1),
(2, 'Tropischer Regenwald', 1),
(3, 'Bambuswald', 2),
(4, 'Steppe', 2),
(5, 'Alpengehege', 3),
(6, 'Prärie', 4),
(7, 'Amazonas', 5),
(8, 'Outback', 6);

-- Tierarten
INSERT IGNORE INTO Tierart (tierartID, TABezeichnung) VALUES
(1, 'Löwe'),
(2, 'Elefant'),
(3, 'Giraffe'),
(4, 'Tiger'),
(5, 'Panda'),
(6, 'Braunbär'),
(7, 'Bison'),
(8, 'Jaguar'),
(9, 'Känguru'),
(10, 'Koala');

-- Tiere
INSERT IGNORE INTO Tiere (tierID, Name, Gewicht, Geburtsdatum, TierartID, GehegeID) VALUES
(1, 'Simba', 190.50, '2018-03-15', 1, 1),
(2, 'Nala', 175.30, '2019-06-22', 1, 1),
(3, 'Dumbo', 5400.00, '2015-01-10', 2, 2),
(4, 'Melman', 1200.00, '2017-08-05', 3, 1),
(5, 'Shir Khan', 220.00, '2016-11-30', 4, 4),
(6, 'Bao Bao', 110.50, '2020-02-14', 5, 3),
(7, 'Baloo', 180.00, '2018-07-20', 6, 5),
(8, 'Thunder', 900.00, '2019-04-12', 7, 6),
(9, 'Diego', 95.00, '2017-09-18', 8, 7),
(10, 'Skippy', 35.00, '2021-05-25', 9, 8),
(11, 'Kimi', 12.50, '2020-10-08', 10, 8);

-- Futtersorten
INSERT IGNORE INTO Futter (futterID, Bezeichnung, Einheit, Preis_pro_Einheit, Lagerbestand, Mindestbestand, Bestellmenge) VALUES
(1, 'Frisches Fleisch', 'kg', 8.50, 150, 100, 200),
(2, 'Heu', 'kg', 0.50, 500, 200, 500),
(3, 'Bambus', 'kg', 3.20, 80, 100, 150),
(4, 'Obst-Mix', 'kg', 2.80, 120, 80, 150),
(5, 'Gemüse-Mix', 'kg', 1.50, 200, 100, 200),
(6, 'Fisch', 'kg', 6.80, 40, 50, 100),
(7, 'Nüsse', 'kg', 12.00, 30, 40, 80),
(8, 'Pellets', 'kg', 1.20, 300, 150, 300),
(9, 'Gras', 'kg', 0.30, 600, 300, 500),
(10, 'Eukалyptus-Blätter', 'kg', 4.50, 45, 50, 100);

-- Fütterungspläne
INSERT IGNORE INTO Tierart_Futter (tierartID, futterID, Menge_pro_Tag, Fütterungszeit) VALUES
-- Löwen: Fleisch
(1, 1, 8.0, '08:00:00'),
(1, 1, 6.0, '16:00:00'),
-- Elefanten: Heu, Obst, Gemüse
(2, 2, 50.0, '07:00:00'),
(2, 4, 20.0, '12:00:00'),
(2, 5, 30.0, '18:00:00'),
-- Giraffen: Heu, Gemüse
(3, 2, 25.0, '08:00:00'),
(3, 5, 15.0, '16:00:00'),
-- Tiger: Fleisch
(4, 1, 7.0, '09:00:00'),
(4, 1, 5.0, '17:00:00'),
-- Pandas: Bambus
(5, 3, 12.0, '07:00:00'),
(5, 3, 10.0, '15:00:00'),
(5, 4, 3.0, '12:00:00'),
-- Braunbären: Fleisch, Obst, Fisch
(6, 1, 5.0, '08:00:00'),
(6, 4, 8.0, '12:00:00'),
(6, 6, 3.0, '17:00:00'),
-- Bisons: Heu, Gras
(7, 2, 20.0, '07:00:00'),
(7, 9, 30.0, '15:00:00'),
-- Jaguare: Fleisch
(8, 1, 6.0, '09:00:00'),
(8, 1, 4.0, '18:00:00'),
-- Kängurus: Gras, Pellets
(9, 9, 5.0, '08:00:00'),
(9, 8, 2.0, '16:00:00'),
-- Koalas: Eukalyptus
(10, 10, 1.0, '06:00:00'),
(10, 10, 1.0, '18:00:00');

-- Beispiel-Bestellungen
INSERT IGNORE INTO Bestellung (bestellungID, Bestelldatum, Lieferdatum, Status, Gesamtpreis, Lieferant) VALUES
(1, '2025-12-01', '2025-12-05', 'geliefert', 1850.00, 'Futter-Großhandel Schmidt'),
(2, '2025-12-08', NULL, 'bestellt', 950.00, 'Bio-Futter Meyer'),
(3, '2025-12-10', NULL, 'offen', 0.00, 'Futter-Express');

-- Bestellpositionen
INSERT IGNORE INTO Bestellung_Position (bestellungID, futterID, Menge, Einzelpreis, Gesamtpreis) VALUES
(1, 1, 200, 8.50, 1700.00),
(1, 6, 100, 6.80, 680.00),
(2, 3, 150, 3.20, 480.00),
(2, 10, 100, 4.50, 450.00);

-- ============================================
-- ABSCHLUSS-INFO
-- ============================================

SELECT '✅ Datenbank erfolgreich eingerichtet!' AS Status;

SELECT 'Kontinente' AS Tabelle, COUNT(*) AS Anzahl FROM Kontinent
UNION ALL SELECT 'Gehege', COUNT(*) FROM Gehege
UNION ALL SELECT 'Tierarten', COUNT(*) FROM Tierart
UNION ALL SELECT 'Tiere', COUNT(*) FROM Tiere
UNION ALL SELECT 'Futtersorten', COUNT(*) FROM Futter
UNION ALL SELECT 'Fütterungspläne', COUNT(*) FROM Tierart_Futter
UNION ALL SELECT 'Bestellungen', COUNT(*) FROM Bestellung
UNION ALL SELECT 'Bestellpositionen', COUNT(*) FROM Bestellung_Position;
