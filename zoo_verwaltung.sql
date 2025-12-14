-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 14. Dez 2025 um 22:56
-- Server-Version: 10.4.32-MariaDB
-- PHP-Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `zoo_verwaltung`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `arbeitszeiten`
--

CREATE TABLE `arbeitszeiten` (
  `arbeitszeitID` int(11) NOT NULL,
  `pflegerID` int(11) NOT NULL,
  `Datum` date NOT NULL,
  `Startzeit` time NOT NULL,
  `Endzeit` time NOT NULL,
  `Pause_minuten` int(11) DEFAULT 30,
  `Arbeitsstunden` decimal(4,2) GENERATED ALWAYS AS (timestampdiff(MINUTE,`Startzeit`,`Endzeit`) / 60.0 - `Pause_minuten` / 60.0) STORED,
  `Notizen` text DEFAULT NULL,
  `Bestätigt_von` int(11) DEFAULT NULL,
  `Bestätigt_am` datetime DEFAULT NULL,
  `GehegeID` int(11) DEFAULT NULL,
  `AufgabenbereichID` int(11) DEFAULT NULL,
  `Krank` tinyint(1) DEFAULT 0,
  `Urlaub` tinyint(1) DEFAULT 0,
  `Ueberstunden` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Arbeitszeitnachweise der Pfleger';

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `audit_log`
--

CREATE TABLE `audit_log` (
  `auditID` int(11) NOT NULL,
  `Zeitpunkt` datetime NOT NULL DEFAULT current_timestamp(),
  `PflegerID` int(11) DEFAULT NULL,
  `Aktion` varchar(100) NOT NULL,
  `Tabelle` varchar(50) DEFAULT NULL,
  `Datensatz_ID` int(11) DEFAULT NULL,
  `Alte_Werte` text DEFAULT NULL COMMENT 'JSON mit alten Werten',
  `Neue_Werte` text DEFAULT NULL COMMENT 'JSON mit neuen Werten',
  `IP_Adresse` varchar(45) DEFAULT NULL,
  `User_Agent` text DEFAULT NULL,
  `Notizen` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Audit-Log für alle wichtigen Änderungen';

--
-- Daten für Tabelle `audit_log`
--

INSERT INTO `audit_log` (`auditID`, `Zeitpunkt`, `PflegerID`, `Aktion`, `Tabelle`, `Datensatz_ID`, `Alte_Werte`, `Neue_Werte`, `IP_Adresse`, `User_Agent`, `Notizen`) VALUES
(1, '2025-12-14 19:56:07', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(2, '2025-12-14 20:20:35', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(3, '2025-12-14 20:30:26', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(4, '2025-12-14 20:39:47', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(5, '2025-12-14 20:41:54', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(6, '2025-12-14 20:42:16', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(7, '2025-12-14 20:42:38', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(8, '2025-12-14 20:46:07', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(9, '2025-12-14 21:29:07', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(10, '2025-12-14 21:29:51', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(11, '2025-12-14 21:31:55', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL),
(12, '2025-12-14 21:38:26', NULL, 'UPDATE', 'pfleger', 3, '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', '{\"Vorname\": \"Michael\", \"Nachname\": \"Weber\", \"Rolle\": \"Pfleger\", \"Aktiv\": 1, \"Account_aktiv\": 1, \"Gehalt\": 2800.00}', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `aufgabenbereich`
--

CREATE TABLE `aufgabenbereich` (
  `aufgabenbereichID` int(11) NOT NULL,
  `Bezeichnung` varchar(100) NOT NULL,
  `Beschreibung` text DEFAULT NULL,
  `Farbe` varchar(7) DEFAULT NULL COMMENT 'Hex-Farbe für UI',
  `Icon` varchar(50) DEFAULT NULL COMMENT 'FontAwesome Icon',
  `Prioritaet` int(11) DEFAULT 1,
  `Kategorie` enum('Pflege','Reinigung','Medizin','Training','Verwaltung','Besucher') DEFAULT 'Pflege',
  `Sichtbar_fuer` enum('Alle','Hauptpfleger','Admin') DEFAULT 'Alle',
  `Aktiv` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Vordefinierte Aufgabenbereiche';

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `bestellung`
--

CREATE TABLE `bestellung` (
  `bestellungID` int(11) NOT NULL,
  `Bestelldatum` date NOT NULL,
  `Lieferdatum` date DEFAULT NULL,
  `Status` enum('Entwurf','Offen','Bestätigt','In Bearbeitung','Versendet','Geliefert','Storniert','Reklamation') DEFAULT 'Offen',
  `Gesamtpreis` decimal(10,2) NOT NULL DEFAULT 0.00,
  `Lieferant` varchar(100) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `anbieterID` int(11) DEFAULT NULL,
  `Besteller_Name` varchar(100) DEFAULT NULL,
  `Zahlungsstatus` enum('Offen','Teilweise bezahlt','Bezahlt','Überfällig') DEFAULT 'Offen',
  `Rechnungsnummer` varchar(50) DEFAULT NULL,
  `Lieferkosten` decimal(10,2) DEFAULT 0.00,
  `MwSt_prozent` decimal(5,2) DEFAULT 19.00,
  `Bestellungsart` enum('Normal','Dringend','Automatisch') DEFAULT 'Normal',
  `Eingereicht_von` varchar(100) DEFAULT NULL,
  `Genehmigt_von` varchar(100) DEFAULT NULL,
  `Genehmigt_am` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `bestellung_position`
--

CREATE TABLE `bestellung_position` (
  `positionID` int(11) NOT NULL,
  `bestellungID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `Menge` decimal(10,2) NOT NULL,
  `Einzelpreis` decimal(10,2) NOT NULL,
  `Gesamtpreis` decimal(10,2) NOT NULL,
  `Gelieferte_Menge` decimal(10,2) DEFAULT 0.00,
  `Reklamationsgrund` text DEFAULT NULL,
  `Status` enum('Bestellt','Teillieferung','Geliefert','Reklamation') DEFAULT 'Bestellt',
  `Charge_Nr` varchar(50) DEFAULT NULL,
  `MHD` date DEFAULT NULL COMMENT 'Mindesthaltbarkeitsdatum'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Trigger `bestellung_position`
--
DELIMITER $$
CREATE TRIGGER `after_bestellung_position_insert` AFTER INSERT ON `bestellung_position` FOR EACH ROW BEGIN
  -- Aktualisiere Futter-Lagerbestand, wenn Bestellung geliefert wird
  UPDATE `futter` f
  JOIN `bestellung` b ON NEW.`bestellungID` = b.`bestellungID`
  SET f.`Lagerbestand` = f.`Lagerbestand` + NEW.`Gelieferte_Menge`
  WHERE f.`futterID` = NEW.`futterID` 
    AND b.`Status` = 'Geliefert'
    AND NEW.`Status` = 'Geliefert';
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `besucher`
--

CREATE TABLE `besucher` (
  `besuchID` int(11) NOT NULL,
  `Datum` date NOT NULL,
  `Anzahl_Besucher` int(11) NOT NULL,
  `Umsatz` decimal(10,2) DEFAULT 0.00,
  `Wetter` varchar(50) DEFAULT NULL,
  `Besonderes_Event` varchar(200) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Geoeffnet_von` time DEFAULT NULL,
  `Geoeffnet_bis` time DEFAULT NULL,
  `Durchschnittliche_Verweildauer` int(11) DEFAULT NULL COMMENT 'in Minuten'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fuetterungsprotokoll`
--

CREATE TABLE `fuetterungsprotokoll` (
  `protokollID` int(11) NOT NULL,
  `tierID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `Menge` decimal(10,2) NOT NULL,
  `Fuetterungszeit` datetime NOT NULL,
  `Pfleger_Name` varchar(100) DEFAULT NULL,
  `Bemerkungen` text DEFAULT NULL,
  `PflegerID` int(11) DEFAULT NULL,
  `Ist_Geplanter_Futterplan` tinyint(1) DEFAULT 1,
  `Besonderheiten` text DEFAULT NULL,
  `Charge_Nr` varchar(50) DEFAULT NULL COMMENT 'Futter-Chargennummer',
  `Abgelaufen` tinyint(1) DEFAULT 0 COMMENT 'War das Futter abgelaufen?'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `futter`
--

CREATE TABLE `futter` (
  `futterID` int(11) NOT NULL,
  `Bezeichnung` varchar(100) NOT NULL,
  `Einheit` varchar(20) NOT NULL,
  `Preis_pro_Einheit` decimal(10,2) NOT NULL,
  `Lagerbestand` decimal(10,2) NOT NULL DEFAULT 0.00,
  `Mindestbestand` decimal(10,2) NOT NULL DEFAULT 50.00,
  `Bestellmenge` decimal(10,2) NOT NULL DEFAULT 100.00,
  `Kategorie` varchar(50) DEFAULT NULL,
  `Beschreibung` text DEFAULT NULL,
  `Bildpfad` varchar(500) DEFAULT NULL,
  `Naehrwert_info` text DEFAULT NULL COMMENT 'Nährwertinformationen',
  `Haltbarkeit_Tage` int(11) DEFAULT NULL COMMENT 'Haltbarkeit in Tagen',
  `Lagerort` varchar(100) DEFAULT NULL,
  `Best_Before` date DEFAULT NULL,
  `Status` enum('Verfügbar','Knapp','Ausverkauft','Abgelaufen') DEFAULT 'Verfügbar'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `futter`
--

INSERT INTO `futter` (`futterID`, `Bezeichnung`, `Einheit`, `Preis_pro_Einheit`, `Lagerbestand`, `Mindestbestand`, `Bestellmenge`, `Kategorie`, `Beschreibung`, `Bildpfad`, `Naehrwert_info`, `Haltbarkeit_Tage`, `Lagerort`, `Best_Before`, `Status`) VALUES
(1, 'Rindfleisch', 'kg', 8.50, 150.00, 100.00, 100.00, 'Fleisch', NULL, NULL, NULL, NULL, NULL, NULL, 'Verfügbar'),
(2, 'Heu', 'kg', 2.00, 50.00, 100.00, 100.00, 'Pflanzen', NULL, NULL, NULL, NULL, NULL, NULL, 'Verfügbar'),
(3, 'Bambus', 'kg', 5.00, 20.00, 50.00, 100.00, 'Pflanzen', NULL, NULL, NULL, NULL, NULL, NULL, 'Verfügbar'),
(4, 'Obst-Mix', 'kg', 3.50, 180.00, 120.00, 100.00, 'Obst', NULL, NULL, NULL, NULL, NULL, NULL, 'Verfügbar'),
(5, 'Gemüse-Mix', 'kg', 2.50, 200.00, 100.00, 100.00, 'Gemüse', NULL, NULL, NULL, NULL, NULL, NULL, 'Verfügbar');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `futter_anbieter`
--

CREATE TABLE `futter_anbieter` (
  `anbieterID` int(11) NOT NULL,
  `Firmenname` varchar(100) NOT NULL,
  `Kontaktperson` varchar(100) DEFAULT NULL,
  `Telefon` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Webseite` varchar(200) DEFAULT NULL,
  `Bewertung` decimal(2,1) DEFAULT 0.0 CHECK (`Bewertung` >= 0 and `Bewertung` <= 5),
  `Notizen` text DEFAULT NULL,
  `Adresse` varchar(200) DEFAULT NULL,
  `PLZ` varchar(10) DEFAULT NULL,
  `Ort` varchar(100) DEFAULT NULL,
  `Land` varchar(50) DEFAULT 'Deutschland',
  `Zahlungsbedingungen` varchar(100) DEFAULT NULL,
  `Lieferkosten` decimal(10,2) DEFAULT 0.00,
  `Mindestbestellwert` decimal(10,2) DEFAULT 0.00,
  `Status` enum('Aktiv','Inaktiv','Gesperrt') DEFAULT 'Aktiv',
  `Zuletzt_bestellt` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `futter_anbieter_preis`
--

CREATE TABLE `futter_anbieter_preis` (
  `preisID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `anbieterID` int(11) NOT NULL,
  `Preis` decimal(10,2) NOT NULL,
  `Lieferzeit_Tage` int(11) DEFAULT 7,
  `Mindestbestellmenge` decimal(10,2) DEFAULT 0.00,
  `Gueltig_ab` date NOT NULL,
  `Gueltig_bis` date DEFAULT NULL,
  `Rabatt_ab_Menge` decimal(10,2) DEFAULT NULL,
  `Rabatt_prozent` decimal(5,2) DEFAULT NULL,
  `Sonderangebot` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `futter_preisverlauf`
--

CREATE TABLE `futter_preisverlauf` (
  `verlaufID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `Datum` date NOT NULL,
  `Boersenpreis` decimal(10,2) NOT NULL,
  `Waehrung` varchar(3) DEFAULT 'EUR',
  `Quelle` varchar(100) DEFAULT NULL,
  `Aenderung_prozent` decimal(5,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `gehege`
--

CREATE TABLE `gehege` (
  `gID` int(11) NOT NULL,
  `GBezeichnung` varchar(100) NOT NULL,
  `kontinentID` int(11) NOT NULL,
  `Flaeche_m2` int(11) DEFAULT NULL,
  `Baujahr` year(4) DEFAULT NULL,
  `Kapazitaet` int(11) DEFAULT NULL,
  `Temperatur_min` decimal(4,1) DEFAULT NULL COMMENT 'Minimale Temperatur (°C)',
  `Temperatur_max` decimal(4,1) DEFAULT NULL COMMENT 'Maximale Temperatur (°C)',
  `Luftfeuchtigkeit_min` int(11) DEFAULT NULL COMMENT 'Minimale Luftfeuchtigkeit (%)',
  `Luftfeuchtigkeit_max` int(11) DEFAULT NULL COMMENT 'Maximale Luftfeuchtigkeit (%)',
  `Besonderheiten` text DEFAULT NULL COMMENT 'Besondere Gehege-Eigenschaften',
  `Wartungsdatum` date DEFAULT NULL COMMENT 'Datum der letzten Wartung',
  `Naechste_Wartung` date DEFAULT NULL COMMENT 'Nächste geplante Wartung',
  `Bildpfad` varchar(500) DEFAULT NULL,
  `Status` enum('Aktiv','In Wartung','Geschlossen') DEFAULT 'Aktiv',
  `Notizen` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `gehege`
--

INSERT INTO `gehege` (`gID`, `GBezeichnung`, `kontinentID`, `Flaeche_m2`, `Baujahr`, `Kapazitaet`, `Temperatur_min`, `Temperatur_max`, `Luftfeuchtigkeit_min`, `Luftfeuchtigkeit_max`, `Besonderheiten`, `Wartungsdatum`, `Naechste_Wartung`, `Bildpfad`, `Status`, `Notizen`) VALUES
(1, 'Savanne Afrika', 1, 5000, '2010', 15, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Aktiv', NULL),
(2, 'Tropenwald Asien', 2, 3000, '2015', 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Aktiv', NULL),
(3, 'Alpen Europa', 3, 2000, '2008', 8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Aktiv', NULL),
(4, 'Prärie Nordamerika', 4, 4000, '2012', 12, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Aktiv', NULL),
(5, 'Amazonas Südamerika', 5, 3500, '2018', 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Aktiv', NULL),
(6, 'Outback Australien', 6, 4500, '2016', 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Aktiv', NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `impfungen`
--

CREATE TABLE `impfungen` (
  `impfungID` int(11) NOT NULL,
  `tierID` int(11) NOT NULL,
  `Impfung` varchar(100) NOT NULL,
  `Datum` date NOT NULL,
  `Naechste_Impfung` date DEFAULT NULL,
  `Tierarzt` varchar(100) DEFAULT NULL,
  `Charge_Nr` varchar(50) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Impfstoff_Hersteller` varchar(100) DEFAULT NULL,
  `PflegerID` int(11) DEFAULT NULL,
  `Kosten` decimal(10,2) DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `kontinent`
--

CREATE TABLE `kontinent` (
  `kID` int(11) NOT NULL,
  `Kbezeichnung` varchar(100) NOT NULL,
  `Klimazone` varchar(50) DEFAULT NULL,
  `Beschreibung` text DEFAULT NULL,
  `Bildpfad` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `kontinent`
--

INSERT INTO `kontinent` (`kID`, `Kbezeichnung`, `Klimazone`, `Beschreibung`, `Bildpfad`) VALUES
(1, 'Afrika', 'Tropisch', 'Savannen, Wüsten, Regenwälder', NULL),
(2, 'Asien', 'Tropisch', 'Regenwälder, Gebirge, Steppen', NULL),
(3, 'Europa', 'Gemäßigt', 'Wälder, Gebirge, Küsten', NULL),
(4, 'Nordamerika', 'Gemäßigt', 'Prärien, Wälder, Gebirge', NULL),
(5, 'Südamerika', 'Tropisch', 'Regenwälder, Anden, Pampa', NULL),
(6, 'Australien', 'Tropisch', 'Outback, Regenwälder, Küsten', NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `logbuch`
--

CREATE TABLE `logbuch` (
  `logID` int(11) NOT NULL,
  `Datum` datetime NOT NULL DEFAULT current_timestamp(),
  `PflegerID` int(11) DEFAULT NULL,
  `TierID` int(11) DEFAULT NULL,
  `GehegeID` int(11) DEFAULT NULL,
  `Kategorie` enum('Fütterung','Reinigung','Gesundheit','Verhalten','Wartung','Besucher','Training','Sonstiges') NOT NULL,
  `Beschreibung` text NOT NULL,
  `Wichtigkeit` enum('Normal','Wichtig','Kritisch') DEFAULT 'Normal',
  `Erledigt` tinyint(1) DEFAULT 0,
  `Erledigt_am` datetime DEFAULT NULL,
  `Erledigt_von` int(11) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Bildpfad` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `login_sessions`
--

CREATE TABLE `login_sessions` (
  `sessionID` varchar(128) NOT NULL,
  `pflegerID` int(11) NOT NULL,
  `Login_Zeit` datetime NOT NULL DEFAULT current_timestamp(),
  `Letzte_Aktivitaet` datetime NOT NULL DEFAULT current_timestamp(),
  `IP_Adresse` varchar(45) DEFAULT NULL,
  `User_Agent` text DEFAULT NULL,
  `Gueltig_bis` datetime NOT NULL,
  `Session_Data` text DEFAULT NULL COMMENT 'JSON-Daten für Session'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Aktive Login-Sessions';

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `medikamente`
--

CREATE TABLE `medikamente` (
  `medikamentID` int(11) NOT NULL,
  `Bezeichnung` varchar(100) NOT NULL,
  `Wirkstoff` varchar(100) DEFAULT NULL,
  `Darreichungsform` enum('Tablette','Injektion','Salbe','Tropfen','Pulver') DEFAULT 'Tablette',
  `Staerke` varchar(50) DEFAULT NULL,
  `Lagerbestand` int(11) DEFAULT 0,
  `Mindestbestand` int(11) DEFAULT 10,
  `Einheit` varchar(20) DEFAULT NULL,
  `MHD` date DEFAULT NULL,
  `Lagerort` varchar(100) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Rezeptpflichtig` tinyint(1) DEFAULT 0,
  `Tierart_Beschraenkung` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `medikamenten_gabe`
--

CREATE TABLE `medikamenten_gabe` (
  `gabeID` int(11) NOT NULL,
  `tierID` int(11) NOT NULL,
  `medikamentID` int(11) NOT NULL,
  `Datum` datetime NOT NULL,
  `Menge` decimal(10,2) NOT NULL,
  `Einheit` varchar(20) DEFAULT NULL,
  `Verabreicht_von` varchar(100) DEFAULT NULL,
  `PflegerID` int(11) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Naechste_Gabe` datetime DEFAULT NULL,
  `Behandlungsgrund` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `pfleger`
--

CREATE TABLE `pfleger` (
  `pflegerID` int(11) NOT NULL,
  `Vorname` varchar(50) NOT NULL,
  `Nachname` varchar(50) NOT NULL,
  `Personalnummer` varchar(20) NOT NULL,
  `Geburtsdatum` date NOT NULL,
  `Einstellungsdatum` date NOT NULL,
  `Position` enum('Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Reinigungskraft','Führungskraft') DEFAULT 'Pfleger',
  `Gehalt` decimal(10,2) NOT NULL DEFAULT 0.00,
  `Telefon` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Adresse` varchar(200) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Foto` varchar(500) DEFAULT NULL COMMENT 'Pfad zum Pfleger-Foto',
  `Aktiv` tinyint(1) DEFAULT 1,
  `Steuerklasse` int(11) DEFAULT NULL COMMENT 'Steuerklasse 1-6',
  `Sozialversicherungsnummer` varchar(20) DEFAULT NULL COMMENT 'SV-Nummer',
  `Bankverbindung` varchar(200) DEFAULT NULL COMMENT 'IBAN',
  `Krankenkasse` varchar(100) DEFAULT NULL COMMENT 'Name der Krankenkasse',
  `HauptpflegerID` int(11) DEFAULT NULL COMMENT 'Referenz zum vorgesetzten Hauptpfleger',
  `IstHauptpfleger` tinyint(1) DEFAULT 0 COMMENT 'Ist diese Person ein Hauptpfleger?',
  `Aufgabenbereich` text DEFAULT NULL COMMENT 'Zugewiesene Aufgabenbereiche',
  `Qualifikationen` text DEFAULT NULL COMMENT 'Berufliche Qualifikationen',
  `Notfallkontakt` varchar(200) DEFAULT NULL COMMENT 'Notfallkontakt Person + Telefon',
  `Benutzername` varchar(50) DEFAULT NULL COMMENT 'Benutzername für Login',
  `Passwort` varchar(255) DEFAULT NULL COMMENT 'Passwort-Hash für Login',
  `Rolle` enum('Admin','Supervisor','Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Leser') DEFAULT 'Pfleger',
  `Letzter_Login` datetime DEFAULT NULL COMMENT 'Zeitpunkt des letzten Logins',
  `Passwort_geaendert_am` date DEFAULT NULL,
  `Account_aktiv` tinyint(1) DEFAULT 1,
  `Login_Versuche` int(11) DEFAULT 0,
  `Account_gesperrt_bis` datetime DEFAULT NULL,
  `Urlaubstage_pro_Jahr` int(11) DEFAULT 30,
  `Resturlaub` int(11) DEFAULT 30,
  `Krankentage_aktuelles_Jahr` int(11) DEFAULT 0,
  `Fortbildungen` text DEFAULT NULL,
  `Ausweis_Nr` varchar(50) DEFAULT NULL,
  `Arbeitszeitmodell` enum('Vollzeit','Teilzeit','Minijob','Auszubildend') DEFAULT 'Vollzeit',
  `Wochenstunden` decimal(5,2) DEFAULT 40.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `pfleger`
--

INSERT INTO `pfleger` (`pflegerID`, `Vorname`, `Nachname`, `Personalnummer`, `Geburtsdatum`, `Einstellungsdatum`, `Position`, `Gehalt`, `Telefon`, `Email`, `Adresse`, `Notizen`, `Foto`, `Aktiv`, `Steuerklasse`, `Sozialversicherungsnummer`, `Bankverbindung`, `Krankenkasse`, `HauptpflegerID`, `IstHauptpfleger`, `Aufgabenbereich`, `Qualifikationen`, `Notfallkontakt`, `Benutzername`, `Passwort`, `Rolle`, `Letzter_Login`, `Passwort_geaendert_am`, `Account_aktiv`, `Login_Versuche`, `Account_gesperrt_bis`, `Urlaubstage_pro_Jahr`, `Resturlaub`, `Krankentage_aktuelles_Jahr`, `Fortbildungen`, `Ausweis_Nr`, `Arbeitszeitmodell`, `Wochenstunden`) VALUES
(37, 'Thomas', 'Müller', 'PFL0001', '1985-03-15', '2010-01-01', 'Hauptpfleger', 3500.00, NULL, 'thomas.mueller@zoo.de', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, 'thomas.mueller', 'faf216fd6a2e93214eeec7e0301a94f4', 'Hauptpfleger', NULL, NULL, 1, 0, NULL, 30, 30, 0, NULL, NULL, 'Vollzeit', 40.00),
(38, 'Sarah', 'Schmidt', 'PFL0002', '1990-07-22', '2015-06-01', 'Hauptpfleger', 3200.00, NULL, 'sarah.schmidt@zoo.de', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, 'sarah.schmidt', 'faf216fd6a2e93214eeec7e0301a94f4', 'Hauptpfleger', NULL, NULL, 1, 0, NULL, 30, 30, 0, NULL, NULL, 'Vollzeit', 40.00),
(39, 'Michael', 'Weber', 'PFL0003', '1992-11-08', '2018-03-15', 'Pfleger', 2800.00, NULL, 'michael.weber@zoo.de', NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 'michael.weber', 'faf216fd6a2e93214eeec7e0301a94f4', 'Pfleger', NULL, NULL, 1, 0, NULL, 30, 30, 0, NULL, NULL, 'Vollzeit', 40.00);

--
-- Trigger `pfleger`
--
DELIMITER $$
CREATE TRIGGER `after_pfleger_update` AFTER UPDATE ON `pfleger` FOR EACH ROW BEGIN
  DECLARE old_json TEXT;
  DECLARE new_json TEXT;
  
  -- Erstelle JSON mit geänderten Werten
  SET old_json = JSON_OBJECT(
    'Vorname', OLD.`Vorname`,
    'Nachname', OLD.`Nachname`,
    'Rolle', OLD.`Rolle`,
    'Aktiv', OLD.`Aktiv`,
    'Account_aktiv', OLD.`Account_aktiv`,
    'Gehalt', OLD.`Gehalt`
  );
  
  SET new_json = JSON_OBJECT(
    'Vorname', NEW.`Vorname`,
    'Nachname', NEW.`Nachname`,
    'Rolle', NEW.`Rolle`,
    'Aktiv', NEW.`Aktiv`,
    'Account_aktiv', NEW.`Account_aktiv`,
    'Gehalt', NEW.`Gehalt`
  );
  
  INSERT INTO `audit_log` (`PflegerID`, `Aktion`, `Tabelle`, `Datensatz_ID`, `Alte_Werte`, `Neue_Werte`)
  VALUES (NEW.`pflegerID`, 'UPDATE', 'pfleger', NEW.`pflegerID`, old_json, new_json);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `before_pfleger_insert` BEFORE INSERT ON `pfleger` FOR EACH ROW BEGIN
  IF NEW.`Personalnummer` IS NULL OR NEW.`Personalnummer` = '' THEN
    SET NEW.`Personalnummer` = CONCAT('PFL', LPAD((SELECT IFNULL(MAX(`pflegerID`), 0) + 1 FROM `pfleger`), 4, '0'));
  END IF;
  
  -- Setze IstHauptpfleger basierend auf Position
  IF NEW.`Position` = 'Hauptpfleger' THEN
    SET NEW.`IstHauptpfleger` = TRUE;
    SET NEW.`Rolle` = 'Hauptpfleger';
  ELSEIF NEW.`Position` = 'Auszubildender' THEN
    SET NEW.`Rolle` = 'Auszubildender';
  ELSEIF NEW.`Position` = 'Tierarzt' THEN
    SET NEW.`Rolle` = 'Tierarzt';
  ELSE
    SET NEW.`Rolle` = 'Pfleger';
  END IF;
  
  -- Setze Standard-Passwort, wenn keins angegeben
  IF NEW.`Passwort` IS NULL THEN
    SET NEW.`Passwort` = MD5('zoo123');
  END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `pfleger_aufgabenbereich`
--

CREATE TABLE `pfleger_aufgabenbereich` (
  `zuordnungID` int(11) NOT NULL,
  `pflegerID` int(11) NOT NULL,
  `aufgabenbereichID` int(11) NOT NULL,
  `Hauptverantwortlich` tinyint(1) DEFAULT 0,
  `ZugeordnetAm` date NOT NULL DEFAULT curdate(),
  `Bis_Datum` date DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Stunden_pro_Woche` decimal(5,2) DEFAULT 0.00,
  `Prioritaet` enum('Hoch','Mittel','Niedrig') DEFAULT 'Mittel'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Zuordnung von Aufgabenbereichen zu Pflegern';

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `pfleger_gehege`
--

CREATE TABLE `pfleger_gehege` (
  `zuordnungID` int(11) NOT NULL,
  `pflegerID` int(11) NOT NULL,
  `gehegeID` int(11) NOT NULL,
  `Hauptverantwortlich` tinyint(1) DEFAULT 0,
  `ZugeordnetAm` date NOT NULL DEFAULT curdate(),
  `Bis_Datum` date DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Stunden_pro_Woche` decimal(5,2) DEFAULT 0.00,
  `Arbeitszeit` enum('Vormittag','Nachmittag','Ganztags','Nachtschicht') DEFAULT 'Ganztags'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `pfleger_tier`
--

CREATE TABLE `pfleger_tier` (
  `zuordnungID` int(11) NOT NULL,
  `pflegerID` int(11) NOT NULL,
  `tierID` int(11) NOT NULL,
  `IstHauptpfleger` tinyint(1) DEFAULT 1 COMMENT 'Hauptpfleger oder Assistent',
  `ZugeordnetAm` date NOT NULL DEFAULT curdate(),
  `Bis_Datum` date DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Prioritaet` enum('Primär','Sekundär','Vertretung') DEFAULT 'Primär',
  `Vertrauensstufe` enum('Neu','Etabliert','Experte') DEFAULT 'Neu'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Zuordnung von Pflegern zu Tieren';

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tierart`
--

CREATE TABLE `tierart` (
  `tierartID` int(11) NOT NULL,
  `TABezeichnung` varchar(100) NOT NULL,
  `Wissenschaftlicher_Name` varchar(100) DEFAULT NULL,
  `Gefaehrdungsstatus` enum('Ausgestorben','Vom Aussterben bedroht','Stark gefährdet','Gefährdet','Potenziell gefährdet','Nicht gefährdet') DEFAULT 'Nicht gefährdet',
  `Lebensraum` varchar(200) DEFAULT NULL,
  `Ernaehrung` varchar(200) DEFAULT NULL,
  `Beschreibung` text DEFAULT NULL,
  `Bildpfad` varchar(500) DEFAULT NULL,
  `Max_Groesse_cm` int(11) DEFAULT NULL COMMENT 'Maximale Körpergröße in cm',
  `Max_Gewicht_kg` decimal(10,2) DEFAULT NULL COMMENT 'Maximales Gewicht in kg',
  `Lebenserwartung_Jahre` int(11) DEFAULT NULL,
  `Tragzeit_Tage` int(11) DEFAULT NULL,
  `Wurfgroesse_min` int(11) DEFAULT NULL,
  `Wurfgroesse_max` int(11) DEFAULT NULL,
  `Besonderheiten` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tierart_futter`
--

CREATE TABLE `tierart_futter` (
  `tierart_futterID` int(11) NOT NULL,
  `tierartID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `Menge_pro_Tag` decimal(10,2) NOT NULL,
  `Fuetterungszeit` time DEFAULT NULL,
  `Prioritaet` enum('Hauptfutter','Zusatzfutter','Leckerli') DEFAULT 'Hauptfutter',
  `Notizen` text DEFAULT NULL,
  `Gueltig_ab` date DEFAULT curdate(),
  `Gueltig_bis` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tierarzt_besuche`
--

CREATE TABLE `tierarzt_besuche` (
  `besuchID` int(11) NOT NULL,
  `tierID` int(11) NOT NULL,
  `Datum` datetime NOT NULL,
  `Tierarzt_Name` varchar(100) NOT NULL,
  `Diagnose` text DEFAULT NULL,
  `Behandlung` text DEFAULT NULL,
  `Medikamente` text DEFAULT NULL,
  `Kosten` decimal(10,2) DEFAULT 0.00,
  `Naechster_Termin` date DEFAULT NULL,
  `Dringlichkeit` enum('Routine','Dringend','Notfall') DEFAULT 'Routine',
  `PflegerID` int(11) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Rechnungsnummer` varchar(50) DEFAULT NULL,
  `Versicherung_uebernommen` decimal(10,2) DEFAULT 0.00,
  `Behandlungsdauer_min` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tiere`
--

CREATE TABLE `tiere` (
  `tierID` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Spitzname` varchar(100) DEFAULT NULL,
  `Gewicht` decimal(10,2) NOT NULL,
  `Geburtsdatum` date NOT NULL,
  `Bildpfad` varchar(500) DEFAULT NULL,
  `Notizen` text DEFAULT NULL,
  `Geschlecht` enum('Männlich','Weiblich','Unbekannt') DEFAULT 'Unbekannt',
  `Gesundheitszustand` enum('Gesund','Leicht erkrankt','Schwer erkrankt','In Behandlung','Genesen') DEFAULT 'Gesund',
  `TierartID` int(11) NOT NULL,
  `GehegeID` int(11) NOT NULL,
  `Groesse_cm` int(11) DEFAULT NULL COMMENT 'Körpergröße in cm',
  `Herkunft` varchar(100) DEFAULT NULL COMMENT 'Ursprungsort',
  `Mikrochip_Nr` varchar(50) DEFAULT NULL COMMENT 'Mikrochip-Identifikation',
  `Letzter_Gesundheitscheck` date DEFAULT NULL COMMENT 'Datum des letzten Gesundheitschecks',
  `Naechster_Impftermin` date DEFAULT NULL COMMENT 'Nächster Impftermin',
  `Ankunft_im_Zoo` date DEFAULT NULL COMMENT 'Datum der Ankunft',
  `Zuchtbuch_Nr` varchar(50) DEFAULT NULL COMMENT 'Zuchtbuchnummer',
  `Eltern_tierID` int(11) DEFAULT NULL COMMENT 'Referenz zu Elterntier',
  `Geschwister_tierID` int(11) DEFAULT NULL COMMENT 'Referenz zu Geschwistertier',
  `Status` enum('Aktiv','Verstorben','Verlegt','In Quarantäne') DEFAULT 'Aktiv',
  `Verstorben_am` date DEFAULT NULL,
  `Todesursache` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Trigger `tiere`
--
DELIMITER $$
CREATE TRIGGER `after_tiere_update` AFTER UPDATE ON `tiere` FOR EACH ROW BEGIN
  IF OLD.`Status` != NEW.`Status` AND NEW.`Status` = 'Verstorben' THEN
    -- Eintrag ins Logbuch
    INSERT INTO `logbuch` (`TierID`, `Kategorie`, `Beschreibung`, `Wichtigkeit`)
    VALUES (NEW.`tierID`, 'Gesundheit', 
            CONCAT('Tier ', NEW.`Name`, ' verstorben. Todesursache: ', COALESCE(NEW.`Todesursache`, 'Unbekannt')),
            'Kritisch');
    
    -- Entferne alle aktiven Pfleger-Zuordnungen
    UPDATE `pfleger_tier` 
    SET `Bis_Datum` = CURDATE(),
        `Notizen` = CONCAT('Automatisch beendet: Tier verstorben am ', CURDATE())
    WHERE `tierID` = NEW.`tierID` 
      AND (`Bis_Datum` IS NULL OR `Bis_Datum` > CURDATE());
  END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `view_futter_lager`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `view_futter_lager` (
`futterID` int(11)
,`Bezeichnung` varchar(100)
,`Einheit` varchar(20)
,`Preis_pro_Einheit` decimal(10,2)
,`Lagerbestand` decimal(10,2)
,`Mindestbestand` decimal(10,2)
,`Bestellmenge` decimal(10,2)
,`Kategorie` varchar(50)
,`Status` varchar(8)
,`Differenz` decimal(11,2)
,`Nachbestell_Menge` decimal(10,2)
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `view_gehege_statistik`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `view_gehege_statistik` (
`gID` int(11)
,`Gehege` varchar(100)
,`Kontinent` varchar(100)
,`Flaeche_m2` int(11)
,`Anzahl_Tiere` bigint(21)
,`Tierarten` mediumtext
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `view_heutige_fuetterungen`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `view_heutige_fuetterungen` (
`Datum` date
,`Tier` varchar(100)
,`Tierart` varchar(100)
,`Gefuettert` varchar(100)
,`Menge` decimal(10,2)
,`Einheit` varchar(20)
,`Pfleger` varchar(100)
,`Uhrzeit` time
,`Bemerkungen` text
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `view_taeglicher_futterbedarf`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `view_taeglicher_futterbedarf` (
`tierID` int(11)
,`Tiername` varchar(100)
,`Tierart` varchar(100)
,`Gehege` varchar(100)
,`Futtersorte` varchar(100)
,`Menge_pro_Tag` decimal(10,2)
,`Einheit` varchar(20)
,`Fuetterungszeit` time
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `view_tiere_uebersicht`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `view_tiere_uebersicht` (
`tierID` int(11)
,`Name` varchar(100)
,`Spitzname` varchar(100)
,`Gewicht` decimal(10,2)
,`Geburtsdatum` date
,`Alter_Jahre` bigint(21)
,`Geschlecht` enum('Männlich','Weiblich','Unbekannt')
,`Gesundheitszustand` enum('Gesund','Leicht erkrankt','Schwer erkrankt','In Behandlung','Genesen')
,`Tierart` varchar(100)
,`Gehege` varchar(100)
,`Kontinent` varchar(100)
,`Anzeige_Name` varchar(203)
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `v_futter_anbieter_aktuell`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `v_futter_anbieter_aktuell` (
`futterID` int(11)
,`Futtersorte` varchar(100)
,`Anbieter` varchar(100)
,`Preis` decimal(10,2)
,`Einheit` varchar(20)
,`Lieferzeit_Tage` int(11)
,`Mindestbestellmenge` decimal(10,2)
,`Bewertung` decimal(2,1)
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `v_pfleger_aufgaben`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `v_pfleger_aufgaben` (
`pflegerID` int(11)
,`PflegerName` varchar(101)
,`Personalnummer` varchar(20)
,`Rolle` enum('Admin','Supervisor','Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Leser')
,`Aufgabenbereich` varchar(100)
,`AufgabenFarbe` varchar(7)
,`Hauptverantwortlich` tinyint(1)
,`ZugeordnetAm` date
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `v_pfleger_hierarchie`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `v_pfleger_hierarchie` (
`HauptpflegerID` int(11)
,`Hauptpfleger` varchar(101)
,`Hauptpfleger_PN` varchar(20)
,`Hauptpfleger_Rolle` enum('Admin','Supervisor','Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Leser')
,`AssistentenID` int(11)
,`Assistent` varchar(101)
,`Assistenten_PN` varchar(20)
,`Assistenten_Rolle` enum('Admin','Supervisor','Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Leser')
,`Assistenten_Gehalt` decimal(10,2)
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `v_pfleger_komplett`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `v_pfleger_komplett` (
`pflegerID` int(11)
,`Personalnummer` varchar(20)
,`Name` varchar(101)
,`Rolle` enum('Admin','Supervisor','Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Leser')
,`Position` enum('Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Reinigungskraft','Führungskraft')
,`Gehalt` decimal(10,2)
,`Email` varchar(100)
,`Telefon` varchar(20)
,`Aktiv` tinyint(1)
,`Benutzername` varchar(50)
,`Vorgesetzter_Vorname` varchar(50)
,`Vorgesetzter_Nachname` varchar(50)
,`Vorgesetzter` varchar(101)
,`Einstellungsdatum` date
,`Geburtsdatum` date
,`Alter` bigint(21)
,`Dienstjahre` bigint(21)
,`Steuerklasse` int(11)
,`Krankenkasse` varchar(100)
,`Qualifikationen` text
,`Notfallkontakt` varchar(200)
,`Zustaendige_Gehege` mediumtext
,`Aufgabenbereiche` mediumtext
,`Betreute_Tierarten` mediumtext
,`Anzahl_Tiere` bigint(21)
,`Anzahl_Gehege` bigint(21)
,`Anzahl_Aufgabenbereiche` bigint(21)
,`Arbeitstage_dieser_Monat` bigint(21)
,`Arbeitsstunden_dieser_Monat` decimal(26,2)
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `v_pfleger_tiere`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `v_pfleger_tiere` (
`pflegerID` int(11)
,`Vorname` varchar(50)
,`Nachname` varchar(50)
,`Personalnummer` varchar(20)
,`Rolle` enum('Admin','Supervisor','Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Leser')
,`tierID` int(11)
,`TierName` varchar(100)
,`Tierart` varchar(100)
,`IstHauptpfleger` tinyint(1)
,`ZugeordnetAm` date
);

-- --------------------------------------------------------

--
-- Stellvertreter-Struktur des Views `v_pfleger_uebersicht`
-- (Siehe unten für die tatsächliche Ansicht)
--
CREATE TABLE `v_pfleger_uebersicht` (
`pflegerID` int(11)
,`Personalnummer` varchar(20)
,`Name` varchar(101)
,`Position` enum('Hauptpfleger','Pfleger','Auszubildender','Tierarzt','Reinigungskraft','Führungskraft')
,`Gehalt` decimal(10,2)
,`Email` varchar(100)
,`Aktiv` tinyint(1)
,`Zustaendige_Gehege` mediumtext
,`Anzahl_Gehege` bigint(21)
);

-- --------------------------------------------------------

--
-- Struktur des Views `view_futter_lager`
--
DROP TABLE IF EXISTS `view_futter_lager`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_futter_lager`  AS SELECT `f`.`futterID` AS `futterID`, `f`.`Bezeichnung` AS `Bezeichnung`, `f`.`Einheit` AS `Einheit`, `f`.`Preis_pro_Einheit` AS `Preis_pro_Einheit`, `f`.`Lagerbestand` AS `Lagerbestand`, `f`.`Mindestbestand` AS `Mindestbestand`, `f`.`Bestellmenge` AS `Bestellmenge`, `f`.`Kategorie` AS `Kategorie`, CASE WHEN `f`.`Lagerbestand` = 0 THEN 'KRITISCH' WHEN `f`.`Lagerbestand` <= `f`.`Mindestbestand` THEN 'NIEDRIG' ELSE 'OK' END AS `Status`, `f`.`Lagerbestand`- `f`.`Mindestbestand` AS `Differenz`, CASE WHEN `f`.`Lagerbestand` <= `f`.`Mindestbestand` THEN `f`.`Bestellmenge` ELSE 0 END AS `Nachbestell_Menge` FROM `futter` AS `f` ORDER BY CASE WHEN `f`.`Lagerbestand` = 0 THEN 1 WHEN `f`.`Lagerbestand` <= `f`.`Mindestbestand` THEN 2 ELSE 3 END ASC, `f`.`Lagerbestand` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `view_gehege_statistik`
--
DROP TABLE IF EXISTS `view_gehege_statistik`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_gehege_statistik`  AS SELECT `g`.`gID` AS `gID`, `g`.`GBezeichnung` AS `Gehege`, `k`.`Kbezeichnung` AS `Kontinent`, `g`.`Flaeche_m2` AS `Flaeche_m2`, count(`t`.`tierID`) AS `Anzahl_Tiere`, group_concat(distinct `ta`.`TABezeichnung` order by `ta`.`TABezeichnung` ASC separator ', ') AS `Tierarten` FROM (((`gehege` `g` left join `kontinent` `k` on(`g`.`kontinentID` = `k`.`kID`)) left join `tiere` `t` on(`g`.`gID` = `t`.`GehegeID`)) left join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) GROUP BY `g`.`gID`, `g`.`GBezeichnung`, `k`.`Kbezeichnung`, `g`.`Flaeche_m2` ORDER BY `g`.`GBezeichnung` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `view_heutige_fuetterungen`
--
DROP TABLE IF EXISTS `view_heutige_fuetterungen`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_heutige_fuetterungen`  AS SELECT cast(`fp`.`Fuetterungszeit` as date) AS `Datum`, `t`.`Name` AS `Tier`, `ta`.`TABezeichnung` AS `Tierart`, `f`.`Bezeichnung` AS `Gefuettert`, `fp`.`Menge` AS `Menge`, `f`.`Einheit` AS `Einheit`, `fp`.`Pfleger_Name` AS `Pfleger`, cast(`fp`.`Fuetterungszeit` as time) AS `Uhrzeit`, `fp`.`Bemerkungen` AS `Bemerkungen` FROM (((`fuetterungsprotokoll` `fp` join `tiere` `t` on(`fp`.`tierID` = `t`.`tierID`)) join `futter` `f` on(`fp`.`futterID` = `f`.`futterID`)) join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) WHERE cast(`fp`.`Fuetterungszeit` as date) = curdate() ORDER BY `fp`.`Fuetterungszeit` DESC ;

-- --------------------------------------------------------

--
-- Struktur des Views `view_taeglicher_futterbedarf`
--
DROP TABLE IF EXISTS `view_taeglicher_futterbedarf`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_taeglicher_futterbedarf`  AS SELECT `t`.`tierID` AS `tierID`, `t`.`Name` AS `Tiername`, `ta`.`TABezeichnung` AS `Tierart`, `g`.`GBezeichnung` AS `Gehege`, `f`.`Bezeichnung` AS `Futtersorte`, `tf`.`Menge_pro_Tag` AS `Menge_pro_Tag`, `f`.`Einheit` AS `Einheit`, `tf`.`Fuetterungszeit` AS `Fuetterungszeit` FROM ((((`tiere` `t` join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) join `gehege` `g` on(`t`.`GehegeID` = `g`.`gID`)) join `tierart_futter` `tf` on(`ta`.`tierartID` = `tf`.`tierartID`)) join `futter` `f` on(`tf`.`futterID` = `f`.`futterID`)) WHERE `tf`.`Fuetterungszeit` is not null ORDER BY `tf`.`Fuetterungszeit` ASC, `t`.`Name` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `view_tiere_uebersicht`
--
DROP TABLE IF EXISTS `view_tiere_uebersicht`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_tiere_uebersicht`  AS SELECT `t`.`tierID` AS `tierID`, `t`.`Name` AS `Name`, `t`.`Spitzname` AS `Spitzname`, `t`.`Gewicht` AS `Gewicht`, `t`.`Geburtsdatum` AS `Geburtsdatum`, timestampdiff(YEAR,`t`.`Geburtsdatum`,curdate()) AS `Alter_Jahre`, `t`.`Geschlecht` AS `Geschlecht`, `t`.`Gesundheitszustand` AS `Gesundheitszustand`, `ta`.`TABezeichnung` AS `Tierart`, `g`.`GBezeichnung` AS `Gehege`, `k`.`Kbezeichnung` AS `Kontinent`, concat(`t`.`Name`,' (',`ta`.`TABezeichnung`,')') AS `Anzeige_Name` FROM (((`tiere` `t` left join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) left join `gehege` `g` on(`t`.`GehegeID` = `g`.`gID`)) left join `kontinent` `k` on(`g`.`kontinentID` = `k`.`kID`)) ORDER BY `t`.`Name` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `v_futter_anbieter_aktuell`
--
DROP TABLE IF EXISTS `v_futter_anbieter_aktuell`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_futter_anbieter_aktuell`  AS SELECT `f`.`futterID` AS `futterID`, `f`.`Bezeichnung` AS `Futtersorte`, `a`.`Firmenname` AS `Anbieter`, `fap`.`Preis` AS `Preis`, `f`.`Einheit` AS `Einheit`, `fap`.`Lieferzeit_Tage` AS `Lieferzeit_Tage`, `fap`.`Mindestbestellmenge` AS `Mindestbestellmenge`, `a`.`Bewertung` AS `Bewertung` FROM ((`futter_anbieter_preis` `fap` join `futter` `f` on(`fap`.`futterID` = `f`.`futterID`)) join `futter_anbieter` `a` on(`fap`.`anbieterID` = `a`.`anbieterID`)) WHERE `fap`.`Gueltig_ab` <= curdate() AND (`fap`.`Gueltig_bis` is null OR `fap`.`Gueltig_bis` >= curdate()) ORDER BY `f`.`Bezeichnung` ASC, `fap`.`Preis` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `v_pfleger_aufgaben`
--
DROP TABLE IF EXISTS `v_pfleger_aufgaben`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_pfleger_aufgaben`  AS SELECT `p`.`pflegerID` AS `pflegerID`, concat(`p`.`Vorname`,' ',`p`.`Nachname`) AS `PflegerName`, `p`.`Personalnummer` AS `Personalnummer`, `p`.`Rolle` AS `Rolle`, `a`.`Bezeichnung` AS `Aufgabenbereich`, `a`.`Farbe` AS `AufgabenFarbe`, `pa`.`Hauptverantwortlich` AS `Hauptverantwortlich`, `pa`.`ZugeordnetAm` AS `ZugeordnetAm` FROM ((`pfleger` `p` left join `pfleger_aufgabenbereich` `pa` on(`p`.`pflegerID` = `pa`.`pflegerID`)) left join `aufgabenbereich` `a` on(`pa`.`aufgabenbereichID` = `a`.`aufgabenbereichID`)) WHERE `p`.`Aktiv` = 1 ORDER BY `p`.`Nachname` ASC, `a`.`Bezeichnung` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `v_pfleger_hierarchie`
--
DROP TABLE IF EXISTS `v_pfleger_hierarchie`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_pfleger_hierarchie`  AS SELECT `h`.`pflegerID` AS `HauptpflegerID`, concat(`h`.`Vorname`,' ',`h`.`Nachname`) AS `Hauptpfleger`, `h`.`Personalnummer` AS `Hauptpfleger_PN`, `h`.`Rolle` AS `Hauptpfleger_Rolle`, `a`.`pflegerID` AS `AssistentenID`, concat(`a`.`Vorname`,' ',`a`.`Nachname`) AS `Assistent`, `a`.`Personalnummer` AS `Assistenten_PN`, `a`.`Rolle` AS `Assistenten_Rolle`, `a`.`Gehalt` AS `Assistenten_Gehalt` FROM (`pfleger` `h` left join `pfleger` `a` on(`h`.`pflegerID` = `a`.`HauptpflegerID`)) WHERE `h`.`IstHauptpfleger` = 1 AND `h`.`Aktiv` = 1 ORDER BY `h`.`Nachname` ASC, `a`.`Nachname` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `v_pfleger_komplett`
--
DROP TABLE IF EXISTS `v_pfleger_komplett`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_pfleger_komplett`  AS SELECT `p`.`pflegerID` AS `pflegerID`, `p`.`Personalnummer` AS `Personalnummer`, concat(`p`.`Vorname`,' ',`p`.`Nachname`) AS `Name`, `p`.`Rolle` AS `Rolle`, `p`.`Position` AS `Position`, `p`.`Gehalt` AS `Gehalt`, `p`.`Email` AS `Email`, `p`.`Telefon` AS `Telefon`, `p`.`Aktiv` AS `Aktiv`, `p`.`Benutzername` AS `Benutzername`, `h`.`Vorname` AS `Vorgesetzter_Vorname`, `h`.`Nachname` AS `Vorgesetzter_Nachname`, concat(`h`.`Vorname`,' ',`h`.`Nachname`) AS `Vorgesetzter`, `p`.`Einstellungsdatum` AS `Einstellungsdatum`, `p`.`Geburtsdatum` AS `Geburtsdatum`, timestampdiff(YEAR,`p`.`Geburtsdatum`,curdate()) AS `Alter`, timestampdiff(YEAR,`p`.`Einstellungsdatum`,curdate()) AS `Dienstjahre`, `p`.`Steuerklasse` AS `Steuerklasse`, `p`.`Krankenkasse` AS `Krankenkasse`, `p`.`Qualifikationen` AS `Qualifikationen`, `p`.`Notfallkontakt` AS `Notfallkontakt`, group_concat(distinct `g`.`GBezeichnung` order by `g`.`GBezeichnung` ASC separator ', ') AS `Zustaendige_Gehege`, group_concat(distinct `a`.`Bezeichnung` order by `a`.`Bezeichnung` ASC separator ', ') AS `Aufgabenbereiche`, group_concat(distinct `ta`.`TABezeichnung` order by `ta`.`TABezeichnung` ASC separator ', ') AS `Betreute_Tierarten`, count(distinct `pt`.`tierID`) AS `Anzahl_Tiere`, count(distinct `pg`.`gehegeID`) AS `Anzahl_Gehege`, count(distinct `pa`.`aufgabenbereichID`) AS `Anzahl_Aufgabenbereiche`, (select count(0) from `arbeitszeiten` `az` where `az`.`pflegerID` = `p`.`pflegerID` and month(`az`.`Datum`) = month(curdate())) AS `Arbeitstage_dieser_Monat`, (select coalesce(sum(`az`.`Arbeitsstunden`),0) from `arbeitszeiten` `az` where `az`.`pflegerID` = `p`.`pflegerID` and month(`az`.`Datum`) = month(curdate())) AS `Arbeitsstunden_dieser_Monat` FROM ((((((((`pfleger` `p` left join `pfleger` `h` on(`p`.`HauptpflegerID` = `h`.`pflegerID`)) left join `pfleger_gehege` `pg` on(`p`.`pflegerID` = `pg`.`pflegerID`)) left join `gehege` `g` on(`pg`.`gehegeID` = `g`.`gID`)) left join `pfleger_aufgabenbereich` `pa` on(`p`.`pflegerID` = `pa`.`pflegerID`)) left join `aufgabenbereich` `a` on(`pa`.`aufgabenbereichID` = `a`.`aufgabenbereichID`)) left join `pfleger_tier` `pt` on(`p`.`pflegerID` = `pt`.`pflegerID`)) left join `tiere` `t` on(`pt`.`tierID` = `t`.`tierID`)) left join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) WHERE `p`.`Account_aktiv` = 1 GROUP BY `p`.`pflegerID`, `p`.`Personalnummer`, `p`.`Vorname`, `p`.`Nachname`, `p`.`Rolle`, `p`.`Position`, `p`.`Gehalt`, `p`.`Email`, `p`.`Telefon`, `p`.`Aktiv`, `p`.`Benutzername`, `h`.`Vorname`, `h`.`Nachname`, `p`.`Einstellungsdatum`, `p`.`Geburtsdatum`, `p`.`Steuerklasse`, `p`.`Krankenkasse`, `p`.`Qualifikationen`, `p`.`Notfallkontakt` ORDER BY CASE `p`.`Rolle` WHEN 'Admin' THEN 1 WHEN 'Tierarzt' THEN 2 WHEN 'Hauptpfleger' THEN 3 WHEN 'Pfleger' THEN 4 ELSE 5 END ASC, `p`.`Nachname` ASC, `p`.`Vorname` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `v_pfleger_tiere`
--
DROP TABLE IF EXISTS `v_pfleger_tiere`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_pfleger_tiere`  AS SELECT `p`.`pflegerID` AS `pflegerID`, `p`.`Vorname` AS `Vorname`, `p`.`Nachname` AS `Nachname`, `p`.`Personalnummer` AS `Personalnummer`, `p`.`Rolle` AS `Rolle`, `t`.`tierID` AS `tierID`, `t`.`Name` AS `TierName`, `ta`.`TABezeichnung` AS `Tierart`, `pt`.`IstHauptpfleger` AS `IstHauptpfleger`, `pt`.`ZugeordnetAm` AS `ZugeordnetAm` FROM (((`pfleger` `p` left join `pfleger_tier` `pt` on(`p`.`pflegerID` = `pt`.`pflegerID`)) left join `tiere` `t` on(`pt`.`tierID` = `t`.`tierID`)) left join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) WHERE `p`.`Aktiv` = 1 ORDER BY `p`.`Nachname` ASC, `p`.`Vorname` ASC, `t`.`Name` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `v_pfleger_uebersicht`
--
DROP TABLE IF EXISTS `v_pfleger_uebersicht`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_pfleger_uebersicht`  AS SELECT `p`.`pflegerID` AS `pflegerID`, `p`.`Personalnummer` AS `Personalnummer`, concat(`p`.`Vorname`,' ',`p`.`Nachname`) AS `Name`, `p`.`Position` AS `Position`, `p`.`Gehalt` AS `Gehalt`, `p`.`Email` AS `Email`, `p`.`Aktiv` AS `Aktiv`, group_concat(`g`.`GBezeichnung` separator ', ') AS `Zustaendige_Gehege`, count(`pg`.`gehegeID`) AS `Anzahl_Gehege` FROM ((`pfleger` `p` left join `pfleger_gehege` `pg` on(`p`.`pflegerID` = `pg`.`pflegerID`)) left join `gehege` `g` on(`pg`.`gehegeID` = `g`.`gID`)) GROUP BY `p`.`pflegerID` ORDER BY `p`.`Position` ASC, `p`.`Nachname` ASC ;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `arbeitszeiten`
--
ALTER TABLE `arbeitszeiten`
  ADD PRIMARY KEY (`arbeitszeitID`),
  ADD UNIQUE KEY `unique_pfleger_datum` (`pflegerID`,`Datum`),
  ADD KEY `fk_arbeitszeiten_pfleger` (`pflegerID`),
  ADD KEY `fk_arbeitszeiten_bestaetigt` (`Bestätigt_von`),
  ADD KEY `fk_arbeitszeiten_gehege` (`GehegeID`),
  ADD KEY `fk_arbeitszeiten_aufgabenbereich` (`AufgabenbereichID`),
  ADD KEY `idx_arbeitszeiten_datum` (`Datum`);

--
-- Indizes für die Tabelle `audit_log`
--
ALTER TABLE `audit_log`
  ADD PRIMARY KEY (`auditID`),
  ADD KEY `fk_audit_log_pfleger` (`PflegerID`),
  ADD KEY `idx_audit_log_zeitpunkt` (`Zeitpunkt`),
  ADD KEY `idx_audit_log_aktion` (`Aktion`),
  ADD KEY `idx_audit_log_tabelle` (`Tabelle`,`Datensatz_ID`),
  ADD KEY `idx_audit_log_composite` (`Zeitpunkt`,`PflegerID`,`Tabelle`),
  ADD KEY `idx_audit_log_recent` (`Zeitpunkt`);

--
-- Indizes für die Tabelle `aufgabenbereich`
--
ALTER TABLE `aufgabenbereich`
  ADD PRIMARY KEY (`aufgabenbereichID`),
  ADD UNIQUE KEY `Bezeichnung` (`Bezeichnung`),
  ADD KEY `idx_aufgabenbereich_kategorie` (`Kategorie`),
  ADD KEY `idx_aufgabenbereich_aktiv` (`Aktiv`);

--
-- Indizes für die Tabelle `bestellung`
--
ALTER TABLE `bestellung`
  ADD PRIMARY KEY (`bestellungID`),
  ADD UNIQUE KEY `Rechnungsnummer` (`Rechnungsnummer`),
  ADD KEY `idx_bestellung_status` (`Status`),
  ADD KEY `idx_bestellung_datum` (`Bestelldatum`),
  ADD KEY `idx_bestellung_lieferdatum` (`Lieferdatum`),
  ADD KEY `fk_bestellung_anbieter` (`anbieterID`),
  ADD KEY `idx_bestellung_zahlungsstatus` (`Zahlungsstatus`),
  ADD KEY `idx_bestellung_composite` (`Status`,`Bestelldatum`,`anbieterID`),
  ADD KEY `idx_bestellung_lieferdatum_status` (`Lieferdatum`,`Status`);

--
-- Indizes für die Tabelle `bestellung_position`
--
ALTER TABLE `bestellung_position`
  ADD PRIMARY KEY (`positionID`),
  ADD KEY `fk_bestellung_position_bestellung` (`bestellungID`),
  ADD KEY `fk_bestellung_position_futter` (`futterID`),
  ADD KEY `idx_bestellung_position_status` (`Status`);

--
-- Indizes für die Tabelle `besucher`
--
ALTER TABLE `besucher`
  ADD PRIMARY KEY (`besuchID`),
  ADD KEY `idx_besucher_datum` (`Datum`),
  ADD KEY `idx_besucher_anzahl` (`Anzahl_Besucher`);

--
-- Indizes für die Tabelle `fuetterungsprotokoll`
--
ALTER TABLE `fuetterungsprotokoll`
  ADD PRIMARY KEY (`protokollID`),
  ADD KEY `fk_fuetterungsprotokoll_futter` (`futterID`),
  ADD KEY `fk_fuetterungsprotokoll_tier` (`tierID`),
  ADD KEY `fk_fuetterungsprotokoll_pfleger` (`PflegerID`),
  ADD KEY `idx_fuetterungsprotokoll_zeit` (`Fuetterungszeit`),
  ADD KEY `idx_fuetterungsprotokoll_tier_datum` (`tierID`,`Fuetterungszeit`);

--
-- Indizes für die Tabelle `futter`
--
ALTER TABLE `futter`
  ADD PRIMARY KEY (`futterID`),
  ADD KEY `idx_futter_bezeichnung` (`Bezeichnung`),
  ADD KEY `idx_futter_lagerbestand` (`Lagerbestand`),
  ADD KEY `idx_futter_kategorie` (`Kategorie`),
  ADD KEY `idx_futter_status` (`Status`),
  ADD KEY `idx_futter_composite` (`Kategorie`,`Status`,`Lagerbestand`),
  ADD KEY `idx_futter_mhd` (`Best_Before`);

--
-- Indizes für die Tabelle `futter_anbieter`
--
ALTER TABLE `futter_anbieter`
  ADD PRIMARY KEY (`anbieterID`),
  ADD KEY `idx_anbieter_firmenname` (`Firmenname`),
  ADD KEY `idx_anbieter_bewertung` (`Bewertung`),
  ADD KEY `idx_anbieter_status` (`Status`);

--
-- Indizes für die Tabelle `futter_anbieter_preis`
--
ALTER TABLE `futter_anbieter_preis`
  ADD PRIMARY KEY (`preisID`),
  ADD KEY `fk_futter_anbieter_preis_futter` (`futterID`),
  ADD KEY `fk_futter_anbieter_preis_anbieter` (`anbieterID`),
  ADD KEY `idx_futter_anbieter_preis_gueltig` (`Gueltig_ab`,`Gueltig_bis`),
  ADD KEY `idx_futter_anbieter_preis_preis` (`Preis`);

--
-- Indizes für die Tabelle `futter_preisverlauf`
--
ALTER TABLE `futter_preisverlauf`
  ADD PRIMARY KEY (`verlaufID`),
  ADD KEY `fk_futter_preisverlauf_futter` (`futterID`),
  ADD KEY `idx_futter_preisverlauf_datum` (`Datum`),
  ADD KEY `idx_futter_preisverlauf_futter_datum` (`futterID`,`Datum`);

--
-- Indizes für die Tabelle `gehege`
--
ALTER TABLE `gehege`
  ADD PRIMARY KEY (`gID`),
  ADD KEY `fk_gehege_kontinent` (`kontinentID`),
  ADD KEY `idx_gbezeichnung` (`GBezeichnung`),
  ADD KEY `idx_gehege_status` (`Status`);

--
-- Indizes für die Tabelle `impfungen`
--
ALTER TABLE `impfungen`
  ADD PRIMARY KEY (`impfungID`),
  ADD KEY `fk_impfungen_tier` (`tierID`),
  ADD KEY `fk_impfungen_pfleger` (`PflegerID`),
  ADD KEY `idx_impfungen_datum` (`Datum`),
  ADD KEY `idx_impfungen_naechste` (`Naechste_Impfung`);

--
-- Indizes für die Tabelle `kontinent`
--
ALTER TABLE `kontinent`
  ADD PRIMARY KEY (`kID`),
  ADD KEY `idx_kbezeichnung` (`Kbezeichnung`);

--
-- Indizes für die Tabelle `logbuch`
--
ALTER TABLE `logbuch`
  ADD PRIMARY KEY (`logID`),
  ADD KEY `fk_logbuch_pfleger` (`PflegerID`),
  ADD KEY `fk_logbuch_tier` (`TierID`),
  ADD KEY `fk_logbuch_gehege` (`GehegeID`),
  ADD KEY `fk_logbuch_erledigt` (`Erledigt_von`),
  ADD KEY `idx_logbuch_datum_kategorie` (`Datum`,`Kategorie`),
  ADD KEY `idx_logbuch_erledigt` (`Erledigt`,`Erledigt_am`);

--
-- Indizes für die Tabelle `login_sessions`
--
ALTER TABLE `login_sessions`
  ADD PRIMARY KEY (`sessionID`),
  ADD KEY `fk_login_sessions_pfleger` (`pflegerID`),
  ADD KEY `idx_login_sessions_gueltig` (`Gueltig_bis`),
  ADD KEY `idx_login_sessions_pfleger_aktiv` (`pflegerID`,`Gueltig_bis`);

--
-- Indizes für die Tabelle `medikamente`
--
ALTER TABLE `medikamente`
  ADD PRIMARY KEY (`medikamentID`),
  ADD KEY `idx_medikamente_bezeichnung` (`Bezeichnung`),
  ADD KEY `idx_medikamente_mhd` (`MHD`),
  ADD KEY `idx_medikamente_lagerbestand` (`Lagerbestand`);

--
-- Indizes für die Tabelle `medikamenten_gabe`
--
ALTER TABLE `medikamenten_gabe`
  ADD PRIMARY KEY (`gabeID`),
  ADD KEY `fk_medikamenten_gabe_tier` (`tierID`),
  ADD KEY `fk_medikamenten_gabe_medikament` (`medikamentID`),
  ADD KEY `fk_medikamenten_gabe_pfleger` (`PflegerID`),
  ADD KEY `idx_medikamenten_gabe_datum` (`Datum`);

--
-- Indizes für die Tabelle `pfleger`
--
ALTER TABLE `pfleger`
  ADD PRIMARY KEY (`pflegerID`),
  ADD UNIQUE KEY `Personalnummer` (`Personalnummer`),
  ADD UNIQUE KEY `Benutzername` (`Benutzername`),
  ADD KEY `fk_pfleger_hauptpfleger` (`HauptpflegerID`),
  ADD KEY `idx_pfleger_aktiv` (`Aktiv`),
  ADD KEY `idx_pfleger_rolle` (`Rolle`),
  ADD KEY `idx_pfleger_benutzername` (`Benutzername`),
  ADD KEY `idx_pfleger_position` (`Position`),
  ADD KEY `idx_pfleger_account_status` (`Account_aktiv`,`Account_gesperrt_bis`),
  ADD KEY `idx_pfleger_composite` (`Rolle`,`Aktiv`,`Account_aktiv`),
  ADD KEY `idx_pfleger_login` (`Benutzername`,`Passwort`,`Account_aktiv`);

--
-- Indizes für die Tabelle `pfleger_aufgabenbereich`
--
ALTER TABLE `pfleger_aufgabenbereich`
  ADD PRIMARY KEY (`zuordnungID`),
  ADD UNIQUE KEY `unique_pfleger_aufgabe_aktiv` (`pflegerID`,`aufgabenbereichID`),
  ADD KEY `fk_pfleger_aufgabenbereich_pfleger` (`pflegerID`),
  ADD KEY `fk_pfleger_aufgabenbereich_aufgabe` (`aufgabenbereichID`),
  ADD KEY `idx_pfleger_aufgabenbereich_gueltig` (`ZugeordnetAm`,`Bis_Datum`);

--
-- Indizes für die Tabelle `pfleger_gehege`
--
ALTER TABLE `pfleger_gehege`
  ADD PRIMARY KEY (`zuordnungID`),
  ADD UNIQUE KEY `unique_pfleger_gehege_aktiv` (`pflegerID`,`gehegeID`),
  ADD KEY `fk_pfleger_gehege_pfleger` (`pflegerID`),
  ADD KEY `fk_pfleger_gehege_gehege` (`gehegeID`),
  ADD KEY `idx_pfleger_gehege_gueltig` (`ZugeordnetAm`,`Bis_Datum`);

--
-- Indizes für die Tabelle `pfleger_tier`
--
ALTER TABLE `pfleger_tier`
  ADD PRIMARY KEY (`zuordnungID`),
  ADD UNIQUE KEY `unique_pfleger_tier_aktiv` (`pflegerID`,`tierID`),
  ADD KEY `fk_pfleger_tier_pfleger` (`pflegerID`),
  ADD KEY `fk_pfleger_tier_tier` (`tierID`),
  ADD KEY `idx_pfleger_tier_gueltig` (`ZugeordnetAm`,`Bis_Datum`);

--
-- Indizes für die Tabelle `tierart`
--
ALTER TABLE `tierart`
  ADD PRIMARY KEY (`tierartID`),
  ADD KEY `idx_tierart_bezeichnung` (`TABezeichnung`),
  ADD KEY `idx_gefaerdungsstatus` (`Gefaehrdungsstatus`);

--
-- Indizes für die Tabelle `tierart_futter`
--
ALTER TABLE `tierart_futter`
  ADD PRIMARY KEY (`tierart_futterID`),
  ADD KEY `fk_tierart_futter_futter` (`futterID`),
  ADD KEY `fk_tierart_futter_tierart` (`tierartID`),
  ADD KEY `idx_tierart_futter_zeit` (`Fuetterungszeit`),
  ADD KEY `idx_tierart_futter_gueltig` (`Gueltig_ab`,`Gueltig_bis`);

--
-- Indizes für die Tabelle `tierarzt_besuche`
--
ALTER TABLE `tierarzt_besuche`
  ADD PRIMARY KEY (`besuchID`),
  ADD KEY `fk_tierarzt_besuche_tier` (`tierID`),
  ADD KEY `fk_tierarzt_besuche_pfleger` (`PflegerID`),
  ADD KEY `idx_tierarzt_besuche_datum` (`Datum`),
  ADD KEY `idx_tierarzt_besuche_tier_datum` (`tierID`,`Datum`);

--
-- Indizes für die Tabelle `tiere`
--
ALTER TABLE `tiere`
  ADD PRIMARY KEY (`tierID`),
  ADD UNIQUE KEY `Mikrochip_Nr` (`Mikrochip_Nr`),
  ADD KEY `fk_tiere_gehege` (`GehegeID`),
  ADD KEY `fk_tiere_tierart` (`TierartID`),
  ADD KEY `idx_tiere_name` (`Name`),
  ADD KEY `idx_tiere_geburtsdatum` (`Geburtsdatum`),
  ADD KEY `idx_tiere_gesundheit` (`Gesundheitszustand`),
  ADD KEY `idx_tiere_status` (`Status`),
  ADD KEY `fk_tiere_eltern` (`Eltern_tierID`),
  ADD KEY `fk_tiere_geschwister` (`Geschwister_tierID`),
  ADD KEY `idx_tiere_composite` (`TierartID`,`GehegeID`,`Status`),
  ADD KEY `idx_tiere_geburtsdatum_status` (`Geburtsdatum`,`Status`),
  ADD KEY `idx_tiere_gesundheit_status` (`Gesundheitszustand`,`Status`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `arbeitszeiten`
--
ALTER TABLE `arbeitszeiten`
  MODIFY `arbeitszeitID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `audit_log`
--
ALTER TABLE `audit_log`
  MODIFY `auditID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT für Tabelle `aufgabenbereich`
--
ALTER TABLE `aufgabenbereich`
  MODIFY `aufgabenbereichID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `bestellung`
--
ALTER TABLE `bestellung`
  MODIFY `bestellungID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `bestellung_position`
--
ALTER TABLE `bestellung_position`
  MODIFY `positionID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `besucher`
--
ALTER TABLE `besucher`
  MODIFY `besuchID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `fuetterungsprotokoll`
--
ALTER TABLE `fuetterungsprotokoll`
  MODIFY `protokollID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `futter`
--
ALTER TABLE `futter`
  MODIFY `futterID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=66;

--
-- AUTO_INCREMENT für Tabelle `futter_anbieter`
--
ALTER TABLE `futter_anbieter`
  MODIFY `anbieterID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `futter_anbieter_preis`
--
ALTER TABLE `futter_anbieter_preis`
  MODIFY `preisID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `futter_preisverlauf`
--
ALTER TABLE `futter_preisverlauf`
  MODIFY `verlaufID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `gehege`
--
ALTER TABLE `gehege`
  MODIFY `gID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT für Tabelle `impfungen`
--
ALTER TABLE `impfungen`
  MODIFY `impfungID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `kontinent`
--
ALTER TABLE `kontinent`
  MODIFY `kID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT für Tabelle `logbuch`
--
ALTER TABLE `logbuch`
  MODIFY `logID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `medikamente`
--
ALTER TABLE `medikamente`
  MODIFY `medikamentID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `medikamenten_gabe`
--
ALTER TABLE `medikamenten_gabe`
  MODIFY `gabeID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `pfleger`
--
ALTER TABLE `pfleger`
  MODIFY `pflegerID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT für Tabelle `pfleger_aufgabenbereich`
--
ALTER TABLE `pfleger_aufgabenbereich`
  MODIFY `zuordnungID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `pfleger_gehege`
--
ALTER TABLE `pfleger_gehege`
  MODIFY `zuordnungID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `pfleger_tier`
--
ALTER TABLE `pfleger_tier`
  MODIFY `zuordnungID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `tierart`
--
ALTER TABLE `tierart`
  MODIFY `tierartID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `tierart_futter`
--
ALTER TABLE `tierart_futter`
  MODIFY `tierart_futterID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `tierarzt_besuche`
--
ALTER TABLE `tierarzt_besuche`
  MODIFY `besuchID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `tiere`
--
ALTER TABLE `tiere`
  MODIFY `tierID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `arbeitszeiten`
--
ALTER TABLE `arbeitszeiten`
  ADD CONSTRAINT `fk_arbeitszeiten_aufgabenbereich` FOREIGN KEY (`AufgabenbereichID`) REFERENCES `aufgabenbereich` (`aufgabenbereichID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_arbeitszeiten_bestaetigt` FOREIGN KEY (`Bestätigt_von`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_arbeitszeiten_gehege` FOREIGN KEY (`GehegeID`) REFERENCES `gehege` (`gID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_arbeitszeiten_pfleger` FOREIGN KEY (`pflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `audit_log`
--
ALTER TABLE `audit_log`
  ADD CONSTRAINT `fk_audit_log_pfleger` FOREIGN KEY (`PflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL;

--
-- Constraints der Tabelle `bestellung`
--
ALTER TABLE `bestellung`
  ADD CONSTRAINT `fk_bestellung_anbieter` FOREIGN KEY (`anbieterID`) REFERENCES `futter_anbieter` (`anbieterID`) ON DELETE SET NULL;

--
-- Constraints der Tabelle `bestellung_position`
--
ALTER TABLE `bestellung_position`
  ADD CONSTRAINT `fk_bestellung_position_bestellung` FOREIGN KEY (`bestellungID`) REFERENCES `bestellung` (`bestellungID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_bestellung_position_futter` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `fuetterungsprotokoll`
--
ALTER TABLE `fuetterungsprotokoll`
  ADD CONSTRAINT `fk_fuetterungsprotokoll_futter` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_fuetterungsprotokoll_pfleger` FOREIGN KEY (`PflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_fuetterungsprotokoll_tier` FOREIGN KEY (`tierID`) REFERENCES `tiere` (`tierID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `futter_anbieter_preis`
--
ALTER TABLE `futter_anbieter_preis`
  ADD CONSTRAINT `fk_futter_anbieter_preis_anbieter` FOREIGN KEY (`anbieterID`) REFERENCES `futter_anbieter` (`anbieterID`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_futter_anbieter_preis_futter` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `futter_preisverlauf`
--
ALTER TABLE `futter_preisverlauf`
  ADD CONSTRAINT `fk_futter_preisverlauf_futter` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `gehege`
--
ALTER TABLE `gehege`
  ADD CONSTRAINT `fk_gehege_kontinent` FOREIGN KEY (`kontinentID`) REFERENCES `kontinent` (`kID`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `impfungen`
--
ALTER TABLE `impfungen`
  ADD CONSTRAINT `fk_impfungen_pfleger` FOREIGN KEY (`PflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_impfungen_tier` FOREIGN KEY (`tierID`) REFERENCES `tiere` (`tierID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `logbuch`
--
ALTER TABLE `logbuch`
  ADD CONSTRAINT `fk_logbuch_erledigt` FOREIGN KEY (`Erledigt_von`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_logbuch_gehege` FOREIGN KEY (`GehegeID`) REFERENCES `gehege` (`gID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_logbuch_pfleger` FOREIGN KEY (`PflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_logbuch_tier` FOREIGN KEY (`TierID`) REFERENCES `tiere` (`tierID`) ON DELETE SET NULL;

--
-- Constraints der Tabelle `login_sessions`
--
ALTER TABLE `login_sessions`
  ADD CONSTRAINT `fk_login_sessions_pfleger` FOREIGN KEY (`pflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `medikamenten_gabe`
--
ALTER TABLE `medikamenten_gabe`
  ADD CONSTRAINT `fk_medikamenten_gabe_medikament` FOREIGN KEY (`medikamentID`) REFERENCES `medikamente` (`medikamentID`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_medikamenten_gabe_pfleger` FOREIGN KEY (`PflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_medikamenten_gabe_tier` FOREIGN KEY (`tierID`) REFERENCES `tiere` (`tierID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `pfleger`
--
ALTER TABLE `pfleger`
  ADD CONSTRAINT `fk_pfleger_hauptpfleger` FOREIGN KEY (`HauptpflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL;

--
-- Constraints der Tabelle `pfleger_aufgabenbereich`
--
ALTER TABLE `pfleger_aufgabenbereich`
  ADD CONSTRAINT `fk_pfleger_aufgabenbereich_aufgabe` FOREIGN KEY (`aufgabenbereichID`) REFERENCES `aufgabenbereich` (`aufgabenbereichID`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_pfleger_aufgabenbereich_pfleger` FOREIGN KEY (`pflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `pfleger_gehege`
--
ALTER TABLE `pfleger_gehege`
  ADD CONSTRAINT `fk_pfleger_gehege_gehege` FOREIGN KEY (`gehegeID`) REFERENCES `gehege` (`gID`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_pfleger_gehege_pfleger` FOREIGN KEY (`pflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `pfleger_tier`
--
ALTER TABLE `pfleger_tier`
  ADD CONSTRAINT `fk_pfleger_tier_pfleger` FOREIGN KEY (`pflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_pfleger_tier_tier` FOREIGN KEY (`tierID`) REFERENCES `tiere` (`tierID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `tierart_futter`
--
ALTER TABLE `tierart_futter`
  ADD CONSTRAINT `fk_tierart_futter_futter` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON DELETE CASCADE,
  ADD CONSTRAINT `fk_tierart_futter_tierart` FOREIGN KEY (`tierartID`) REFERENCES `tierart` (`tierartID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `tierarzt_besuche`
--
ALTER TABLE `tierarzt_besuche`
  ADD CONSTRAINT `fk_tierarzt_besuche_pfleger` FOREIGN KEY (`PflegerID`) REFERENCES `pfleger` (`pflegerID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_tierarzt_besuche_tier` FOREIGN KEY (`tierID`) REFERENCES `tiere` (`tierID`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `tiere`
--
ALTER TABLE `tiere`
  ADD CONSTRAINT `fk_tiere_eltern` FOREIGN KEY (`Eltern_tierID`) REFERENCES `tiere` (`tierID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_tiere_gehege` FOREIGN KEY (`GehegeID`) REFERENCES `gehege` (`gID`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_tiere_geschwister` FOREIGN KEY (`Geschwister_tierID`) REFERENCES `tiere` (`tierID`) ON DELETE SET NULL,
  ADD CONSTRAINT `fk_tiere_tierart` FOREIGN KEY (`TierartID`) REFERENCES `tierart` (`tierartID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
