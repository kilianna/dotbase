namespace DotBase
{
    partial class AccessLockerForm
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
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
            System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessLockerForm));
            this.messageText = new System.Windows.Forms.TextBox();
            this.retryButton = new System.Windows.Forms.Button();
            this.otherButton = new System.Windows.Forms.Button();
            this.otherMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.otherMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(277, 24);
            toolStripMenuItem1.Text = "&Zamknij program";
            toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(277, 24);
            toolStripMenuItem2.Text = "Z&ignoruj błąd (niebezpieczne)";
            toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // messageText
            // 
            this.messageText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageText.BackColor = System.Drawing.SystemColors.Control;
            this.messageText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageText.Location = new System.Drawing.Point(12, 12);
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.ReadOnly = true;
            this.messageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageText.Size = new System.Drawing.Size(752, 370);
            this.messageText.TabIndex = 2;
            this.messageText.TabStop = false;
            this.messageText.Text = resources.GetString("messageText.Text");
            // 
            // retryButton
            // 
            this.retryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.retryButton.Location = new System.Drawing.Point(551, 398);
            this.retryButton.Name = "retryButton";
            this.retryButton.Size = new System.Drawing.Size(213, 41);
            this.retryButton.TabIndex = 0;
            this.retryButton.Text = "Spróbuj ponownie";
            this.retryButton.UseVisualStyleBackColor = true;
            this.retryButton.Click += new System.EventHandler(this.retryButton_Click);
            // 
            // otherButton
            // 
            this.otherButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.otherButton.ContextMenuStrip = this.otherMenu;
            this.otherButton.Location = new System.Drawing.Point(12, 398);
            this.otherButton.Name = "otherButton";
            this.otherButton.Size = new System.Drawing.Size(46, 41);
            this.otherButton.TabIndex = 1;
            this.otherButton.Text = "...";
            this.otherButton.UseVisualStyleBackColor = true;
            this.otherButton.Click += new System.EventHandler(this.otherButton_Click);
            // 
            // otherMenu
            // 
            this.otherMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripMenuItem1,
            toolStripMenuItem2});
            this.otherMenu.Name = "otherMenu";
            this.otherMenu.Size = new System.Drawing.Size(278, 52);
            // 
            // AccessLockerForm
            // 
            this.AcceptButton = this.retryButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 450);
            this.Controls.Add(this.otherButton);
            this.Controls.Add(this.retryButton);
            this.Controls.Add(this.messageText);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccessLockerForm";
            this.ShowIcon = false;
            this.Text = "Nie można uzyskać dostępu do bazy danych.";
            this.otherMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Button retryButton;
        private System.Windows.Forms.Button otherButton;
        private System.Windows.Forms.ContextMenuStrip otherMenu;
    }
}