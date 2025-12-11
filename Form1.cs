using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ZooApp
{
    // Hauptformular der Zoo-Verwaltung
    public partial class Form1 : Form
    {
        #region Private Felder

        private readonly DB db = new DB();
        private readonly ZooDB zooDb = new ZooDB();
        
        // Aktuell ausgewählte IDs
        private int currentKontinentId = 0;
        private int currentGehegeId = 0;
        private int currentTierartId = 0;
        private int currentTierId = 0;
        private int currentFutterId = 0;

        #endregion

        #region Initialisierung

        public Form1()
        {
            InitializeComponent();
            
            // Button zum Erstellen von Futterplänen hinzufügen
            AddFutterplanButton();
        }

        // Fügt dynamisch einen Button zum Erstellen von Futterplänen hinzu
        private void AddFutterplanButton()
        {
            Button btnFutterplanNeu = new Button
            {
                Text = "➕ Fütterungsplan erstellen",
                Left = 230,
                Top = 10,
                Width = 200,
                Height = 35,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            
            btnFutterplanNeu.FlatAppearance.BorderSize = 0;
            btnFutterplanNeu.Click += btnFutterplanNeu_Click;
            
            // Button zum Fütterungsplan-Tab hinzufügen (Null-Check)
            if (tabControl1?.TabPages != null && tabControl1.TabPages.Count > 7)
            {
                tabControl1.TabPages[7].Controls.Add(btnFutterplanNeu);
            }
        }

        // Wird beim Laden des Formulars aufgerufen
        private void Form1_Load(object sender, EventArgs e)
        {
            // Datenbankverbindung testen
            if (!db.Test())
            {
                MessageBox.Show("❌ Keine Verbindung zur Datenbank!\n\nBitte XAMPP starten und MySQL läuft.",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("❌ Datenbank nicht verbunden");
                return;
            }

            UpdateStatus("✅ Verbunden mit Datenbank");

            try
            {
                // Alle Daten laden
                LoadKontinente();
                LoadGehege();
                LoadTierarten();
                LoadTiere();
                LoadKontinentComboBox();
                LoadTierartComboBox();
                LoadGehegeComboBox();
                LoadUebersicht();
                LoadTierartComboBoxFutterplan();
                ClearFutterFields();

                // Alle Tabs initial laden
                LoadFutterListe();
                LoadNachbestellung();
                LoadFutterplan();
                LoadTagesbedarf();
                LoadBestellungen();

                UpdateStatus("✅ Alle Daten geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden: {ex.Message}\n\nDetails: {ex.StackTrace}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tab wurde gewechselt - Daten neu laden
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;

            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 4: LoadUebersicht(); UpdateStatus("✅ Übersicht geladen"); break;
                    case 5: LoadFutterListe(); UpdateStatus("✅ Futtersorten geladen"); break;
                    case 6: LoadNachbestellung(); UpdateStatus("✅ Nachbestellliste geladen"); break;
                    case 7: LoadFutterplan(); UpdateStatus("✅ Fütterungsplan geladen"); break;
                    case 8: LoadTagesbedarf(); UpdateStatus("✅ Tagesbedarf geladen"); break;
                    case 9: LoadBestellungen(); UpdateStatus("✅ Bestellungen geladen"); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Aktualisiert die Statusleiste
        private void UpdateStatus(string msg)
        {
            if (lblStatus != null)
                lblStatus.Text = msg;
        }

        #endregion

        #region Hilfsmethoden

        // Füllt eine ListBox mit Daten
        private void FillListBox(ListBox box, DataTable dt, string idCol, string textCol, string postfix = "")
        {
            box.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string id = row[idCol].ToString();
                string txt = row[textCol].ToString();
                box.Items.Add($"{id} - {txt}{postfix}");
            }
        }

        // Füllt eine ComboBox mit ComboBoxItem-Objekten
        private void FillComboBox(ComboBox box, DataTable dt, string idCol, string textCol)
        {
            box.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                box.Items.Add(new ComboBoxItem
                {
                    Value = Convert.ToInt32(row[idCol]),
                    Text = row[textCol].ToString()
                });
            }
            box.DisplayMember = "Text";
            box.ValueMember = "Value";
        }

        #endregion

        #region KONTINENTE

        // Lädt alle Kontinente
        private void LoadKontinente()
        {
            DataTable dt = db.Get("SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung");
            FillListBox(lbKontinent, dt, "kID", "Kbezeichnung");
        }

        // Lädt Kontinente in ComboBox
        private void LoadKontinentComboBox()
        {
            DataTable dt = db.Get("SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung");
            FillComboBox(cmbKontinentGehege, dt, "kID", "Kbezeichnung");
        }

        // Leert Eingabefelder
        private void ClearKontinentFields()
        {
            txtKBezeichnung.Text = "";
            currentKontinentId = 0;
        }

        private void btnNewKontinent_Click(object sender, EventArgs e)
        {
            ClearKontinentFields();
        }

        // Speichert neuen oder bestehenden Kontinent
        private void btnSaveKontinent_Click(object sender, EventArgs e)
        {
            if (txtKBezeichnung.Text == "")
            {
                MessageBox.Show("Bitte Bezeichnung eingeben.");
                return;
            }

            if (currentKontinentId == 0)
                db.Execute("INSERT INTO Kontinent (Kbezeichnung) VALUES (@p)", ("@p", txtKBezeichnung.Text));
            else
                db.Execute("UPDATE Kontinent SET Kbezeichnung=@p WHERE kID=@id",
                    ("@p", txtKBezeichnung.Text), ("@id", currentKontinentId));

            LoadKontinente();
            LoadKontinentComboBox();
            ClearKontinentFields();
        }

        // Löscht Kontinent nach Bestätigung
        private void btnDelKontinent_Click(object sender, EventArgs e)
        {
            if (currentKontinentId == 0) return;

            if (MessageBox.Show("Kontinent wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Kontinent WHERE kID=@id", ("@id", currentKontinentId));
                LoadKontinente();
                LoadKontinentComboBox();
                ClearKontinentFields();
            }
        }

        // Kontinent wurde ausgewählt
        private void lbKontinent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbKontinent.SelectedItem == null) return;

            currentKontinentId = int.Parse(lbKontinent.SelectedItem.ToString().Split('-')[0].Trim());
            DataTable dt = db.Get("SELECT Kbezeichnung FROM Kontinent WHERE kID=@id", ("@id", currentKontinentId));

            if (dt.Rows.Count > 0)
                txtKBezeichnung.Text = dt.Rows[0][0].ToString();
        }

        #endregion

        #region GEHEGE

        // Lädt alle Gehege mit Kontinent
        private void LoadGehege()
        {
            DataTable dt = db.Get(@"
                SELECT g.gID, g.GBezeichnung, COALESCE(k.Kbezeichnung,'?') AS Kontinent
                FROM Gehege g
                LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                ORDER BY g.GBezeichnung");

            lbGehege.Items.Clear();
            foreach (DataRow r in dt.Rows)
                lbGehege.Items.Add($"{r["gID"]} - {r["GBezeichnung"]} ({r["Kontinent"]})");
        }

        // Lädt Gehege in ComboBox
        private void LoadGehegeComboBox()
        {
            DataTable dt = db.Get("SELECT gID, GBezeichnung FROM Gehege ORDER BY GBezeichnung");
            FillComboBox(cmbGehegeTiere, dt, "gID", "GBezeichnung");
        }

        private void ClearGehegeFields()
        {
            txtGBezeichnung.Text = "";
            cmbKontinentGehege.SelectedIndex = -1;
            currentGehegeId = 0;
        }

        private void btnNewGehege_Click(object sender, EventArgs e)
        {
            ClearGehegeFields();
        }

        // Speichert Gehege
        private void btnSaveGehege_Click(object sender, EventArgs e)
        {
            if (txtGBezeichnung.Text == "" || cmbKontinentGehege.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte alles ausfüllen.");
                return;
            }

            int kontinentId = ((ComboBoxItem)cmbKontinentGehege.SelectedItem).Value;

            if (currentGehegeId == 0)
                db.Execute("INSERT INTO Gehege (GBezeichnung, kontinentID) VALUES (@n,@k)",
                    ("@n", txtGBezeichnung.Text), ("@k", kontinentId));
            else
                db.Execute("UPDATE Gehege SET GBezeichnung=@n, kontinentID=@k WHERE gID=@id",
                    ("@n", txtGBezeichnung.Text), ("@k", kontinentId), ("@id", currentGehegeId));

            LoadGehege();
            LoadGehegeComboBox();
            ClearGehegeFields();
        }

        private void btnDelGehege_Click(object sender, EventArgs e)
        {
            if (currentGehegeId == 0) return;

            if (MessageBox.Show("Gehege wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Gehege WHERE gID=@id", ("@id", currentGehegeId));
                LoadGehege();
                LoadGehegeComboBox();
                ClearGehegeFields();
            }
        }

        // Gehege wurde ausgewählt
        private void lbGehege_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbGehege.SelectedItem == null) return;

            currentGehegeId = int.Parse(lbGehege.SelectedItem.ToString().Split('-')[0].Trim());
            DataTable dt = db.Get("SELECT GBezeichnung, kontinentID FROM Gehege WHERE gID=@id", ("@id", currentGehegeId));

            if (dt.Rows.Count > 0)
            {
                txtGBezeichnung.Text = dt.Rows[0]["GBezeichnung"].ToString();
                int kontinentId = Convert.ToInt32(dt.Rows[0]["kontinentID"]);

                foreach (ComboBoxItem it in cmbKontinentGehege.Items)
                    if (it.Value == kontinentId)
                        cmbKontinentGehege.SelectedItem = it;
            }
        }

        #endregion

        #region TIERARTEN

        private void LoadTierarten()
        {
            DataTable dt = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            FillListBox(lbTierart, dt, "tierartID", "TABezeichnung");
        }

        private void LoadTierartComboBox()
        {
            DataTable dt = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            FillComboBox(cmbTierartTiere, dt, "tierartID", "TABezeichnung");
        }

        private void ClearTierartFields()
        {
            txtTABezeichnung.Text = "";
            currentTierartId = 0;
        }

        private void btnNewTierart_Click(object sender, EventArgs e)
        {
            ClearTierartFields();
        }

        private void btnSaveTierart_Click(object sender, EventArgs e)
        {
            if (txtTABezeichnung.Text == "")
            {
                MessageBox.Show("Bitte eine Tierart eingeben.");
                return;
            }

            if (currentTierartId == 0)
                db.Execute("INSERT INTO Tierart (TABezeichnung) VALUES (@p)", ("@p", txtTABezeichnung.Text));
            else
                db.Execute("UPDATE Tierart SET TABezeichnung=@p WHERE tierartID=@id",
                    ("@p", txtTABezeichnung.Text), ("@id", currentTierartId));

            LoadTierarten();
            LoadTierartComboBox();
            ClearTierartFields();
        }

        private void btnDelTierart_Click(object sender, EventArgs e)
        {
            if (currentTierartId == 0) return;

            if (MessageBox.Show("Tierart wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Tierart WHERE tierartID=@id", ("@id", currentTierartId));
                LoadTierarten();
                LoadTierartComboBox();
                ClearTierartFields();
            }
        }

        private void lbTierart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTierart.SelectedItem == null) return;

            currentTierartId = int.Parse(lbTierart.SelectedItem.ToString().Split('-')[0].Trim());
            DataTable dt = db.Get("SELECT TABezeichnung FROM Tierart WHERE tierartID=@id", ("@id", currentTierartId));

            if (dt.Rows.Count > 0)
                txtTABezeichnung.Text = dt.Rows[0][0].ToString();
        }

        #endregion

        #region TIERE

        // Lädt alle Tiere mit Details
        private void LoadTiere()
        {
            DataTable dt = db.Get(@"
                SELECT t.tierID, t.Name, ta.TABezeichnung, g.GBezeichnung
                FROM Tiere t
                LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                LEFT JOIN Gehege g ON t.GehegeID = g.gID
                ORDER BY t.Name");

            lbTiere.Items.Clear();
            foreach (DataRow r in dt.Rows)
                lbTiere.Items.Add($"{r["tierID"]} - {r["Name"]} ({r["TABezeichnung"]}, {r["GBezeichnung"]})");
        }

        private void ClearTiereFields()
        {
            txtTierName.Text = "";
            txtGewicht.Text = "";
            dtpGeburtsdatum.Value = DateTime.Now;
            cmbTierartTiere.SelectedIndex = -1;
            cmbGehegeTiere.SelectedIndex = -1;
            currentTierId = 0;
        }

        private void btnNewTier_Click(object sender, EventArgs e)
        {
            ClearTiereFields();
        }

        // Speichert Tier mit Validierung
        private void btnSaveTier_Click(object sender, EventArgs e)
        {
            if (txtTierName.Text == "" || txtGewicht.Text == "" ||
                !decimal.TryParse(txtGewicht.Text, out decimal gewicht) ||
                cmbTierartTiere.SelectedIndex == -1 || cmbGehegeTiere.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte alle Felder korrekt ausfüllen.");
                return;
            }

            int tierartId = ((ComboBoxItem)cmbTierartTiere.SelectedItem).Value;
            int gehegeId = ((ComboBoxItem)cmbGehegeTiere.SelectedItem).Value;

            if (currentTierId == 0)
                db.Execute(@"INSERT INTO Tiere (Name,Gewicht,Geburtsdatum,TierartID,GehegeID)
                            VALUES (@n,@g,@d,@t,@h)",
                    ("@n", txtTierName.Text), ("@g", gewicht), ("@d", dtpGeburtsdatum.Value),
                    ("@t", tierartId), ("@h", gehegeId));
            else
                db.Execute(@"UPDATE Tiere SET Name=@n, Gewicht=@g, Geburtsdatum=@d,
                            TierartID=@t, GehegeID=@h WHERE tierID=@id",
                    ("@n", txtTierName.Text), ("@g", gewicht), ("@d", dtpGeburtsdatum.Value),
                    ("@t", tierartId), ("@h", gehegeId), ("@id", currentTierId));

            LoadTiere();
            LoadUebersicht();
            ClearTiereFields();
        }

        private void btnDelTier_Click(object sender, EventArgs e)
        {
            if (currentTierId == 0) return;

            if (MessageBox.Show("Tier wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Tiere WHERE tierID=@id", ("@id", currentTierId));
                LoadTiere();
                LoadUebersicht();
                ClearTiereFields();
            }
        }

        // Tier wurde ausgewählt
        private void lbTiere_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTiere.SelectedItem == null) return;

            currentTierId = int.Parse(lbTiere.SelectedItem.ToString().Split('-')[0].Trim());
            DataTable dt = db.Get("SELECT * FROM Tiere WHERE tierID=@id", ("@id", currentTierId));

            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];
            txtTierName.Text = r["Name"].ToString();
            txtGewicht.Text = r["Gewicht"].ToString();
            dtpGeburtsdatum.Value = Convert.ToDateTime(r["Geburtsdatum"]);

            int tierartId = Convert.ToInt32(r["TierartID"]);
            int gehegeId = Convert.ToInt32(r["GehegeID"]);

            foreach (ComboBoxItem it in cmbTierartTiere.Items)
                if (it.Value == tierartId)
                    cmbTierartTiere.SelectedItem = it;

            foreach (ComboBoxItem it in cmbGehegeTiere.Items)
                if (it.Value == gehegeId)
                    cmbGehegeTiere.SelectedItem = it;
        }

        #endregion

        #region ÜBERSICHT

        // Lädt komplette Tier-Übersicht mit Fehlerbehandlung
        private void LoadUebersicht()
        {
            if (dgvUebersicht == null) return;

            try
            {
                // Prüfe zuerst ob Spalten existieren
                bool hasBildpfad = CheckColumnExists("Tiere", "Bildpfad");
                bool hasNotizen = CheckColumnExists("Tiere", "Notizen");

                string sql;
                
                if (hasBildpfad && hasNotizen)
                {
                    // Erweiterte Version mit allen Spalten
                    sql = @"
                        SELECT 
                            t.tierID AS 'ID',
                            t.Name AS 'Name',
                            ta.TABezeichnung AS 'Tierart',
                            t.Gewicht AS 'Gewicht (kg)',
                            DATE_FORMAT(t.Geburtsdatum, '%d.%m.%Y') AS 'Geburtsdatum',
                            TIMESTAMPDIFF(YEAR, t.Geburtsdatum, CURDATE()) AS 'Alter',
                            g.GBezeichnung AS 'Gehege',
                            k.Kbezeichnung AS 'Kontinent',
                            CASE 
                                WHEN t.Bildpfad IS NOT NULL AND t.Bildpfad != '' THEN '🖼️ Ja'
                                ELSE '❌ Nein'
                            END AS 'Bild',
                            CASE 
                                WHEN t.Notizen IS NOT NULL AND t.Notizen != '' THEN '📝 Ja'
                                ELSE '❌ Nein'
                            END AS 'Notizen'
                        FROM Tiere t
                        LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                        LEFT JOIN Gehege g ON t.GehegeID = g.gID
                        LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                        ORDER BY t.Name";
                }
                else
                {
                    // Basis-Version ohne Bild/Notizen
                    sql = @"
                        SELECT 
                            t.tierID AS 'ID',
                            t.Name AS 'Name',
                            ta.TABezeichnung AS 'Tierart',
                            t.Gewicht AS 'Gewicht (kg)',
                            DATE_FORMAT(t.Geburtsdatum, '%d.%m.%Y') AS 'Geburtsdatum',
                            TIMESTAMPDIFF(YEAR, t.Geburtsdatum, CURDATE()) AS 'Alter',
                            g.GBezeichnung AS 'Gehege',
                            k.Kbezeichnung AS 'Kontinent'
                        FROM Tiere t
                        LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                        LEFT JOIN Gehege g ON t.GehegeID = g.gID
                        LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                        ORDER BY t.Name";
                }

                DataTable dt = db.Get(sql);
                dgvUebersicht.DataSource = dt;
                dgvUebersicht.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                
                // ID-Spalte schmal machen
                if (dgvUebersicht.Columns.Contains("ID"))
                    dgvUebersicht.Columns["ID"].Width = 50;
                
                // Doppelklick-Event hinzufügen (nur einmal)
                dgvUebersicht.CellDoubleClick -= dgvUebersicht_CellDoubleClick;
                dgvUebersicht.CellDoubleClick += dgvUebersicht_CellDoubleClick;
                
                string statusMsg = hasBildpfad && hasNotizen 
                    ? $"✅ {dt.Rows.Count} Tiere - Doppelklick für Details" 
                    : $"✅ {dt.Rows.Count} Tiere (Basis-Ansicht - DB-Update nötig für Details)";
                
                UpdateStatus(statusMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Übersicht:\n{ex.Message}\n\nBitte prüfen Sie die Datenbank-Verbindung.", 
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Fallback: Basis-Query ohne spezielle Spalten
                try
                {
                    DataTable dt = db.Get(@"
                        SELECT t.tierID, t.Name AS Tiername, t.Gewicht,
                            ta.TABezeichnung AS Tierart, g.GBezeichnung AS Gehege,
                            k.Kbezeichnung AS Kontinent
                        FROM Tiere t
                        LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                        LEFT JOIN Gehege g ON t.GehegeID = g.gID
                        LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                        ORDER BY t.Name");

                    dgvUebersicht.DataSource = dt;
                    dgvUebersicht.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    UpdateStatus("✅ Basis-Übersicht geladen");
                }
                catch
                {
                    UpdateStatus("❌ Fehler beim Laden der Übersicht");
                }
            }
        }
        
        // Prüft ob Datenbank-Spalte existiert
        private bool CheckColumnExists(string tableName, string columnName)
        {
            try
            {
                DataTable dt = db.Get(@"
                    SELECT COUNT(*) as cnt 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = @table 
                    AND COLUMN_NAME = @column",
                    ("@table", tableName),
                    ("@column", columnName));
                
                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cnt"]) > 0;
            }
            catch
            {
                return false;
            }
        }
        
        // Event: Doppelklick auf Tier öffnet Detail-Fenster
        private void dgvUebersicht_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            
            try
            {
                // Prüfe ob Bild/Notizen-Spalten existieren
                bool detailsAvailable = CheckColumnExists("Tiere", "Bildpfad");
                
                if (!detailsAvailable)
                {
                    MessageBox.Show(
                        "⚠️ Detail-Ansicht nicht verfügbar!\n\n" +
                        "Bitte führen Sie zuerst das Datenbank-Update aus:\n" +
                        "database_update_bilder.sql in phpMyAdmin ausführen.",
                        "Information", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    return;
                }
                
                int tierID = Convert.ToInt32(dgvUebersicht.Rows[e.RowIndex].Cells["ID"].Value);
                TierDetailForm detailForm = new TierDetailForm(tierID);
                detailForm.ShowDialog();
                
                // Nach dem Schließen Übersicht neu laden
                LoadUebersicht();
                LoadTiere();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Öffnen: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Erlaubt direktes Bearbeiten von Name und Gewicht
        private void dgvUebersicht_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvUebersicht.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string col = dgvUebersicht.Columns[e.ColumnIndex].Name;
            var value = row.Cells[e.ColumnIndex].Value;

            switch (col)
            {
                case "Name":
                    db.Execute("UPDATE Tiere SET Name=@v WHERE tierID=@id", ("@v", value), ("@id", id));
                    break;

                case "Gewicht (kg)":
                    if (!decimal.TryParse(value.ToString(), out _))
                    {
                        MessageBox.Show("Ungültiges Gewicht.");
                        LoadUebersicht();
                        return;
                    }
                    db.Execute("UPDATE Tiere SET Gewicht=@v WHERE tierID=@id", ("@v", value), ("@id", id));
                    break;

                default:
                    MessageBox.Show("Diese Spalte ist nicht editierbar.");
                    LoadUebersicht();
                    return;
            }

            LoadTiere();
            LoadUebersicht();
        }

        #endregion

        #region FUTTERVERWALTUNG

        private void ClearFutterFields()
        {
            txtFutterBezeichnung.Text = "";
            txtFutterEinheit.Text = "kg";
            numFutterPreis.Value = 1;
            numFutterLagerbestand.Value = 0;
            numFutterMindestbestand.Value = 50;
            numFutterBestellmenge.Value = 100;
            currentFutterId = 0;
        }

        private void btnFutterNeu_Click(object sender, EventArgs e)
        {
            ClearFutterFields();
        }

        private void btnLadeFutter_Click(object sender, EventArgs e)
        {
            LoadFutterListe();
            UpdateStatus("✅ Futtersorten neu geladen");
        }

        // Lädt alle Futtersorten
        private void LoadFutterListe()
        {
            try
            {
                DataTable dt = zooDb.GetAlleFuttersorten();
                dgvFutter.DataSource = dt;
                UpdateStatus($"✅ {dt.Rows.Count} Futtersorten geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Speichert Futtersorte
        private void btnFutterSpeichern_Click(object sender, EventArgs e)
        {
            if (txtFutterBezeichnung.Text == "" || txtFutterEinheit.Text == "")
            {
                MessageBox.Show("Bitte Bezeichnung und Einheit eingeben.");
                return;
            }

            try
            {
                if (currentFutterId == 0)
                {
                    string sql = @"INSERT INTO Futter 
                                (Bezeichnung, Einheit, Preis_pro_Einheit, Lagerbestand, Mindestbestand, Bestellmenge)
                                VALUES (@bez, @einheit, @preis, @lager, @mindest, @bestell)";

                    db.Execute(sql,
                        ("@bez", txtFutterBezeichnung.Text), ("@einheit", txtFutterEinheit.Text),
                        ("@preis", numFutterPreis.Value), ("@lager", (int)numFutterLagerbestand.Value),
                        ("@mindest", (int)numFutterMindestbestand.Value), ("@bestell", (int)numFutterBestellmenge.Value));
                }
                else
                {
                    string sql = @"UPDATE Futter SET Bezeichnung=@bez, Einheit=@einheit,
                                Preis_pro_Einheit=@preis, Lagerbestand=@lager,
                                Mindestbestand=@mindest, Bestellmenge=@bestell
                                WHERE futterID=@id";

                    db.Execute(sql,
                        ("@bez", txtFutterBezeichnung.Text), ("@einheit", txtFutterEinheit.Text),
                        ("@preis", numFutterPreis.Value), ("@lager", (int)numFutterLagerbestand.Value),
                        ("@mindest", (int)numFutterMindestbestand.Value),
                        ("@bestell", (int)numFutterBestellmenge.Value), ("@id", currentFutterId));
                }

                LoadFutterListe();
                ClearFutterFields();
                UpdateStatus("✅ Futter gespeichert");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFutterLöschen_Click(object sender, EventArgs e)
        {
            if (currentFutterId == 0) return;

            if (MessageBox.Show("Futtersorte wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    db.Execute("DELETE FROM Futter WHERE futterID=@id", ("@id", currentFutterId));
                    LoadFutterListe();
                    ClearFutterFields();
                    UpdateStatus("✅ Futter gelöscht");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}\n\nMöglicherweise wird dieses Futter noch verwendet.",
                        "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Futtersorte wurde ausgewählt
        private void dgvFutter_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFutter.SelectedRows.Count > 0)
            {
                var row = dgvFutter.SelectedRows[0];
                currentFutterId = Convert.ToInt32(row.Cells["futterID"].Value);
                txtFutterBezeichnung.Text = row.Cells["Bezeichnung"].Value.ToString();
                txtFutterEinheit.Text = row.Cells["Einheit"].Value.ToString();
                numFutterPreis.Value = Convert.ToDecimal(row.Cells["Preis_pro_Einheit"].Value);
                numFutterLagerbestand.Value = Convert.ToInt32(row.Cells["Lagerbestand"].Value);
                numFutterMindestbestand.Value = Convert.ToInt32(row.Cells["Mindestbestand"].Value);
                numFutterBestellmenge.Value = Convert.ToInt32(row.Cells["Bestellmenge"].Value);
            }
        }

        #endregion

        #region NACHBESTELLUNG

        private void LoadNachbestellung()
        {
            try
            {
                DataTable dt = zooDb.GetNachbestellListe();
                dgvNachbestellung.DataSource = dt;
                UpdateStatus($"✅ {dt.Rows.Count} Nachbestellpositionen geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLadeNachbestellung_Click(object sender, EventArgs e)
        {
            LoadNachbestellung();
        }

        #endregion

        #region FÜTTERUNGSPLAN

        // Lädt Tierarten für Filterung
        private void LoadTierartComboBoxFutterplan()
        {
            DataTable dt = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            cmbTierartFutterplan.Items.Clear();

            cmbTierartFutterplan.Items.Add(new ComboBoxItem { Value = 0, Text = "-- Alle Tierarten --" });

            foreach (DataRow row in dt.Rows)
            {
                cmbTierartFutterplan.Items.Add(new ComboBoxItem
                {
                    Value = Convert.ToInt32(row["tierartID"]),
                    Text = row["TABezeichnung"].ToString()
                });
            }

            cmbTierartFutterplan.DisplayMember = "Text";
            cmbTierartFutterplan.ValueMember = "Value";
            cmbTierartFutterplan.SelectedIndex = 0;
        }

        private void LoadFutterplan()
        {
            try
            {
                DataTable dt;

                if (cmbTierartFutterplan.SelectedIndex > 0)
                {
                    int tierartId = ((ComboBoxItem)cmbTierartFutterplan.SelectedItem).Value;
                    dt = zooDb.GetFutterplanFuerTierart(tierartId);
                    UpdateStatus($"✅ Fütterungsplan für Tierart geladen");
                }
                else
                {
                    // Alle Fütterungspläne laden
                    string sql = @"
                        SELECT ta.TABezeichnung AS Tierart, f.Bezeichnung AS Futtersorte,
                            tf.Menge_pro_Tag, f.Einheit, tf.Fütterungszeit,
                            CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit, ' um ', tf.Fütterungszeit) AS Fütterungsplan
                        FROM Tierart_Futter tf
                        JOIN Tierart ta ON tf.tierartID = ta.tierartID
                        JOIN Futter f ON tf.futterID = f.futterID
                        ORDER BY ta.TABezeichnung, tf.Fütterungszeit";

                    dt = db.Get(sql);
                    UpdateStatus($"✅ Fütterungsplan geladen ({dt.Rows.Count} Einträge)");
                }

                dgvFutterplan.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLadeFutterplan_Click(object sender, EventArgs e)
        {
            LoadFutterplan();
        }

        // Öffnet Dialog zum Erstellen eines Fütterungsplans
        private void btnFutterplanNeu_Click(object sender, EventArgs e)
        {
            // Dialog erstellen
            Form dialog = new Form
            {
                Text = "🍽️ Fütterungsplan erstellen",
                Size = new Size(450, 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            // Labels
            Label lblTierart = new Label { Text = "Tierart:", Left = 20, Top = 20, Width = 100 };
            Label lblFutter = new Label { Text = "Futter:", Left = 20, Top = 60, Width = 100 };
            Label lblMenge = new Label { Text = "Menge pro Tag:", Left = 20, Top = 100, Width = 100 };
            Label lblZeit = new Label { Text = "Fütterungszeit:", Left = 20, Top = 140, Width = 100 };
            Label lblEinheit = new Label { Text = "(kg)", Left = 350, Top = 100, Width = 50 };

            // ComboBoxen und Controls
            ComboBox cmbTierart = new ComboBox { Left = 130, Top = 17, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            ComboBox cmbFutter = new ComboBox { Left = 130, Top = 57, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            NumericUpDown numMenge = new NumericUpDown { Left = 130, Top = 97, Width = 210, DecimalPlaces = 2, Minimum = 0.01m, Maximum = 1000, Value = 5, Increment = 0.5m };
            MaskedTextBox mtxtZeit = new MaskedTextBox { Left = 130, Top = 137, Width = 100, Mask = "00:00", Text = "08:00" };

            // Buttons
            Button btnSpeichern = new Button { Text = "💾 Speichern", Left = 150, Top = 220, Width = 130, Height = 45 };
            btnSpeichern.BackColor = Color.FromArgb(46, 204, 113);
            btnSpeichern.ForeColor = Color.White;
            btnSpeichern.FlatStyle = FlatStyle.Flat;
            btnSpeichern.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSpeichern.Cursor = Cursors.Hand;

            Button btnAbbrechen = new Button { Text = "❌ Abbrechen", Left = 290, Top = 220, Width = 130, Height = 45 };
            btnAbbrechen.BackColor = Color.FromArgb(231, 76, 60);
            btnAbbrechen.ForeColor = Color.White;
            btnAbbrechen.FlatStyle = FlatStyle.Flat;
            btnAbbrechen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAbbrechen.Cursor = Cursors.Hand;

            // Daten laden
            DataTable dtTierarten = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            DataTable dtFutter = db.Get("SELECT futterID, Bezeichnung, Einheit FROM Futter ORDER BY Bezeichnung");

            foreach (DataRow row in dtTierarten.Rows)
            {
                cmbTierart.Items.Add(new ComboBoxItem
                {
                    Value = Convert.ToInt32(row["tierartID"]),
                    Text = row["TABezeichnung"].ToString()
                });
            }
            cmbTierart.DisplayMember = "Text";
            cmbTierart.ValueMember = "Value";

            foreach (DataRow row in dtFutter.Rows)
            {
                cmbFutter.Items.Add(new ComboBoxItem
                {
                    Value = Convert.ToInt32(row["futterID"]),
                    Text = $"{row["Bezeichnung"]} ({row["Einheit"]})"
                });
            }
            cmbFutter.DisplayMember = "Text";
            cmbFutter.ValueMember = "Value";

            // Event Handlers
            btnSpeichern.Click += (s, ev) =>
            {
                if (cmbTierart.SelectedIndex == -1 || cmbFutter.SelectedIndex == -1)
                {
                    MessageBox.Show("Bitte Tierart und Futter auswählen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int tierartId = ((ComboBoxItem)cmbTierart.SelectedItem).Value;
                int futterId = ((ComboBoxItem)cmbFutter.SelectedItem).Value;
                decimal menge = numMenge.Value;
                string zeit = mtxtZeit.Text + ":00";

                try
                {
                    // Prüfen ob schon vorhanden
                    DataTable check = db.Get(@"SELECT * FROM Tierart_Futter 
                                              WHERE tierartID=@tid AND futterID=@fid AND Fütterungszeit=@zeit",
                        ("@tid", tierartId), ("@fid", futterId), ("@zeit", zeit));

                    if (check.Rows.Count > 0)
                    {
                        // Update
                        db.Execute(@"UPDATE Tierart_Futter SET Menge_pro_Tag=@menge 
                                   WHERE tierartID=@tid AND futterID=@fid AND Fütterungszeit=@zeit",
                            ("@menge", menge), ("@tid", tierartId), ("@fid", futterId), ("@zeit", zeit));
                        MessageBox.Show("✅ Fütterungsplan aktualisiert!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Insert
                        db.Execute(@"INSERT INTO Tierart_Futter (tierartID, futterID, Menge_pro_Tag, Fütterungszeit) 
                                   VALUES (@tid, @fid, @menge, @zeit)",
                            ("@tid", tierartId), ("@fid", futterId), ("@menge", menge), ("@zeit", zeit));
                        MessageBox.Show("✅ Fütterungsplan erstellt!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadFutterplan();
                    LoadTagesbedarf();
                    dialog.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnAbbrechen.Click += (s, ev) => dialog.Close();

            // Controls hinzufügen
            dialog.Controls.AddRange(new Control[] {
                lblTierart, lblFutter, lblMenge, lblZeit, lblEinheit,
                cmbTierart, cmbFutter, numMenge, mtxtZeit,
                btnSpeichern, btnAbbrechen
            });

            dialog.ShowDialog();
        }

        #endregion

        #region TAGESBEDARF

        private void LoadTagesbedarf()
        {
            try
            {
                DataTable dt = zooDb.GetTagesbedarfProTier();
                dgvTagesbedarf.DataSource = dt;
                UpdateStatus($"✅ Tagesbedarf für {dt.Rows.Count} Tiere geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLadeTagesbedarf_Click(object sender, EventArgs e)
        {
            LoadTagesbedarf();
        }

        #endregion

        #region BESTELLUNGEN

        private void LoadBestellungen()
        {
            try
            {
                DataTable dt = zooDb.GetBestellungenMitPositionen();
                dgvBestellungen.DataSource = dt;
                UpdateStatus($"✅ {dt.Rows.Count} Bestellungen geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLadeBestellungen_Click(object sender, EventArgs e)
        {
            LoadBestellungen();
        }

        #endregion

        #region Hilfsklasse

        // Für ComboBox mit ID und Text
        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        #endregion
    }
}
