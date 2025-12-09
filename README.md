🐾 Zoo Verwaltungssoftware

Modernes CRUD-Verwaltungssystem für einen Zoo (C# WinForms + MySQL)








📚 Inhalt

Funktionen

Datenbankstruktur

Voraussetzungen

Installation

Bedienung

Architektur

Einschränkungen

Erweiterungen

Entwickler

📌 Funktionen
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>

Die App unterstützt vollständiges CRUD:

🌍 Kontinente

🏠 Gehege

🐾 Tierarten

🦁 Tiere

📊 Übersicht (mit Inline-Editing)

</details>
🗄️ Datenbankstruktur
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>
Tabelle: Kontinent
kID (PK)
Kbezeichnung

Tabelle: Gehege
gID (PK)
GBezeichnung
kontinentID (FK → Kontinent)

Tabelle: Tierart
tierartID (PK)
TABezeichnung

Tabelle: Tiere
tierID (PK)
Name
Gewicht
Geburtsdatum
TierartID (FK)
GehegeID (FK)

</details>
🧰 Voraussetzungen
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>

Windows

.NET 9 (oder neuer)

XAMPP / MySQL

NuGet-Paket: MySql.Data

</details>
🔧 Installation
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>
1️⃣ Datenbank importieren

XAMPP starten

phpMyAdmin öffnen

database_setup.sql importieren

2️⃣ Verbindungsdaten anpassen (falls nötig)

In DB.cs:

private readonly string connStr =
    "server=localhost;port=3306;database=zoo_verwaltung;uid=root;pwd=;";

3️⃣ Projekt starten

Lösung in Visual Studio laden

F5 drücken

</details>
🖥️ Bedienung
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>
Jeder Tab besitzt:
Button	Funktion
Neu	Felder leeren
Speichern	Eintrag anlegen/aktualisieren
Löschen	Eintrag entfernen
ListBox	Auswahl eines Datensatzes
Übersicht

Liste aller Tiere

Bearbeiten der Spalten Name & Gewicht möglich

</details>
🧩 Architektur
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>
✔ DB.cs (modern, kurz)

Get() – SELECT

Execute() – INSERT/UPDATE/DELETE

Test() – Verbindung testen

✔ Form1.cs (stark verkürzt)

übersichtlichere Struktur

gemeinsame Hilfsfunktionen:

FillListBox()

FillComboBox()

UpdateStatus()

✔ ComboBoxItem

Speichert ID und Text

ideal für Foreign Keys

</details>
🚫 Einschränkungen
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>

Einträge können nur gelöscht werden, wenn keine Abhängigkeiten existieren

In der Übersicht sind nur Name & Gewicht direkt editierbar

</details>
🚀 Erweiterungen
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>

Suchfelder

PDF-/Excel-Export

Tierfotos

Fütterungsplan

Statistiken

User-Login & Rollen

</details>
👤 Entwickler
<details> <summary><strong>Klicken zum Aufklappen</strong></summary>

Schul-/Ausbildungsprojekt zur Übung von:

C# WinForms

MySQL

CRUD

relationalen Datenbanken

Softwarearchitektur

</details>