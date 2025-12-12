using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ZooApp
{
    // Detail-Formular mit editierbaren Feldern für einzelne Tiere
    public class TierDetailForm : Form
    {
        private readonly DB db = new DB();
        private readonly int tierID;
        private string currentBildpfad = "";
        
        // Controls - ALLE EDITIERBAR
        private PictureBox pbTierBild;
        private Panel panelID;
        private Label lblIDNummer;
        private TextBox txtName;
        private ComboBox cmbTierart;
        private DateTimePicker dtpGeburtsdatum;
        private NumericUpDown numGewicht;
        private ComboBox cmbGehege;
        private Label lblKontinent;
        private Label lblAlter;
        private TextBox txtNotizen;
        private Button btnBildLaden;
        private Button btnBildLoeschen;
        private Button btnFutterplan; // Neu: Button für Fütterungsplan
        private Button btnSpeichern;
        private Button btnSchliessen;

        public TierDetailForm(int tierID)
        {
            this.tierID = tierID;
            InitializeControls();
            LoadTierData();
        }

        // Initialisiert alle Controls
        private void InitializeControls()
        {
            // Formular-Einstellungen
            this.Text = "🐾 Tier-Details";
            this.Size = new Size(900, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Header-Panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(52, 152, 219)
            };

            Label lblHeader = new Label
            {
                Text = "🐾 TIER-PROFIL BEARBEITEN",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            headerPanel.Controls.Add(lblHeader);
            this.Controls.Add(headerPanel);

            // Linke Seite - Bild und ID
            Panel leftPanel = new Panel
            {
                Left = 20,
                Top = 90,
                Width = 400,
                Height = 600,
                BackColor = Color.White
            };

            // Tier-Bild
            pbTierBild = new PictureBox
            {
                Left = 20,
                Top = 20,
                Width = 360,
                Height = 270,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            leftPanel.Controls.Add(pbTierBild);

            // Bild-Buttons
            btnBildLaden = new Button
            {
                Text = "📷 Bild laden",
                Left = 20,
                Top = 300,
                Width = 175,
                Height = 40,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBildLaden.Click += btnBildLaden_Click;
            leftPanel.Controls.Add(btnBildLaden);

            btnBildLoeschen = new Button
            {
                Text = "🗑️ Entfernen",
                Left = 205,
                Top = 300,
                Width = 175,
                Height = 40,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBildLoeschen.Click += btnBildLoeschen_Click;
            leftPanel.Controls.Add(btnBildLoeschen);

            // ID-Ausweis (nur Anzeige)
            panelID = new Panel
            {
                Left = 20,
                Top = 360,
                Width = 360,
                Height = 120,
                BackColor = Color.FromArgb(241, 196, 15),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblIDTitel = new Label
            {
                Text = "🆔 ZOO-AUSWEIS",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            panelID.Controls.Add(lblIDTitel);

            lblIDNummer = new Label
            {
                Text = "ID: #00000",
                Font = new Font("Consolas", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, 50)
            };
            panelID.Controls.Add(lblIDNummer);

            leftPanel.Controls.Add(panelID);

            // Info-Label
            Label lblInfo = new Label
            {
                Text = "ℹ️ hier kann noch was sinnvolles rein!",
                Left = 20,
                Top = 495,
                Width = 360,
                Height = 60,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(230, 240, 250)
            };
            leftPanel.Controls.Add(lblInfo);

            // Fütterungsplan-Button
            btnFutterplan = new Button
            {
                Text = "🍽️ Fütterungsplan",
                Left = 20,
                Top = 565,
                Width = 360,
                Height = 45,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFutterplan.FlatAppearance.BorderSize = 0;
            btnFutterplan.Click += btnFutterplan_Click;
            leftPanel.Controls.Add(btnFutterplan);

            this.Controls.Add(leftPanel);

            // Rechte Seite - EDITIERBARE Details
            Panel rightPanel = new Panel
            {
                Left = 440,
                Top = 90,
                Width = 420,
                Height = 600,
                BackColor = Color.White
            };

            int yPos = 20;

            // Name - EDITIERBAR
            Label lblNameTitel = new Label
            {
                Text = "🏷️ Name:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            rightPanel.Controls.Add(lblNameTitel);
            yPos += 30;

            txtName = new TextBox
            {
                Left = 20,
                Top = yPos,
                Width = 380,
                Height = 30,
                Font = new Font("Segoe UI", 11F)
            };
            rightPanel.Controls.Add(txtName);
            yPos += 50;

            // Tierart - EDITIERBAR
            Label lblTierartTitel = new Label
            {
                Text = "🦁 Tierart:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            rightPanel.Controls.Add(lblTierartTitel);
            yPos += 30;

            cmbTierart = new ComboBox
            {
                Left = 20,
                Top = yPos,
                Width = 380,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            rightPanel.Controls.Add(cmbTierart);
            yPos += 50;

            // Geburtsdatum - EDITIERBAR
            Label lblGeburtsdatumTitel = new Label
            {
                Text = "🎂 Geburtsdatum:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            rightPanel.Controls.Add(lblGeburtsdatumTitel);
            yPos += 30;

            dtpGeburtsdatum = new DateTimePicker
            {
                Left = 20,
                Top = yPos,
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                Format = DateTimePickerFormat.Short
            };
            dtpGeburtsdatum.ValueChanged += (s, e) => UpdateAlter();
            rightPanel.Controls.Add(dtpGeburtsdatum);

            // Alter - AUTO-BERECHNET
            lblAlter = new Label
            {
                Text = "0 Jahre alt",
                Left = 230,
                Top = yPos + 5,
                Width = 170,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100)
            };
            rightPanel.Controls.Add(lblAlter);
            yPos += 50;

            // Gewicht - EDITIERBAR
            Label lblGewichtTitel = new Label
            {
                Text = "⚖️ Gewicht:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            rightPanel.Controls.Add(lblGewichtTitel);
            yPos += 30;

            numGewicht = new NumericUpDown
            {
                Left = 20,
                Top = yPos,
                Width = 150,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 1,
                Minimum = 0.1m,
                Maximum = 10000m,
                Increment = 0.5m
            };
            rightPanel.Controls.Add(numGewicht);

            Label lblKg = new Label
            {
                Text = "kg",
                Left = 180,
                Top = yPos + 5,
                Width = 30,
                Font = new Font("Segoe UI", 10F)
            };
            rightPanel.Controls.Add(lblKg);
            yPos += 50;

            // Gehege - EDITIERBAR
            Label lblGehegeTitel = new Label
            {
                Text = "🏠 Gehege:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            rightPanel.Controls.Add(lblGehegeTitel);
            yPos += 30;

            cmbGehege = new ComboBox
            {
                Left = 20,
                Top = yPos,
                Width = 380,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbGehege.SelectedIndexChanged += (s, e) => UpdateKontinent();
            rightPanel.Controls.Add(cmbGehege);
            yPos += 40;

            // Kontinent - AUTO-ANZEIGE
            lblKontinent = new Label
            {
                Text = "🌍 Kontinent: -",
                Left = 20,
                Top = yPos,
                Width = 380,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100)
            };
            rightPanel.Controls.Add(lblKontinent);
            yPos += 40;

            // Notizen - EDITIERBAR
            Label lblNotizenTitel = new Label
            {
                Text = "📝 Notizen:",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, yPos)
            };
            rightPanel.Controls.Add(lblNotizenTitel);
            yPos += 30;

            txtNotizen = new TextBox
            {
                Left = 20,
                Top = yPos,
                Width = 380,
                Height = 80,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F)
            };
            rightPanel.Controls.Add(txtNotizen);
            yPos += 100;

            // Buttons
            btnSpeichern = new Button
            {
                Text = "💾 ALLE ÄNDERUNGEN SPEICHERN",
                Left = 20,
                Top = yPos,
                Width = 380,
                Height = 50,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSpeichern.Click += btnSpeichern_Click;
            rightPanel.Controls.Add(btnSpeichern);
            yPos += 60;

            btnSchliessen = new Button
            {
                Text = "❌ Schließen (ohne Speichern)",
                Left = 20,
                Top = yPos,
                Width = 380,
                Height = 40,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSchliessen.Click += (s, e) => this.Close();
            rightPanel.Controls.Add(btnSchliessen);

            this.Controls.Add(rightPanel);
        }

        // Lädt Tier-Daten aus der Datenbank
        private void LoadTierData()
        {
            try
            {
                // Tierarten laden
                DataTable dtTierarten = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
                cmbTierart.Items.Clear();
                foreach (DataRow tierartRow in dtTierarten.Rows)
                {
                    cmbTierart.Items.Add(new ComboBoxItem
                    {
                        Value = Convert.ToInt32(tierartRow["tierartID"]),
                        Text = tierartRow["TABezeichnung"].ToString()
                    });
                }
                cmbTierart.DisplayMember = "Text";
                cmbTierart.ValueMember = "Value";

                // Gehege laden
                DataTable dtGehege = db.Get(@"
                    SELECT g.gID, g.GBezeichnung, k.Kbezeichnung 
                    FROM Gehege g 
                    LEFT JOIN Kontinent k ON g.kontinentID = k.kID 
                    ORDER BY g.GBezeichnung");
                
                cmbGehege.Items.Clear();
                foreach (DataRow gehegeRow in dtGehege.Rows)
                {
                    cmbGehege.Items.Add(new GehegeItem
                    {
                        GehegeID = Convert.ToInt32(gehegeRow["gID"]),
                        GehegeName = gehegeRow["GBezeichnung"].ToString(),
                        KontinentName = gehegeRow["Kbezeichnung"]?.ToString() ?? "Unbekannt"
                    });
                }
                cmbGehege.DisplayMember = "Display";

                // Tier-Daten laden
                DataTable dt = db.Get(@"
                    SELECT t.*, ta.TABezeichnung, g.GBezeichnung, k.Kbezeichnung
                    FROM Tiere t
                    LEFT JOIN Tierart ta ON t.TierartID = ta.tierartID
                    LEFT JOIN Gehege g ON t.GehegeID = g.gID
                    LEFT JOIN Kontinent k ON g.kontinentID = k.kID
                    WHERE t.tierID = @id", ("@id", tierID));

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tier nicht gefunden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                DataRow tierRow = dt.Rows[0];

                // ID
                lblIDNummer.Text = $"ID: #{tierID.ToString("D5")}";
                this.Text = $"🐾 {tierRow["Name"]} - Bearbeiten";

                // Name
                txtName.Text = tierRow["Name"].ToString();

                // Tierart
                int tierartID = Convert.ToInt32(tierRow["TierartID"]);
                foreach (ComboBoxItem item in cmbTierart.Items)
                {
                    if (item.Value == tierartID)
                    {
                        cmbTierart.SelectedItem = item;
                        break;
                    }
                }

                // Geburtsdatum & Alter
                dtpGeburtsdatum.Value = Convert.ToDateTime(tierRow["Geburtsdatum"]);
                UpdateAlter();

                // Gewicht
                numGewicht.Value = Convert.ToDecimal(tierRow["Gewicht"]);

                // Gehege
                int gehegeID = Convert.ToInt32(tierRow["GehegeID"]);
                foreach (GehegeItem item in cmbGehege.Items)
                {
                    if (item.GehegeID == gehegeID)
                    {
                        cmbGehege.SelectedItem = item;
                        break;
                    }
                }
                UpdateKontinent();

                // Notizen
                if (tierRow["Notizen"] != DBNull.Value)
                    txtNotizen.Text = tierRow["Notizen"].ToString();

                // Bild laden
                if (tierRow["Bildpfad"] != DBNull.Value && !string.IsNullOrEmpty(tierRow["Bildpfad"].ToString()))
                {
                    currentBildpfad = tierRow["Bildpfad"].ToString();
                    LoadImage(currentBildpfad);
                }
                else
                {
                    pbTierBild.Image = CreatePlaceholderImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Aktualisiert Alter-Anzeige
        private void UpdateAlter()
        {
            int alter = DateTime.Now.Year - dtpGeburtsdatum.Value.Year;
            if (DateTime.Now < dtpGeburtsdatum.Value.AddYears(alter)) alter--;
            lblAlter.Text = $"{alter} Jahre alt";
        }

        // Aktualisiert Kontinent-Anzeige
        private void UpdateKontinent()
        {
            if (cmbGehege.SelectedItem is GehegeItem item)
            {
                lblKontinent.Text = $"🌍 Kontinent: {item.KontinentName}";
            }
        }

        // Lädt Bild aus Pfad
        private void LoadImage(string pfad)
        {
            try
            {
                string fullPath = Path.Combine(Application.StartupPath, pfad);
                if (File.Exists(fullPath))
                {
                    pbTierBild.Image = Image.FromFile(fullPath);
                }
                else
                {
                    pbTierBild.Image = CreatePlaceholderImage();
                }
            }
            catch
            {
                pbTierBild.Image = CreatePlaceholderImage();
            }
        }

        // Erstellt Platzhalter-Bild
        private Image CreatePlaceholderImage()
        {
            Bitmap bmp = new Bitmap(360, 270);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(240, 240, 240));
                g.DrawString("Kein Bild", new Font("Segoe UI", 16F), Brushes.Gray, new PointF(120, 120));
            }
            return bmp;
        }

        // Event: Bild laden
        private void btnBildLaden_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Bilder|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Tier-Bild auswählen";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string imagesFolder = Path.Combine(Application.StartupPath, "Images");
                        if (!Directory.Exists(imagesFolder))
                            Directory.CreateDirectory(imagesFolder);

                        string fileName = $"tier_{tierID}_{Path.GetFileName(ofd.FileName)}";
                        string destPath = Path.Combine(imagesFolder, fileName);

                        File.Copy(ofd.FileName, destPath, true);
                        currentBildpfad = Path.Combine("Images", fileName);
                        pbTierBild.Image = Image.FromFile(destPath);

                        MessageBox.Show("✅ Bild geladen! Speichern nicht vergessen.", "Erfolg", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Event: Bild löschen
        private void btnBildLoeschen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bild wirklich entfernen?", "Bestätigen", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                currentBildpfad = "";
                pbTierBild.Image = CreatePlaceholderImage();
            }
        }

        // Event: ALLE Änderungen speichern
        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            // Validierung
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("⚠️ Bitte einen Namen eingeben!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (cmbTierart.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Bitte eine Tierart auswählen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbGehege.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Bitte ein Gehege auswählen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int tierartID = ((ComboBoxItem)cmbTierart.SelectedItem).Value;
                int gehegeID = ((GehegeItem)cmbGehege.SelectedItem).GehegeID;

                // ALLE Daten speichern
                db.Execute(@"
                    UPDATE Tiere SET 
                        Name = @name,
                        TierartID = @tierart,
                        Geburtsdatum = @geburt,
                        Gewicht = @gewicht,
                        GehegeID = @gehege,
                        Bildpfad = @bild,
                        Notizen = @notizen
                    WHERE tierID = @id",
                    ("@name", txtName.Text.Trim()),
                    ("@tierart", tierartID),
                    ("@geburt", dtpGeburtsdatum.Value),
                    ("@gewicht", numGewicht.Value),
                    ("@gehege", gehegeID),
                    ("@bild", string.IsNullOrEmpty(currentBildpfad) ? (object)DBNull.Value : currentBildpfad),
                    ("@notizen", string.IsNullOrEmpty(txtNotizen.Text) ? (object)DBNull.Value : txtNotizen.Text.Trim()),
                    ("@id", tierID));

                MessageBox.Show("✅ Alle Änderungen erfolgreich gespeichert!", "Erfolg", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Speichern:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event: Fütterungsplan öffnen
        private void btnFutterplan_Click(object sender, EventArgs e)
        {
            try
            {
                // Tierart-ID ermitteln
                int tierartID = 0;
                if (cmbTierart.SelectedItem != null)
                {
                    tierartID = ((ComboBoxItem)cmbTierart.SelectedItem).Value;
                }
                else
                {
                    // Falls ComboBox noch nicht geladen, aus DB holen
                    DataTable dt = db.Get("SELECT TierartID FROM Tiere WHERE tierID = @id", ("@id", tierID));
                    if (dt.Rows.Count > 0)
                    {
                        tierartID = Convert.ToInt32(dt.Rows[0]["TierartID"]);
                    }
                }

                if (tierartID == 0)
                {
                    MessageBox.Show("⚠️ Tierart konnte nicht ermittelt werden!", "Fehler", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Fütterungsplan-Fenster öffnen
                TierFutterplanForm futterplanForm = new TierFutterplanForm(tierID, tierartID);
                futterplanForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Öffnen des Fütterungsplans:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hilfsklassen
        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        private class GehegeItem
        {
            public int GehegeID { get; set; }
            public string GehegeName { get; set; }
            public string KontinentName { get; set; }
            public string Display => $"{GehegeName} ({KontinentName})";
            public override string ToString() => Display;
        }
    }
}
