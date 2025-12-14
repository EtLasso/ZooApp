-- ===============================================
-- PFLEGER TABELLE - UPGRADE MIT ERWEITERTEN FELDERN
-- ===============================================

-- 1. Neue Spalten zur bestehenden Tabelle hinzufügen
ALTER TABLE pfleger
ADD COLUMN Foto VARCHAR(500) NULL COMMENT 'Pfad zum Pfleger-Foto',
ADD COLUMN Steuerklasse INT NULL COMMENT 'Steuerklasse 1-6',
ADD COLUMN Sozialversicherungsnummer VARCHAR(20) NULL COMMENT 'SV-Nummer',
ADD COLUMN Personalnummer VARCHAR(20) NULL COMMENT 'Eindeutige Personalnummer',
ADD COLUMN Bankverbindung VARCHAR(200) NULL COMMENT 'IBAN',
ADD COLUMN Krankenkasse VARCHAR(100) NULL COMMENT 'Name der Krankenkasse',
ADD COLUMN HauptpflegerID INT NULL COMMENT 'Referenz zum vorgesetzten Hauptpfleger',
ADD COLUMN IstHauptpfleger BOOLEAN DEFAULT FALSE COMMENT 'Ist diese Person ein Hauptpfleger?',
ADD COLUMN Aufgabenbereich TEXT NULL COMMENT 'Zugewiesene Aufgabenbereiche',
ADD COLUMN Qualifikationen TEXT NULL COMMENT 'Berufliche Qualifikationen',
ADD COLUMN Notfallkontakt VARCHAR(200) NULL COMMENT 'Notfallkontakt Person + Telefon';

-- Foreign Key für Hauptpfleger-Hierarchie
ALTER TABLE pfleger
ADD CONSTRAINT fk_pfleger_hauptpfleger 
FOREIGN KEY (HauptpflegerID) REFERENCES pfleger(pflegerID) ON DELETE SET NULL;

-- ===============================================
-- 2. NEUE TABELLE: Pfleger-Tier-Zuordnung
-- ===============================================

CREATE TABLE IF NOT EXISTS Pfleger_Tier (
    zuordnungID INT AUTO_INCREMENT PRIMARY KEY,
    pflegerID INT NOT NULL,
    tierID INT NOT NULL,
    IstHauptpfleger BOOLEAN DEFAULT TRUE COMMENT 'Hauptpfleger oder Assistent',
    ZugeordnetAm DATE NOT NULL DEFAULT CURDATE(),
    Notizen TEXT NULL,
    FOREIGN KEY (pflegerID) REFERENCES pfleger(pflegerID) ON DELETE CASCADE,
    FOREIGN KEY (tierID) REFERENCES Tier(tierID) ON DELETE CASCADE,
    UNIQUE KEY unique_pfleger_tier (pflegerID, tierID)
) COMMENT 'Zuordnung von Pflegern zu Tieren';

-- ===============================================
-- 3. NEUE TABELLE: Aufgabenbereiche
-- ===============================================

CREATE TABLE IF NOT EXISTS Aufgabenbereich (
    aufgabenbereichID INT AUTO_INCREMENT PRIMARY KEY,
    Bezeichnung VARCHAR(100) NOT NULL UNIQUE,
    Beschreibung TEXT NULL,
    Farbe VARCHAR(7) NULL COMMENT 'Hex-Farbe für UI'
) COMMENT 'Vordefinierte Aufgabenbereiche';

-- Standard-Aufgabenbereiche einfügen
INSERT INTO Aufgabenbereich (Bezeichnung, Beschreibung, Farbe) VALUES
('Fütterung', 'Verantwortlich für Fütterung der Tiere', '#27ae60'),
('Reinigung', 'Reinigung von Gehegen und Anlagen', '#3498db'),
('Medizinische Versorgung', 'Gesundheitschecks und Behandlungen', '#e74c3c'),
('Training', 'Tiertraining und Verhaltensarbeit', '#f39c12'),
('Besucherführungen', 'Führungen und Bildungsarbeit', '#9b59b6'),
('Gehege-Wartung', 'Instandhaltung der Gehege', '#95a5a6'),
('Zucht-Programm', 'Zuchtkoordination', '#e67e22'),
('Notfall-Bereitschaft', 'Notfall-Einsatzteam', '#c0392b');

-- ===============================================
-- 4. NEUE TABELLE: Pfleger-Aufgabenbereich-Zuordnung
-- ===============================================

CREATE TABLE IF NOT EXISTS Pfleger_Aufgabenbereich (
    zuordnungID INT AUTO_INCREMENT PRIMARY KEY,
    pflegerID INT NOT NULL,
    aufgabenbereichID INT NOT NULL,
    Hauptverantwortlich BOOLEAN DEFAULT FALSE,
    ZugeordnetAm DATE NOT NULL DEFAULT CURDATE(),
    FOREIGN KEY (pflegerID) REFERENCES pfleger(pflegerID) ON DELETE CASCADE,
    FOREIGN KEY (aufgabenbereichID) REFERENCES Aufgabenbereich(aufgabenbereichID) ON DELETE CASCADE,
    UNIQUE KEY unique_pfleger_aufgabe (pflegerID, aufgabenbereichID)
) COMMENT 'Zuordnung von Aufgabenbereichen zu Pflegern';

-- ===============================================
-- 5. Beispiel-Daten aktualisieren
-- ===============================================

-- Personalnummern vergeben
UPDATE pfleger SET 
    Personalnummer = CONCAT('PFL', LPAD(pflegerID, 4, '0')),
    Steuerklasse = 1,
    IstHauptpfleger = (pflegerID <= 2),
    Qualifikationen = 'Tierpfleger (IHK)',
    Krankenkasse = 'AOK Bayern'
WHERE pflegerID IS NOT NULL;

-- Hans Mueller und Anna Schmidt als Hauptpfleger
UPDATE pfleger SET IstHauptpfleger = TRUE WHERE pflegerID IN (1, 2);

-- Peter, Lisa als Assistenten von Hans
UPDATE pfleger SET HauptpflegerID = 1 WHERE pflegerID IN (3, 4);

-- Thomas als Assistent von Anna
UPDATE pfleger SET HauptpflegerID = 2 WHERE pflegerID = 5;

-- ===============================================
-- 6. VIEWS für einfache Abfragen
-- ===============================================

-- View: Pfleger mit ihren Tieren
CREATE OR REPLACE VIEW V_Pfleger_Tiere AS
SELECT 
    p.pflegerID,
    p.Vorname,
    p.Nachname,
    p.Personalnummer,
    t.tierID,
    t.Name AS TierName,
    ta.Bezeichnung AS Tierart,
    pt.IstHauptpfleger,
    pt.ZugeordnetAm
FROM pfleger p
LEFT JOIN Pfleger_Tier pt ON p.pflegerID = pt.pflegerID
LEFT JOIN Tier t ON pt.tierID = t.tierID
LEFT JOIN Tierart ta ON t.tierartID = ta.tierartID
ORDER BY p.Nachname, p.Vorname, t.Name;

-- View: Pfleger mit Assistenten
CREATE OR REPLACE VIEW V_Pfleger_Hierarchie AS
SELECT 
    h.pflegerID AS HauptpflegerID,
    CONCAT(h.Vorname, ' ', h.Nachname) AS Hauptpfleger,
    h.Personalnummer AS Hauptpfleger_PN,
    a.pflegerID AS AssistentenID,
    CONCAT(a.Vorname, ' ', a.Nachname) AS Assistent,
    a.Personalnummer AS Assistenten_PN,
    a.Gehalt AS Assistenten_Gehalt
FROM pfleger h
LEFT JOIN pfleger a ON h.pflegerID = a.HauptpflegerID
WHERE h.IstHauptpfleger = TRUE
ORDER BY h.Nachname, a.Nachname;

-- View: Pfleger mit Aufgabenbereichen
CREATE OR REPLACE VIEW V_Pfleger_Aufgaben AS
SELECT 
    p.pflegerID,
    CONCAT(p.Vorname, ' ', p.Nachname) AS PflegerName,
    p.Personalnummer,
    a.Bezeichnung AS Aufgabenbereich,
    a.Farbe AS AufgabenFarbe,
    pa.Hauptverantwortlich
FROM pfleger p
LEFT JOIN Pfleger_Aufgabenbereich pa ON p.pflegerID = pa.pflegerID
LEFT JOIN Aufgabenbereich a ON pa.aufgabenbereichID = a.aufgabenbereichID
ORDER BY p.Nachname, a.Bezeichnung;

-- ===============================================
-- 7. Trigger: Automatische Personalnummer
-- ===============================================

DELIMITER $$

CREATE TRIGGER before_pfleger_insert
BEFORE INSERT ON pfleger
FOR EACH ROW
BEGIN
    IF NEW.Personalnummer IS NULL THEN
        SET NEW.Personalnummer = CONCAT('PFL', LPAD((SELECT IFNULL(MAX(pflegerID), 0) + 1 FROM pfleger), 4, '0'));
    END IF;
END$$

DELIMITER ;

-- ===============================================
-- FERTIG! Die Datenbank ist jetzt bereit für:
-- ✅ Fotos
-- ✅ Erweiterte Pfleger-Informationen
-- ✅ Tier-Zuordnungen
-- ✅ Assistenten-Hierarchie
-- ✅ Aufgabenbereiche
-- ===============================================
