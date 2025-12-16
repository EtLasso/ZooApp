using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace ZooApp
{
    /// <summary>
    /// Pfleger-Detailformular mit allen Informationen
    /// Zeigt ID-Karte, persönliche Daten, Arbeitsinformationen und Zuordnungen
    /// </summary>
    public class PflegerDetailForm : Form
    {
        // Datenbankverbindung
        private readonly DB db = new DB();
        private readonly int pflegerID;  // ID des aktuell bearbeiteten Pflegers
        private string currentFotoPath = "";  // Pfad zum Profilfoto

        // Hauptpanels für die 4 Bereiche
        private Panel idPanel, personalPanel, arbeitPanel, zuordnungenPanel;
        
        // ID-Karte Bereich
        private PictureBox pbFoto;  // Profilfoto
        private Label lblPersonalnummer, lblPflegerName, lblPosition, lblRolle;
        private Button btnFotoHochladen;
        
        // Persönliche Daten Bereich
        private TextBox txtVorname, txtNachname, txtTelefon, txtEmail, txtAdresse, txtNotfallkontakt;
        private DateTimePicker dtpGeburtsdatum;
        private Label lblAlter;  // Zeigt berechnetes Alter
        
        // Arbeit & Finanzen Bereich
        private NumericUpDown numGehalt, numWochenstunden, numUrlaubstage, numResturlaub;
        private DateTimePicker dtpEinstellung;
        private ComboBox cmbSteuerklasse, cmbPosition, cmbRolle, cmbArbeitszeitmodell, cmbHauptpfleger;
        private TextBox txtSV, txtIBAN, txtKrankenkasse, txtAusweis, txtQualifikationen, txtFortbildungen;
        private Label lblAnstellungsdauer, lblKrankentage;  // Zeigt berechnete Jahre
        private CheckBox chkIstHauptpfleger, chkAktiv, chkAccountAktiv;
        
        // Zuordnungen Bereich
        private ListBox lbAssistenten, lbTiere;  // Listen für Assistenten und Tiere
        private CheckedListBox clbAufgaben;  // Aufgabenbereiche mit Checkboxen
        private Button btnTierHinzu, btnAufgabeHinzu;
        
        // Aktions-Buttons
        private Button btnSpeichern, btnSchliessen;

        /// <summary>
        /// Konstruktor - Erstellt Formular für spezifischen Pfleger
        /// </summary>
        /// <param name="pflegerID">ID des zu bearbeitenden Pflegers</param>
        public PflegerDetailForm(int pflegerID)
        {
            this.pflegerID = pflegerID;
            InitializeUI();      // UI-Elemente erstellen
            LoadPflegerData();   // Daten aus DB laden
        }

        /// <summary>
        /// Erstellt die komplette Benutzeroberfläche
        /// Layout: Header + 4 Panels (ID, Personal, Arbeit, Zuordnungen) + Buttons
        /// </summary>
        private void InitializeUI()
        {
            // Formular
            this.Text = "👨‍🌾 Pfleger-Profil";
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
                    Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185), 0f))
                    e.Graphics.FillRectangle(brush, header.ClientRectangle);
            };
            Label lblHeader = new Label
            {
                Text = "👨‍🌾 PFLEGER-PROFIL VERWALTUNG",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 22),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            header.Controls.Add(lblHeader);
            this.Controls.Add(header);

            // ID-Karte (Spalte 1, Zeile 1)
            idPanel = CreateCard(20, 100, 380, 320);
            CreateIDCard();
            this.Controls.Add(idPanel);

            // Persönliche Daten (Spalte 1, Zeile 2)
            personalPanel = CreateCard(20, 440, 380, 520);
            personalPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CreatePersonalSection();
            this.Controls.Add(personalPanel);

            // Arbeit & Finanzen (Spalte 2)
            arbeitPanel = CreateCard(420, 100, 540, 760);
            arbeitPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CreateArbeitSection();
            this.Controls.Add(arbeitPanel);

            // Zuordnungen (Spalte 3)
            zuordnungenPanel = CreateCard(980, 100, this.ClientSize.Width - 1010, 760);
            zuordnungenPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CreateZuordnungenSection();
            this.Controls.Add(zuordnungenPanel);

            // Buttons
            CreateButtons();
        }

        /// <summary>
        /// Erstellt ID-Karten-Bereich (links oben)
        /// Enthält: Foto, Personalnummer, Name, Position, Rolle, Status
        /// </summary>
        private void CreateIDCard()
        {
            // Header
            Panel hdr = new Panel { Width = idPanel.Width, Height = 45, BackColor = Color.FromArgb(44, 62, 80), Dock = DockStyle.Top };
            hdr.Controls.Add(new Label
            {
                Text = "🆔 MITARBEITER-AUSWEIS",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            });
            idPanel.Controls.Add(hdr);

            // Foto
            pbFoto = new PictureBox
            {
                Left = 20, Top = 60, Width = 140, Height = 180,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(230, 230, 230)
            };
            pbFoto.Paint += (s, e) =>
            {
                if (pbFoto.Image == null)
                {
                    e.Graphics.DrawString("👤", new Font("Segoe UI", 60F), Brushes.Gray, 25, 40);
                    e.Graphics.DrawString("Kein Foto", new Font("Segoe UI", 9F), Brushes.Gray, 30, 150);
                }
            };
            idPanel.Controls.Add(pbFoto);

            btnFotoHochladen = new Button
            {
                Text = "📷 Foto",
                Left = 20, Top = 245, Width = 140, Height = 32,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnFotoHochladen.FlatAppearance.BorderSize = 0;
            btnFotoHochladen.Click += BtnFotoHochladen_Click;
            idPanel.Controls.Add(btnFotoHochladen);

            // Info rechts vom Foto
            lblPersonalnummer = new Label
            {
                Text = "#PFL0000",
                Left = 175, Top = 60, Width = 180,
                Font = new Font("Consolas", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            idPanel.Controls.Add(lblPersonalnummer);

            lblPflegerName = new Label
            {
                Text = "Name",
                Left = 175, Top = 90, Width = 180, Height = 60,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            idPanel.Controls.Add(lblPflegerName);

            lblPosition = new Label
            {
                Text = "Position",
                Left = 175, Top = 155, Width = 180,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113)
            };
            idPanel.Controls.Add(lblPosition);

            lblRolle = new Label
            {
                Text = "Rolle",
                Left = 175, Top = 180, Width = 180,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray
            };
            idPanel.Controls.Add(lblRolle);

            // Status-Checkboxen unten
            chkAktiv = new CheckBox { Text = "✅ Aktiv", Left = 175, Top = 215, Width = 85, Font = new Font("Segoe UI", 8F) };
            chkAccountAktiv = new CheckBox { Text = "🔐 Login", Left = 270, Top = 215, Width = 85, Font = new Font("Segoe UI", 8F) };
            idPanel.Controls.AddRange(new Control[] { chkAktiv, chkAccountAktiv });
        }

        /// <summary>
        /// Erstellt Bereich für persönliche Daten (links unten)
        /// Enthält: Name, Geburtsdatum, Telefon, Email, Notfallkontakt
        /// </summary>
        private void CreatePersonalSection()
        {
            AddHeader(personalPanel, "👤 PERSÖNLICH", Color.FromArgb(52, 152, 219));
            int y = 70;
            
            // Vorname
            personalPanel.Controls.Add(new Label { Text = "Vorname", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtVorname = new TextBox { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F) };
            personalPanel.Controls.Add(txtVorname);
            y += 75;
            
            // Nachname
            personalPanel.Controls.Add(new Label { Text = "Nachname", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtNachname = new TextBox { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F) };
            personalPanel.Controls.Add(txtNachname);
            y += 75;
            
            // Geburtsdatum
            personalPanel.Controls.Add(new Label { Text = "🎂 Geburtsdatum", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            dtpGeburtsdatum = new DateTimePicker { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F), Format = DateTimePickerFormat.Short };
            dtpGeburtsdatum.ValueChanged += (s, e) => UpdateAlter();
            personalPanel.Controls.Add(dtpGeburtsdatum);
            lblAlter = new Label { Text = "0 Jahre", Left = 25, Top = y + 55, Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = Color.Gray };
            personalPanel.Controls.Add(lblAlter);
            y += 95;
            
            // Telefon
            personalPanel.Controls.Add(new Label { Text = "📞 Telefon", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtTelefon = new TextBox { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F) };
            personalPanel.Controls.Add(txtTelefon);
            y += 75;
            
            // Email
            personalPanel.Controls.Add(new Label { Text = "📧 Email", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtEmail = new TextBox { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F) };
            personalPanel.Controls.Add(txtEmail);
            y += 75;
            
            // Notfallkontakt
            personalPanel.Controls.Add(new Label { Text = "🚨 Notfallkontakt", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtNotfallkontakt = new TextBox { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F) };
            personalPanel.Controls.Add(txtNotfallkontakt);
        }

        /// <summary>
        /// Erstellt Bereich für Arbeit & Finanzen (Mitte)
        /// Enthält: Gehalt, Position, Steuer, Urlaub, Arbeitszeit, Qualifikationen
        /// </summary>
        private void CreateArbeitSection()
        {
            AddHeader(arbeitPanel, "💼 ARBEIT & FINANZEN", Color.FromArgb(46, 204, 113));
            int y = 70;
            
            // Gehalt
            arbeitPanel.Controls.Add(new Label { Text = "💰 Gehalt (monatlich)", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            numGehalt = new NumericUpDown { Left = 25, Top = y + 25, Width = 200, Font = new Font("Segoe UI", 10F), DecimalPlaces = 2, Maximum = 20000, Value = 2500 };
            arbeitPanel.Controls.Add(numGehalt);
            arbeitPanel.Controls.Add(new Label { Text = "€", Left = 235, Top = y + 28, Font = new Font("Segoe UI", 10F, FontStyle.Bold) });
            y += 65;

            // Position
            arbeitPanel.Controls.Add(new Label { Text = "🏆 Position", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            cmbPosition = new ComboBox { Left = 25, Top = y + 25, Width = 240, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPosition.Items.AddRange(new[] { "Hauptpfleger", "Pfleger", "Auszubildender", "Tierarzt", "Reinigungskraft", "Führungskraft" });
            arbeitPanel.Controls.Add(cmbPosition);
            y += 65;

            // Rolle
            arbeitPanel.Controls.Add(new Label { Text = "👔 Rolle", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            cmbRolle = new ComboBox { Left = 25, Top = y + 25, Width = 240, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRolle.Items.AddRange(new[] { "Admin", "Supervisor", "Hauptpfleger", "Pfleger", "Auszubildender", "Tierarzt", "Leser" });
            arbeitPanel.Controls.Add(cmbRolle);
            y += 65;

            // Steuerklasse
            arbeitPanel.Controls.Add(new Label { Text = "📊 Steuerklasse", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            cmbSteuerklasse = new ComboBox { Left = 25, Top = y + 25, Width = 240, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbSteuerklasse.Items.AddRange(new[] { "1 - Ledig", "2 - Alleinerziehend", "3 - Verheiratet (höher)", "4 - Verheiratet", "5 - Verheiratet (niedriger)", "6 - Zweitjob" });
            arbeitPanel.Controls.Add(cmbSteuerklasse);
            y += 65;

            // SV-Nummer, IBAN, Krankenkasse (Spalte 2)
            AddFieldSecondCol("🔒 SV-Nummer", ref txtSV, 70);
            AddFieldSecondCol("🏦 IBAN", ref txtIBAN, 135);
            AddFieldSecondCol("🏥 Krankenkasse", ref txtKrankenkasse, 200);
            AddFieldSecondCol("🆔 Ausweis-Nr", ref txtAusweis, 265);

            // Einstellungsdatum
            arbeitPanel.Controls.Add(new Label { Text = "📅 Einstellung", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            dtpEinstellung = new DateTimePicker { Left = 25, Top = y + 25, Width = 240, Font = new Font("Segoe UI", 10F), Format = DateTimePickerFormat.Short };
            dtpEinstellung.ValueChanged += (s, e) => UpdateAnstellungsdauer();
            arbeitPanel.Controls.Add(dtpEinstellung);
            lblAnstellungsdauer = new Label { Text = "0 Jahre", Left = 25, Top = y + 55, Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = Color.Gray };
            arbeitPanel.Controls.Add(lblAnstellungsdauer);
            y += 90;

            // Arbeitszeit
            arbeitPanel.Controls.Add(new Label { Text = "⏰ Arbeitszeitmodell", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            cmbArbeitszeitmodell = new ComboBox { Left = 25, Top = y + 25, Width = 150, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbArbeitszeitmodell.Items.AddRange(new[] { "Vollzeit", "Teilzeit", "Minijob", "Auszubildend" });
            arbeitPanel.Controls.Add(cmbArbeitszeitmodell);
            
            arbeitPanel.Controls.Add(new Label { Text = "🕐 Wochenstunden", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(185, y), AutoSize = true });
            numWochenstunden = new NumericUpDown { Left = 185, Top = y + 25, Width = 80, Font = new Font("Segoe UI", 10F), DecimalPlaces = 2, Maximum = 60, Value = 40 };
            arbeitPanel.Controls.Add(numWochenstunden);
            y += 65;

            // Urlaub
            arbeitPanel.Controls.Add(new Label { Text = "🏖️ Urlaubstage/Jahr", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            numUrlaubstage = new NumericUpDown { Left = 25, Top = y + 25, Width = 100, Font = new Font("Segoe UI", 10F), Maximum = 50, Value = 30 };
            arbeitPanel.Controls.Add(numUrlaubstage);
            
            arbeitPanel.Controls.Add(new Label { Text = "Resturlaub", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(145, y), AutoSize = true });
            numResturlaub = new NumericUpDown { Left = 145, Top = y + 25, Width = 80, Font = new Font("Segoe UI", 10F), Maximum = 50, Value = 30 };
            arbeitPanel.Controls.Add(numResturlaub);
            y += 65;

            // Qualifikationen, Fortbildungen
            arbeitPanel.Controls.Add(new Label { Text = "🎓 Qualifikationen", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtQualifikationen = new TextBox { Left = 25, Top = y + 25, Width = 480, Height = 50, Font = new Font("Segoe UI", 9F), Multiline = true, ScrollBars = ScrollBars.Vertical };
            arbeitPanel.Controls.Add(txtQualifikationen);
            y += 85;

            arbeitPanel.Controls.Add(new Label { Text = "📚 Fortbildungen", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            txtFortbildungen = new TextBox { Left = 25, Top = y + 25, Width = 480, Height = 50, Font = new Font("Segoe UI", 9F), Multiline = true, ScrollBars = ScrollBars.Vertical };
            arbeitPanel.Controls.Add(txtFortbildungen);
        }

        /// <summary>
        /// Erstellt Bereich für Zuordnungen (rechts)
        /// Enthält: Vorgesetzter, Assistenten, Tiere, Aufgabenbereiche
        /// </summary>
        private void CreateZuordnungenSection()
        {
            AddHeader(zuordnungenPanel, "🎯 ZUORDNUNGEN", Color.FromArgb(155, 89, 182));
            int y = 70;

            // Hierarchie
            chkIstHauptpfleger = new CheckBox
            {
                Text = "🏆 Ist Hauptpfleger / Teamleiter",
                Left = 25, Top = y, Width = zuordnungenPanel.Width - 60,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113)
            };
            chkIstHauptpfleger.CheckedChanged += (s, e) =>
            {
                cmbHauptpfleger.Enabled = !chkIstHauptpfleger.Checked;
                lbAssistenten.Enabled = chkIstHauptpfleger.Checked;
                if (chkIstHauptpfleger.Checked) LoadAssistenten();
            };
            zuordnungenPanel.Controls.Add(chkIstHauptpfleger);
            y += 35;

            zuordnungenPanel.Controls.Add(new Label { Text = "👔 Vorgesetzter", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            cmbHauptpfleger = new ComboBox { Left = 25, Top = y + 25, Width = zuordnungenPanel.Width - 60, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            zuordnungenPanel.Controls.Add(cmbHauptpfleger);
            y += 65;

            zuordnungenPanel.Controls.Add(new Label { Text = "👥 Meine Assistenten", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            lbAssistenten = new ListBox { Left = 25, Top = y + 25, Width = zuordnungenPanel.Width - 60, Height = 100, Font = new Font("Segoe UI", 9F), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            zuordnungenPanel.Controls.Add(lbAssistenten);
            y += 135;

            // Aufgabenbereiche
            zuordnungenPanel.Controls.Add(new Label { Text = "📋 Aufgabenbereiche", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            clbAufgaben = new CheckedListBox { Left = 25, Top = y + 25, Width = zuordnungenPanel.Width - 60, Height = 140, Font = new Font("Segoe UI", 9F), CheckOnClick = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            zuordnungenPanel.Controls.Add(clbAufgaben);
            y += 175;

            // Tiere
            zuordnungenPanel.Controls.Add(new Label { Text = "🐾 Betreute Tiere", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(25, y), AutoSize = true });
            lbTiere = new ListBox { Left = 25, Top = y + 25, Width = zuordnungenPanel.Width - 140, Height = 200, Font = new Font("Segoe UI", 9F), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            zuordnungenPanel.Controls.Add(lbTiere);
            
            btnTierHinzu = new Button
            {
                Text = "➕",
                Left = zuordnungenPanel.Width - 105, Top = y + 25, Width = 80, Height = 35,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnTierHinzu.FlatAppearance.BorderSize = 0;
            btnTierHinzu.Click += BtnTierHinzu_Click;
            zuordnungenPanel.Controls.Add(btnTierHinzu);
        }

        /// <summary>
        /// Erstellt Speichern- und Schließen-Buttons am unteren Rand
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
        /// Lädt alle Daten des Pflegers aus der Datenbank
        /// Füllt alle Felder mit den aktuellen Werten
        /// </summary>
        private void LoadPflegerData()
        {
            try
            {
                DataTable dt = db.Get("SELECT * FROM pfleger WHERE pflegerID = @id", ("@id", pflegerID));
                if (dt.Rows.Count == 0) { this.Close(); return; }

                DataRow r = dt.Rows[0];
                txtVorname.Text = r["Vorname"].ToString();
                txtNachname.Text = r["Nachname"].ToString();
                txtTelefon.Text = r["Telefon"]?.ToString() ?? "";
                txtEmail.Text = r["Email"]?.ToString() ?? "";
                txtNotfallkontakt.Text = r["Notfallkontakt"]?.ToString() ?? "";
                
                if (r["Geburtsdatum"] != DBNull.Value)
                {
                    dtpGeburtsdatum.Value = Convert.ToDateTime(r["Geburtsdatum"]);
                    UpdateAlter();
                }

                numGehalt.Value = r["Gehalt"] != DBNull.Value ? Convert.ToDecimal(r["Gehalt"]) : 0;
                dtpEinstellung.Value = Convert.ToDateTime(r["Einstellungsdatum"]);
                UpdateAnstellungsdauer();

                lblPersonalnummer.Text = "#" + r["Personalnummer"].ToString();
                
                if (r["Position"] != DBNull.Value) cmbPosition.SelectedItem = r["Position"].ToString();
                if (r["Rolle"] != DBNull.Value) cmbRolle.SelectedItem = r["Rolle"].ToString();
                if (r["Steuerklasse"] != DBNull.Value) cmbSteuerklasse.SelectedIndex = Convert.ToInt32(r["Steuerklasse"]) - 1;
                
                txtSV.Text = r["Sozialversicherungsnummer"]?.ToString() ?? "";
                txtIBAN.Text = r["Bankverbindung"]?.ToString() ?? "";
                txtKrankenkasse.Text = r["Krankenkasse"]?.ToString() ?? "";
                txtAusweis.Text = r["Ausweis_Nr"]?.ToString() ?? "";
                
                if (r["Arbeitszeitmodell"] != DBNull.Value) cmbArbeitszeitmodell.SelectedItem = r["Arbeitszeitmodell"].ToString();
                numWochenstunden.Value = r["Wochenstunden"] != DBNull.Value ? Convert.ToDecimal(r["Wochenstunden"]) : 40;
                numUrlaubstage.Value = r["Urlaubstage_pro_Jahr"] != DBNull.Value ? Convert.ToInt32(r["Urlaubstage_pro_Jahr"]) : 30;
                numResturlaub.Value = r["Resturlaub"] != DBNull.Value ? Convert.ToInt32(r["Resturlaub"]) : 30;
                
                txtQualifikationen.Text = r["Qualifikationen"]?.ToString() ?? "";
                txtFortbildungen.Text = r["Fortbildungen"]?.ToString() ?? "";
                
                chkIstHauptpfleger.Checked = r["IstHauptpfleger"] != DBNull.Value && Convert.ToBoolean(r["IstHauptpfleger"]);
                chkAktiv.Checked = r["Aktiv"] != DBNull.Value && Convert.ToBoolean(r["Aktiv"]);
                chkAccountAktiv.Checked = r["Account_aktiv"] != DBNull.Value && Convert.ToBoolean(r["Account_aktiv"]);

                UpdateIDCard();
                currentFotoPath = r["Foto"]?.ToString() ?? "";
                LoadFoto();
                LoadDropdowns();
                LoadAssistenten();
                LoadAufgaben();
                LoadTiere();

                this.Text = $"👨‍🌾 {r["Vorname"]} {r["Nachname"]}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lädt Vorgesetzte in Dropdown
        private void LoadDropdowns()
        {
            try
            {
                DataTable dt = db.Get("SELECT pflegerID, CONCAT(Vorname, ' ', Nachname) AS Name FROM pfleger WHERE IstHauptpfleger = 1 AND pflegerID != @id ORDER BY Nachname", ("@id", pflegerID));
                cmbHauptpfleger.Items.Clear();
                cmbHauptpfleger.Items.Add("(Kein Vorgesetzter)");
                foreach (DataRow r in dt.Rows)
                    cmbHauptpfleger.Items.Add(new ComboItem { Value = Convert.ToInt32(r["pflegerID"]), Text = r["Name"].ToString() });
                cmbHauptpfleger.SelectedIndex = 0;
            }
            catch { }
        }

        // Lädt Liste der Assistenten (wenn Hauptpfleger)
        private void LoadAssistenten()
        {
            lbAssistenten.Items.Clear();
            if (!chkIstHauptpfleger.Checked) return;
            try
            {
                DataTable dt = db.Get("SELECT CONCAT(Vorname, ' ', Nachname, ' (', Personalnummer, ')') AS Info FROM pfleger WHERE HauptpflegerID = @id ORDER BY Nachname", ("@id", pflegerID));
                foreach (DataRow r in dt.Rows) lbAssistenten.Items.Add(r["Info"].ToString());
                if (lbAssistenten.Items.Count == 0) lbAssistenten.Items.Add("(Keine Assistenten)");
            }
            catch { }
        }

        // Lädt Aufgabenbereiche mit Checkboxen
        private void LoadAufgaben()
        {
            try
            {
                DataTable aufg = db.Get("SELECT * FROM aufgabenbereich ORDER BY Bezeichnung");
                DataTable zug = db.Get("SELECT aufgabenbereichID FROM pfleger_aufgabenbereich WHERE pflegerID = @id", ("@id", pflegerID));
                var ids = new System.Collections.Generic.HashSet<int>();
                foreach (DataRow r in zug.Rows) ids.Add(Convert.ToInt32(r["aufgabenbereichID"]));
                clbAufgaben.Items.Clear();
                foreach (DataRow r in aufg.Rows)
                {
                    int id = Convert.ToInt32(r["aufgabenbereichID"]);
                    clbAufgaben.Items.Add(new ComboItem { Value = id, Text = r["Bezeichnung"].ToString() }, ids.Contains(id));
                }
            }
            catch { }
        }

        // Lädt zugeordnete Tiere
        private void LoadTiere()
        {
            try
            {
                DataTable dt = db.Get(@"SELECT t.Name, ta.TABezeichnung AS Tierart, CASE WHEN pt.IstHauptpfleger THEN '(Haupt)' ELSE '(Ass)' END AS Rolle
                    FROM pfleger_tier pt JOIN Tiere t ON pt.tierID = t.tierID JOIN Tierart ta ON t.TierartID = ta.tierartID
                    WHERE pt.pflegerID = @id ORDER BY t.Name", ("@id", pflegerID));
                lbTiere.Items.Clear();
                foreach (DataRow r in dt.Rows) lbTiere.Items.Add($"{r["Name"]} ({r["Tierart"]}) {r["Rolle"]}");
                if (lbTiere.Items.Count == 0) lbTiere.Items.Add("(Keine Tiere)");
            }
            catch { }
        }

        // Aktualisiert Anzeige der ID-Karte mit aktuellen Daten
        private void UpdateIDCard()
        {
            lblPflegerName.Text = $"{txtVorname.Text}\n{txtNachname.Text}";
            lblPosition.Text = $"🏆 {cmbPosition.SelectedItem?.ToString() ?? "Position"}";
            lblRolle.Text = $"👔 {cmbRolle.SelectedItem?.ToString() ?? "Rolle"}";
        }

        // Lädt und zeigt das Profilfoto
        private void LoadFoto()
        {
            if (!string.IsNullOrEmpty(currentFotoPath) && File.Exists(currentFotoPath))
            {
                try { pbFoto.Image = Image.FromFile(currentFotoPath); }
                catch { pbFoto.Image = null; }
            }
            else pbFoto.Image = null;
        }

        // Button "Foto hochladen" - Öffnet Dateiauswahl für Profilfoto
        private void BtnFotoHochladen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Bilder|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string ordner = Path.Combine(Application.StartupPath, "bilder", "pfleger");
                        Directory.CreateDirectory(ordner);
                        string ziel = Path.Combine(ordner, $"pfleger_{pflegerID}{Path.GetExtension(ofd.FileName)}");
                        File.Copy(ofd.FileName, ziel, true);
                        currentFotoPath = ziel;
                        LoadFoto();
                        MessageBox.Show("✅ Foto hochgeladen!", "Erfolg");
                    }
                    catch (Exception ex) { MessageBox.Show($"❌ Fehler: {ex.Message}", "Fehler"); }
                }
            }
        }

        // Button "Tier hinzufügen" - Ordnet Pfleger ein Tier zu
        private void BtnTierHinzu_Click(object sender, EventArgs e)
        {
            Form dlg = new Form { Text = "Tier zuordnen", Size = new Size(400, 250), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            ComboBox cmb = new ComboBox { Left = 130, Top = 37, Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };
            DataTable tiere = db.Get("SELECT tierID, Name FROM Tiere ORDER BY Name");
            foreach (DataRow r in tiere.Rows)
                cmb.Items.Add(new ComboItem { Value = Convert.ToInt32(r["tierID"]), Text = r["Name"].ToString() });
            CheckBox chk = new CheckBox { Text = "Als Hauptpfleger", Left = 130, Top = 77, Width = 230, Checked = true };
            Button ok = new Button { Text = "OK", Left = 200, Top = 140, Width = 80, DialogResult = DialogResult.OK };
            Button cancel = new Button { Text = "Abbrechen", Left = 290, Top = 140, Width = 80, DialogResult = DialogResult.Cancel };
            dlg.Controls.AddRange(new Control[] { new Label { Text = "Tier:", Left = 20, Top = 40 }, cmb, chk, ok, cancel });
            if (dlg.ShowDialog() == DialogResult.OK && cmb.SelectedItem != null)
            {
                try
                {
                    var item = (ComboItem)cmb.SelectedItem;
                    db.Execute("INSERT INTO pfleger_tier (pflegerID, tierID, IstHauptpfleger) VALUES (@p, @t, @h) ON DUPLICATE KEY UPDATE IstHauptpfleger = @h",
                        ("@p", pflegerID), ("@t", item.Value), ("@h", chk.Checked));
                    LoadTiere();
                    MessageBox.Show("✅ Tier zugeordnet!");
                }
                catch (Exception ex) { MessageBox.Show($"❌ {ex.Message}"); }
            }
        }

        // Button "Speichern" - Speichert alle Änderungen in Datenbank
        private void BtnSpeichern_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVorname.Text) || string.IsNullOrWhiteSpace(txtNachname.Text))
            {
                MessageBox.Show("⚠️ Vor- und Nachname erforderlich!");
                return;
            }

            try
            {
                int? hauptID = null;
                if (!chkIstHauptpfleger.Checked && cmbHauptpfleger.SelectedItem is ComboItem item)
                    hauptID = item.Value;

                db.Execute(@"UPDATE pfleger SET Vorname = @vn, Nachname = @nn, Geburtsdatum = @geb, Telefon = @tel, Email = @email, Notfallkontakt = @nk,
                    Gehalt = @gehalt, Einstellungsdatum = @einst, Position = @pos, Rolle = @rolle, Steuerklasse = @steuer, 
                    Sozialversicherungsnummer = @sv, Bankverbindung = @bank, Krankenkasse = @kk, Ausweis_Nr = @ausw,
                    Arbeitszeitmodell = @azm, Wochenstunden = @ws, Urlaubstage_pro_Jahr = @urlaub, Resturlaub = @rest,
                    Qualifikationen = @qual, Fortbildungen = @fort, Foto = @foto, IstHauptpfleger = @haupt, HauptpflegerID = @hauptID,
                    Aktiv = @aktiv, Account_aktiv = @accaktiv WHERE pflegerID = @id",
                    ("@vn", txtVorname.Text.Trim()), ("@nn", txtNachname.Text.Trim()), ("@geb", dtpGeburtsdatum.Value),
                    ("@tel", txtTelefon.Text), ("@email", txtEmail.Text), ("@nk", txtNotfallkontakt.Text),
                    ("@gehalt", numGehalt.Value), ("@einst", dtpEinstellung.Value), ("@pos", cmbPosition.SelectedItem?.ToString() ?? "Pfleger"),
                    ("@rolle", cmbRolle.SelectedItem?.ToString() ?? "Pfleger"), ("@steuer", cmbSteuerklasse.SelectedIndex + 1),
                    ("@sv", txtSV.Text), ("@bank", txtIBAN.Text), ("@kk", txtKrankenkasse.Text), ("@ausw", txtAusweis.Text),
                    ("@azm", cmbArbeitszeitmodell.SelectedItem?.ToString() ?? "Vollzeit"), ("@ws", numWochenstunden.Value),
                    ("@urlaub", (int)numUrlaubstage.Value), ("@rest", (int)numResturlaub.Value),
                    ("@qual", txtQualifikationen.Text), ("@fort", txtFortbildungen.Text), ("@foto", currentFotoPath),
                    ("@haupt", chkIstHauptpfleger.Checked), ("@hauptID", hauptID.HasValue ? (object)hauptID.Value : DBNull.Value),
                    ("@aktiv", chkAktiv.Checked), ("@accaktiv", chkAccountAktiv.Checked), ("@id", pflegerID));

                db.Execute("DELETE FROM pfleger_aufgabenbereich WHERE pflegerID = @id", ("@id", pflegerID));
                foreach (var i in clbAufgaben.CheckedItems)
                    if (i is ComboItem a)
                        db.Execute("INSERT INTO pfleger_aufgabenbereich (pflegerID, aufgabenbereichID) VALUES (@p, @a)", ("@p", pflegerID), ("@a", a.Value));

                MessageBox.Show("✅ Gespeichert!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show($"❌ {ex.Message}"); }
        }

        // Berechnet und aktualisiert Altersanzeige
        private void UpdateAlter()
        {
            int alter = DateTime.Now.Year - dtpGeburtsdatum.Value.Year;
            if (DateTime.Now < dtpGeburtsdatum.Value.AddYears(alter)) alter--;
            lblAlter.Text = $"{alter} Jahre";
        }

        // Berechnet und aktualisiert Anstellungsdauer in Jahren
        private void UpdateAnstellungsdauer()
        {
            int jahre = DateTime.Now.Year - dtpEinstellung.Value.Year;
            if (DateTime.Now < dtpEinstellung.Value.AddYears(jahre)) jahre--;
            lblAnstellungsdauer.Text = $"{jahre} Jahre";
        }

        // --- Hilfsmethoden ---
        
        // Erstellt ein Panel mit weißem Hintergrund ("Karte")
        private Panel CreateCard(int x, int y, int w, int h)
        {
            var p = new Panel { Left = x, Top = y, Width = w, Height = h, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            return p;
        }

        // Fügt farbigen Header zu einem Panel hinzu
        private void AddHeader(Panel p, string txt, Color c)
        {
            var hdr = new Panel { Width = p.Width, Height = 55, BackColor = c, Dock = DockStyle.Top };
            hdr.Controls.Add(new Label { Text = txt, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 17), AutoSize = true, BackColor = Color.Transparent });
            p.Controls.Add(hdr);
        }

        // Fügt ein Label hinzu (wird selten verwendet)
        private void AddLabel(string txt, int y, int x = 25)
        {
            Panel targetPanel = (y < 500 && x == 25) ? personalPanel : 
                               (x == 25 || x == 185 || x == 145) ? arbeitPanel : 
                               zuordnungenPanel;
            var lbl = new Label { Text = txt, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(x, y), AutoSize = true };
            targetPanel.Controls.Add(lbl);
        }

        // Fügt Label + TextBox zum Personal-Panel hinzu
        private void AddField(string lbl, ref TextBox txt, int y)
        {
            AddLabel(lbl, y);
            txt = new TextBox { Left = 25, Top = y + 25, Width = 320, Font = new Font("Segoe UI", 10F) };
            personalPanel.Controls.Add(txt);
        }

        // Fügt Label + TextBox in zweiter Spalte zum Arbeit-Panel hinzu
        private void AddFieldSecondCol(string lbl, ref TextBox txt, int y)
        {
            arbeitPanel.Controls.Add(new Label { Text = lbl, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(52, 73, 94), Location = new Point(285, y), AutoSize = true });
            txt = new TextBox { Left = 285, Top = y + 25, Width = 220, Font = new Font("Segoe UI", 10F) };
            arbeitPanel.Controls.Add(txt);
        }

        // Hilfsklasse für ComboBox-Einträge mit ID und Text
        private class ComboItem
        {
            public int Value { get; set; }      // ID (z.B. pflegerID)
            public string Text { get; set; }     // Anzeigetext
            public override string ToString() => Text;
        }
    }
}
