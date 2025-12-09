using System.Data;
using MySql.Data.MySqlClient;

namespace ZooApp
{
    public partial class Form1 : Form
    {
        // ===========================================================
        // FELDER + KONSTANTEN
        // ===========================================================

        private readonly DB db = new DB();   // neue vereinfachte DB-Klasse

        private int currentKontinentId = 0;
        private int currentGehegeId = 0;
        private int currentTierartId = 0;
        private int currentTierId = 0;

        public Form1()
        {
            InitializeComponent();
        }

        // ===========================================================
        // FORM LOAD
        // ===========================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            // Datenbank testen
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
                LoadKontinente();
                LoadGehege();
                LoadTierarten();
                LoadTiere();

                LoadKontinentComboBox();
                LoadTierartComboBox();
                LoadGehegeComboBox();

                LoadUebersicht();

                UpdateStatus("✅ Alle Daten geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ===========================================================
        // ALLGEMEINE HILFEN
        // ===========================================================

        private void UpdateStatus(string msg)
        {
            lblStatus.Text = msg;
        }

        /// <summary>
        /// Hilfsfunktion zum Füllen einer ListBox mit Format "ID - Name"
        /// </summary>
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
        /// Hilfsfunktion zum Füllen einer ComboBox mit ComboBoxItem
        /// </summary>
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
        // ===========================================================
        // KONTINENTE
        // ===========================================================

        private void LoadKontinente()
        {
            DataTable dt = db.Get("SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung");
            FillListBox(lbKontinent, dt, "kID", "Kbezeichnung");
        }

        private void ClearKontinentFields()
        {
            txtKBezeichnung.Text = "";
            currentKontinentId = 0;
        }

        private void btnNewKontinent_Click(object sender, EventArgs e)
        {
            ClearKontinentFields();
        }

        private void btnSaveKontinent_Click(object sender, EventArgs e)
        {
            if (txtKBezeichnung.Text == "")
            {
                MessageBox.Show("Bitte Bezeichnung eingeben.");
                return;
            }

            if (currentKontinentId == 0)
            {
                db.Execute("INSERT INTO Kontinent (Kbezeichnung) VALUES (@p)",
                    ("@p", txtKBezeichnung.Text));
            }
            else
            {
                db.Execute("UPDATE Kontinent SET Kbezeichnung=@p WHERE kID=@id",
                    ("@p", txtKBezeichnung.Text),
                    ("@id", currentKontinentId));
            }

            LoadKontinente();
            LoadKontinentComboBox();
            ClearKontinentFields();
        }

        private void btnDelKontinent_Click(object sender, EventArgs e)
        {
            if (currentKontinentId == 0) return;

            db.Execute("DELETE FROM Kontinent WHERE kID=@id",
                ("@id", currentKontinentId));

            LoadKontinente();
            LoadKontinentComboBox();
            ClearKontinentFields();
        }

        private void lbKontinent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbKontinent.SelectedItem == null) return;

            currentKontinentId = int.Parse(lbKontinent.SelectedItem.ToString().Split('-')[0]);

            DataTable dt = db.Get("SELECT Kbezeichnung FROM Kontinent WHERE kID=@id",
                ("@id", currentKontinentId));

            if (dt.Rows.Count > 0)
                txtKBezeichnung.Text = dt.Rows[0][0].ToString();
        }


        // ===========================================================
        // GEHEGE
        // ===========================================================

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

        private void LoadKontinentComboBox()
        {
            DataTable dt = db.Get("SELECT kID, Kbezeichnung FROM Kontinent ORDER BY Kbezeichnung");
            FillComboBox(cmbKontinentGehege, dt, "kID", "Kbezeichnung");
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

        private void btnSaveGehege_Click(object sender, EventArgs e)
        {
            if (txtGBezeichnung.Text == "" || cmbKontinentGehege.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte alles ausfüllen.");
                return;
            }

            int kontinentId = ((ComboBoxItem)cmbKontinentGehege.SelectedItem).Value;

            if (currentGehegeId == 0)
            {
                db.Execute("INSERT INTO Gehege (GBezeichnung, kontinentID) VALUES (@n,@k)",
                    ("@n", txtGBezeichnung.Text),
                    ("@k", kontinentId));
            }
            else
            {
                db.Execute("UPDATE Gehege SET GBezeichnung=@n, kontinentID=@k WHERE gID=@id",
                    ("@n", txtGBezeichnung.Text),
                    ("@k", kontinentId),
                    ("@id", currentGehegeId));
            }

            LoadGehege();
            LoadGehegeComboBox();
            ClearGehegeFields();
        }

        private void btnDelGehege_Click(object sender, EventArgs e)
        {
            if (currentGehegeId == 0) return;

            db.Execute("DELETE FROM Gehege WHERE gID=@id",
                ("@id", currentGehegeId));

            LoadGehege();
            LoadGehegeComboBox();
            ClearGehegeFields();
        }

        private void lbGehege_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbGehege.SelectedItem == null) return;

            currentGehegeId = int.Parse(lbGehege.SelectedItem.ToString().Split('-')[0]);

            DataTable dt = db.Get("SELECT GBezeichnung, kontinentID FROM Gehege WHERE gID=@id",
                ("@id", currentGehegeId));

            if (dt.Rows.Count > 0)
            {
                txtGBezeichnung.Text = dt.Rows[0]["GBezeichnung"].ToString();
                int kontinentId = Convert.ToInt32(dt.Rows[0]["kontinentID"]);

                foreach (ComboBoxItem it in cmbKontinentGehege.Items)
                    if (it.Value == kontinentId)
                        cmbKontinentGehege.SelectedItem = it;
            }
        }
        // ===========================================================
        // TIERARTEN
        // ===========================================================

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

        private void btnDelTierart_Click(object sender, EventArgs e)
        {
            if (currentTierartId == 0) return;

            db.Execute("DELETE FROM Tierart WHERE tierartID=@id",
                ("@id", currentTierartId));

            LoadTierarten();
            LoadTierartComboBox();
            ClearTierartFields();
        }

        private void lbTierart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTierart.SelectedItem == null) return;

            currentTierartId = int.Parse(lbTierart.SelectedItem.ToString().Split('-')[0]);

            DataTable dt = db.Get("SELECT TABezeichnung FROM Tierart WHERE tierartID=@id",
                ("@id", currentTierartId));

            if (dt.Rows.Count > 0)
                txtTABezeichnung.Text = dt.Rows[0][0].ToString();
        }


        // ===========================================================
        // TIERE
        // ===========================================================

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

        private void LoadGehegeComboBox()
        {
            DataTable dt = db.Get("SELECT gID, GBezeichnung FROM Gehege ORDER BY GBezeichnung");
            FillComboBox(cmbGehegeTiere, dt, "gID", "GBezeichnung");
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

        private void btnSaveTier_Click(object sender, EventArgs e)
        {
            if (txtTierName.Text == "" ||
                txtGewicht.Text == "" ||
                !decimal.TryParse(txtGewicht.Text, out decimal gewicht) ||
                cmbTierartTiere.SelectedIndex == -1 ||
                cmbGehegeTiere.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte alle Felder korrekt ausfüllen.");
                return;
            }

            int tierartId = ((ComboBoxItem)cmbTierartTiere.SelectedItem).Value;
            int gehegeId = ((ComboBoxItem)cmbGehegeTiere.SelectedItem).Value;

            if (currentTierId == 0)
            {
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

        private void btnDelTier_Click(object sender, EventArgs e)
        {
            if (currentTierId == 0) return;

            db.Execute("DELETE FROM Tiere WHERE tierID=@id",
                ("@id", currentTierId));

            LoadTiere();
            LoadUebersicht();
            ClearTiereFields();
        }

        private void lbTiere_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTiere.SelectedItem == null) return;

            currentTierId = int.Parse(lbTiere.SelectedItem.ToString().Split('-')[0]);

            DataTable dt = db.Get("SELECT * FROM Tiere WHERE tierID=@id",
                ("@id", currentTierId));

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
        // ===========================================================
        // ÜBERSICHT (DataGridView)
        // ===========================================================

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

        private void dgvUebersicht_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvUebersicht.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["tierID"].Value);
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


        // ===========================================================
        // ComboBoxItem Hilfsklasse
        // ===========================================================

        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }

            public override string ToString() => Text;
        }
    }
}
