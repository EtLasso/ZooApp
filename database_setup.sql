-- Zoo Verwaltung Datenbank Setup
-- Diese SQL-Datei erstellt die Datenbank und alle notwendigen Tabellen

-- Datenbank erstellen (falls nicht vorhanden)
CREATE DATABASE IF NOT EXISTS zoo_verwaltung;
USE zoo_verwaltung;

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
        ON DELETE RESTRICT
        ON UPDATE CASCADE
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
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    FOREIGN KEY (GehegeID) REFERENCES Gehege(gID)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Beispieldaten einfügen (optional)

-- Kontinente
INSERT INTO Kontinent (Kbezeichnung) VALUES
('Afrika'),
('Asien'),
('Europa'),
('Nordamerika'),
('Südamerika'),
('Australien');

-- Gehege
INSERT INTO Gehege (GBezeichnung, kontinentID) VALUES
('Savanne', 1),
('Tropischer Regenwald', 1),
('Bambuswald', 2),
('Steppe', 2),
('Alpengehege', 3),
('Prärie', 4),
('Amazonas', 5),
('Outback', 6);

-- Tierarten
INSERT INTO Tierart (TABezeichnung) VALUES
('Löwe'),
('Elefant'),
('Giraffe'),
('Tiger'),
('Panda'),
('Braunbär'),
('Bison'),
('Jaguar'),
('Känguru'),
('Koala');

-- Tiere
INSERT INTO Tiere (Name, Gewicht, Geburtsdatum, TierartID, GehegeID) VALUES
('Simba', 190.50, '2018-03-15', 1, 1),
('Nala', 175.30, '2019-06-22', 1, 1),
('Dumbo', 5400.00, '2015-01-10', 2, 2),
('Melman', 1200.00, '2017-08-05', 3, 1),
('Shir Khan', 220.00, '2016-11-30', 4, 4),
('Bao Bao', 110.50, '2020-02-14', 5, 3),
('Baloo', 180.00, '2018-07-20', 6, 5),
('Thunder', 900.00, '2019-04-12', 7, 6),
('Diego', 95.00, '2017-09-18', 8, 7),
('Skippy', 35.00, '2021-05-25', 9, 8),
('Kimi', 12.50, '2020-10-08', 10, 8);

-- Anzeigen der erstellten Daten
SELECT 'Kontinente' AS Tabelle, COUNT(*) AS Anzahl FROM Kontinent
UNION ALL
SELECT 'Gehege', COUNT(*) FROM Gehege
UNION ALL
SELECT 'Tierarten', COUNT(*) FROM Tierart
UNION ALL
SELECT 'Tiere', COUNT(*) FROM Tiere;
