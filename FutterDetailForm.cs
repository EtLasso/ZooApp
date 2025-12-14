using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace ZooApp
{
    /// <summary>
    /// VOLLSTÄNDIGES Futter-Detail-Fenster mit Lager, Preisen, Haltbarkeit, Nährwerten, Anbietern
    /// Nutzt ALLE verfügbaren Datenbank-Felder
    /// </summary>
    public class FutterDetailForm : Form
    {
        private readonly DB db = new DB();
        private readonly int futterID;
        private string currentBildpfad = "";

        // Panels - 3-Panel Layout
        private Panel basisPanel, lagerPanel, infoPanel;
        
        // Basis-Daten
        private PictureBox pbBild;
        private Button btnBildHochladen;
        private TextBox txtBezeichnung, txtEinheit, txtKategorie, txtBeschreibung;
        private NumericUpDown numPreis;
        private ComboBox cmbStatus;
        private Label lblFutterID, lblStatusInfo;
        
        // Lager
        private NumericUpDown numLagerbestand, numMindestbestand, numBestellmenge;
        private TextBox txtLagerort;
        private DateTimePicker dtpBestBefore;
        private NumericUpDown numHaltbarkeitTage;
        private Label lblBestandWarnung, lblHaltbarWarnung;
        private ProgressBar pbLagerstand;
        
        // Info & Zuordnungen
        private TextBox txtNaehrwerte;
        private DataGridView dgvTierarten;
        private DataGridView dgvAnbieter;
        private Button btnNeuerAnbieter, btnBestellen;
        
        // Buttons
        private Button btnSpeichern, btnSchliessen;

        public FutterDetailForm(int futterID)
        {
            this.futterID = futterID;
            InitializeUI();
            LoadFutterData();
        }

        /// <summary>
        /// Initialisiert komplettes UI mit 3 Panels
        /// </summary>
        private void InitializeUI()
        {
            // Formular
            this.Text = "🍖 Futter-Details";
            this.Size = new Size(1650, 1000);
            this.MinimumSize = new Size(1650, 1000);
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header
            Panel header = new Panel { Dock = DockStyle.Top, Height = 80 };
            header.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(header.ClientRectangle,
                    Color.FromArgb(26, 188, 156), Color.FromArgb(22, 160, 133), 0f))
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
            };
            Label lblHeader = new Label
            {
                Text = "🍖 FUTTER-VERWALTUNG",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 22),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            header.Controls.Add(lblHeader);
            this.Controls.Add(header);

            // Basis-Daten (Spalte 1)
            basisPanel = CreateCard(20, 100, 480, 760);
            basisPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CreateBasisSection();
            this.Controls.Add(basisPanel);

            // Lager & Haltbarkeit (Spalte 2)
            lagerPanel = CreateCard(520, 100, 480, 760);
            lagerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CreateLagerSection();
            this.Controls.Add(lagerPanel);

            // Info & Zuordnungen (Spalte 3)
            infoPanel = CreateCard(1020, 100, this.ClientSize.Width - 1050, 760);
            infoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CreateInfoSection();
            this.Controls.Add(infoPanel);

            // Buttons
            CreateButtons();
        }

        /// <summary>
        /// Basis-Daten: Name, Kategorie, Preis, Bild, Status
        /// </summary>
        private void CreateBasisSection()
        {
            AddHeader(basisPanel, "📦 BASIS-DATEN", Color.FromArgb(26, 188, 156));
            int y = 70;

            // Bild
            pbBild = new PictureBox
            {
                Left = 20, Top = y, Width = 220, Height = 220,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            pbBild.Paint += (s, e) =>
            {
                if (pbBild.Image == null)
                {
                    e.Graphics.DrawString("🍖", new Font("Segoe UI", 80F), Brushes.Gray, 40, 40);
                    e.Graphics.DrawString("Kein Bild", new Font("Segoe UI", 10F), Brushes.Gray, 60, 180);
                }
            };
            basisPanel.Controls.Add(pbBild);

            btnBildHochladen = new Button
            {
                Text = "📷 Bild hochladen",
                Left = 20, Top = y + 230, Width = 220, Height = 35,
                BackColor = Color.FromArgb(26, 188, 156),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBildHochladen.FlatAppearance.BorderSize = 0;
            btnBildHochladen.Click += BtnBildHochladen_Click;
            basisPanel.Controls.Add(btnBildHochladen);

            // Info rechts vom Bild
            lblFutterID = new Label
            {
                Text = "#F0000",
                Left = 255, Top = y + 10, Width = 200,
                Font = new Font("Consolas", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            basisPanel.Controls.Add(lblFutterID);

            AddLabel(basisPanel, "Status", 255, y + 45);
            cmbStatus = new ComboBox
            {
                Left = 255, Top = y + 70, Width = 200,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new[] { "Verfügbar", "Knapp", "Ausverkauft", "Abgelaufen" });
            cmbStatus.SelectedIndexChanged += (s, e) => UpdateStatusInfo();
            basisPanel.Controls.Add(cmbStatus);

            lblStatusInfo = new Label
            {
                Text = "●",
                Left = 255, Top = y + 100, Width = 200,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.Gray
            };
            basisPanel.Controls.Add(lblStatusInfo);

            y += 280;

            // Bezeichnung
            AddLabel(basisPanel, "📝 Bezeichnung *", 20, y);
            txtBezeichnung = new TextBox { Left = 20, Top = y + 25, Width = 435, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            basisPanel.Controls.Add(txtBezeichnung);
            y += 65;

            // Kategorie
            AddLabel(basisPanel, "🏷️ Kategorie", 20, y);
            txtKategorie = new TextBox { Left = 20, Top = y + 25, Width = 210, Font = new Font("Segoe UI", 10F) };
            basisPanel.Controls.Add(txtKategorie);

            AddLabel(basisPanel, "📏 Einheit *", 245, y);
            txtEinheit = new TextBox { Left = 245, Top = y + 25, Width = 210, Font = new Font("Segoe UI", 10F) };
            basisPanel.Controls.Add(txtEinheit);
            y += 65;

            // Preis
            AddLabel(basisPanel, "💰 Preis pro Einheit *", 20, y);
            numPreis = new NumericUpDown
            {
                Left = 20, Top = y + 25, Width = 180,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 2,
                Maximum = 10000,
                Value = 0
            };
            basisPanel.Controls.Add(numPreis);
            basisPanel.Controls.Add(new Label { Text = "€", Left = 210, Top = y + 28, Font = new Font("Segoe UI", 11F, FontStyle.Bold) });
            y += 65;

            // Beschreibung
            AddLabel(basisPanel, "📄 Beschreibung", 20, y);
            txtBeschreibung = new TextBox
            {
                Left = 20, Top = y + 25, Width = 435, Height = 110,
                Font = new Font("Segoe UI", 10F),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            basisPanel.Controls.Add(txtBeschreibung);
        }

        /// <summary>
        /// Lager & Haltbarkeit: Bestand, Mindest, Bestellung, Lagerort, MHD
        /// </summary>
        private void CreateLagerSection()
        {
            AddHeader(lagerPanel, "📊 LAGER & HALTBARKEIT", Color.FromArgb(41, 128, 185));
            int y = 70;

            // Lagerbestand mit Fortschrittsbalken
            AddLabel(lagerPanel, "📦 Lagerbestand", 20, y);
            numLagerbestand = new NumericUpDown
            {
                Left = 20, Top = y + 25, Width = 150,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                DecimalPlaces = 2,
                Maximum = 100000,
                Value = 0
            };
            numLagerbestand.ValueChanged += (s, e) => UpdateBestandsWarnung();
            lagerPanel.Controls.Add(numLagerbestand);
            
            pbLagerstand = new ProgressBar
            {
                Left = 20, Top = y + 55, Width = 435, Height = 25,
                Minimum = 0,
                Maximum = 100,
                Value = 50
            };
            lagerPanel.Controls.Add(pbLagerstand);

            lblBestandWarnung = new Label
            {
                Text = "✅ Bestand ausreichend",
                Left = 20, Top = y + 85, Width = 435,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96)
            };
            lagerPanel.Controls.Add(lblBestandWarnung);
            y += 125;

            // Mindestbestand
            AddLabel(lagerPanel, "⚠️ Mindestbestand", 20, y);
            numMindestbestand = new NumericUpDown
            {
                Left = 20, Top = y + 25, Width = 150,
                Font = new Font("Segoe UI", 10F),
                DecimalPlaces = 2,
                Maximum = 100000,
                Value = 50
            };
            numMindestbestand.ValueChanged += (s, e) => UpdateBestandsWarnung();
            lagerPanel.Controls.Add(numMindestbestand);
            y += 65;

            // Bestellmenge
            AddLabel(lagerPanel, "📋 Standard-Bestellmenge", 20, y);
            numBestellmenge = new NumericUpDown
            {
                Left = 20, Top = y + 25, Width = 150,
                Font = new Font("Segoe UI", 10F),
                DecimalPlaces = 2,
                Maximum = 100000,
                Value = 100
            };
            lagerPanel.Controls.Add(numBestellmenge);
            y += 65;

            // Lagerort
            AddLabel(lagerPanel, "🏢 Lagerort", 20, y);
            txtLagerort = new TextBox { Left = 20, Top = y + 25, Width = 435, Font = new Font("Segoe UI", 10F) };
            lagerPanel.Controls.Add(txtLagerort);
            y += 80;

            // Trennlinie
            var sep = new Panel { Left = 20, Top = y, Width = 435, Height = 2, BackColor = Color.FromArgb(189, 195, 199) };
            lagerPanel.Controls.Add(sep);
            y += 20;

            // Haltbarkeit in Tagen
            AddLabel(lagerPanel, "⏳ Haltbarkeit (Tage)", 20, y);
            numHaltbarkeitTage = new NumericUpDown
            {
                Left = 20, Top = y + 25, Width = 150,
                Font = new Font("Segoe UI", 10F),
                Maximum = 3650,
                Value = 0
            };
            lagerPanel.Controls.Add(numHaltbarkeitTage);

            lagerPanel.Controls.Add(new Label
            {
                Text = "⚠️ 0 = unbegrenzt",
                Left = 180, Top = y + 28,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            });
            y += 65;

            // Best Before Datum
            AddLabel(lagerPanel, "📅 Mindesthaltbarkeitsdatum", 20, y);
            dtpBestBefore = new DateTimePicker
            {
                Left = 20, Top = y + 25, Width = 200,
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short
            };
            dtpBestBefore.ValueChanged += (s, e) => UpdateHaltbarkeitsWarnung();
            lagerPanel.Controls.Add(dtpBestBefore);

            CheckBox chkKeinMHD = new CheckBox
            {
                Text = "Kein MHD",
                Left = 230, Top = y + 28, Width = 120,
                Font = new Font("Segoe UI", 9F)
            };
            chkKeinMHD.CheckedChanged += (s, e) => dtpBestBefore.Enabled = !chkKeinMHD.Checked;
            lagerPanel.Controls.Add(chkKeinMHD);

            lblHaltbarWarnung = new Label
            {
                Text = "✅ Haltbar",
                Left = 20, Top = y + 55, Width = 435,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96)
            };
            lagerPanel.Controls.Add(lblHaltbarWarnung);
            y += 95;

            // Bestell-Button
            Button btnQuickBestellung = new Button
            {
                Text = "🛒 SCHNELL-BESTELLUNG",
                Left = 20, Top = y, Width = 435, Height = 50,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnQuickBestellung.FlatAppearance.BorderSize = 0;
            btnQuickBestellung.Click += (s, e) => SchnellBestellung();
            lagerPanel.Controls.Add(btnQuickBestellung);
        }

        /// <summary>
        /// Info: Nährwerte, Tierarten, Anbieter
        /// </summary>
        private void CreateInfoSection()
        {
            AddHeader(infoPanel, "ℹ️ WEITERE INFORMATIONEN", Color.FromArgb(155, 89, 182));
            int y = 70;

            // Nährwerte
            AddLabel(infoPanel, "🥗 Nährwertinformationen", 20, y);
            txtNaehrwerte = new TextBox
            {
                Left = 20, Top = y + 25,
                Width = infoPanel.Width - 50,
                Height = 100,
                Font = new Font("Segoe UI", 9F),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            txtNaehrwerte.Text = "Protein: \nFett: \nKohlenhydrate: \nKalorien pro 100g: ";
            infoPanel.Controls.Add(txtNaehrwerte);
            y += 140;

            // Welche Tierarten bekommen dieses Futter
            AddLabel(infoPanel, "🐾 Wird gefüttert an folgende Tierarten:", 20, y);
            dgvTierarten = new DataGridView
            {
                Left = 20, Top = y + 25,
                Width = infoPanel.Width - 50,
                Height = 180,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvTierarten.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(155, 89, 182);
            dgvTierarten.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTierarten.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvTierarten.ColumnHeadersHeight = 35;
            infoPanel.Controls.Add(dgvTierarten);
            y += 215;

            // Anbieter
            AddLabel(infoPanel, "🏭 Anbieter-Übersicht", 20, y);
            dgvAnbieter = new DataGridView
            {
                Left = 20, Top = y + 25,
                Width = infoPanel.Width - 50,
                Height = 180,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9F),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvAnbieter.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvAnbieter.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAnbieter.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvAnbieter.ColumnHeadersHeight = 35;
            infoPanel.Controls.Add(dgvAnbieter);
        }

        /// <summary>
        /// Speichern & Schließen Buttons
        /// </summary>
        private void CreateButtons()
        {
            int y = this.ClientSize.Height - 80;

            btnSpeichern = new Button
            {
                Text = "💾 ALLE ÄNDERUNGEN SPEICHERN",
                Left = 30, Top = y, Width = 500, Height = 55,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnSpeichern.FlatAppearance.BorderSize = 0;
            btnSpeichern.Click += BtnSpeichern_Click;
            this.Controls.Add(btnSpeichern);

            btnSchliessen = new Button
            {
                Text = "❌ Schließen",
                Left = this.ClientSize.Width - 230, Top = y, Width = 200, Height = 55,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnSchliessen.FlatAppearance.BorderSize = 0;
            btnSchliessen.Click += (s, e) => this.Close();
            this.Controls.Add(btnSchliessen);
        }

        /// <summary>
        /// Lädt alle Futter-Daten aus DB
        /// </summary>
        private void LoadFutterData()
        {
            try
            {
                DataTable dt = db.Get("SELECT * FROM futter WHERE futterID = @id", ("@id", futterID));
                if (dt.Rows.Count == 0) { this.Close(); return; }

                DataRow r = dt.Rows[0];
                
                txtBezeichnung.Text = r["Bezeichnung"].ToString();
                txtEinheit.Text = r["Einheit"].ToString();
                numPreis.Value = r["Preis_pro_Einheit"] != DBNull.Value ? Convert.ToDecimal(r["Preis_pro_Einheit"]) : 0;
                txtKategorie.Text = r["Kategorie"]?.ToString() ?? "";
                txtBeschreibung.Text = r["Beschreibung"]?.ToString() ?? "";
                
                numLagerbestand.Value = r["Lagerbestand"] != DBNull.Value ? Convert.ToDecimal(r["Lagerbestand"]) : 0;
                numMindestbestand.Value = r["Mindestbestand"] != DBNull.Value ? Convert.ToDecimal(r["Mindestbestand"]) : 50;
                numBestellmenge.Value = r["Bestellmenge"] != DBNull.Value ? Convert.ToDecimal(r["Bestellmenge"]) : 100;
                txtLagerort.Text = r["Lagerort"]?.ToString() ?? "";
                
                if (r["Status"] != DBNull.Value) cmbStatus.SelectedItem = r["Status"].ToString();
                else cmbStatus.SelectedIndex = 0;
                
                txtNaehrwerte.Text = r["Naehrwert_info"]?.ToString() ?? txtNaehrwerte.Text;
                numHaltbarkeitTage.Value = r["Haltbarkeit_Tage"] != DBNull.Value ? Convert.ToInt32(r["Haltbarkeit_Tage"]) : 0;
                
                if (r["Best_Before"] != DBNull.Value)
                    dtpBestBefore.Value = Convert.ToDateTime(r["Best_Before"]);

                lblFutterID.Text = $"#F{futterID:D4}";
                this.Text = $"🍖 {r["Bezeichnung"]}";

                currentBildpfad = r["Bildpfad"]?.ToString() ?? "";
                LoadBild();
                UpdateBestandsWarnung();
                UpdateHaltbarkeitsWarnung();
                UpdateStatusInfo();
                LoadTierarten();
                LoadAnbieter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTierarten()
        {
            try
            {
                DataTable dt = db.Get(@"
                    SELECT ta.TABezeichnung AS 'Tierart',
                           tf.Menge_pro_Tag AS 'Menge/Tag',
                           f.Einheit,
                           TIME_FORMAT(tf.Fuetterungszeit, '%H:%i Uhr') AS 'Fütterungszeit',
                           tf.Prioritaet AS 'Priorität'
                    FROM tierart_futter tf
                    JOIN Tierart ta ON tf.tierartID = ta.tierartID
                    JOIN Futter f ON tf.futterID = f.futterID
                    WHERE tf.futterID = @id
                    ORDER BY ta.TABezeichnung", ("@id", futterID));
                dgvTierarten.DataSource = dt;
                if (dgvTierarten.Rows.Count > 0) dgvTierarten.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Laden der Tierarten: {ex.Message}");
            }
        }

        private void LoadAnbieter()
        {
            try
            {
                DataTable dt = db.Get(@"
                    SELECT Firmenname AS 'Anbieter',
                           Kontaktperson AS 'Kontakt',
                           Telefon,
                           Email,
                           CONCAT(Bewertung, ' ⭐') AS 'Bewertung',
                           CONCAT(Lieferkosten, ' €') AS 'Lieferkosten'
                    FROM futter_anbieter
                    ORDER BY Bewertung DESC, Firmenname");
                dgvAnbieter.DataSource = dt;
                if (dgvAnbieter.Rows.Count > 0) dgvAnbieter.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Laden der Anbieter: {ex.Message}");
            }
        }

        private void UpdateBestandsWarnung()
        {
            decimal bestand = numLagerbestand.Value;
            decimal mindest = numMindestbestand.Value;
            
            if (mindest == 0)
            {
                pbLagerstand.Value = 50;
                lblBestandWarnung.Text = "ℹ️ Kein Mindestbestand definiert";
                lblBestandWarnung.ForeColor = Color.Gray;
                return;
            }

            int prozent = Math.Min(100, (int)((bestand / mindest) * 100));
            pbLagerstand.Value = Math.Max(0, prozent);

            if (bestand == 0)
            {
                lblBestandWarnung.Text = "❌ AUSVERKAUFT! Sofort nachbestellen!";
                lblBestandWarnung.ForeColor = Color.FromArgb(192, 57, 43);
            }
            else if (bestand < mindest)
            {
                lblBestandWarnung.Text = $"⚠️ KNAPP! Nur noch {bestand} {txtEinheit.Text} vorrätig";
                lblBestandWarnung.ForeColor = Color.FromArgb(230, 126, 34);
            }
            else if (bestand < mindest * 1.5m)
            {
                lblBestandWarnung.Text = "⚡ Bestand niedrig, bald nachbestellen";
                lblBestandWarnung.ForeColor = Color.FromArgb(241, 196, 15);
            }
            else
            {
                lblBestandWarnung.Text = $"✅ Bestand ausreichend ({bestand} {txtEinheit.Text})";
                lblBestandWarnung.ForeColor = Color.FromArgb(39, 174, 96);
            }
        }

        private void UpdateHaltbarkeitsWarnung()
        {
            DateTime mhd = dtpBestBefore.Value.Date;
            int tage = (mhd - DateTime.Now.Date).Days;

            if (tage < 0)
            {
                lblHaltbarWarnung.Text = $"❌ ABGELAUFEN seit {Math.Abs(tage)} Tagen!";
                lblHaltbarWarnung.ForeColor = Color.FromArgb(192, 57, 43);
            }
            else if (tage == 0)
            {
                lblHaltbarWarnung.Text = "⚠️ Läuft HEUTE ab!";
                lblHaltbarWarnung.ForeColor = Color.FromArgb(230, 126, 34);
            }
            else if (tage <= 7)
            {
                lblHaltbarWarnung.Text = $"⚡ Läuft in {tage} Tagen ab";
                lblHaltbarWarnung.ForeColor = Color.FromArgb(241, 196, 15);
            }
            else if (tage <= 30)
            {
                lblHaltbarWarnung.Text = $"⏳ Läuft in {tage} Tagen ab";
                lblHaltbarWarnung.ForeColor = Color.FromArgb(52, 152, 219);
            }
            else
            {
                lblHaltbarWarnung.Text = $"✅ Haltbar bis {mhd:dd.MM.yyyy}";
                lblHaltbarWarnung.ForeColor = Color.FromArgb(39, 174, 96);
            }
        }

        private void UpdateStatusInfo()
        {
            switch (cmbStatus.SelectedItem?.ToString())
            {
                case "Verfügbar":
                    lblStatusInfo.Text = "● Verfügbar";
                    lblStatusInfo.ForeColor = Color.FromArgb(39, 174, 96);
                    break;
                case "Knapp":
                    lblStatusInfo.Text = "● Knapp";
                    lblStatusInfo.ForeColor = Color.FromArgb(230, 126, 34);
                    break;
                case "Ausverkauft":
                    lblStatusInfo.Text = "● Ausverkauft";
                    lblStatusInfo.ForeColor = Color.FromArgb(192, 57, 43);
                    break;
                case "Abgelaufen":
                    lblStatusInfo.Text = "● Abgelaufen";
                    lblStatusInfo.ForeColor = Color.FromArgb(127, 140, 141);
                    break;
            }
        }

        private void LoadBild()
        {
            if (!string.IsNullOrEmpty(currentBildpfad) && File.Exists(currentBildpfad))
            {
                try { pbBild.Image = Image.FromFile(currentBildpfad); }
                catch { pbBild.Image = null; }
            }
            else pbBild.Image = null;
        }

        private void BtnBildHochladen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Bilder|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string ordner = Path.Combine(Application.StartupPath, "bilder", "futter");
                        Directory.CreateDirectory(ordner);
                        string ziel = Path.Combine(ordner, $"futter_{futterID}{Path.GetExtension(ofd.FileName)}");
                        File.Copy(ofd.FileName, ziel, true);
                        currentBildpfad = ziel;
                        LoadBild();
                        MessageBox.Show("✅ Bild hochgeladen!", "Erfolg");
                    }
                    catch (Exception ex) { MessageBox.Show($"❌ Fehler: {ex.Message}", "Fehler"); }
                }
            }
        }

        private void SchnellBestellung()
        {
            decimal menge = numBestellmenge.Value;
            string text = $"Bestellung für {txtBezeichnung.Text}\n\nMenge: {menge} {txtEinheit.Text}\nGeschätzter Preis: {menge * numPreis.Value:C2}\n\nBestellung jetzt aufgeben?";
            
            if (MessageBox.Show(text, "Schnell-Bestellung", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    numLagerbestand.Value += menge;
                    UpdateBestandsWarnung();
                    MessageBox.Show($"✅ Bestellung aufgegeben!\nNeuer Bestand: {numLagerbestand.Value} {txtEinheit.Text}", "Erfolg");
                }
                catch (Exception ex) { MessageBox.Show($"❌ {ex.Message}"); }
            }
        }

        private void BtnSpeichern_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBezeichnung.Text))
            {
                MessageBox.Show("⚠️ Bezeichnung erforderlich!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEinheit.Text))
            {
                MessageBox.Show("⚠️ Einheit erforderlich!");
                return;
            }

            try
            {
                db.Execute(@"UPDATE futter SET 
                    Bezeichnung = @bez, Einheit = @einh, Preis_pro_Einheit = @preis,
                    Lagerbestand = @bestand, Mindestbestand = @mindest, Bestellmenge = @bestell,
                    Kategorie = @kat, Beschreibung = @beschr, Bildpfad = @bild,
                    Naehrwert_info = @naehr, Haltbarkeit_Tage = @halt, Lagerort = @ort,
                    Best_Before = @mhd, Status = @status
                    WHERE futterID = @id",
                    ("@bez", txtBezeichnung.Text.Trim()),
                    ("@einh", txtEinheit.Text.Trim()),
                    ("@preis", numPreis.Value),
                    ("@bestand", numLagerbestand.Value),
                    ("@mindest", numMindestbestand.Value),
                    ("@bestell", numBestellmenge.Value),
                    ("@kat", txtKategorie.Text),
                    ("@beschr", txtBeschreibung.Text),
                    ("@bild", currentBildpfad),
                    ("@naehr", txtNaehrwerte.Text),
                    ("@halt", (int)numHaltbarkeitTage.Value),
                    ("@ort", txtLagerort.Text),
                    ("@mhd", dtpBestBefore.Value),
                    ("@status", cmbStatus.SelectedItem?.ToString() ?? "Verfügbar"),
                    ("@id", futterID));

                MessageBox.Show("✅ Gespeichert!", "Erfolg");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler: {ex.Message}", "Fehler");
            }
        }

        // Hilfsmethoden
        private Panel CreateCard(int x, int y, int w, int h)
        {
            var p = new Panel { Left = x, Top = y, Width = w, Height = h, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            return p;
        }

        private void AddHeader(Panel p, string txt, Color c)
        {
            var hdr = new Panel { Width = p.Width, Height = 55, BackColor = c, Dock = DockStyle.Top };
            hdr.Controls.Add(new Label
            {
                Text = txt,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 17),
                AutoSize = true,
                BackColor = Color.Transparent
            });
            p.Controls.Add(hdr);
        }

        private void AddLabel(Panel p, string txt, int x, int y)
        {
            p.Controls.Add(new Label
            {
                Text = txt,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(x, y),
                AutoSize = true
            });
        }
    }
}
