namespace DotBase.Szablony
{
    partial class DocxOverrideWindow
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
            this.komunikat = new System.Windows.Forms.Label();
            this.nadpisz = new System.Windows.Forms.Button();
            this.anuluj = new System.Windows.Forms.Button();
            this.nowaNazwa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // komunikat
            // 
            this.komunikat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.komunikat.Location = new System.Drawing.Point(12, 9);
            this.komunikat.Name = "komunikat";
            this.komunikat.Size = new System.Drawing.Size(524, 104);
            this.komunikat.TabIndex = 0;
            this.komunikat.Text = "Plik o nazwie \"{0}\" już istnieje.\r\n\r\nCo zrobić?";
            // 
            // nadpisz
            // 
            this.nadpisz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nadpisz.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.nadpisz.Location = new System.Drawing.Point(12, 116);
            this.nadpisz.Name = "nadpisz";
            this.nadpisz.Size = new System.Drawing.Size(258, 34);
            this.nadpisz.TabIndex = 1;
            this.nadpisz.Text = "Nadpisz";
            this.nadpisz.UseVisualStyleBackColor = true;
            // 
            // anuluj
            // 
            this.anuluj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.anuluj.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.anuluj.Location = new System.Drawing.Point(276, 116);
            this.anuluj.Name = "anuluj";
            this.anuluj.Size = new System.Drawing.Size(258, 34);
            this.anuluj.TabIndex = 2;
            this.anuluj.Text = "Anuluj";
            this.anuluj.UseVisualStyleBackColor = true;
            // 
            // nowaNazwa
            // 
            this.nowaNazwa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nowaNazwa.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.nowaNazwa.Location = new System.Drawing.Point(12, 156);
            this.nowaNazwa.Name = "nowaNazwa";
            this.nowaNazwa.Size = new System.Drawing.Size(522, 34);
            this.nowaNazwa.TabIndex = 3;
            this.nowaNazwa.Text = "Zapisz pod nazwą: {0}";
            this.nowaNazwa.UseVisualStyleBackColor = true;
            // 
            // DocxOverrideWindow
            // 
            this.AcceptButton = this.nadpisz;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.anuluj;
            this.ClientSize = new System.Drawing.Size(548, 202);
            this.Controls.Add(this.nowaNazwa);
            this.Controls.Add(this.anuluj);
            this.Controls.Add(this.nadpisz);
            this.Controls.Add(this.komunikat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocxOverrideWindow";
            this.Text = "Plik już istnieje";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label komunikat;
        private System.Windows.Forms.Button anuluj;
        private System.Windows.Forms.Button nowaNazwa;
        private System.Windows.Forms.Button nadpisz;
    }
}