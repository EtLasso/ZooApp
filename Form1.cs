using System.Data;
using System.Windows.Forms;

namespace ZooApp
{
    /// <summary>
    /// Hauptformular der Zoo-Verwaltungs-Anwendung
    /// Verwaltet Kontinente, Gehege, Tierarten, Tiere und Futter
    /// </summary>
    public partial class Form1 : Form
    {
        #region Private Felder - Datenbank und aktuelle IDs

        // Datenbankverbindungen
        private readonly DB db = new DB();
        private readonly ZooDB zooDb = new ZooDB();

        // Aktuell ausgewählte IDs für jeden Bereich
        private int currentKontinentId = 0;
        private int currentGehegeId = 0;
        private int currentTierartId = 0;
        private int currentTierId = 0;
        private int currentFutterId = 0;

        #endregion

        #region Konstruktor und Initialisierung

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Wird beim Laden des Formulars aufgerufen
        /// Testet die Datenbankverbindung und lädt alle initialen Daten
        /// </summary>
        private void Form1_Load(object sender, System.EventArgs e)
        {
            // Verbindung zur Datenbank testen
            if (!db.Test())
            {
                MessageBox.Show(
                    "❌ Keine Verbindung zur Datenbank!\n\n" +
                    "Bitte XAMPP starten und sicherstellen, dass MySQL läuft.",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                UpdateStatus("❌ Datenbank nicht verbunden");
                return;
            }

            UpdateStatus("✅ Verbunden mit Datenbank");

            try
            {
                // Alle Stammdaten laden
                LoadKontinente();
                LoadGehege();
                LoadTierarten();
                LoadTiere();

                // ComboBoxen für Dropdowns füllen
                LoadKontinentComboBox();
                LoadTierartComboBox();
                LoadGehegeComboBox();

                // Übersichtstabelle laden
                LoadUebersicht();

                // Futter-Tab initialisieren
                LoadTierartComboBoxFutterplan();
                ClearFutterFields();

                UpdateStatus("✅ Alle Daten geladen");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Aktualisiert die Statusleiste mit einer neuen Nachricht
        /// </summary>
        /// <param name="msg">Anzuzeigende Statusnachricht</param>
        private void UpdateStatus(string msg)
        {
            lblStatus.Text = msg;
        }

        #endregion

        #region Hilfsmethoden - ListBox und ComboBox füllen

        /// <summary>
        /// Füllt eine ListBox mit Daten aus einer DataTable
        /// Format: "ID - Text"
        /// </summary>
        /// <param name="box">Zu füllende ListBox</param>
        /// <param name="dt">Datenquelle (DataTable)</param>
        /// <param name="idCol">Name der ID-Spalte</param>
        /// <param name="textCol">Name der Text-Spalte</param>
        /// <param name="postfix">Optionaler Text am Ende (z.B. " (aktiv)")</param>
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

        /// <summary>
        /// Füllt eine ComboBox mit ComboBoxItem-Objekten
        /// Ermöglicht das Speichern von ID und Text in einem Dropdown
        /// </summary>
        /// <param name="box">Zu füllende ComboBox</param>
        /// <param name="dt">Datenquelle (DataTable)</param>
        /// <param name="idCol">Name der ID-Spalte</param>
        /// <param name="textCol">Name der Text-Spalte</param>
        private void FillComboBox(ComboBox box, DataTable dt, string idCol, string textCol)
        {
            box.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                box.Items.Add(new ComboBoxItem
                {
                    Value = System.Convert.ToInt32(row[idCol]),
                    Text = row[textCol].ToString()
                });
            }
            box.DisplayMember = "Text";
            box.ValueMember = "Value";
        }

        #endregion

        #region KONTINENTE - Verwaltung von Kontinenten

        /// <summary>
        /// Lädt alle Kontinente aus der Datenbank und zeigt sie in der ListBox an
        /// </summary>
        private void LoadKontinente()
        {
            DataTable dt = db.Get("SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung");
            FillListBox(lbKontinent, dt, "kID", "Kbezeichnung");
        }

        /// <summary>
        /// Lädt Kontinente in die ComboBox (für Gehege-Zuordnung)
        /// </summary>
        private void LoadKontinentComboBox()
        {
            DataTable dt = db.Get("SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung");
            FillComboBox(cmbKontinentGehege, dt, "kID", "Kbezeichnung");
        }

        /// <summary>
        /// Leert die Eingabefelder für Kontinente
        /// Setzt die aktuelle ID zurück
        /// </summary>
        private void ClearKontinentFields()
        {
            txtKBezeichnung.Text = "";
            currentKontinentId = 0;
        }

        /// <summary>
        /// Event: Button "Neu" für Kontinente
        /// Bereitet die Eingabe eines neuen Kontinents vor
        /// </summary>
        private void btnNewKontinent_Click(object sender, System.EventArgs e)
        {
            ClearKontinentFields();
        }

        /// <summary>
        /// Event: Button "Speichern" für Kontinente
        /// Erstellt einen neuen Kontinent oder aktualisiert einen bestehenden
        /// </summary>
        private void btnSaveKontinent_Click(object sender, System.EventArgs e)
        {
            // Validierung: Bezeichnung muss ausgefüllt sein
            if (txtKBezeichnung.Text == "")
            {
                MessageBox.Show("Bitte Bezeichnung eingeben.");
                return;
            }

            // Neuen Kontinent anlegen oder bestehenden aktualisieren
            if (currentKontinentId == 0)
            {
                // INSERT: Neuer Kontinent
                db.Execute("INSERT INTO Kontinent (Kbezeichnung) VALUES (@p)",
                    ("@p", txtKBezeichnung.Text));
            }
            else
            {
                // UPDATE: Bestehender Kontinent
                db.Execute("UPDATE Kontinent SET Kbezeichnung=@p WHERE kID=@id",
                    ("@p", txtKBezeichnung.Text),
                    ("@id", currentKontinentId));
            }

            // Listen neu laden und Felder leeren
            LoadKontinente();
            LoadKontinentComboBox();
            ClearKontinentFields();
        }

        /// <summary>
        /// Event: Button "Löschen" für Kontinente
        /// Löscht den ausgewählten Kontinent nach Bestätigung
        /// </summary>
        private void btnDelKontinent_Click(object sender, System.EventArgs e)
        {
            if (currentKontinentId == 0) return;

            // Sicherheitsabfrage
            if (MessageBox.Show("Kontinent wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Kontinent WHERE kID=@id",
                    ("@id", currentKontinentId));
                LoadKontinente();
                LoadKontinentComboBox();
                ClearKontinentFields();
            }
        }

        /// <summary>
        /// Event: Kontinent in ListBox wurde ausgewählt
        /// Lädt die Details des ausgewählten Kontinents in die Eingabefelder
        /// </summary>
        private void lbKontinent_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbKontinent.SelectedItem == null) return;

            // ID aus dem Format "ID - Bezeichnung" extrahieren
            currentKontinentId = int.Parse(lbKontinent.SelectedItem.ToString().Split('-')[0].Trim());

            // Daten des Kontinents laden
            DataTable dt = db.Get("SELECT Kbezeichnung FROM Kontinent WHERE kID=@id",
                ("@id", currentKontinentId));

            if (dt.Rows.Count > 0)
                txtKBezeichnung.Text = dt.Rows[0][0].ToString();
        }

        #endregion

        #region GEHEGE - Verwaltung von Gehegen

        /// <summary>
        /// Lädt alle Gehege mit zugehörigem Kontinent
        /// Zeigt sie im Format "ID - Name (Kontinent)" an
        /// </summary>
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

        /// <summary>
        /// Lädt Gehege in die ComboBox (für Tier-Zuordnung)
        /// </summary>
        private void LoadGehegeComboBox()
        {
            DataTable dt = db.Get("SELECT gID, GBezeichnung FROM Gehege ORDER BY GBezeichnung");
            FillComboBox(cmbGehegeTiere, dt, "gID", "GBezeichnung");
        }

        /// <summary>
        /// Leert die Eingabefelder für Gehege
        /// </summary>
        private void ClearGehegeFields()
        {
            txtGBezeichnung.Text = "";
            cmbKontinentGehege.SelectedIndex = -1;
            currentGehegeId = 0;
        }

        /// <summary>
        /// Event: Button "Neu" für Gehege
        /// </summary>
        private void btnNewGehege_Click(object sender, System.EventArgs e)
        {
            ClearGehegeFields();
        }

        /// <summary>
        /// Event: Button "Speichern" für Gehege
        /// Speichert ein neues oder aktualisiert ein bestehendes Gehege
        /// </summary>
        private void btnSaveGehege_Click(object sender, System.EventArgs e)
        {
            // Validierung: Beide Felder müssen ausgefüllt sein
            if (txtGBezeichnung.Text == "" || cmbKontinentGehege.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte alles ausfüllen.");
                return;
            }

            // Ausgewählten Kontinent aus ComboBox holen
            int kontinentId = ((ComboBoxItem)cmbKontinentGehege.SelectedItem).Value;

            if (currentGehegeId == 0)
            {
                // Neues Gehege anlegen
                db.Execute("INSERT INTO Gehege (GBezeichnung, kontinentID) VALUES (@n,@k)",
                    ("@n", txtGBezeichnung.Text),
                    ("@k", kontinentId));
            }
            else
            {
                // Bestehendes Gehege aktualisieren
                db.Execute("UPDATE Gehege SET GBezeichnung=@n, kontinentID=@k WHERE gID=@id",
                    ("@n", txtGBezeichnung.Text),
                    ("@k", kontinentId),
                    ("@id", currentGehegeId));
            }

            LoadGehege();
            LoadGehegeComboBox();
            ClearGehegeFields();
        }

        /// <summary>
        /// Event: Button "Löschen" für Gehege
        /// </summary>
        private void btnDelGehege_Click(object sender, System.EventArgs e)
        {
            if (currentGehegeId == 0) return;

            if (MessageBox.Show("Gehege wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Gehege WHERE gID=@id",
                    ("@id", currentGehegeId));
                LoadGehege();
                LoadGehegeComboBox();
                ClearGehegeFields();
            }
        }

        /// <summary>
        /// Event: Gehege in ListBox wurde ausgewählt
        /// Lädt die Details des Geheges in die Eingabefelder
        /// </summary>
        private void lbGehege_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbGehege.SelectedItem == null) return;

            currentGehegeId = int.Parse(lbGehege.SelectedItem.ToString().Split('-')[0].Trim());

            DataTable dt = db.Get("SELECT GBezeichnung, kontinentID FROM Gehege WHERE gID=@id",
                ("@id", currentGehegeId));

            if (dt.Rows.Count > 0)
            {
                txtGBezeichnung.Text = dt.Rows[0]["GBezeichnung"].ToString();
                int kontinentId = System.Convert.ToInt32(dt.Rows[0]["kontinentID"]);

                // Richtigen Kontinent in ComboBox auswählen
                foreach (ComboBoxItem it in cmbKontinentGehege.Items)
                    if (it.Value == kontinentId)
                        cmbKontinentGehege.SelectedItem = it;
            }
        }

        #endregion

        #region TIERARTEN - Verwaltung von Tierarten

        /// <summary>
        /// Lädt alle Tierarten aus der Datenbank
        /// </summary>
        private void LoadTierarten()
        {
            DataTable dt = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            FillListBox(lbTierart, dt, "tierartID", "TABezeichnung");
        }

        /// <summary>
        /// Lädt Tierarten in die ComboBox (für Tier-Zuordnung)
        /// </summary>
        private void LoadTierartComboBox()
        {
            DataTable dt = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            FillComboBox(cmbTierartTiere, dt, "tierartID", "TABezeichnung");
        }

        /// <summary>
        /// Leert die Eingabefelder für Tierarten
        /// </summary>
        private void ClearTierartFields()
        {
            txtTABezeichnung.Text = "";
            currentTierartId = 0;
        }

        /// <summary>
        /// Event: Button "Neu" für Tierarten
        /// </summary>
        private void btnNewTierart_Click(object sender, System.EventArgs e)
        {
            ClearTierartFields();
        }

        /// <summary>
        /// Event: Button "Speichern" für Tierarten
        /// </summary>
        private void btnSaveTierart_Click(object sender, System.EventArgs e)
        {
            if (txtTABezeichnung.Text == "")
            {
                MessageBox.Show("Bitte eine Tierart eingeben.");
                return;
            }

            if (currentTierartId == 0)
            {
                db.Execute("INSERT INTO Tierart (TABezeichnung) VALUES (@p)",
                    ("@p", txtTABezeichnung.Text));
            }
            else
            {
                db.Execute("UPDATE Tierart SET TABezeichnung=@p WHERE tierartID=@id",
                    ("@p", txtTABezeichnung.Text),
                    ("@id", currentTierartId));
            }

            LoadTierarten();
            LoadTierartComboBox();
            ClearTierartFields();
        }

        /// <summary>
        /// Event: Button "Löschen" für Tierarten
        /// </summary>
        private void btnDelTierart_Click(object sender, System.EventArgs e)
        {
            if (currentTierartId == 0) return;

            if (MessageBox.Show("Tierart wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Tierart WHERE tierartID=@id",
                    ("@id", currentTierartId));
                LoadTierarten();
                LoadTierartComboBox();
                ClearTierartFields();
            }
        }

        /// <summary>
        /// Event: Tierart in ListBox wurde ausgewählt
        /// </summary>
        private void lbTierart_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbTierart.SelectedItem == null) return;

            currentTierartId = int.Parse(lbTierart.SelectedItem.ToString().Split('-')[0].Trim());

            DataTable dt = db.Get("SELECT TABezeichnung FROM Tierart WHERE tierartID=@id",
                ("@id", currentTierartId));

            if (dt.Rows.Count > 0)
                txtTABezeichnung.Text = dt.Rows[0][0].ToString();
        }

        #endregion

        #region TIERE - Verwaltung von einzelnen Tieren

        /// <summary>
        /// Lädt alle Tiere mit Tierart und Gehege
        /// </summary>
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

        /// <summary>
        /// Leert alle Eingabefelder für Tiere
        /// </summary>
        private void ClearTiereFields()
        {
            txtTierName.Text = "";
            txtGewicht.Text = "";
            dtpGeburtsdatum.Value = System.DateTime.Now;
            cmbTierartTiere.SelectedIndex = -1;
            cmbGehegeTiere.SelectedIndex = -1;
            currentTierId = 0;
        }

        /// <summary>
        /// Event: Button "Neu" für Tiere
        /// </summary>
        private void btnNewTier_Click(object sender, System.EventArgs e)
        {
            ClearTiereFields();
        }

        /// <summary>
        /// Event: Button "Speichern" für Tiere
        /// Validiert alle Eingaben und speichert das Tier
        /// </summary>
        private void btnSaveTier_Click(object sender, System.EventArgs e)
        {
            // Validierung: Alle Felder müssen korrekt ausgefüllt sein
            if (txtTierName.Text == "" ||
                txtGewicht.Text == "" ||
                !decimal.TryParse(txtGewicht.Text, out decimal gewicht) ||
                cmbTierartTiere.SelectedIndex == -1 ||
                cmbGehegeTiere.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte alle Felder korrekt ausfüllen.");
                return;
            }

            // IDs aus den ComboBoxen holen
            int tierartId = ((ComboBoxItem)cmbTierartTiere.SelectedItem).Value;
            int gehegeId = ((ComboBoxItem)cmbGehegeTiere.SelectedItem).Value;

            if (currentTierId == 0)
            {
                // Neues Tier anlegen
                db.Execute(@"INSERT INTO Tiere (Name,Gewicht,Geburtsdatum,TierartID,GehegeID)
                            VALUES (@n,@g,@d,@t,@h)",
                    ("@n", txtTierName.Text),
                    ("@g", gewicht),
                    ("@d", dtpGeburtsdatum.Value),
                    ("@t", tierartId),
                    ("@h", gehegeId));
            }
            else
            {
                // Bestehendes Tier aktualisieren
                db.Execute(@"UPDATE Tiere SET Name=@n, Gewicht=@g, Geburtsdatum=@d,
                            TierartID=@t, GehegeID=@h WHERE tierID=@id",
                    ("@n", txtTierName.Text),
                    ("@g", gewicht),
                    ("@d", dtpGeburtsdatum.Value),
                    ("@t", tierartId),
                    ("@h", gehegeId),
                    ("@id", currentTierId));
            }

            LoadTiere();
            LoadUebersicht();
            ClearTiereFields();
        }

        /// <summary>
        /// Event: Button "Löschen" für Tiere
        /// </summary>
        private void btnDelTier_Click(object sender, System.EventArgs e)
        {
            if (currentTierId == 0) return;

            if (MessageBox.Show("Tier wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.Execute("DELETE FROM Tiere WHERE tierID=@id",
                    ("@id", currentTierId));
                LoadTiere();
                LoadUebersicht();
                ClearTiereFields();
            }
        }

        /// <summary>
        /// Event: Tier in ListBox wurde ausgewählt
        /// Lädt alle Details des Tiers in die Eingabefelder
        /// </summary>
        private void lbTiere_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbTiere.SelectedItem == null) return;

            currentTierId = int.Parse(lbTiere.SelectedItem.ToString().Split('-')[0].Trim());

            DataTable dt = db.Get("SELECT * FROM Tiere WHERE tierID=@id",
                ("@id", currentTierId));

            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];
            txtTierName.Text = r["Name"].ToString();
            txtGewicht.Text = r["Gewicht"].ToString();
            dtpGeburtsdatum.Value = System.Convert.ToDateTime(r["Geburtsdatum"]);

            int tierartId = System.Convert.ToInt32(r["TierartID"]);
            int gehegeId = System.Convert.ToInt32(r["GehegeID"]);

            // Richtige Einträge in ComboBoxen auswählen
            foreach (ComboBoxItem it in cmbTierartTiere.Items)
                if (it.Value == tierartId)
                    cmbTierartTiere.SelectedItem = it;

            foreach (ComboBoxItem it in cmbGehegeTiere.Items)
                if (it.Value == gehegeId)
                    cmbGehegeTiere.SelectedItem = it;
        }

        #endregion

        #region ÜBERSICHT - DataGridView mit allen Tieren

        /// <summary>
        /// Lädt die komplette Tier-Übersicht mit allen verknüpften Daten
        /// Zeigt: Tier, Gewicht, Tierart, Gehege, Kontinent
        /// </summary>
        private void LoadUebersicht()
        {
            DataTable dt = db.Get(@"
                SELECT
                    t.tierID,
                    t.Name AS Tiername,
                    t.Gewicht,
                    ta.TABezeichnung AS Tierart,
                    g.GBezeichnung AS Gehege,
                    k.Kbezeichnung AS Kontinent
                FROM Tiere t
                LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                LEFT JOIN Gehege g ON t.GehegeID = g.gID
                LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                ORDER BY t.Name");

            dgvUebersicht.DataSource = dt;
            dgvUebersicht.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Event: Zelle in der Übersicht wurde geändert
        /// Erlaubt das direkte Bearbeiten von Tiername und Gewicht in der Tabelle
        /// </summary>
        private void dgvUebersicht_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvUebersicht.Rows[e.RowIndex];
            int id = System.Convert.ToInt32(row.Cells["tierID"].Value);
            string col = dgvUebersicht.Columns[e.ColumnIndex].Name;
            var value = row.Cells[e.ColumnIndex].Value;

            switch (col)
            {
                case "Tiername":
                    db.Execute("UPDATE Tiere SET Name=@v WHERE tierID=@id",
                        ("@v", value),
                        ("@id", id));
                    break;

                case "Gewicht":
                    // Validierung: Muss eine Zahl sein
                    if (!decimal.TryParse(value.ToString(), out _))
                    {
                        MessageBox.Show("Ungültiges Gewicht.");
                        LoadUebersicht();
                        return;
                    }
                    db.Execute("UPDATE Tiere SET Gewicht=@v WHERE tierID=@id",
                        ("@v", value),
                        ("@id", id));
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

        #region FUTTERVERWALTUNG - Verwaltung von Futtersorten

        /// <summary>
        /// Leert alle Eingabefelder für Futter
        /// Setzt Standardwerte für Einheit, Preis, etc.
        /// </summary>
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

        /// <summary>
        /// Event: Button "Neu" für Futter
        /// </summary>
        private void btnFutterNeu_Click(object sender, System.EventArgs e)
        {
            ClearFutterFields();
        }

        /// <summary>
        /// Event: Button "Laden" für Futterliste
        /// </summary>
        private void btnLadeFutter_Click(object sender, System.EventArgs e)
        {
            LoadFutterListe();
        }

        /// <summary>
        /// Lädt alle Futtersorten mit Details aus der Datenbank
        /// </summary>
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
                MessageBox.Show($"Fehler beim Laden der Futtersorten: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event: Button "Speichern" für Futter
        /// Erstellt eine neue Futtersorte oder aktualisiert eine bestehende
        /// </summary>
        private void btnFutterSpeichern_Click(object sender, System.EventArgs e)
        {
            // Validierung
            if (txtFutterBezeichnung.Text == "" || txtFutterEinheit.Text == "")
            {
                MessageBox.Show("Bitte Bezeichnung und Einheit eingeben.");
                return;
            }

            try
            {
                if (currentFutterId == 0)
                {
                    // Neues Futter anlegen
                    string sql = @"INSERT INTO Futter 
                                (Bezeichnung, Einheit, Preis_pro_Einheit, Lagerbestand, Mindestbestand, Bestellmenge)
                                VALUES (@bez, @einheit, @preis, @lager, @mindest, @bestell)";

                    db.Execute(sql,
                        ("@bez", txtFutterBezeichnung.Text),
                        ("@einheit", txtFutterEinheit.Text),
                        ("@preis", numFutterPreis.Value),
                        ("@lager", (int)numFutterLagerbestand.Value),
                        ("@mindest", (int)numFutterMindestbestand.Value),
                        ("@bestell", (int)numFutterBestellmenge.Value));
                }
                else
                {
                    // Bestehendes Futter aktualisieren
                    string sql = @"UPDATE Futter SET 
                                Bezeichnung = @bez,
                                Einheit = @einheit,
                                Preis_pro_Einheit = @preis,
                                Lagerbestand = @lager,
                                Mindestbestand = @mindest,
                                Bestellmenge = @bestell
                                WHERE futterID = @id";

                    db.Execute(sql,
                        ("@bez", txtFutterBezeichnung.Text),
                        ("@einheit", txtFutterEinheit.Text),
                        ("@preis", numFutterPreis.Value),
                        ("@lager", (int)numFutterLagerbestand.Value),
                        ("@mindest", (int)numFutterMindestbestand.Value),
                        ("@bestell", (int)numFutterBestellmenge.Value),
                        ("@id", currentFutterId));
                }

                LoadFutterListe();
                ClearFutterFields();
                UpdateStatus("✅ Futter gespeichert");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Event: Button "Löschen" für Futter
        /// </summary>
        private void btnFutterLöschen_Click(object sender, System.EventArgs e)
        {
            if (currentFutterId == 0) return;

            if (MessageBox.Show("Futtersorte wirklich löschen?", "Löschen bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    db.Execute("DELETE FROM Futter WHERE futterID = @id",
                        ("@id", currentFutterId));

                    LoadFutterListe();
                    ClearFutterFields();
                    UpdateStatus("✅ Futter gelöscht");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Löschen: {ex.Message}\n\n" +
                        "Möglicherweise wird dieses Futter noch in Fütterungsplänen verwendet.",
                        "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Event: Zeile in Futter-DataGridView wurde ausgewählt
        /// Lädt die Details der Futtersorte in die Eingabefelder
        /// </summary>
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

        #region NACHBESTELLUNG - Anzeige von Futtersorten mit niedrigem Bestand

        /// <summary>
        /// Event: Button "Nachbestellung laden"
        /// Zeigt alle Futtersorten an, die nachbestellt werden müssen
        /// (Lagerbestand unter Mindestbestand)
        /// </summary>
        private void btnLadeNachbestellung_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = zooDb.GetNachbestellListe();
                dgvNachbestellung.DataSource = dt;
                UpdateStatus($"✅ {dt.Rows.Count} Nachbestellpositionen geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Nachbestellung: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region FÜTTERUNGSPLAN - Übersicht der Fütterungspläne

        /// <summary>
        /// Lädt Tierarten in die ComboBox für Fütterungsplan-Filter
        /// Fügt "Alle Tierarten" als erste Option hinzu
        /// </summary>
        private void LoadTierartComboBoxFutterplan()
        {
            DataTable dt = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
            cmbTierartFutterplan.Items.Clear();

            // "Alle" Option hinzufügen
            cmbTierartFutterplan.Items.Add(new ComboBoxItem { Value = 0, Text = "-- Alle Tierarten --" });

            // Tierarten hinzufügen
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

        /// <summary>
        /// Event: Button "Fütterungsplan laden"
        /// Lädt den Fütterungsplan für die ausgewählte Tierart oder alle Tierarten
        /// </summary>
        private void btnLadeFutterplan_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;

                if (cmbTierartFutterplan.SelectedIndex > 0)
                {
                    // Fütterungsplan für spezifische Tierart
                    int tierartId = ((ComboBoxItem)cmbTierartFutterplan.SelectedItem).Value;
                    dt = zooDb.GetFutterplanFuerTierart(tierartId);
                    UpdateStatus($"✅ Fütterungsplan für Tierart geladen");
                }
                else
                {
                    // Alle Fütterungspläne laden
                    string sql = @"
                        SELECT 
                            ta.TABezeichnung AS Tierart,
                            f.Bezeichnung AS Futtersorte,
                            tf.Menge_pro_Tag,
                            f.Einheit,
                            tf.Fütterungszeit,
                            CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit, ' (', tf.Fütterungszeit, ')') AS Fütterungsplan
                        FROM Tierart_Futter tf
                        JOIN Tierart ta ON tf.tierartID = ta.tierartID
                        JOIN Futter f ON tf.futterID = f.futterID
                        ORDER BY ta.TABezeichnung, tf.Fütterungszeit";

                    dt = db.Get(sql);
                    UpdateStatus($"✅ Alle Fütterungspläne geladen");
                }

                dgvFutterplan.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden des Fütterungsplans: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region TAGESBEDARF - Anzeige des täglichen Futterbedarfs

        /// <summary>
        /// Event: Button "Tagesbedarf laden"
        /// Zeigt den täglichen Futterbedarf für jedes einzelne Tier an
        /// </summary>
        private void btnLadeTagesbedarf_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = zooDb.GetTagesbedarfProTier();
                dgvTagesbedarf.DataSource = dt;
                UpdateStatus($"✅ Tagesbedarf für {dt.Rows.Count} Tiere geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden des Tagesbedarfs: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region BESTELLUNGEN - Verwaltung von Futterbestellungen

        /// <summary>
        /// Event: Button "Bestellungen laden"
        /// Zeigt alle Bestellungen mit ihren Positionen an
        /// </summary>
        private void btnLadeBestellungen_Click(object sender, System.EventArgs e)
        {
            try
            {
                DataTable dt = zooDb.GetBestellungenMitPositionen();
                dgvBestellungen.DataSource = dt;
                UpdateStatus($"✅ {dt.Rows.Count} Bestellungen geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Bestellungen: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Hilfsklasse - ComboBoxItem

        /// <summary>
        /// Hilfsklasse für ComboBox-Elemente
        /// Speichert eine ID (Value) und einen Anzeigetext (Text)
        /// Wird verwendet, um Dropdowns mit ID-Text-Paaren zu füllen
        /// </summary>
        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        #endregion
    }
}
