-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 12. Dez 2025 um 15:25
-- Server-Version: 10.4.32-MariaDB
-- PHP-Version: 8.2.12

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
-- Tabellenstruktur für Tabelle `bestellung`
--

CREATE TABLE `bestellung` (
  `bestellungID` int(11) NOT NULL,
  `Bestelldatum` date NOT NULL,
  `Lieferdatum` date DEFAULT NULL,
  `Status` enum('offen','bestellt','geliefert','storniert') DEFAULT 'offen',
  `Gesamtpreis` decimal(10,2) NOT NULL DEFAULT 0.00,
  `Lieferant` varchar(100) DEFAULT NULL,
  `Notizen` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `bestellung`
--

INSERT INTO `bestellung` (`bestellungID`, `Bestelldatum`, `Lieferdatum`, `Status`, `Gesamtpreis`, `Lieferant`, `Notizen`) VALUES
(1, '2024-01-15', '2024-01-20', 'geliefert', 1250.00, 'FutterPro GmbH', 'Regulaere Lieferung'),
(2, '2024-02-10', NULL, 'bestellt', 800.00, 'TierNahrung AG', 'Nachbestellung'),
(3, '2025-12-11', NULL, 'offen', 0.00, 'BioFutter GmbH', 'Woechentliche Bestellung');

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
  `Gesamtpreis` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `bestellung_position`
--

INSERT INTO `bestellung_position` (`positionID`, `bestellungID`, `futterID`, `Menge`, `Einzelpreis`, `Gesamtpreis`) VALUES
(1, 1, 1, 100.00, 8.00, 800.00),
(2, 1, 5, 50.00, 3.50, 175.00),
(3, 1, 6, 100.00, 2.50, 250.00),
(4, 2, 2, 150.00, 2.00, 300.00),
(5, 2, 3, 100.00, 5.00, 500.00),
(6, 3, 1, 50.00, 8.50, 425.00),
(7, 3, 4, 30.00, 12.00, 360.00);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fuetterungsplan`
--

CREATE TABLE `fuetterungsplan` (
  `planID` int(11) NOT NULL,
  `tierartID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `Menge_pro_Tag` decimal(10,2) NOT NULL,
  `Fuetterungszeit` time DEFAULT NULL,
  `Notizen` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `fuetterungsplan`
--

INSERT INTO `fuetterungsplan` (`planID`, `tierartID`, `futterID`, `Menge_pro_Tag`, `Fuetterungszeit`, `Notizen`) VALUES
(1, 1, 1, 8.00, '08:00:00', 'Fleisch morgens'),
(2, 1, 5, 2.00, '16:00:00', 'Obst nachmittags'),
(3, 2, 2, 40.00, '07:00:00', 'Heu frueh'),
(4, 2, 5, 10.00, '12:00:00', 'Obst mittags'),
(5, 2, 6, 15.00, '17:00:00', 'Gemuese abends'),
(6, 3, 2, 25.00, '08:30:00', 'Heu morgens'),
(7, 3, 6, 8.00, '15:00:00', 'Gemuese nachmittags'),
(8, 4, 1, 6.00, '09:00:00', 'Fleisch morgens'),
(9, 4, 4, 4.00, '17:30:00', 'Fisch abends'),
(10, 5, 3, 15.00, '10:00:00', 'Bambus ganztags'),
(11, 5, 5, 3.00, '14:00:00', 'Obst mittags'),
(12, 1, 1, 8.00, '08:00:00', 'Test-Eintrag - Tierart 1 existiert'),
(13, 10, 1, 2.00, NULL, NULL);

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
  `Bemerkungen` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `fuetterungsprotokoll`
--

INSERT INTO `fuetterungsprotokoll` (`protokollID`, `tierID`, `futterID`, `Menge`, `Fuetterungszeit`, `Pfleger_Name`, `Bemerkungen`) VALUES
(1, 1, 1, 8.50, '2025-12-11 07:28:46', 'Hans Mueller', 'Hat gut gefressen'),
(2, 2, 2, 42.00, '2025-12-11 05:28:46', 'Anna Schmidt', 'Normaler Appetit'),
(3, 3, 2, 26.00, '2025-12-11 06:28:46', 'Peter Weber', 'Alles gut'),
(4, 4, 1, 6.20, '2025-12-11 08:28:46', 'Lisa Fischer', 'Sehr hungrig'),
(5, 5, 3, 16.00, '2025-12-11 04:28:46', 'Thomas Klein', 'Hat Bambus bevorzugt');

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
  `Kategorie` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `futter`
--

INSERT INTO `futter` (`futterID`, `Bezeichnung`, `Einheit`, `Preis_pro_Einheit`, `Lagerbestand`, `Mindestbestand`, `Bestellmenge`, `Kategorie`) VALUES
(1, 'Rindfleisch', 'kg', 8.50, 150.00, 100.00, 200.00, 'Fleisch'),
(2, 'Heu', 'kg', 2.00, 50.00, 100.00, 150.00, 'Pflanzen'),
(3, 'Bambus', 'kg', 5.00, 20.00, 50.00, 100.00, 'Pflanzen'),
(4, 'Lachs', 'kg', 12.00, 80.00, 80.00, 150.00, 'Fisch'),
(5, 'Obst-Mix', 'kg', 3.50, 180.00, 120.00, 200.00, 'Obst'),
(6, 'Gemuese-Mix', 'kg', 2.50, 200.00, 100.00, 150.00, 'Gemuese'),
(7, 'Insekten', 'kg', 15.00, 70.00, 50.00, 80.00, 'Insekten'),
(8, 'Gras', 'kg', 1.50, 400.00, 150.00, 250.00, 'Pflanzen'),
(9, 'Koerner-Mix', 'kg', 4.00, 150.00, 80.00, 150.00, 'Koerner'),
(10, 'Spezial-Pellets', 'kg', 6.00, 220.00, 100.00, 200.00, 'Fertigfutter');

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
  `Flaeche` decimal(10,2) DEFAULT NULL,
  `Kapazitaet` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `gehege`
--

INSERT INTO `gehege` (`gID`, `GBezeichnung`, `kontinentID`, `Flaeche_m2`, `Baujahr`, `Flaeche`, `Kapazitaet`) VALUES
(1, 'Savanne Afrika', 1, 5000, '2010', NULL, NULL),
(2, 'Tropenwald Asien', 2, 3000, '2015', NULL, NULL),
(3, 'Alpen Europa', 3, 2000, '2008', NULL, NULL),
(4, 'Praerie Nordamerika', 4, 4000, '2012', NULL, NULL),
(5, 'Amazonas Suedamerika', 5, 3500, '2018', NULL, NULL),
(6, 'Outback Australien', 6, 4500, '2016', NULL, NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `kontinent`
--

CREATE TABLE `kontinent` (
  `kID` int(11) NOT NULL,
  `Kbezeichnung` varchar(100) NOT NULL,
  `Klimazone` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `kontinent`
--

INSERT INTO `kontinent` (`kID`, `Kbezeichnung`, `Klimazone`) VALUES
(1, 'Afrika', NULL),
(2, 'Asien', NULL),
(3, 'Europa', NULL),
(4, 'Nordamerika', NULL),
(5, 'Suedamerika', NULL),
(6, 'Australien', NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tierart`
--

CREATE TABLE `tierart` (
  `tierartID` int(11) NOT NULL,
  `TABezeichnung` varchar(100) NOT NULL,
  `Wissenschaftlicher_Name` varchar(100) DEFAULT NULL,
  `Gefaehrdungsstatus` varchar(50) DEFAULT 'Nicht gefährdet',
  `Lebensraum` varchar(200) DEFAULT NULL,
  `Ernaehrung` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `tierart`
--

INSERT INTO `tierart` (`tierartID`, `TABezeichnung`, `Wissenschaftlicher_Name`, `Gefaehrdungsstatus`, `Lebensraum`, `Ernaehrung`) VALUES
(1, 'Loewe', 'Panthera leo', 'Gefaehrdet', NULL, NULL),
(2, 'Elefant', 'Loxodonta africana', 'Stark gefaehrdet', NULL, NULL),
(3, 'Giraffe', 'Giraffa camelopardalis', 'Verletzlich', NULL, NULL),
(4, 'Tiger', 'Panthera tigris', 'Stark gefaehrdet', NULL, NULL),
(5, 'Panda', 'Ailuropoda melanoleuca', 'Gefaehrdet', NULL, NULL),
(6, 'Braunbär', 'Ursus arctos', 'Nicht gefaehrdet', NULL, NULL),
(7, 'Bison', 'Bison bison', 'Nicht gefaehrdet', NULL, NULL),
(8, 'Jaguar', 'Panthera onca', 'Gefaehrdet', NULL, NULL),
(9, 'Kaenguru', 'Macropus rufus', 'Nicht gefaehrdet', NULL, NULL),
(10, 'Koala', 'Phascolarctos cinereus', 'Gefaehrdet', NULL, NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tierart_futter`
--

CREATE TABLE `tierart_futter` (
  `tierart_futterID` int(11) NOT NULL,
  `tierartID` int(11) NOT NULL,
  `futterID` int(11) NOT NULL,
  `Menge_pro_Tag` decimal(10,2) NOT NULL,
  `Fuetterungszeit` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `tierart_futter`
--

INSERT INTO `tierart_futter` (`tierart_futterID`, `tierartID`, `futterID`, `Menge_pro_Tag`, `Fuetterungszeit`) VALUES
(1, 1, 1, 10.50, '08:00:00'),
(2, 1, 2, 5.00, '14:00:00'),
(3, 2, 1, 8.00, '09:00:00'),
(4, 3, 3, 2.50, '12:00:00');

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
  `Geschlecht` enum('Maennlich','Weiblich','Unbekannt') DEFAULT 'Unbekannt',
  `Gesundheitsstatus` varchar(50) DEFAULT 'Gesund',
  `TierartID` int(11) NOT NULL,
  `GehegeID` int(11) NOT NULL,
  `Groesse` decimal(5,2) DEFAULT NULL,
  `Gesundheitszustand` varchar(100) DEFAULT 'Gut',
  `Fuetterungszeiten` varchar(200) DEFAULT '08:00, 12:00, 18:00',
  `Besonderheiten` text DEFAULT NULL,
  `Ankunft_im_Zoo` date DEFAULT NULL,
  `Betreuender_Tierarzt` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Daten für Tabelle `tiere`
--

INSERT INTO `tiere` (`tierID`, `Name`, `Spitzname`, `Gewicht`, `Geburtsdatum`, `Bildpfad`, `Notizen`, `Geschlecht`, `Gesundheitsstatus`, `TierartID`, `GehegeID`, `Groesse`, `Gesundheitszustand`, `Fuetterungszeiten`, `Besonderheiten`, `Ankunft_im_Zoo`, `Betreuender_Tierarzt`) VALUES
(1, 'Simba', 'Koenig', 190.50, '2018-03-15', NULL, NULL, 'Maennlich', 'Gesund', 1, 1, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(2, 'Nala', 'Koenigin', 175.30, '2019-06-22', NULL, NULL, 'Weiblich', 'Gesund', 1, 1, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(3, 'Dumbo', 'Flapper', 5400.00, '2015-01-10', NULL, NULL, 'Maennlich', 'Gesund', 2, 1, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(4, 'Melman', 'Langhals', 1200.00, '2017-08-05', NULL, NULL, 'Maennlich', 'Gesund', 3, 1, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(5, 'Shir Khan', 'Herrscher', 220.00, '2016-11-30', NULL, NULL, 'Maennlich', 'Gesund', 4, 2, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(6, 'Bao Bao', 'Bambusliebhaber', 110.50, '2020-02-14', NULL, NULL, 'Weiblich', 'Gesund', 5, 2, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(7, 'Baloo', 'Brummbaer', 280.00, '2018-07-20', 'Images\\tier_7_balu.png', NULL, 'Maennlich', 'Gesund', 6, 3, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(8, 'Thunder', 'Donner', 900.00, '2019-04-12', NULL, NULL, 'Maennlich', 'Gesund', 7, 4, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(9, 'Diego', 'Jaeger', 95.00, '2017-09-18', NULL, NULL, 'Maennlich', 'Gesund', 8, 5, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(10, 'Skippy', 'Springer', 35.00, '2021-05-25', NULL, NULL, 'Weiblich', 'Gesund', 9, 6, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL),
(11, 'Kimi', 'Schlaefer', 12.50, '2020-10-08', NULL, NULL, 'Weiblich', 'Gesund', 10, 6, NULL, 'Gut', '08:00, 12:00, 18:00', NULL, NULL, NULL);

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
,`Geschlecht` enum('Maennlich','Weiblich','Unbekannt')
,`Gesundheitsstatus` varchar(50)
,`Tierart` varchar(100)
,`Gehege` varchar(100)
,`Kontinent` varchar(100)
,`Anzeige_Name` varchar(203)
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

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_taeglicher_futterbedarf`  AS SELECT `t`.`tierID` AS `tierID`, `t`.`Name` AS `Tiername`, `ta`.`TABezeichnung` AS `Tierart`, `g`.`GBezeichnung` AS `Gehege`, `f`.`Bezeichnung` AS `Futtersorte`, `fp`.`Menge_pro_Tag` AS `Menge_pro_Tag`, `f`.`Einheit` AS `Einheit`, `fp`.`Fuetterungszeit` AS `Fuetterungszeit` FROM ((((`tiere` `t` join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) join `gehege` `g` on(`t`.`GehegeID` = `g`.`gID`)) join `fuetterungsplan` `fp` on(`ta`.`tierartID` = `fp`.`tierartID`)) join `futter` `f` on(`fp`.`futterID` = `f`.`futterID`)) WHERE `fp`.`Fuetterungszeit` is not null ORDER BY `fp`.`Fuetterungszeit` ASC, `t`.`Name` ASC ;

-- --------------------------------------------------------

--
-- Struktur des Views `view_tiere_uebersicht`
--
DROP TABLE IF EXISTS `view_tiere_uebersicht`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_tiere_uebersicht`  AS SELECT `t`.`tierID` AS `tierID`, `t`.`Name` AS `Name`, `t`.`Spitzname` AS `Spitzname`, `t`.`Gewicht` AS `Gewicht`, `t`.`Geburtsdatum` AS `Geburtsdatum`, timestampdiff(YEAR,`t`.`Geburtsdatum`,curdate()) AS `Alter_Jahre`, `t`.`Geschlecht` AS `Geschlecht`, `t`.`Gesundheitsstatus` AS `Gesundheitsstatus`, `ta`.`TABezeichnung` AS `Tierart`, `g`.`GBezeichnung` AS `Gehege`, `k`.`Kbezeichnung` AS `Kontinent`, concat(`t`.`Name`,' (',`ta`.`TABezeichnung`,')') AS `Anzeige_Name` FROM (((`tiere` `t` left join `tierart` `ta` on(`t`.`TierartID` = `ta`.`tierartID`)) left join `gehege` `g` on(`t`.`GehegeID` = `g`.`gID`)) left join `kontinent` `k` on(`g`.`kontinentID` = `k`.`kID`)) ORDER BY `t`.`Name` ASC ;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `bestellung`
--
ALTER TABLE `bestellung`
  ADD PRIMARY KEY (`bestellungID`),
  ADD KEY `idx_status` (`Status`),
  ADD KEY `idx_datum` (`Bestelldatum`);

--
-- Indizes für die Tabelle `bestellung_position`
--
ALTER TABLE `bestellung_position`
  ADD PRIMARY KEY (`positionID`),
  ADD KEY `bestellungID` (`bestellungID`),
  ADD KEY `futterID` (`futterID`);

--
-- Indizes für die Tabelle `fuetterungsplan`
--
ALTER TABLE `fuetterungsplan`
  ADD PRIMARY KEY (`planID`),
  ADD KEY `futterID` (`futterID`),
  ADD KEY `idx_tierart` (`tierartID`),
  ADD KEY `idx_zeit` (`Fuetterungszeit`);

--
-- Indizes für die Tabelle `fuetterungsprotokoll`
--
ALTER TABLE `fuetterungsprotokoll`
  ADD PRIMARY KEY (`protokollID`),
  ADD KEY `futterID` (`futterID`),
  ADD KEY `idx_fuetterungszeit` (`Fuetterungszeit`),
  ADD KEY `idx_tier` (`tierID`);

--
-- Indizes für die Tabelle `futter`
--
ALTER TABLE `futter`
  ADD PRIMARY KEY (`futterID`),
  ADD KEY `idx_bezeichnung` (`Bezeichnung`),
  ADD KEY `idx_lagerbestand` (`Lagerbestand`);

--
-- Indizes für die Tabelle `gehege`
--
ALTER TABLE `gehege`
  ADD PRIMARY KEY (`gID`),
  ADD KEY `kontinentID` (`kontinentID`),
  ADD KEY `idx_gbezeichnung` (`GBezeichnung`);

--
-- Indizes für die Tabelle `kontinent`
--
ALTER TABLE `kontinent`
  ADD PRIMARY KEY (`kID`),
  ADD KEY `idx_kbezeichnung` (`Kbezeichnung`);

--
-- Indizes für die Tabelle `tierart`
--
ALTER TABLE `tierart`
  ADD PRIMARY KEY (`tierartID`);

--
-- Indizes für die Tabelle `tierart_futter`
--
ALTER TABLE `tierart_futter`
  ADD PRIMARY KEY (`tierart_futterID`),
  ADD KEY `futterID` (`futterID`),
  ADD KEY `tierartID` (`tierartID`);

--
-- Indizes für die Tabelle `tiere`
--
ALTER TABLE `tiere`
  ADD PRIMARY KEY (`tierID`),
  ADD KEY `GehegeID` (`GehegeID`),
  ADD KEY `idx_name` (`Name`),
  ADD KEY `idx_geburtsdatum` (`Geburtsdatum`),
  ADD KEY `TierartID` (`TierartID`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `bestellung`
--
ALTER TABLE `bestellung`
  MODIFY `bestellungID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT für Tabelle `bestellung_position`
--
ALTER TABLE `bestellung_position`
  MODIFY `positionID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT für Tabelle `fuetterungsplan`
--
ALTER TABLE `fuetterungsplan`
  MODIFY `planID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT für Tabelle `fuetterungsprotokoll`
--
ALTER TABLE `fuetterungsprotokoll`
  MODIFY `protokollID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT für Tabelle `futter`
--
ALTER TABLE `futter`
  MODIFY `futterID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT für Tabelle `gehege`
--
ALTER TABLE `gehege`
  MODIFY `gID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT für Tabelle `kontinent`
--
ALTER TABLE `kontinent`
  MODIFY `kID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT für Tabelle `tierart`
--
ALTER TABLE `tierart`
  MODIFY `tierartID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT für Tabelle `tierart_futter`
--
ALTER TABLE `tierart_futter`
  MODIFY `tierart_futterID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `tiere`
--
ALTER TABLE `tiere`
  MODIFY `tierID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `bestellung_position`
--
ALTER TABLE `bestellung_position`
  ADD CONSTRAINT `bestellung_position_ibfk_1` FOREIGN KEY (`bestellungID`) REFERENCES `bestellung` (`bestellungID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `bestellung_position_ibfk_2` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `fuetterungsplan`
--
ALTER TABLE `fuetterungsplan`
  ADD CONSTRAINT `fk_fuetterungsplan_tierart` FOREIGN KEY (`tierartID`) REFERENCES `tierart` (`tierartID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fuetterungsplan_ibfk_2` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `fuetterungsprotokoll`
--
ALTER TABLE `fuetterungsprotokoll`
  ADD CONSTRAINT `fuetterungsprotokoll_ibfk_1` FOREIGN KEY (`tierID`) REFERENCES `tiere` (`tierID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fuetterungsprotokoll_ibfk_2` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `gehege`
--
ALTER TABLE `gehege`
  ADD CONSTRAINT `gehege_ibfk_1` FOREIGN KEY (`kontinentID`) REFERENCES `kontinent` (`kID`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `tierart_futter`
--
ALTER TABLE `tierart_futter`
  ADD CONSTRAINT `tierart_futter_ibfk_2` FOREIGN KEY (`futterID`) REFERENCES `futter` (`futterID`),
  ADD CONSTRAINT `tierart_futter_ibfk_3` FOREIGN KEY (`tierartID`) REFERENCES `tierart` (`tierartID`);

--
-- Constraints der Tabelle `tiere`
--
ALTER TABLE `tiere`
  ADD CONSTRAINT `tiere_ibfk_2` FOREIGN KEY (`GehegeID`) REFERENCES `gehege` (`gID`) ON UPDATE CASCADE,
  ADD CONSTRAINT `tiere_ibfk_3` FOREIGN KEY (`TierartID`) REFERENCES `tierart` (`tierartID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
