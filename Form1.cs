using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Linq;

namespace ZooApp
{
    // Hauptformular der Zoo-Verwaltung
    public partial class Form1 : Form
    {
        #region Private Felder

        // Datenbankverbindungen
        private readonly DB db = new DB();           // Basis-Datenbank-Klasse
        private readonly ZooDB zooDb = new ZooDB();  // Erweiterte Zoo-Funktionen

        // Aktuell ausgewählte IDs für die verschiedenen Entitäten
        private int currentKontinentId = 0;
        private int currentGehegeId = 0;
        private int currentTierartId = 0;
        private int currentTierId = 0;
        private int currentFutterId = 0;
        private int currentFuelId = 0;
        private int currentGoldId = 0;
        private int currentPflegerId = 0;

        #endregion

        #region Initialisierung

        // Konstruktor - wird beim Start der App aufgerufen
        public Form1()
        {
            // Initialisiert alle Designer-Controls (aus Form1.Designer.cs)
            InitializeComponent();

            // Fügt den Fütterungsplan-Button dynamisch hinzu
            AddFutterplanButton();
        }

        // Fügt dynamisch einen Button zum Erstellen von Futterplänen hinzu
        private void AddFutterplanButton()
        {
            // Erstelle neuen Button mit allen Eigenschaften
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

            // Event-Handler für Klick registrieren
            btnFutterplanNeu.Click += btnFutterplanNeu_Click;

            // Button zum Fütterungsplan-Tab hinzufügen (mit Null-Check)
            // Tab-Index 7 = tabPage8 (Fütterungsplan)
            if (tabControl1?.TabPages != null && tabControl1.TabPages.Count > 7)
            {
                tabControl1.TabPages[7].Controls.Add(btnFutterplanNeu);

                // FlatAppearance NACH dem Hinzufügen zum Container setzen
                btnFutterplanNeu.FlatAppearance.BorderSize = 0;
            }
        }

        // Wird beim Laden des Formulars aufgerufen (nach dem Anzeigen)
        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Datenbankverbindung testen
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
                // 2. Stammdaten laden (Kontinente, Gehege, Tierarten, Tiere)
                LoadKontinente();
                LoadGehege();
                LoadTierarten();
                LoadTiere();

                // 3. ComboBoxen füllen
                LoadKontinentComboBox();
                LoadTierartComboBox();
                LoadGehegeComboBox();
                LoadTierartComboBoxFutterplan();

                // 4. Übersicht und Eingabefelder initialisieren
                LoadUebersicht();
                ClearFutterFields();

                // 5. Dynamische Buttons erstellen (NACH Daten geladen)
                AddFutterplanButton();
                CreateXmlButtons();  // NEU: XML-Buttons hinzufügen

                // 6. Doppelklick-Events registrieren
                lbTiere.DoubleClick += lbTiere_DoubleClick;
                dgvUebersicht.CellDoubleClick += dgvUebersicht_CellDoubleClick;
                dgvFutter.CellDoubleClick += dgvFutter_CellDoubleClick; // NEU: Für Futter-Details

                // 7. Rechtsklick-Kontextmenüs erstellen
                CreateContextMenus();

                // 8. Alle Tabs mit Daten befüllen
                LoadFutterListe();
                LoadNachbestellung();
                LoadFutterplan();
                LoadTagesbedarf();
                LoadBestellungen();
                LoadPfleger();  // Pfleger laden

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
                    case 10: LoadPfleger(); UpdateStatus("✅ Pfleger geladen"); break;  // Pfleger-Tab
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
                lblStatus.Text = "Status: " + msg;
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

        // Erstellt Rechtsklick-Kontextmenüs für DataGridViews
        private void CreateContextMenus()
        {
            // Kontextmenü für Übersicht (Tiere)
            ContextMenuStrip menuUebersicht = new ContextMenuStrip();
            menuUebersicht.Items.Add("Details anzeigen", null, (s, e) => {
                if (dgvUebersicht.SelectedRows.Count > 0)
                    OpenTierDetailFromGrid();
            });
            menuUebersicht.Items.Add("-"); // Trennlinie
            menuUebersicht.Items.Add("Exportieren als XML", null, (s, e) => {
                BtnXmlExport_Click(null, null);
            });
            menuUebersicht.Items.Add("Auswahl kopieren", null, (s, e) => {
                CopySelectedGridData(dgvUebersicht);
            });
            dgvUebersicht.ContextMenuStrip = menuUebersicht;

            // Kontextmenü für Futterliste
            ContextMenuStrip menuFutter = new ContextMenuStrip();
            menuFutter.Items.Add("Details anzeigen", null, (s, e) => {
                if (dgvFutter.SelectedRows.Count > 0)
                    OpenFutterDetailFromGrid();
            });
            menuFutter.Items.Add("Schnellbestellung", null, (s, e) => {
                OpenSchnellBestellung();
            });
            menuFutter.Items.Add("-"); // Trennlinie
            menuFutter.Items.Add("Bearbeiten", null, (s, e) => {
                EditSelectedFutter();
            });
            menuFutter.Items.Add("Löschen", null, (s, e) => {
                DeleteSelectedFutter();
            });
            menuFutter.Items.Add("-"); // Trennlinie
            menuFutter.Items.Add("Nach oben", null, (s, e) => {
                MoveGridSelection(dgvFutter, -1);
            });
            menuFutter.Items.Add("Nach unten", null, (s, e) => {
                MoveGridSelection(dgvFutter, 1);
            });
            dgvFutter.ContextMenuStrip = menuFutter;

            // Kontextmenü für Nachbestellung
            ContextMenuStrip menuNachbestellung = new ContextMenuStrip();
            menuNachbestellung.Items.Add("Details der Futtersorte", null, (s, e) => {
                if (dgvNachbestellung.SelectedRows.Count > 0)
                    OpenFutterDetailFromNachbestellung();
            });
            menuNachbestellung.Items.Add("Bestellung erstellen", null, (s, e) => {
                CreateBestellungFromNachbestellung();
            });
            menuNachbestellung.Items.Add("-"); // Trennlinie
            menuNachbestellung.Items.Add("Als erledigt markieren", null, (s, e) => {
                MarkNachbestellungAsDone();
            });
            dgvNachbestellung.ContextMenuStrip = menuNachbestellung;

            // Kontextmenü für Bestellungen
            ContextMenuStrip menuBestellungen = new ContextMenuStrip();
            menuBestellungen.Items.Add("Details anzeigen", null, (s, e) => {
                ShowBestellungDetails();
            });
            menuBestellungen.Items.Add("Als geliefert markieren", null, (s, e) => {
                MarkAsDelivered();
            });
            menuBestellungen.Items.Add("Stornieren", null, (s, e) => {
                CancelBestellung();
            });
            dgvBestellungen.ContextMenuStrip = menuBestellungen;

            // Kontextmenü für Pfleger-ListBox
            ContextMenuStrip menuPfleger = new ContextMenuStrip();
            menuPfleger.Items.Add("Details anzeigen", null, (s, e) => {
                if (lbPfleger.SelectedItem != null)
                    OpenPflegerDetailFromList();
            });
            menuPfleger.Items.Add("Neuer Pfleger", null, (s, e) => {
                btnNewPfleger_Click(null, null);
            });
            menuPfleger.Items.Add("Bearbeiten", null, (s, e) => {
                btnEditPfleger_Click(null, null);
            });
            menuPfleger.Items.Add("Löschen", null, (s, e) => {
                btnDelPfleger_Click(null, null);
            });
            lbPfleger.ContextMenuStrip = menuPfleger;
        }

        #region Kontextmenü-Helfer
        private void OpenTierDetailFromGrid()
        {
            try
            {
                int tierID = Convert.ToInt32(dgvUebersicht.SelectedRows[0].Cells["ID"].Value);
                TierDetailForm detailForm = new TierDetailForm(tierID);
                detailForm.ShowDialog();
                LoadUebersicht();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFutterDetailFromGrid()
        {
            try
            {
                int futterID = Convert.ToInt32(dgvFutter.SelectedRows[0].Cells["futterID"].Value);
                FutterDetailForm detailForm = new FutterDetailForm(futterID);
                detailForm.ShowDialog();
                LoadFutterListe();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFutterDetailFromNachbestellung()
        {
            try
            {
                if (dgvNachbestellung.SelectedRows.Count > 0)
                {
                    var row = dgvNachbestellung.SelectedRows[0];
                    // Annahme: Spalte 'futterID' existiert oder wir parsen den Namen
                    string futterName = row.Cells["Futtersorte"].Value.ToString();

                    // Futter-ID aus Name holen
                    DataTable dt = db.Get("SELECT futterID FROM futter WHERE Bezeichnung = @name",
                        ("@name", futterName));

                    if (dt.Rows.Count > 0)
                    {
                        int futterID = Convert.ToInt32(dt.Rows[0]["futterID"]);
                        FutterDetailForm detailForm = new FutterDetailForm(futterID);
                        detailForm.ShowDialog();
                        LoadNachbestellung();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopySelectedGridData(DataGridView grid)
        {
            if (grid.SelectedCells.Count > 0)
            {
                string data = "";
                foreach (DataGridViewCell cell in grid.SelectedCells)
                {
                    if (cell.Value != null)
                        data += cell.Value.ToString() + "\t";
                }
                Clipboard.SetText(data);
                UpdateStatus("✅ Daten kopiert");
            }
        }

        private void MoveGridSelection(DataGridView grid, int direction)
        {
            if (grid.SelectedRows.Count > 0 && direction != 0)
            {
                int newIndex = grid.SelectedRows[0].Index + direction;
                if (newIndex >= 0 && newIndex < grid.Rows.Count)
                {
                    grid.ClearSelection();
                    grid.Rows[newIndex].Selected = true;
                    grid.FirstDisplayedScrollingRowIndex = newIndex;
                }
            }
        }

        private void OpenSchnellBestellung()
        {
            if (dgvFutter.SelectedRows.Count == 0) return;

            var row = dgvFutter.SelectedRows[0];
            string bezeichnung = row.Cells["Bezeichnung"].Value.ToString();
            decimal preis = Convert.ToDecimal(row.Cells["Preis_pro_Einheit"].Value);
            string einheit = row.Cells["Einheit"].Value.ToString();
            decimal bestellmenge = Convert.ToDecimal(row.Cells["Bestellmenge"].Value);

            using (var dialog = new Form
            {
                Text = "🛒 Schnellbestellung",
                Size = new Size(400, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            })
            {
                Label lbl = new Label { Text = $"Bestellung für:\n{bezeichnung}", Left = 20, Top = 20, AutoSize = true };
                NumericUpDown num = new NumericUpDown
                {
                    Left = 20,
                    Top = 70,
                    Width = 150,
                    Minimum = 1,
                    Maximum = 10000,
                    Value = bestellmenge
                };
                Label lblSumme = new Label { Text = $"Summe: {(bestellmenge * preis):C2}", Left = 20, Top = 110, AutoSize = true };

                num.ValueChanged += (s, e) =>
                    lblSumme.Text = $"Summe: {(num.Value * preis):C2}";

                Button btnOk = new Button { Text = "✅ Bestellen", Left = 20, Top = 160, Width = 150, DialogResult = DialogResult.OK };
                Button btnCancel = new Button { Text = "❌ Abbrechen", Left = 180, Top = 160, Width = 150, DialogResult = DialogResult.Cancel };

                dialog.Controls.AddRange(new Control[] { lbl, num, lblSumme, btnOk, btnCancel });

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    decimal menge = num.Value;
                    // Hier könnte die Bestellung in DB gespeichert werden
                    MessageBox.Show($"Bestellung für {menge} {einheit} {bezeichnung} aufgegeben!",
                        "Bestellung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EditSelectedFutter()
        {
            if (dgvFutter.SelectedRows.Count > 0)
            {
                // Futter in die Eingabefelder laden
                var row = dgvFutter.SelectedRows[0];
                currentFutterId = Convert.ToInt32(row.Cells["futterID"].Value);
                txtFutterBezeichnung.Text = row.Cells["Bezeichnung"].Value.ToString();
                txtFutterEinheit.Text = row.Cells["Einheit"].Value.ToString();
                numFutterPreis.Value = Convert.ToDecimal(row.Cells["Preis_pro_Einheit"].Value);
                numFutterLagerbestand.Value = Convert.ToInt32(row.Cells["Lagerbestand"].Value);
                numFutterMindestbestand.Value = Convert.ToInt32(row.Cells["Mindestbestand"].Value);
                numFutterBestellmenge.Value = Convert.ToInt32(row.Cells["Bestellmenge"].Value);

                UpdateStatus($"Futter '{txtFutterBezeichnung.Text}' zum Bearbeiten geladen");
            }
        }

        private void DeleteSelectedFutter()
        {
            if (dgvFutter.SelectedRows.Count > 0)
            {
                string name = dgvFutter.SelectedRows[0].Cells["Bezeichnung"].Value.ToString();
                if (MessageBox.Show($"Futtersorte '{name}' wirklich löschen?", "Löschen bestätigen",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnFutterLöschen_Click(null, null);
                }
            }
        }

        private void CreateBestellungFromNachbestellung()
        {
            if (dgvNachbestellung.SelectedRows.Count > 0)
            {
                var row = dgvNachbestellung.SelectedRows[0];
                string futterName = row.Cells["Futtersorte"].Value.ToString();
                decimal fehlendeMenge = Convert.ToDecimal(row.Cells["Fehlende_Menge"].Value);

                MessageBox.Show($"Bestellung für {fehlendeMenge} {futterName} würde jetzt erstellt.\n\n(Funktion kann erweitert werden)",
                    "Bestellung", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MarkNachbestellungAsDone()
        {
            if (dgvNachbestellung.SelectedRows.Count > 0)
            {
                // Hier könnte der Bestand manuell erhöht werden
                UpdateStatus("Nachbestellung als erledigt markiert (Demo)");
            }
        }

        private void ShowBestellungDetails()
        {
            if (dgvBestellungen.SelectedRows.Count > 0)
            {
                var row = dgvBestellungen.SelectedRows[0];
                string details = "";
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                        details += $"{dgvBestellungen.Columns[cell.ColumnIndex].HeaderText}: {cell.Value}\n";
                }
                MessageBox.Show(details, "Bestellungsdetails", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MarkAsDelivered()
        {
            if (dgvBestellungen.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Bestellung als geliefert markieren?", "Bestätigen",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateStatus("Bestellung als geliefert markiert (Demo)");
                }
            }
        }

        private void CancelBestellung()
        {
            if (dgvBestellungen.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Bestellung wirklich stornieren?", "Bestätigen",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    UpdateStatus("Bestellung storniert (Demo)");
                }
            }
        }

        private void OpenPflegerDetailFromList()
        {
            if (lbPfleger.SelectedItem != null)
                lbPfleger_DoubleClick(null, null);
        }
        #endregion

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

        // Doppelklick auf Tier öffnet Detail-Fenster
        private void lbTiere_DoubleClick(object sender, EventArgs e)
        {
            // Prüfen ob ein Tier ausgewählt ist
            if (lbTiere.SelectedItem == null)
            {
                MessageBox.Show("Bitte wähle zuerst ein Tier aus.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // TierID aus dem ausgewählten Eintrag extrahieren
                int tierID = int.Parse(lbTiere.SelectedItem.ToString().Split('-')[0].Trim());

                // Detail-Fenster öffnen
                TierDetailForm detailForm = new TierDetailForm(tierID);
                detailForm.ShowDialog();

                // Nach dem Schließen die Listen neu laden
                LoadTiere();
                LoadUebersicht();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Öffnen des Detail-Fensters:\n{ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ÜBERSICHT

        /// <summary>
        /// Lädt komplette Tier-Übersicht mit allen Details.
        /// Zeigt alle Tiere mit Stammdaten in der DataGridView an.
        /// </summary>
        private void LoadUebersicht()
        {
            // Sicherheitscheck: Ist das DataGridView überhaupt vorhanden?
            if (dgvUebersicht == null)
            {
                Console.WriteLine("FEHLER: dgvUebersicht ist null!");
                return;
            }

            try
            {
                // SQL-Abfrage vorbereiten
                string sql = @"
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
                    FROM tiere t
                    LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                    LEFT JOIN Gehege g ON t.GehegeID = g.gID
                    LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                    ORDER BY t.Name";

                Console.WriteLine("LoadUebersicht: Starte DB-Abfrage...");

                // Daten aus Datenbank laden
                DataTable dt = db.Get(sql);
                Console.WriteLine($"LoadUebersicht: {dt.Rows.Count} Zeilen geladen");

                // Event-Handler VORHER entfernen (verhindert Events während des Ladens)
                dgvUebersicht.CellDoubleClick -= dgvUebersicht_CellDoubleClick;

                // Layout-Updates während der Aktualisierung suspendieren (Performance + Stabilität)
                dgvUebersicht.SuspendLayout();

                try
                {
                    // DataGridView mit Daten füllen
                    dgvUebersicht.DataSource = dt;
                    Console.WriteLine("LoadUebersicht: DataSource gesetzt");

                    // AutoSizeColumnsMode setzen
                    // WICHTIG: Bei Fill-Modus sollte Width NICHT manuell gesetzt werden!
                    dgvUebersicht.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    Console.WriteLine("LoadUebersicht: AutoSizeColumnsMode gesetzt");
                }
                finally
                {
                    // Layout-Updates wieder aktivieren (immer ausführen, auch bei Fehler)
                    dgvUebersicht.ResumeLayout();
                    Console.WriteLine("LoadUebersicht: ResumeLayout abgeschlossen");
                }

                // GUI-Update abwarten
                Application.DoEvents();
                Console.WriteLine("LoadUebersicht: DoEvents abgeschlossen");

                // Spalten-Einstellungen vornehmen (nur ReadOnly, KEINE Width-Änderungen!)
                if (dgvUebersicht.Columns != null && dgvUebersicht.Columns.Count > 0)
                {
                    Console.WriteLine($"LoadUebersicht: {dgvUebersicht.Columns.Count} Spalten vorhanden");

                    if (dgvUebersicht.Columns.Contains("ID"))
                    {
                        // Spalte existiert und ist vollständig initialisiert
                        var idColumn = dgvUebersicht.Columns["ID"];

                        if (idColumn != null)
                        {
                            // WICHTIG: Bei AutoSizeColumnsMode.Fill keine Width setzen!
                            // Stattdessen FillWeight verwenden für relative Größe
                            idColumn.FillWeight = 10;  // ID-Spalte bekommt weniger Platz (10% statt Standard 100%)
                            idColumn.ReadOnly = true;
                            Console.WriteLine("LoadUebersicht: ID-Spalte konfiguriert (FillWeight=10)");
                        }
                    }
                    else
                    {
                        Console.WriteLine("WARNUNG: ID-Spalte nicht gefunden!");
                    }

                    // Optional: Andere Spalten auch mit FillWeight konfigurieren
                    if (dgvUebersicht.Columns.Contains("Name"))
                    {
                        dgvUebersicht.Columns["Name"].FillWeight = 120;  // Name bekommt mehr Platz
                    }
                }
                else
                {
                    Console.WriteLine("WARNUNG: Keine Spalten vorhanden!");
                }

                // Event-Handler NACH der vollständigen Initialisierung registrieren
                try
                {
                    Console.WriteLine("LoadUebersicht: Registriere Event-Handler...");
                    dgvUebersicht.CellDoubleClick += dgvUebersicht_CellDoubleClick;
                    Console.WriteLine("LoadUebersicht: Event-Handler hinzugefügt");
                }
                catch (Exception eventEx)
                {
                    Console.WriteLine($"FEHLER beim Event-Handler: {eventEx.Message}");
                    // Nicht weitergeben - das ist kein kritischer Fehler
                }

                // Status aktualisieren
                UpdateStatus($"✅ {dt.Rows.Count} Tiere - Doppelklick für Details");
                Console.WriteLine("LoadUebersicht: Erfolgreich abgeschlossen");
            }
            catch (Exception ex)
            {
                // Detaillierte Fehlerausgabe
                string errorMessage = $"Fehler in LoadUebersicht():\n" +
                                     $"Message: {ex.Message}\n" +
                                     $"Source: {ex.Source}\n" +
                                     $"StackTrace:\n{ex.StackTrace}";

                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nInner Exception:\n{ex.InnerException.Message}";
                }

                Console.WriteLine($"FEHLER: {errorMessage}");

                MessageBox.Show($"Fehler beim Laden der Übersicht:\n\n{ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // UpdateStatus nur aufrufen wenn lblStatus existiert
                try
                {
                    UpdateStatus("❌ Fehler beim Laden");
                }
                catch
                {
                    Console.WriteLine("Konnte Status nicht aktualisieren (lblStatus = null?)");
                }
            }
        }

        /// <summary>
        /// Event-Handler für Doppelklick auf eine Zeile in der Übersicht.
        /// Öffnet das Detail-Fenster für das ausgewählte Tier.
        /// </summary>
        private void dgvUebersicht_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"CellDoubleClick: Ereignis ausgelöst für Zeile {e.RowIndex}, Spalte {e.ColumnIndex}");

            // Prüfen ob eine gültige Zeile angeklickt wurde (nicht Header)
            if (e.RowIndex < 0)
            {
                Console.WriteLine("CellDoubleClick: Header-Zeile angeklickt, ignoriere...");
                return;
            }

            try
            {
                Console.WriteLine("CellDoubleClick: Prüfe Columns...");

                // Prüfen ob Columns-Collection existiert
                if (dgvUebersicht.Columns == null)
                {
                    MessageBox.Show("Fehler: Keine Spalten vorhanden!", "Fehler",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("FEHLER: dgvUebersicht.Columns ist null!");
                    return;
                }

                // Prüfen ob ID-Spalte existiert
                if (!dgvUebersicht.Columns.Contains("ID"))
                {
                    MessageBox.Show("Fehler: ID-Spalte nicht gefunden!", "Fehler",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("FEHLER: ID-Spalte existiert nicht!");
                    return;
                }

                Console.WriteLine("CellDoubleClick: Hole Zellenwert...");

                // Zellenwert holen
                var cellValue = dgvUebersicht.Rows[e.RowIndex].Cells["ID"].Value;

                // Prüfen ob Wert vorhanden
                if (cellValue == null || cellValue == DBNull.Value)
                {
                    MessageBox.Show("Fehler: Keine Tier-ID gefunden!", "Fehler",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"FEHLER: Zellenwert ist null oder DBNull bei Zeile {e.RowIndex}");
                    return;
                }

                // TierID extrahieren
                int tierID = Convert.ToInt32(cellValue);
                Console.WriteLine($"CellDoubleClick: Öffne Detail-Fenster für TierID {tierID}");

                // Detail-Fenster öffnen
                TierDetailForm detailForm = new TierDetailForm(tierID);
                detailForm.ShowDialog();

                Console.WriteLine("CellDoubleClick: Detail-Fenster geschlossen, lade Daten neu...");

                // Daten nach dem Schließen neu laden
                LoadUebersicht();
                LoadTiere();

                Console.WriteLine("CellDoubleClick: Erfolgreich abgeschlossen");
            }
            catch (Exception ex)
            {
                // Detaillierte Fehlerausgabe
                string errorMessage = $"Fehler in dgvUebersicht_CellDoubleClick():\n" +
                                     $"Message: {ex.Message}\n" +
                                     $"Source: {ex.Source}\n" +
                                     $"StackTrace:\n{ex.StackTrace}";

                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nInner Exception:\n{ex.InnerException.Message}";
                }

                Console.WriteLine($"FEHLER: {errorMessage}");

                MessageBox.Show($"Fehler beim Öffnen des Detail-Fensters:\n\n{ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Direktes Bearbeiten von Name und Gewicht
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

        // Doppelklick auf Futtersorte öffnet Detail-Ansicht
        private void dgvFutter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"Futter-Doppelklick: Zeile {e.RowIndex}, Spalte {e.ColumnIndex}");

            // Prüfen ob Header oder ungültige Zeile angeklickt
            if (e.RowIndex < 0)
            {
                Console.WriteLine("Futter-Doppelklick: Header-Zeile - ignoriere");
                return;
            }

            try
            {

                // ID aus der ausgewählten Zeile holen
                var row = dgvFutter.Rows[e.RowIndex];
                var cellValue = row.Cells["futterID"].Value;

                if (cellValue == null || cellValue == DBNull.Value)
                {
                    MessageBox.Show("Keine gültige Futtersorte ausgewählt!",
                                  "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ID parsen
                int futterID = Convert.ToInt32(cellValue);
                Console.WriteLine($"Öffne Futter-Detail für ID: {futterID}");

                // Detail-Fenster öffnen (Modal = Dialog)
                FutterDetailForm detailForm = new FutterDetailForm(futterID);
                DialogResult result = detailForm.ShowDialog(this); // 'this' = Hauptform als Owner

                // Nach dem Schließen Daten NEU laden
                if (result == DialogResult.OK || result == DialogResult.Cancel)
                {
                    LoadFutterListe();
                    LoadNachbestellung(); // Wichtig: Nachbestell-Liste aktualisieren
                    UpdateStatus($"✅ Futter-Details geschlossen - Daten aktualisiert");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Öffnen der Futter-Details:\n{ex.Message}",
                               "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"FEHLER Futter-Doppelklick: {ex.Message}\n{ex.StackTrace}");
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

        // Button-Click-Handler für manuelle Detail-Ansicht
        private void BtnFutterDetails_Click(object sender, EventArgs e)
        {
            if (dgvFutter.SelectedRows.Count == 0)
            {
                MessageBox.Show("Bitte wähle zuerst eine Futtersorte aus!", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int futterID = Convert.ToInt32(dgvFutter.SelectedRows[0].Cells["futterID"].Value);
                FutterDetailForm detailForm = new FutterDetailForm(futterID);
                detailForm.ShowDialog();

                // Nach dem Schließen neu laden
                LoadFutterListe();
                LoadNachbestellung();
                UpdateStatus("✅ Futter-Details geschlossen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            tf.Menge_pro_Tag, f.Einheit, tf.Fuetterungszeit,
                            CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit, ' um ', tf.Fuetterungszeit) AS Fütterungsplan
                        FROM Tierart_Futter tf
                        JOIN Tierart ta ON tf.tierartID = ta.tierartID
                        JOIN Futter f ON tf.futterID = f.futterID
                        ORDER BY ta.TABezeichnung, tf.Fuetterungszeit";

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

            Label lblTierart = new Label { Text = "Tierart:", Left = 20, Top = 20, Width = 100 };
            Label lblFutter = new Label { Text = "Futter:", Left = 20, Top = 60, Width = 100 };
            Label lblMenge = new Label { Text = "Menge pro Tag:", Left = 20, Top = 100, Width = 100 };
            Label lblZeit = new Label { Text = "Fütterungszeit:", Left = 20, Top = 140, Width = 100 };
            Label lblEinheit = new Label { Text = "(kg)", Left = 350, Top = 100, Width = 50 };

            ComboBox cmbTierart = new ComboBox { Left = 130, Top = 17, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            ComboBox cmbFutter = new ComboBox { Left = 130, Top = 57, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            NumericUpDown numMenge = new NumericUpDown { Left = 130, Top = 97, Width = 210, DecimalPlaces = 2, Minimum = 0.01m, Maximum = 1000, Value = 5, Increment = 0.5m };
            MaskedTextBox mtxtZeit = new MaskedTextBox { Left = 130, Top = 137, Width = 100, Mask = "00:00", Text = "08:00" };

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
                    DataTable check = db.Get(@"SELECT * FROM Tierart_Futter 
                                              WHERE tierartID=@tid AND futterID=@fid AND Fütterungszeit=@zeit",
                        ("@tid", tierartId), ("@fid", futterId), ("@zeit", zeit));

                    if (check.Rows.Count > 0)
                    {
                        db.Execute(@"UPDATE Tierart_Futter SET Menge_pro_Tag=@menge 
                                   WHERE tierartID=@tid AND futterID=@fid AND Fütterungszeit=@zeit",
                            ("@menge", menge), ("@tid", tierartId), ("@fid", futterId), ("@zeit", zeit));
                        MessageBox.Show("✅ Fütterungsplan aktualisiert!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
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

        #region PFLEGER

        /// <summary>
        /// Lädt alle Pfleger aus der Datenbank in die ListBox
        /// Zeigt: ID, Name, Gehalt und Einstellungsdatum
        /// </summary>
        private void LoadPfleger()
        {
            try
            {
                // SQL: Alle Pfleger sortiert nach Nachname, Vorname
                DataTable dt = db.Get(@"
                    SELECT pflegerID, Vorname, Nachname, Gehalt, Einstellungsdatum
                    FROM pfleger
                    ORDER BY Nachname, Vorname");

                // ListBox leeren
                lbPfleger.Items.Clear();
                
                // Jeden Pfleger als Eintrag hinzufügen
                foreach (DataRow row in dt.Rows)
                {
                    string eintrag = $"{row["pflegerID"]} - {row["Nachname"]}, {row["Vorname"]} " +
                                   $"(Gehalt: {Convert.ToDecimal(row["Gehalt"]):C}, seit {Convert.ToDateTime(row["Einstellungsdatum"]):dd.MM.yyyy})";
                    lbPfleger.Items.Add(eintrag);
                }

                // Statusleiste aktualisieren
                UpdateStatus($"✅ {dt.Rows.Count} Pfleger geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Pfleger:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn ein Pfleger in der ListBox ausgewählt wird
        /// Speichert die aktuelle Pfleger-ID
        /// </summary>
        private void lbPfleger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPfleger.SelectedItem == null) return;

            try
            {
                // ID aus dem String extrahieren (Format: "37 - Müller, Hans...")
                currentPflegerId = int.Parse(lbPfleger.SelectedItem.ToString().Split('-')[0].Trim());
            }
            catch
            {
                currentPflegerId = 0;
            }
        }

        /// <summary>
        /// Doppelklick auf einen Pfleger öffnet das Detail-Fenster
        /// Erlaubt schnelle Bearbeitung ohne extra Button
        /// </summary>
        private void lbPfleger_DoubleClick(object sender, EventArgs e)
        {
            if (lbPfleger.SelectedItem == null)
            {
                MessageBox.Show("Bitte zuerst einen Pfleger auswählen.", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // ID aus dem String extrahieren
                int pflegerID = int.Parse(lbPfleger.SelectedItem.ToString().Split('-')[0].Trim());
                
                // Detail-Fenster öffnen
                PflegerDetailForm detailForm = new PflegerDetailForm(pflegerID);
                detailForm.ShowDialog();
                
                // Liste nach Änderungen neu laden
                LoadPfleger();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Button "Neu" - Erstellt einen neuen Pfleger
        /// Fügt Dummy-Eintrag ein und öffnet Detail-Fenster zum Bearbeiten
        /// </summary>
        private void btnNewPfleger_Click(object sender, EventArgs e)
        {
            try
            {
                // Neuen Pfleger mit Standardwerten in DB einfügen
                db.Execute(@"
                    INSERT INTO pfleger (Vorname, Nachname, Einstellungsdatum, Gehalt)
                    VALUES ('Neuer', 'Pfleger', CURDATE(), 2500.00)");

                // ID des neu erstellten Pflegers holen (LAST_INSERT_ID)
                DataTable dt = db.Get("SELECT LAST_INSERT_ID() as id");
                int neueID = Convert.ToInt32(dt.Rows[0]["id"]);

                // Detail-Fenster öffnen zum Bearbeiten
                PflegerDetailForm detailForm = new PflegerDetailForm(neueID);
                if (detailForm.ShowDialog() == DialogResult.OK)
                {
                    // Bei OK: Liste neu laden
                    LoadPfleger();
                }
                else
                {
                    // Bei Abbruch: Leeren Eintrag wieder löschen
                    db.Execute("DELETE FROM pfleger WHERE pflegerID = @id", ("@id", neueID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Button "Bearbeiten" - Öffnet Detail-Fenster für ausgewählten Pfleger
        /// Prüft zuerst, ob ein Pfleger ausgewählt ist
        /// </summary>
        private void btnEditPfleger_Click(object sender, EventArgs e)
        {
            // Prüfen ob Pfleger ausgewählt
            if (currentPflegerId == 0)
            {
                MessageBox.Show("Bitte zuerst einen Pfleger auswählen!", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Detail-Fenster öffnen
                PflegerDetailForm detailForm = new PflegerDetailForm(currentPflegerId);
                detailForm.ShowDialog();
                
                // Liste nach Änderungen neu laden
                LoadPfleger();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Button "Löschen" - Löscht ausgewählten Pfleger
        /// Fragt vorher zur Sicherheit nach Bestätigung
        /// </summary>
        private void btnDelPfleger_Click(object sender, EventArgs e)
        {
            // Prüfen ob Pfleger ausgewählt
            if (currentPflegerId == 0)
            {
                MessageBox.Show("Bitte zuerst einen Pfleger auswählen!", "Hinweis",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Name des Pflegers für Bestätigung holen
                DataTable dt = db.Get("SELECT Vorname, Nachname FROM pfleger WHERE pflegerID = @id",
                    ("@id", currentPflegerId));

                if (dt.Rows.Count == 0) return;

                string name = $"{dt.Rows[0]["Vorname"]} {dt.Rows[0]["Nachname"]}";

                // Sicherheitsabfrage vor dem Löschen
                if (MessageBox.Show($"Pfleger '{name}' wirklich löschen?", "Löschen bestätigen",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Aus Datenbank löschen
                    db.Execute("DELETE FROM pfleger WHERE pflegerID = @id", ("@id", currentPflegerId));
                    
                    // Liste neu laden
                    LoadPfleger();
                    currentPflegerId = 0;
                    
                    MessageBox.Show("✅ Pfleger gelöscht!", "Erfolg",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Button "Aktualisieren" - Lädt Pfleger-Liste neu
        /// Nützlich wenn andere Benutzer Änderungen vorgenommen haben
        /// </summary>
        private void btnRefreshPfleger_Click(object sender, EventArgs e)
        {
            LoadPfleger();
        }

        #endregion

        #region XML-Funktionen

        // Erstellt XML-Export/Import Buttons unter der DataGridView
        private void CreateXmlButtons()
        {
            try
            {
                // 1. Panel für die Buttons erstellen (für besseres Layout)
                Panel buttonPanel = new Panel
                {
                    Height = 60,
                    Dock = DockStyle.Bottom,  // Unten im Tab fixieren
                    BackColor = Color.FromArgb(240, 240, 240),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 2. Export-Button auf dem Panel
                Button exportBtn = new Button
                {
                    Text = "📤 XML Export",
                    Left = 20,
                    Top = 10,
                    Width = 140,
                    Height = 40,
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Name = "btnXmlExport"
                };
                exportBtn.FlatAppearance.BorderSize = 0;
                exportBtn.Click += BtnXmlExport_Click;

                // 3. Import-Button auf dem Panel
                Button importBtn = new Button
                {
                    Text = "📥 XML Import",
                    Left = 180,
                    Top = 10,
                    Width = 140,
                    Height = 40,
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Name = "btnXmlImport"
                };
                importBtn.FlatAppearance.BorderSize = 0;
                importBtn.Click += BtnXmlImport_Click;

                // 4. Buttons zum Panel hinzufügen
                buttonPanel.Controls.Add(exportBtn);
                buttonPanel.Controls.Add(importBtn);

                // 5. Panel zum Übersicht-Tab hinzufügen
                if (tabControl1.TabPages.Count > 4)  // Index 4 = Übersicht
                {
                    // DataGridView anpassen (höher, damit Platz für Panel bleibt)
                    if (dgvUebersicht != null)
                    {
                        dgvUebersicht.Dock = DockStyle.Fill;  // Füllt den gesamten verfügbaren Platz
                        dgvUebersicht.Height -= buttonPanel.Height;
                    }

                    // Panel zum Tab hinzufügen
                    tabControl1.TabPages[4].Controls.Add(buttonPanel);
                    buttonPanel.BringToFront();  // Immer sichtbar
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler XML-Buttons: {ex.Message}");
            }
        }

        // XML Export Funktion
        private void BtnXmlExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
                    saveDialog.FileName = $"Zoo_Tiere_{DateTime.Now:yyyy-MM-dd}.xml";
                    saveDialog.Title = "Tierdaten exportieren";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveDialog.FileName;

                        // Daten aus DB holen
                        string sql = @"
                            SELECT t.tierID, t.Name, t.Gewicht, t.Geschlecht, 
                                   ta.TABezeichnung AS Tierart, g.GBezeichnung AS Gehege,
                                   DATE_FORMAT(t.Geburtsdatum, '%Y-%m-%d') AS Geburtsdatum,
                                   t.Notizen
                            FROM tiere t
                            LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                            LEFT JOIN Gehege g ON t.GehegeID = g.gID
                            WHERE t.Status = 'Aktiv'";

                        DataTable dt = db.Get(sql);

                        // XML erstellen
                        XmlDocument xmlDoc = new XmlDocument();
                        XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        xmlDoc.AppendChild(xmlDecl);

                        XmlElement root = xmlDoc.CreateElement("ZooTiere");
                        root.SetAttribute("ExportDatum", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        root.SetAttribute("AnzahlTiere", dt.Rows.Count.ToString());
                        xmlDoc.AppendChild(root);

                        foreach (DataRow row in dt.Rows)
                        {
                            XmlElement tier = xmlDoc.CreateElement("Tier");

                            AddXmlElement(xmlDoc, tier, "ID", row["tierID"]);
                            AddXmlElement(xmlDoc, tier, "Name", row["Name"]);
                            AddXmlElement(xmlDoc, tier, "Gewicht", row["Gewicht"], "kg");
                            AddXmlElement(xmlDoc, tier, "Tierart", row["Tierart"]);
                            AddXmlElement(xmlDoc, tier, "Gehege", row["Gehege"]);
                            AddXmlElement(xmlDoc, tier, "Geschlecht", row["Geschlecht"]);
                            AddXmlElement(xmlDoc, tier, "Geburtsdatum", row["Geburtsdatum"]);

                            // Notizen als optionales Element
                            if (!string.IsNullOrEmpty(row["Notizen"]?.ToString()))
                            {
                                AddXmlElement(xmlDoc, tier, "Notizen", row["Notizen"]);
                            }

                            root.AppendChild(tier);
                        }

                        // XML speichern
                        xmlDoc.Save(filePath);

                        MessageBox.Show($"✅ {dt.Rows.Count} aktive Tiere exportiert!\n\nDatei: {filePath}",
                            "Export erfolgreich", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Export fehlgeschlagen:\n{ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // XML Import Funktion
        private void BtnXmlImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openDialog = new OpenFileDialog())
                {
                    openDialog.Filter = "XML-Dateien (*.xml)|*.xml|Alle Dateien (*.*)|*.*";
                    openDialog.Title = "Tierdaten importieren";

                    if (openDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openDialog.FileName;

                        if (MessageBox.Show("Möchten Sie Tierdaten aus der XML-Datei importieren?\n\n" +
                                          "Hinweis: Diese Funktion importiert nur Daten, erstellt aber keine Tiere in der Datenbank.",
                                          "Import bestätigen",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // XML-Datei laden
                            string xmlContent = File.ReadAllText(filePath);

                            // Einfache Validierung
                            if (xmlContent.Contains("ZooTiere") && xmlContent.Contains("Tier"))
                            {
                                // Zeige Vorschau
                                int anzahlTiere = xmlContent.Split(new string[] { "<Tier>" }, StringSplitOptions.None).Length - 1;

                                MessageBox.Show($"✅ XML-Datei geladen!\n\n" +
                                              $"Enthält: {anzahlTiere} Tier-Datensätze\n" +
                                              $"Datei: {Path.GetFileName(filePath)}\n\n" +
                                              "Die Import-Funktion kann hier erweitert werden,\num tatsächlich Tiere in die Datenbank einzufügen.",
                                              "Import erfolgreich",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("❌ Ungültiges XML-Format!\n\n" +
                                              "Die Datei scheint keine gültigen Zoo-Tierdaten zu enthalten.",
                                              "Import-Fehler",
                                              MessageBoxButtons.OK,
                                              MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Import fehlgeschlagen:\n{ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hilfsmethode für XML-Elemente
        private void AddXmlElement(XmlDocument doc, XmlElement parent, string name, object value, string unit = null)
        {
            if (value != null && value != DBNull.Value && !string.IsNullOrEmpty(value.ToString()))
            {
                XmlElement elem = doc.CreateElement(name);
                if (!string.IsNullOrEmpty(unit))
                {
                    elem.SetAttribute("Einheit", unit);
                }
                elem.InnerText = value.ToString();
                parent.AppendChild(elem);
            }
        }

        #endregion

        #region Hilfsklasse

        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        #endregion
    }
}