namespace DotBase.Controls
{
    partial class SimpleHtmlTextBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rt = new System.Windows.Forms.RichTextBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pogrubienieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kurysywaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.podkreślenieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.indeksgórnyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indeksdolnyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.znakispecyjalneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.µMikroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twardaSpacjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plusMiniusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.wytnijToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kopiujToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wklejToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zaznaczWszystkoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cofnijToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powtórzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.innyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // rt
            // 
            this.rt.AcceptsTab = true;
            this.rt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rt.ContextMenuStrip = this.contextMenu;
            this.rt.DetectUrls = false;
            this.rt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rt.Location = new System.Drawing.Point(0, 0);
            this.rt.Name = "rt";
            this.rt.ShortcutsEnabled = false;
            this.rt.Size = new System.Drawing.Size(505, 346);
            this.rt.TabIndex = 0;
            this.rt.Text = "";
            this.rt.SelectionChanged += new System.EventHandler(this.rt_SelectionChanged);
            this.rt.TextChanged += new System.EventHandler(this.rt_TextChanged);
            this.rt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rt_KeyDown);
            this.rt.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.rt_PreviewKeyDown);
            this.rt.Validating += new System.ComponentModel.CancelEventHandler(this.rt_Validating);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatToolStripMenuItem,
            this.znakispecyjalneToolStripMenuItem,
            this.toolStripMenuItem1,
            this.wytnijToolStripMenuItem,
            this.kopiujToolStripMenuItem,
            this.wklejToolStripMenuItem,
            this.zaznaczWszystkoToolStripMenuItem,
            this.toolStripMenuItem2,
            this.cofnijToolStripMenuItem,
            this.powtórzToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(247, 230);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pogrubienieToolStripMenuItem,
            this.kurysywaToolStripMenuItem,
            this.podkreślenieToolStripMenuItem,
            this.toolStripMenuItem3,
            this.indeksgórnyToolStripMenuItem,
            this.indeksdolnyToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.formatToolStripMenuItem.Text = "&Format";
            // 
            // pogrubienieToolStripMenuItem
            // 
            this.pogrubienieToolStripMenuItem.Name = "pogrubienieToolStripMenuItem";
            this.pogrubienieToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+B";
            this.pogrubienieToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
            this.pogrubienieToolStripMenuItem.Text = "&Pogrubienie";
            this.pogrubienieToolStripMenuItem.Click += new System.EventHandler(this.pogrubienieToolStripMenuItem_Click);
            // 
            // kurysywaToolStripMenuItem
            // 
            this.kurysywaToolStripMenuItem.Name = "kurysywaToolStripMenuItem";
            this.kurysywaToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+I";
            this.kurysywaToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
            this.kurysywaToolStripMenuItem.Text = "&Kurysywa";
            this.kurysywaToolStripMenuItem.Click += new System.EventHandler(this.kurysywaToolStripMenuItem_Click);
            // 
            // podkreślenieToolStripMenuItem
            // 
            this.podkreślenieToolStripMenuItem.Name = "podkreślenieToolStripMenuItem";
            this.podkreślenieToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
            this.podkreślenieToolStripMenuItem.Text = "P&odkreślenie";
            this.podkreślenieToolStripMenuItem.Click += new System.EventHandler(this.podkreślenieToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
            // 
            // indeksgórnyToolStripMenuItem
            // 
            this.indeksgórnyToolStripMenuItem.Name = "indeksgórnyToolStripMenuItem";
            this.indeksgórnyToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
            this.indeksgórnyToolStripMenuItem.Text = "Indeks &górny";
            this.indeksgórnyToolStripMenuItem.Click += new System.EventHandler(this.indeksgórnyToolStripMenuItem_Click);
            // 
            // indeksdolnyToolStripMenuItem
            // 
            this.indeksdolnyToolStripMenuItem.Name = "indeksdolnyToolStripMenuItem";
            this.indeksdolnyToolStripMenuItem.Size = new System.Drawing.Size(208, 24);
            this.indeksdolnyToolStripMenuItem.Text = "Indeks &dolny";
            this.indeksdolnyToolStripMenuItem.Click += new System.EventHandler(this.indeksdolnyToolStripMenuItem_Click);
            // 
            // znakispecyjalneToolStripMenuItem
            // 
            this.znakispecyjalneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.µMikroToolStripMenuItem,
            this.twardaSpacjaToolStripMenuItem,
            this.plusMiniusToolStripMenuItem,
            this.toolStripMenuItem4,
            this.innyToolStripMenuItem});
            this.znakispecyjalneToolStripMenuItem.Name = "znakispecyjalneToolStripMenuItem";
            this.znakispecyjalneToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.znakispecyjalneToolStripMenuItem.Text = "Znaki &specyjalne";
            // 
            // µMikroToolStripMenuItem
            // 
            this.µMikroToolStripMenuItem.Name = "µMikroToolStripMenuItem";
            this.µMikroToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+M";
            this.µMikroToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.µMikroToolStripMenuItem.Tag = "µ";
            this.µMikroToolStripMenuItem.Text = "µ &Mikro";
            this.µMikroToolStripMenuItem.Click += new System.EventHandler(this.µMikroToolStripMenuItem_Click);
            // 
            // twardaSpacjaToolStripMenuItem
            // 
            this.twardaSpacjaToolStripMenuItem.Name = "twardaSpacjaToolStripMenuItem";
            this.twardaSpacjaToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Space";
            this.twardaSpacjaToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.twardaSpacjaToolStripMenuItem.Text = "_ &Twarda spacja";
            this.twardaSpacjaToolStripMenuItem.Click += new System.EventHandler(this.twardaSpacjaToolStripMenuItem_Click);
            // 
            // plusMiniusToolStripMenuItem
            // 
            this.plusMiniusToolStripMenuItem.Name = "plusMiniusToolStripMenuItem";
            this.plusMiniusToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.plusMiniusToolStripMenuItem.Tag = "±";
            this.plusMiniusToolStripMenuItem.Text = "± &Plus Minius";
            this.plusMiniusToolStripMenuItem.Click += new System.EventHandler(this.µMikroToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(243, 6);
            // 
            // wytnijToolStripMenuItem
            // 
            this.wytnijToolStripMenuItem.Name = "wytnijToolStripMenuItem";
            this.wytnijToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.wytnijToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.wytnijToolStripMenuItem.Text = "&Wytnij";
            this.wytnijToolStripMenuItem.Click += new System.EventHandler(this.wytnijToolStripMenuItem_Click);
            // 
            // kopiujToolStripMenuItem
            // 
            this.kopiujToolStripMenuItem.Name = "kopiujToolStripMenuItem";
            this.kopiujToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.kopiujToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.kopiujToolStripMenuItem.Text = "&Kopiuj";
            this.kopiujToolStripMenuItem.Click += new System.EventHandler(this.kopiujToolStripMenuItem_Click);
            // 
            // wklejToolStripMenuItem
            // 
            this.wklejToolStripMenuItem.Name = "wklejToolStripMenuItem";
            this.wklejToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            this.wklejToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.wklejToolStripMenuItem.Text = "Wkl&ej";
            this.wklejToolStripMenuItem.Click += new System.EventHandler(this.wklejToolStripMenuItem_Click);
            // 
            // zaznaczWszystkoToolStripMenuItem
            // 
            this.zaznaczWszystkoToolStripMenuItem.Name = "zaznaczWszystkoToolStripMenuItem";
            this.zaznaczWszystkoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
            this.zaznaczWszystkoToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.zaznaczWszystkoToolStripMenuItem.Text = "&Zaznacz wszystko";
            this.zaznaczWszystkoToolStripMenuItem.Click += new System.EventHandler(this.zaznaczWszystkoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(243, 6);
            // 
            // cofnijToolStripMenuItem
            // 
            this.cofnijToolStripMenuItem.Name = "cofnijToolStripMenuItem";
            this.cofnijToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Z";
            this.cofnijToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.cofnijToolStripMenuItem.Text = "&Cofnij";
            this.cofnijToolStripMenuItem.Click += new System.EventHandler(this.cofnijToolStripMenuItem_Click);
            // 
            // powtórzToolStripMenuItem
            // 
            this.powtórzToolStripMenuItem.Name = "powtórzToolStripMenuItem";
            this.powtórzToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Y";
            this.powtórzToolStripMenuItem.Size = new System.Drawing.Size(246, 24);
            this.powtórzToolStripMenuItem.Text = "&Powtórz";
            this.powtórzToolStripMenuItem.Click += new System.EventHandler(this.powtórzToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(261, 6);
            // 
            // innyToolStripMenuItem
            // 
            this.innyToolStripMenuItem.Name = "innyToolStripMenuItem";
            this.innyToolStripMenuItem.Size = new System.Drawing.Size(264, 24);
            this.innyToolStripMenuItem.Text = "Inny";
            this.innyToolStripMenuItem.Click += new System.EventHandler(this.innyToolStripMenuItem_Click);
            // 
            // SimpleHtmlTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.rt);
            this.Name = "SimpleHtmlTextBox";
            this.Size = new System.Drawing.Size(505, 346);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rt;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pogrubienieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kurysywaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem podkreślenieToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem indeksgórnyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indeksdolnyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem znakispecyjalneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem µMikroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twardaSpacjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem wytnijToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kopiujToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wklejToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cofnijToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powtórzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zaznaczWszystkoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plusMiniusToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem innyToolStripMenuItem;
    }
}
