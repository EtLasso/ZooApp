# Zoo Verwaltungssoftware

Eine vollständige Windows Forms Anwendung zur Verwaltung eines Zoos mit MySQL-Datenbankanbindung.

## Funktionen

Die Anwendung bietet vollständiges CRUD (Create, Read, Update, Delete) für:
- **Kontinente**: Verwaltung der geografischen Kontinente
- **Gehege**: Verwaltung der Gehege mit Zuordnung zu Kontinenten
- **Tierarten**: Verwaltung verschiedener Tierarten
- **Tiere**: Verwaltung einzelner Tiere mit Name, Gewicht, Geburtsdatum, Tierart und Gehege
- **Übersicht**: Tabellarische Gesamtansicht aller Tiere mit allen Informationen

## Datenbankstruktur

### Tabelle: Kontinent
- `kID` (Primary Key)
- `Kbezeichnung` (Bezeichnung des Kontinents)

### Tabelle: Gehege
- `gID` (Primary Key)
- `GBezeichnung` (Bezeichnung des Geheges)
- `kontinentID` (Foreign Key zu Kontinent)

### Tabelle: Tierart
- `tierartID` (Primary Key)
- `TABezeichnung` (Bezeichnung der Tierart)

### Tabelle: Tiere
- `tierID` (Primary Key)
- `Name` (Name des Tieres)
- `Gewicht` (Gewicht in kg)
- `Geburtsdatum` (Geburtsdatum)
- `TierartID` (Foreign Key zu Tierart)
- `GehegeID` (Foreign Key zu Gehege)

## Voraussetzungen

1. **XAMPP** (oder anderer MySQL-Server)
   - MySQL-Server muss laufen
   - Standard-Port: 3306
   - Standard-User: root (ohne Passwort)

2. **.NET 9.0** (oder höher)
   - Windows Forms Desktop Application

3. **NuGet Pakete** (bereits in .csproj enthalten):
   - MySql.Data (Version 9.5.0)
   - System.Data.SqlClient (Version 4.9.0)

## Installation

### Schritt 1: Datenbank einrichten

1. Starten Sie XAMPP und aktivieren Sie den MySQL-Server
2. Öffnen Sie phpMyAdmin (http://localhost/phpmyadmin)
3. Importieren Sie die Datei `database_setup.sql` ODER führen Sie das SQL-Skript aus:
   - Klicken Sie auf "SQL" Tab
   - Kopieren Sie den Inhalt von `database_setup.sql`
   - Klicken Sie auf "Ausführen"

Die Datenbank `zoo_verwaltung` wird automatisch erstellt und mit Beispieldaten gefüllt.

### Schritt 2: Verbindungsstring anpassen (falls nötig)

Wenn Ihre MySQL-Konfiguration von den Standardeinstellungen abweicht, passen Sie die Verbindung in `DatabaseHelper.cs` an:

```csharp
connectionString = "server=localhost;port=3306;database=zoo_verwaltung;uid=root;pwd=;";
```

Ändern Sie:
- `server`: Wenn MySQL auf einem anderen Server läuft
- `port`: Wenn ein anderer Port verwendet wird
- `uid`: Wenn ein anderer Benutzername verwendet wird
- `pwd`: Wenn ein Passwort gesetzt ist

### Schritt 3: Anwendung starten

1. Öffnen Sie die Solution `ZooApp.sln` in Visual Studio
2. Stellen Sie sicher, dass alle NuGet-Pakete wiederhergestellt werden
3. Drücken Sie F5 oder klicken Sie auf "Start"

## Bedienung

### Allgemeines Bedienkonzept

Alle Verwaltungs-Tabs (Kontinent, Gehege, Tierart, Tiere) folgen dem gleichen Prinzip:

1. **Eingabebereich**: Formular links zum Eingeben/Bearbeiten von Daten
2. **ListBox**: Rechts werden alle vorhandenen Einträge angezeigt
3. **Buttons**:
   - **Neu**: Leert die Eingabefelder für einen neuen Eintrag
   - **Speichern**: Speichert den aktuellen Eintrag (neu oder aktualisiert)
   - **Löschen**: Löscht den ausgewählten Eintrag (mit Bestätigung)

### Workflow

#### Neuen Eintrag erstellen:
1. Klicken Sie auf "Neu"
2. Füllen Sie die Felder aus
3. Klicken Sie auf "Speichern"

#### Eintrag bearbeiten:
1. Wählen Sie einen Eintrag in der ListBox aus
2. Die Daten werden automatisch in die Eingabefelder geladen
3. Ändern Sie die gewünschten Felder
4. Klicken Sie auf "Speichern"

#### Eintrag löschen:
1. Wählen Sie einen Eintrag in der ListBox aus
2. Klicken Sie auf "Löschen"
3. Bestätigen Sie die Löschung

### Tab-spezifische Hinweise

#### Kontinent-Tab
- Eingabe: Bezeichnung des Kontinents
- Beispiel: "Afrika", "Asien", "Europa"

#### Gehege-Tab
- Eingabe: Bezeichnung und zugehöriger Kontinent (ComboBox)
- Der Kontinent muss zuerst angelegt sein
- Beispiel: "Savanne" in "Afrika"

#### Tierart-Tab
- Eingabe: Bezeichnung der Tierart
- Beispiel: "Löwe", "Tiger", "Elefant"

#### Tiere-Tab
- Eingabe:
  - Name des Tieres
  - Gewicht in kg (Dezimalzahl)
  - Geburtsdatum (DatePicker)
  - Tierart (ComboBox)
  - Gehege (ComboBox)
- Tierart und Gehege müssen zuerst angelegt sein

#### Übersicht-Tab
- Zeigt alle Tiere mit allen Informationen in einer Tabelle
- Spalten: Tiername, Gewicht, Tierart, Gehege, Kontinent
- Die Daten werden automatisch bei jeder Änderung aktualisiert

## Technische Details

### Architektur

- **DatabaseHelper.cs**: Zentrale Datenbankverbindungsklasse
  - `GetConnection()`: Erstellt MySQL-Verbindung
  - `ExecuteNonQuery()`: Führt INSERT, UPDATE, DELETE aus
  - `GetData()`: Führt SELECT-Abfragen aus und gibt DataTable zurück

- **Form1.cs**: Hauptformular mit gesamter Geschäftslogik
  - Separate Regions für jede Entität (Kontinent, Gehege, Tierart, Tiere, Übersicht)
  - Verwendung von parametrisierten Queries zur SQL-Injection-Vermeidung

- **ComboBoxItem**: Hilfsklasse für Dropdown-Listen
  - Speichert ID und Anzeigetext getrennt

### Sicherheit

- Alle SQL-Queries verwenden **parametrisierte Statements**
- Schutz vor SQL-Injection
- Foreign Key Constraints verhindern inkonsistente Daten
- Lösch-Bestätigungsdialoge

### Fehlerbehandlung

- Try-Catch-Blöcke um alle Datenbankoperationen
- Benutzerfreundliche Fehlermeldungen
- Validierung der Eingaben vor dem Speichern

## Bekannte Einschränkungen

1. **Löschen mit Abhängigkeiten**:
   - Kontinente können nicht gelöscht werden, wenn Gehege zugeordnet sind
   - Gehege können nicht gelöscht werden, wenn Tiere zugeordnet sind
   - Tierarten können nicht gelöscht werden, wenn Tiere zugeordnet sind
   - Dies ist gewollt (referentielle Integrität)

2. **Verbindung**:
   - Die Anwendung geht von einer lokalen MySQL-Installation aus
   - Bei Verbindungsproblemen wird eine Fehlermeldung angezeigt

## Erweiterungsmöglichkeiten

Mögliche zukünftige Features:
- Suchfunktion in den ListBoxen
- Export der Übersicht nach Excel/PDF
- Fotos für Tiere
- Fütterungsplan
- Statistiken und Diagramme
- Multi-User-Verwaltung mit Berechtigungen

## Entwickler

Erstellt als Schulprojekt für die Verwaltung eines Zoos mit vollständiger CRUD-Funktionalität.

## Lizenz

Dieses Projekt ist für Bildungszwecke erstellt.
