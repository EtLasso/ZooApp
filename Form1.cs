using System.Data;
using MySql.Data.MySqlClient;

namespace ZooApp
{
    public partial class Form1 : Form
    {
        // ============================================
        // KLASSENVARIABLEN
        // ============================================

        // DatabaseHelper-Instanz für Datenbankzugriffe
        private DatabaseHelper dbHelper;

        // Aktuelle IDs für CRUD-Operationen
        // 0 = Neuer Eintrag (Create), >0 = Bearbeiten vorhandenen Eintrags (Update)
        private int currentKontinentId = 0;
        private int currentGehegeId = 0;
        private int currentTierartId = 0;
        private int currentTierId = 0;

        // ============================================
        // KONSTRUKTOR
        // ============================================
        public Form1()
        {
            InitializeComponent();  // UI-Komponenten initialisieren
            dbHelper = new DatabaseHelper();  // DatabaseHelper-Instanz erstellen
        }

        // ============================================
        // FORM LOAD EVENT - Wird beim Start der Anwendung ausgeführt
        // ============================================
        private void Form1_Load(object sender, EventArgs e)
        {
            // Teste die Datenbankverbindung
            if (!dbHelper.TestConnection())
            {
                // Fehlermeldung bei Verbindungsproblemen
                MessageBox.Show(
                    "❌ Fehler: Keine Verbindung zur Datenbank!\n\n" +
                    "Bitte stelle sicher, dass:\n" +
                    "1. XAMPP läuft\n" +
                    "2. MySQL gestartet ist\n" +
                    "3. Die Datenbank 'zoo_verwaltung' existiert",
                    "Datenbankverbindung fehlgeschlagen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                UpdateStatus("❌ Keine Datenbankverbindung");
                return;  // Beende die Methode, da keine Verbindung besteht
            }

            UpdateStatus("✅ Verbunden mit Datenbank");

            // Lade alle Daten beim Start der Anwendung
            try
            {
                // Lade alle Listen und Comboboxen
                LoadKontinente();          // Kontinente in ListBox
                LoadGehege();              // Gehege in ListBox
                LoadTierarten();           // Tierarten in ListBox
                LoadTiere();               // Tiere in ListBox
                LoadKontinentComboBox();   // Kontinente in ComboBox (für Gehege)
                LoadTierartComboBox();     // Tierarten in ComboBox (für Tiere)
                LoadGehegeComboBox();      // Gehege in ComboBox (für Tiere)
                LoadUebersicht();          // Übersicht in DataGridView
                UpdateStatus("✅ Bereit - Alle Daten geladen");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung beim Laden der Daten
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Laden");
            }
        }

        // ============================================
        // HILFSMETHODEN
        // ============================================

        /// <summary>
        /// Aktualisiert den Status-Text im Status-Label
        /// </summary>
        /// <param name="message">Statusnachricht</param>
        private void UpdateStatus(string message)
        {
            lblStatus.Text = message;
        }

        #region Kontinent Methoden

        /// <summary>
        /// Leert die Eingabefelder für Kontinente
        /// </summary>
        private void ClearKontinentFields()
        {
            txtKBezeichnung.Text = "";     // Textfeld leeren
            currentKontinentId = 0;        // ID auf 0 setzen (neuer Eintrag)
            txtKBezeichnung.Focus();       // Fokus auf das Eingabefeld setzen
        }

        /// <summary>
        /// Lädt alle Kontinente aus der Datenbank in die ListBox
        /// </summary>
        private void LoadKontinente()
        {
            try
            {
                lbKontinent.Items.Clear();  // ListBox leeren

                // SQL-Abfrage für alle Kontinente, sortiert nach Bezeichnung
                string query = "SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung";
                DataTable dt = dbHelper.GetData(query);  // Daten abrufen

                // Jeden Kontinent zur ListBox hinzufügen (Format: "ID - Bezeichnung")
                foreach (DataRow row in dt.Rows)
                {
                    string item = $"{row["kID"]} - {row["Kbezeichnung"]}";
                    lbKontinent.Items.Add(item);
                }
                UpdateStatus($"📍 {dt.Rows.Count} Kontinente geladen");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                MessageBox.Show($"Fehler beim Laden der Kontinente: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Laden der Kontinente");
            }
        }

        /// <summary>
        /// Event-Handler für "Neuer Kontinent"-Button
        /// </summary>
        private void btnNewKontinent_Click(object sender, EventArgs e)
        {
            ClearKontinentFields();  // Felder leeren
            UpdateStatus("✨ Neuer Kontinent");
        }

        /// <summary>
        /// Event-Handler für "Speichern"-Button (Kontinent)
        /// Führt INSERT oder UPDATE durch, je nach aktueller ID
        /// </summary>
        private void btnSaveKontinent_Click(object sender, EventArgs e)
        {
            // Validierung: Prüfen ob Eingabe vorhanden
            if (string.IsNullOrWhiteSpace(txtKBezeichnung.Text))
            {
                MessageBox.Show("Bitte geben Sie eine Bezeichnung ein.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKBezeichnung.Focus();
                return;  // Methode abbrechen
            }

            try
            {
                string query;  // SQL-Query
                MySqlParameter[] parameters;  // SQL-Parameter für SQL-Injection-Schutz

                // Unterscheidung zwischen INSERT (Neu) und UPDATE (Bearbeiten)
                if (currentKontinentId == 0)  // Neuer Eintrag
                {
                    // INSERT-Query für neuen Kontinent
                    query = "INSERT INTO Kontinent (Kbezeichnung) VALUES (@bezeichnung)";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@bezeichnung", txtKBezeichnung.Text)
                    };
                }
                else  // Vorhandenen Eintrag bearbeiten
                {
                    // UPDATE-Query für vorhandenen Kontinent
                    query = "UPDATE Kontinent SET Kbezeichnung = @bezeichnung WHERE kID = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@bezeichnung", txtKBezeichnung.Text),
                        new MySqlParameter("@id", currentKontinentId)
                    };
                }

                // Query ausführen
                dbHelper.ExecuteNonQuery(query, parameters);

                // Erfolgsmeldung mit entsprechender Aktion
                string action = currentKontinentId == 0 ? "hinzugefügt" : "aktualisiert";
                MessageBox.Show($"✅ Kontinent erfolgreich {action}!",
                    "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Daten aktualisieren
                LoadKontinente();           // ListBox aktualisieren
                LoadKontinentComboBox();    // ComboBox aktualisieren (für Gehege)
                LoadUebersicht();           // Übersicht aktualisieren
                ClearKontinentFields();     // Felder leeren für nächste Eingabe
                UpdateStatus($"💾 Kontinent {action}");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Speichern");
            }
        }

        /// <summary>
        /// Event-Handler für "Löschen"-Button (Kontinent)
        /// </summary>
        private void btnDelKontinent_Click(object sender, EventArgs e)
        {
            // Prüfen ob ein Kontinent ausgewählt ist
            if (currentKontinentId == 0)
            {
                MessageBox.Show("Bitte wählen Sie einen Kontinent aus.",
                    "Keine Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Bestätigungsdialog anzeigen (Sicherheitsabfrage)
            DialogResult result = MessageBox.Show(
                $"Möchten Sie den Kontinent '{txtKBezeichnung.Text}' wirklich löschen?",
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // DELETE-Query für Kontinent
                    string query = "DELETE FROM Kontinent WHERE kID = @id";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", currentKontinentId)
                    };

                    // Query ausführen
                    dbHelper.ExecuteNonQuery(query, parameters);

                    // Erfolgsmeldung
                    MessageBox.Show("✅ Kontinent erfolgreich gelöscht!",
                        "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Daten aktualisieren
                    LoadKontinente();
                    LoadKontinentComboBox();
                    LoadUebersicht();
                    ClearKontinentFields();
                    UpdateStatus("🗑️ Kontinent gelöscht");
                }
                catch (Exception ex)
                {
                    // Fehlerbehandlung (z.B. wenn noch Gehege mit diesem Kontinent existieren)
                    MessageBox.Show(
                        $"❌ Fehler beim Löschen: {ex.Message}\n\n" +
                        "Hinweis: Der Kontinent kann nicht gelöscht werden, wenn noch Gehege zugeordnet sind.",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    UpdateStatus("❌ Löschen fehlgeschlagen");
                }
            }
        }

        /// <summary>
        /// Event-Handler für Auswahl in der Kontinent-ListBox
        /// Lädt die Daten des ausgewählten Kontinents in die Eingabefelder
        /// </summary>
        private void lbKontinent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Prüfen ob ein Element ausgewählt ist
            if (lbKontinent.SelectedItem != null)
            {
                string selected = lbKontinent.SelectedItem.ToString();
                string idStr = selected.Split('-')[0].Trim();  // ID extrahieren (Format: "ID - Bezeichnung")
                currentKontinentId = int.Parse(idStr);        // Aktuelle ID setzen

                // Daten des ausgewählten Kontinents aus Datenbank laden
                string query = "SELECT Kbezeichnung FROM Kontinent WHERE kID = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", currentKontinentId)
                };

                DataTable dt = dbHelper.GetData(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    // Daten in die Eingabefelder laden
                    txtKBezeichnung.Text = dt.Rows[0]["Kbezeichnung"].ToString();
                    UpdateStatus($"✏️ Kontinent '{txtKBezeichnung.Text}' ausgewählt");
                }
            }
        }

        #endregion

        #region Gehege Methoden

        /// <summary>
        /// Leert die Eingabefelder für Gehege
        /// </summary>
        private void ClearGehegeFields()
        {
            txtGBezeichnung.Text = "";
            cmbKontinentGehege.SelectedIndex = -1;  // Keine Auswahl in ComboBox
            currentGehegeId = 0;
            txtGBezeichnung.Focus();
        }

        /// <summary>
        /// Lädt alle Gehege aus der Datenbank in die ListBox
        /// </summary>
        private void LoadGehege()
        {
            try
            {
                lbGehege.Items.Clear();
                // JOIN-Abfrage: Gehege mit zugehörigem Kontinent
                string query = @"SELECT g.gID, g.GBezeichnung, k.Kbezeichnung
                                FROM Gehege g
                                LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                                ORDER BY g.GBezeichnung";
                DataTable dt = dbHelper.GetData(query);

                foreach (DataRow row in dt.Rows)
                {
                    // Format: "ID - Gehegename (Kontinent)"
                    string kontinent = row["Kbezeichnung"] != DBNull.Value ? row["Kbezeichnung"].ToString() : "N/A";
                    string item = $"{row["gID"]} - {row["GBezeichnung"]} ({kontinent})";
                    lbGehege.Items.Add(item);
                }
                UpdateStatus($"🏠 {dt.Rows.Count} Gehege geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Gehege: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Laden der Gehege");
            }
        }

        /// <summary>
        /// Lädt alle Kontinente in die ComboBox (für Gehege-Zuordnung)
        /// </summary>
        private void LoadKontinentComboBox()
        {
            try
            {
                cmbKontinentGehege.Items.Clear();
                string query = "SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung";
                DataTable dt = dbHelper.GetData(query);

                // ComboBox mit ComboBoxItem-Objekten füllen (Wert-Text-Paarung)
                foreach (DataRow row in dt.Rows)
                {
                    cmbKontinentGehege.Items.Add(new ComboBoxItem
                    {
                        Value = Convert.ToInt32(row["kID"]),      // ID als Wert
                        Text = row["Kbezeichnung"].ToString()     // Name als Text
                    });
                }

                // ComboBox-Eigenschaften setzen
                cmbKontinentGehege.DisplayMember = "Text";   // Was angezeigt wird
                cmbKontinentGehege.ValueMember = "Value";    // Was als Wert gespeichert wird
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Kontinente: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event-Handler für "Neues Gehege"-Button
        /// </summary>
        private void btnNewGehege_Click(object sender, EventArgs e)
        {
            ClearGehegeFields();
            UpdateStatus("✨ Neues Gehege");
        }

        /// <summary>
        /// Event-Handler für "Speichern"-Button (Gehege)
        /// </summary>
        private void btnSaveGehege_Click(object sender, EventArgs e)
        {
            // Validierung der Eingaben
            if (string.IsNullOrWhiteSpace(txtGBezeichnung.Text))
            {
                MessageBox.Show("Bitte geben Sie eine Bezeichnung ein.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGBezeichnung.Focus();
                return;
            }

            if (cmbKontinentGehege.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Kontinent aus.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbKontinentGehege.Focus();
                return;
            }

            try
            {
                // Wert aus der ComboBox extrahieren
                int kontinentId = ((ComboBoxItem)cmbKontinentGehege.SelectedItem).Value;
                string query;
                MySqlParameter[] parameters;

                if (currentGehegeId == 0)  // Neues Gehege
                {
                    query = "INSERT INTO Gehege (GBezeichnung, kontinentID) VALUES (@bezeichnung, @kontinentId)";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@bezeichnung", txtGBezeichnung.Text),
                        new MySqlParameter("@kontinentId", kontinentId)
                    };
                }
                else  // Gehege bearbeiten
                {
                    query = "UPDATE Gehege SET GBezeichnung = @bezeichnung, kontinentID = @kontinentId WHERE gID = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@bezeichnung", txtGBezeichnung.Text),
                        new MySqlParameter("@kontinentId", kontinentId),
                        new MySqlParameter("@id", currentGehegeId)
                    };
                }

                dbHelper.ExecuteNonQuery(query, parameters);

                string action = currentGehegeId == 0 ? "hinzugefügt" : "aktualisiert";
                MessageBox.Show($"✅ Gehege erfolgreich {action}!",
                    "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Daten aktualisieren
                LoadGehege();
                LoadGehegeComboBox();  // Für Tier-ComboBox aktualisieren
                LoadUebersicht();
                ClearGehegeFields();
                UpdateStatus($"💾 Gehege {action}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Speichern");
            }
        }

        /// <summary>
        /// Event-Handler für "Löschen"-Button (Gehege)
        /// </summary>
        private void btnDelGehege_Click(object sender, EventArgs e)
        {
            if (currentGehegeId == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Gehege aus.",
                    "Keine Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Möchten Sie das Gehege '{txtGBezeichnung.Text}' wirklich löschen?",
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Gehege WHERE gID = @id";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", currentGehegeId)
                    };

                    dbHelper.ExecuteNonQuery(query, parameters);
                    MessageBox.Show("✅ Gehege erfolgreich gelöscht!",
                        "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadGehege();
                    LoadGehegeComboBox();
                    LoadUebersicht();
                    ClearGehegeFields();
                    UpdateStatus("🗑️ Gehege gelöscht");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"❌ Fehler beim Löschen: {ex.Message}\n\n" +
                        "Hinweis: Das Gehege kann nicht gelöscht werden, wenn noch Tiere zugeordnet sind.",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    UpdateStatus("❌ Löschen fehlgeschlagen");
                }
            }
        }

        /// <summary>
        /// Event-Handler für Auswahl in der Gehege-ListBox
        /// </summary>
        private void lbGehege_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbGehege.SelectedItem != null)
            {
                string selected = lbGehege.SelectedItem.ToString();
                string idStr = selected.Split('-')[0].Trim();
                currentGehegeId = int.Parse(idStr);

                // Daten des ausgewählten Geheges laden
                string query = "SELECT GBezeichnung, kontinentID FROM Gehege WHERE gID = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", currentGehegeId)
                };

                DataTable dt = dbHelper.GetData(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    txtGBezeichnung.Text = dt.Rows[0]["GBezeichnung"].ToString();
                    int kontinentId = Convert.ToInt32(dt.Rows[0]["kontinentID"]);

                    // Entsprechenden Kontinent in ComboBox auswählen
                    foreach (ComboBoxItem item in cmbKontinentGehege.Items)
                    {
                        if (item.Value == kontinentId)
                        {
                            cmbKontinentGehege.SelectedItem = item;
                            break;
                        }
                    }
                    UpdateStatus($"✏️ Gehege '{txtGBezeichnung.Text}' ausgewählt");
                }
            }
        }

        #endregion

        #region Tierart Methoden

        /// <summary>
        /// Leert die Eingabefelder für Tierarten
        /// </summary>
        private void ClearTierartFields()
        {
            txtTABezeichnung.Text = "";
            currentTierartId = 0;
            txtTABezeichnung.Focus();
        }

        /// <summary>
        /// Lädt alle Tierarten aus der Datenbank in die ListBox
        /// </summary>
        private void LoadTierarten()
        {
            try
            {
                lbTierart.Items.Clear();
                string query = "SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung";
                DataTable dt = dbHelper.GetData(query);

                foreach (DataRow row in dt.Rows)
                {
                    string item = $"{row["tierartID"]} - {row["TABezeichnung"]}";
                    lbTierart.Items.Add(item);
                }
                UpdateStatus($"🦒 {dt.Rows.Count} Tierarten geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Tierarten: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Laden der Tierarten");
            }
        }

        /// <summary>
        /// Lädt alle Tierarten in die ComboBox (für Tier-Zuordnung)
        /// </summary>
        private void LoadTierartComboBox()
        {
            try
            {
                cmbTierartTiere.Items.Clear();
                string query = "SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung";
                DataTable dt = dbHelper.GetData(query);

                foreach (DataRow row in dt.Rows)
                {
                    cmbTierartTiere.Items.Add(new ComboBoxItem
                    {
                        Value = Convert.ToInt32(row["tierartID"]),
                        Text = row["TABezeichnung"].ToString()
                    });
                }

                cmbTierartTiere.DisplayMember = "Text";
                cmbTierartTiere.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Tierarten: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event-Handler für "Neue Tierart"-Button
        /// </summary>
        private void btnNewTierart_Click(object sender, EventArgs e)
        {
            ClearTierartFields();
            UpdateStatus("✨ Neue Tierart");
        }

        /// <summary>
        /// Event-Handler für "Speichern"-Button (Tierart)
        /// </summary>
        private void btnSaveTierart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTABezeichnung.Text))
            {
                MessageBox.Show("Bitte geben Sie eine Tierart ein.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTABezeichnung.Focus();
                return;
            }

            try
            {
                string query;
                MySqlParameter[] parameters;

                if (currentTierartId == 0)
                {
                    query = "INSERT INTO Tierart (TABezeichnung) VALUES (@bezeichnung)";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@bezeichnung", txtTABezeichnung.Text)
                    };
                }
                else
                {
                    query = "UPDATE Tierart SET TABezeichnung = @bezeichnung WHERE tierartID = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@bezeichnung", txtTABezeichnung.Text),
                        new MySqlParameter("@id", currentTierartId)
                    };
                }

                dbHelper.ExecuteNonQuery(query, parameters);

                string action = currentTierartId == 0 ? "hinzugefügt" : "aktualisiert";
                MessageBox.Show($"✅ Tierart erfolgreich {action}!",
                    "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTierarten();
                LoadTierartComboBox();  // Für Tier-ComboBox aktualisieren
                LoadUebersicht();
                ClearTierartFields();
                UpdateStatus($"💾 Tierart {action}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Speichern");
            }
        }

        /// <summary>
        /// Event-Handler für "Löschen"-Button (Tierart)
        /// </summary>
        private void btnDelTierart_Click(object sender, EventArgs e)
        {
            if (currentTierartId == 0)
            {
                MessageBox.Show("Bitte wählen Sie eine Tierart aus.",
                    "Keine Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Möchten Sie die Tierart '{txtTABezeichnung.Text}' wirklich löschen?",
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Tierart WHERE tierartID = @id";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", currentTierartId)
                    };

                    dbHelper.ExecuteNonQuery(query, parameters);
                    MessageBox.Show("✅ Tierart erfolgreich gelöscht!",
                        "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTierarten();
                    LoadTierartComboBox();
                    LoadUebersicht();
                    ClearTierartFields();
                    UpdateStatus("🗑️ Tierart gelöscht");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"❌ Fehler beim Löschen: {ex.Message}\n\n" +
                        "Hinweis: Die Tierart kann nicht gelöscht werden, wenn noch Tiere zugeordnet sind.",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    UpdateStatus("❌ Löschen fehlgeschlagen");
                }
            }
        }

        /// <summary>
        /// Event-Handler für Auswahl in der Tierart-ListBox
        /// </summary>
        private void lbTierart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTierart.SelectedItem != null)
            {
                string selected = lbTierart.SelectedItem.ToString();
                string idStr = selected.Split('-')[0].Trim();
                currentTierartId = int.Parse(idStr);

                string query = "SELECT TABezeichnung FROM Tierart WHERE tierartID = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", currentTierartId)
                };

                DataTable dt = dbHelper.GetData(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    txtTABezeichnung.Text = dt.Rows[0]["TABezeichnung"].ToString();
                    UpdateStatus($"✏️ Tierart '{txtTABezeichnung.Text}' ausgewählt");
                }
            }
        }

        #endregion

        #region Tiere Methoden

        /// <summary>
        /// Leert die Eingabefelder für Tiere
        /// </summary>
        private void ClearTiereFields()
        {
            txtTierName.Text = "";
            txtGewicht.Text = "";
            dtpGeburtsdatum.Value = DateTime.Now;  // Aktuelles Datum setzen
            cmbTierartTiere.SelectedIndex = -1;
            cmbGehegeTiere.SelectedIndex = -1;
            currentTierId = 0;
            txtTierName.Focus();
        }

        /// <summary>
        /// Lädt alle Tiere aus der Datenbank in die ListBox
        /// </summary>
        private void LoadTiere()
        {
            try
            {
                lbTiere.Items.Clear();
                // Komplexe JOIN-Abfrage: Tier mit Tierart und Gehege
                string query = @"SELECT t.tierID, t.Name, ta.TABezeichnung, g.GBezeichnung
                                FROM Tiere t
                                LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                                LEFT JOIN Gehege g ON t.GehegeID = g.gID
                                ORDER BY t.Name";
                DataTable dt = dbHelper.GetData(query);

                foreach (DataRow row in dt.Rows)
                {
                    // Format: "ID - Name (Tierart, Gehege)"
                    string tierart = row["TABezeichnung"] != DBNull.Value ? row["TABezeichnung"].ToString() : "N/A";
                    string gehege = row["GBezeichnung"] != DBNull.Value ? row["GBezeichnung"].ToString() : "N/A";
                    string item = $"{row["tierID"]} - {row["Name"]} ({tierart}, {gehege})";
                    lbTiere.Items.Add(item);
                }
                UpdateStatus($"🐾 {dt.Rows.Count} Tiere geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Tiere: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Laden der Tiere");
            }
        }

        /// <summary>
        /// Lädt alle Gehege in die ComboBox (für Tier-Zuordnung)
        /// </summary>
        private void LoadGehegeComboBox()
        {
            try
            {
                cmbGehegeTiere.Items.Clear();
                string query = "SELECT gID, GBezeichnung FROM Gehege ORDER BY GBezeichnung";
                DataTable dt = dbHelper.GetData(query);

                foreach (DataRow row in dt.Rows)
                {
                    cmbGehegeTiere.Items.Add(new ComboBoxItem
                    {
                        Value = Convert.ToInt32(row["gID"]),
                        Text = row["GBezeichnung"].ToString()
                    });
                }

                cmbGehegeTiere.DisplayMember = "Text";
                cmbGehegeTiere.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Gehege: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event-Handler für "Neues Tier"-Button
        /// </summary>
        private void btnNewTier_Click(object sender, EventArgs e)
        {
            ClearTiereFields();
            UpdateStatus("✨ Neues Tier");
        }

        /// <summary>
        /// Event-Handler für "Speichern"-Button (Tier)
        /// Umfangreichste Validierung, da mehrere Felder geprüft werden müssen
        /// </summary>
        private void btnSaveTier_Click(object sender, EventArgs e)
        {
            // Mehrstufige Validierung aller Eingabefelder

            // 1. Name validieren
            if (string.IsNullOrWhiteSpace(txtTierName.Text))
            {
                MessageBox.Show("Bitte geben Sie einen Namen ein.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTierName.Focus();
                return;
            }

            // 2. Gewicht validieren
            if (string.IsNullOrWhiteSpace(txtGewicht.Text))
            {
                MessageBox.Show("Bitte geben Sie ein Gewicht ein.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGewicht.Focus();
                return;
            }

            // 3. Gewicht als Zahl prüfen
            if (!decimal.TryParse(txtGewicht.Text, out decimal gewicht))
            {
                MessageBox.Show("Bitte geben Sie ein gültiges Gewicht ein (nur Zahlen).",
                    "Ungültige Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGewicht.Focus();
                return;
            }

            // 4. Tierart validieren
            if (cmbTierartTiere.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie eine Tierart aus.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTierartTiere.Focus();
                return;
            }

            // 5. Gehege validieren
            if (cmbGehegeTiere.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Gehege aus.",
                    "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGehegeTiere.Focus();
                return;
            }

            try
            {
                // Werte aus ComboBoxen extrahieren
                int tierartId = ((ComboBoxItem)cmbTierartTiere.SelectedItem).Value;
                int gehegeId = ((ComboBoxItem)cmbGehegeTiere.SelectedItem).Value;
                string query;
                MySqlParameter[] parameters;

                if (currentTierId == 0)  // Neues Tier
                {
                    query = @"INSERT INTO Tiere (Name, Gewicht, Geburtsdatum, TierartID, GehegeID) 
                             VALUES (@name, @gewicht, @geburtsdatum, @tierartId, @gehegeId)";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@name", txtTierName.Text),
                        new MySqlParameter("@gewicht", gewicht),
                        new MySqlParameter("@geburtsdatum", dtpGeburtsdatum.Value),
                        new MySqlParameter("@tierartId", tierartId),
                        new MySqlParameter("@gehegeId", gehegeId)
                    };
                }
                else  // Tier bearbeiten
                {
                    query = @"UPDATE Tiere SET Name = @name, Gewicht = @gewicht, 
                             Geburtsdatum = @geburtsdatum, TierartID = @tierartId, 
                             GehegeID = @gehegeId WHERE tierID = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@name", txtTierName.Text),
                        new MySqlParameter("@gewicht", gewicht),
                        new MySqlParameter("@geburtsdatum", dtpGeburtsdatum.Value),
                        new MySqlParameter("@tierartId", tierartId),
                        new MySqlParameter("@gehegeId", gehegeId),
                        new MySqlParameter("@id", currentTierId)
                    };
                }

                dbHelper.ExecuteNonQuery(query, parameters);

                string action = currentTierId == 0 ? "hinzugefügt" : "aktualisiert";
                MessageBox.Show($"✅ Tier erfolgreich {action}!",
                    "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTiere();
                LoadUebersicht();
                ClearTiereFields();
                UpdateStatus($"💾 Tier {action}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Speichern");
            }
        }

        /// <summary>
        /// Event-Handler für "Löschen"-Button (Tier)
        /// </summary>
        private void btnDelTier_Click(object sender, EventArgs e)
        {
            if (currentTierId == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Tier aus.",
                    "Keine Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Möchten Sie das Tier '{txtTierName.Text}' wirklich löschen?",
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Tiere WHERE tierID = @id";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", currentTierId)
                    };

                    dbHelper.ExecuteNonQuery(query, parameters);
                    MessageBox.Show("✅ Tier erfolgreich gelöscht!",
                        "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTiere();
                    LoadUebersicht();
                    ClearTiereFields();
                    UpdateStatus("🗑️ Tier gelöscht");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Fehler beim Löschen: {ex.Message}",
                        "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("❌ Löschen fehlgeschlagen");
                }
            }
        }

        /// <summary>
        /// Event-Handler für Auswahl in der Tier-ListBox
        /// </summary>
        private void lbTiere_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTiere.SelectedItem != null)
            {
                string selected = lbTiere.SelectedItem.ToString();
                string idStr = selected.Split('-')[0].Trim();
                currentTierId = int.Parse(idStr);

                // Alle Daten des ausgewählten Tieres laden
                string query = "SELECT Name, Gewicht, Geburtsdatum, TierartID, GehegeID FROM Tiere WHERE tierID = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id", currentTierId)
                };

                DataTable dt = dbHelper.GetData(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    // Einfache Felder setzen
                    txtTierName.Text = dt.Rows[0]["Name"].ToString();
                    txtGewicht.Text = dt.Rows[0]["Gewicht"].ToString();
                    dtpGeburtsdatum.Value = Convert.ToDateTime(dt.Rows[0]["Geburtsdatum"]);

                    // Fremdschlüssel-IDs extrahieren
                    int tierartId = Convert.ToInt32(dt.Rows[0]["TierartID"]);
                    int gehegeId = Convert.ToInt32(dt.Rows[0]["GehegeID"]);

                    // Entsprechende Einträge in ComboBoxen auswählen
                    foreach (ComboBoxItem item in cmbTierartTiere.Items)
                    {
                        if (item.Value == tierartId)
                        {
                            cmbTierartTiere.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (ComboBoxItem item in cmbGehegeTiere.Items)
                    {
                        if (item.Value == gehegeId)
                        {
                            cmbGehegeTiere.SelectedItem = item;
                            break;
                        }
                    }
                    UpdateStatus($"✏️ Tier '{txtTierName.Text}' ausgewählt");
                }
            }
        }

        #endregion

        #region Übersicht

        /// <summary>
        /// Lädt die Übersicht aller Tiere mit ihren Daten in das DataGridView
        /// Komplexe JOIN-Abfrage über alle vier Tabellen
        /// </summary>
        private void LoadUebersicht()
        {
            try
            {
                // Mehrfach-JOIN über alle Tabellen
                string query = @"SELECT
                                    t.Name AS Tiername,
                                    t.Gewicht,
                                    ta.TABezeichnung AS Tierart,
                                    g.GBezeichnung AS Gehege,
                                    k.Kbezeichnung AS Kontinent
                                FROM Tiere t
                                LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                                LEFT JOIN Gehege g ON t.GehegeID = g.gID
                                LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                                ORDER BY t.Name";

                // Daten abrufen und DataGridView zuweisen
                DataTable dt = dbHelper.GetData(query);
                dgvUebersicht.DataSource = dt;

                // DataGridView formatieren für bessere Darstellung
                if (dgvUebersicht.Columns.Count > 0)
                {
                    dgvUebersicht.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Formatierung der Spaltenüberschriften
                    dgvUebersicht.ColumnHeadersDefaultCellStyle.Font =
                        new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                    dgvUebersicht.ColumnHeadersDefaultCellStyle.BackColor =
                        System.Drawing.Color.FromArgb(41, 128, 185);
                    dgvUebersicht.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;

                    // Zebra-Streifen für bessere Lesbarkeit
                    dgvUebersicht.AlternatingRowsDefaultCellStyle.BackColor =
                        System.Drawing.Color.FromArgb(240, 240, 240);

                    // Auswahl-Farben
                    dgvUebersicht.DefaultCellStyle.SelectionBackColor =
                        System.Drawing.Color.FromArgb(52, 152, 219);
                    dgvUebersicht.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                }

                UpdateStatus($"📊 Übersicht mit {dt.Rows.Count} Tieren angezeigt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Übersicht: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Fehler beim Laden der Übersicht");
            }
        }

        /// <summary>
        /// Event-Handler für Zelländerungen in der Übersichtstabelle
        /// Speichert Änderungen automatisch in die Datenbank
        /// </summary>
        private void dgvUebersicht_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Prüfe ob gültige Zeile/Spalte (nicht Header-Zeile)
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            try
            {
                // Hole den Tiernamen aus der ersten Spalte (zur Identifikation)
                string tiername = dgvUebersicht.Rows[e.RowIndex].Cells["Tiername"].Value?.ToString();
                
                if (string.IsNullOrEmpty(tiername))
                    return;

                // Hole den Spaltennamen und neuen Wert
                string spaltenname = dgvUebersicht.Columns[e.ColumnIndex].Name;
                var neuerWert = dgvUebersicht.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                // Nur bestimmte Felder sind editierbar
                string query = "";
                MySqlParameter[] parameters = null;

                switch (spaltenname)
                {
                    case "Tiername":
                        // Name geändert - UPDATE durchführen
                        query = "UPDATE Tiere SET Name = @wert WHERE Name = @alterName";
                        parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@wert", neuerWert),
                            new MySqlParameter("@alterName", tiername)
                        };
                        break;

                    case "Gewicht":
                        // Gewicht geändert - validieren und UPDATE
                        if (!decimal.TryParse(neuerWert?.ToString(), out decimal gewicht))
                        {
                            MessageBox.Show("Bitte geben Sie ein gültiges Gewicht ein!",
                                "Ungültige Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            LoadUebersicht();  // Ursprünglichen Wert wiederherstellen
                            return;
                        }
                        query = "UPDATE Tiere SET Gewicht = @wert WHERE Name = @name";
                        parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@wert", gewicht),
                            new MySqlParameter("@name", tiername)
                        };
                        break;

                    default:
                        // Andere Spalten (Tierart, Gehege, Kontinent) sind nicht direkt editierbar
                        MessageBox.Show($"Die Spalte '{spaltenname}' kann nicht direkt bearbeitet werden.\n\n" +
                            "Bitte verwenden Sie die entsprechenden Tabs zum Bearbeiten.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUebersicht();  // Ursprünglichen Wert wiederherstellen
                        return;
                }

                // Änderung in Datenbank speichern
                dbHelper.ExecuteNonQuery(query, parameters);
                
                // Alle Ansichten aktualisieren
                LoadTiere();
                LoadUebersicht();
                
                UpdateStatus($"✅ {spaltenname} für '{tiername}' aktualisiert");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadUebersicht();  // Ursprünglichen Wert wiederherstellen
                UpdateStatus("❌ Fehler beim Speichern");
            }
        }

        #endregion

        // ============================================
        // UNBENUTZTE EVENT-HANDLER (vom Designer generiert)
        // ============================================

        private void lblTitle_Click(object sender, EventArgs e) { }
        private void panelHeader_Paint(object sender, PaintEventArgs e) { }
        private void cmbTierartTiere_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtTierName_TextChanged(object sender, EventArgs e) { }
    }

    // ============================================
    // HILFSKLASSE FÜR COMBOBOX-ITEMS
    // ============================================

    /// <summary>
    /// Hilfsklasse für ComboBox-Items mit Wert-Text-Paarung
    /// Ermöglicht die Speicherung von IDs (Wert) und Anzeigetexten (Text)
    /// </summary>
    public class ComboBoxItem
    {
        /// <summary>
        /// ID-Wert (für Datenbankoperationen)
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Anzeigetext (für Benutzer)
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Überschriebene ToString-Methode für die Anzeige in der ComboBox
        /// </summary>
        /// <returns>Den Anzeigetext</returns>
        public override string ToString()
        {
            return Text;
        }
    }
}