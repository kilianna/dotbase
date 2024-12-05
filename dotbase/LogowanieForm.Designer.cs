namespace DotBase
{
    partial class LogowanieForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button zakonczBtn;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Button wybierzBtn;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogowanieForm));
            this.zalogujBtn = new System.Windows.Forms.Button();
            this.bazaFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.hasloTextBox = new System.Windows.Forms.TextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.bazaTextBox = new System.Windows.Forms.TextBox();
            this.uzytkownikTextBox = new System.Windows.Forms.TextBox();
            this.administracjaBtn = new System.Windows.Forms.Button();
            this.wersjaLabel = new System.Windows.Forms.TextBox();
            this.adminMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.użytkownicyIHasłoBazyDanychToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.podlądLogówDiagnostycznychToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            zakonczBtn = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            wybierzBtn = new System.Windows.Forms.Button();
            this.adminMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // zakonczBtn
            // 
            zakonczBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            zakonczBtn.Location = new System.Drawing.Point(59, 201);
            zakonczBtn.Name = "zakonczBtn";
            zakonczBtn.Size = new System.Drawing.Size(115, 34);
            zakonczBtn.TabIndex = 8;
            zakonczBtn.Text = "Zakończ";
            zakonczBtn.UseVisualStyleBackColor = true;
            zakonczBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 9);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(65, 13);
            label4.TabIndex = 0;
            label4.Text = "&Użytkownik:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(12, 68);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 13);
            label5.TabIndex = 2;
            label5.Text = "&Hasło:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(12, 133);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(90, 13);
            label6.TabIndex = 4;
            label6.Text = "&Plik bazy danych:";
            // 
            // wybierzBtn
            // 
            wybierzBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            wybierzBtn.Location = new System.Drawing.Point(362, 147);
            wybierzBtn.Name = "wybierzBtn";
            wybierzBtn.Size = new System.Drawing.Size(124, 24);
            wybierzBtn.TabIndex = 6;
            wybierzBtn.Text = "&Wybierz...";
            wybierzBtn.UseVisualStyleBackColor = true;
            wybierzBtn.Click += new System.EventHandler(this.wybierzBtn_Click);
            // 
            // zalogujBtn
            // 
            this.zalogujBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zalogujBtn.Location = new System.Drawing.Point(321, 201);
            this.zalogujBtn.Name = "zalogujBtn";
            this.zalogujBtn.Size = new System.Drawing.Size(116, 34);
            this.zalogujBtn.TabIndex = 7;
            this.zalogujBtn.Text = "Zaloguj";
            this.zalogujBtn.UseVisualStyleBackColor = true;
            this.zalogujBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // bazaFileDialog
            // 
            this.bazaFileDialog.Filter = "(*.accdb)|*.accdb";
            this.bazaFileDialog.Tag = "";
            // 
            // hasloTextBox
            // 
            this.hasloTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hasloTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hasloTextBox.Location = new System.Drawing.Point(15, 84);
            this.hasloTextBox.Name = "hasloTextBox";
            this.hasloTextBox.Size = new System.Drawing.Size(470, 32);
            this.hasloTextBox.TabIndex = 3;
            this.hasloTextBox.UseSystemPasswordChar = true;
            this.hasloTextBox.TextChanged += new System.EventHandler(this.hasloTextBox_TextChanged);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.ForeColor = System.Drawing.Color.Red;
            this.infoLabel.Location = new System.Drawing.Point(12, 181);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(16, 13);
            this.infoLabel.TabIndex = 13;
            this.infoLabel.Text = "...";
            // 
            // bazaTextBox
            // 
            this.bazaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bazaTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DotBase.Properties.Settings.Default, "PlikBazy", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bazaTextBox.Location = new System.Drawing.Point(15, 149);
            this.bazaTextBox.Name = "bazaTextBox";
            this.bazaTextBox.Size = new System.Drawing.Size(340, 20);
            this.bazaTextBox.TabIndex = 5;
            this.bazaTextBox.Text = global::DotBase.Properties.Settings.Default.PlikBazy;
            this.bazaTextBox.TextChanged += new System.EventHandler(this.bazaTextBox_TextChanged);
            // 
            // uzytkownikTextBox
            // 
            this.uzytkownikTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uzytkownikTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DotBase.Properties.Settings.Default, "OstatniUzytkownik", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.uzytkownikTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uzytkownikTextBox.Location = new System.Drawing.Point(15, 25);
            this.uzytkownikTextBox.Name = "uzytkownikTextBox";
            this.uzytkownikTextBox.Size = new System.Drawing.Size(470, 32);
            this.uzytkownikTextBox.TabIndex = 1;
            this.uzytkownikTextBox.Text = global::DotBase.Properties.Settings.Default.OstatniUzytkownik;
            this.uzytkownikTextBox.TextChanged += new System.EventHandler(this.uzytkownikTextBox_TextChanged);
            // 
            // administracjaBtn
            // 
            this.administracjaBtn.Location = new System.Drawing.Point(190, 201);
            this.administracjaBtn.Name = "administracjaBtn";
            this.administracjaBtn.Size = new System.Drawing.Size(116, 34);
            this.administracjaBtn.TabIndex = 14;
            this.administracjaBtn.Text = "Administracja";
            this.administracjaBtn.UseVisualStyleBackColor = true;
            this.administracjaBtn.Visible = false;
            this.administracjaBtn.Click += new System.EventHandler(this.administracjaBtn_Click);
            // 
            // wersjaLabel
            // 
            this.wersjaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wersjaLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.wersjaLabel.ForeColor = System.Drawing.Color.Gray;
            this.wersjaLabel.Location = new System.Drawing.Point(12, 257);
            this.wersjaLabel.Name = "wersjaLabel";
            this.wersjaLabel.Size = new System.Drawing.Size(482, 13);
            this.wersjaLabel.TabIndex = 16;
            this.wersjaLabel.Text = "Wersja programu: ";
            this.wersjaLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // adminMenu
            // 
            this.adminMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.adminMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.użytkownicyIHasłoBazyDanychToolStripMenuItem,
            this.podlądLogówDiagnostycznychToolStripMenuItem});
            this.adminMenu.Name = "contextMenuStrip1";
            this.adminMenu.Size = new System.Drawing.Size(248, 48);
            // 
            // użytkownicyIHasłoBazyDanychToolStripMenuItem
            // 
            this.użytkownicyIHasłoBazyDanychToolStripMenuItem.Name = "użytkownicyIHasłoBazyDanychToolStripMenuItem";
            this.użytkownicyIHasłoBazyDanychToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.użytkownicyIHasłoBazyDanychToolStripMenuItem.Text = "Użytkownicy i hasło bazy danych";
            this.użytkownicyIHasłoBazyDanychToolStripMenuItem.Click += new System.EventHandler(this.adminMenuItem_Click);
            // 
            // podlądLogówDiagnostycznychToolStripMenuItem
            // 
            this.podlądLogówDiagnostycznychToolStripMenuItem.Name = "podlądLogówDiagnostycznychToolStripMenuItem";
            this.podlądLogówDiagnostycznychToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.podlądLogówDiagnostycznychToolStripMenuItem.Text = "Podląd logów diagnostycznych";
            this.podlądLogówDiagnostycznychToolStripMenuItem.Click += new System.EventHandler(this.podlądLogówDiagnostycznychToolStripMenuItem_Click);
            // 
            // LogowanieForm
            // 
            this.AcceptButton = this.zalogujBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = zakonczBtn;
            this.ClientSize = new System.Drawing.Size(497, 273);
            this.Controls.Add(this.wersjaLabel);
            this.Controls.Add(this.administracjaBtn);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(wybierzBtn);
            this.Controls.Add(label6);
            this.Controls.Add(this.bazaTextBox);
            this.Controls.Add(label5);
            this.Controls.Add(this.hasloTextBox);
            this.Controls.Add(label4);
            this.Controls.Add(this.uzytkownikTextBox);
            this.Controls.Add(zakonczBtn);
            this.Controls.Add(this.zalogujBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(438, 312);
            this.Name = "LogowanieForm";
            this.Text = "Logowanie do bazy danych";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuGlowneForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LogowanieForm_FormClosed);
            this.Load += new System.EventHandler(this.LogowanieForm_Load);
            this.Shown += new System.EventHandler(this.LogowanieForm_Shown);
            this.adminMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog bazaFileDialog;
        private System.Windows.Forms.TextBox uzytkownikTextBox;
        private System.Windows.Forms.TextBox hasloTextBox;
        private System.Windows.Forms.TextBox bazaTextBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button zalogujBtn;
        private System.Windows.Forms.Button administracjaBtn;
        private System.Windows.Forms.TextBox wersjaLabel;
        private System.Windows.Forms.ContextMenuStrip adminMenu;
        private System.Windows.Forms.ToolStripMenuItem użytkownicyIHasłoBazyDanychToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem podlądLogówDiagnostycznychToolStripMenuItem;
    }
}

