using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace ZooApp
{
    /// <summary>
    /// Modernes Detail-Formular für einzelne Tiere mit vollständiger Bearbeitungsmöglichkeit.
    /// Features: Bildupload, Fütterungsplan-Integration, alle Daten editierbar
    /// </summary>
    public class TierDetailForm : Form
    {
        #region Private Felder
        
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
        private Button btnFutterplan;
        private Button btnSpeichern;
        private Button btnSchliessen;
        
        #endregion

        #region Konstruktor
        
        public TierDetailForm(int tierID)
        {
            this.tierID = tierID;
            InitializeControls();
            LoadTierData();
        }
        
        #endregion

        #region UI-Initialisierung
        
        /// <summary>
        /// Initialisiert alle Controls mit modernem Design
        /// </summary>
        private void InitializeControls()
        {
            // Formular-Einstellungen (größer für bessere Sichtbarkeit)
            this.Text = "🐾 Tier-Details";
            this.Size = new Size(950, 820);  // Erhöht von 900x750
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(236, 240, 241);  // Hellgrau-Hintergrund

            // Header-Panel mit Gradient-Effekt
            Panel headerPanel = CreateHeaderPanel();
            this.Controls.Add(headerPanel);

            // Linke Seite - Bild und ID (mit Card-Style)
            Panel leftPanel = CreateLeftPanel();
            this.Controls.Add(leftPanel);

            // Rechte Seite - EDITIERBARE Details (mit Card-Style)
            Panel rightPanel = CreateRightPanel();
            this.Controls.Add(rightPanel);
        }

        /// <summary>
        /// Erstellt den Header-Bereich mit Gradient
        /// </summary>
        private Panel CreateHeaderPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(52, 152, 219)
            };
            
            // Gradient-Effekt simulieren durch zweites Panel
            Panel gradientOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            gradientOverlay.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    gradientOverlay.ClientRectangle,
                    Color.FromArgb(52, 152, 219),
                    Color.FromArgb(41, 128, 185),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, gradientOverlay.ClientRectangle);
                }
            };
            panel.Controls.Add(gradientOverlay);

            Label lblHeader = new Label
            {
                Text = "🐾 TIER-PROFIL BEARBEITEN",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            gradientOverlay.Controls.Add(lblHeader);
            
            return panel;
        }

        /// <summary>
        /// Erstellt das linke Panel mit Bild und ID-Ausweis
        /// </summary>
        private Panel CreateLeftPanel()
        {
            // Äußeres Card-Panel mit Schatten-Effekt
            Panel outerCard = CreateCardPanel(20, 90, 430, 660);
            
            // Innerer Inhalt
            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(15)
            };

            int yPos = 15;

            // Tier-Bild mit abgerundeten Ecken
            pbTierBild = new PictureBox
            {
                Left = 15,
                Top = yPos,
                Width = 380,
                Height = 285,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            pbTierBild.Paint += (s, e) =>
            {
                // Rahmen zeichnen
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pbTierBild.Width - 1, pbTierBild.Height - 1);
                }
            };
            contentPanel.Controls.Add(pbTierBild);
            yPos += 300;

            // Bild-Buttons nebeneinander
            btnBildLaden = CreateModernButton("📷 Bild laden", 15, yPos, 185, 45, 
                Color.FromArgb(52, 152, 219), Color.White);
            btnBildLaden.Click += btnBildLaden_Click;
            contentPanel.Controls.Add(btnBildLaden);

            btnBildLoeschen = CreateModernButton("🗑️ Entfernen", 210, yPos, 185, 45, 
                Color.FromArgb(231, 76, 60), Color.White);
            btnBildLoeschen.Click += btnBildLoeschen_Click;
            contentPanel.Controls.Add(btnBildLoeschen);
            yPos += 60;

            // ID-Ausweis als Card
            panelID = new Panel
            {
                Left = 15,
                Top = yPos,
                Width = 380,
                Height = 110,
                BackColor = Color.FromArgb(241, 196, 15)
            };
            panelID.Paint += (s, e) =>
            {
                // Abgerundete Ecken
                using (Pen pen = new Pen(Color.FromArgb(243, 156, 18), 3))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panelID.Width - 1, panelID.Height - 1);
                }
            };

            Label lblIDTitel = new Label
            {
                Text = "🆔 ZOO-AUSWEIS",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(15, 15)
            };
            panelID.Controls.Add(lblIDTitel);

            lblIDNummer = new Label
            {
                Text = "ID: #00000",
                Font = new Font("Consolas", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(15, 50)
            };
            panelID.Controls.Add(lblIDNummer);

            contentPanel.Controls.Add(panelID);
            yPos += 125;

            // Info-Box
            Label lblInfo = new Label
            {
                Text = "💡 Tipp: Alle Änderungen müssen gespeichert werden!",
                Left = 15,
                Top = yPos,
                Width = 380,
                Height = 60,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(174, 214, 241),
                Padding = new Padding(10)
            };
            contentPanel.Controls.Add(lblInfo);
            yPos += 75;

            // Fütterungsplan-Button (prominent)
            btnFutterplan = CreateModernButton("🍽️ Fütterungsplan anzeigen", 15, yPos, 380, 50, 
                Color.FromArgb(46, 204, 113), Color.White);
            btnFutterplan.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnFutterplan.Click += btnFutterplan_Click;
            contentPanel.Controls.Add(btnFutterplan);

            outerCard.Controls.Add(contentPanel);
            return outerCard;
        }

        /// <summary>
        /// Erstellt das rechte Panel mit allen editierbaren Feldern
        /// </summary>
        private Panel CreateRightPanel()
        {
            // Äußeres Card-Panel
            Panel outerCard = CreateCardPanel(470, 90, 450, 660);
            
            // Scrollbares inneres Panel (für Sicherheit falls Content zu lang)
            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(15)
            };

            int yPos = 15;

            // Name
            yPos = AddLabelAndTextBox(scrollPanel, "🏷️ Name:", ref txtName, yPos, 380);

            // Tierart
            yPos = AddLabelAndComboBox(scrollPanel, "🦁 Tierart:", ref cmbTierart, yPos, 380);

            // Geburtsdatum + Alter
            Label lblGeburtsdatumTitel = CreateLabel("🎂 Geburtsdatum:", 15, yPos);
            scrollPanel.Controls.Add(lblGeburtsdatumTitel);
            yPos += 30;

            dtpGeburtsdatum = new DateTimePicker
            {
                Left = 15,
                Top = yPos,
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                Format = DateTimePickerFormat.Short
            };
            dtpGeburtsdatum.ValueChanged += (s, e) => UpdateAlter();
            scrollPanel.Controls.Add(dtpGeburtsdatum);

            lblAlter = new Label
            {
                Text = "0 Jahre alt",
                Left = 225,
                Top = yPos + 5,
                Width = 170,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(127, 140, 141)
            };
            scrollPanel.Controls.Add(lblAlter);
            yPos += 50;

            // Gewicht
            Label lblGewichtTitel = CreateLabel("⚖️ Gewicht:", 15, yPos);
            scrollPanel.Controls.Add(lblGewichtTitel);
            yPos += 30;

            numGewicht = new NumericUpDown
            {
                Left = 15,
                Top = yPos,
                Width = 150,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                DecimalPlaces = 1,
                Minimum = 0.1m,
                Maximum = 10000m,
                Increment = 0.5m
            };
            scrollPanel.Controls.Add(numGewicht);

            Label lblKg = new Label
            {
                Text = "kg",
                Left = 175,
                Top = yPos + 5,
                Width = 30,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            scrollPanel.Controls.Add(lblKg);
            yPos += 50;

            // Gehege
            yPos = AddLabelAndComboBox(scrollPanel, "🏠 Gehege:", ref cmbGehege, yPos, 380);
            cmbGehege.SelectedIndexChanged += (s, e) => UpdateKontinent();

            // Kontinent (auto-anzeige)
            lblKontinent = new Label
            {
                Text = "🌍 Kontinent: -",
                Left = 15,
                Top = yPos,
                Width = 380,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(127, 140, 141),
                BackColor = Color.FromArgb(236, 240, 241),
                Padding = new Padding(5)
            };
            scrollPanel.Controls.Add(lblKontinent);
            yPos += 40;

            // Notizen
            Label lblNotizenTitel = CreateLabel("📝 Notizen:", 15, yPos);
            scrollPanel.Controls.Add(lblNotizenTitel);
            yPos += 30;

            txtNotizen = new TextBox
            {
                Left = 15,
                Top = yPos,
                Width = 400,
                Height = 90,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };
            scrollPanel.Controls.Add(txtNotizen);
            yPos += 105;

            // Speichern-Button (prominent)
            btnSpeichern = CreateModernButton("💾 ALLE ÄNDERUNGEN SPEICHERN", 15, yPos, 400, 55, 
                Color.FromArgb(46, 204, 113), Color.White);
            btnSpeichern.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnSpeichern.Click += btnSpeichern_Click;
            // Hover-Effekt
            btnSpeichern.MouseEnter += (s, e) => btnSpeichern.BackColor = Color.FromArgb(39, 174, 96);
            btnSpeichern.MouseLeave += (s, e) => btnSpeichern.BackColor = Color.FromArgb(46, 204, 113);
            scrollPanel.Controls.Add(btnSpeichern);
            yPos += 65;

            // Schließen-Button
            btnSchliessen = CreateModernButton("❌ Schließen (ohne Speichern)", 15, yPos, 400, 45, 
                Color.FromArgb(149, 165, 166), Color.White);
            btnSchliessen.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSchliessen.Click += (s, e) => this.Close();
            btnSchliessen.MouseEnter += (s, e) => btnSchliessen.BackColor = Color.FromArgb(127, 140, 141);
            btnSchliessen.MouseLeave += (s, e) => btnSchliessen.BackColor = Color.FromArgb(149, 165, 166);
            scrollPanel.Controls.Add(btnSchliessen);

            outerCard.Controls.Add(scrollPanel);
            return outerCard;
        }

        #endregion

        #region Helper-Methoden für UI-Erstellung

        /// <summary>
        /// Erstellt ein Panel mit Card-Style (Schatten-Effekt)
        /// </summary>
        private Panel CreateCardPanel(int x, int y, int width, int height)
        {
            Panel panel = new Panel
            {
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                BackColor = Color.White
            };
            
            // Schatten-Effekt durch Rahmen
            panel.Paint += (s, e) =>
            {
                using (Pen shadowPen = new Pen(Color.FromArgb(189, 195, 199), 1))
                {
                    e.Graphics.DrawRectangle(shadowPen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };
            
            return panel;
        }

        /// <summary>
        /// Erstellt einen modernen Button mit Hover-Effekt
        /// </summary>
        private Button CreateModernButton(string text, int x, int y, int width, int height, 
            Color bgColor, Color fgColor)
        {
            Button btn = new Button
            {
                Text = text,
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                BackColor = bgColor,
                ForeColor = fgColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        /// <summary>
        /// Erstellt ein Label mit Standard-Styling
        /// </summary>
        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        /// <summary>
        /// Fügt Label + TextBox hinzu und gibt neue Y-Position zurück
        /// </summary>
        private int AddLabelAndTextBox(Panel parent, string labelText, ref TextBox textBox, int yPos, int width)
        {
            Label lbl = CreateLabel(labelText, 15, yPos);
            parent.Controls.Add(lbl);
            yPos += 30;

            textBox = new TextBox
            {
                Left = 15,
                Top = yPos,
                Width = width,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(textBox);
            
            return yPos + 50;
        }

        /// <summary>
        /// Fügt Label + ComboBox hinzu und gibt neue Y-Position zurück
        /// </summary>
        private int AddLabelAndComboBox(Panel parent, string labelText, ref ComboBox comboBox, int yPos, int width)
        {
            Label lbl = CreateLabel(labelText, 15, yPos);
            parent.Controls.Add(lbl);
            yPos += 30;

            comboBox = new ComboBox
            {
                Left = 15,
                Top = yPos,
                Width = width,
                Height = 30,
                Font = new Font("Segoe UI", 11F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            parent.Controls.Add(comboBox);
            
            return yPos + 50;
        }

        #endregion

        #region Daten laden und aktualisieren

        /// <summary>
        /// Lädt Tier-Daten aus der Datenbank und füllt alle Felder
        /// </summary>
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

                // ID und Titel
                lblIDNummer.Text = $"ID: #{tierID.ToString("D5")}";
                this.Text = $"🐾 {tierRow["Name"]} - Bearbeiten";

                // Alle Felder füllen
                txtName.Text = tierRow["Name"].ToString();

                // Tierart auswählen
                int tierartID = Convert.ToInt32(tierRow["TierartID"]);
                foreach (ComboBoxItem item in cmbTierart.Items)
                {
                    if (item.Value == tierartID)
                    {
                        cmbTierart.SelectedItem = item;
                        break;
                    }
                }

                // Geburtsdatum und Alter
                dtpGeburtsdatum.Value = Convert.ToDateTime(tierRow["Geburtsdatum"]);
                UpdateAlter();

                // Gewicht
                numGewicht.Value = Convert.ToDecimal(tierRow["Gewicht"]);

                // Gehege auswählen
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

        /// <summary>
        /// Aktualisiert die Alters-Anzeige basierend auf dem Geburtsdatum
        /// </summary>
        private void UpdateAlter()
        {
            int alter = DateTime.Now.Year - dtpGeburtsdatum.Value.Year;
            if (DateTime.Now < dtpGeburtsdatum.Value.AddYears(alter)) alter--;
            lblAlter.Text = $"📅 {alter} Jahre alt";
        }

        /// <summary>
        /// Aktualisiert die Kontinent-Anzeige basierend auf dem gewählten Gehege
        /// </summary>
        private void UpdateKontinent()
        {
            if (cmbGehege.SelectedItem is GehegeItem item)
            {
                lblKontinent.Text = $"🌍 Kontinent: {item.KontinentName}";
            }
        }

        /// <summary>
        /// Lädt ein Bild aus dem angegebenen Pfad
        /// </summary>
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

        /// <summary>
        /// Erstellt ein Platzhalter-Bild wenn kein Tier-Bild vorhanden
        /// </summary>
        private Image CreatePlaceholderImage()
        {
            Bitmap bmp = new Bitmap(380, 285);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(245, 245, 245));
                
                // "Kein Bild" Text
                string text = "📷 Kein Bild";
                Font font = new Font("Segoe UI", 18F, FontStyle.Bold);
                SizeF textSize = g.MeasureString(text, font);
                float x = (bmp.Width - textSize.Width) / 2;
                float y = (bmp.Height - textSize.Height) / 2;
                
                g.DrawString(text, font, Brushes.Gray, new PointF(x, y));
            }
            return bmp;
        }

        #endregion

        #region Event-Handler

        /// <summary>
        /// Öffnet einen Dialog zum Laden eines Tierbildes
        /// </summary>
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
                        // Images-Ordner erstellen falls nicht vorhanden
                        string imagesFolder = Path.Combine(Application.StartupPath, "Images");
                        if (!Directory.Exists(imagesFolder))
                            Directory.CreateDirectory(imagesFolder);

                        // Datei kopieren
                        string fileName = $"tier_{tierID}_{Path.GetFileName(ofd.FileName)}";
                        string destPath = Path.Combine(imagesFolder, fileName);

                        File.Copy(ofd.FileName, destPath, true);
                        currentBildpfad = Path.Combine("Images", fileName);
                        pbTierBild.Image = Image.FromFile(destPath);

                        MessageBox.Show("✅ Bild geladen! Vergiss nicht zu speichern.", "Erfolg", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler beim Laden: {ex.Message}", "Fehler", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Entfernt das Tier-Bild
        /// </summary>
        private void btnBildLoeschen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bild wirklich entfernen?", "Bestätigen", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                currentBildpfad = "";
                pbTierBild.Image = CreatePlaceholderImage();
            }
        }

        /// <summary>
        /// Speichert ALLE Änderungen in die Datenbank
        /// </summary>
        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            // Validierung
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("⚠️ Bitte einen Namen eingeben!", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (cmbTierart.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Bitte eine Tierart auswählen!", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbGehege.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Bitte ein Gehege auswählen!", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        /// <summary>
        /// Öffnet das Fütterungsplan-Fenster für dieses Tier
        /// </summary>
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

        #endregion

        #region Hilfsklassen

        /// <summary>
        /// Hilfsklasse für ComboBox-Einträge (Tierart)
        /// </summary>
        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }

        /// <summary>
        /// Hilfsklasse für Gehege-ComboBox mit Kontinent-Info
        /// </summary>
        private class GehegeItem
        {
            public int GehegeID { get; set; }
            public string GehegeName { get; set; }
            public string KontinentName { get; set; }
            public string Display => $"{GehegeName} ({KontinentName})";
            public override string ToString() => Display;
        }

        #endregion
    }
}
