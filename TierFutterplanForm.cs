using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ZooApp
{
    /// <summary>
    /// Modernes, responsives Formular zum Anzeigen und Bearbeiten des Fütterungsplans.
    /// Passt sich automatisch an Fenstergröße an.
    /// </summary>
    public class TierFutterplanForm : Form
    {
        #region Private Felder
        
        private readonly DB db = new DB();
        private readonly int tierID;
        private readonly int tierartID;
        
        private DataGridView dgvFutterplan;
        private Button btnNeu;
        private Button btnBearbeiten;
        private Button btnLoeschen;
        private Button btnSchliessen;
        private Label lblTierInfo;
        private Panel cardPanel;
        private Panel buttonPanel;
        
        #endregion

        #region Konstruktor

        public TierFutterplanForm(int tierID, int tierartID)
        {
            this.tierID = tierID;
            this.tierartID = tierartID;
            
            InitializeControls();
            LoadFutterplan();
        }
        
        #endregion

        #region UI-Initialisierung

        /// <summary>
        /// Initialisiert alle Controls mit responsivem Design
        /// </summary>
        private void InitializeControls()
        {
            // Formular-Einstellungen
            this.Text = "🍽️ Fütterungsplan";
            this.Size = new Size(950, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(800, 600);
            this.BackColor = Color.FromArgb(236, 240, 241);
            
            // Resize-Event für dynamische Anpassung
            this.Resize += (s, e) => AdjustLayout();

            // Header-Panel mit Gradient
            Panel headerPanel = CreateHeaderPanel();
            this.Controls.Add(headerPanel);

            // Card-Panel für DataGridView (RESPONSIVE)
            cardPanel = new Panel
            {
                Left = 20,
                Top = 100,
                Width = this.ClientSize.Width - 40,
                Height = this.ClientSize.Height - 180,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            cardPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(189, 195, 199), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, cardPanel.Width - 1, cardPanel.Height - 1);
                }
            };
            
            // DataGridView (RESPONSIVE)
            dgvFutterplan = new DataGridView
            {
                Left = 15,
                Top = 15,
                Width = cardPanel.Width - 30,
                Height = cardPanel.Height - 30,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Header-Styling
            dgvFutterplan.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 204, 113);
            dgvFutterplan.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFutterplan.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvFutterplan.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvFutterplan.ColumnHeadersHeight = 40;
            
            // Zellen-Styling
            dgvFutterplan.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvFutterplan.DefaultCellStyle.Padding = new Padding(5);
            dgvFutterplan.RowTemplate.Height = 35;
            dgvFutterplan.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            
            // Selection-Styling (HELLGRAU STATT BLAU!)
            dgvFutterplan.DefaultCellStyle.SelectionBackColor = Color.FromArgb(189, 195, 199);
            dgvFutterplan.DefaultCellStyle.SelectionForeColor = Color.FromArgb(44, 62, 80);
            
            cardPanel.Controls.Add(dgvFutterplan);
            this.Controls.Add(cardPanel);

            // Button-Panel (RESPONSIVE)
            buttonPanel = new Panel
            {
                Left = 20,
                Top = this.ClientSize.Height - 70,
                Width = this.ClientSize.Width - 40,
                Height = 50,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            
            CreateButtons();
            this.Controls.Add(buttonPanel);
        }

        /// <summary>
        /// Erstellt Header-Panel mit Gradient
        /// </summary>
        private Panel CreateHeaderPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(46, 204, 113)
            };
            
            panel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    panel.ClientRectangle,
                    Color.FromArgb(46, 204, 113),
                    Color.FromArgb(39, 174, 96),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }
            };

            Label lblHeader = new Label
            {
                Text = "🍽️ FÜTTERUNGSPLAN",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 12),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(lblHeader);

            lblTierInfo = new Label
            {
                Text = "Lade Tier-Informationen...",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 48),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(lblTierInfo);
            
            return panel;
        }

        /// <summary>
        /// Erstellt alle Buttons mit dynamischer Positionierung
        /// </summary>
        private void CreateButtons()
        {
            int buttonWidth = 180;
            int buttonSpacing = 15;
            int totalButtonsWidth = (buttonWidth * 4) + (buttonSpacing * 3);
            int startX = Math.Max(0, (buttonPanel.Width - totalButtonsWidth) / 2);
            
            // Wenn nicht genug Platz, Buttons kleiner machen
            if (buttonPanel.Width < totalButtonsWidth)
            {
                buttonWidth = (buttonPanel.Width - (buttonSpacing * 3)) / 4;
                startX = 0;
            }

            // Neu-Button
            btnNeu = CreateModernButton("➕ Neu", startX, 0, buttonWidth, 50, 
                Color.FromArgb(46, 204, 113));
            btnNeu.Click += btnNeu_Click;
            btnNeu.MouseEnter += (s, e) => btnNeu.BackColor = Color.FromArgb(39, 174, 96);
            btnNeu.MouseLeave += (s, e) => btnNeu.BackColor = Color.FromArgb(46, 204, 113);

            // Bearbeiten-Button
            btnBearbeiten = CreateModernButton("✏️ Bearbeiten", 
                startX + buttonWidth + buttonSpacing, 0, buttonWidth, 50, 
                Color.FromArgb(52, 152, 219));
            btnBearbeiten.Click += btnBearbeiten_Click;
            btnBearbeiten.MouseEnter += (s, e) => btnBearbeiten.BackColor = Color.FromArgb(41, 128, 185);
            btnBearbeiten.MouseLeave += (s, e) => btnBearbeiten.BackColor = Color.FromArgb(52, 152, 219);

            // Löschen-Button
            btnLoeschen = CreateModernButton("🗑️ Löschen", 
                startX + (buttonWidth + buttonSpacing) * 2, 0, buttonWidth, 50, 
                Color.FromArgb(231, 76, 60));
            btnLoeschen.Click += btnLoeschen_Click;
            btnLoeschen.MouseEnter += (s, e) => btnLoeschen.BackColor = Color.FromArgb(192, 57, 43);
            btnLoeschen.MouseLeave += (s, e) => btnLoeschen.BackColor = Color.FromArgb(231, 76, 60);

            // Schließen-Button
            btnSchliessen = CreateModernButton("❌ Schließen", 
                startX + (buttonWidth + buttonSpacing) * 3, 0, buttonWidth, 50, 
                Color.FromArgb(149, 165, 166));
            btnSchliessen.Click += (s, e) => this.Close();
            btnSchliessen.MouseEnter += (s, e) => btnSchliessen.BackColor = Color.FromArgb(127, 140, 141);
            btnSchliessen.MouseLeave += (s, e) => btnSchliessen.BackColor = Color.FromArgb(149, 165, 166);

            buttonPanel.Controls.AddRange(new Control[] { btnNeu, btnBearbeiten, btnLoeschen, btnSchliessen });
        }

        /// <summary>
        /// Passt Button-Layout bei Größenänderung an
        /// </summary>
        private void AdjustLayout()
        {
            if (buttonPanel == null || buttonPanel.Controls.Count != 4) return;
            
            int buttonWidth = 180;
            int buttonSpacing = 15;
            int totalButtonsWidth = (buttonWidth * 4) + (buttonSpacing * 3);
            int startX = Math.Max(0, (buttonPanel.Width - totalButtonsWidth) / 2);
            
            if (buttonPanel.Width < totalButtonsWidth)
            {
                buttonWidth = (buttonPanel.Width - (buttonSpacing * 3)) / 4;
                startX = 0;
            }
            
            // Buttons neu positionieren
            for (int i = 0; i < 4; i++)
            {
                buttonPanel.Controls[i].Left = startX + i * (buttonWidth + buttonSpacing);
                buttonPanel.Controls[i].Width = buttonWidth;
            }
        }

        /// <summary>
        /// Erstellt einen modernen Button
        /// </summary>
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

        #endregion

        #region Daten laden

        /// <summary>
        /// Lädt Fütterungsplan-Daten
        /// </summary>
        private void LoadFutterplan()
        {
            try
            {
                // Tier-Informationen laden
                DataTable tierInfo = db.Get(@"
                    SELECT t.Name, ta.TABezeichnung
                    FROM Tiere t
                    JOIN Tierart ta ON t.TierartID = ta.tierartID
                    WHERE t.tierID = @id",
                    ("@id", tierID));

                if (tierInfo.Rows.Count > 0)
                {
                    string tierName = tierInfo.Rows[0]["Name"].ToString();
                    string tierart = tierInfo.Rows[0]["TABezeichnung"].ToString();
                    lblTierInfo.Text = $"Tier: {tierName} | Tierart: {tierart}";
                }

                // Fütterungsplan laden
                DataTable dt = db.Get(@"
                    SELECT 
                        tf.tierart_futterID AS 'ID',
                        f.Bezeichnung AS 'Futter',
                        CONCAT(tf.Menge_pro_Tag, ' ', f.Einheit) AS 'Menge',
                        TIME_FORMAT(tf.Fuetterungszeit, '%H:%i Uhr') AS 'Fütterungszeit'
                    FROM tierart_futter tf
                    JOIN futter f ON tf.futterID = f.futterID
                    WHERE tf.tierartID = @tierartID
                    ORDER BY tf.Fuetterungszeit",
                    ("@tierartID", tierartID));

                dgvFutterplan.DataSource = dt;

                // ID-Spalte verstecken
                if (dgvFutterplan.Columns.Contains("ID"))
                {
                    dgvFutterplan.Columns["ID"].Visible = false;
                }

                // Spaltenbreiten anpassen
                if (dgvFutterplan.Columns.Contains("Futter"))
                    dgvFutterplan.Columns["Futter"].FillWeight = 150;
                if (dgvFutterplan.Columns.Contains("Menge"))
                    dgvFutterplan.Columns["Menge"].FillWeight = 80;
                if (dgvFutterplan.Columns.Contains("Fütterungszeit"))
                    dgvFutterplan.Columns["Fütterungszeit"].FillWeight = 100;

                // WICHTIG: Selektion entfernen (verhindert blaue erste Zeile!)
                if (dgvFutterplan.Rows.Count > 0)
                {
                    dgvFutterplan.ClearSelection();
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("ℹ️ Noch kein Fütterungsplan vorhanden.\nKlicke auf 'Neu' zum Erstellen.", 
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Laden:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event-Handler

        private void btnNeu_Click(object sender, EventArgs e)
        {
            FutterplanEditDialog dialog = new FutterplanEditDialog(db, tierartID);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadFutterplan();
            }
        }

        private void btnBearbeiten_Click(object sender, EventArgs e)
        {
            if (dgvFutterplan.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Bitte wähle zuerst einen Eintrag aus!", "Hinweis", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = Convert.ToInt32(dgvFutterplan.SelectedRows[0].Cells["ID"].Value);
                FutterplanEditDialog dialog = new FutterplanEditDialog(db, tierartID, id);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadFutterplan();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            if (dgvFutterplan.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Bitte wähle zuerst einen Eintrag aus!", "Hinweis", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Eintrag wirklich löschen?", "Bestätigen", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                int id = Convert.ToInt32(dgvFutterplan.SelectedRows[0].Cells["ID"].Value);
                db.Execute("DELETE FROM tierart_futter WHERE tierart_futterID = @id", ("@id", id));
                MessageBox.Show("✅ Eintrag gelöscht!", "Erfolg", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadFutterplan();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }

    #region Bearbeiten-Dialog

    internal class FutterplanEditDialog : Form
    {
        private readonly DB db;
        private readonly int tierartID;
        private readonly int? editID;
        
        private ComboBox cmbFutter;
        private NumericUpDown numMenge;
        private MaskedTextBox txtZeit;

        public FutterplanEditDialog(DB db, int tierartID, int? editID = null)
        {
            this.db = db;
            this.tierartID = tierartID;
            this.editID = editID;
            
            InitializeDialog();
            
            if (editID.HasValue)
                LoadExistingData();
        }

        private void InitializeDialog()
        {
            this.Text = editID.HasValue ? "✏️ Bearbeiten" : "➕ Neu";
            this.Size = new Size(500, 340);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(236, 240, 241);

            Panel cardPanel = new Panel
            {
                Left = 20,
                Top = 20,
                Width = 440,
                Height = 250,
                BackColor = Color.White
            };

            int yPos = 20;

            // Futter
            Label lblFutter = new Label
            {
                Text = "🍖 Futter:",
                Left = 20,
                Top = yPos,
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            cardPanel.Controls.Add(lblFutter);
            yPos += 30;

            cmbFutter = new ComboBox
            {
                Left = 20,
                Top = yPos,
                Width = 400,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11F)
            };
            cardPanel.Controls.Add(cmbFutter);
            yPos += 50;

            // Menge
            Label lblMenge = new Label
            {
                Text = "📦 Menge pro Tag:",
                Left = 20,
                Top = yPos,
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            cardPanel.Controls.Add(lblMenge);
            yPos += 30;

            numMenge = new NumericUpDown
            {
                Left = 20,
                Top = yPos,
                Width = 180,
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 1000,
                Value = 5,
                Font = new Font("Segoe UI", 11F)
            };
            cardPanel.Controls.Add(numMenge);
            yPos += 50;

            // Zeit
            Label lblZeit = new Label
            {
                Text = "🕐 Fütterungszeit:",
                Left = 20,
                Top = yPos,
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            cardPanel.Controls.Add(lblZeit);
            yPos += 30;

            txtZeit = new MaskedTextBox
            {
                Left = 20,
                Top = yPos,
                Width = 120,
                Mask = "00:00",
                Text = "08:00",
                Font = new Font("Segoe UI", 11F),
                TextAlign = HorizontalAlignment.Center
            };
            cardPanel.Controls.Add(txtZeit);

            this.Controls.Add(cardPanel);

            // Buttons
            Button btnSpeichern = new Button
            {
                Text = "💾 Speichern",
                Left = 155,
                Top = 280,
                Width = 150,
                Height = 45,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSpeichern.FlatAppearance.BorderSize = 0;
            btnSpeichern.Click += btnSpeichern_Click;
            this.Controls.Add(btnSpeichern);

            Button btnAbbrechen = new Button
            {
                Text = "❌ Abbrechen",
                Left = 315,
                Top = 280,
                Width = 150,
                Height = 45,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAbbrechen.FlatAppearance.BorderSize = 0;
            btnAbbrechen.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnAbbrechen);

            LoadFutterComboBox();
        }

        private void LoadFutterComboBox()
        {
            try
            {
                DataTable dt = db.Get("SELECT futterID, Bezeichnung, Einheit FROM Futter ORDER BY Bezeichnung");
                
                cmbFutter.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbFutter.Items.Add(new FutterItem
                    {
                        FutterID = Convert.ToInt32(row["futterID"]),
                        Bezeichnung = row["Bezeichnung"].ToString(),
                        Einheit = row["Einheit"].ToString()
                    });
                }
                cmbFutter.DisplayMember = "Display";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler");
            }
        }

        private void LoadExistingData()
        {
            try
            {
                DataTable dt = db.Get(@"
                    SELECT futterID, Menge_pro_Tag, TIME_FORMAT(Fuetterungszeit, '%H:%i') AS Zeit
                    FROM tierart_futter
                    WHERE tierart_futterID = @id",
                    ("@id", editID.Value));

                if (dt.Rows.Count > 0)
                {
                    int futterID = Convert.ToInt32(dt.Rows[0]["futterID"]);
                    numMenge.Value = Convert.ToDecimal(dt.Rows[0]["Menge_pro_Tag"]);
                    txtZeit.Text = dt.Rows[0]["Zeit"].ToString();

                    foreach (FutterItem item in cmbFutter.Items)
                    {
                        if (item.FutterID == futterID)
                        {
                            cmbFutter.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            if (cmbFutter.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Bitte Futter auswählen!", "Fehler");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtZeit.Text) || txtZeit.Text.Contains("_"))
            {
                MessageBox.Show("⚠️ Bitte gültige Uhrzeit eingeben!", "Fehler");
                return;
            }

            try
            {
                int futterID = ((FutterItem)cmbFutter.SelectedItem).FutterID;
                decimal menge = numMenge.Value;
                string zeit = txtZeit.Text + ":00";

                if (editID.HasValue)
                {
                    db.Execute(@"UPDATE tierart_futter 
                        SET futterID = @futter, Menge_pro_Tag = @menge, Fuetterungszeit = @zeit
                        WHERE tierart_futterID = @id",
                        ("@futter", futterID), ("@menge", menge), ("@zeit", zeit), ("@id", editID.Value));
                    MessageBox.Show("✅ Aktualisiert!", "Erfolg");
                }
                else
                {
                    db.Execute(@"INSERT INTO tierart_futter (tierartID, futterID, Menge_pro_Tag, Fuetterungszeit)
                        VALUES (@tierart, @futter, @menge, @zeit)",
                        ("@tierart", tierartID), ("@futter", futterID), ("@menge", menge), ("@zeit", zeit));
                    MessageBox.Show("✅ Hinzugefügt!", "Erfolg");
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler:\n{ex.Message}", "Fehler");
            }
        }

        private class FutterItem
        {
            public int FutterID { get; set; }
            public string Bezeichnung { get; set; }
            public string Einheit { get; set; }
            public string Display => $"{Bezeichnung} ({Einheit})";
        }
    }

    #endregion
}
