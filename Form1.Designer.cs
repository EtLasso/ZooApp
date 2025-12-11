namespace ZooApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            mySqlDataAdapter1 = new MySql.Data.MySqlClient.MySqlDataAdapter();
            tabControl1 = new TabControl();
            tabPage4 = new TabPage();
            btnDelKontinent = new Button();
            btnSaveKontinent = new Button();
            btnNewKontinent = new Button();
            lbKontinent = new ListBox();
            gbKontinent = new GroupBox();
            txtKBezeichnung = new TextBox();
            lblKBezeichnung = new Label();
            tabPage2 = new TabPage();
            btnDelGehege = new Button();
            btnSaveGehege = new Button();
            btnNewGehege = new Button();
            lbGehege = new ListBox();
            gbGehege = new GroupBox();
            cmbKontinentGehege = new ComboBox();
            txtGBezeichnung = new TextBox();
            lblKontinentGehege = new Label();
            lblGBezeichnung = new Label();
            tabPage3 = new TabPage();
            btnDelTierart = new Button();
            btnSaveTierart = new Button();
            btnNewTierart = new Button();
            lbTierart = new ListBox();
            gbTierart = new GroupBox();
            txtTABezeichnung = new TextBox();
            lblTABezeichnung = new Label();
            tabPage1 = new TabPage();
            btnDelTier = new Button();
            btnSaveTier = new Button();
            btnNewTier = new Button();
            lbTiere = new ListBox();
            gbTiere = new GroupBox();
            cmbGehegeTiere = new ComboBox();
            cmbTierartTiere = new ComboBox();
            dtpGeburtsdatum = new DateTimePicker();
            txtGewicht = new TextBox();
            txtTierName = new TextBox();
            lblGehegeTiere = new Label();
            lblTierartTiere = new Label();
            lblGeburtsdatum = new Label();
            lblGewicht = new Label();
            lblTierName = new Label();
            tabPage5 = new TabPage();
            dgvUebersicht = new DataGridView();
            tabPage6 = new TabPage();
            dgvFutter = new DataGridView();
            btnLadeFutter = new Button();
            btnFutterNeu = new Button();
            btnFutterSpeichern = new Button();
            btnFutterLöschen = new Button();
            gbFutterDetails = new GroupBox();
            numFutterBestellmenge = new NumericUpDown();
            numFutterMindestbestand = new NumericUpDown();
            numFutterLagerbestand = new NumericUpDown();
            numFutterPreis = new NumericUpDown();
            txtFutterEinheit = new TextBox();
            txtFutterBezeichnung = new TextBox();
            lblBestellmenge = new Label();
            lblMindestbestand = new Label();
            lblLagerbestand = new Label();
            lblPreis = new Label();
            lblEinheit = new Label();
            lblFutterBezeichnung = new Label();
            tabPage7 = new TabPage();
            dgvNachbestellung = new DataGridView();
            btnLadeNachbestellung = new Button();
            tabPage8 = new TabPage();
            dgvFutterplan = new DataGridView();
            cmbTierartFutterplan = new ComboBox();
            btnLadeFutterplan = new Button();
            lblTierartFutterplan = new Label();
            tabPage9 = new TabPage();
            dgvTagesbedarf = new DataGridView();
            btnLadeTagesbedarf = new Button();
            tabPage10 = new TabPage();
            dgvBestellungen = new DataGridView();
            btnLadeBestellungen = new Button();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            panelHeader = new Panel();
            lblTitle = new Label();
            tabControl1.SuspendLayout();
            tabPage4.SuspendLayout();
            gbKontinent.SuspendLayout();
            tabPage2.SuspendLayout();
            gbGehege.SuspendLayout();
            tabPage3.SuspendLayout();
            gbTierart.SuspendLayout();
            tabPage1.SuspendLayout();
            gbTiere.SuspendLayout();
            tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUebersicht).BeginInit();
            tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFutter).BeginInit();
            gbFutterDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFutterBestellmenge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFutterMindestbestand).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFutterLagerbestand).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFutterPreis).BeginInit();
            tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNachbestellung).BeginInit();
            tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFutterplan).BeginInit();
            tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTagesbedarf).BeginInit();
            tabPage10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBestellungen).BeginInit();
            statusStrip1.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // mySqlDataAdapter1
            // 
            mySqlDataAdapter1.DeleteCommand = null;
            mySqlDataAdapter1.InsertCommand = null;
            mySqlDataAdapter1.SelectCommand = null;
            mySqlDataAdapter1.UpdateCommand = null;
            // 
            // tabControl1
            // 
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Controls.Add(tabPage8);
            tabControl1.Controls.Add(tabPage9);
            tabControl1.Controls.Add(tabPage10);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Segoe UI", 10F);
            tabControl1.Location = new Point(0, 70);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(10, 5);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1200, 658);
            tabControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = Color.WhiteSmoke;
            tabPage4.Controls.Add(btnDelKontinent);
            tabPage4.Controls.Add(btnSaveKontinent);
            tabPage4.Controls.Add(btnNewKontinent);
            tabPage4.Controls.Add(lbKontinent);
            tabPage4.Controls.Add(gbKontinent);
            tabPage4.Location = new Point(4, 30);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(15);
            tabPage4.Size = new Size(1192, 624);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "① 🌍 Kontinent";
            // 
            // btnDelKontinent
            // 
            btnDelKontinent.BackColor = Color.FromArgb(231, 76, 60);
            btnDelKontinent.Cursor = Cursors.Hand;
            btnDelKontinent.FlatAppearance.BorderSize = 0;
            btnDelKontinent.FlatStyle = FlatStyle.Flat;
            btnDelKontinent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelKontinent.ForeColor = Color.White;
            btnDelKontinent.Location = new Point(280, 150);
            btnDelKontinent.Name = "btnDelKontinent";
            btnDelKontinent.Size = new Size(115, 45);
            btnDelKontinent.TabIndex = 9;
            btnDelKontinent.Text = "🗑️ Löschen";
            btnDelKontinent.UseVisualStyleBackColor = false;
            btnDelKontinent.Click += btnDelKontinent_Click;
            // 
            // btnSaveKontinent
            // 
            btnSaveKontinent.BackColor = Color.FromArgb(46, 204, 113);
            btnSaveKontinent.Cursor = Cursors.Hand;
            btnSaveKontinent.FlatAppearance.BorderSize = 0;
            btnSaveKontinent.FlatStyle = FlatStyle.Flat;
            btnSaveKontinent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveKontinent.ForeColor = Color.White;
            btnSaveKontinent.Location = new Point(140, 150);
            btnSaveKontinent.Name = "btnSaveKontinent";
            btnSaveKontinent.Size = new Size(130, 45);
            btnSaveKontinent.TabIndex = 8;
            btnSaveKontinent.Text = "💾 Speichern";
            btnSaveKontinent.UseVisualStyleBackColor = false;
            btnSaveKontinent.Click += btnSaveKontinent_Click;
            // 
            // btnNewKontinent
            // 
            btnNewKontinent.BackColor = Color.FromArgb(52, 152, 219);
            btnNewKontinent.Cursor = Cursors.Hand;
            btnNewKontinent.FlatAppearance.BorderSize = 0;
            btnNewKontinent.FlatStyle = FlatStyle.Flat;
            btnNewKontinent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewKontinent.ForeColor = Color.White;
            btnNewKontinent.Location = new Point(15, 150);
            btnNewKontinent.Name = "btnNewKontinent";
            btnNewKontinent.Size = new Size(115, 45);
            btnNewKontinent.TabIndex = 7;
            btnNewKontinent.Text = "✨ Neu";
            btnNewKontinent.UseVisualStyleBackColor = false;
            btnNewKontinent.Click += btnNewKontinent_Click;
            // 
            // lbKontinent
            // 
            lbKontinent.BackColor = Color.White;
            lbKontinent.BorderStyle = BorderStyle.FixedSingle;
            lbKontinent.Font = new Font("Segoe UI", 10F);
            lbKontinent.FormattingEnabled = true;
            lbKontinent.Location = new Point(410, 15);
            lbKontinent.Name = "lbKontinent";
            lbKontinent.Size = new Size(760, 580);
            lbKontinent.TabIndex = 6;
            lbKontinent.SelectedIndexChanged += lbKontinent_SelectedIndexChanged;
            // 
            // gbKontinent
            // 
            gbKontinent.BackColor = Color.White;
            gbKontinent.Controls.Add(txtKBezeichnung);
            gbKontinent.Controls.Add(lblKBezeichnung);
            gbKontinent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbKontinent.Location = new Point(15, 15);
            gbKontinent.Name = "gbKontinent";
            gbKontinent.Padding = new Padding(15);
            gbKontinent.Size = new Size(380, 120);
            gbKontinent.TabIndex = 5;
            gbKontinent.TabStop = false;
            gbKontinent.Text = "Kontinent Details";
            // 
            // txtKBezeichnung
            // 
            txtKBezeichnung.Font = new Font("Segoe UI", 10F);
            txtKBezeichnung.Location = new Point(160, 37);
            txtKBezeichnung.Name = "txtKBezeichnung";
            txtKBezeichnung.Size = new Size(200, 25);
            txtKBezeichnung.TabIndex = 1;
            // 
            // lblKBezeichnung
            // 
            lblKBezeichnung.AutoSize = true;
            lblKBezeichnung.Font = new Font("Segoe UI", 10F);
            lblKBezeichnung.Location = new Point(20, 40);
            lblKBezeichnung.Name = "lblKBezeichnung";
            lblKBezeichnung.Size = new Size(89, 19);
            lblKBezeichnung.TabIndex = 0;
            lblKBezeichnung.Text = "Bezeichnung:";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.WhiteSmoke;
            tabPage2.Controls.Add(btnDelGehege);
            tabPage2.Controls.Add(btnSaveGehege);
            tabPage2.Controls.Add(btnNewGehege);
            tabPage2.Controls.Add(lbGehege);
            tabPage2.Controls.Add(gbGehege);
            tabPage2.Location = new Point(4, 30);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(15);
            tabPage2.Size = new Size(1192, 624);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "② 🏠 Gehege";
            // 
            // btnDelGehege
            // 
            btnDelGehege.BackColor = Color.FromArgb(231, 76, 60);
            btnDelGehege.Cursor = Cursors.Hand;
            btnDelGehege.FlatAppearance.BorderSize = 0;
            btnDelGehege.FlatStyle = FlatStyle.Flat;
            btnDelGehege.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelGehege.ForeColor = Color.White;
            btnDelGehege.Location = new Point(280, 210);
            btnDelGehege.Name = "btnDelGehege";
            btnDelGehege.Size = new Size(115, 45);
            btnDelGehege.TabIndex = 9;
            btnDelGehege.Text = "🗑️ Löschen";
            btnDelGehege.UseVisualStyleBackColor = false;
            btnDelGehege.Click += btnDelGehege_Click;
            // 
            // btnSaveGehege
            // 
            btnSaveGehege.BackColor = Color.FromArgb(46, 204, 113);
            btnSaveGehege.Cursor = Cursors.Hand;
            btnSaveGehege.FlatAppearance.BorderSize = 0;
            btnSaveGehege.FlatStyle = FlatStyle.Flat;
            btnSaveGehege.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveGehege.ForeColor = Color.White;
            btnSaveGehege.Location = new Point(140, 210);
            btnSaveGehege.Name = "btnSaveGehege";
            btnSaveGehege.Size = new Size(130, 45);
            btnSaveGehege.TabIndex = 8;
            btnSaveGehege.Text = "💾 Speichern";
            btnSaveGehege.UseVisualStyleBackColor = false;
            btnSaveGehege.Click += btnSaveGehege_Click;
            // 
            // btnNewGehege
            // 
            btnNewGehege.BackColor = Color.FromArgb(52, 152, 219);
            btnNewGehege.Cursor = Cursors.Hand;
            btnNewGehege.FlatAppearance.BorderSize = 0;
            btnNewGehege.FlatStyle = FlatStyle.Flat;
            btnNewGehege.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewGehege.ForeColor = Color.White;
            btnNewGehege.Location = new Point(15, 210);
            btnNewGehege.Name = "btnNewGehege";
            btnNewGehege.Size = new Size(115, 45);
            btnNewGehege.TabIndex = 7;
            btnNewGehege.Text = "✨ Neu";
            btnNewGehege.UseVisualStyleBackColor = false;
            btnNewGehege.Click += btnNewGehege_Click;
            // 
            // lbGehege
            // 
            lbGehege.BackColor = Color.White;
            lbGehege.BorderStyle = BorderStyle.FixedSingle;
            lbGehege.Font = new Font("Segoe UI", 10F);
            lbGehege.FormattingEnabled = true;
            lbGehege.Location = new Point(410, 15);
            lbGehege.Name = "lbGehege";
            lbGehege.Size = new Size(760, 580);
            lbGehege.TabIndex = 6;
            lbGehege.SelectedIndexChanged += lbGehege_SelectedIndexChanged;
            // 
            // gbGehege
            // 
            gbGehege.BackColor = Color.White;
            gbGehege.Controls.Add(cmbKontinentGehege);
            gbGehege.Controls.Add(txtGBezeichnung);
            gbGehege.Controls.Add(lblKontinentGehege);
            gbGehege.Controls.Add(lblGBezeichnung);
            gbGehege.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbGehege.Location = new Point(15, 15);
            gbGehege.Name = "gbGehege";
            gbGehege.Padding = new Padding(15);
            gbGehege.Size = new Size(380, 180);
            gbGehege.TabIndex = 5;
            gbGehege.TabStop = false;
            gbGehege.Text = "Gehege Details";
            // 
            // cmbKontinentGehege
            // 
            cmbKontinentGehege.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKontinentGehege.Font = new Font("Segoe UI", 10F);
            cmbKontinentGehege.FormattingEnabled = true;
            cmbKontinentGehege.Location = new Point(160, 87);
            cmbKontinentGehege.Name = "cmbKontinentGehege";
            cmbKontinentGehege.Size = new Size(200, 25);
            cmbKontinentGehege.TabIndex = 3;
            // 
            // txtGBezeichnung
            // 
            txtGBezeichnung.Font = new Font("Segoe UI", 10F);
            txtGBezeichnung.Location = new Point(160, 37);
            txtGBezeichnung.Name = "txtGBezeichnung";
            txtGBezeichnung.Size = new Size(200, 25);
            txtGBezeichnung.TabIndex = 2;
            // 
            // lblKontinentGehege
            // 
            lblKontinentGehege.AutoSize = true;
            lblKontinentGehege.Font = new Font("Segoe UI", 10F);
            lblKontinentGehege.Location = new Point(20, 90);
            lblKontinentGehege.Name = "lblKontinentGehege";
            lblKontinentGehege.Size = new Size(72, 19);
            lblKontinentGehege.TabIndex = 1;
            lblKontinentGehege.Text = "Kontinent:";
            // 
            // lblGBezeichnung
            // 
            lblGBezeichnung.AutoSize = true;
            lblGBezeichnung.Font = new Font("Segoe UI", 10F);
            lblGBezeichnung.Location = new Point(20, 40);
            lblGBezeichnung.Name = "lblGBezeichnung";
            lblGBezeichnung.Size = new Size(89, 19);
            lblGBezeichnung.TabIndex = 0;
            lblGBezeichnung.Text = "Bezeichnung:";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.WhiteSmoke;
            tabPage3.Controls.Add(btnDelTierart);
            tabPage3.Controls.Add(btnSaveTierart);
            tabPage3.Controls.Add(btnNewTierart);
            tabPage3.Controls.Add(lbTierart);
            tabPage3.Controls.Add(gbTierart);
            tabPage3.Location = new Point(4, 30);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(15);
            tabPage3.Size = new Size(1192, 624);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "③ \U0001f992 Tierart";
            // 
            // btnDelTierart
            // 
            btnDelTierart.BackColor = Color.FromArgb(231, 76, 60);
            btnDelTierart.Cursor = Cursors.Hand;
            btnDelTierart.FlatAppearance.BorderSize = 0;
            btnDelTierart.FlatStyle = FlatStyle.Flat;
            btnDelTierart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelTierart.ForeColor = Color.White;
            btnDelTierart.Location = new Point(280, 150);
            btnDelTierart.Name = "btnDelTierart";
            btnDelTierart.Size = new Size(115, 45);
            btnDelTierart.TabIndex = 9;
            btnDelTierart.Text = "🗑️ Löschen";
            btnDelTierart.UseVisualStyleBackColor = false;
            btnDelTierart.Click += btnDelTierart_Click;
            // 
            // btnSaveTierart
            // 
            btnSaveTierart.BackColor = Color.FromArgb(46, 204, 113);
            btnSaveTierart.Cursor = Cursors.Hand;
            btnSaveTierart.FlatAppearance.BorderSize = 0;
            btnSaveTierart.FlatStyle = FlatStyle.Flat;
            btnSaveTierart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveTierart.ForeColor = Color.White;
            btnSaveTierart.Location = new Point(140, 150);
            btnSaveTierart.Name = "btnSaveTierart";
            btnSaveTierart.Size = new Size(130, 45);
            btnSaveTierart.TabIndex = 8;
            btnSaveTierart.Text = "💾 Speichern";
            btnSaveTierart.UseVisualStyleBackColor = false;
            btnSaveTierart.Click += btnSaveTierart_Click;
            // 
            // btnNewTierart
            // 
            btnNewTierart.BackColor = Color.FromArgb(52, 152, 219);
            btnNewTierart.Cursor = Cursors.Hand;
            btnNewTierart.FlatAppearance.BorderSize = 0;
            btnNewTierart.FlatStyle = FlatStyle.Flat;
            btnNewTierart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewTierart.ForeColor = Color.White;
            btnNewTierart.Location = new Point(15, 150);
            btnNewTierart.Name = "btnNewTierart";
            btnNewTierart.Size = new Size(115, 45);
            btnNewTierart.TabIndex = 7;
            btnNewTierart.Text = "✨ Neu";
            btnNewTierart.UseVisualStyleBackColor = false;
            btnNewTierart.Click += btnNewTierart_Click;
            // 
            // lbTierart
            // 
            lbTierart.BackColor = Color.White;
            lbTierart.BorderStyle = BorderStyle.FixedSingle;
            lbTierart.Font = new Font("Segoe UI", 10F);
            lbTierart.FormattingEnabled = true;
            lbTierart.Location = new Point(410, 15);
            lbTierart.Name = "lbTierart";
            lbTierart.Size = new Size(760, 580);
            lbTierart.TabIndex = 6;
            lbTierart.SelectedIndexChanged += lbTierart_SelectedIndexChanged;
            // 
            // gbTierart
            // 
            gbTierart.BackColor = Color.White;
            gbTierart.Controls.Add(txtTABezeichnung);
            gbTierart.Controls.Add(lblTABezeichnung);
            gbTierart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbTierart.Location = new Point(15, 15);
            gbTierart.Name = "gbTierart";
            gbTierart.Padding = new Padding(15);
            gbTierart.Size = new Size(380, 120);
            gbTierart.TabIndex = 5;
            gbTierart.TabStop = false;
            gbTierart.Text = "Tierart Details";
            // 
            // txtTABezeichnung
            // 
            txtTABezeichnung.Font = new Font("Segoe UI", 10F);
            txtTABezeichnung.Location = new Point(160, 37);
            txtTABezeichnung.Name = "txtTABezeichnung";
            txtTABezeichnung.Size = new Size(200, 25);
            txtTABezeichnung.TabIndex = 1;
            // 
            // lblTABezeichnung
            // 
            lblTABezeichnung.AutoSize = true;
            lblTABezeichnung.Font = new Font("Segoe UI", 10F);
            lblTABezeichnung.Location = new Point(20, 40);
            lblTABezeichnung.Name = "lblTABezeichnung";
            lblTABezeichnung.Size = new Size(51, 19);
            lblTABezeichnung.TabIndex = 0;
            lblTABezeichnung.Text = "Tierart:";
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.WhiteSmoke;
            tabPage1.Controls.Add(btnDelTier);
            tabPage1.Controls.Add(btnSaveTier);
            tabPage1.Controls.Add(btnNewTier);
            tabPage1.Controls.Add(lbTiere);
            tabPage1.Controls.Add(gbTiere);
            tabPage1.Location = new Point(4, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(15);
            tabPage1.Size = new Size(1192, 624);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "④ 🐾 Tiere";
            // 
            // btnDelTier
            // 
            btnDelTier.BackColor = Color.FromArgb(231, 76, 60);
            btnDelTier.Cursor = Cursors.Hand;
            btnDelTier.FlatAppearance.BorderSize = 0;
            btnDelTier.FlatStyle = FlatStyle.Flat;
            btnDelTier.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelTier.ForeColor = Color.White;
            btnDelTier.Location = new Point(280, 350);
            btnDelTier.Name = "btnDelTier";
            btnDelTier.Size = new Size(115, 45);
            btnDelTier.TabIndex = 4;
            btnDelTier.Text = "🗑️ Löschen";
            btnDelTier.UseVisualStyleBackColor = false;
            btnDelTier.Click += btnDelTier_Click;
            // 
            // btnSaveTier
            // 
            btnSaveTier.BackColor = Color.FromArgb(46, 204, 113);
            btnSaveTier.Cursor = Cursors.Hand;
            btnSaveTier.FlatAppearance.BorderSize = 0;
            btnSaveTier.FlatStyle = FlatStyle.Flat;
            btnSaveTier.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveTier.ForeColor = Color.White;
            btnSaveTier.Location = new Point(140, 350);
            btnSaveTier.Name = "btnSaveTier";
            btnSaveTier.Size = new Size(130, 45);
            btnSaveTier.TabIndex = 3;
            btnSaveTier.Text = "💾 Speichern";
            btnSaveTier.UseVisualStyleBackColor = false;
            btnSaveTier.Click += btnSaveTier_Click;
            // 
            // btnNewTier
            // 
            btnNewTier.BackColor = Color.FromArgb(52, 152, 219);
            btnNewTier.Cursor = Cursors.Hand;
            btnNewTier.FlatAppearance.BorderSize = 0;
            btnNewTier.FlatStyle = FlatStyle.Flat;
            btnNewTier.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewTier.ForeColor = Color.White;
            btnNewTier.Location = new Point(15, 350);
            btnNewTier.Name = "btnNewTier";
            btnNewTier.Size = new Size(115, 45);
            btnNewTier.TabIndex = 2;
            btnNewTier.Text = "✨ Neu";
            btnNewTier.UseVisualStyleBackColor = false;
            btnNewTier.Click += btnNewTier_Click;
            // 
            // lbTiere
            // 
            lbTiere.BackColor = Color.White;
            lbTiere.BorderStyle = BorderStyle.FixedSingle;
            lbTiere.Font = new Font("Segoe UI", 10F);
            lbTiere.FormattingEnabled = true;
            lbTiere.Location = new Point(429, 3);
            lbTiere.Name = "lbTiere";
            lbTiere.Size = new Size(760, 580);
            lbTiere.TabIndex = 1;
            lbTiere.SelectedIndexChanged += lbTiere_SelectedIndexChanged;
            // 
            // gbTiere
            // 
            gbTiere.BackColor = Color.White;
            gbTiere.Controls.Add(cmbGehegeTiere);
            gbTiere.Controls.Add(cmbTierartTiere);
            gbTiere.Controls.Add(dtpGeburtsdatum);
            gbTiere.Controls.Add(txtGewicht);
            gbTiere.Controls.Add(txtTierName);
            gbTiere.Controls.Add(lblGehegeTiere);
            gbTiere.Controls.Add(lblTierartTiere);
            gbTiere.Controls.Add(lblGeburtsdatum);
            gbTiere.Controls.Add(lblGewicht);
            gbTiere.Controls.Add(lblTierName);
            gbTiere.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbTiere.Location = new Point(15, 15);
            gbTiere.Name = "gbTiere";
            gbTiere.Padding = new Padding(15);
            gbTiere.Size = new Size(380, 320);
            gbTiere.TabIndex = 0;
            gbTiere.TabStop = false;
            gbTiere.Text = "Tier Details";
            // 
            // cmbGehegeTiere
            // 
            cmbGehegeTiere.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGehegeTiere.Font = new Font("Segoe UI", 10F);
            cmbGehegeTiere.FormattingEnabled = true;
            cmbGehegeTiere.Location = new Point(160, 237);
            cmbGehegeTiere.Name = "cmbGehegeTiere";
            cmbGehegeTiere.Size = new Size(200, 25);
            cmbGehegeTiere.TabIndex = 9;
            // 
            // cmbTierartTiere
            // 
            cmbTierartTiere.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTierartTiere.Font = new Font("Segoe UI", 10F);
            cmbTierartTiere.FormattingEnabled = true;
            cmbTierartTiere.Location = new Point(160, 187);
            cmbTierartTiere.Name = "cmbTierartTiere";
            cmbTierartTiere.Size = new Size(200, 25);
            cmbTierartTiere.TabIndex = 8;
            // 
            // dtpGeburtsdatum
            // 
            dtpGeburtsdatum.Font = new Font("Segoe UI", 10F);
            dtpGeburtsdatum.Format = DateTimePickerFormat.Short;
            dtpGeburtsdatum.Location = new Point(160, 137);
            dtpGeburtsdatum.Name = "dtpGeburtsdatum";
            dtpGeburtsdatum.Size = new Size(200, 25);
            dtpGeburtsdatum.TabIndex = 7;
            // 
            // txtGewicht
            // 
            txtGewicht.Font = new Font("Segoe UI", 10F);
            txtGewicht.Location = new Point(160, 87);
            txtGewicht.Name = "txtGewicht";
            txtGewicht.Size = new Size(200, 25);
            txtGewicht.TabIndex = 6;
            // 
            // txtTierName
            // 
            txtTierName.Font = new Font("Segoe UI", 10F);
            txtTierName.Location = new Point(160, 37);
            txtTierName.Name = "txtTierName";
            txtTierName.Size = new Size(200, 25);
            txtTierName.TabIndex = 5;
            // 
            // lblGehegeTiere
            // 
            lblGehegeTiere.AutoSize = true;
            lblGehegeTiere.Font = new Font("Segoe UI", 10F);
            lblGehegeTiere.Location = new Point(20, 240);
            lblGehegeTiere.Name = "lblGehegeTiere";
            lblGehegeTiere.Size = new Size(59, 19);
            lblGehegeTiere.TabIndex = 4;
            lblGehegeTiere.Text = "Gehege:";
            // 
            // lblTierartTiere
            // 
            lblTierartTiere.AutoSize = true;
            lblTierartTiere.Font = new Font("Segoe UI", 10F);
            lblTierartTiere.Location = new Point(20, 190);
            lblTierartTiere.Name = "lblTierartTiere";
            lblTierartTiere.Size = new Size(51, 19);
            lblTierartTiere.TabIndex = 3;
            lblTierartTiere.Text = "Tierart:";
            // 
            // lblGeburtsdatum
            // 
            lblGeburtsdatum.AutoSize = true;
            lblGeburtsdatum.Font = new Font("Segoe UI", 10F);
            lblGeburtsdatum.Location = new Point(20, 140);
            lblGeburtsdatum.Name = "lblGeburtsdatum";
            lblGeburtsdatum.Size = new Size(101, 19);
            lblGeburtsdatum.TabIndex = 2;
            lblGeburtsdatum.Text = "Geburtsdatum:";
            // 
            // lblGewicht
            // 
            lblGewicht.AutoSize = true;
            lblGewicht.Font = new Font("Segoe UI", 10F);
            lblGewicht.Location = new Point(20, 90);
            lblGewicht.Name = "lblGewicht";
            lblGewicht.Size = new Size(88, 19);
            lblGewicht.TabIndex = 1;
            lblGewicht.Text = "Gewicht (kg):";
            // 
            // lblTierName
            // 
            lblTierName.AutoSize = true;
            lblTierName.Font = new Font("Segoe UI", 10F);
            lblTierName.Location = new Point(20, 40);
            lblTierName.Name = "lblTierName";
            lblTierName.Size = new Size(48, 19);
            lblTierName.TabIndex = 0;
            lblTierName.Text = "Name:";
            // 
            // tabPage5
            // 
            tabPage5.BackColor = Color.WhiteSmoke;
            tabPage5.Controls.Add(dgvUebersicht);
            tabPage5.Location = new Point(4, 30);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(15);
            tabPage5.Size = new Size(1192, 624);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "⑤ 📊 Übersicht";
            // 
            // dgvUebersicht
            // 
            dgvUebersicht.AllowUserToAddRows = false;
            dgvUebersicht.AllowUserToDeleteRows = false;
            dgvUebersicht.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUebersicht.BackgroundColor = Color.White;
            dgvUebersicht.BorderStyle = BorderStyle.None;
            dgvUebersicht.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUebersicht.Dock = DockStyle.Fill;
            dgvUebersicht.Location = new Point(15, 15);
            dgvUebersicht.Name = "dgvUebersicht";
            dgvUebersicht.ReadOnly = false;
            dgvUebersicht.RowHeadersWidth = 51;
            dgvUebersicht.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUebersicht.Size = new Size(1162, 594);
            dgvUebersicht.TabIndex = 0;
            dgvUebersicht.CellValueChanged += dgvUebersicht_CellValueChanged;
            // 
            // tabPage6
            // 
            tabPage6.BackColor = Color.WhiteSmoke;
            tabPage6.Controls.Add(dgvFutter);
            tabPage6.Controls.Add(btnLadeFutter);
            tabPage6.Controls.Add(btnFutterNeu);
            tabPage6.Controls.Add(btnFutterSpeichern);
            tabPage6.Controls.Add(btnFutterLöschen);
            tabPage6.Controls.Add(gbFutterDetails);
            tabPage6.Location = new Point(4, 30);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(15);
            tabPage6.Size = new Size(1192, 624);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "⑥ 🍎 Futterverwaltung";
            // 
            // dgvFutter
            // 
            dgvFutter.AllowUserToAddRows = false;
            dgvFutter.AllowUserToDeleteRows = false;
            dgvFutter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFutter.BackgroundColor = Color.White;
            dgvFutter.BorderStyle = BorderStyle.None;
            dgvFutter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFutter.Location = new Point(410, 15);
            dgvFutter.Name = "dgvFutter";
            dgvFutter.ReadOnly = true;
            dgvFutter.RowHeadersWidth = 51;
            dgvFutter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFutter.Size = new Size(760, 580);
            dgvFutter.TabIndex = 11;
            dgvFutter.SelectionChanged += dgvFutter_SelectionChanged;
            // 
            // btnLadeFutter
            // 
            btnLadeFutter.BackColor = Color.FromArgb(155, 89, 182);
            btnLadeFutter.Cursor = Cursors.Hand;
            btnLadeFutter.FlatAppearance.BorderSize = 0;
            btnLadeFutter.FlatStyle = FlatStyle.Flat;
            btnLadeFutter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLadeFutter.ForeColor = Color.White;
            btnLadeFutter.Location = new Point(280, 350);
            btnLadeFutter.Name = "btnLadeFutter";
            btnLadeFutter.Size = new Size(115, 45);
            btnLadeFutter.TabIndex = 10;
            btnLadeFutter.Text = "🔄 Laden";
            btnLadeFutter.UseVisualStyleBackColor = false;
            btnLadeFutter.Click += btnLadeFutter_Click;
            // 
            // btnFutterNeu
            // 
            btnFutterNeu.BackColor = Color.FromArgb(52, 152, 219);
            btnFutterNeu.Cursor = Cursors.Hand;
            btnFutterNeu.FlatAppearance.BorderSize = 0;
            btnFutterNeu.FlatStyle = FlatStyle.Flat;
            btnFutterNeu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFutterNeu.ForeColor = Color.White;
            btnFutterNeu.Location = new Point(15, 350);
            btnFutterNeu.Name = "btnFutterNeu";
            btnFutterNeu.Size = new Size(115, 45);
            btnFutterNeu.TabIndex = 9;
            btnFutterNeu.Text = "✨ Neu";
            btnFutterNeu.UseVisualStyleBackColor = false;
            btnFutterNeu.Click += btnFutterNeu_Click;
            // 
            // btnFutterSpeichern
            // 
            btnFutterSpeichern.BackColor = Color.FromArgb(46, 204, 113);
            btnFutterSpeichern.Cursor = Cursors.Hand;
            btnFutterSpeichern.FlatAppearance.BorderSize = 0;
            btnFutterSpeichern.FlatStyle = FlatStyle.Flat;
            btnFutterSpeichern.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFutterSpeichern.ForeColor = Color.White;
            btnFutterSpeichern.Location = new Point(140, 350);
            btnFutterSpeichern.Name = "btnFutterSpeichern";
            btnFutterSpeichern.Size = new Size(130, 45);
            btnFutterSpeichern.TabIndex = 8;
            btnFutterSpeichern.Text = "💾 Speichern";
            btnFutterSpeichern.UseVisualStyleBackColor = false;
            btnFutterSpeichern.Click += btnFutterSpeichern_Click;
            // 
            // btnFutterLöschen
            // 
            btnFutterLöschen.BackColor = Color.FromArgb(231, 76, 60);
            btnFutterLöschen.Cursor = Cursors.Hand;
            btnFutterLöschen.FlatAppearance.BorderSize = 0;
            btnFutterLöschen.FlatStyle = FlatStyle.Flat;
            btnFutterLöschen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFutterLöschen.ForeColor = Color.White;
            btnFutterLöschen.Location = new Point(280, 400);
            btnFutterLöschen.Name = "btnFutterLöschen";
            btnFutterLöschen.Size = new Size(115, 45);
            btnFutterLöschen.TabIndex = 7;
            btnFutterLöschen.Text = "🗑️ Löschen";
            btnFutterLöschen.UseVisualStyleBackColor = false;
            btnFutterLöschen.Click += btnFutterLöschen_Click;
            // 
            // gbFutterDetails
            // 
            gbFutterDetails.BackColor = Color.White;
            gbFutterDetails.Controls.Add(numFutterBestellmenge);
            gbFutterDetails.Controls.Add(numFutterMindestbestand);
            gbFutterDetails.Controls.Add(numFutterLagerbestand);
            gbFutterDetails.Controls.Add(numFutterPreis);
            gbFutterDetails.Controls.Add(txtFutterEinheit);
            gbFutterDetails.Controls.Add(txtFutterBezeichnung);
            gbFutterDetails.Controls.Add(lblBestellmenge);
            gbFutterDetails.Controls.Add(lblMindestbestand);
            gbFutterDetails.Controls.Add(lblLagerbestand);
            gbFutterDetails.Controls.Add(lblPreis);
            gbFutterDetails.Controls.Add(lblEinheit);
            gbFutterDetails.Controls.Add(lblFutterBezeichnung);
            gbFutterDetails.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbFutterDetails.Location = new Point(15, 15);
            gbFutterDetails.Name = "gbFutterDetails";
            gbFutterDetails.Padding = new Padding(15);
            gbFutterDetails.Size = new Size(380, 320);
            gbFutterDetails.TabIndex = 0;
            gbFutterDetails.TabStop = false;
            gbFutterDetails.Text = "Futter Details";
            // 
            // numFutterBestellmenge
            // 
            numFutterBestellmenge.Font = new Font("Segoe UI", 10F);
            numFutterBestellmenge.Location = new Point(160, 267);
            numFutterBestellmenge.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numFutterBestellmenge.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFutterBestellmenge.Name = "numFutterBestellmenge";
            numFutterBestellmenge.Size = new Size(200, 25);
            numFutterBestellmenge.TabIndex = 11;
            numFutterBestellmenge.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // numFutterMindestbestand
            // 
            numFutterMindestbestand.Font = new Font("Segoe UI", 10F);
            numFutterMindestbestand.Location = new Point(160, 227);
            numFutterMindestbestand.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numFutterMindestbestand.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFutterMindestbestand.Name = "numFutterMindestbestand";
            numFutterMindestbestand.Size = new Size(200, 25);
            numFutterMindestbestand.TabIndex = 10;
            numFutterMindestbestand.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // numFutterLagerbestand
            // 
            numFutterLagerbestand.Font = new Font("Segoe UI", 10F);
            numFutterLagerbestand.Location = new Point(160, 187);
            numFutterLagerbestand.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numFutterLagerbestand.Name = "numFutterLagerbestand";
            numFutterLagerbestand.Size = new Size(200, 25);
            numFutterLagerbestand.TabIndex = 9;
            // 
            // numFutterPreis
            // 
            numFutterPreis.DecimalPlaces = 2;
            numFutterPreis.Font = new Font("Segoe UI", 10F);
            numFutterPreis.Location = new Point(160, 147);
            numFutterPreis.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numFutterPreis.Name = "numFutterPreis";
            numFutterPreis.Size = new Size(200, 25);
            numFutterPreis.TabIndex = 8;
            numFutterPreis.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtFutterEinheit
            // 
            txtFutterEinheit.Font = new Font("Segoe UI", 10F);
            txtFutterEinheit.Location = new Point(160, 107);
            txtFutterEinheit.Name = "txtFutterEinheit";
            txtFutterEinheit.Size = new Size(200, 25);
            txtFutterEinheit.TabIndex = 7;
            txtFutterEinheit.Text = "kg";
            // 
            // txtFutterBezeichnung
            // 
            txtFutterBezeichnung.Font = new Font("Segoe UI", 10F);
            txtFutterBezeichnung.Location = new Point(160, 67);
            txtFutterBezeichnung.Name = "txtFutterBezeichnung";
            txtFutterBezeichnung.Size = new Size(200, 25);
            txtFutterBezeichnung.TabIndex = 6;
            // 
            // lblBestellmenge
            // 
            lblBestellmenge.AutoSize = true;
            lblBestellmenge.Font = new Font("Segoe UI", 10F);
            lblBestellmenge.Location = new Point(20, 270);
            lblBestellmenge.Name = "lblBestellmenge";
            lblBestellmenge.Size = new Size(95, 19);
            lblBestellmenge.TabIndex = 5;
            lblBestellmenge.Text = "Bestellmenge:";
            // 
            // lblMindestbestand
            // 
            lblMindestbestand.AutoSize = true;
            lblMindestbestand.Font = new Font("Segoe UI", 10F);
            lblMindestbestand.Location = new Point(20, 230);
            lblMindestbestand.Name = "lblMindestbestand";
            lblMindestbestand.Size = new Size(121, 19);
            lblMindestbestand.TabIndex = 4;
            lblMindestbestand.Text = "Mindestbestand:";
            // 
            // lblLagerbestand
            // 
            lblLagerbestand.AutoSize = true;
            lblLagerbestand.Font = new Font("Segoe UI", 10F);
            lblLagerbestand.Location = new Point(20, 190);
            lblLagerbestand.Name = "lblLagerbestand";
            lblLagerbestand.Size = new Size(98, 19);
            lblLagerbestand.TabIndex = 3;
            lblLagerbestand.Text = "Lagerbestand:";
            // 
            // lblPreis
            // 
            lblPreis.AutoSize = true;
            lblPreis.Font = new Font("Segoe UI", 10F);
            lblPreis.Location = new Point(20, 150);
            lblPreis.Name = "lblPreis";
            lblPreis.Size = new Size(91, 19);
            lblPreis.TabIndex = 2;
            lblPreis.Text = "Preis (pro €):";
            // 
            // lblEinheit
            // 
            lblEinheit.AutoSize = true;
            lblEinheit.Font = new Font("Segoe UI", 10F);
            lblEinheit.Location = new Point(20, 110);
            lblEinheit.Name = "lblEinheit";
            lblEinheit.Size = new Size(55, 19);
            lblEinheit.TabIndex = 1;
            lblEinheit.Text = "Einheit:";
            // 
            // lblFutterBezeichnung
            // 
            lblFutterBezeichnung.AutoSize = true;
            lblFutterBezeichnung.Font = new Font("Segoe UI", 10F);
            lblFutterBezeichnung.Location = new Point(20, 70);
            lblFutterBezeichnung.Name = "lblFutterBezeichnung";
            lblFutterBezeichnung.Size = new Size(89, 19);
            lblFutterBezeichnung.TabIndex = 0;
            lblFutterBezeichnung.Text = "Bezeichnung:";
            // 
            // tabPage7
            // 
            tabPage7.BackColor = Color.WhiteSmoke;
            tabPage7.Controls.Add(dgvNachbestellung);
            tabPage7.Controls.Add(btnLadeNachbestellung);
            tabPage7.Location = new Point(4, 30);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(15);
            tabPage7.Size = new Size(1192, 624);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "⑦ ⚠️ Nachbestellung";
            // 
            // dgvNachbestellung
            // 
            dgvNachbestellung.AllowUserToAddRows = false;
            dgvNachbestellung.AllowUserToDeleteRows = false;
            dgvNachbestellung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNachbestellung.BackgroundColor = Color.White;
            dgvNachbestellung.BorderStyle = BorderStyle.None;
            dgvNachbestellung.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNachbestellung.Location = new Point(15, 70);
            dgvNachbestellung.Name = "dgvNachbestellung";
            dgvNachbestellung.ReadOnly = true;
            dgvNachbestellung.RowHeadersWidth = 51;
            dgvNachbestellung.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNachbestellung.Size = new Size(1162, 539);
            dgvNachbestellung.TabIndex = 3;
            // 
            // btnLadeNachbestellung
            // 
            btnLadeNachbestellung.BackColor = Color.FromArgb(155, 89, 182);
            btnLadeNachbestellung.Cursor = Cursors.Hand;
            btnLadeNachbestellung.FlatAppearance.BorderSize = 0;
            btnLadeNachbestellung.FlatStyle = FlatStyle.Flat;
            btnLadeNachbestellung.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLadeNachbestellung.ForeColor = Color.White;
            btnLadeNachbestellung.Location = new Point(15, 15);
            btnLadeNachbestellung.Name = "btnLadeNachbestellung";
            btnLadeNachbestellung.Size = new Size(200, 45);
            btnLadeNachbestellung.TabIndex = 2;
            btnLadeNachbestellung.Text = "🔄 Liste laden";
            btnLadeNachbestellung.UseVisualStyleBackColor = false;
            btnLadeNachbestellung.Click += btnLadeNachbestellung_Click;
            // 
            // tabPage8
            // 
            tabPage8.BackColor = Color.WhiteSmoke;
            tabPage8.Controls.Add(dgvFutterplan);
            tabPage8.Controls.Add(cmbTierartFutterplan);
            tabPage8.Controls.Add(btnLadeFutterplan);
            tabPage8.Controls.Add(lblTierartFutterplan);
            tabPage8.Location = new Point(4, 30);
            tabPage8.Name = "tabPage8";
            tabPage8.Padding = new Padding(15);
            tabPage8.Size = new Size(1192, 624);
            tabPage8.TabIndex = 7;
            tabPage8.Text = "⑧ 🍽️ Fütterungsplan";
            // 
            // dgvFutterplan
            // 
            dgvFutterplan.AllowUserToAddRows = false;
            dgvFutterplan.AllowUserToDeleteRows = false;
            dgvFutterplan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFutterplan.BackgroundColor = Color.White;
            dgvFutterplan.BorderStyle = BorderStyle.None;
            dgvFutterplan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFutterplan.Location = new Point(15, 100);
            dgvFutterplan.Name = "dgvFutterplan";
            dgvFutterplan.ReadOnly = true;
            dgvFutterplan.RowHeadersWidth = 51;
            dgvFutterplan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFutterplan.Size = new Size(1162, 509);
            dgvFutterplan.TabIndex = 7;
            // 
            // cmbTierartFutterplan
            // 
            cmbTierartFutterplan.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTierartFutterplan.Font = new Font("Segoe UI", 10F);
            cmbTierartFutterplan.FormattingEnabled = true;
            cmbTierartFutterplan.Location = new Point(120, 55);
            cmbTierartFutterplan.Name = "cmbTierartFutterplan";
            cmbTierartFutterplan.Size = new Size(300, 25);
            cmbTierartFutterplan.TabIndex = 6;
            // 
            // btnLadeFutterplan
            // 
            btnLadeFutterplan.BackColor = Color.FromArgb(155, 89, 182);
            btnLadeFutterplan.Cursor = Cursors.Hand;
            btnLadeFutterplan.FlatAppearance.BorderSize = 0;
            btnLadeFutterplan.FlatStyle = FlatStyle.Flat;
            btnLadeFutterplan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLadeFutterplan.ForeColor = Color.White;
            btnLadeFutterplan.Location = new Point(15, 15);
            btnLadeFutterplan.Name = "btnLadeFutterplan";
            btnLadeFutterplan.Size = new Size(200, 30);
            btnLadeFutterplan.TabIndex = 5;
            btnLadeFutterplan.Text = "🔄 Plan laden";
            btnLadeFutterplan.UseVisualStyleBackColor = false;
            btnLadeFutterplan.Click += btnLadeFutterplan_Click;
            // 
            // lblTierartFutterplan
            // 
            lblTierartFutterplan.AutoSize = true;
            lblTierartFutterplan.Font = new Font("Segoe UI", 10F);
            lblTierartFutterplan.Location = new Point(15, 58);
            lblTierartFutterplan.Name = "lblTierartFutterplan";
            lblTierartFutterplan.Size = new Size(99, 19);
            lblTierartFutterplan.TabIndex = 4;
            lblTierartFutterplan.Text = "Tierart wählen:";
            // 
            // tabPage9
            // 
            tabPage9.BackColor = Color.WhiteSmoke;
            tabPage9.Controls.Add(dgvTagesbedarf);
            tabPage9.Controls.Add(btnLadeTagesbedarf);
            tabPage9.Location = new Point(4, 30);
            tabPage9.Name = "tabPage9";
            tabPage9.Padding = new Padding(15);
            tabPage9.Size = new Size(1192, 624);
            tabPage9.TabIndex = 8;
            tabPage9.Text = "⑨ 📈 Tagesbedarf";
            // 
            // dgvTagesbedarf
            // 
            dgvTagesbedarf.AllowUserToAddRows = false;
            dgvTagesbedarf.AllowUserToDeleteRows = false;
            dgvTagesbedarf.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTagesbedarf.BackgroundColor = Color.White;
            dgvTagesbedarf.BorderStyle = BorderStyle.None;
            dgvTagesbedarf.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTagesbedarf.Location = new Point(15, 70);
            dgvTagesbedarf.Name = "dgvTagesbedarf";
            dgvTagesbedarf.ReadOnly = true;
            dgvTagesbedarf.RowHeadersWidth = 51;
            dgvTagesbedarf.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTagesbedarf.Size = new Size(1162, 539);
            dgvTagesbedarf.TabIndex = 5;
            // 
            // btnLadeTagesbedarf
            // 
            btnLadeTagesbedarf.BackColor = Color.FromArgb(155, 89, 182);
            btnLadeTagesbedarf.Cursor = Cursors.Hand;
            btnLadeTagesbedarf.FlatAppearance.BorderSize = 0;
            btnLadeTagesbedarf.FlatStyle = FlatStyle.Flat;
            btnLadeTagesbedarf.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLadeTagesbedarf.ForeColor = Color.White;
            btnLadeTagesbedarf.Location = new Point(15, 15);
            btnLadeTagesbedarf.Name = "btnLadeTagesbedarf";
            btnLadeTagesbedarf.Size = new Size(200, 45);
            btnLadeTagesbedarf.TabIndex = 4;
            btnLadeTagesbedarf.Text = "🔄 Bedarf laden";
            btnLadeTagesbedarf.UseVisualStyleBackColor = false;
            btnLadeTagesbedarf.Click += btnLadeTagesbedarf_Click;
            // 
            // tabPage10
            // 
            tabPage10.BackColor = Color.WhiteSmoke;
            tabPage10.Controls.Add(dgvBestellungen);
            tabPage10.Controls.Add(btnLadeBestellungen);
            tabPage10.Location = new Point(4, 30);
            tabPage10.Name = "tabPage10";
            tabPage10.Padding = new Padding(15);
            tabPage10.Size = new Size(1192, 624);
            tabPage10.TabIndex = 9;
            tabPage10.Text = "⑩ 📦 Bestellungen";
            // 
            // dgvBestellungen
            // 
            dgvBestellungen.AllowUserToAddRows = false;
            dgvBestellungen.AllowUserToDeleteRows = false;
            dgvBestellungen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBestellungen.BackgroundColor = Color.White;
            dgvBestellungen.BorderStyle = BorderStyle.None;
            dgvBestellungen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBestellungen.Location = new Point(15, 70);
            dgvBestellungen.Name = "dgvBestellungen";
            dgvBestellungen.ReadOnly = true;
            dgvBestellungen.RowHeadersWidth = 51;
            dgvBestellungen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBestellungen.Size = new Size(1162, 539);
            dgvBestellungen.TabIndex = 7;
            // 
            // btnLadeBestellungen
            // 
            btnLadeBestellungen.BackColor = Color.FromArgb(155, 89, 182);
            btnLadeBestellungen.Cursor = Cursors.Hand;
            btnLadeBestellungen.FlatAppearance.BorderSize = 0;
            btnLadeBestellungen.FlatStyle = FlatStyle.Flat;
            btnLadeBestellungen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLadeBestellungen.ForeColor = Color.White;
            btnLadeBestellungen.Location = new Point(15, 15);
            btnLadeBestellungen.Name = "btnLadeBestellungen";
            btnLadeBestellungen.Size = new Size(200, 45);
            btnLadeBestellungen.TabIndex = 6;
            btnLadeBestellungen.Text = "🔄 Bestellungen laden";
            btnLadeBestellungen.UseVisualStyleBackColor = false;
            btnLadeBestellungen.Click += btnLadeBestellungen_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 728);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1200, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(37, 17);
            lblStatus.Text = "Bereit";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(41, 128, 185);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1200, 70);
            panelHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.FromArgb(42, 128, 185);
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(314, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🦁 Zoo Verwaltung";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1200, 750);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(panelHeader);
            MinimumSize = new Size(1000, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Zoo Verwaltungssoftware";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            gbKontinent.ResumeLayout(false);
            gbKontinent.PerformLayout();
            tabPage2.ResumeLayout(false);
            gbGehege.ResumeLayout(false);
            gbGehege.PerformLayout();
            tabPage3.ResumeLayout(false);
            gbTierart.ResumeLayout(false);
            gbTierart.PerformLayout();
            tabPage1.ResumeLayout(false);
            gbTiere.ResumeLayout(false);
            gbTiere.PerformLayout();
            tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUebersicht).EndInit();
            tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFutter).EndInit();
            gbFutterDetails.ResumeLayout(false);
            gbFutterDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFutterBestellmenge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFutterMindestbestand).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFutterLagerbestand).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFutterPreis).EndInit();
            tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNachbestellung).EndInit();
            tabPage8.ResumeLayout(false);
            tabPage8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFutterplan).EndInit();
            tabPage9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTagesbedarf).EndInit();
            tabPage10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBestellungen).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MySql.Data.MySqlClient.MySqlDataAdapter mySqlDataAdapter1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private GroupBox gbTiere;
        private Button btnDelTier;
        private Button btnSaveTier;
        private Button btnNewTier;
        private ListBox lbTiere;
        private DataGridView dgvUebersicht;
        private Button btnDelGehege;
        private Button btnSaveGehege;
        private Button btnNewGehege;
        private ListBox lbGehege;
        private GroupBox gbGehege;
        private Button btnDelTierart;
        private Button btnSaveTierart;
        private Button btnNewTierart;
        private ListBox lbTierart;
        private GroupBox gbTierart;
        private Button btnDelKontinent;
        private Button btnSaveKontinent;
        private Button btnNewKontinent;
        private ListBox lbKontinent;
        private GroupBox gbKontinent;
        private ComboBox cmbGehegeTiere;
        private ComboBox cmbTierartTiere;
        private DateTimePicker dtpGeburtsdatum;
        private TextBox txtGewicht;
        private TextBox txtTierName;
        private Label lblGehegeTiere;
        private Label lblTierartTiere;
        private Label lblGeburtsdatum;
        private Label lblGewicht;
        private Label lblTierName;
        private ComboBox cmbKontinentGehege;
        private TextBox txtGBezeichnung;
        private Label lblKontinentGehege;
        private Label lblGBezeichnung;
        private TextBox txtTABezeichnung;
        private Label lblTABezeichnung;
        private TextBox txtKBezeichnung;
        private Label lblKBezeichnung;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private TabPage tabPage8;
        private TabPage tabPage9;
        private TabPage tabPage10;
        private GroupBox gbFutterDetails;
        private NumericUpDown numFutterBestellmenge;
        private NumericUpDown numFutterMindestbestand;
        private NumericUpDown numFutterLagerbestand;
        private NumericUpDown numFutterPreis;
        private TextBox txtFutterEinheit;
        private TextBox txtFutterBezeichnung;
        private Label lblBestellmenge;
        private Label lblMindestbestand;
        private Label lblLagerbestand;
        private Label lblPreis;
        private Label lblEinheit;
        private Label lblFutterBezeichnung;
        private Button btnFutterLöschen;
        private Button btnLadeFutter;
        private Button btnFutterNeu;
        private Button btnFutterSpeichern;
        private DataGridView dgvFutter;
        private DataGridView dgvNachbestellung;
        private Button btnLadeNachbestellung;
        private DataGridView dgvFutterplan;
        private ComboBox cmbTierartFutterplan;
        private Button btnLadeFutterplan;
        private Label lblTierartFutterplan;
        private DataGridView dgvTagesbedarf;
        private Button btnLadeTagesbedarf;
        private DataGridView dgvBestellungen;
        private Button btnLadeBestellungen;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private Panel panelHeader;
        private Label lblTitle;
    }
}