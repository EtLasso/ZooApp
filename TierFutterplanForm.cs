using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ZooApp
{
    // Formular zum Anzeigen und Bearbeiten des Fütterungsplans für ein Tier
    public class TierFutterplanForm : Form
    {
        private readonly DB db = new DB();
        private readonly int tierID;
        private readonly int tierartID;
        
        private DataGridView dgvFutterplan;
        private Button btnNeu;
        private Button btnBearbeiten;
        private Button btnLoeschen;
        private Button btnSchliessen;
        private Label lblTierInfo;

        public TierFutterplanForm(int tierID, int tierartID)
        {
            this.tierID = tierID;
            this.tierartID = tierartID;
            InitializeControls();
            LoadFutterplan();
        }

        // Initialisiert alle Controls
        private void InitializeControls()
        {
            // Formular-Einstellungen
            this.Text = "🍽️ Fütterungsplan";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Header-Panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(46, 204, 113)
            };

            Label lblHeader = new Label
            {
                Text = "🍽️ FÜTTERUNGSPLAN",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 10)
            };
            headerPanel.Controls.Add(lblHeader);

            // Tier-Info
            lblTierInfo = new Label
            {
                Text = "Lade Tier-Informationen...",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 45)
            };
            headerPanel.Controls.Add(lblTierInfo);

            this.Controls.Add(headerPanel);

            // DataGridView für Fütterungsplan
            dgvFutterplan = new DataGridView
            {
                Left = 20,
                Top = 100,
                Width = 840,
                Height = 380,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvFutterplan);

            // Button-Panel
            Panel buttonPanel = new Panel
            {
                Left = 20,
                Top = 490,
                Width = 840,
                Height = 60
            };

            btnNeu = new Button
            {
                Text = "➕ Neu",
                Left = 0,
                Top = 10,
                Width = 150,
                Height = 40,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNeu.Click += btnNeu_Click;
            buttonPanel.Controls.Add(btnNeu);

            btnBearbeiten = new Button
            {
                Text = "✏️ Bearbeiten",
                Left = 160,
                Top = 10,
                Width = 150,
                Height = 40,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBearbeiten.Click += btnBearbeiten_Click;
            buttonPanel.Controls.Add(btnBearbeiten);

            btnLoeschen = new Button
            {
                Text = "🗑️ Löschen",
                Left = 320,
                Top = 10,
                Width = 150,
                Height = 40,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLoeschen.Click += btnLoeschen_Click;
            buttonPanel.Controls.Add(btnLoeschen);

            btnSchliessen = new Button
            {
                Text = "❌ Schließen",
                Left = 690,
                Top = 10,
                Width = 150,
                Height = 40,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSchliessen.Click += (s, e) => this.Close();
            buttonPanel.Controls.Add(btnSchliessen);

            this.Controls.Add(buttonPanel);
        }

        // Lädt den Fütterungsplan
        private void LoadFutterplan()
        {
            try
            {
                // Tier-Info laden
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
                        tf.Menge_pro_Tag AS 'Menge',
                        f.Einheit AS 'Einheit',
                        TIME_FORMAT(tf.Fuetterungszeit, '%H:%i') AS 'Fuetterungszeit'
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

                // Hinweis wenn keine Einträge
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("ℹ️ Für diese Tierart ist noch kein Fütterungsplan angelegt.\n\n" +
                        "Klicke auf 'Neu' um einen Eintrag hinzuzufügen.", 
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Laden:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Neuen Eintrag hinzufügen
        private void btnNeu_Click(object sender, EventArgs e)
        {
            FutterplanEditDialog dialog = new FutterplanEditDialog(db, tierartID);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadFutterplan(); // Neu laden
            }
        }

        // Bestehenden Eintrag bearbeiten
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
                    LoadFutterplan(); // Neu laden
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Eintrag löschen
        private void btnLoeschen_Click(object sender, EventArgs e)
        {
            if (dgvFutterplan.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Bitte wähle zuerst einen Eintrag aus!", "Hinweis", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Diesen Fütterungsplan-Eintrag wirklich löschen?", 
                "Löschen bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                int id = Convert.ToInt32(dgvFutterplan.SelectedRows[0].Cells["ID"].Value);
                
                db.Execute("DELETE FROM tierart_futter WHERE tierart_futterID = @id", ("@id", id));
                
                MessageBox.Show("✅ Eintrag gelöscht!", "Erfolg", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadFutterplan(); // Neu laden
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Löschen:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Dialog zum Hinzufügen/Bearbeiten von Fütterungsplan-Einträgen
    internal class FutterplanEditDialog : Form
    {
        private readonly DB db;
        private readonly int tierartID;
        private readonly int? editID; // null = Neu, sonst = Bearbeiten
        
        private ComboBox cmbFutter;
        private NumericUpDown numMenge;
        private MaskedTextBox txtZeit;
        private Button btnSpeichern;
        private Button btnAbbrechen;

        public FutterplanEditDialog(DB db, int tierartID, int? editID = null)
        {
            this.db = db;
            this.tierartID = tierartID;
            this.editID = editID;
            
            InitializeDialog();
            
            if (editID.HasValue)
            {
                LoadExistingData();
            }
        }

        // Dialog initialisieren
        private void InitializeDialog()
        {
            this.Text = editID.HasValue ? "✏️ Fütterungsplan bearbeiten" : "➕ Fütterungsplan hinzufügen";
            this.Size = new Size(450, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int yPos = 20;

            // Futter
            Label lblFutter = new Label
            {
                Text = "Futter:",
                Left = 20,
                Top = yPos,
                Width = 100,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(lblFutter);

            cmbFutter = new ComboBox
            {
                Left = 130,
                Top = yPos - 3,
                Width = 280,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(cmbFutter);
            yPos += 50;

            // Menge
            Label lblMenge = new Label
            {
                Text = "Menge pro Tag:",
                Left = 20,
                Top = yPos,
                Width = 100,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(lblMenge);

            numMenge = new NumericUpDown
            {
                Left = 130,
                Top = yPos - 3,
                Width = 150,
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 1000,
                Value = 5,
                Increment = 0.5m,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(numMenge);
            yPos += 50;

            // Fütterungszeit
            Label lblZeit = new Label
            {
                Text = "Fütterungszeit:",
                Left = 20,
                Top = yPos,
                Width = 100,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(lblZeit);

            txtZeit = new MaskedTextBox
            {
                Left = 130,
                Top = yPos - 3,
                Width = 100,
                Mask = "00:00",
                Text = "08:00",
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(txtZeit);

            Label lblZeitHinweis = new Label
            {
                Text = "(HH:MM)",
                Left = 240,
                Top = yPos,
                Width = 100,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblZeitHinweis);
            yPos += 60;

            // Buttons
            btnSpeichern = new Button
            {
                Text = "💾 Speichern",
                Left = 130,
                Top = yPos,
                Width = 130,
                Height = 40,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSpeichern.Click += btnSpeichern_Click;
            this.Controls.Add(btnSpeichern);

            btnAbbrechen = new Button
            {
                Text = "❌ Abbrechen",
                Left = 270,
                Top = yPos,
                Width = 130,
                Height = 40,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAbbrechen.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnAbbrechen);

            // Futter-Liste laden
            LoadFutterComboBox();
        }

        // Lädt Futter in ComboBox
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
                cmbFutter.ValueMember = "FutterID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Futtersorten:\n{ex.Message}", "Fehler");
            }
        }

        // Lädt vorhandene Daten beim Bearbeiten
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

                    // Futter auswählen
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
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden:\n{ex.Message}", "Fehler");
            }
        }

        // Speichern
        private void btnSpeichern_Click(object sender, EventArgs e)
        {
            // Validierung
            if (cmbFutter.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Bitte Futter auswählen!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtZeit.Text) || txtZeit.Text.Contains("_"))
            {
                MessageBox.Show("⚠️ Bitte gültige Uhrzeit eingeben!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int futterID = ((FutterItem)cmbFutter.SelectedItem).FutterID;
                decimal menge = numMenge.Value;
                string zeit = txtZeit.Text + ":00"; // Sekunden hinzufügen

                if (editID.HasValue)
                {
                    // UPDATE
                    db.Execute(@"
                        UPDATE tierart_futter 
                        SET futterID = @futter, Menge_pro_Tag = @menge, Fuetterungszeit = @zeit
                        WHERE tierart_futterID = @id",
                        ("@futter", futterID),
                        ("@menge", menge),
                        ("@zeit", zeit),
                        ("@id", editID.Value));

                    MessageBox.Show("✅ Eintrag aktualisiert!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // INSERT
                    db.Execute(@"
                        INSERT INTO tierart_futter (tierartID, futterID, Menge_pro_Tag, Fuetterungszeit)
                        VALUES (@tierart, @futter, @menge, @zeit)",
                        ("@tierart", tierartID),
                        ("@futter", futterID),
                        ("@menge", menge),
                        ("@zeit", zeit));

                    MessageBox.Show("✅ Eintrag hinzugefügt!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler beim Speichern:\n{ex.Message}", "Fehler", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hilfsklasse für ComboBox
        private class FutterItem
        {
            public int FutterID { get; set; }
            public string Bezeichnung { get; set; }
            public string Einheit { get; set; }
            public string Display => $"{Bezeichnung} ({Einheit})";
        }
    }
}
