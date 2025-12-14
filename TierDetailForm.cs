using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace ZooApp
{
    /// <summary>
    /// Modernes Tier-Detail-Formular - ALLES ohne Scrollen sichtbar!
    /// </summary>
    public class TierDetailForm : Form
    {
        private readonly DB db = new DB();
        private readonly int tierID;
        private string currentBildpfad = "";

        // Controls
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
        
        private Panel leftCard;
        private Panel rightCard;

        public TierDetailForm(int tierID)
        {
            this.tierID = tierID;
            InitializeControls();
            LoadTierData();
        }

        /// <summary>
        /// Initialisiert alle Controls - optimiert für vollständige Sichtbarkeit ohne Scrollen
        /// </summary>
        private void InitializeControls()
        {
            // Formular-Einstellungen (HÖHER für vollständige Sichtbarkeit!)
            this.Text = "🐾 Tier-Details";
            this.Size = new Size(1100, 900);  // Erhöht von 750 auf 900!
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(1000, 850);
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header-Panel mit Gradient
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(52, 152, 219)
            };
            
            headerPanel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(52, 152, 219),
                    Color.FromArgb(41, 128, 185),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };

            Label lblHeader = new Label
            {
                Text = "🐾 TIER-PROFIL BEARBEITEN",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 22),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblHeader);
            this.Controls.Add(headerPanel);

            // Linke Card
            leftCard = CreateCard(30, 100, 420, this.ClientSize.Height - 130);
            leftCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CreateLeftCardContent();
            this.Controls.Add(leftCard);

            // Rechte Card
            rightCard = CreateCard(470, 100, this.ClientSize.Width - 500, this.ClientSize.Height - 130);
            rightCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CreateRightCardContent();
            this.Controls.Add(rightCard);
        }

        private Panel CreateCard(int x, int y, int width, int height)
        {
            Panel card = new Panel
            {
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                BackColor = Color.White
            };
            
            card.Paint += (s, e) =>
            {
                using (Pen shadowPen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(shadowPen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };
            
            return card;
        }

        private void CreateLeftCardContent()
        {
            // Card-Header
            Panel cardHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = leftCard.Width,
                Height = 55,
                BackColor = Color.FromArgb(52, 152, 219),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            
            Label lblCardTitle = new Label
            {
                Text = "📸 BILD & IDENTIFIKATION",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 16),
                BackColor = Color.Transparent
            };
            cardHeader.Controls.Add(lblCardTitle);
            leftCard.Controls.Add(cardHeader);

            // Tier-Bild
            pbTierBild = new PictureBox
            {
                Left = 20,
                Top = 70,
                Width = leftCard.Width - 40,
                Height = 260,
                BorderStyle = BorderStyle.None,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            leftCard.Controls.Add(pbTierBild);

            // Bild-Buttons
            int btnY = 345;
            int btnWidth = (leftCard.Width - 60) / 2;
            
            btnBildLaden = CreateModernButton("📷 Laden", 20, btnY, btnWidth, 42, Color.FromArgb(52, 152, 219));
            btnBildLaden.Click += btnBildLaden_Click;
            btnBildLaden.MouseEnter += (s, e) => btnBildLaden.BackColor = Color.FromArgb(41, 128, 185);
            btnBildLaden.MouseLeave += (s, e) => btnBildLaden.BackColor = Color.FromArgb(52, 152, 219);
            leftCard.Controls.Add(btnBildLaden);

            btnBildLoeschen = CreateModernButton("🗑️ Entfernen", 30 + btnWidth, btnY, btnWidth, 42, Color.FromArgb(231, 76, 60));
            btnBildLoeschen.Click += btnBildLoeschen_Click;
            btnBildLoeschen.MouseEnter += (s, e) => btnBildLoeschen.BackColor = Color.FromArgb(192, 57, 43);
            btnBildLoeschen.MouseLeave += (s, e) => btnBildLoeschen.BackColor = Color.FromArgb(231, 76, 60);
            leftCard.Controls.Add(btnBildLoeschen);

            // ID-Ausweis (modernes, klares Design)
            panelID = new Panel
            {
                Left = 20,
                Top = 405,
                Width = leftCard.Width - 40,
                Height = 100,
                BackColor = Color.FromArgb(241, 196, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Dicker Border für ID-Panel
            panelID.Paint += (s, e) =>
            {
                using (Pen borderPen = new Pen(Color.FromArgb(243, 156, 18), 3))
                {
                    e.Graphics.DrawRectangle(borderPen, 1, 1, panelID.Width - 3, panelID.Height - 3);
                }
            };

            // Großes ID-Icon links
            Label lblIDIcon = new Label
            {
                Text = "🆔",
                Font = new Font("Segoe UI", 36F),  // Etwas kleiner von 40F auf 36F
                AutoSize = true,
                Location = new Point(10, 28),
                BackColor = Color.Transparent
            };
            panelID.Controls.Add(lblIDIcon);

            // Text-Bereich rechts vom Icon (weiter nach rechts!)
            Label lblIDLabel = new Label
            {
                Text = "ZOO-AUSWEIS",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(150, 5),  // Von 85 auf 100 verschoben!
                BackColor = Color.Transparent
            };
            panelID.Controls.Add(lblIDLabel);

            lblIDNummer = new Label
            {
                Text = "#00000",
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(100, 48),  // Von 85 auf 100 verschoben!
                BackColor = Color.Transparent
            };
            panelID.Controls.Add(lblIDNummer);

            leftCard.Controls.Add(panelID);

            // FÜTTERUNGSPLAN-Button
            btnFutterplan = CreateModernButton("🍽️ FÜTTERUNGSPLAN ANSEHEN", 20, 525, leftCard.Width - 40, 52,
                Color.FromArgb(46, 204, 113));
            btnFutterplan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnFutterplan.Click += btnFutterplan_Click;
            btnFutterplan.MouseEnter += (s, e) => btnFutterplan.BackColor = Color.FromArgb(39, 174, 96);
            btnFutterplan.MouseLeave += (s, e) => btnFutterplan.BackColor = Color.FromArgb(46, 204, 113);
            btnFutterplan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            leftCard.Controls.Add(btnFutterplan);
        }

        private void CreateRightCardContent()
        {
            // Card-Header
            Panel cardHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = rightCard.Width,
                Height = 55,
                BackColor = Color.FromArgb(46, 204, 113),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            
            Label lblCardTitle = new Label
            {
                Text = "✏️ TIER-INFORMATIONEN",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 16),
                BackColor = Color.Transparent
            };
            cardHeader.Controls.Add(lblCardTitle);
            rightCard.Controls.Add(cardHeader);

            // Formular-Panel (KEIN Scroll!)
            Panel formPanel = new Panel
            {
                Left = 0,
                Top = 55,
                Width = rightCard.Width,
                Height = rightCard.Height - 55,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            int yPos = 20;

            // Name
            Label lblName = CreateFieldLabel("🏷️ Name", 25, yPos);
            formPanel.Controls.Add(lblName);
            yPos += 30;

            txtName = new TextBox
            {
                Left = 25,
                Top = yPos,
                Width = formPanel.Width - 65,
                Font = new Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            formPanel.Controls.Add(txtName);
            yPos += 50;

            // Tierart
            Label lblTierart = CreateFieldLabel("🦁 Tierart", 25, yPos);
            formPanel.Controls.Add(lblTierart);
            yPos += 30;

            cmbTierart = new ComboBox
            {
                Left = 25,
                Top = yPos,
                Width = formPanel.Width - 65,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            formPanel.Controls.Add(cmbTierart);
            yPos += 50;

            // Geburtsdatum + Alter
            Label lblGeburtsdatum = CreateFieldLabel("🎂 Geburtsdatum", 25, yPos);
            formPanel.Controls.Add(lblGeburtsdatum);
            yPos += 30;

            dtpGeburtsdatum = new DateTimePicker
            {
                Left = 25,
                Top = yPos,
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short
            };
            dtpGeburtsdatum.ValueChanged += (s, e) => UpdateAlter();
            formPanel.Controls.Add(dtpGeburtsdatum);

            lblAlter = new Label
            {
                Text = "0 Jahre alt",
                Left = 235,
                Top = yPos + 3,
                Width = 180,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(52, 152, 219)
            };
            formPanel.Controls.Add(lblAlter);
            yPos += 50;

            // Gewicht
            Label lblGewicht = CreateFieldLabel("⚖️ Gewicht", 25, yPos);
            formPanel.Controls.Add(lblGewicht);
            yPos += 30;

            numGewicht = new NumericUpDown
            {
                Left = 25,
                Top = yPos,
                Width = 160,
                Font = new Font("Segoe UI", 10F),
                DecimalPlaces = 1,
                Minimum = 0.1m,
                Maximum = 10000m,
                Increment = 0.5m
            };
            formPanel.Controls.Add(numGewicht);

            Label lblKg = new Label
            {
                Text = "kg",
                Left = 195,
                Top = yPos + 3,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true
            };
            formPanel.Controls.Add(lblKg);
            yPos += 50;

            // Gehege
            Label lblGehege = CreateFieldLabel("🏠 Gehege", 25, yPos);
            formPanel.Controls.Add(lblGehege);
            yPos += 30;

            cmbGehege = new ComboBox
            {
                Left = 25,
                Top = yPos,
                Width = formPanel.Width - 65,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cmbGehege.SelectedIndexChanged += (s, e) => UpdateKontinent();
            formPanel.Controls.Add(cmbGehege);
            yPos += 50;

            // Kontinent
            lblKontinent = new Label
            {
                Text = "🌍 Kontinent: -",
                Left = 25,
                Top = yPos,
                Width = formPanel.Width - 60,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(149, 165, 166),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            formPanel.Controls.Add(lblKontinent);
            yPos += 38;

            // Notizen
            Label lblNotizen = CreateFieldLabel("📝 Notizen", 25, yPos);
            formPanel.Controls.Add(lblNotizen);
            yPos += 30;

            txtNotizen = new TextBox
            {
                Left = 25,
                Top = yPos,
                Width = formPanel.Width - 65,
                Height = 75,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            formPanel.Controls.Add(txtNotizen);
            yPos += 95;

            // Speichern-Button
            btnSpeichern = CreateModernButton("💾 ALLE ÄNDERUNGEN SPEICHERN", 25, yPos,
                formPanel.Width - 65, 50, Color.FromArgb(46, 204, 113));
            btnSpeichern.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSpeichern.Click += btnSpeichern_Click;
            btnSpeichern.MouseEnter += (s, e) => btnSpeichern.BackColor = Color.FromArgb(39, 174, 96);
            btnSpeichern.MouseLeave += (s, e) => btnSpeichern.BackColor = Color.FromArgb(46, 204, 113);
            btnSpeichern.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            formPanel.Controls.Add(btnSpeichern);
            yPos += 60;

            // Schließen-Button
            btnSchliessen = CreateModernButton("❌ Schließen", 25, yPos,
                formPanel.Width - 65, 42, Color.FromArgb(149, 165, 166));
            btnSchliessen.Click += (s, e) => this.Close();
            btnSchliessen.MouseEnter += (s, e) => btnSchliessen.BackColor = Color.FromArgb(127, 140, 141);
            btnSchliessen.MouseLeave += (s, e) => btnSchliessen.BackColor = Color.FromArgb(149, 165, 166);
            btnSchliessen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            formPanel.Controls.Add(btnSchliessen);

            rightCard.Controls.Add(formPanel);
        }

        private Label CreateFieldLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private Button CreateModernButton(string text, int x, int y, int width, int height, Color bgColor)
        {
            Button btn = new Button
            {
                Text = text,
                Left = x,
                Top = y,
                Width = width,
                Height = height,
                BackColor = bgColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadTierData()
        {
            try
            {
                // Tierarten laden
                DataTable dtTierarten = db.Get("SELECT tierartID, TABezeichnung FROM Tierart ORDER BY TABezeichnung");
                cmbTierart.Items.Clear();
                foreach (DataRow row in dtTierarten.Rows)
                {
                    cmbTierart.Items.Add(new ComboBoxItem
                    {
                        Value = Convert.ToInt32(row["tierartID"]),
                        Text = row["TABezeichnung"].ToString()
                    });
                }
                cmbTierart.DisplayMember = "Text";

                // Gehege laden
                DataTable dtGehege = db.Get(@"
                    SELECT g.gID, g.GBezeichnung, k.Kbezeichnung 
                    FROM Gehege g 
                    LEFT JOIN Kontinent k ON g.kontinentID = k.kID 
                    ORDER BY g.GBezeichnung");

                cmbGehege.Items.Clear();
                foreach (DataRow row in dtGehege.Rows)
                {
                    cmbGehege.Items.Add(new GehegeItem
                    {
                        GehegeID = Convert.ToInt32(row["gID"]),
                        GehegeName = row["GBezeichnung"].ToString(),
                        KontinentName = row["Kbezeichnung"]?.ToString() ?? "Unbekannt"
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
                    MessageBox.Show("❌ Tier nicht gefunden!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                DataRow tierRow = dt.Rows[0];

                lblIDNummer.Text = $"#{tierID:D5}";
                this.Text = $"🐾 {tierRow["Name"]} - Bearbeiten";
                txtName.Text = tierRow["Name"].ToString();

                int tierartID = Convert.ToInt32(tierRow["TierartID"]);
                foreach (ComboBoxItem item in cmbTierart.Items)
                {
                    if (item.Value == tierartID)
                    {
                        cmbTierart.SelectedItem = item;
                        break;
                    }
                }

                dtpGeburtsdatum.Value = Convert.ToDateTime(tierRow["Geburtsdatum"]);
                UpdateAlter();
                numGewicht.Value = Convert.ToDecimal(tierRow["Gewicht"]);

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

                if (tierRow["Notizen"] != DBNull.Value)
                    txtNotizen.Text = tierRow["Notizen"].ToString();

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
                MessageBox.Show($"❌ Fehler beim Laden:\n{ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateAlter()
        {
            int alter = DateTime.Now.Year - dtpGeburtsdatum.Value.Year;
            if (DateTime.Now < dtpGeburtsdatum.Value.AddYears(alter)) alter--;
            lblAlter.Text = $"{alter} Jahre alt";
        }

        private void UpdateKontinent()
        {
            if (cmbGehege.SelectedItem is GehegeItem item)
            {
                lblKontinent.Text = $"🌍 Kontinent: {item.KontinentName}";
            }
        }

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

        private Image CreatePlaceholderImage()
        {
            Bitmap bmp = new Bitmap(380, 260);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(245, 245, 245));
                
                Font iconFont = new Font("Segoe UI", 42F);
                string icon = "🖼️";
                SizeF iconSize = g.MeasureString(icon, iconFont);
                g.DrawString(icon, iconFont, Brushes.LightGray,
                    (bmp.Width - iconSize.Width) / 2, (bmp.Height - iconSize.Height) / 2 - 15);
                
                Font textFont = new Font("Segoe UI", 11F);
                string text = "Kein Bild";
                SizeF textSize = g.MeasureString(text, textFont);
                g.DrawString(text, textFont, Brushes.Gray,
                    (bmp.Width - textSize.Width) / 2, (bmp.Height + iconSize.Height) / 2);
            }
            return bmp;
        }

        #region Event-Handler

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
                        MessageBox.Show($"❌ Fehler:\n{ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBildLoeschen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bild wirklich entfernen?", "Bestätigen",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                currentBildpfad = "";
                pbTierBild.Image = CreatePlaceholderImage();
            }
        }

        private void btnFutterplan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTierart.SelectedItem == null)
                {
                    MessageBox.Show("⚠️ Bitte zuerst eine Tierart auswählen!", "Hinweis",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int tierartID = ((ComboBoxItem)cmbTierart.SelectedItem).Value;
                TierFutterplanForm futterplanForm = new TierFutterplanForm(tierID, tierartID);
                futterplanForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler:\n{ex.Message}", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSpeichern_Click(object sender, EventArgs e)
        {
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

        #endregion

        #region Hilfsklassen

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

        #endregion
    }
}
