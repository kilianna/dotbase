namespace DotBase.Logging
{
    partial class DecryptLogsForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.wybierzBtn = new System.Windows.Forms.Button();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.odszyfrujBtn = new System.Windows.Forms.Button();
            this.resultsBox = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.katalogText = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(60, 17);
            label1.TabIndex = 2;
            label1.Text = "Katalog:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 43);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(48, 17);
            label2.TabIndex = 3;
            label2.Text = "Hasło:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(102, 65);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(313, 17);
            label3.TabIndex = 6;
            label3.Text = "(pozostaw puste, aby użyć hasła z bazy danych)";
            // 
            // wybierzBtn
            // 
            this.wybierzBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.wybierzBtn.Location = new System.Drawing.Point(558, 12);
            this.wybierzBtn.Name = "wybierzBtn";
            this.wybierzBtn.Size = new System.Drawing.Size(53, 22);
            this.wybierzBtn.TabIndex = 0;
            this.wybierzBtn.Text = "···";
            this.wybierzBtn.UseVisualStyleBackColor = true;
            this.wybierzBtn.Click += new System.EventHandler(this.wybierzBtn_Click);
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(105, 40);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(506, 22);
            this.passwordBox.TabIndex = 5;
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // odszyfrujBtn
            // 
            this.odszyfrujBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.odszyfrujBtn.Location = new System.Drawing.Point(235, 109);
            this.odszyfrujBtn.Name = "odszyfrujBtn";
            this.odszyfrujBtn.Size = new System.Drawing.Size(152, 32);
            this.odszyfrujBtn.TabIndex = 7;
            this.odszyfrujBtn.Text = "Odszyfruj";
            this.odszyfrujBtn.UseVisualStyleBackColor = true;
            this.odszyfrujBtn.Click += new System.EventHandler(this.odszyfrujBtn_Click);
            // 
            // resultsBox
            // 
            this.resultsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsBox.Location = new System.Drawing.Point(12, 165);
            this.resultsBox.MaxLength = 3;
            this.resultsBox.Multiline = true;
            this.resultsBox.Name = "resultsBox";
            this.resultsBox.ReadOnly = true;
            this.resultsBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultsBox.Size = new System.Drawing.Size(599, 289);
            this.resultsBox.TabIndex = 8;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // katalogText
            // 
            this.katalogText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.katalogText.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DotBase.Properties.Settings.Default, "LastLogsDecryptDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.katalogText.Location = new System.Drawing.Point(105, 12);
            this.katalogText.Name = "katalogText";
            this.katalogText.Size = new System.Drawing.Size(447, 22);
            this.katalogText.TabIndex = 1;
            this.katalogText.Text = global::DotBase.Properties.Settings.Default.LastLogsDecryptDirectory;
            // 
            // DecryptLogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 466);
            this.Controls.Add(this.resultsBox);
            this.Controls.Add(this.odszyfrujBtn);
            this.Controls.Add(label3);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.katalogText);
            this.Controls.Add(this.wybierzBtn);
            this.MinimizeBox = false;
            this.Name = "DecryptLogsForm";
            this.ShowIcon = false;
            this.Text = "Odszyfruj logi diagnostyczne";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DecryptLogsForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button wybierzBtn;
        private System.Windows.Forms.TextBox katalogText;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button odszyfrujBtn;
        private System.Windows.Forms.TextBox resultsBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}